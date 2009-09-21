using System;
using System.Collections.Generic;
using System.Text;
using Globals;

namespace ChessLogic.Pieces
{
    class Piece : Globals.PrototypePiece
    {
        //////////////////////////////////////////////////////////////////////////
        // Members
        //////////////////////////////////////////////////////////////////////////
        #region Members
        #endregion

        //////////////////////////////////////////////////////////////////////////
        // Public Methods
        //////////////////////////////////////////////////////////////////////////
        #region Public Methods

        // constructor
        public Piece()
        {
            // initialize variables
            m_Color = PColor.None;
            m_Value = PValue.None;
            m_Type = PType.None;
            m_nPosition = -1;
            m_nPrevPosition = -1;
            m_lAttackingSquares = new List<int>();
            m_lValidMoves = new List<int>();
            bIsEnPassantCandidate = false;
        }

        // gets the piece type
		public override PType GetPieceType()
        {
            return m_Type;
        }

        // gets current position
        public override int GetPosition()
        {
            return m_nPosition;
        }

        // gets current position
        public override PColor GetColor()
        {
            return m_Color;
        }

        // sets current position
		public override void SetPosition(int NewPosition)
        {
            // store previous position
            m_nPrevPosition = m_nPosition;

            // set new position
            m_nPosition = NewPosition;

            // updates attacking squares after moving
            UpdateAttackingSquares();

            // update the squares this piece could potentially move to
            UpdateValidMoves();
        }

		// updates all lists
		public override void Update()
		{
			// updates attacking squares after moving
			UpdateAttackingSquares();

			// update the squares this piece could potentially move to
			UpdateValidMoves();
		}

        // gets the squares this piece is attacking
		public override List<int> GetAttackingSquares()
        {
            return m_lAttackingSquares;
        }

        // gets the squares this piece can move to
		public override List<int> GetValidMoves()
        {
            return m_lValidMoves;
        }

        // returns true if this piece can move to the destination square
		public override bool CanMoveTo(int Destination)
        {
            return m_lValidMoves.Contains(Destination);
        }

        #endregion

        //////////////////////////////////////////////////////////////////////////
        // Helpers
        //////////////////////////////////////////////////////////////////////////
        #region Helpers

		// updates the squares this piece is attacking
		protected override void UpdateAttackingSquares() { }

		// updates the squares this piece could potentially move to
		protected override void UpdateValidMoves() { }

        // returns all diagonal moves a piece can make
		protected override List<int> CalculateDiagonalMoves(int MaxRange)
        {
            // we can use our current row and column number to make sure we only move inside the board
            // and can't cheat wraparound
            int Row = 0;
            int Col = 0;
            int CurrentSquare = -1;
            int CurrentRange = 0;
            List<int> PotentialMoves = new List<int>();

            // if Maxrange = 0, then Maxrange is infinite
            if (MaxRange == 0)
                MaxRange = int.MaxValue;

            // iterate through all possible move and add them to a prevalidated moves list
            // moving NW
            Etc.GetRowColFromSquare(m_nPosition, out Row, out Col);
            do
            {
                Row++;
                Col--;

                if (Etc.GetSquareFromRowCol(Row, Col, out CurrentSquare))
                {
                    // calculate range from current position
                    CurrentRange = Math.Abs((CurrentSquare - m_nPosition) - 8);

                    // add this square
                    PotentialMoves.Add(CurrentSquare);

                    // do not pursue this line further when another piece is in the way
                    if (GameData.g_CurrentGameState[CurrentSquare] != null || CurrentRange >= MaxRange)
                        break;
                }

            } while (Col >= 0 && Col <= 7 && Row >= 0 && Row <= 7);

            // moving NE
            Etc.GetRowColFromSquare(m_nPosition, out Row, out Col);
            do
            {
                Row++;
                Col++;

                if (Etc.GetSquareFromRowCol(Row, Col, out CurrentSquare))
                {
                    // calculate range from current position
                    CurrentRange = Math.Abs((CurrentSquare - m_nPosition) - 8);

                    // add this square
                    PotentialMoves.Add(CurrentSquare);

                    // do not pursue this line further when another piece is in the way
                    if (GameData.g_CurrentGameState[CurrentSquare] != null || CurrentRange >= MaxRange)
                        break;
                }

            } while (Col >= 0 && Col <= 7 && Row >= 0 && Row <= 7);

            // moving SW
            Etc.GetRowColFromSquare(m_nPosition, out Row, out Col);
            do
            {
                Row--;
                Col--;

                if (Etc.GetSquareFromRowCol(Row, Col, out CurrentSquare))
                {
                    // calculate range from current position
                    CurrentRange = Math.Abs((CurrentSquare - m_nPosition) - 8);

                    // add this square
                    PotentialMoves.Add(CurrentSquare);

                    // do not pursue this line further when another piece is in the way
                    if (GameData.g_CurrentGameState[CurrentSquare] != null || CurrentRange >= MaxRange)
                        break;
                }

            } while (Col >= 0 && Col <= 7 && Row >= 0 && Row <= 7);

            // moving SE
            Etc.GetRowColFromSquare(m_nPosition, out Row, out Col);
            do
            {
                Row--;
                Col++;

                if (Etc.GetSquareFromRowCol(Row, Col, out CurrentSquare))
                {
                    // calculate range from current position
                    CurrentRange = Math.Abs((CurrentSquare - m_nPosition) - 8);

                    // add this square
                    PotentialMoves.Add(CurrentSquare);

                    // do not pursue this line further when another piece is in the way
                    if (GameData.g_CurrentGameState[CurrentSquare] != null || CurrentRange >= MaxRange)
                        break;
                }

            } while (Col >= 0 && Col <= 7 && Row >= 0 && Row <= 7);

            // return our list
            return PotentialMoves;
        }

