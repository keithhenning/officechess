using System;
using System.Collections.Generic;
using System.Text;
using Globals;

namespace ChessLogic.Pieces
{
	[Serializable]
    class Bishop : Piece
    {
        //////////////////////////////////////////////////////////////////////////
        // Public Methods
        //////////////////////////////////////////////////////////////////////////
        #region Public Methods

        // constructor
        public Bishop(PColor PieceColor)
        {
            m_Color = PieceColor;
            m_Value = PValue.Bishop;
            m_Type = (m_Color == PColor.White) ? PType.WhiteBishop : PType.BlackBishop;
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

            // get all valid moves with "infinite" range
            AttackingSquares = CalculateDiagonalMoves(0);

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

            // get all valid moves with "infinite" range
            PreValidatedMoves = CalculateDiagonalMoves(0);

            // validate moves
            ValidMoves = ValidateMoves(PreValidatedMoves);

            // finally add the valid moves to our member list
            m_lValidMoves.AddRange(ValidMoves);
        }

        #endregion
    }
}
