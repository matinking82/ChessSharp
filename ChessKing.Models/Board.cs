﻿using System.Text.RegularExpressions;
using System.Linq;

namespace ChessKing.Models
{
    public class Board
    {
        public static List<string> Pieces = new List<string>
        {
            "P","N","B","R","Q","K",
            "p","n","b","r","q","k",
        };
        private static List<string> FileNames = new List<string>()
            {
                "a", "b", "c", "d", "e", "f", "g", "h"
            };

        public bool WhitesTurn { get; set; }
        public bool WhiteShortCastle { get; set; }
        public bool WhiteLongCastle { get; set; }
        public bool BlackShortCastle { get; set; }
        public bool BlackLongCastle { get; set; }
        private ChessSquare EnPassantSquare;

        private List<List<ChessSquare>> _squares;



        public Board(string FEN = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq")
        {
            InitializeBoard();
            SetFEN(FEN);
        }

        public void Move(string pgnMove)
        {
            pgnMove = pgnMove.Replace("+", "");
            pgnMove = pgnMove.Replace("#", "");
            pgnMove = pgnMove.Replace("x", "");

            pgnType Type = GetPgnType(pgnMove);
            var pieces = GetAllPiecesForColor(WhitesTurn);

            string StartSquare = "";
            string EndSquare = "";

            switch (Type)
            {
                case pgnType.PawnMove://a4 ab5

                    EndSquare = pgnMove;
                    if (pgnMove.Length == 3)
                    {
                        EndSquare = pgnMove.Substring(1, 2);
                    }

                    int pawnId = 0;
                    if (!WhitesTurn)
                    {
                        pawnId = 6;
                    }

                    StartSquare = pieces
                        .Where(p => this[p].PieceId == pawnId)
                        .Where(p => AvailableSquares(p).Contains(EndSquare))
                        .First();


                    break;

                case pgnType.PieceMove1://Ba4

                    EndSquare = pgnMove.Substring(1, 2);
                    int PieceId = Pieces.IndexOf(pgnMove[0].ToString());
                    if (!WhitesTurn)
                    {
                        PieceId += 6;
                    }

                    StartSquare = pieces
                        .Where(p => this[p].PieceId == PieceId)
                        .Where(p => AvailableSquares(p).Contains(EndSquare))
                        .First();

                    break;
                case pgnType.PieceMove2://Bba4

                    EndSquare = pgnMove.Substring(2, 2);
                    PieceId = Pieces.IndexOf(pgnMove[0].ToString());
                    if (!WhitesTurn)
                    {
                        PieceId += 6;
                    }

                    int file = FileNames.IndexOf(pgnMove[1].ToString());

                    var FoundPieces = pieces
                        .Where(p => this[p].PieceId == PieceId)
                        .Where(p => AvailableSquares(p).Contains(EndSquare))
                        .ToList();

                    for (int i = 0; i <= 7; i++)
                    {
                        var item = _squares[file][i];


                        if (FoundPieces.Select(p => this[p]).Contains(item))
                        {
                            StartSquare = GetSquareName(new Tuple<int, int>(file,i));
                            break;
                        }
                    }

                    break;
                case pgnType.PieceMove3:
                    EndSquare = pgnMove.Substring(2, 2);
                    PieceId = Pieces.IndexOf(pgnMove[0].ToString());
                    if (!WhitesTurn)
                    {
                        PieceId += 6;
                    }

                    int rank = FileNames.IndexOf(pgnMove[1].ToString());

                    FoundPieces = pieces
                        .Where(p => this[p].PieceId == PieceId)
                        .Where(p => AvailableSquares(p).Contains(EndSquare))
                        .ToList();

                    for (int i = 0; i <= 7; i++)
                    {
                        var item = _squares[i][rank];


                        if (FoundPieces.Select(p => this[p]).Contains(item))
                        {
                            StartSquare = GetSquareName(new Tuple<int, int>(i, rank));
                            break;
                        }
                    }
                    break;
                case pgnType.PieceMove4:
                    StartSquare = pgnMove.Substring(1,2);
                    EndSquare = pgnMove.Substring(3, 2);
                    PieceId = Pieces.IndexOf(pgnMove[0].ToString());

                    if (this[StartSquare].PieceId!=PieceId)
                    {
                        return;
                    }

                    break;
                case pgnType.PawnPromote:
                    break;
                default:
                    break;
            }

            MoveSquare(StartSquare, EndSquare);

            WhitesTurn = !WhitesTurn;
        }

        public List<string> AvailableSquares(string startSquare, bool atack = false)
        {
            ChessSquare Square = this[startSquare];

            switch (Square.PieceId)
            {
                case 0://pawn
                    if (atack)
                    {
                        return PawnAtackedSquares(startSquare);
                    }
                    return PawnAvailableSquares(startSquare);
                    break;
                case 1://knight
                    return knightAvailableSquares(startSquare, atack);
                    break;
                case 2://bishop
                    return BishopAvailableSquares(startSquare, atack);
                    break;
                case 3://rook
                    return RookAvailableSquares(startSquare, atack);
                    break;
                case 4://queen
                    return QueenAvailableSquares(startSquare, atack);
                    break;
                case 5://king
                    if (atack)
                    {
                        return KingAtackedSquares(startSquare);
                    }
                    return KingAvailableSquares(startSquare);
                    break;
                case 6://pawn
                    if (atack)
                    {
                        return PawnAtackedSquares(startSquare);
                    }
                    return PawnAvailableSquares(startSquare);
                    break;
                case 7://knight
                    return knightAvailableSquares(startSquare, atack);
                    break;
                case 8://bishop
                    return BishopAvailableSquares(startSquare, atack);
                    break;
                case 9://rook
                    return RookAvailableSquares(startSquare, atack);
                    break;
                case 10://queen
                    return QueenAvailableSquares(startSquare, atack);
                    break;
                case 11://king
                    if (atack)
                    {
                        return KingAtackedSquares(startSquare);
                    }
                    return KingAvailableSquares(startSquare);
                    break;
                default:
                    break;
            }

            throw new NotImplementedException();
        }

        public ChessSquare this[string key]
        {
            get
            {
                return GetSquare(key);
            }
        }



        ///////////////////////////////////////////

        private void InitializeBoard()
        {
            List<List<ChessSquare>> Squares = new List<List<ChessSquare>>();
            for (int i = 0; i < 8; i++)
            {
                List<ChessSquare> File = new List<ChessSquare>();
                for (int j = 0; j < 8; j++)
                {
                    File.Add(new ChessSquare());
                }
                Squares.Add(File);
            }
            _squares = Squares;
        }

        private bool MoveSquare(string startSquare, string EndSquare)
        {
            var StartChessSquare = this[startSquare];

            var AvailableSquares = this.AvailableSquares(startSquare);

            if (!AvailableSquares.Contains(EndSquare))
            {
                return false;
            }

            this[EndSquare].PieceId = StartChessSquare.PieceId;

            StartChessSquare.PieceId = null;

            return true;
        }

        private List<string> PawnAvailableSquares(string startSquare)
        {
            var Coordinate = GetCoordinate(startSquare);

            int file = Coordinate.Item1;
            int rank = Coordinate.Item2;

            List<string> AvailableSquares = new List<string>();

            int unit = 1;
            if (!IsWhitePiece(this[startSquare]))
            {
                unit = -1;
            }


            for (var i = file - 1; i <= file + 1; i++)
            {
                if (i < 0 || i > 7)
                {
                    continue;
                }
                var sqr = new Tuple<int, int>(i, Coordinate.Item2 + unit);
                //var sqr = _squares[i][Coordinate.Item2 + unit];

                if (i == file)
                {
                    if (!HasPiece(this[GetSquareName(sqr)]))
                    {
                        AvailableSquares.Add(GetSquareName(sqr));
                        if ((IsWhitePiece(this[startSquare]) && rank == 1) || (!IsWhitePiece(this[startSquare]) && rank == 6))
                        {
                            var sqer = new Tuple<int, int>(file, rank + 2 * unit);
                            //var sqer = _squares[file][rank + 2 * unit];
                            if (!HasPiece(this[GetSquareName(sqer)]))
                            {
                                AvailableSquares.Add(GetSquareName(sqer));
                            }
                        }
                    }
                }
                else
                {
                    if (EnPassantSquare == this[GetSquareName(sqr)])
                    {
                        AvailableSquares.Add(GetSquareName(sqr));
                    }
                    else
                    {
                        if (HasPiece(this[GetSquareName(sqr)]))
                        {
                            if (!IsFriend(this[startSquare], this[GetSquareName(sqr)]))
                            {
                                AvailableSquares.Add(GetSquareName(sqr));
                            }
                        }
                    }
                }
            }


            return AvailableSquares;
        }

        private List<string> PawnAtackedSquares(string startSquare)
        {
            var Coordinate = GetCoordinate(startSquare);

            int file = Coordinate.Item1;

            List<string> AvailableSquares = new List<string>();

            int unit = 1;
            if (!IsWhitePiece(this[startSquare]))
            {
                unit = -1;
            }


            for (var i = file - 1; i <= file + 1; i++)
            {
                if (i < 0 || i > 7)
                {
                    continue;
                }
                var sqr = new Tuple<int, int>(i, Coordinate.Item2 + unit);

                if (i != file)
                {
                    AvailableSquares.Add(GetSquareName(sqr));
                }
            }


            return AvailableSquares;
        }

        private List<string> knightAvailableSquares(string startSquare, bool atack = false)
        {
            var Coordinate = GetCoordinate(startSquare);

            int file = Coordinate.Item1;
            int rank = Coordinate.Item2;

            List<string> AvailableSquares = new List<string>();

            for (var i = file - 2; i <= file + 2; i++)
            {
                if (i < 0 || i > 7)
                {
                    continue;
                }
                for (var j = rank - 2; j <= rank + 2; j++)
                {
                    if (j < 0 || j > 7)
                    {
                        continue;
                    }


                    var side1 = Math.Abs(i - file);
                    var side2 = Math.Abs(j - rank);

                    if (side1 + side2 == 3)
                    {
                        var sqr = _squares[i][j];
                        if (HasPiece(sqr))
                        {
                            if (IsFriend(sqr, this[startSquare]))
                            {
                                if (!atack)
                                {
                                    continue;
                                }
                            }
                        }
                        AvailableSquares.Add(GetSquareName(new Tuple<int, int>(i, j)));
                    }
                }
            }

            return AvailableSquares;
        }

        private List<string> BishopAvailableSquares(string startSquare, bool atack = false)
        {
            List<string> AvailableSquares = new List<string>();
            AvailableSquares.AddRange(GetDiagonal(1, 1, startSquare, atack));
            AvailableSquares.AddRange(GetDiagonal(-1, 1, startSquare, atack));
            AvailableSquares.AddRange(GetDiagonal(1, -1, startSquare, atack));
            AvailableSquares.AddRange(GetDiagonal(-1, -1, startSquare, atack));

            return AvailableSquares;

        }

        private List<string> GetDiagonal(int unit1, int unit2, string square, bool atack = false)
        {
            var Coordinate = GetCoordinate(square);

            List<string> AvailableSquares = new List<string>();

            var file = Coordinate.Item1;
            var rank = Coordinate.Item2;

            for (var i = 1; true; i++)
            {
                var sqrFile = file + (i * unit1);
                var sqrRank = rank + (i * unit2);
                if (sqrFile < 0 || sqrFile > 7 || sqrRank > 7 || sqrRank < 0)
                {
                    break;
                }

                var sqr = _squares[sqrFile][sqrRank];
                var sqrname = GetSquareName(new Tuple<int, int>(sqrFile, sqrRank));
                if (HasPiece(sqr))
                {
                    if (!IsFriend(sqr, this[square]))
                    {
                        AvailableSquares.Add(sqrname);
                    }
                    else if (atack)
                    {
                        AvailableSquares.Add(sqrname);
                    }
                    if (atack)
                    {
                        var pieceId = sqr.PieceId;
                        if (!((pieceId == 5 && !IsWhitePiece(this[square])) || (pieceId == 11 && IsWhitePiece(this[square]))))
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                AvailableSquares.Add(sqrname);
            }
            return AvailableSquares;
        }

        private List<string> RookAvailableSquares(string startSquare, bool atack = false)
        {
            return GetVertical(startSquare, atack);
        }

        private List<string> GetVertical(string square, bool atack = false)
        {
            var Coordinate = GetCoordinate(square);

            List<string> AvailableSquares = new List<string>();

            var file = Coordinate.Item1;
            var rank = Coordinate.Item2;

            for (var i = file + 1; true; i++)
            {
                if (i > 7 || i < 0)
                {
                    break;
                }

                var sqr = _squares[i][rank];
                var sqrname = GetSquareName(new Tuple<int, int>(i, rank));
                if (HasPiece(sqr))
                {
                    if (!IsFriend(sqr, this[square]))
                    {
                        AvailableSquares.Add(sqrname);
                    }
                    else if (atack)
                    {
                        AvailableSquares.Add(sqrname);
                    }
                    if (atack)
                    {
                        var pieceId = sqr.PieceId;
                        if (!((pieceId == 5 && !IsWhitePiece(this[square])) || (pieceId == 11 && IsWhitePiece(this[square]))))
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                AvailableSquares.Add(sqrname);
            }

            for (var i = file - 1; true; i--)
            {
                if (i > 7 || i < 0)
                {
                    break;
                }

                var sqr = _squares[i][rank];
                var sqrname = GetSquareName(new Tuple<int, int>(i, rank));
                if (HasPiece(sqr))
                {
                    if (!IsFriend(sqr, this[square]))
                    {
                        AvailableSquares.Add(sqrname);
                    }
                    else if (atack)
                    {
                        AvailableSquares.Add(sqrname);

                    }
                    if (atack)
                    {
                        var pieceId = sqr.PieceId;
                        if (!((pieceId == 5 && !IsWhitePiece(this[square])) || (pieceId == 11 && IsWhitePiece(this[square]))))
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                AvailableSquares.Add(sqrname);

            }

            for (var i = rank + 1; true; i++)
            {
                if (i > 7 || i < 0)
                {
                    break;
                }

                var sqr = _squares[file][i];
                var sqrname = GetSquareName(new Tuple<int, int>(file, i));
                if (HasPiece(sqr))
                {
                    if (!IsFriend(sqr, this[square]))
                    {
                        AvailableSquares.Add(sqrname);

                    }
                    else if (atack)
                    {
                        AvailableSquares.Add(sqrname);

                    }
                    if (atack)
                    {
                        var pieceId = sqr.PieceId;
                        if (!((pieceId == 5 && !IsWhitePiece(this[square])) || (pieceId == 11 && IsWhitePiece(this[square]))))
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                AvailableSquares.Add(sqrname);

            }

            for (var i = rank - 1; true; i--)
            {
                if (i > 7 || i < 0)
                {
                    break;
                }

                var sqr = _squares[file][i];
                var sqrname = GetSquareName(new Tuple<int, int>(file, i));
                if (HasPiece(sqr))
                {
                    if (!IsFriend(sqr, this[square]))
                    {
                        AvailableSquares.Add(sqrname);
                    }
                    else if (atack)
                    {
                        AvailableSquares.Add(sqrname);
                    }
                    if (atack)
                    {
                        var pieceId = sqr.PieceId;
                        if (!((pieceId == 5 && !IsWhitePiece(this[square])) || (pieceId == 11 && IsWhitePiece(this[square]))))
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                AvailableSquares.Add(sqrname);
            }

            return AvailableSquares;
        }

        private List<string> QueenAvailableSquares(string startSquare, bool atack = false)
        {
            List<string> AvailableSquares = new List<string>();
            AvailableSquares.AddRange(GetDiagonal(1, 1, startSquare, atack));
            AvailableSquares.AddRange(GetDiagonal(-1, 1, startSquare, atack));
            AvailableSquares.AddRange(GetDiagonal(1, -1, startSquare, atack));
            AvailableSquares.AddRange(GetDiagonal(-1, -1, startSquare, atack));
            AvailableSquares.AddRange(GetVertical(startSquare, atack));

            return AvailableSquares;
        }

        private List<string> KingAvailableSquares(string startSquare)
        {
            var Coordinate = GetCoordinate(startSquare);

            List<string> AvailableSquares = new List<string>();

            var file = Coordinate.Item1;
            var rank = Coordinate.Item2;

            var enemyAtackedSquares = GetAtackedSquaresForColor(!IsWhitePiece(this[startSquare]));


            for (var i = file - 1; i <= file + 1; i++)
            {
                for (var j = rank - 1; j <= rank + 1; j++)
                {

                    if (i > 7 || i < 0 || j > 7 || j < 0)
                    {
                        continue;
                    }

                    var sqr = _squares[i][j];
                    var sqrname = GetSquareName(new Tuple<int, int>(i, j));
                    if (enemyAtackedSquares.Contains(sqrname))
                    {
                        continue;
                    }
                    if (HasPiece(sqr))
                    {
                        if (IsFriend(sqr, this[startSquare]))
                        {
                            continue;
                        }
                    }
                    AvailableSquares.Add(sqrname);
                }
            }

            //castling
            if (IsWhitePiece(this[startSquare]))
            {
                if (!(IsCheck(true)))
                {
                    if (WhiteShortCastle)
                    {
                        if (!HasPiece(_squares[6][0]) && !HasPiece(this[GetSquareName(new Tuple<int, int>(6, 0))])
                            && !enemyAtackedSquares.Contains(GetSquareName(new Tuple<int, int>(5, 0)))
                            && !enemyAtackedSquares.Contains(GetSquareName(new Tuple<int, int>(6, 0)))
                            && !enemyAtackedSquares.Contains(GetSquareName(new Tuple<int, int>(7, 0))))
                        {
                            var sqrname = GetSquareName(new Tuple<int, int>(6, 0));
                            AvailableSquares.Add(sqrname);
                        }
                    }
                    if (WhiteLongCastle)
                    {
                        if (!HasPiece(_squares[1][0]) && !HasPiece(_squares[2][0]) && !HasPiece(_squares[3][0])
                            && !enemyAtackedSquares.Contains(GetSquareName(new Tuple<int, int>(0, 0)))
                            && !enemyAtackedSquares.Contains(GetSquareName(new Tuple<int, int>(1, 0)))
                            && !enemyAtackedSquares.Contains(GetSquareName(new Tuple<int, int>(2, 0)))
                            && !enemyAtackedSquares.Contains(GetSquareName(new Tuple<int, int>(3, 0))))
                        {
                            var sqrname = GetSquareName(new Tuple<int, int>(2, 0));
                            AvailableSquares.Add(sqrname);
                        }
                    }
                }
            }
            else
            {
                if (!(IsCheck(false)))
                {
                    if (BlackShortCastle)
                    {
                        if (!HasPiece(_squares[5][7]) && !HasPiece(_squares[6][7])
                            && !enemyAtackedSquares.Contains(GetSquareName(new Tuple<int, int>(5, 7)))
                            && !enemyAtackedSquares.Contains(GetSquareName(new Tuple<int, int>(6, 7)))
                            && !enemyAtackedSquares.Contains(GetSquareName(new Tuple<int, int>(7, 7))))
                        {
                            var sqrname = GetSquareName(new Tuple<int, int>(6, 7));
                            AvailableSquares.Add(sqrname);
                        }
                    }
                    if (BlackLongCastle)
                    {
                        if (!HasPiece(_squares[1][7]) && !HasPiece(_squares[2][7]) && !HasPiece(_squares[3][7])
                            && !enemyAtackedSquares.Contains(GetSquareName(new Tuple<int, int>(0, 7)))
                            && !enemyAtackedSquares.Contains(GetSquareName(new Tuple<int, int>(1, 7)))
                            && !enemyAtackedSquares.Contains(GetSquareName(new Tuple<int, int>(2, 7)))
                            && !enemyAtackedSquares.Contains(GetSquareName(new Tuple<int, int>(3, 7))))
                        {
                            var sqrname = GetSquareName(new Tuple<int, int>(2, 7));
                            AvailableSquares.Add(sqrname);
                        }
                    }
                }
            }

            return AvailableSquares;
        }

        private List<string> KingAtackedSquares(string startSquare)
        {
            var Coordinate = GetCoordinate(startSquare);

            List<string> AtackedSquares = new List<string>();

            var file = Coordinate.Item1;
            var rank = Coordinate.Item2;


            for (var i = file - 1; i <= file + 1; i++)
            {
                for (var j = rank - 1; j <= rank + 1; j++)
                {

                    if (i > 7 || i < 0 || j > 7 || j < 0)
                    {
                        continue;
                    }

                    AtackedSquares.Add(GetSquareName(new Tuple<int, int>(i, j)));
                }
            }

            return AtackedSquares;
        }

        private List<string> GetAtackedSquaresForColor(bool White)
        {
            var AllPieces = GetAllPiecesForColor(White);

            List<string> Squares = new List<string>();

            foreach (var item in AllPieces)
            {
                Squares.AddRange(AvailableSquares(item, true));
            }

            Squares = Squares.Distinct().ToList();

            return Squares;
        }

        private List<string> GetAllPiecesForColor(bool White)
        {
            List<string> FoundPieces = new List<string>();
            for (int file = 0; file <= 7; file++)
            {
                for (int rank = 0; rank <= 7; rank++)
                {
                    var squareName = GetSquareName(new Tuple<int, int>(file, rank));

                    var square = this[squareName];

                    if (square.PieceId != null)
                    {
                        if (square.PieceId <= 5)
                        {
                            if (White)
                            {
                                FoundPieces.Add(squareName);
                            }
                        }
                        else
                        {
                            if (!White)
                            {
                                FoundPieces.Add(squareName);
                            }
                        }
                    }
                }
            }

            return FoundPieces;
        }

        private bool IsCheck(bool White = true)
        {
            var atackedSquares = GetAtackedSquaresForColor(!White);

            var kingId = 5;
            if (!White)
            {
                kingId = 11;
            }

            for (var i = 0; i < atackedSquares.Count; i++)
            {
                var square = atackedSquares[i];

                if (HasPiece(this[square]))
                {

                    var pieceId = this[square].PieceId;

                    if (pieceId == kingId)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private ChessSquare GetSquare(string key)
        {
            var Coordinate = GetCoordinate(key);

            int file = Coordinate.Item1;
            int rank = Coordinate.Item2;


            return _squares[file][rank];
        }

        private void ClearBoard()
        {
            foreach (var File in _squares)
            {
                foreach (var square in File)
                {
                    square.PieceId = null;
                }
            }
        }

        private void ResetValues()
        {
            WhitesTurn = true;
            WhiteShortCastle = true;
            WhiteLongCastle = true;
            BlackShortCastle = true;
            BlackLongCastle = true;


        }

        private void SetFEN(string FEN)
        {
            ClearBoard();
            var Temp = FEN.Split(' ');

            if (Temp.Length > 1)
            {
                WhitesTurn = Temp[1] == "w";
            }
            else
            {
                WhitesTurn = true;
            }

            #region Castles

            WhiteShortCastle = false;
            WhiteLongCastle = false;
            BlackLongCastle = false;
            BlackShortCastle = false;

            string Castles = "";
            Regex CastleRegex = new Regex("^[K]?[Q]?[k]?[q]?$");

            foreach (var item in Temp)
            {
                if (CastleRegex.IsMatch(item))
                {
                    Castles = item;
                    break;
                }
            }

            if (CastleRegex.IsMatch(Castles))
            {
                if (new Regex("[K]{1}").IsMatch(Castles))
                {
                    WhiteShortCastle = true;
                }
                if (new Regex("[Q]{1}").IsMatch(Castles))
                {
                    WhiteLongCastle = true;
                }
                if (new Regex("[k]{1}").IsMatch(Castles))
                {
                    BlackShortCastle = true;
                }
                if (new Regex("[q]{1}").IsMatch(Castles))
                {
                    BlackLongCastle = true;
                }
            }

            #endregion

            var FenRows = Temp[0].Split('/');


            for (int i = FenRows.Length - 1; i >= 0; i--)
            {
                var Row = FenRows[i];
                int rank = 7 - i;

                int file = 0;

                for (int j = 0; j < Row.Length; j++)
                {
                    var item = Row.ElementAt(j).ToString();

                    if (Pieces.Contains(item))
                    {
                        int pieceId = Pieces.IndexOf(item);

                        _squares[file][rank].PieceId = pieceId;

                        file++;
                    }
                    else
                    {
                        file += int.Parse(item);
                    }

                }

            }
        }

        private pgnType GetPgnType(string pgn)
        {
            Regex PawnMove = new Regex("^[a-h]?[a-h][1-8]$");
            Regex PieceMove1 = new Regex("^[B|N|R|Q|K][a-h][1-8]$");
            Regex PieceMove2 = new Regex("^[B|N|R|Q|K][a-h]{2}[1-8]$");
            Regex PieceMove3 = new Regex("^[B|N|R|Q|K][1-8][a-h][1-8]$");
            Regex PieceMove4 = new Regex("^[B|N|R|Q|K][a-h][1-8][a-h][1-8]$");
            Regex PawnPromote = new Regex("^[a-h]?[a-h][1-8]=[B|N|R|Q]$");

            if (PawnMove.IsMatch(pgn))
            {
                return pgnType.PawnMove;
            }
            else if (PieceMove1.IsMatch(pgn))
            {
                return pgnType.PieceMove1;
            }
            else if (PieceMove2.IsMatch(pgn))
            {
                return pgnType.PieceMove2;
            }
            else if (PieceMove3.IsMatch(pgn))
            {
                return pgnType.PieceMove3;
            }
            else if (PieceMove4.IsMatch(pgn))
            {
                return pgnType.PieceMove4;
            }
            else if (PawnPromote.IsMatch(pgn))
            {
                return pgnType.PawnPromote;
            }
            return pgnType.error;
        }

        private Tuple<int, int> GetCoordinate(string SquareName)
        {
            Regex regex = new Regex("^[a-h]{1}[1-8]{1}$");

            if (!regex.IsMatch(SquareName))
            {
                throw new Exception("Wrong Key!!");
            }

            int file = FileNames.IndexOf(SquareName.ElementAt(0).ToString());
            int rank = int.Parse(SquareName.ElementAt(1).ToString()) - 1;

            return new Tuple<int, int>(file, rank);
        }

        private bool HasPiece(ChessSquare square)
        {
            if (square.PieceId == null)
            {
                return false;
            }
            return true;
        }

        private bool IsWhitePiece(ChessSquare square)
        {
            return square.PieceId <= 5;
        }

        private bool IsFriend(ChessSquare square1, ChessSquare square2)
        {
            int piece1 = square1.PieceId.Value;
            int piece2 = square2.PieceId.Value;

            if ((piece1 <= 5 && piece2 <= 5) || (piece1 > 5 && piece2 > 5))
            {
                return true;
            }
            return false;
        }

        private string GetSquareName(Tuple<int, int> Coordinate)
        {
            return $"{FileNames[Coordinate.Item1]}{Coordinate.Item2 + 1}";
        }

        private enum pgnType
        {
            PawnMove,//a4 or ab5
            PieceMove1,//Ba4
            PieceMove2,//Bba4
            PieceMove3,//B5a4
            PieceMove4,//Bb5a4
            PawnPromote,//e8=Q or de8=Q
            error
        }

    }

    public class ChessSquare
    {
        public static List<string> Pieces = new List<string>
        {
            "P","N","B","R","Q","K",
            "p","n","b","r","q","k",
        };
        public int? PieceId { get; set; }
        public string PieceName
        {
            get
            {
                if (PieceId == null)
                {
                    return " ";
                }
                return Pieces[PieceId.Value];
            }
        }
    }
}