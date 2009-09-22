using System;
using System.Collections.Generic;
using System.Text;
using Globals;

namespace ChessLogic.Pieces
{
	[Serializable]
    class Pawn : Piece
    {
        //////////////////////////////////////////////////////////////////////////
        // Public Methods
        //////////////////////////////////////////////////////////////////////////
        #region Public Methods

        // constructor
        public Pawn(PColor PieceColor)
        {
            m_Color = PieceColor;
            m_Value = PValue.Pawn;
            m_Type = (m_Color == PColor.White) ? PType.WhitePawn : PType.BlackPawn;
        }

        // override setposition to handle enpassant
        public override void SetPosition(int Position)
        {
            // call the base
            base.SetPosition(Position);

            // check to see if this pawn can enpassant-ed
            if (Math.Abs(m_nPosition - m_nPrevPosition) == 16)
                bIsEnPassantCandidate = true;
            else
                bIsEnPassantCandidate = false;
        }

        #endregion

        //////////////////////////////////////////////////////////////////////////
        // Helpers
        //////////////////////////////////////////////////////////////////////////
        #region Helpers

        // updates the squares this piece is attacking
        protected override void UpdateAttackingSquares()
        {
            // initialize
            List<int> AttackedSquares = new List<int>();

            // clear our list
            m_lAttackingSquares.Clear();

            // we can use our current row and column number to make sure we only move inside the board
            // and can't cheat wraparound
            int Row = 0;
            int Col = 0;
			int CurrentSquare = -1;

            // gets row and column number from current square
            Etc.GetRowColFromSquare(m_nPosition, out Row, out Col);

            // update when white
            if (m_Color == PColor.White)
            {
                #region NORMAL_TAKE
                // gets row and column number from current square
				Etc.GetRowColFromSquare(m_nPosition, out Row, out Col);

				// look to the NW to see if we can capture any pieces
				Row++;
				Col--;

				Etc.GetSquareFromRowCol(Row, Col, out CurrentSquare);

				if (CurrentSquare >= 0)
				{
					AttackedSquares.Add(CurrentSquare);
				}

				// gets row and column number from current square
				Etc.GetRowColFromSquare(m_nPosition, out Row, out Col);

				// look to the NW to see if we can capture any pieces
				Row++;
				Col++;

				Etc.GetSquareFromRowCol(Row, Col, out CurrentSquare);

				if (CurrentSquare >= 0)
				{
					AttackedSquares.Add(CurrentSquare);
                }
                #endregion

                #region ENPASSANT
                // gets row and column number from current square
                Etc.GetRowColFromSquare(m_nPosition, out Row, out Col);

                // look to the NW to see if enpassant any pieces
                Col++;

                Etc.GetSquareFromRowCol(Row, Col, out CurrentSquare);

                if (CurrentSquare >= 0 && GameData.g_CurrentGameState[CurrentSquare] != null && GameData.g_CurrentGameState[CurrentSquare].GetPieceType() == PType.BlackPawn && GameData.g_CurrentGameState[CurrentSquare].GetEnPassantStatus() ==  true)
                {
                    AttackedSquares.Add(CurrentSquare);
                }

                // gets row and column number from current square
                Etc.GetRowColFromSquare(m_nPosition, out Row, out Col);

                // look to the NE to see if enpassant any pieces
                Col--;

                Etc.GetSquareFromRowCol(Row, Col, out CurrentSquare);

                if (CurrentSquare >= 0 && GameData.g_CurrentGameState[CurrentSquare] != null && GameData.g_CurrentGameState[CurrentSquare].GetPieceType() == PType.BlackPawn && GameData.g_CurrentGameState[CurrentSquare].GetEnPassantStatus() == true)
                {
                    AttackedSquares.Add(CurrentSquare);
                }
                #endregion
            }

            // update when black
            if (m_Color == PColor.Black)
            {
                #region NORMAL_TAKE
                // gets row and column number from current square
				Etc.GetRowColFromSquare(m_nPosition, out Row, out Col);

				// look to the SW to see if we can capture any pieces
				Row--;
				Col--;

				Etc.GetSquareFromRowCol(Row, Col, out CurrentSquare);

				if (CurrentSquare >= 0)
				{
					AttackedSquares.Add(CurrentSquare);
				}

				// gets row and column number from current square
				Etc.GetRowColFromSquare(m_nPosition, out Row, out Col);

				// look to the SE to see if we can capture any pieces
				Row--;
				Col++;

				Etc.GetSquareFromRowCol(Row, Col, out CurrentSquare);

				if (CurrentSquare >= 0)
				{
					AttackedSquares.Add(CurrentSquare);
                }
                #endregion

                #region ENPASSANT
                // gets row and column number from current square
                Etc.GetRowColFromSquare(m_nPosition, out Row, out Col);

                // look to the SW to see if enpassant any pieces
                Col++;

                Etc.GetSquareFromRowCol(Row, Col, out CurrentSquare);

                if (CurrentSquare >= 0 && GameData.g_CurrentGameState[CurrentSquare] != null && GameData.g_CurrentGameState[CurrentSquare].GetPieceType() == PType.WhitePawn && GameData.g_CurrentGameState[CurrentSquare].GetEnPassantStatus() == true)
                {
                    AttackedSquares.Add(CurrentSquare);
                }

                // gets row and column number from current square
                Etc.GetRowColFromSquare(m_nPosition, out Row, out Col);

                // look to the SE to see if enpassant any pieces
                Col--;

                Etc.GetSquareFromRowCol(Row, Col, out CurrentSquare);

                if (CurrentSquare >= 0 && GameData.g_CurrentGameState[CurrentSquare] != null && GameData.g_CurrentGameState[CurrentSquare].GetPieceType() == PType.WhitePawn && GameData.g_CurrentGameState[CurrentSquare].GetEnPassantStatus() == true)
                {
                    AttackedSquares.Add(CurrentSquare);
                }
                #endregion
            }

            // add these square to the list
            m_lAttackingSquares.AddRange(AttackedSquares);
        }

