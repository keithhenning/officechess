using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Globals;
using Network;

namespace OfficeChess8
{
	public partial class Form1 : Form
	{
        Network.Client cl = null;

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

            Network.Server srv = new Network.Server();
            srv.StartListen();

            cl = new Network.Client();
		}

		private void Chessboard_OnMoveMade(int CurrSquare, int TargetSquare)
		{
			Console.WriteLine("A move was made from " + CurrSquare.ToString() + " to " + TargetSquare.ToString());
			
			bool bMoveAllowed = this.ChessRules.DoMove(CurrSquare, TargetSquare);

			Console.WriteLine( "The rules says... " + ((bMoveAllowed==true) ? "allowed :-)" : "not allowed :-(") );
		}

		private void button1_Click(object sender, EventArgs e)
		{
			GameData.SaveToFile("savegame.ocs");
		}

		private void button2_Click(object sender, EventArgs e)
		{
			GameData.LoadFromFile("savegame.ocs");
		}

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void saveGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GameData.SaveToFile("savegame.ocs");
        }

        private void loadGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GameData.LoadFromFile("savegame.ocs");
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ChessRules.NewGame();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        private void Chessboard_MouseMove(object sender, MouseEventArgs e)
        {
            int curCol = this.Chessboard.GetMouseColPosition();
            if (curCol < 0)
                curCol = 0;

            int curRow = this.Chessboard.GetMouseRowPosition();
            if (curRow < 0)
                curRow = 0;

            int curSqu = this.Chessboard.GetMouseSquarePosition();
            if (curSqu < 0)
                curSqu = 0;

            this.toolStripStatusLabel1.Text = "Column: " + curCol.ToString() + " row: " + curRow.ToString() + " square: " + curSqu.ToString();
        }

        private void showAttackedSquaresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Chessboard.SetShowAttackedPieces(showAttackedSquaresToolStripMenuItem.Checked);
        }

        private void showValidMovesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Chessboard.SetShowValidMoves(showValidMovesToolStripMenuItem.Checked);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            cl.Connect("127.0.0.1", 12345);

            NetworkPackage nwpack = new NetworkPackage();
            nwpack.m_Command = NetworkCommand.MAKE_MOVE;
            nwpack.m_FromSquare = 5;
            nwpack.m_ToSquare = 16;

            cl.Send(Etc.ObjectToByteArray(nwpack));
        }
	}
}