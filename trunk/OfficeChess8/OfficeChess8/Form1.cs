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
        public static Network.Server m_Server = new Network.Server();
        public static Network.Client m_Client = new Network.Client();

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

            m_Server.SetServerIP("127.0.0.1");
            m_Server.SetServerPort(12345);
            m_Server.OnNetworkError += NetworkError;
            m_Server.OnNetworkReceivedData += ServerDataReceived;
            
            m_Client.SetTargetIP("127.0.0.1");
            m_Client.SetTargetPort(12345);
            m_Client.OnNetworkError += NetworkError;
		}

        private void NetworkError(String errorMsg)
        {
            Console.WriteLine("NETWORK ERROR OCCURED: " + errorMsg);
        }

        private void ServerDataReceived(NetworkPackage nwPackage)
        {
            // make sure we only process data from correct client
            if ( m_Server.GetConnectionID() != 0 && m_Server.GetConnectionID() != nwPackage.m_ConnectionID )
            {
                Console.WriteLine("Received data from unknown client, discarding...");
            }
            // if we have not yet received any data, start playing with this client
            else if ( m_Server.GetConnectionID() == 0 && nwPackage.m_Command == NetworkCommand.NEW_GAME )
            {
                m_Server.SetConnectionID(nwPackage.m_ConnectionID);
                m_Client.SetConnectionID(nwPackage.m_ConnectionID);
                this.ChessRules.NewGame();
            }
            // game in progress
            else if ( m_Server.GetConnectionID() != 0 )
            {
                // process network data
                switch (nwPackage.m_Command)
                {
                    case NetworkCommand.NEW_GAME:
                    {
                        m_Server.SetConnectionID( nwPackage.m_ConnectionID );
                        m_Client.SetConnectionID( nwPackage.m_ConnectionID );
                        this.ChessRules.NewGame();
                        break;
                    }
                    case NetworkCommand.LOAD_GAME:
                    {
                        break;
                    }
                    case NetworkCommand.MAKE_MOVE:
                    {
                        bool bMoveAllowed = this.ChessRules.IsMoveAllowed(nwPackage.m_FromSquare, nwPackage.m_ToSquare);
                        if (bMoveAllowed)
                        {
                            nwPackage.m_Command = NetworkCommand.MOVE_ACCEPTED;
                            m_Client.Send(nwPackage);
                            
                            this.ChessRules.DoMove(nwPackage.m_FromSquare, nwPackage.m_ToSquare);
                            this.Chessboard.Invalidate();
                        }
                        else
                        {
                            nwPackage.m_Command = NetworkCommand.INVALID_MOVE;
                            m_Client.Send(nwPackage);
                        }
                        break;
                    }
                    case NetworkCommand.MOVE_ACCEPTED:
                    {
                        this.ChessRules.DoMove(nwPackage.m_FromSquare, nwPackage.m_ToSquare);
                        this.Chessboard.Invalidate();
                        Console.WriteLine("Network command success...");
                        break;
                    }
                    case NetworkCommand.INVALID_MOVE:
                    {
                        Console.WriteLine("Other side reports move is invalid, boards could be out of sync.");
                        break;
                    }
                }
            }  
        }

		private void Chessboard_OnMoveMade(int CurrSquare, int TargetSquare)
		{
			Console.WriteLine("A move was made from " + CurrSquare.ToString() + " to " + TargetSquare.ToString());
			
			bool bMoveAllowed = this.ChessRules.IsMoveAllowed(CurrSquare, TargetSquare);

            if (bMoveAllowed)
            {
                NetworkPackage nwpack = new NetworkPackage();
                nwpack.m_Command = NetworkCommand.MAKE_MOVE;
                nwpack.m_FromSquare = (byte)CurrSquare;
                nwpack.m_ToSquare = (byte)TargetSquare;
                nwpack.m_ConnectionID = m_Client.GetConnectionID();

                m_Client.Send(nwpack);
            }
            
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
            NewGameForm ngf = new NewGameForm();
            ngf.ShowDialog();
            m_Server.Start();
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

            NetworkPackage nwpack = new NetworkPackage();
            nwpack.m_Command = NetworkCommand.NEW_GAME;
            nwpack.m_FromSquare = 0xFF;
            nwpack.m_ToSquare = 0xFF;
            nwpack.m_ConnectionID = m_Client.GetConnectionID();
            m_Server.SetConnectionID(m_Client.GetConnectionID());

            m_Client.Send(nwpack);
        }
	}
}