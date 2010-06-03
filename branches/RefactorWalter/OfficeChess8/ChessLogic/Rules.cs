using System;
using System.Collections.Generic;
using System.Text;
using Globals;

namespace ChessLogic
{
	public class Rules
	{
		//////////////////////////////////////////////////////////////////////////
		// Events
		//////////////////////////////////////////////////////////////////////////
		#region Events
		#endregion

		//////////////////////////////////////////////////////////////////////////
		// Members
		//////////////////////////////////////////////////////////////////////////
		#region Members

        private int m_nNumMoves = 0;
        private int m_nNumMovesSinceLastCapture = 0;
        private int m_nNumCaptured = 0;
        //private PColor m_LastColorMoved = PColor.Black;

		#endregion

		//////////////////////////////////////////////////////////////////////////
		// Initialization
		//////////////////////////////////////////////////////////////////////////
		#region Initialization

		public void Initialize()
		{

		}

        #endregion

        //////////////////////////////////////////////////////////////////////////
        // Public methods
        //////////////////////////////////////////////////////////////////////////
        #region Public methods

        // reset all variables to default values
        public void NewGame()
        {
            // reset other variables
            m_nNumMoves = 0;
            m_nNumMovesSinceLastCapture = 0;
            m_nNumCaptured = 0;

            // reset globals
            GameData.g_ColorMoving = PColor.White;
            GameData.g_MoveHistory.Clear();
			ResetBoard();
            Update();
        }

        // evaluates the move
        public bool IsMoveAllowed(int CurrentSquare, int TargetSquare)
        {
            bool bCanMove = false;

            // checks to see if the piece on the current square can move to the destination square
			if (GameData.g_CurrentGameState[CurrentSquare] != null)
				bCanMove = GameData.g_CurrentGameState[CurrentSquare].CanMoveTo(TargetSquare) && !IsChecked(GameData.g_CurrentGameState[CurrentSquare].GetColor());

            return bCanMove;
        }

        // does the actual move and updates all necessary information
        public bool DoMove(int CurrentSquare, int TargetSquare)
        {
            bool bMoveAllowed = false;

            // checks to see if the piece on the current square can move to the destination square
            bMoveAllowed = IsMoveAllowed(CurrentSquare, TargetSquare);

            // do the actual move
            if (bMoveAllowed)
            {
				// update game state
                if (GameData.g_CurrentGameState[CurrentSquare] != null)
                {
                    // first see if we are castling or doing enpassant
                    if (!TryCastling(CurrentSquare, TargetSquare) && !TryEnPassant(CurrentSquare, TargetSquare))
                    {
                        // if not, do normal move
                        GameData.g_CurrentGameState[CurrentSquare].SetPosition(TargetSquare);
                        GameData.g_CurrentGameState[TargetSquare] = GameData.g_CurrentGameState[CurrentSquare];
                        GameData.g_CurrentGameState[CurrentSquare] = null;

                        // store this for 50 move rule
                        if (GameData.g_CurrentGameState[TargetSquare] != null)
                        {
                            m_nNumCaptured++;
                            m_nNumMovesSinceLastCapture = 0;
                        }
                        else
                            m_nNumMovesSinceLastCapture++;
                    }

                    // store last move
                    GameData.g_LastMove.ColorMoved = GameData.g_CurrentGameState[TargetSquare].GetColor();
                    GameData.g_LastMove.FromSquare = CurrentSquare;
                    GameData.g_LastMove.ToSquare = TargetSquare;

                    // store move history
                    GameData.g_MoveHistory.Add(GameData.g_LastMove);

                    // update the pieces
                    Update();

                    // inc num moves
                    m_nNumMoves++;

					// store current color that should be playing
					if (m_nNumMoves % 2 == 0)
						GameData.g_ColorMoving = PColor.White;
					else
						GameData.g_ColorMoving = PColor.Black;
                }
                else
                {
                    Console.WriteLine("WARNING: trying to move piece with NULL pointer, something is very wrong here!");
                }
            }

			return bMoveAllowed;
        }

		#endregion