        // returns all diagonal moves a piece can make
		protected override List<int> CalculateHorizontalMoves(int MaxRange)
        {
            // we can use our current row and column number to make sure we only move inside the board
            // and can't cheat wraparound
            int Row = 0;
            int Col = 0;
            int CurrentSquare = -1;
            int CurrentRange = 0;
            List<int> PotentialMoves = new List<int>();

            // if Maxrange = 0, then Maxrange is infinite
            if (MaxRange == 0)
                MaxRange = int.MaxValue;

            // iterate through all possible move and add them to a prevalidated moves list
            // moving W
            Etc.GetRowColFromSquare(m_nPosition, out Row, out Col);
            do
            {
                Col--;

                if (Etc.GetSquareFromRowCol(Row, Col, out CurrentSquare))
                {
                    // calculate range from current position
                    CurrentRange = Math.Abs(CurrentSquare - m_nPosition);

                    // add this square
                    PotentialMoves.Add(CurrentSquare);

                    // do not pursue this line further when another piece is in the way
                    if (GameData.g_CurrentGameState[CurrentSquare] != null || CurrentRange >= MaxRange)
                        break;
                }

            } while (Col >= 0 && Col <= 7 && Row >= 0 && Row <= 7);

            // moving E
            Etc.GetRowColFromSquare(m_nPosition, out Row, out Col);
            do
            {
                Col++;

                if (Etc.GetSquareFromRowCol(Row, Col, out CurrentSquare))
                {
                    // calculate range from current position
                    CurrentRange = Math.Abs(CurrentSquare - m_nPosition);

                    // add this square
                    PotentialMoves.Add(CurrentSquare);

                    // do not pursue this line further when another piece is in the way
                    if (GameData.g_CurrentGameState[CurrentSquare] != null || CurrentRange >= MaxRange)
                        break;
                }

            } while (Col >= 0 && Col <= 7 && Row >= 0 && Row <= 7);

            // return our list
            return PotentialMoves;
        }

        // returns all diagonal moves a piece can make
		protected override List<int> CalculateVerticalMoves(int MaxRange)
        {
            // we can use our current row and column number to make sure we only move inside the board
            // and can't cheat wraparound
            int Row = 0;
            int Col = 0;
            int CurrentSquare = -1;
            int CurrentRange = 0;
            List<int> PotentialMoves = new List<int>();

            // if Maxrange = 0, then Maxrange is infinite
            if (MaxRange == 0)
                MaxRange = int.MaxValue;

            // iterate through all possible move and add them to a prevalidated moves list
            // moving W
            Etc.GetRowColFromSquare(m_nPosition, out Row, out Col);
            do
            {
                Row--;

                if (Etc.GetSquareFromRowCol(Row, Col, out CurrentSquare))
                {
                    // calculate range from current position
                    CurrentRange = Math.Abs((CurrentSquare - m_nPosition) - 7);

                    // add this square
                    PotentialMoves.Add(CurrentSquare);

                    // do not pursue this line further when another piece is in the way
                    if (GameData.g_CurrentGameState[CurrentSquare] != null || CurrentRange >= MaxRange)
                        break;
                }

            } while (Col >= 0 && Col <= 7 && Row >= 0 && Row <= 7);

            // moving E
            Etc.GetRowColFromSquare(m_nPosition, out Row, out Col);
            do
            {
                Row++;

                if (Etc.GetSquareFromRowCol(Row, Col, out CurrentSquare))
                {
                    // calculate range from current position
                    CurrentRange = Math.Abs((CurrentSquare - m_nPosition) - 7);

                    // add this square
                    PotentialMoves.Add(CurrentSquare);

                    // do not pursue this line further when another piece is in the way
                    if (GameData.g_CurrentGameState[CurrentSquare] != null || CurrentRange >= MaxRange)
                        break;
                }

            } while (Col >= 0 && Col <= 7 && Row >= 0 && Row <= 7);

            // return our list
            return PotentialMoves;
        }

        // calculates all valid moves
		protected override List<int> ValidateMoves(List<int> PreValidatedMoves)
        {
            // locals
            List<int> ValidMoves = new List<int>();
            int CurrentSquare = -1;

            // validate moves
            for (int i = 0; i < PreValidatedMoves.Count; i++)
            {
                // get square from list
                CurrentSquare = PreValidatedMoves[i];

                // if square is unoccupied we can move there
                if (GameData.g_CurrentGameState[CurrentSquare] == null)
                    ValidMoves.Add(CurrentSquare);
                else
                {
                    // if square is occupied by enemy, we can also still move there
                    if (m_Color == PColor.White && GameData.g_CurrentGameState[CurrentSquare] != null && GameData.g_CurrentGameState[CurrentSquare].GetColor() == PColor.Black)
                    {
                        ValidMoves.Add(CurrentSquare);
                    }

                    if (m_Color == PColor.Black && GameData.g_CurrentGameState[CurrentSquare] != null && GameData.g_CurrentGameState[CurrentSquare].GetColor() == PColor.White)
                    {
                        ValidMoves.Add(CurrentSquare);
                    }
                }
            }

            return ValidMoves;
        }

        #endregion

    }
}
