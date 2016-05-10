using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.Xml;
using System.Web;

namespace Guesser
{
    public partial class Main : Form
    {
        //TCP Connection
        byte[] data = new byte[256];
        bool connected = false;
        TcpClient tcpclnt;
        Stream stm;
        string ip = "173.255.196.158";
        int port = 5005;
        List<string> sendQueue = new List<string>();

        //Other
        int id = -1;
        string name = null;
        List<Room> rooms = new List<Room>();
        Random rand = new Random();
        string localdir = AppDomain.CurrentDomain.BaseDirectory;
        List<string> mutedPlayers = new List<string>();
        List<string> bannedPlayers = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "banned.txt").ToList();

        //Recording
        bool recording = false;
        int recordTime = 0;
        List<int> recordTimes = new List<int>();
        List<string> recordShapes = new List<string>();
        List<string> playTimes = new List<string>();
        int playCount = 1;

        //Guessing the word
        List<string> words = new List<string>();
        List<string> guessedWords = new List<string>();
        bool idle = true;
        string winWord = null;
        string[] bannedWords = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "bannedwords.txt");
        Player drawer = null;
        Room currentRoom = null;

        //Grind
        bool grind = false;
        int grindIndex = 1;
        List<string> allWords = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "allwordseasy.txt").ToList();

        //Partial
        List<string> partials = new List<string>();
        List<string> partialWords = new List<string>();
        List<string> popPartials = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "popPartials.txt").ToList();
        List<string> words2guess = new List<string>();

        //GIF
        int gifIndex = 0;
        bool gif = false;
        string gifURL;
        int gifFrames = 0;

        public Main()
        {
            InitializeComponent();

            //Set Chat backcolor
            richChat.BackColor = Color.FromKnownColor(KnownColor.ControlLightLight);
        }

        //Change status text and color
        public void Status(string text)
        {
            labelStatus.Text = text;

            switch (text)
            {
                case "Disconnected":
                    labelStatus.ForeColor = Color.DarkRed;
                    break;
                case "Connected":
                    labelStatus.ForeColor = Color.Green;
                    break;
                case "Connecting":
                    labelStatus.ForeColor = Color.DarkGray;
                    break;
                default:
                    labelStatus.ForeColor = Color.Black;
                    break;
            }
        }

        private void butLogin_Click(object sender, EventArgs e)
        {
            if (!connected && id == -1)
            {
                Status("Connecting");
                try
                {
                    tcpclnt = new TcpClient();

                    tcpclnt.Connect(ip, port);
                    // use the ipaddress as in the server program
                }
                catch (Exception)
                {
                    //ERROR :C
                    Disconnect();
                    return;
                }
                timerChatUpdate.Enabled = true;
                connected = true;
                Status("Connected");
                name = textUser.Text;
                string login1 = null;
                if (name == "SketchoBOT")
                {
                    login1 = "LOGIN=FB 111939789219572";
                    //login1 = "LOGIN=FB 100001931520666";
                }
                else
                {
                    //login1 = "LOGIN=ANON " + name;
                    login1 = "LOGIN=ANON " + name;
                }

                string[] loginString = {
                login1,
                "PIC=http://lorempixel.com/600/400/cats/",
                "SQUAREPIC=http://lorempixel.com/600/400/cats/",
                "CLIENTTYPE=flash"
                };
                for (int i = 0; i < loginString.Length; i++)
                {
                    Send(loginString[i]);
                }
                if (comboRoom.Text != "")
                {
                    string[] joinRoom =
                    {
                        "JOINROOM=<joinroom><name>" + rooms[comboRoom.SelectedIndex].name + "</name><password>" + textPass.Text + "</password></joinroom>",
                        "MYROOM"
                    };
                    for (int i = 0; i < joinRoom.Length; i++)
                    {
                        Send(joinRoom[i]);
                    }
                }
                butRoomRefresh.PerformClick();
            }
            else
            {
                if (comboRoom.Text != "")
                {
                    string[] joinRoom =
                    {
                        "JOINROOM=<joinroom><name>" + rooms[comboRoom.SelectedIndex].name + "</name><password>" + textPass.Text + "</password></joinroom>",
                        "MYROOM"
                    };
                    for (int i = 0; i < joinRoom.Length; i++)
                    {
                        Send(joinRoom[i]);
                    }
                }
            }
        }

        //Update receive code
        private void timerChatUpdate_Tick(object sender, EventArgs e)
        {
            if (!backChat.IsBusy)
            {
                backChat.RunWorkerAsync();
            }

            //Partial
            if (partials.Count > 0)
            {
                //Make sure that 1 partial is not a popular one
                if (partials.Count == 1)
                {
                    if (!popPartials.Contains(partials[0]))
                    {
                        if (!backPartialGuess.IsBusy)
                        {
                            backPartialGuess.RunWorkerAsync();
                        }
                    }
                }
                else
                {
                    if (!backPartialGuess.IsBusy)
                    {
                        backPartialGuess.RunWorkerAsync();
                    }
                }
            }

            //Grind
            if (grind && !backGrind.IsBusy)
            {
                backGrind.RunWorkerAsync();
            }

            //Empty sendQueue list
            if (sendQueue.Count > 0)
            {
                for (int i = 0; i < sendQueue.Count; i++)
                {
                    Send(sendQueue[i]);
                    sendQueue.RemoveAt(i);
                }
            }
        }

        //Receieve code
        private void backChat_DoWork(object sender, DoWorkEventArgs e)
        {
            List<string> result = new List<string>();
            try
            {
                stm = tcpclnt.GetStream();
                byte[] bytes = new byte[tcpclnt.ReceiveBufferSize];

                // Read can return anything from 0 to numBytesToRead. 
                // This method blocks until at least one byte is read.
                stm.Read(bytes, 0, (int)tcpclnt.ReceiveBufferSize);

                // Returns the data received from the host to the console.
                string returndata = Encoding.UTF8.GetString(bytes);

                string[] lines = returndata.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                for (int i = 0; i < lines.Length - 1; i++)
                {
                    result.Add(lines[i]);
                }
            }
            catch (Exception)
            {
                result.Clear();
                result.Add("streamerror");
            }

            e.Result = result;
        }

        //Parse receieved data
        private void backChat_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (connected)
            {
                List<string> listResult = (List<string>)e.Result;
                if (listResult.Count < 1)
                {
                    listResult.Add("streamerror");
                }
                if (listResult.ElementAt(0) != "streamerror")
                {
                    string[] result = listResult.ToArray();

                    for (int i = 0; i < result.Length; i++)
                    {
                        string cmd = result[i].Split(' ')[0];
                        string message = result[i].Remove(0, cmd.Length);
                        switch (cmd.ToLower())
                        {
                            case "draw":
                                //System.Diagnostics.Process.Start("https://www.google.com/search?q=" + message + "&tbm=isch&tbs=itp:lineart");
                                UpdateChat("error", "Word is = " + message);
                                if (grind)
                                {
                                    /*
                                    if (!allWords.Contains(message.Remove(0, 1)))
                                    {
                                        allWords.Add(message.Remove(0, 1));
                                        string[] grindWord = { message.Remove(0, 1) };
                                        File.AppendAllLines(localdir + "allwords.txt", grindWord);
                                    }
                                    */
                                    SendKeys.SendWait(message.Remove(0, 1));
                                    SendKeys.SendWait("{Enter}");
                                }
                                break;
                            case "chat":
                                string chatName = message.Split(':')[0].Remove(0, 1);
                                //Update chat with new message if not from muted or banned
                                if (!mutedPlayers.Contains(chatName) && !bannedPlayers.Contains(chatName))
                                {
                                    UpdateChat(cmd, message);
                                }

                                //If "SketchoBOT say" are the first two words, it'll repeat the sentence
                                if (message.ToLower().Contains("sketchobot say") /*&& message.Split(':')[0].Remove(0, 1) != name*/)
                                {
                                    string[] checkSketcho = message.ToLower().Split(':');
                                    string checkSketchoTemp = message.Remove(0, checkSketcho.Length + 1);
                                    checkSketcho = checkSketchoTemp.Split(' ');
                                    UpdateChat("schat", checkSketcho[1] + "+" + checkSketcho[2]);
                                    for (int j = 0; j < checkSketcho.Length; j++)
                                    {
                                        checkSketcho[j].Replace(" ", "");
                                    }
                                    if (checkSketcho[1] == "sketchobot" && checkSketcho[2] == "say")
                                    {
                                        //Say the sentence
                                        try
                                        {
                                            string repeat = message.Split(':')[1].Remove(0, 16);

                                            if (repeat[0] == '/')
                                            {
                                                repeat.Remove(0, 1);
                                            }

                                            Send("CHAT=" + repeat);
                                        }
                                        catch (Exception)
                                        {
                                        }
                                    }
                                }

                                //If guessing
                                if (!idle && checkGuess.Checked)
                                {
                                    //If not self and player not muted or banned
                                    //UpdateChat("schat", message.Split(':')[0] + " + " + name);
                                    if (chatName != name && !mutedPlayers.Contains(chatName) && !bannedPlayers.Contains(chatName))
                                    {
                                        Guess(message);
                                    }
                                }
                                break;
                            case "schat":
                                UpdateChat(cmd, message);

                                //Check partial
                                if (checkPartial.Checked && !idle && checkGuess.Checked)
                                {
                                    if (message.Contains("guessed partial match with"))
                                    {
                                        string partial = message.Split(':')[1].Remove(0, 1);
                                        partial = partial.Remove(partial.Length - 1, 1);

                                        if (partial.Length > 3)
                                        {
                                            GetPartial(partial);
                                        }
                                        else
                                        {
                                            //if partial came from myself
                                            if (message.Split(' ')[1] == name)
                                            {
                                                if (backPartial.IsBusy)
                                                {
                                                    backPartial.CancelAsync();
                                                }
                                                
                                            }
                                            partials.Add(partial);
                                        }
                                    }
                                }
                                break;
                            case "error":
                                HandleError(message);
                                break;
                            case "id":
                                id = Convert.ToInt32(message);
                                break;
                            case "<roomplayerlist>":
                                rooms = UpdateRooms(result[i]);
                                comboRoom.Items.Clear();

                                string[] roomsInfo = new string[rooms.Count];
                                for (int j = 0; j < roomsInfo.Length; j++)
                                {
                                    roomsInfo[j] = rooms[j].name + " " + rooms[j].players.Count + "/" + rooms[j].maxPlayers;
                                }
                                comboRoom.Items.AddRange(roomsInfo);
                                break;
                            case "<playerlist>":
                                //Check if there is next drawer
                                if (currentRoom != null)
                                {
                                    currentRoom.players = UpdatePlayers(result[i]);
                                    foreach (var player in currentRoom.players)
                                    {
                                        if (player.queue == 0)
                                        {
                                            drawer = player;
                                            break;
                                        }
                                    }
                                }
                                break;
                            case "shape":
                                if (recording)
                                {
                                    Record(result[i]);
                                }
                                break;
                            case "idle":
                                idle = true;
                                //If any words were saved
                                if (words.Count > 0)
                                {
                                    //Check if it found a winning word and the drawer is not muted or banned
                                    try
                                    {
                                        if (winWord != null || !mutedPlayers.Contains(drawer.name) || !bannedPlayers.Contains(drawer.name))
                                        {
                                            GuessSave(words, winWord);
                                        }
                                        else
                                        {
                                            UpdateChat("error", "winWord is null or drawer is muted/banned!");
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        UpdateChat("error", "try error, maybe drawer is null");
                                    }
                                }
                                break;
                            case "gamestart":
                                idle = false;
                                partials.Clear();
                                partialWords.Clear();
                                break;
                            case "room":
                                idle = true;
                                if (checkGreet.Checked)
                                {
                                    Greet();
                                }
                                break;
                            case "roundwinner":
                                StopPartial();

                                winWord = message.Remove(0, message.Split(' ')[1].Length + 2);
                                if (message.Split(' ')[1] == name)
                                {
                                    MessageBox.Show("SKETCHOBOT WON!");
                                }
                                UpdateChat("schat", message.Split(' ')[1] + " wins");
                                break;
                            case "joined":
                                foreach (var room in rooms)
                                {
                                    if (room.name == message)
                                    {
                                        currentRoom = room;
                                    }
                                }
                                break;
                            default:
                                //MessageBox.Show(result.ElementAt(0) + " " + result.ElementAt(1));
                                break;
                        }
                    }
                }
                else
                {
                    //Disconnect();
                    UpdateChat("error", "Stream error");
                }
            }
        }

        public void Send(string text)
        {
            if (!backSend.IsBusy)
            {
                backSend.RunWorkerAsync(text);
            }
            else
            {
                sendQueue.Add(text);
            }
        }

        public void Ping()
        {
            if (connected)
            {
                Send("PING");
            }
        }

        public void Disconnect()
        {
            Status("Disconnected");
            id = -1;
            connected = false;
        }

        public string Guess(string message)
        {
            string word = message.Split(':')[1].Remove(0, 1);
            //Skip saving guess if word > 3 words
            if (word.Split(' ').Length <= 3)
            {
                //If not one of banned words
                if (!bannedWords.Contains(word.ToLower()))
                {
                    //Add to word list
                    if (!words.Contains(word))
                    {
                        //If words is longer than 3 letters and shorter than 12
                        if (word.Length > 2 && word.Length < 12)
                        {
                            words.Add(word);
                        }
                    }
                    //Check if word is in memory
                    string guess = GuessReturn(word);
                    if (guess != null && !guessedWords.Contains(guess))
                    {
                        bool doGuess = true;
                        if (checkPartial.Checked)
                        {
                            try
                            {
                                foreach (var partial in partials)
                                {
                                    if (!guess.Contains(partial))
                                    {
                                        doGuess = false;
                                    }
                                }
                            }
                            catch (Exception) { }
                        }
                        if (doGuess && !idle)
                        {
                            Send("CHAT=" + guess);
                            guessedWords.Add(guess);
                        }
                    }
                }
            }
            return null;
        }

        public void GuessSave(List<string> words, string win)
        {
            List<string> savedWords = new List<string>();
            List<string> sameWords = new List<string>();
            List<string> newWords = new List<string>();
            XmlDocument doc = new XmlDocument();
            string path = localdir + "words.xml";

            doc.Load(path);
            XmlNode root = doc.SelectSingleNode("words");
            var guesses = doc.GetElementsByTagName("guess");
            //Get all saved words
            foreach (XmlNode savedWord in guesses)
            {
                savedWords.Add(savedWord.Attributes[0].InnerText.ToLower());
            }

            //Find new words and put them in a new list
            foreach (var word in words)
            {
                if (!savedWords.Contains(word))
                {
                    newWords.Add(word);
                }
                else
                {
                    sameWords.Add(word);
                }
            }

            //Update existing words
            foreach (var savedWord in sameWords)
            {
                try
                {
                    XmlNode word = doc.SelectSingleNode("/words//guess[@word='" + savedWord + "']");
                    bool updated = false;
                    foreach (XmlNode wins in word.ChildNodes)
                    {
                        //If win is saved, just update x
                        if (!updated && wins.InnerText == win.ToLower())
                        {
                            XmlNode xNode = wins.SelectSingleNode("@x");
                            int x = Convert.ToInt32(xNode.InnerText);
                            x += 1;
                            xNode.Value = x.ToString();
                            updated = true;
                        }
                    }
                    //Create new win
                    if (!updated)
                    {
                        XmlElement newWin = doc.CreateElement("win");
                        newWin.InnerText = win;
                        newWin.SetAttribute("x", "1");
                        word.AppendChild(newWin);
                    }
                }
                catch (Exception)
                {
                }
            }

            //Write new words
            foreach (var newWord in newWords)
            {
                string goodWord = EvaluateWord(newWord);
                if (goodWord != null)
                {
                    XmlElement newNewWord = doc.CreateElement("guess");
                    newNewWord.SetAttribute("word", newWord.ToLower());

                    XmlElement newWin = doc.CreateElement("win");
                    newWin.InnerText = win;
                    newWin.SetAttribute("x", "1");

                    newNewWord.AppendChild(newWin);
                    root.AppendChild(newNewWord);
                }
            }

            doc.Save(path);
            winWord = null;
            savedWords = null;
            newWords = null;
            words.Clear();
        }

        public string GuessReturn(string word)
        {
            if (word.Length < 3)
            {
                return null;
            }

            XmlDocument doc = new XmlDocument();
            string path = localdir + "words.xml";

            doc.Load(path);
            XmlNode root = doc.SelectSingleNode("words");
            var guesses = doc.GetElementsByTagName("guess");
            //Get all saved words
            foreach (XmlNode savedWord in guesses)
            {
                if (savedWord.Attributes[0].InnerText == word)
                {
                    var wins = savedWord.ChildNodes;
                    int maxWin = 0;
                    string winWord = wins[0].InnerText;
                    foreach (XmlNode win in wins)
                    {
                        if (Convert.ToInt32(win.Attributes[0].InnerText) > maxWin && !guessedWords.Contains(win.InnerText))
                        {
                            maxWin = Convert.ToInt32(win.Attributes[0].InnerText);
                            winWord = win.InnerText;
                        }
                    }
                    return winWord;
                }
            }

            return null;
        }

        public string EvaluateWord(string word)
        {
            bool accept = true;
            for (int i = 0; i < word.Length; i++)
            {
                if (!char.IsLetter(word[i]))
                {
                    word.Replace(word[i].ToString(), "");
                }
            }
            string[] split = word.Split(' ');
            for (int i = 0; i < split.Length; i++)
            {
                split[i].Replace(" ", "");
                if (bannedWords.Contains(split[i].ToLower()))
                {
                    accept = false;
                }
            }

            if (accept)
            {
                return word;
            }
            else
            {
                return null;
            }
        }

        //Ban player (ignores chat and drawings permanently)
        public void BanPlayer(string name, bool ban)
        {
            if (ban)
            {
                bannedPlayers.Add(name);
            }
            else
            {
                bannedPlayers.Remove(name);
            }
            File.WriteAllLines(localdir + "banned.txt", bannedPlayers);
        }

        public void HandleCommand(string command)
        {
            string cmd = command.Split(' ')[0];

            switch (cmd.ToLower())
            {
                case "mute":
                    mutedPlayers.Add(command.Split(' ')[1]);
                    UpdateChat("schat", "Muted " + command.Split(' ')[1]);
                    break;
                case "unmute":
                    mutedPlayers.Remove(command.Split(' ')[1]);
                    UpdateChat("schat", "Unmuted " + command.Split(' ')[1]);
                    break;
                case "ban":
                    BanPlayer(command.Split(' ')[1], true);
                    UpdateChat("schat", "Banned " + command.Split(' ')[1]);
                    break;
                case "unban":
                    BanPlayer(command.Split(' ')[1], false);
                    UpdateChat("schat", "Unbanned " + command.Split(' ')[1]);
                    break;
                case "pic":
                    Send("PIC=http://cdn.osxdaily.com/wp-content/uploads/2013/07/dancing-banana.gif");
                    Send("SQUAREPIC=http://cdn.osxdaily.com/wp-content/uploads/2013/07/dancing-banana.gif");
                    break;
                default:
                    break;
            }
        }

        public List<Room> UpdateRooms(string xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            var rooms = doc.GetElementsByTagName("room");
            List<Room> listRooms = new List<Room>();

            for (int i = 0; i < rooms.Count; i++)
            {
                Room room = new Room();
                List<Player> players = new List<Player>();

                room.name = rooms[i].ChildNodes[0].InnerText;
                room.maxPlayers = Convert.ToInt32(rooms[i].ChildNodes[1].InnerText);

                for (int j = 0; j < rooms[i].ChildNodes.Count; j++)
                {
                    if (rooms[i].ChildNodes[j].Name == "player")
                    {
                        Player player = new Player();
                        player.name = rooms[i].ChildNodes[j].ChildNodes[0].InnerText;
                        player.score = Convert.ToInt32(rooms[i].ChildNodes[j].ChildNodes[1].InnerText);
                        try
                        {
                            player.queue = Convert.ToInt32(rooms[i].ChildNodes[j].ChildNodes[2].InnerText);
                        }
                        catch (Exception)
                        {
                            player.queue = -1;
                        }
                        players.Add(player);
                    }
                }

                room.players = players;

                listRooms.Add(room);
            }
            return listRooms;
        }

        public List<Player> UpdatePlayers(string xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            List<Player> players = new List<Player>();
            var playerNodes = doc.GetElementsByTagName("player");

            for (int i = 0; i < playerNodes.Count; i++)
            {
                Player player = new Player();
                player.name = playerNodes[i].ChildNodes[0].InnerText;
                player.score = Convert.ToInt32(playerNodes[i].ChildNodes[1].InnerText);
                try
                {
                    player.queue = Convert.ToInt32(playerNodes[i].ChildNodes[2].InnerText);
                }
                catch (Exception)
                {
                    player.queue = -1;
                }
                players.Add(player);
            }

            return players;
        }

        public void Greet()
        {
            string[] greetings =
            {
                "Hello world!",
                "I'm back!",
                "[Place greeting text here]",
                "Hello fellow humans!",
                "Ding dong!",
                "It's time to beat humans and drink oil... and I'm all out of oil.",
                "0100100001101001",
                "(͡° ͜ʖ ͡°) sketchobot is here (͡° ͜ʖ ͡°)",
                "Mess with the best, lose like the rest."
            };
            Send("CHAT=" + greetings[rand.Next(0, greetings.Length)]);
        }

        public void UpdateChat(string cmd, string message)
        {
            RichTextBox box = richChat;
            string br = Environment.NewLine;
            Color stringColor = Color.Black;
            switch (cmd.ToLower())
            {
                case "chat":
                    stringColor = Color.LimeGreen;
                    break;
                case "schat":
                    stringColor = Color.DarkRed;
                    break;
                case "error":
                    stringColor = Color.Red;
                    break;
                default:
                    stringColor = Color.Black;
                    break;
            }
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;

            box.SelectionColor = stringColor;
            box.AppendText(message + br);
            box.SelectionColor = box.ForeColor;
        }

        public void HandleError(string message)
        {
            string code = message.Split(' ')[1];
            switch (code)
            {
                case "102":
                    Disconnect();
                    UpdateChat("error", message.Remove(0, code.Length+1));
                    break;
                case "106":
                    Disconnect();
                    UpdateChat("error", message.Remove(0, code.Length+1));
                    break;
                case "112":
                    Disconnect();
                    UpdateChat("error", message.Remove(0, code.Length+1));
                    break;
                default:
                    UpdateChat("error", "ERROR!" + message);
                    break;
            }
        }

        private void timerPing_Tick(object sender, EventArgs e)
        {
            Ping();
        }

        //Press Enter in chat
        private void textChat_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (connected)
                {
                    //Check if command or not
                    if (textChat.Text.Length > 1)
                    {
                        if (textChat.Text[0] == '!')
                        {
                            //is writing command
                            string command = textChat.Text.Remove(0, 1);
                            HandleCommand(command);
                            textChat.Text = "";
                            return;
                        }
                    }
                    Send("CHAT=" + textChat.Text);
                }
                textChat.Text = "";
            }
        }

        public void GetPartial(string partial)
        {
            if (!backPartial.IsBusy)
            {
                backPartial.RunWorkerAsync(partial);
            }
        }
        /////////////////////
        /* DRAWING METHODS */
        /////////////////////

        public static Bitmap ResizeImage(Bitmap imgToResize, Size size)
        {
            Console.WriteLine(Convert.ToString(size));
            Bitmap b = new Bitmap(size.Width, size.Height);
            using (Graphics g = Graphics.FromImage((Image)b))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(imgToResize, 0, 0, size.Width, size.Height);
            }
            return b;
        }

        public Bitmap ImageProcess(string url)
        {
            int maxWidth = 600;
            int maxHeight = 400;
            try
            {
                System.Net.WebRequest request = System.Net.WebRequest.Create(url);
                System.Net.WebResponse response = request.GetResponse();
                System.IO.Stream responseStream = response.GetResponseStream();
                Bitmap img = new Bitmap(responseStream);

                if (img.Width > maxWidth || img.Height > maxHeight)
                {
                    if (img.Width > img.Height)
                    {
                        decimal ratio = Convert.ToDecimal(maxWidth) / Convert.ToDecimal(img.Width);
                        img = ResizeImage(img, new Size(maxWidth, Convert.ToInt32(maxHeight * ratio)));
                    }
                    else
                    {
                        decimal ratio = Convert.ToDecimal(maxHeight) / Convert.ToDecimal(img.Height);
                        img = ResizeImage(img, new Size(Convert.ToInt32(maxWidth * ratio), maxHeight));
                    }
                }

                return img;
            }
            catch (Exception e)
            {
                UpdateChat("error", "Image error = " + e.Message);
                return null;
            }
        }

        public static Bitmap ConvertTo1Bit(Bitmap input)
        {
            var masks = new byte[] { 0x80, 0x40, 0x20, 0x10, 0x08, 0x04, 0x02, 0x01 };
            var output = new Bitmap(input.Width, input.Height, PixelFormat.Format1bppIndexed);
            var data = new sbyte[input.Width, input.Height];
            var inputData = input.LockBits(new Rectangle(0, 0, input.Width, input.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppPArgb);
            try
            {
                var scanLine = inputData.Scan0;
                var line = new byte[inputData.Stride];
                for (var y = 0; y < inputData.Height; y++, scanLine += inputData.Stride)
                {
                    Marshal.Copy(scanLine, line, 0, line.Length);
                    for (var x = 0; x < input.Width; x++)
                    {
                        if (line[x * 4 + 3] > 50)
                        {
                            data[x, y] = (sbyte)(64 * (GetGreyLevel(line[x * 4 + 2], line[x * 4 + 1], line[x * 4 + 0]) - 0.5));
                        }
                        else
                        {
                            data[x, y] = (sbyte)(64 * (1 - 0.5));
                        }
                    }
                }
            }
            finally
            {
                input.UnlockBits(inputData);
            }
            var outputData = output.LockBits(new Rectangle(0, 0, output.Width, output.Height), ImageLockMode.WriteOnly, PixelFormat.Format1bppIndexed);
            try
            {
                var scanLine = outputData.Scan0;
                for (var y = 0; y < outputData.Height; y++, scanLine += outputData.Stride)
                {
                    var line = new byte[outputData.Stride];
                    for (var x = 0; x < input.Width; x++)
                    {
                        var j = data[x, y] > 0;
                        if (j) line[x / 8] |= masks[x % 8];
                        var error = (sbyte)(data[x, y] - (j ? 32 : -32));
                        if (x < input.Width - 1) data[x + 1, y] += (sbyte)(7 * error / 16);
                        if (y < input.Height - 1)
                        {
                            if (x > 0) data[x - 1, y + 1] += (sbyte)(3 * error / 16);
                            data[x, y + 1] += (sbyte)(5 * error / 16);
                            if (x < input.Width - 1) data[x + 1, y + 1] += (sbyte)(1 * error / 16);
                        }
                    }
                    Marshal.Copy(line, 0, scanLine, outputData.Stride);
                }
            }
            finally
            {
                output.UnlockBits(outputData);
            }
            return output;
        }

        public static double GetGreyLevel(byte r, byte g, byte b)
        {
            return (r * 0.299 + g * 0.587 + b * 0.114) / 255;
        }

        public static Bitmap ConvertTo8bpp(Bitmap img)
        {
            var ms = new MemoryStream();   // Don't use using!!!
            img.Save(ms, ImageFormat.Gif);
            ms.Position = 0;
            return new Bitmap(ms);
        }

        void ScribbleImage(Bitmap img, bool color)
        {
            int xZero = 300 - (img.Width / 2);
            int yZero = 200 - (img.Height / 2);
            int x1, y1;
            bool[,] pixelDrawn = new bool[img.Width, img.Height];

            for (int i = 0; i < img.Height; i++)
            {
                for (int j = 0; j < img.Width; j++)
                {
                    if (!pixelDrawn[j, i])
                    {
                        Color pixelColor = img.GetPixel(j, i);
                        if (color)
                        {
                            //Color to decimal
                            string hexColor = ColorTranslator.ToHtml(Color.FromArgb(255, pixelColor.R, pixelColor.G, pixelColor.B));
                            int iColor = int.Parse(hexColor.Remove(0, 1), System.Globalization.NumberStyles.HexNumber);

                            pixelDrawn[j, i] = true;

                            x1 = xZero + j;
                            y1 = yZero + i;
                            int length = 1;
                            bool loop = true;
                            try
                            {
                                while (img.GetPixel(j + length, i + length) == pixelColor && loop)
                                {
                                    pixelDrawn[j + length, i + length] = true;
                                    if (j + length >= img.Width || i + length >= img.Height)
                                    {
                                        Console.WriteLine("BREAK!");
                                        loop = false;
                                        break;
                                    }
                                    else
                                    {
                                        length++;
                                    }
                                }
                            }
                            catch (Exception)
                            {
                                loop = false;
                            }

                            length -= 1;

                            Send("SHAPE=LINE%3A" + iColor.ToString() + "%3A1%3A1%3A1%3A0%3A0%3A0%3A" + (xZero + j) + "%3A" + (yZero + i) + "%3A" + length + "%3A" + length);
                            Send("SHAPE=STOREUNDO");
                            Thread.Sleep(6);
                        }
                        else
                        {
                            if (pixelColor.GetBrightness() == 0)
                            {
                                pixelDrawn[j, i] = true;

                                x1 = xZero + j;
                                y1 = yZero + i;
                                int length = 1;
                                bool loop = true;
                                try
                                {
                                    while (img.GetPixel(j + length, i + length).GetBrightness() == 0 && loop)
                                    {
                                        pixelDrawn[j + length, i + length] = true;
                                        if (j + length - 2 >= img.Width || i + length - 2 >= img.Height)
                                        {
                                            Console.WriteLine("BREAK!");
                                            loop = false;
                                            break;
                                        }
                                        else
                                        {
                                            length++;
                                        }
                                    }
                                }
                                catch (Exception)
                                {
                                    loop = false;
                                }

                                length -= 1;

                                Send("SHAPE=LINE%3A0%3A1%3A1%3A0%3A0%3A0%3A0%3A" + (xZero + j) + "%3A" + (yZero + i) + "%3A" + length + "%3A" + length);
                                Send("SHAPE=STOREUNDO");
                                Thread.Sleep(3);
                            }
                        }
                    }
                }
            }
        }

        public static string[] RandomizeStrings(string[] arr)
        {
            Random rand = new Random();
            List<KeyValuePair<int, string>> list = new List<KeyValuePair<int, string>>();
            // Add all strings from array
            // Add new random int each time
            foreach (string s in arr)
            {
                list.Add(new KeyValuePair<int, string>(rand.Next(), s));
            }
            // Sort the list by the random number
            var sorted = from item in list
                         orderby item.Key
                         select item;
            // Allocate new string array
            string[] result = new string[arr.Length];
            // Copy values to array
            int index = 0;
            foreach (KeyValuePair<int, string> pair in sorted)
            {
                result[index] = pair.Value;
                index++;
            }
            // Return copied array
            return result;
        }

        public void SketchImageRandom(Bitmap img)
        {
            int count = 0;
            int xZero = 300 - (img.Width / 2);
            int yZero = 200 - (img.Height / 2);
            int x1, y1;

            //Create array with every pixel shuffled
            string[] shuffledPixels = new string[img.Width * img.Height];
            for (int i = 0; i < img.Width; i++)
            {
                for (int j = 0; j < img.Height; j++)
                {
                    shuffledPixels[count] = Convert.ToString(i + "." + j);
                    count++;
                }
            }
            //Shuffle(new Random(), shuffledPixels);
            shuffledPixels = RandomizeStrings(shuffledPixels);

            count = 0;
            for (int i = 0; i < img.Height; i++)
            {
                for (int j = 0; j < img.Width; j++)
                {
                    x1 = xZero + j;
                    y1 = yZero + i;
                    int x = Convert.ToInt32(shuffledPixels[count].Split('.')[0]);
                    int y = Convert.ToInt32(shuffledPixels[count].Split('.')[1]);
                    Color pixelColor = img.GetPixel(x, y);

                    //Color to decimal
                    string hexColor = ColorTranslator.ToHtml(Color.FromArgb(255, pixelColor.R, pixelColor.G, pixelColor.B));
                    int iColor = int.Parse(hexColor.Remove(0, 1), System.Globalization.NumberStyles.HexNumber);

                    if (pixelColor.A > 0)
                    {
                        if (i % 2 == 0 || j % 2 == 0 || true)
                        {
                            Send("SHAPE=PEN%3A" + iColor + "%3A1%3A2%3A0%3A0%3A0%3A0%3A0%3A" + x1 + "%3A" + y1);
                            System.Threading.Thread.Sleep(5);
                        }
                    }
                    count++;
                }
            }
        }

        private void butDraw_Click(object sender, EventArgs e)
        {
            Bitmap img = ImageProcess(textURL.Text);
            if (img != null)
            {
                Bitmap bitImg = ConvertTo1Bit(img);
                //Bitmap bitImg = ConvertTo8bpp(img);
                ScribbleImage(img, false);
                //SketchImageRandom(img);
            }
        }

        private void butQueue_Click(object sender, EventArgs e)
        {
            Send("WANNADRAW");
        }

        //Press Enter to Login
        private void textUser_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                butLogin.PerformClick();
            }
        }

        //Refresh rooms
        private void butRoomRefresh_Click(object sender, EventArgs e)
        {
            Send("ROOMPLAYERLIST");
        }

        //Send chat scroll to bottom
        private void richChat_TextChanged(object sender, EventArgs e)
        {
            richChat.SelectionStart = richChat.Text.Length;
            richChat.ScrollToCaret();
        }

        private void menuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        //Start recording
        public void StartRecording()
        {
            UpdateChat("schat", "Recording start");
            recordTimes.Clear();
            recordShapes.Clear();
            recordTime = 0;
            recording = true;
            timerRecord.Enabled = true;
        }

        //Record
        public void Record(string shape)
        {
            recordTimes.Add(recordTime);
            recordShapes.Add(shape);
        }

        //Stop recording
        public void StopRecording()
        {
            UpdateChat("schat", "Recording end");
            timerRecord.Enabled = false;
            saveDialogRecord.InitialDirectory = localdir + @"recordings\";
            saveDialogRecord.DefaultExt = "REC files |*.rec|";
            saveDialogRecord.ShowDialog();
            if (saveDialogRecord.FileName != "")
            {
                using (StreamWriter file = new StreamWriter(saveDialogRecord.FileName, true))
                {
                    //First line says amount of shapes saved
                    file.WriteLine(recordTimes.Count);

                    //Write all times
                    foreach (var time in recordTimes)
                    {
                        file.WriteLine(time);
                    }

                    //Write all shapes
                    foreach (var shape in recordShapes)
                    {
                        file.WriteLine(shape);
                    }
                }
            }
        }

        //Recording Tick
        private void timerRecord_Tick(object sender, EventArgs e)
        {
            //If playing, then play!
            if (!recording)
            {
                int count = Convert.ToInt32(playTimes[0]);
                if (playCount >= count)
                {
                    StopPlayRecording();
                }
                else
                {
                    while (recordTime == Convert.ToInt32(playTimes[playCount - 1]))
                    {
                        playCount++;
                        Send("SHAPE=" + HttpUtility.UrlEncode(playTimes[count + playCount - 1].Split(' ')[1]).ToUpper());
                    }
                }
                
            }
            recordTime++;
        }

        //Play recording
        public void StartPlayRecording(string path)
        {
            UpdateChat("schat", "Playing started");
            recordTime = 0;
            playCount = 1;
            playTimes.Clear();
            playTimes.AddRange(File.ReadAllLines(path));
            timerRecord.Enabled = true;
        }

        public void StopPlayRecording()
        {
            UpdateChat("schat", "Playing stopped");
            timerRecord.Enabled = false;
            playCount = 1;
            recordTime = 0;
            playTimes.Clear();
        }

        private void startRecordingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartRecording();
            startRecordingToolStripMenuItem.Enabled = false;
            stopRecordingToolStripMenuItem.Enabled = true;
        }

        private void stopRecordingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StopRecording();
            stopRecordingToolStripMenuItem.Enabled = false;
            startRecordingToolStripMenuItem.Enabled = true;
        }

        private void playRecordingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openDialogRecord.DefaultExt = "REC files |*.rec|";
            openDialogRecord.InitialDirectory = localdir + @"recordings\";
            openDialogRecord.ShowDialog();
            StartPlayRecording(openDialogRecord.FileName);
        }

        //Show players in chat of selected room
        private void comboRoom_SelectedIndexChanged(object sender, EventArgs e)
        {
            richChat.Clear();
            foreach (var player in rooms[comboRoom.SelectedIndex].players)
            {
                UpdateChat("schat", player.score + " | " + player.name);
            }
        }

        //
        //GRIND
        //
        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            grind = true;
            if (!backGrind.IsBusy)
            {
                backGrind.RunWorkerAsync();
            }
        }

        private void backGrind_DoWork(object sender, DoWorkEventArgs e)
        {
            /*grindIndex++;
            if (grindIndex > 2)
            {
                grindIndex = 1;
            }
            string[] joinRoom =
                    {
                        "JOINROOM=<joinroom><name>testo" + grindIndex + "</name><password>kami99</password></joinroom>",
                        "MYROOM"
                    };
            for (int i = 0; i < joinRoom.Length; i++)
            {
                Send(joinRoom[i]);
            }*/
            Thread.Sleep(100);
            Send("WANNADRAW");
        }

        private void stopGrindToolStripMenuItem_Click(object sender, EventArgs e)
        {
            grind = false;
        }

        //PARTIAL

        private void backPartial_DoWork(object sender, DoWorkEventArgs e)
        {
            string partial = (string)e.Argument;
            for (int i = 0; i < partial.Length - 2; i++)
            {
                if (!backPartial.CancellationPending)
                {
                    if (!partial.Substring(i, 3).Contains(' '))
                    {
                        if (!guessedWords.Contains(partial.Substring(i, 3)))
                        {
                            Send("CHAT=" + partial.Substring(i, 3));
                            Thread.Sleep(400);
                            //UpdateChat("error", "partial = " + partial.Substring(i, 3));
                        }
                    }
                }
                else
                {
                    e.Cancel = true;
                    return;
                }
            }
        }

        private void backPartialGuess_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                foreach (var word in allWords)
                {
                    bool good = true;
                    foreach (var partial in partials)
                    {
                        if (!word.Contains(partial.ToLower()))
                        {
                            good = false;
                        }
                    }
                    if (good)
                    {
                        if (!backPartialGuess.CancellationPending)
                        {
                            if (!partialWords.Contains(word))
                            {
                                partialWords.Add(word);
                                if (words2guess.Count > 2)
                                {
                                    Send("CHAT=" + words2guess[0] + words2guess[1] + words2guess[2]);
                                    words2guess.Clear();
                                }
                                else
                                {
                                    words2guess.Add(word);
                                }
                                Thread.Sleep(600);
                                //UpdateChat("error", "PARTIAL GUESS = " + word);
                            }
                        }
                        else
                        {
                            e.Cancel = true;
                            return;
                        }
                    }
                }
                if (words2guess.Count > 0)
                {
                    foreach (var word in words2guess)
                    {
                        Send("CHAT=" + word);
                        words2guess.Remove(word);
                        Thread.Sleep(200);
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        public void StopPartial()
        {
            if (backPartial.IsBusy)
            {
                backPartial.CancelAsync();
            }
            if (backPartialGuess.IsBusy)
            {
                backPartialGuess.CancelAsync();
            }
        }

        private void butGIF_Click(object sender, EventArgs e)
        {
            gif = true;

            gifURL = "http://im2.ezgif.com/tmp/03772be308-gif-gifsicle/frame_0_delay-0.02s.gif";
            gifFrames = 25;
        }

        private void timerGIF_Tick(object sender, EventArgs e)
        {
            //GIF
            if (gif)
            {
                Send("SQUAREPIC=" + gifURL.Replace("frame_0", "frame_" + gifIndex));
                gifIndex++;
                if (gifIndex == gifFrames)
                {
                    gifIndex = 0;
                    Send("PING");
                }
            }
        }

        private void backSend_DoWork(object sender, DoWorkEventArgs e)
        {
            string text = (string)e.Argument;
            try
            {
                stm = tcpclnt.GetStream();
                ASCIIEncoding asen = new ASCIIEncoding();
                byte[] ba = asen.GetBytes((Uri.EscapeDataString(text)).Replace("%20", " ").Replace("%3D", "=") + Environment.NewLine);

                stm.Write(ba, 0, ba.Length);
            }
            catch (Exception)
            {
                UpdateChat("error", "stream error");
            }
        }
    }

    public class Room
    {
        public string name { get; set; }
        public List<Player> players { get; set; }
        public int maxPlayers { get; set; }
    }
    
    public class Player
    {
        public string name { get; set; }
        public int score { get; set; }
        public int queue { get; set; }
    }
}
