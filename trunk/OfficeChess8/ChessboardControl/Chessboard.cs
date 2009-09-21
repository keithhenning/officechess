using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Globals;

namespace ChessboardControl
{
	public partial class Chessboard : UserControl
	{
		//////////////////////////////////////////////////////////////////////////
		// Enumerators
		//////////////////////////////////////////////////////////////////////////
		#region Enumerators

		// enumarates our piece images
		private enum PieceImages
		{
			kImageWhitePawn = 0,
			kImageWhiteKnight,
			kImageWhiteKing,
			kImageWhiteBishop,
			kImageWhiteRook,
			kImageWhiteQueen,
			kImageBlackQueen,
			kImageBlackRook,
			kImageBlackBishop,
			kImageBlackKing,
			kImageBlackKnight,
			kImageBlackPawn,
			kNumImages
		}

		#endregion

		//////////////////////////////////////////////////////////////////////////
		// Events
		//////////////////////////////////////////////////////////////////////////
		#region Events
		public delegate void NotifyMoveMade(int CurrSquare, int TargetSquare);
		public NotifyMoveMade EventMoveMade;
		#endregion

		//////////////////////////////////////////////////////////////////////////
		// Members
		//////////////////////////////////////////////////////////////////////////
		#region Members

		private Point m_LocalMousePosition = new Point();
		private int m_SquareSize = 0;
		private Image[] m_PieceBitmaps = new Image[12];
		private int m_SelectedSquare = 0;
		private int m_TargetSquare = 0;
		private bool m_bMouseDown = false;
		private float m_MouseDownPosX = 0.0f;
		private float m_MouseDownPosY = 0.0f;
		private float m_PieceOffsetX = 0.0f;
		private float m_PieceOffsetY = 0.0f;
		private bool m_bImageFilesLoaded = false;
		private List<int> m_SupportedSquareSizes = new List<int>();
		private bool m_bShownValidMoves = true;
		private bool m_bShownAttackedPieces = true;

		#endregion

		//////////////////////////////////////////////////////////////////////////
		// Initalization
		//////////////////////////////////////////////////////////////////////////
		#region Initalization

		public Chessboard()
		{
			InitializeComponent();

			// set styles
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			SetStyle(ControlStyles.ResizeRedraw, true);
			SetStyle(ControlStyles.UserPaint, true);

			// add handlers
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(Chessboard_MouseDown);
			this.MouseUp += new System.Windows.Forms.MouseEventHandler(Chessboard_MouseUp);
			this.MouseMove += new System.Windows.Forms.MouseEventHandler(Chessboard_MouseMove);

			// set properties
			this.BorderStyle = BorderStyle.FixedSingle;

			// init variables
			m_SquareSize = this.Width / 8;

			// invalidate mouse position and reset variables
			m_LocalMousePosition.X = -1;
			m_LocalMousePosition.Y = -1;
			m_SelectedSquare = -1;
			m_TargetSquare = 0;
			m_bMouseDown = false;
			m_PieceOffsetX = 0.0f;
			m_PieceOffsetY = 0.0f;
		}
		
		// do all kinds of stuff here we can't do in the designer
		public void Initialize()
		{
			// start loading bitmaps for pieces
			m_bImageFilesLoaded = LoadBitmaps();
		}

		#endregion
		//////////////////////////////////////////////////////////////////////////
		// Helpers
		//////////////////////////////////////////////////////////////////////////
		#region Helpers

		// gets column the mouse is hovering over
		public int GetMouseColPosition()
		{
			// make sure we have a valid coordinate
			if (m_LocalMousePosition.X < 0)
				return -1;

			// calculate normalized coordinates and expand it to range 0...8
			float normX = (float)m_LocalMousePosition.X / (float)this.Width * 8.0f;
			int col = Convert.ToInt32(Math.Ceiling(normX)) - 1;
			return col;
		}

		// gets row the mouse is hovering over
		public int GetMouseRowPosition()
		{
			// make sure we have a valid coordinate
			if (m_LocalMousePosition.Y < 0)
				return -1;

			// calculate normalized coordinates and expand it to range 0...8
			float normY = (float)m_LocalMousePosition.Y / (float)this.Height * 8.0f;
			int row = Convert.ToInt32(Math.Ceiling(normY)) - 1;
			return row;
		}

		// gets square the mouse is hovering over
		public int GetMouseSquarePosition()
		{
			// calculate square position based on current col and row number
			int square = (GetMouseRowPosition() * 8) + GetMouseColPosition();
			
			// make sure the mouse is over a valid square
			if (square >= 0)
				return square;
			else
				return -1;
		}

