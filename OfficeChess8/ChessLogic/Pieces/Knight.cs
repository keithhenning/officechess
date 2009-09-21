using System;
using System.Collections.Generic;
using System.Text;
using Globals;

namespace ChessLogic.Pieces
{
	[Serializable]
    class Knight : Piece
    {
        //////////////////////////////////////////////////////////////////////////
        // Public Methods
        //////////////////////////////////////////////////////////////////////////
        #region Public Methods

        // constructor
        public Knight(PColor PieceColor)
        {
            m_Color = PieceColor;
            m_Value = PValue.Knight;
            m_Type = (m_Color == PColor.White) ? PType.WhiteKnight : PType.BlackKnight;
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
            List<int> AttackingSquares = new List<int>();

            // clear our list
            m_lAttackingSquares.Clear();

            // get all the moves this knight can do
            AttackingSquares = CalculateKnightMoves();

            // finally add the attacked squares to our member list
            m_lAttackingSquares.AddRange(AttackingSquares);
        }

        // updates the squares this piece could potentially move to
        protected override void UpdateValidMoves()
        {
            // initialize
            List<int> PreValidatedMoves = new List<int>();
            List<int> ValidMoves = new List<int>();

            // clear our list
            m_lValidMoves.Clear();

            // get all the moves this knight can do
            PreValidatedMoves = CalculateKnightMoves();

            // validate moves
            ValidMoves = ValidateMoves(PreValidatedMoves);

            // finally add the attacked squares to our member list
            m_lValidMoves.AddRange(ValidMoves);
        }

        // returns all potential moves regardless of other pieces
        private List<int> CalculateKnightMoves()
        {
            List<int> PotentialMoves = new List<int>();

            int Row = 0;
            int Col = 0;
            int CurrentSquare = -1;

            Etc.GetRowColFromSquare(m_nPosition, out Row, out Col);
            Row += 2;
            Col += 1;
            if (Etc.GetSquareFromRowCol(Row, Col, out CurrentSquare))
                PotentialMoves.Add(CurrentSquare);

            Etc.GetRowColFromSquare(m_nPosition, out Row, out Col);
            Row += 2;
            Col -= 1;
            if (Etc.GetSquareFromRowCol(Row, Col, out CurrentSquare))
                PotentialMoves.Add(CurrentSquare);

            Etc.GetRowColFromSquare(m_nPosition, out Row, out Col);
            Row += 1;
            Col += 2;
            if (Etc.GetSquareFromRowCol(Row, Col, out CurrentSquare))
                PotentialMoves.Add(CurrentSquare);

            Etc.GetRowColFromSquare(m_nPosition, out Row, out Col);
            Row += 1;
            Col -= 2;
            if (Etc.GetSquareFromRowCol(Row, Col, out CurrentSquare))
                PotentialMoves.Add(CurrentSquare);

            Etc.GetRowColFromSquare(m_nPosition, out Row, out Col);
            Row -= 2;
            Col += 1;
            if (Etc.GetSquareFromRowCol(Row, Col, out CurrentSquare))
                PotentialMoves.Add(CurrentSquare);

            Etc.GetRowColFromSquare(m_nPosition, out Row, out Col);
            Row -= 2;
            Col -= 1;
            if (Etc.GetSquareFromRowCol(Row, Col, out CurrentSquare))
                PotentialMoves.Add(CurrentSquare);

            Etc.GetRowColFromSquare(m_nPosition, out Row, out Col);
            Row -= 1;
            Col += 2;
            if (Etc.GetSquareFromRowCol(Row, Col, out CurrentSquare))
                PotentialMoves.Add(CurrentSquare);

            Etc.GetRowColFromSquare(m_nPosition, out Row, out Col);
            Row -= 1;
            Col -= 2;
            if (Etc.GetSquareFromRowCol(Row, Col, out CurrentSquare))
                PotentialMoves.Add(CurrentSquare);

            return PotentialMoves;
        }

        #endregion
    }
}
