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

        //////////////////////////////////////////////////////////////////////////////////////////////////
        // INITALIZATION
        //////////////////////////////////////////////////////////////////////////////////////////////////
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

            // hide taskbar icon
            notifyIcon1.Visible = false;
		}

        //////////////////////////////////////////////////////////////////////////////////////////////////
        // FORM METHODS
        //////////////////////////////////////////////////////////////////////////////////////////////////
        private void Form1_Resize(object sender, EventArgs e)
        {
            // hide when minimized, we have a taskbar icon for that
            if (WindowState == FormWindowState.Minimized)
            {
                Hide();

                // show taskbar icon
                notifyIcon1.Visible = true;
            }
            else
            {
                // hide taskbar icon
                notifyIcon1.Visible = false;
            }
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            // show form when taskbar icon was double clicked
            Show();
            WindowState = FormWindowState.Normal;
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            quitToolStripMenuItem_Click(sender, e);
        }
        
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            notifyIcon1_DoubleClick(sender, e);
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////
        // NETWORK METHODS
        //////////////////////////////////////////////////////////////////////////////////////////////////
        private void NetworkError(String errorMsg)
        {
            Console.WriteLine("NETWORK ERROR OCCURED: " + errorMsg);
        }

        private void ServerDataReceived(NetworkPackage nwPackage)
        {
            // if we have not yet received any data, start playing with this client
            if (GameData.g_ConnectionID == 0 && nwPackage.m_Command == NetworkCommand.CONNECT_REQUEST)
            {
                // store our new connection ID, blocking transmissions from other sources
                GameData.g_ConnectionID = nwPackage.m_ConnectionID;

                // let other side know we agree
                nwPackage.m_Command = NetworkCommand.CONNECT_ACCEPT;
                m_Client.Send(nwPackage);
            }
            // game in progress
            else if (GameData.g_ConnectionID != 0)
            {
                // make sure connection ID's match up
                if (GameData.g_ConnectionID != nwPackage.m_ConnectionID)
                {
                    NetworkError("Received data from unknown client, discarding...");
                }
                else
                {
                    // process data
                    ProcessNetworkData(nwPackage);
                }
            }  
        }

        private void ProcessNetworkData(NetworkPackage nwPackage)
        {
            // process network data
            switch (nwPackage.m_Command)
            {
                case NetworkCommand.CONNECT_ACCEPT:
                {
                    // if these aren't the same there is a problem
                    if (GameData.g_ConnectionID != nwPackage.m_ConnectionID)
                    {
                        NetworkError("Connection ID's do not match up while connecting... abort.");
                        GameData.g_ConnectionID = 0;
                    }

                    Console.WriteLine("Connection synchronized...");

                    break;
                }
                case NetworkCommand.NEW_GAME_REQUEST:
                {
                    // reset board
                    this.ChessRules.NewGame();

                    // let other side know we agree
                    nwPackage.m_Command = NetworkCommand.NEW_GAME_ACCEPT;
                    m_Client.Send(nwPackage);

                    break;
                }
                case NetworkCommand.NEW_GAME_ACCEPT:
                {
                    // other side accepted, reset board
                    this.ChessRules.NewGame();

                    break;
                }
                case NetworkCommand.MAKE_MOVE_REQUEST:
                {
                    bool bMoveAllowed = this.ChessRules.IsMoveAllowed(nwPackage.m_FromSquare, nwPackage.m_ToSquare);
                    if (bMoveAllowed)
                    {
                        // move is allowed, let other side know
                        nwPackage.m_Command = NetworkCommand.MAKE_MOVE_ACCEPT;
                        m_Client.Send(nwPackage);

                        // make the actual move
                        this.ChessRules.DoMove(nwPackage.m_FromSquare, nwPackage.m_ToSquare);

                        // update the board
                        this.Chessboard.Invalidate();

                        // notify user
                        notifyIcon1.ShowBalloonTip(1, "", "A move was made from " + nwPackage.m_FromSquare.ToString() + " to " + nwPackage.m_ToSquare.ToString(), ToolTipIcon.Info);
                    }
                    else
                    {
                        // let other side know we do not agree with this move
                        nwPackage.m_Command = NetworkCommand.MAKE_MOVE_DENY;
                        m_Client.Send(nwPackage);
                    }
                    break;
                }
                case NetworkCommand.MAKE_MOVE_ACCEPT:
                {
                    // move was accepted by the other side, make move
                    this.ChessRules.DoMove(nwPackage.m_FromSquare, nwPackage.m_ToSquare);

                    // update the board
                    this.Chessboard.Invalidate();

                    break;
                }
                case NetworkCommand.MAKE_MOVE_DENY:
                {
                    NetworkError("Other side reports move is invalid, boards could be out of sync.");
                    break;
                }
                default:
                {
                    NetworkError("Handling unspecified case!");
                    break;
                }
            }


        }

        //////////////////////////////////////////////////////////////////////////////////////////////////
        // CHESSBOARD METHODS
        //////////////////////////////////////////////////////////////////////////////////////////////////
        // update our status bar
        private void Chessboard_MouseMove(object sender, MouseEventArgs e)
        {
            int curCol = Etc.Clamp(this.Chessboard.GetMouseColPosition(), 0, 7)     + 1;
            int curRow = Etc.Clamp(this.Chessboard.GetMouseRowPosition(), 0, 7)     + 1;
            int curSqu = Etc.Clamp(this.Chessboard.GetMouseSquarePosition(), 0, 63) + 1;

            this.toolStripStatusLabel1.Text = "Column: " + curCol.ToString() + " row: " + curRow.ToString() + " square: " + curSqu.ToString();
        }

        // try to make the move
		private void Chessboard_OnMoveMade(int CurrSquare, int TargetSquare)
		{
			Console.WriteLine("A move was made from " + CurrSquare.ToString() + " to " + TargetSquare.ToString());
			
			bool bMoveAllowed = this.ChessRules.IsMoveAllowed(CurrSquare, TargetSquare);
            
            Console.WriteLine("The rules says... " + ((bMoveAllowed == true) ? "allowed :-)" : "not allowed :-("));

            // send through network
            if (bMoveAllowed)
            {
                NetworkPackage nwpack = new NetworkPackage();
                nwpack.m_Command = NetworkCommand.MAKE_MOVE_REQUEST;
                nwpack.m_FromSquare = (byte)CurrSquare;
                nwpack.m_ToSquare = (byte)TargetSquare;
                nwpack.m_ConnectionID = GameData.g_ConnectionID;

                m_Client.Send(nwpack);
            }
		}

        //////////////////////////////////////////////////////////////////////////////////////////////////
        // MENU METHODS
        //////////////////////////////////////////////////////////////////////////////////////////////////
        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // stop listening
            m_Server.Stop();

            // exit
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
        }

        private void showAttackedSquaresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Chessboard.SetShowAttackedPieces(showAttackedSquaresToolStripMenuItem.Checked);
        }

        private void showValidMovesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Chessboard.SetShowValidMoves(showValidMovesToolStripMenuItem.Checked);
        }

        private void resignToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // send resign message to other side
            NetworkPackage nwpack = new NetworkPackage();
            nwpack.m_Command = NetworkCommand.RESIGN_REQUEST;
            nwpack.m_FromSquare = 0;
            nwpack.m_ToSquare = 0;
            nwpack.m_ConnectionID = GameData.g_ConnectionID;

            m_Client.Send(nwpack);
        }

        private void offerDrawToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // send draw offer message to other side
            NetworkPackage nwpack = new NetworkPackage();
            nwpack.m_Command = NetworkCommand.OFFER_DRAW_REQUEST;
            nwpack.m_FromSquare = 0;
            nwpack.m_ToSquare = 0;
            nwpack.m_ConnectionID = GameData.g_ConnectionID;

            m_Client.Send(nwpack);
        }

        private void takeBackMoveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // send take back move request message to other side
            NetworkPackage nwpack = new NetworkPackage();
            nwpack.m_Command = NetworkCommand.TAKE_BACK_MOVE_REQUEST;
            nwpack.m_FromSquare = (byte)GameData.g_LastMove.FromSquare;
            nwpack.m_ToSquare = (byte)GameData.g_LastMove.ToSquare;
            nwpack.m_ConnectionID = GameData.g_ConnectionID;

            m_Client.Send(nwpack);
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////
        // DEBUG METHODS
        //////////////////////////////////////////////////////////////////////////////////////////////////
        private void button1_Click(object sender, EventArgs e)
        {
            m_Server.Stop();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            m_Server.Start();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // send take back move request message to other side
            NetworkPackage nwpack = new NetworkPackage();
            nwpack.m_Command = NetworkCommand.CONNECT_REQUEST;

            m_Client.Send(nwpack);

            // TODO: build timer for connection request time-out
        }
	}
}