		// gets closest highest number in array
		private int GetNextValidSize(int size, List<int> validSizes)  
		{          
			int returnValue = size;
 
			// make sure list is sorted
			validSizes.Sort();

			foreach (int validSize in validSizes)      
			{          
				if (validSize >= size)          
				{              
					return validSize;          
				}      
			}      // Nothing valid          
			return size;  
		}

		// loads bitmaps appropriate for the current square size
		private bool LoadBitmaps()
		{
			// the locals
			int ImageSize = 0;
			bool bResult = false;
			string BaseDir = "../../../Resources/Pieces/";

			// read all directories in the resource path, try to convert the name to an int,
			// this ensures we are dealing with the correct directories
			string[] DirectoryNames = Directory.GetDirectories(BaseDir);
			foreach (string DirectoryName in DirectoryNames)
			{
				if (Int32.TryParse(Path.GetFileName(DirectoryName).ToString(), out ImageSize) )
				{
					m_SupportedSquareSizes.Add(ImageSize);
				}
			}

			// calculate closest available image size for our square size
			ImageSize = GetNextValidSize(m_SquareSize, m_SupportedSquareSizes);

			// start loading  images
			string ResourcePath = BaseDir + ImageSize + "/";
			try
			{
				m_PieceBitmaps[(int)PieceImages.kImageBlackBishop] = Image.FromFile(ResourcePath + "bb.png");
				m_PieceBitmaps[(int)PieceImages.kImageBlackKing] = Image.FromFile(ResourcePath + "bk.png");
				m_PieceBitmaps[(int)PieceImages.kImageBlackKnight] = Image.FromFile(ResourcePath + "bn.png");
				m_PieceBitmaps[(int)PieceImages.kImageBlackPawn] = Image.FromFile(ResourcePath + "bp.png");
				m_PieceBitmaps[(int)PieceImages.kImageBlackQueen] = Image.FromFile(ResourcePath + "bq.png");
				m_PieceBitmaps[(int)PieceImages.kImageBlackRook] = Image.FromFile(ResourcePath + "br.png");

				m_PieceBitmaps[(int)PieceImages.kImageWhiteBishop] = Image.FromFile(ResourcePath + "wb.png");
				m_PieceBitmaps[(int)PieceImages.kImageWhiteKing] = Image.FromFile(ResourcePath + "wk.png");
				m_PieceBitmaps[(int)PieceImages.kImageWhiteKnight] = Image.FromFile(ResourcePath + "wn.png");
				m_PieceBitmaps[(int)PieceImages.kImageWhitePawn] = Image.FromFile(ResourcePath + "wp.png");
				m_PieceBitmaps[(int)PieceImages.kImageWhiteQueen] = Image.FromFile(ResourcePath + "wq.png");
				m_PieceBitmaps[(int)PieceImages.kImageWhiteRook] = Image.FromFile(ResourcePath + "wr.png");
				bResult = true;
			}
			catch (System.IO.IOException ioex)
			{
				Console.WriteLine("An error occurred while trying to load piece image files:" + ioex.Message.ToString());
				bResult = false;
			}

			return bResult;
		}

		#endregion

		//////////////////////////////////////////////////////////////////////////
		// MouseEvents
		//////////////////////////////////////////////////////////////////////////
		#region MouseEvents

		// handles mouse down event
		private void Chessboard_MouseDown(object sender, MouseEventArgs e)
		{
			// set bool
			m_bMouseDown = true;

			// set selected square, if already selected, unselect
			m_SelectedSquare = GetMouseSquarePosition();

			// make board repaint when the mouse moves
			this.Invalidate(new Rectangle(0, 0, this.Width, this.Height));

			// store mouse down x and y so we can calculate the offset from this spot later on
			m_MouseDownPosX = PointToClient(MousePosition).X;
			m_MouseDownPosY = PointToClient(MousePosition).Y;
		}

		// handles mouse up event
		private void Chessboard_MouseUp(object sender, MouseEventArgs e)
		{
			// set bool
			m_bMouseDown = false;

			// reset offsets
			m_PieceOffsetX = 0.0f;
			m_PieceOffsetY = 0.0f;

			// store target square
			m_TargetSquare = GetMouseSquarePosition();

			// trigger event to notify listeners the player made a move
			if (EventMoveMade != null && m_SelectedSquare != m_TargetSquare)
				EventMoveMade(m_SelectedSquare, m_TargetSquare);
		}