		//////////////////////////////////////////////////////////////////////////
		// Private / protected methods
		//////////////////////////////////////////////////////////////////////////
		#region Private / protected methods

        // iterates through all pieces and lets them update
        protected void Update()
        {
			UpdatePieces();
			UpdateAttackedSquares();
			UpdateValidMoves();
            UpdateCastling();
        }

		// update all moves and attacked squares for all pieces
		protected void UpdatePieces()
		{
            // let all pieces update their valid moves and their attacked squares
			for (int idx = 0; idx < GameData.g_CurrentGameState.Length; idx++)
			{
				if (GameData.g_CurrentGameState[idx] != null)
					GameData.g_CurrentGameState[idx].Update();
			}
		}

		// updates global list of attacked squares
		protected void UpdateAttackedSquares()
		{
			// update all attacked squares
			List<int> AttackedSquaresWhite = new List<int>();
			List<int> AttackedSquaresBlack = new List<int>();

			// iterate through all pieces of the right color
			for (int idx = 0; idx < GameData.g_CurrentGameState.Length; idx++)
			{
				if (GameData.g_CurrentGameState[idx] != null && GameData.g_CurrentGameState[idx].GetColor() == PColor.White)
				{
					// if it's the right color add it's attacking squares to the list
					AttackedSquaresWhite.AddRange(GameData.g_CurrentGameState[idx].GetAttackingSquares());
				}

				if (GameData.g_CurrentGameState[idx] != null && GameData.g_CurrentGameState[idx].GetColor() == PColor.Black)
				{
					// if it's the right color add it's attacking squares to the list
					AttackedSquaresBlack.AddRange(GameData.g_CurrentGameState[idx].GetAttackingSquares());
				}
			}

			// remove duplicates from the list
			AttackedSquaresWhite = Etc.RemoveDuplicatesFromList(AttackedSquaresWhite);
			AttackedSquaresBlack = Etc.RemoveDuplicatesFromList(AttackedSquaresBlack);

			// update global list
			GameData.g_SquaresAttackedByWhite.Clear();
			GameData.g_SquaresAttackedByWhite.AddRange(AttackedSquaresWhite);

			GameData.g_SquaresAttackedByBlack.Clear();
			GameData.g_SquaresAttackedByBlack.AddRange(AttackedSquaresBlack);
		}

        // updates the global valid move list
		protected void UpdateValidMoves()
		{
			// update all valid moves
			List<int> ValidMovesWhite = new List<int>();
			List<int> ValidMovesBlack = new List<int>();

			// iterate through all pieces of the right color
			for (int idx = 0; idx < GameData.g_CurrentGameState.Length; idx++)
			{
				if (GameData.g_CurrentGameState[idx] != null && GameData.g_CurrentGameState[idx].GetColor() == PColor.White)
				{
					// if it's the right color add it's attacking squares to the list
					ValidMovesWhite.AddRange(GameData.g_CurrentGameState[idx].GetValidMoves());
				}

				if (GameData.g_CurrentGameState[idx] != null && GameData.g_CurrentGameState[idx].GetColor() == PColor.Black)
				{
					// if it's the right color add it's attacking squares to the list
					ValidMovesBlack.AddRange(GameData.g_CurrentGameState[idx].GetValidMoves());
				}
			}

			// remove duplicates from the list
			ValidMovesWhite = Etc.RemoveDuplicatesFromList(ValidMovesWhite);
			ValidMovesBlack = Etc.RemoveDuplicatesFromList(ValidMovesBlack);

			// update global list
			GameData.g_ValidMovesWhite.Clear();
			GameData.g_ValidMovesWhite.AddRange(ValidMovesWhite);

			GameData.g_ValidMovesBlack.Clear();
			GameData.g_ValidMovesBlack.AddRange(ValidMovesBlack);
		}