        // updates the squares this piece could potentially move to
        protected override void UpdateValidMoves()
        {
            // initialize
            List<int> ValidMoves = new List<int>();
            
            // clear our list
            m_lValidMoves.Clear();

            // we can use our current row and column number to make sure we only move inside the board
            // and can't cheat wraparound
            int Row = 0;
            int Col = 0;
			int CurrentSquare = -1;

            // gets row and column number from current square
            Etc.GetRowColFromSquare(m_nPosition, out Row, out Col);

			// check for white
            if (m_Color == PColor.White)
            {
                #region STRAIGHT_AHEAD
                // move one square ahead
				Row++;
				Etc.GetSquareFromRowCol(Row, Col, out CurrentSquare);

				// add square if not occupied
				if (CurrentSquare >=0 && GameData.g_CurrentGameState[CurrentSquare] == null)
				{
					m_lValidMoves.Add(CurrentSquare);

					// move one square ahead again
					Row++;
					Etc.GetSquareFromRowCol(Row, Col, out CurrentSquare);

					// get row col from current position
					Etc.GetRowColFromSquare(m_nPosition, out Row, out Col);

					// add square if not occupied
					if (Row == 1 && CurrentSquare >= 0 && GameData.g_CurrentGameState[CurrentSquare] == null)
					{
						m_lValidMoves.Add(CurrentSquare);
					}
                }
                #endregion

                #region NORMAL_TAKE
                // gets row and column number from current square
				Etc.GetRowColFromSquare(m_nPosition, out Row, out Col);

				// look to the NW to see if we can capture any pieces
				Row++;
				Col--;

				Etc.GetSquareFromRowCol(Row, Col, out CurrentSquare);

				if (CurrentSquare >= 0 &&
					GameData.g_CurrentGameState[CurrentSquare] != null && 
					GameData.g_CurrentGameState[CurrentSquare].GetColor() == PColor.Black)
				{
					m_lValidMoves.Add(CurrentSquare);
				}

				// gets row and column number from current square
				Etc.GetRowColFromSquare(m_nPosition, out Row, out Col);

				// look to the NW to see if we can capture any pieces
				Row++;
				Col++;

				Etc.GetSquareFromRowCol(Row, Col, out CurrentSquare);

				if (CurrentSquare >= 0 &&
					GameData.g_CurrentGameState[CurrentSquare] != null &&
					GameData.g_CurrentGameState[CurrentSquare].GetColor() == PColor.Black)
				{
					m_lValidMoves.Add(CurrentSquare);
                }
                #endregion

                #region ENPASSANT
                // gets row and column number from current square
                Etc.GetRowColFromSquare(m_nPosition, out Row, out Col);

                // look to the NW to see if we EnPassant any pieces
                Col--;

                Etc.GetSquareFromRowCol(Row, Col, out CurrentSquare);

                if (CurrentSquare >= 0 &&
                    GameData.g_CurrentGameState[CurrentSquare] != null &&
                    GameData.g_CurrentGameState[CurrentSquare].GetColor() == PColor.Black && 
                    GameData.g_CurrentGameState[CurrentSquare].GetEnPassantStatus() == true)
                {
                    Row++;
                    Etc.GetSquareFromRowCol(Row, Col, out CurrentSquare);
                    m_lValidMoves.Add(CurrentSquare);
                }

                // gets row and column number from current square
                Etc.GetRowColFromSquare(m_nPosition, out Row, out Col);

                // look to the NE to see if we EnPassant any pieces
                Col++;

                Etc.GetSquareFromRowCol(Row, Col, out CurrentSquare);

                if (CurrentSquare >= 0 &&
                    GameData.g_CurrentGameState[CurrentSquare] != null &&
                    GameData.g_CurrentGameState[CurrentSquare].GetColor() == PColor.Black && 
                    GameData.g_CurrentGameState[CurrentSquare].GetEnPassantStatus() == true)
                {
                    Row++;
                    Etc.GetSquareFromRowCol(Row, Col, out CurrentSquare);
                    m_lValidMoves.Add(CurrentSquare);
                }
                #endregion
            }

			// check for black
			if (m_Color == PColor.Black)
            {
                #region STRAIGHT_AHEAD
                // move one square ahead
				Row--;
				Etc.GetSquareFromRowCol(Row, Col, out CurrentSquare);

				// add square if not occupied
				if (CurrentSquare >= 0 && GameData.g_CurrentGameState[CurrentSquare] == null)
				{
					m_lValidMoves.Add(CurrentSquare);

					// move one square ahead again
					Row--;
					Etc.GetSquareFromRowCol(Row, Col, out CurrentSquare);

					// get row col from current position
					Etc.GetRowColFromSquare(m_nPosition, out Row, out Col);

					// add square if not occupied
					if (Row == 6 && CurrentSquare >= 0 && GameData.g_CurrentGameState[CurrentSquare] == null)
					{
						m_lValidMoves.Add(CurrentSquare);
					}
                }
                #endregion

                #region NORMAL_TAKE
                // gets row and column number from current square
				Etc.GetRowColFromSquare(m_nPosition, out Row, out Col);

				// look to the NW to see if we can capture any pieces
				Row--;
				Col--;

				Etc.GetSquareFromRowCol(Row, Col, out CurrentSquare);

				if (CurrentSquare >= 0 &&
					GameData.g_CurrentGameState[CurrentSquare] != null &&
					GameData.g_CurrentGameState[CurrentSquare].GetColor() == PColor.White)
				{
					m_lValidMoves.Add(CurrentSquare);
				}

				// gets row and column number from current square
				Etc.GetRowColFromSquare(m_nPosition, out Row, out Col);

				// look to the NW to see if we can capture any pieces
				Row--;
				Col++;

				Etc.GetSquareFromRowCol(Row, Col, out CurrentSquare);

				if (CurrentSquare >= 0 &&
					GameData.g_CurrentGameState[CurrentSquare] != null &&
					GameData.g_CurrentGameState[CurrentSquare].GetColor() == PColor.White)
				{
					m_lValidMoves.Add(CurrentSquare);
                }
                #endregion

                #region ENPASSANT
                // gets row and column number from current square
                Etc.GetRowColFromSquare(m_nPosition, out Row, out Col);

                // look to the SW to see if we EnPassant any pieces
                Col--;

                Etc.GetSquareFromRowCol(Row, Col, out CurrentSquare);

                if (CurrentSquare >= 0 &&
                    GameData.g_CurrentGameState[CurrentSquare] != null &&
                    GameData.g_CurrentGameState[CurrentSquare].GetColor() == PColor.White && 
                    GameData.g_CurrentGameState[CurrentSquare].GetEnPassantStatus() == true)
                {
                    Row--;
                    Etc.GetSquareFromRowCol(Row, Col, out CurrentSquare);
                    m_lValidMoves.Add(CurrentSquare);
                }

                // gets row and column number from current square
                Etc.GetRowColFromSquare(m_nPosition, out Row, out Col);

                // look to the SE to see if we EnPassant any pieces
                Col++;

                Etc.GetSquareFromRowCol(Row, Col, out CurrentSquare);

                if (CurrentSquare >= 0 &&
                    GameData.g_CurrentGameState[CurrentSquare] != null &&
                    GameData.g_CurrentGameState[CurrentSquare].GetColor() == PColor.White && 
                    GameData.g_CurrentGameState[CurrentSquare].GetEnPassantStatus() == true)
                {
                    Row--;
                    Etc.GetSquareFromRowCol(Row, Col, out CurrentSquare);
                    m_lValidMoves.Add(CurrentSquare);
                }
                #endregion
            }

            // if there are moves, add them
            m_lValidMoves.AddRange(ValidMoves);
        }

        #endregion
    }
}
