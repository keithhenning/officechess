using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace OfficeChess8
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
		}

		private void Chessboard_Load(object sender, EventArgs e)
		{
			// make the chess board load his assets
			this.Chessboard.Initialize();
			this.Chessboard.EventMoveMade += Chessboard_OnMoveMade;

			// initialize the rules
			this.ChessRules.Initialize();
            this.ChessRules.NewGame();
		}

		private void Chessboard_OnMoveMade(int CurrSquare, int TargetSquare)
		{
			Console.WriteLine("A move was made from " + CurrSquare.ToString() + " to " + TargetSquare.ToString());
			
			bool bMoveAllowed = this.ChessRules.DoMove(CurrSquare, TargetSquare);

			Console.WriteLine( "The rules says... " + ((bMoveAllowed==true) ? "allowed :-)" : "not allowed :-(") );
		}
	}
}