        // updates castling opportunities
        protected void UpdateCastling()
        {
            // update the kings again because pieces might have been updated in the wrong order
            for (int idx = 0; idx < GameData.g_CurrentGameState.Length; idx++)
            {
                if (GameData.g_CurrentGameState[idx] != null && (GameData.g_CurrentGameState[idx].GetPieceType() == PType.WhiteKing || GameData.g_CurrentGameState[idx].GetPieceType() == PType.BlackKing))
                    GameData.g_CurrentGameState[idx].Update();
            }

            // first the white king, long castle
            if (GameData.g_CurrentGameState[4] != null &&
                GameData.g_CurrentGameState[0] != null &&

                GameData.g_CurrentGameState[4].GetPieceType() == PType.WhiteKing &&
                GameData.g_CurrentGameState[0].GetPieceType() == PType.WhiteRook &&
                
                !GameData.g_CurrentGameState[4].HasMoved() &&
                !GameData.g_CurrentGameState[0].HasMoved())
            {
                if (GameData.g_CurrentGameState[3] == null &&
                    !GameData.g_SquaresAttackedByBlack.Contains(3) &&

                    GameData.g_CurrentGameState[2] == null &&
                    !GameData.g_SquaresAttackedByBlack.Contains(2) &&

                    !GameData.g_SquaresAttackedByBlack.Contains(4) &&
                    !GameData.g_SquaresAttackedByBlack.Contains(0)
                    )
                {
                    GameData.g_CurrentGameState[4].AddValidMove(2);
                }
            }

            // now check the white king, short castle
            if (GameData.g_CurrentGameState[4] != null &&
                GameData.g_CurrentGameState[7] != null &&

                GameData.g_CurrentGameState[4].GetPieceType() == PType.WhiteKing &&
                GameData.g_CurrentGameState[7].GetPieceType() == PType.WhiteRook &&

                !GameData.g_CurrentGameState[4].HasMoved() &&
                !GameData.g_CurrentGameState[7].HasMoved())
            {
                if (GameData.g_CurrentGameState[5] == null &&
                    !GameData.g_SquaresAttackedByBlack.Contains(5) &&

                    GameData.g_CurrentGameState[6] == null &&
                    !GameData.g_SquaresAttackedByBlack.Contains(6) &&

                    !GameData.g_SquaresAttackedByBlack.Contains(4) &&
                    !GameData.g_SquaresAttackedByBlack.Contains(7)
                    )
                {
                    GameData.g_CurrentGameState[4].AddValidMove(6);
                }
            }

            // now the black king, long castle
            if (GameData.g_CurrentGameState[60] != null &&
                GameData.g_CurrentGameState[56] != null &&

                GameData.g_CurrentGameState[60].GetPieceType() == PType.BlackKing &&
                GameData.g_CurrentGameState[56].GetPieceType() == PType.BlackRook &&

                !GameData.g_CurrentGameState[60].HasMoved() &&
                !GameData.g_CurrentGameState[56].HasMoved())
            {
                if (GameData.g_CurrentGameState[59] == null &&
                    !GameData.g_SquaresAttackedByWhite.Contains(59) &&

                    GameData.g_CurrentGameState[58] == null &&
                    !GameData.g_SquaresAttackedByWhite.Contains(58) &&


                    !GameData.g_SquaresAttackedByWhite.Contains(60) &&
                    !GameData.g_SquaresAttackedByWhite.Contains(56)
                    )
                {
                    GameData.g_CurrentGameState[60].AddValidMove(58);
                }
            }

            // now check the black king, short castle
            if (GameData.g_CurrentGameState[60] != null &&
                GameData.g_CurrentGameState[63] != null &&

                GameData.g_CurrentGameState[60].GetPieceType() == PType.BlackKing &&
                GameData.g_CurrentGameState[63].GetPieceType() == PType.BlackRook &&

                !GameData.g_CurrentGameState[60].HasMoved() &&
                !GameData.g_CurrentGameState[63].HasMoved())
            {
                if (GameData.g_CurrentGameState[61] == null &&
                    !GameData.g_SquaresAttackedByWhite.Contains(61) &&

                    GameData.g_CurrentGameState[62] == null &&
                    !GameData.g_SquaresAttackedByWhite.Contains(62) &&

                    !GameData.g_SquaresAttackedByWhite.Contains(60) &&
                    !GameData.g_SquaresAttackedByWhite.Contains(63)
                    )
                {
                    GameData.g_CurrentGameState[60].AddValidMove(62);
                }
            }
        }