		// handles mouse mouse event
		private void Chessboard_MouseMove(object sender, MouseEventArgs e)
		{
			// get local mouse location
			m_LocalMousePosition = PointToClient(MousePosition);

			// clamp location
			m_LocalMousePosition.X = Etc.Clamp(m_LocalMousePosition.X, 1, this.Width);
			m_LocalMousePosition.Y = Etc.Clamp(m_LocalMousePosition.Y, 1, this.Height);

			// invert Y coordinate
			m_LocalMousePosition.Y = this.Height - m_LocalMousePosition.Y;

			// update mouse down offset
			if (m_bMouseDown)
			{
				m_PieceOffsetX = m_LocalMousePosition.X - m_MouseDownPosX;
				m_PieceOffsetY = this.Height - m_LocalMousePosition.Y - m_MouseDownPosY;
			}

			// make board repaint when the mouse moves
			this.Invalidate(new Rectangle(0, 0, this.Width, this.Height));
		}

		// mouse leave event
		private void Chessboard_MouseLeave(object sender, EventArgs e)
		{
			// invalidate mouse position and reset variables
			m_LocalMousePosition.X = -1;
			m_LocalMousePosition.Y = -1;
			m_TargetSquare = 0;
			m_bMouseDown = false;
			m_PieceOffsetX = 0.0f;
			m_PieceOffsetY = 0.0f;

			// make board repaint when the mouse moves out
			this.Invalidate(new Rectangle(0, 0, this.Width, this.Height));
		}

		#endregion

		//////////////////////////////////////////////////////////////////////////
		// Drawing
		//////////////////////////////////////////////////////////////////////////
		#region Drawing

		// override onpaint
		protected override void OnPaint(PaintEventArgs e)
		{
			// call the super
			base.OnPaint(e);

			// clear background
			e.Graphics.Clear(Color.Black);

			// first draw the board
			Chessboard_DrawCheckerdBoard(e);

			// now draw the pieces
			Chessboard_DrawPieces(e);
		}

		// draw a checkered board
		private void Chessboard_DrawCheckerdBoard(PaintEventArgs e)
		{
			int square = 0;
			for (int r = 0; r < 8; ++r)
			{
				for (int c = 0; c < 8; ++c)
				{
					RectangleF rect = new RectangleF(c * m_SquareSize, (7 - r) * m_SquareSize, m_SquareSize, m_SquareSize);
					Etc.GetSquareFromRowCol(r, c, out square);
					
					// color the squares
					if ((c + r) % 2 == 0)
					{
						e.Graphics.FillRectangle(Brushes.LightGray, rect);
					}
					else
					{
						e.Graphics.FillRectangle(Brushes.White, rect);
					}

					// color square the mouse is over
					if (GetMouseSquarePosition() == square)
					{
						e.Graphics.FillRectangle(Brushes.LightBlue, rect);
					}

					// color square the mouse is over
					if (m_SelectedSquare >= 0 && m_SelectedSquare == square)
					{
						e.Graphics.FillRectangle(Brushes.MediumSeaGreen, rect);
					}

					// show attacked pieces
					if (m_bShownAttackedPieces)
					{
						if (GameData.g_SquaresAttackedByBlack.Contains(square) && 
							GameData.g_CurrentGameState[square] != null && 
							GameData.g_CurrentGameState[square].GetColor() == PColor.White)
						{
							rect = new RectangleF(c * m_SquareSize + 6, (7 - r) * m_SquareSize + 6, 6, 6);
							e.Graphics.FillRectangle(Brushes.IndianRed, rect);
						}
						else 
						if (GameData.g_SquaresAttackedByWhite.Contains(square) &&
							GameData.g_CurrentGameState[square] != null && 
							GameData.g_CurrentGameState[square].GetColor() == PColor.Black)
						{
							rect = new RectangleF(c * m_SquareSize + 6, (7 - r) * m_SquareSize + 6, 6, 6);
							e.Graphics.FillRectangle(Brushes.IndianRed, rect);
						}
					}
					
					// now add some indicators
					if (m_bShownValidMoves && m_bMouseDown &&
						GameData.g_CurrentGameState[m_SelectedSquare] != null &&
						GameData.g_CurrentGameState[m_SelectedSquare].GetValidMoves().Contains(square))
					{
						rect = new RectangleF(c * m_SquareSize + 3, (7 - r) * m_SquareSize + 3, 6, 6);
						e.Graphics.FillRectangle(Brushes.MediumSeaGreen, rect);
					}
				}
			}
		}

