namespace OfficeChess8
{
	partial class Form1
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
            this.Chessboard = new ChessboardControl.Chessboard();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newGameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadGameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveGameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showAttackedSquaresToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showValidMovesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resignToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.offerDrawToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.takeBackMoveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.button1 = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Chessboard
            // 
            this.Chessboard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Chessboard.Location = new System.Drawing.Point(12, 27);
            this.Chessboard.Name = "Chessboard";
            this.Chessboard.Size = new System.Drawing.Size(512, 512);
            this.Chessboard.TabIndex = 0;
            this.Chessboard.Load += new System.EventHandler(this.Chessboard_Load);
            this.Chessboard.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Chessboard_MouseMove);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(808, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newGameToolStripMenuItem,
            this.loadGameToolStripMenuItem,
            this.saveGameToolStripMenuItem,
            this.quitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newGameToolStripMenuItem
            // 
            this.newGameToolStripMenuItem.Name = "newGameToolStripMenuItem";
            this.newGameToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.newGameToolStripMenuItem.Text = "New game...";
            this.newGameToolStripMenuItem.Click += new System.EventHandler(this.newGameToolStripMenuItem_Click);
            // 
            // loadGameToolStripMenuItem
            // 
            this.loadGameToolStripMenuItem.Name = "loadGameToolStripMenuItem";
            this.loadGameToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.loadGameToolStripMenuItem.Text = "Load game...";
            this.loadGameToolStripMenuItem.Click += new System.EventHandler(this.loadGameToolStripMenuItem_Click);
            // 
            // saveGameToolStripMenuItem
            // 
            this.saveGameToolStripMenuItem.Name = "saveGameToolStripMenuItem";
            this.saveGameToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.saveGameToolStripMenuItem.Text = "Save game...";
            this.saveGameToolStripMenuItem.Click += new System.EventHandler(this.saveGameToolStripMenuItem_Click);
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.quitToolStripMenuItem.Text = "Quit";
            this.quitToolStripMenuItem.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showAttackedSquaresToolStripMenuItem,
            this.showValidMovesToolStripMenuItem,
            this.resignToolStripMenuItem,
            this.offerDrawToolStripMenuItem,
            this.takeBackMoveToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // showAttackedSquaresToolStripMenuItem
            // 
            this.showAttackedSquaresToolStripMenuItem.Checked = true;
            this.showAttackedSquaresToolStripMenuItem.CheckOnClick = true;
            this.showAttackedSquaresToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showAttackedSquaresToolStripMenuItem.Name = "showAttackedSquaresToolStripMenuItem";
            this.showAttackedSquaresToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.showAttackedSquaresToolStripMenuItem.Text = "Show attacked squares";
            this.showAttackedSquaresToolStripMenuItem.Click += new System.EventHandler(this.showAttackedSquaresToolStripMenuItem_Click);
            // 
            // showValidMovesToolStripMenuItem
            // 
            this.showValidMovesToolStripMenuItem.Checked = true;
            this.showValidMovesToolStripMenuItem.CheckOnClick = true;
            this.showValidMovesToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showValidMovesToolStripMenuItem.Name = "showValidMovesToolStripMenuItem";
            this.showValidMovesToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.showValidMovesToolStripMenuItem.Text = "Show valid moves";
            this.showValidMovesToolStripMenuItem.Click += new System.EventHandler(this.showValidMovesToolStripMenuItem_Click);
            // 
            // resignToolStripMenuItem
            // 
            this.resignToolStripMenuItem.Name = "resignToolStripMenuItem";
            this.resignToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.resignToolStripMenuItem.Text = "Resign";
            // 
            // offerDrawToolStripMenuItem
            // 
            this.offerDrawToolStripMenuItem.Name = "offerDrawToolStripMenuItem";
            this.offerDrawToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.offerDrawToolStripMenuItem.Text = "Offer draw";
            // 
            // takeBackMoveToolStripMenuItem
            // 
            this.takeBackMoveToolStripMenuItem.Name = "takeBackMoveToolStripMenuItem";
            this.takeBackMoveToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.takeBackMoveToolStripMenuItem.Text = "Take back move";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpToolStripMenuItem,
            this.aboutToolStripMenuItem1});
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem1
            // 
            this.aboutToolStripMenuItem1.Name = "aboutToolStripMenuItem1";
            this.aboutToolStripMenuItem1.Size = new System.Drawing.Size(116, 22);
            this.aboutToolStripMenuItem1.Text = "About...";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 552);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(808, 22);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(57, 17);
            this.toolStripStatusLabel1.Text = "Waiting...";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(530, 27);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "Send data";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(808, 574);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.Chessboard);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "OfficeChess";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		private ChessboardControl.Chessboard Chessboard;
		private ChessLogic.Rules ChessRules = new ChessLogic.Rules();

		#endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newGameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadGameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showAttackedSquaresToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showValidMovesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resignToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem offerDrawToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem takeBackMoveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem saveGameToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Button button1;

	}
}

