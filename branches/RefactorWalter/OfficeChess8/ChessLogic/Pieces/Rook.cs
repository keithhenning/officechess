using System;
using System.Collections.Generic;
using System.Text;
using Globals;

namespace ChessLogic.Pieces
{
	[Serializable]
    class Rook : Piece
    {
        //////////////////////////////////////////////////////////////////////////
        // Public Methods
        //////////////////////////////////////////////////////////////////////////
        #region Public Methods

        // constructor
        public Rook(PColor PieceColor)
        {
            m_Color = PieceColor;
            m_Value = PValue.Rook;
            m_Type = (m_Color == PColor.White) ? PType.WhiteRook : PType.BlackRook;
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
            List<int> AttackingSquaresHorz = new List<int>();
            List<int> AttackingSquaresVert = new List<int>();

            // clear our list
            m_lAttackingSquares.Clear();

            // get all valid moves
            AttackingSquaresHorz = CalculateHorizontalMoves(0);
            AttackingSquaresVert = CalculateVerticalMoves(0);

            // finally add the attacked squares to our member list
            m_lAttackingSquares.AddRange(AttackingSquaresHorz);
            m_lAttackingSquares.AddRange(AttackingSquaresVert);
        }

        // updates the squares this piece could potentially move to
        protected override void UpdateValidMoves()
        {
            // initialize
            List<int> PreValidatedMovesHorz = new List<int>();
            List<int> PreValidatedMovesVert = new List<int>();
            List<int> PreValidatedMoves = new List<int>();
            List<int> ValidMoves = new List<int>();

            // clear our list
            m_lValidMoves.Clear();

            // get all valid moves
            PreValidatedMovesHorz = CalculateHorizontalMoves(0);
            PreValidatedMovesVert = CalculateVerticalMoves(0);

            // concat all moves
            PreValidatedMoves.AddRange(PreValidatedMovesHorz);
            PreValidatedMoves.AddRange(PreValidatedMovesVert);

            // validate moves
            ValidMoves = ValidateMoves(PreValidatedMoves);

            // finally add the attacked squares to our member list
            m_lValidMoves.AddRange(ValidMoves);
        }

        #endregion
    }
}