		// draws the pieces
		private void Chessboard_DrawPieces(PaintEventArgs e)
		{
			// make sure we have our images
			if (!m_bImageFilesLoaded)
				return;

			// locals
			int square = 0;
			RectangleF RectImage;
			Image currImage = null;

			// iterate through board
			for (int r = 0; r < 8; ++r)
			{
				for (int c = 0; c < 8; ++c)
				{
					// create new rectangle
					RectImage = new RectangleF(c * m_SquareSize, (7 - r) * m_SquareSize, m_SquareSize, m_SquareSize);

					// calculate current square we are on
					Etc.GetSquareFromRowCol(r, c, out square);

					// get piece for this square
                    if (GameData.g_CurrentGameState[square] != null)
                    {
                        PType currVal = GameData.g_CurrentGameState[square].GetPieceType();

                        // if there is a piece on this square
                        if (currVal != PType.None)
                        {
                            // get appropriate image for this piece
                            currImage = GetImageForValue(currVal);

                            // draw image
                            if (currImage != null)
                            {
                                // draw images not currently dragged by player
                                if (m_bMouseDown && m_SelectedSquare == square)
                                {
                                    // do not draw this image, we want to draw it later so it overdraws the whole board
                                }
                                else
                                {
                                    // draw image and tag
                                    e.Graphics.DrawImage(currImage, RectImage);
                                }
                            }
                        }
                    }
				}
			}

			// add offset to current image for mouse dragging
			if (m_bMouseDown && m_SelectedSquare >= 0)
			{
                // get row and coloumn for current square
                int r = 0;
                int c = 0;
                Etc.GetRowColFromSquare(m_SelectedSquare, out r, out c);

                // get image on current square
                if (GameData.g_CurrentGameState[m_SelectedSquare] != null)
                {
                    PType currVal = GameData.g_CurrentGameState[m_SelectedSquare].GetPieceType();
                    currImage = GetImageForValue(currVal);

                    // calculate drawing rectangle
                    RectImage = new RectangleF((c * m_SquareSize) + m_PieceOffsetX, ((7 - r) * m_SquareSize) + m_PieceOffsetY, m_SquareSize, m_SquareSize);

                    // draw image
                    if (currImage != null)
                    {
                        e.Graphics.DrawImage(currImage, RectImage);
                    }
                }				
			}
		}

		// return appropriate image for value x
		private Image GetImageForValue(PType currVal)
		{
			Image currImage = null;

			if (currVal == PType.BlackBishop)	{ currImage = m_PieceBitmaps[(int)PieceImages.kImageBlackBishop]; }
			if (currVal == PType.BlackKing)	    { currImage = m_PieceBitmaps[(int)PieceImages.kImageBlackKing]; }
			if (currVal == PType.BlackKnight)	{ currImage = m_PieceBitmaps[(int)PieceImages.kImageBlackKnight]; }
			if (currVal == PType.BlackPawn)	    { currImage = m_PieceBitmaps[(int)PieceImages.kImageBlackPawn]; }
			if (currVal == PType.BlackQueen)	{ currImage = m_PieceBitmaps[(int)PieceImages.kImageBlackQueen]; }
			if (currVal == PType.BlackRook)	    { currImage = m_PieceBitmaps[(int)PieceImages.kImageBlackRook]; }

			if (currVal == PType.WhiteBishop)	{ currImage = m_PieceBitmaps[(int)PieceImages.kImageWhiteBishop]; }
			if (currVal == PType.WhiteKing)	    { currImage = m_PieceBitmaps[(int)PieceImages.kImageWhiteKing]; }
			if (currVal == PType.WhiteKnight)	{ currImage = m_PieceBitmaps[(int)PieceImages.kImageWhiteKnight]; }
			if (currVal == PType.WhitePawn)	    { currImage = m_PieceBitmaps[(int)PieceImages.kImageWhitePawn]; }
			if (currVal == PType.WhiteQueen)	{ currImage = m_PieceBitmaps[(int)PieceImages.kImageWhiteQueen]; }
			if (currVal == PType.WhiteRook)	    { currImage = m_PieceBitmaps[(int)PieceImages.kImageWhiteRook]; }

			return currImage;
		}
		#endregion

		//////////////////////////////////////////////////////////////////////////
		// Form Events
		//////////////////////////////////////////////////////////////////////////
		#region FormEvents

		// resize event
		private void Chessboard_Resize(object sender, EventArgs e)
		{
			// make sure size is proportional
			if (this.Width > this.Height)
				this.Width = this.Height;
			if (this.Height > this.Width)
				this.Height = this.Width;

			// recalc square size
			m_SquareSize = this.Width / 8;
		}

		#endregion
	}
}