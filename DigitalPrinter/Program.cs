using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing.Imaging;
using OpenQA.Selenium;
using Guesser;

namespace DigitalPrinter
{
    static class Program
    {
        
        static Random rand = new Random();

        public static string[] RandomizeStrings(string[] arr)
        {
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

        //This is a replacement for Cursor.Position in WinForms
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        public const int MOUSEEVENTF_LEFTDOWN = 0x02;
        public const int MOUSEEVENTF_LEFTUP = 0x04;

        //This simulates a left mouse click
        public static void LeftMouseClick(int xpos, int ypos)
        {
            SetCursorPos(xpos, ypos);
            mouse_event(MOUSEEVENTF_LEFTDOWN, xpos, ypos, 0, 0);
            System.Threading.Thread.Sleep(4);
            mouse_event(MOUSEEVENTF_LEFTUP, xpos, ypos, 0, 0);
        }

        public static void LeftMouseDown(int xpos, int ypos)
        {
            SetCursorPos(xpos, ypos);
            mouse_event(MOUSEEVENTF_LEFTDOWN, xpos, ypos, 0, 0);
        }

        public static void LeftMouseUp(int xpos, int ypos)
        {
            SetCursorPos(xpos, ypos);
            mouse_event(MOUSEEVENTF_LEFTUP, xpos, ypos, 0, 0);
        }

        static void Main(string[] args)
        {

            string url = null;
            string method = null;
            while (url != "quit")
            {
                Console.Write("URL: ");
                url = Console.ReadLine();

                if (url.ToLower() != "quit")
                {
                    if (url.Contains(" "))
                    {
                        method = url.Split(' ')[1];
                        url = url.Split(' ')[0];
                    }
                    Bitmap img = ImageProcess(url);
                    if (img.Width > 400 || img.Height > 400)
                    { }
                    else
                    {
                        Bitmap bitImg = ConvertTo1Bit(img);
                        switch (method)
                        {
                            case "down":
                                SketchImage(bitImg);
                                break;
                            case "rand":
                                SketchImageRandom(bitImg);
                                break;
                            case "curt":
                                SketchImageCurtain(bitImg);
                                break;
                            case "up":
                                SketchImageBackwards(bitImg);
                                break;
                            case "spiral":
                                SketchImageSpiral(bitImg);
                                break;
                            case "draw":
                                ScribbleImage(bitImg);
                                break;
                            default:
                                ScribbleImage(bitImg);
                                break;
                        }
                    }
                }
            }
        }

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

        static Bitmap ImageProcess(string url)
        {
            int maxSize = 300;
            System.Net.WebRequest request = System.Net.WebRequest.Create(url);
            System.Net.WebResponse response = request.GetResponse();
            System.IO.Stream responseStream = response.GetResponseStream();

            Bitmap img = new Bitmap(responseStream);

            if (img.Width > maxSize || img.Width > maxSize)
            {
                if (img.Width > img.Height)
                {
                    decimal ratio = Convert.ToDecimal(img.Height) / Convert.ToDecimal(img.Width);
                    img = ResizeImage(img, new Size(maxSize, Convert.ToInt32(maxSize * ratio)));
                }
                else
                {
                    decimal ratio = Convert.ToDecimal(img.Width) / Convert.ToDecimal(img.Height);
                    img = ResizeImage(img, new Size(Convert.ToInt32(maxSize * ratio), maxSize));
                }
            }

            return img;
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


        static void SketchImage(Bitmap img)
        {
            int mouseX = Cursor.Position.X;
            int mouseY = Cursor.Position.Y;

            for (int i = 0; i < img.Height; i++)
            {
                for (int j = 0; j < img.Width; j++)
                {
                    Color pixel = img.GetPixel(j, i);
                    if (pixel.GetBrightness() == 0)
                    {
                        if (i % 2 == 0)
                        {
                            LeftMouseClick(mouseX + j, mouseY + i);
                            System.Threading.Thread.Sleep(25);
                        }
                    }
                }
            }
        }

        static void SketchImageBackwards(Bitmap img)
        {
            int mouseX = Cursor.Position.X;
            int mouseY = Cursor.Position.Y;

            for (int i = img.Height - 1; i >= 0; i--)
            {
                for (int j = 0; j < img.Width; j++)
                {
                    Color pixel = img.GetPixel(j, i);
                    if (pixel.GetBrightness() == 0)
                    {
                        if (i % 2 == 0)
                        {
                            LeftMouseClick(mouseX + j, mouseY + i);
                            System.Threading.Thread.Sleep(25);
                        }
                    }
                }
            }
        }

        static void SketchImageCurtain(Bitmap img)
        {
            int mouseX = Cursor.Position.X;
            int mouseY = Cursor.Position.Y;

            int x = 0;
            for (int i = 0; i < img.Height; i++)
            {
                if (i % 2 == 0)
                {
                    x = i;
                }
                else
                {
                    x = img.Height - i;
                }
                for (int j = 0; j < img.Width; j++)
                {
                    Color pixel = img.GetPixel(j, x);
                    if (pixel.GetBrightness() == 0)
                    {
                        if (i % 2 == 0 || j % 2 == 0 || true)
                        {
                            LeftMouseClick(mouseX + j, mouseY + x);
                            System.Threading.Thread.Sleep(20);
                        }
                    }
                }
            }
        }

        static void SketchImageSpiral(Bitmap img)
        {
            int mouseX = Cursor.Position.X;
            int mouseY = Cursor.Position.Y;
            int radius = 0;
            int angle = 0;
            int xCenter = img.Width / 2;
            int yCenter = img.Height / 2;

            for (int i = 0; i < img.Width * img.Height; i++)
            {
                int x = Convert.ToInt32(Math.Floor(Convert.ToDouble(xCenter) + Convert.ToDouble(radius) * Math.Cos(Math.PI * angle / 180)));
                //Console.WriteLine("x= " + Convert.ToInt32(Math.Floor(Convert.ToDouble(xCenter) + Convert.ToDouble(radius) * Math.Cos(Math.PI * angle / 180))) + " + " + radius + " + " + angle + "\ny= " + yCenter + radius * Convert.ToInt32(Math.Sin(Math.PI * angle / 180)));
                int y = Convert.ToInt32(Math.Floor(Convert.ToDouble(yCenter) + Convert.ToDouble(radius) * Math.Sin(Math.PI * angle / 180)));

                if (Enumerable.Range(0, img.Width).Contains(x) && Enumerable.Range(0, img.Height).Contains(y))
                {
                    Color pixel = img.GetPixel(x, y);
                    if (pixel.GetBrightness() == 0)
                    {
                        if (i % 2 == 0)
                        {
                            LeftMouseClick(mouseX + x, mouseY + y);
                            System.Threading.Thread.Sleep(10);
                        }
                    }
                    if (i % (img.Width + img.Height) / 2 == 0)
                    {
                        radius++;
                    }
                }
                if (angle >= 360)
                {
                    angle = 0;
                }
                angle++;
            }
        }

        static void SketchImageRandom(Bitmap img)
        {
            int mouseX = Cursor.Position.X;
            int mouseY = Cursor.Position.Y;
            int count = 0;

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
                    int x = Convert.ToInt32(shuffledPixels[count].Split('.')[0]);
                    int y = Convert.ToInt32(shuffledPixels[count].Split('.')[1]);
                    Color pixel = img.GetPixel(x, y);
                    if (pixel.GetBrightness() == 0)
                    {
                        if (i % 2 == 0 || j % 2 == 0)
                        {
                            LeftMouseClick(mouseX + x, mouseY + y);
                            System.Threading.Thread.Sleep(10);
                        }
                    }
                    count++;
                }
            }
        }

