namespace Guesser
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.groupLogin = new System.Windows.Forms.GroupBox();
            this.checkGreet = new System.Windows.Forms.CheckBox();
            this.butRoomRefresh = new System.Windows.Forms.Button();
            this.comboRoom = new System.Windows.Forms.ComboBox();
            this.labelPass = new System.Windows.Forms.Label();
            this.textPass = new System.Windows.Forms.TextBox();
            this.labelRoom = new System.Windows.Forms.Label();
            this.labelUsername = new System.Windows.Forms.Label();
            this.textUser = new System.Windows.Forms.TextBox();
            this.butLogin = new System.Windows.Forms.Button();
            this.fileDialogAvatar = new System.Windows.Forms.OpenFileDialog();
            this.timerPing = new System.Windows.Forms.Timer(this.components);
            this.textChat = new System.Windows.Forms.TextBox();
            this.timerChatUpdate = new System.Windows.Forms.Timer(this.components);
            this.groupDraw = new System.Windows.Forms.GroupBox();
            this.butGIF = new System.Windows.Forms.Button();
            this.butQueue = new System.Windows.Forms.Button();
            this.butDraw = new System.Windows.Forms.Button();
            this.labelURL = new System.Windows.Forms.Label();
            this.textURL = new System.Windows.Forms.TextBox();
            this.backChat = new System.ComponentModel.BackgroundWorker();
            this.richChat = new System.Windows.Forms.RichTextBox();
            this.labelStatus = new System.Windows.Forms.Label();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.drawToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startRecordingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopRecordingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playRecordingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.grindToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopGrindToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timerRecord = new System.Windows.Forms.Timer(this.components);
            this.saveDialogRecord = new System.Windows.Forms.SaveFileDialog();
            this.openDialogRecord = new System.Windows.Forms.OpenFileDialog();
            this.checkPartial = new System.Windows.Forms.CheckBox();
            this.backGrind = new System.ComponentModel.BackgroundWorker();
            this.checkGuess = new System.Windows.Forms.CheckBox();
            this.backPartial = new System.ComponentModel.BackgroundWorker();
            this.backPartialGuess = new System.ComponentModel.BackgroundWorker();
            this.timerGIF = new System.Windows.Forms.Timer(this.components);
            this.backSend = new System.ComponentModel.BackgroundWorker();
            this.groupLogin.SuspendLayout();
            this.groupDraw.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupLogin
            // 
            this.groupLogin.Controls.Add(this.checkGreet);
            this.groupLogin.Controls.Add(this.butRoomRefresh);
            this.groupLogin.Controls.Add(this.comboRoom);
            this.groupLogin.Controls.Add(this.labelPass);
            this.groupLogin.Controls.Add(this.textPass);
            this.groupLogin.Controls.Add(this.labelRoom);
            this.groupLogin.Controls.Add(this.labelUsername);
            this.groupLogin.Controls.Add(this.textUser);
            this.groupLogin.Controls.Add(this.butLogin);
            this.groupLogin.Location = new System.Drawing.Point(12, 74);
            this.groupLogin.Name = "groupLogin";
            this.groupLogin.Size = new System.Drawing.Size(233, 123);
            this.groupLogin.TabIndex = 2;
            this.groupLogin.TabStop = false;
            this.groupLogin.Text = "Login";
            // 
            // checkGreet
            // 
            this.checkGreet.AutoSize = true;
            this.checkGreet.Checked = true;
            this.checkGreet.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkGreet.Location = new System.Drawing.Point(6, 100);
            this.checkGreet.Name = "checkGreet";
            this.checkGreet.Size = new System.Drawing.Size(52, 17);
            this.checkGreet.TabIndex = 9;
            this.checkGreet.Text = "Greet";
            this.checkGreet.UseVisualStyleBackColor = true;
            // 
            // butRoomRefresh
            // 
            this.butRoomRefresh.BackColor = System.Drawing.Color.PowderBlue;
            this.butRoomRefresh.Location = new System.Drawing.Point(50, 44);
            this.butRoomRefresh.Name = "butRoomRefresh";
            this.butRoomRefresh.Size = new System.Drawing.Size(20, 20);
            this.butRoomRefresh.TabIndex = 8;
            this.butRoomRefresh.Text = "R";
            this.butRoomRefresh.UseVisualStyleBackColor = false;
            this.butRoomRefresh.Click += new System.EventHandler(this.butRoomRefresh_Click);
            // 
            // comboRoom
            // 
            this.comboRoom.FormattingEnabled = true;
            this.comboRoom.Location = new System.Drawing.Point(77, 44);
            this.comboRoom.Name = "comboRoom";
            this.comboRoom.Size = new System.Drawing.Size(150, 21);
            this.comboRoom.TabIndex = 7;
            this.comboRoom.SelectedIndexChanged += new System.EventHandler(this.comboRoom_SelectedIndexChanged);
            // 
            // labelPass
            // 
            this.labelPass.Location = new System.Drawing.Point(6, 74);
            this.labelPass.Name = "labelPass";
            this.labelPass.Size = new System.Drawing.Size(65, 17);
            this.labelPass.TabIndex = 6;
            this.labelPass.Text = "Pass:";
            // 
            // textPass
            // 
            this.textPass.Location = new System.Drawing.Point(77, 71);
            this.textPass.Name = "textPass";
            this.textPass.Size = new System.Drawing.Size(150, 20);
            this.textPass.TabIndex = 5;
            this.textPass.Text = "kami99";
            this.textPass.UseSystemPasswordChar = true;
            // 
            // labelRoom
            // 
            this.labelRoom.Location = new System.Drawing.Point(6, 48);
            this.labelRoom.Name = "labelRoom";
            this.labelRoom.Size = new System.Drawing.Size(38, 17);
            this.labelRoom.TabIndex = 4;
            this.labelRoom.Text = "Room:";
            // 
            // labelUsername
            // 
            this.labelUsername.Location = new System.Drawing.Point(6, 22);
            this.labelUsername.Name = "labelUsername";
            this.labelUsername.Size = new System.Drawing.Size(65, 17);
            this.labelUsername.TabIndex = 2;
            this.labelUsername.Text = "Username:";
            // 
            // textUser
            // 
            this.textUser.Location = new System.Drawing.Point(77, 19);
            this.textUser.Name = "textUser";
            this.textUser.Size = new System.Drawing.Size(150, 20);
            this.textUser.TabIndex = 1;
            this.textUser.Text = "SketchoBOT";
            this.textUser.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textUser_KeyDown);
            // 
            // butLogin
            // 
            this.butLogin.Location = new System.Drawing.Point(77, 97);
            this.butLogin.Name = "butLogin";
            this.butLogin.Size = new System.Drawing.Size(150, 20);
            this.butLogin.TabIndex = 0;
            this.butLogin.Text = "Login";
            this.butLogin.UseVisualStyleBackColor = true;
            this.butLogin.Click += new System.EventHandler(this.butLogin_Click);
            // 
            // fileDialogAvatar
            // 
            this.fileDialogAvatar.Title = "Select avatar...";
            // 
            // timerPing
            // 
            this.timerPing.Enabled = true;
            this.timerPing.Interval = 15000;
            this.timerPing.Tick += new System.EventHandler(this.timerPing_Tick);
            // 
            // textChat
            // 
            this.textChat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textChat.Location = new System.Drawing.Point(251, 255);
            this.textChat.Name = "textChat";
            this.textChat.Size = new System.Drawing.Size(250, 20);
            this.textChat.TabIndex = 3;
            this.textChat.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textChat_KeyDown);
            // 
            // timerChatUpdate
            // 
            this.timerChatUpdate.Tick += new System.EventHandler(this.timerChatUpdate_Tick);
            // 
            // groupDraw
            // 
            this.groupDraw.Controls.Add(this.butGIF);
            this.groupDraw.Controls.Add(this.butQueue);
            this.groupDraw.Controls.Add(this.butDraw);
            this.groupDraw.Controls.Add(this.labelURL);
            this.groupDraw.Controls.Add(this.textURL);
            this.groupDraw.Location = new System.Drawing.Point(12, 203);
            this.groupDraw.Name = "groupDraw";
            this.groupDraw.Size = new System.Drawing.Size(233, 72);
            this.groupDraw.TabIndex = 4;
            this.groupDraw.TabStop = false;
            this.groupDraw.Text = "Draw Image";
            // 
            // butGIF
            // 
            this.butGIF.Location = new System.Drawing.Point(183, 41);
            this.butGIF.Name = "butGIF";
            this.butGIF.Size = new System.Drawing.Size(44, 23);
            this.butGIF.TabIndex = 7;
            this.butGIF.Text = "GIF";
            this.butGIF.UseVisualStyleBackColor = true;
            this.butGIF.Click += new System.EventHandler(this.butGIF_Click);
            // 
            // butQueue
            // 
            this.butQueue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.butQueue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.butQueue.Location = new System.Drawing.Point(6, 41);
            this.butQueue.Name = "butQueue";
            this.butQueue.Size = new System.Drawing.Size(65, 23);
            this.butQueue.TabIndex = 6;
            this.butQueue.Text = "Queue";
            this.butQueue.UseVisualStyleBackColor = true;
            this.butQueue.Click += new System.EventHandler(this.butQueue_Click);
            // 
            // butDraw
            // 
            this.butDraw.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.butDraw.Location = new System.Drawing.Point(77, 41);
            this.butDraw.Name = "butDraw";
            this.butDraw.Size = new System.Drawing.Size(100, 23);
            this.butDraw.TabIndex = 5;
            this.butDraw.Text = "Draw";
            this.butDraw.UseVisualStyleBackColor = true;
            this.butDraw.Click += new System.EventHandler(this.butDraw_Click);
            // 
            // labelURL
            // 
            this.labelURL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelURL.Location = new System.Drawing.Point(6, 18);
            this.labelURL.Name = "labelURL";
            this.labelURL.Size = new System.Drawing.Size(65, 17);
            this.labelURL.TabIndex = 4;
            this.labelURL.Text = "URL:";
            // 
            // textURL
            // 
            this.textURL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textURL.Location = new System.Drawing.Point(77, 15);
            this.textURL.Name = "textURL";
            this.textURL.Size = new System.Drawing.Size(150, 20);
            this.textURL.TabIndex = 3;
            // 
            // backChat
            // 
            this.backChat.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backChat_DoWork);
            this.backChat.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backChat_RunWorkerCompleted);
            // 
            // richChat
            // 
            this.richChat.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richChat.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.richChat.Location = new System.Drawing.Point(251, 26);
            this.richChat.Name = "richChat";
            this.richChat.ReadOnly = true;
            this.richChat.Size = new System.Drawing.Size(375, 212);
            this.richChat.TabIndex = 7;
            this.richChat.Text = "";
            this.richChat.TextChanged += new System.EventHandler(this.richChat_TextChanged);
            // 
            // labelStatus
            // 
            this.labelStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelStatus.ForeColor = System.Drawing.Color.DarkRed;
            this.labelStatus.Location = new System.Drawing.Point(12, 27);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(233, 44);
            this.labelStatus.TabIndex = 8;
            this.labelStatus.Text = "Disconnected";
            this.labelStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.drawToolStripMenuItem,
            this.grindToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.menuStrip.Size = new System.Drawing.Size(638, 24);
            this.menuStrip.TabIndex = 9;
            this.menuStrip.Text = "menuStrip";
            // 
            // drawToolStripMenuItem
            // 
            this.drawToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startRecordingToolStripMenuItem,
            this.stopRecordingToolStripMenuItem,
            this.playRecordingToolStripMenuItem});
            this.drawToolStripMenuItem.Name = "drawToolStripMenuItem";
            this.drawToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.drawToolStripMenuItem.Text = "Draw";
            // 
            // startRecordingToolStripMenuItem
            // 
            this.startRecordingToolStripMenuItem.Name = "startRecordingToolStripMenuItem";
            this.startRecordingToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.startRecordingToolStripMenuItem.Text = "Start Recording";
            this.startRecordingToolStripMenuItem.Click += new System.EventHandler(this.startRecordingToolStripMenuItem_Click);
            // 
            // stopRecordingToolStripMenuItem
            // 
            this.stopRecordingToolStripMenuItem.Enabled = false;
            this.stopRecordingToolStripMenuItem.Name = "stopRecordingToolStripMenuItem";
            this.stopRecordingToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.stopRecordingToolStripMenuItem.Text = "Stop Recording";
            this.stopRecordingToolStripMenuItem.Click += new System.EventHandler(this.stopRecordingToolStripMenuItem_Click);
            // 
            // playRecordingToolStripMenuItem
            // 
            this.playRecordingToolStripMenuItem.Name = "playRecordingToolStripMenuItem";
            this.playRecordingToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.playRecordingToolStripMenuItem.Text = "Play Recording";
            this.playRecordingToolStripMenuItem.Click += new System.EventHandler(this.playRecordingToolStripMenuItem_Click);
            // 
            // grindToolStripMenuItem
            // 
            this.grindToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startToolStripMenuItem,
            this.stopGrindToolStripMenuItem});
            this.grindToolStripMenuItem.Name = "grindToolStripMenuItem";
            this.grindToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.grindToolStripMenuItem.Text = "Grind";
            // 
            // startToolStripMenuItem
            // 
            this.startToolStripMenuItem.Name = "startToolStripMenuItem";
            this.startToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.startToolStripMenuItem.Text = "Start grind";
            this.startToolStripMenuItem.Click += new System.EventHandler(this.startToolStripMenuItem_Click);
            // 
            // stopGrindToolStripMenuItem
            // 
            this.stopGrindToolStripMenuItem.Name = "stopGrindToolStripMenuItem";
            this.stopGrindToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.stopGrindToolStripMenuItem.Text = "Stop grind";
            this.stopGrindToolStripMenuItem.Click += new System.EventHandler(this.stopGrindToolStripMenuItem_Click);
            // 
            // timerRecord
            // 
            this.timerRecord.Interval = 10;
            this.timerRecord.Tick += new System.EventHandler(this.timerRecord_Tick);
            // 
            // saveDialogRecord
            // 
            this.saveDialogRecord.RestoreDirectory = true;
            // 
            // openDialogRecord
            // 
            this.openDialogRecord.FileName = "openDialogRecord";
            // 
            // checkPartial
            // 
            this.checkPartial.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkPartial.AutoSize = true;
            this.checkPartial.Location = new System.Drawing.Point(569, 257);
            this.checkPartial.Name = "checkPartial";
            this.checkPartial.Size = new System.Drawing.Size(55, 17);
            this.checkPartial.TabIndex = 10;
            this.checkPartial.Text = "Partial";
            this.checkPartial.UseVisualStyleBackColor = true;
            // 
            // backGrind
            // 
            this.backGrind.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backGrind_DoWork);
            // 
            // checkGuess
            // 
            this.checkGuess.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkGuess.AutoSize = true;
            this.checkGuess.Checked = true;
            this.checkGuess.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkGuess.Location = new System.Drawing.Point(507, 257);
            this.checkGuess.Name = "checkGuess";
            this.checkGuess.Size = new System.Drawing.Size(56, 17);
            this.checkGuess.TabIndex = 11;
            this.checkGuess.Text = "Guess";
            this.checkGuess.UseVisualStyleBackColor = true;
            // 
            // backPartial
            // 
            this.backPartial.WorkerSupportsCancellation = true;
            this.backPartial.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backPartial_DoWork);
            // 
            // backPartialGuess
            // 
            this.backPartialGuess.WorkerSupportsCancellation = true;
            this.backPartialGuess.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backPartialGuess_DoWork);
            // 
            // timerGIF
            // 
            this.timerGIF.Enabled = true;
            this.timerGIF.Tick += new System.EventHandler(this.timerGIF_Tick);
            // 
            // backSend
            // 
            this.backSend.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backSend_DoWork);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(638, 287);
            this.Controls.Add(this.checkGuess);
            this.Controls.Add(this.checkPartial);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.richChat);
            this.Controls.Add(this.groupDraw);
            this.Controls.Add(this.textChat);
            this.Controls.Add(this.groupLogin);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "Main";
            this.Text = "Guesser";
            this.groupLogin.ResumeLayout(false);
            this.groupLogin.PerformLayout();
            this.groupDraw.ResumeLayout(false);
            this.groupDraw.PerformLayout();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.GroupBox groupLogin;
        private System.Windows.Forms.Button butLogin;
        private System.Windows.Forms.Label labelUsername;
        private System.Windows.Forms.TextBox textUser;
        private System.Windows.Forms.OpenFileDialog fileDialogAvatar;
        private System.Windows.Forms.Timer timerPing;
        private System.Windows.Forms.TextBox textChat;
        private System.Windows.Forms.Timer timerChatUpdate;
        private System.Windows.Forms.GroupBox groupDraw;
        private System.Windows.Forms.Button butDraw;
        private System.Windows.Forms.Label labelURL;
        private System.Windows.Forms.TextBox textURL;
        private System.Windows.Forms.Button butQueue;
        private System.ComponentModel.BackgroundWorker backChat;
        private System.Windows.Forms.Label labelPass;
        private System.Windows.Forms.TextBox textPass;
        private System.Windows.Forms.Label labelRoom;
        private System.Windows.Forms.RichTextBox richChat;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.ComboBox comboRoom;
        private System.Windows.Forms.Button butRoomRefresh;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem drawToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startRecordingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopRecordingToolStripMenuItem;
        private System.Windows.Forms.Timer timerRecord;
        private System.Windows.Forms.SaveFileDialog saveDialogRecord;
        private System.Windows.Forms.ToolStripMenuItem playRecordingToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openDialogRecord;
        private System.Windows.Forms.CheckBox checkPartial;
        private System.Windows.Forms.CheckBox checkGreet;
        private System.Windows.Forms.ToolStripMenuItem grindToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopGrindToolStripMenuItem;
        private System.ComponentModel.BackgroundWorker backGrind;
        private System.Windows.Forms.CheckBox checkGuess;
        private System.ComponentModel.BackgroundWorker backPartial;
        private System.ComponentModel.BackgroundWorker backPartialGuess;
        private System.Windows.Forms.Button butGIF;
        private System.Windows.Forms.Timer timerGIF;
        private System.ComponentModel.BackgroundWorker backSend;
    }
}