        // sees if we can castle
        protected bool TryCastling(int CurrentSquare, int TargetSquare)
        {
            Pieces.Piece CurrentPiece = (Pieces.Piece)GameData.g_CurrentGameState[CurrentSquare];

            if (CurrentPiece != null)
            {
                int SquareDifference = TargetSquare - CurrentSquare;
                if (CurrentPiece.GetPieceType() == PType.WhiteKing)
                {
                    // white, long castle
                    if (SquareDifference == -2 && CurrentPiece.CanMoveTo(TargetSquare))
                    {
                        // do move
                        GameData.g_CurrentGameState[3] = GameData.g_CurrentGameState[0];
                        GameData.g_CurrentGameState[3].SetPosition(3);
                        GameData.g_CurrentGameState[0] = null;

                        GameData.g_CurrentGameState[2] = GameData.g_CurrentGameState[4];
                        GameData.g_CurrentGameState[4].SetPosition(4);
                        GameData.g_CurrentGameState[4] = null;
                        return true;
                    }
                    // white, short castle
                    else if (SquareDifference == 2 && CurrentPiece.CanMoveTo(TargetSquare))
                    {
                        // do move
                        GameData.g_CurrentGameState[6] = GameData.g_CurrentGameState[4];
                        GameData.g_CurrentGameState[4].SetPosition(4);
                        GameData.g_CurrentGameState[4] = null;

                        GameData.g_CurrentGameState[5] = GameData.g_CurrentGameState[7];
                        GameData.g_CurrentGameState[5].SetPosition(5);
                        GameData.g_CurrentGameState[7] = null;
                        return true;
                    }
                }
                else if (CurrentPiece.GetPieceType() == PType.BlackKing)
                {
                    // black, short castle 
                    if (SquareDifference == 2 && CurrentPiece.CanMoveTo(TargetSquare))
                    {
                        // do move
                        GameData.g_CurrentGameState[61] = GameData.g_CurrentGameState[63];
                        GameData.g_CurrentGameState[61].SetPosition(61);
                        GameData.g_CurrentGameState[63] = null;

                        GameData.g_CurrentGameState[62] = GameData.g_CurrentGameState[60];
                        GameData.g_CurrentGameState[62].SetPosition(62);
                        GameData.g_CurrentGameState[60] = null;
                        return true;
                    }
                    // black long castle
                    else if (SquareDifference == -2 && CurrentPiece.CanMoveTo(TargetSquare))
                    {
                        // do move
                        GameData.g_CurrentGameState[58] = GameData.g_CurrentGameState[60];
                        GameData.g_CurrentGameState[58].SetPosition(58);
                        GameData.g_CurrentGameState[60] = null;

                        GameData.g_CurrentGameState[59] = GameData.g_CurrentGameState[56];
                        GameData.g_CurrentGameState[59].SetPosition(59);
                        GameData.g_CurrentGameState[56] = null;
                        return true;
                    }
                }
            }

            return false;
        }

        // sees if we need to do enpassant
        protected bool TryEnPassant(int CurrentSquare, int TargetSquare)
        {
            Pieces.Piece CurrentPiece = (Pieces.Piece)GameData.g_CurrentGameState[CurrentSquare];

            if (CurrentPiece != null)
            {
                if (CurrentPiece.GetEnPassantMoves().Contains(TargetSquare))
                {
                    // do move
                    if (CurrentPiece.GetColor() == PColor.White)
                    {
                        GameData.g_CurrentGameState[TargetSquare-8] = null;
                        GameData.g_CurrentGameState[CurrentSquare] = null;
                        CurrentPiece.SetPosition(TargetSquare);
                        GameData.g_CurrentGameState[TargetSquare] = CurrentPiece;
                        return true;
                    }
                    // do move
                    else if (CurrentPiece.GetColor() == PColor.Black)
                    {
                        GameData.g_CurrentGameState[TargetSquare + 8] = null;
                        GameData.g_CurrentGameState[CurrentSquare] = null;
                        CurrentPiece.SetPosition(TargetSquare);
                        GameData.g_CurrentGameState[TargetSquare] = CurrentPiece;
                        return true;
                    }
                }
            }

            return false;
        }

