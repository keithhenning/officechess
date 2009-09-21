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
            this.SuspendLayout();
            // 
            // Chessboard
            // 
            this.Chessboard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Chessboard.Location = new System.Drawing.Point(12, 12);
            this.Chessboard.Name = "Chessboard";
            this.Chessboard.Size = new System.Drawing.Size(512, 512);
            this.Chessboard.TabIndex = 0;
            this.Chessboard.Load += new System.EventHandler(this.Chessboard_Load);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(831, 773);
            this.Controls.Add(this.Chessboard);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

		}

		private ChessboardControl.Chessboard Chessboard;
		private ChessLogic.Rules ChessRules = new ChessLogic.Rules();

		#endregion

	}
}

