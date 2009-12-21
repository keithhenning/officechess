using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace Globals
{
	// store some version information
	public enum VersionInformation : uint
	{
		GAME_VERSION = 1,
		SAVEGAME_FILEVERSION = 102,
	}

    // defines the actual value of a piece
    public enum PValue : int
    {
        None = 0,
        Pawn = 1,
        Knight = 3,
        Bishop = 3,
        Rook = 5,
        Queen = 9,
        King = 100
    }

    // defnies the type of a piece
	public enum PType : sbyte
	{
		BlackQueen = -7,
		BlackRook = -6,
		BlackBishop = -5,
		BlackKing = -3,
		BlackKnight = -2,
		BlackPawn = -1,
		None = 0,
		WhitePawn = 1,
		WhiteKnight = 2,
		WhiteKing = 3,
		WhiteBishop = 5,
		WhiteRook = 6,
		WhiteQueen = 7,
	}

    // defines the color of a piece
	public enum PColor : byte
	{
		Black = 0,
		White,
        None
	}

	// defines the abstract class for a chess piece
	[Serializable]
	abstract public class PrototypePiece
	{
		//////////////////////////////////////////////////////////////////////////
		// Members
		//////////////////////////////////////////////////////////////////////////
		#region Members

		protected PColor m_Color;
		protected PValue m_Value;
		protected PType m_Type;
		protected int m_nPosition;
		protected int m_nPrevPosition;
		protected List<int> m_lAttackingSquares;
		protected List<int> m_lValidMoves;
        public List<int> m_lEnPassantMoves;
        protected bool m_bCanEnPassant;
        protected int m_ID;

		#endregion

		//////////////////////////////////////////////////////////////////////////
		// Public Methods
		//////////////////////////////////////////////////////////////////////////
		#region Public Methods

		// gets the piece type
		abstract public PType GetPieceType();

        // gets current position
        abstract public PColor GetColor();

		// gets current position
		abstract public int GetPosition();

		// sets current position
		abstract public void SetPosition(int NewPosition);

		// updates all lists
		abstract public void Update();

		// gets the squares this piece is attacking
		abstract public List<int> GetAttackingSquares();

		// gets the squares this piece can move to
		abstract public List<int> GetValidMoves();

		// returns true if this piece can move to the destination square
		abstract public bool CanMoveTo(int Destination);
        
        // returns whether or not this piece is still in it's initial position
        abstract public bool HasMoved();

        // adds a valid move to the valid moves list
        abstract public void AddValidMove(int Square);

        // adds enpassant move
        abstract public void AddEnPassantMove(int Square);

        // adds enpassant move
        abstract public List<int> GetEnPassantMoves();

		#endregion

		//////////////////////////////////////////////////////////////////////////
		// Helpers
		//////////////////////////////////////////////////////////////////////////
		#region Helpers

		// updates the squares this piece is attacking
		abstract protected void UpdateAttackingSquares();

		// updates the squares this piece could potentially move to
		abstract protected void UpdateValidMoves();

		// returns all diagonal moves a piece can make
		abstract protected List<int> CalculateDiagonalMoves(int MaxRange);

		// returns all diagonal moves a piece can make
		abstract protected List<int> CalculateHorizontalMoves(int MaxRange);

		// returns all diagonal moves a piece can make
		abstract protected List<int> CalculateVerticalMoves(int MaxRange);

		// calculates all valid moves
		abstract protected List<int> ValidateMoves(List<int> PreValidatedMoves);

		#endregion
	}

    // possible network commands
    public enum NetworkCommand : byte
    {
        CONNECT_REQUEST,
        CONNECT_ACCEPT,
        CONNECT_DENY,
        DISCONNECT_REQUEST,
        DISCONNECT_ACCEPT,
        DISCONNECT_DENY,
        NEW_GAME_REQUEST,
        NEW_GAME_ACCEPT,
        NEW_GAME_DENY,
        MAKE_MOVE_REQUEST,
        MAKE_MOVE_ACCEPT,
        MAKE_MOVE_DENY,
        TAKE_BACK_MOVE_REQUEST,
        TAKE_BACK_MOVE_ACCEPT,
        TAKE_BACK_MOVE_DENY,
        OFFER_DRAW_REQUEST,
        OFFER_DRAW_ACCEPT,
        OFFER_DRAW_DENY,
        RESIGN_REQUEST,
        RESIGN_ACCEPT,
        RESIGN_DENY
    };

    // package used to send and receive data from clients
    // make sure MAX_PACKET_SIZE for server can contain this data
    unsafe public struct NetworkPackage
    {
        public int              m_ConnectionID;
        public NetworkCommand   m_Command;
        public byte             m_FromSquare;
        public byte             m_ToSquare;
        public fixed byte       m_Padding[128];
    };
    
    // last move data
    [Serializable]
    public class AMove
    {
        public PColor ColorMoved;
        public int FromSquare;
        public int ToSquare;
    }

	// holds all global gamedata
	[Serializable]
	public class SaveData
	{
		public uint FileVersion = (uint)VersionInformation.SAVEGAME_FILEVERSION;
		public PrototypePiece[] CurrentGameState = new PrototypePiece[64];
		public PColor ColorMoving = PColor.White;
        public List<AMove> MoveHistory = new List<AMove>();
	}

    // current game state data
	static public class GameData
	{
        // connection ID for network
        static public int g_ConnectionID = 0;

		// current gamestate
        static public PrototypePiece[] g_CurrentGameState = new PrototypePiece[64];

		// global lists
		static public List<int> g_SquaresAttackedByWhite = new List<int>();
		static public List<int> g_SquaresAttackedByBlack = new List<int>();

		static public List<int> g_ValidMovesWhite = new List<int>();
		static public List<int> g_ValidMovesBlack = new List<int>();

		static public PColor g_ColorMoving = PColor.White;

        static public List<AMove> g_MoveHistory = new List<AMove>();
        static public AMove g_LastMove = new AMove();

		// classic starting positions 
		static public sbyte[] g_StartingPositions = 
		{  
			 6,  2,  5,  7,  3,  5,  2,  6,
			 1,  1,  1,  1,  1,  1,  1,  1,
			 0,  0,  0,  0,  0,  0,  0,  0,
			 0,  0,  0,  0,  0,  0,  0,  0,
			 0,  0,  0,  0,  0,  0,  0,  0,
			 0,  0,  0,  0,  0,  0,  0,  0,
			-1, -1, -1, -1, -1, -1, -1, -1,
			-6, -2, -5, -7, -3, -5, -2, -6,
		};

		// saves current game state to file
		static public bool SaveToFile(String FileName)
		{
			try
			{
				// create new save data
				SaveData sd = new SaveData();
				sd.CurrentGameState = g_CurrentGameState;
				sd.ColorMoving = g_ColorMoving;
                sd.MoveHistory = g_MoveHistory;

				// save to file
				BinaryFormatter bf = new BinaryFormatter();
				FileStream fs = new FileStream(FileName, FileMode.Create);
                GZipStream gzs = new GZipStream(fs, CompressionMode.Compress);
                bf.Serialize(gzs, sd);
                gzs.Close();
				fs.Close();
				Console.WriteLine("Successfully saved game to: " + FileName);
			}
			catch (Exception e)
			{
				Console.WriteLine("ERROR: unable to save file..." + FileName + " " +e.Message );
                return false;
			}

			return true;
		}

		// loads game state from file
		static public bool LoadFromFile(String FileName)
		{
			try
			{
				// create new savedata instance
				SaveData sd = new SaveData();

				// load file
				BinaryFormatter bf = new BinaryFormatter();
				FileStream fs = new FileStream(FileName, FileMode.Open);
                GZipStream gzs = new GZipStream(fs, CompressionMode.Decompress);
				sd = (SaveData)bf.Deserialize(gzs);
                gzs.Close();
				fs.Close();

				// check version number
				if (sd.FileVersion == (uint)VersionInformation.SAVEGAME_FILEVERSION)
				{
					// restore savegame data
					GameData.g_CurrentGameState = sd.CurrentGameState;
					GameData.g_ColorMoving = sd.ColorMoving;
                    GameData.g_MoveHistory = sd.MoveHistory;
                    GameData.g_LastMove = GameData.g_MoveHistory[GameData.g_MoveHistory.Count - 1];
					Console.WriteLine("Successfully loaded saved game: " + FileName);
				}
				else
				{
					Console.WriteLine("ERROR: unable to open file: " + FileName + ", incorrect save game file version...");
                    return false;
				}
			}
            catch (Exception e)
            {
                Console.WriteLine("ERROR: unable to open file..." + FileName + " " + e.Message);
                return false;
            }


			return true;
		}
    }

    // global helper functions
    static public class Etc
    {
        // removes duplicates from a list of integers
        static public List<int> RemoveDuplicatesFromList(List<int> list)
        {
            list.Sort(); 
            Int32 index = 0; 
            while (index < list.Count - 1) 
            { 
                if (list[index] == list[index + 1])        
                    list.RemoveAt(index); 
                else        
                    index++; 
            }

            return list;
        }

		// generic clamp function
		static public T Clamp<T>(T Value, T Min, T Max) where T : System.IComparable<T>
		{
			if (Value.CompareTo(Max) > 0)
				return Max;
			if (Value.CompareTo(Min) < 0)
				return Min;
			return Value;
		}

		// return row and column of current square
		static public void GetRowColFromSquare(int CurrSquare, out int Row, out int Col)
		{
			Row = CurrSquare / 8;
			Col = CurrSquare % 8;
		}

		// return current square from row and column
		static public bool GetSquareFromRowCol(int Row, int Col, out int CurrSquare)
		{
            if (Col >= 0 && Col <= 7 && Row >= 0 && Row <= 7)
            {
                CurrSquare = (Row * 8) + Col;
                return true;
            }
            else
            {
                CurrSquare = -1;
                return false;
            }
		}
		
        // returns human readable board coordinate
		static public string SquareToString(int square)
		{
            string resultString = "";
			int row;
			int col;
			
			GetRowColFromSquare(square, out row, out col);
            resultString = ((char)(65 + col)).ToString();
            resultString += (row+1).ToString();

            return resultString;
		}

        // serialize object to byte array
        static public byte[] ObjectToByteArray(object anything)
        {
            try
            {
                int structsize = Marshal.SizeOf(anything);
                IntPtr buffer = Marshal.AllocHGlobal(structsize);
                Marshal.StructureToPtr(anything, buffer, false);
                byte[] streamdatas = new byte[structsize];
                Marshal.Copy(buffer, streamdatas, 0, structsize);
                Marshal.FreeHGlobal(buffer);
                return streamdatas;
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("ERROR: ObjectToByteArray() - " + e.Message.ToString());
                return null;
            }
        }

        // Deserialize byte array to object
        static public object ByteArrayToObject(byte[] rawdata, Type anytype)
        {
            object retobj = null;
            int rawsize = Marshal.SizeOf(anytype);

            if (rawsize <= rawdata.Length)
            {
                try
                {
                    IntPtr buffer = Marshal.AllocHGlobal(rawsize);
                    Marshal.Copy(rawdata, 0, buffer, rawsize);
                    retobj = Marshal.PtrToStructure(buffer, anytype);
                    Marshal.FreeHGlobal(buffer);
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine("ERROR: ByteArrayToObject() - " + e.Message.ToString());
                }
            }

            return retobj;
        }
	}

}