        static int[] SearchPixel(Bitmap img, int x, int y, int radius)
        {
            int minDist = 100;
            string minPixel = null;
            for (int i = 0; i < radius; i++)
            {
                for (int j = 0; j < radius; j++)
                {
                    Color pixel = img.GetPixel(x-radius+j, y-radius+j);
                    if (pixel.GetBrightness() == 0)
                    {
                        int distance = Convert.ToInt32(Math.Floor(Convert.ToDouble(radius - j) + Convert.ToDouble(radius - i) / 2));
                        if (distance < minDist)
                        {
                            minDist = distance;
                            minPixel = (0 - radius + j) + "." + (0 - radius - i);
                        }
                    }
                }
            }
            int[] coord = { Convert.ToInt32(minPixel.Split('.')[0]), Convert.ToInt32(minPixel.Split('.')[1]) };
            return coord;
        }

        static void ScribbleImage(Bitmap img)
        {
            int mouseX = Cursor.Position.X;
            int mouseY = Cursor.Position.Y;
            bool[,] pixelDrawn = new bool[img.Width, img.Height];

            for (int i = 0; i < img.Height; i++)
            {
                for (int j = 0; j < img.Width; j++)
                {
                    if (!pixelDrawn[j, i])
                    {
                        Color pixelColor = img.GetPixel(j, i);
                        if (pixelColor.GetBrightness() == 0)
                        {
                            pixelDrawn[j, i] = true;
                            LeftMouseDown(mouseX + j, mouseY + i);
                            int offset = 1;
                            bool loop = true;
                            try
                            {
                                while (img.GetPixel(j + offset, i + offset).GetBrightness() == 0 && loop)
                                {
                                    pixelDrawn[j + offset, i + offset] = true;
                                    if (j + offset - 2 >= img.Width || i + offset - 2 >= img.Height)
                                    {
                                        Console.WriteLine("BREAK!");
                                        loop = false;
                                        break;
                                    }
                                    else
                                    {
                                        offset++;
                                        SetCursorPos(mouseX + j + offset, mouseY + i + offset);
                                        System.Threading.Thread.Sleep(10);
                                    }
                                }
                            }
                            catch (Exception)
                            {
                                loop = false;
                            }
                            
                            LeftMouseUp(mouseX + j + (offset - 1), mouseY + i + (offset - 1));
                        }
                    }
                }
            }
        }
    }
}