        protected bool IsChecked(PColor colorToCheck)
        {
            int indexOfKing = 0;

            // find king piece
            for (; indexOfKing < GameData.g_CurrentGameState.Length; indexOfKing++)
            {
                if (GameData.g_CurrentGameState[indexOfKing] != null)
                {
                    if (colorToCheck == PColor.Black)
                    {
                        if (GameData.g_CurrentGameState[indexOfKing].GetPieceType() == PType.BlackKing)
                            break;
                    }
                    else if (colorToCheck == PColor.White)
                    {
                        if (GameData.g_CurrentGameState[indexOfKing].GetPieceType() == PType.WhiteKing)
                            break;
                    }
                }
            }

            // see if it's being attacked
            if (colorToCheck == PColor.Black)
            {
                if (GameData.g_SquaresAttackedByWhite.Contains(indexOfKing))
                    return true;
            }
            else if (colorToCheck == PColor.White)
            {
                if (GameData.g_SquaresAttackedByBlack.Contains(indexOfKing))
                    return true;
            }

            return false;
        }

		// reset internal board to the current game state
		protected void ResetBoard()
		{
			Pieces.Piece CurrentPiece = null;
			
			// initialize our board
            for (int Position = 0; Position < GameData.g_StartingPositions.Length; Position++)
			{
				// create appropriate class
                switch ((PType)GameData.g_StartingPositions[Position])
				{
					case PType.BlackBishop:
						{
							CurrentPiece = new Pieces.Bishop(PColor.Black);
							CurrentPiece.SetPosition(Position);
							break;
						}
					case PType.BlackKing:
						{
							CurrentPiece = new Pieces.King(PColor.Black);
							CurrentPiece.SetPosition(Position);
							break;
						}
					case PType.BlackKnight:
						{
							CurrentPiece = new Pieces.Knight(PColor.Black);
							CurrentPiece.SetPosition(Position);
							break;
						}
					case PType.BlackPawn:
						{
							CurrentPiece = new Pieces.Pawn(PColor.Black);
							CurrentPiece.SetPosition(Position);
							break;
						}
					case PType.BlackQueen:
						{
							CurrentPiece = new Pieces.Queen(PColor.Black);
							CurrentPiece.SetPosition(Position);
							break;
						}
					case PType.BlackRook:
						{
							CurrentPiece = new Pieces.Rook(PColor.Black);
							CurrentPiece.SetPosition(Position);
							break;
						}

					case PType.WhiteBishop:
						{
							CurrentPiece = new Pieces.Bishop(PColor.White);
							CurrentPiece.SetPosition(Position);
							break;
						}
					case PType.WhiteKing:
						{
							CurrentPiece = new Pieces.King(PColor.White);
							CurrentPiece.SetPosition(Position);
							break;
						}
					case PType.WhiteKnight:
						{
							CurrentPiece = new Pieces.Knight(PColor.White);
							CurrentPiece.SetPosition(Position);
							break;
						}
					case PType.WhitePawn:
						{
							CurrentPiece = new Pieces.Pawn(PColor.White);
							CurrentPiece.SetPosition(Position);
							break;
						}
					case PType.WhiteQueen:
						{
							CurrentPiece = new Pieces.Queen(PColor.White);
							CurrentPiece.SetPosition(Position);
							break;
						}
					case PType.WhiteRook:
						{
							CurrentPiece = new Pieces.Rook(PColor.White);
							CurrentPiece.SetPosition(Position);
							break;
						}
					default:
						{
							CurrentPiece = null;
							break;
						}
				}

				// add piece to board
				GameData.g_CurrentGameState[Position] = CurrentPiece;
			}
		}

		#endregion
        
        //////////////////////////////////////////////////////////////////////////
        // Helpers
        //////////////////////////////////////////////////////////////////////////
        #region Helpers

        #endregion

        //////////////////////////////////////////////////////////////////////////
        // The rules
        //////////////////////////////////////////////////////////////////////////
        #region TheRules

        #endregion
    }
}
