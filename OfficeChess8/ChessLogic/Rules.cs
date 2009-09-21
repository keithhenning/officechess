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
			ResetBoard();
            Update();
        }

        // evaluates the move
        private bool IsRegularMoveAllowed(int CurrentSquare, int TargetSquare)
        {
            bool bCanMove = false;

            // checks to see if the piece on the current square can move to the destination square
			if (GameData.g_CurrentGameState[CurrentSquare] != null)
				bCanMove = GameData.g_CurrentGameState[CurrentSquare].CanMoveTo(TargetSquare);

            return bCanMove;
        }

        // does the actual move and updates all necessary information
        public bool DoMove(int CurrentSquare, int TargetSquare)
        {
            bool bMoveAllowed = false;

            // checks to see if the piece on the current square can move to the destination square
            bMoveAllowed = IsRegularMoveAllowed(CurrentSquare, TargetSquare);

            // do the actual move
            if (bMoveAllowed)
            {
				// set internal board
                if (GameData.g_CurrentGameState[CurrentSquare] != null)
                {
                    // update the game state
                    GameData.g_CurrentGameState[CurrentSquare].SetPosition(TargetSquare);
                    GameData.g_CurrentGameState[TargetSquare] = GameData.g_CurrentGameState[CurrentSquare];
                    GameData.g_CurrentGameState[CurrentSquare] = null;

                    // updtae the pieces
                    Update();

                    // inc num moves
                    m_nNumMoves++;

					// store current color that should be playing
					if (m_nNumMoves % 2 == 0)
						GameData.ColorMoving = PColor.White;
					else
						GameData.ColorMoving = PColor.Black;
                }
                else
                {
                    Console.WriteLine("WARNING: trying to move piece with NULL pointer, something is very wrong here!");
                }
            }

			return bMoveAllowed;
        }

        // gets all squares that a piece from <PieceColor> can move to
        public List<int> GetValidMovesFor(PColor PieceColor)
        {
            List<int> ValidMoves = new List<int>();

            // iterate through all pieces of the right color
            for (int idx = 0; idx < GameData.g_CurrentGameState.Length; idx++)
            {
                if (GameData.g_CurrentGameState[idx] != null && GameData.g_CurrentGameState[idx].GetColor() == PieceColor)
                {
                    // if it's the right color add it's valid moves to the list
                    ValidMoves.AddRange(GameData.g_CurrentGameState[idx].GetValidMoves());
                }
            }

            // remove duplicates from the list
            ValidMoves = Etc.RemoveDuplicatesFromList(ValidMoves);

            return ValidMoves;
		}
		#endregion

		//////////////////////////////////////////////////////////////////////////
		// Public/protected methods
		//////////////////////////////////////////////////////////////////////////
		#region Private / protected methods

        // iterates through all pieces and lets them update
        protected void Update()
        {
			UpdatePieces();
			UpdateAttackedSquares();
			UpdateValidMoves();
        }

		// update all moves and attacked squares for all pieces
		protected void UpdatePieces()
		{
			for (int idx = 0; idx < GameData.g_CurrentGameState.Length; idx++)
			{
				if (GameData.g_CurrentGameState[idx] != null)
					GameData.g_CurrentGameState[idx].Update();
			}
		}

		// upates global list of attacked squares
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
