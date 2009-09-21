using System;
using System.Collections.Generic;
using System.Text;
using Globals;

namespace ChessLogic.Pieces
{
	[Serializable]
    class King : Piece
    {
        //////////////////////////////////////////////////////////////////////////
        // Public Methods
        //////////////////////////////////////////////////////////////////////////
        #region Public Methods

        // constructor
        public King(PColor PieceColor)
        {
            m_Color = PieceColor;
            m_Value = PValue.King;
            m_Type = (m_Color == PColor.White) ? PType.WhiteKing : PType.BlackKing;
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
            List<int> AttackingSquaresDiag = new List<int>();
            List<int> AttackingSquaresHorz = new List<int>();
            List<int> AttackingSquaresVert = new List<int>();
            int CurrentSquare = m_nPosition;

            // clear our list
            m_lAttackingSquares.Clear();

            // get all valid moves
            AttackingSquaresDiag = CalculateDiagonalMoves(1);
            AttackingSquaresHorz = CalculateHorizontalMoves(1);
            AttackingSquaresVert = CalculateVerticalMoves(1);

            // finally add the attacked squares to our member list
            m_lAttackingSquares.AddRange(AttackingSquaresDiag);
            m_lAttackingSquares.AddRange(AttackingSquaresHorz);
            m_lAttackingSquares.AddRange(AttackingSquaresVert);
        }

        // updates the squares this piece could potentially move to
        protected override void UpdateValidMoves()
        {
            // initialize
            List<int> PreValidatedSquaresDiag = new List<int>();
            List<int> PreValidatedSquaresHorz = new List<int>();
            List<int> PreValidatedSquaresVert = new List<int>();
            List<int> PreValidatedMoves = new List<int>();
            List<int> ValidMoves = new List<int>();

            // clear our list
            m_lValidMoves.Clear();

            // get all valid moves
            PreValidatedSquaresDiag = CalculateDiagonalMoves(1);
            PreValidatedSquaresHorz = CalculateHorizontalMoves(1);
            PreValidatedSquaresVert = CalculateVerticalMoves(1);

            // concat all moves
            PreValidatedMoves.AddRange(PreValidatedSquaresDiag);
            PreValidatedMoves.AddRange(PreValidatedSquaresHorz);
            PreValidatedMoves.AddRange(PreValidatedSquaresVert);

            // validate moves
            ValidMoves = ValidateMoves(PreValidatedMoves);

             // finally add the attacked squares to our member list
            m_lValidMoves.AddRange(ValidMoves);
        }

        #endregion
    }
}
