using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessKing.Models.DataStructures.MyLinkedList;

namespace ChessKing.Models.DataStructures.GameTree
{
    public class GameTreeNode
    {
        public string FEN { get; set; }
        public string? StartSquare { get; set; }
        public string? EndSquare { get; set; }
        public string? PieceName { get; set; }
        public string? Promote { get; set; }

        public MyLinkedList<GameTreeNode> Children { get; set; }

        public GameTreeNode()
        {
            Children = new MyLinkedList<GameTreeNode>();
        }

        public GameTreeNode? FindParent(GameTreeNode root)
        {
            if (this == root)
            {
                return null;
            }

            if (root.ContainsChild(this))
            {
                return root;
            }

            var copyhead = root.Children.Head;

            for (int i = 0; i < root.Children.Count; i++)
            {
                var found = this.FindParent(copyhead.Data);

                if (found != null)
                {
                    return found;
                }

                copyhead = copyhead.Next;
            }

            return null;
        }

        public bool ContainsChild(GameTreeNode node)
        {
            var copy = this.Children.Head;
            for (int i = 0; i < this.Children.Count; i++)
            {
                if (copy.Data == node)
                {
                    return true;
                }
                copy = copy.Next;
            }

            return false;
        }

        public string getPgn()
        {
            string mid = FEN.Split(' ').ElementAt(4) == "0" ? "x" : "";
            if (this.PieceName == "p" || this.PieceName == "P")
            {
                if (this.StartSquare.ElementAt(0) == this.EndSquare.ElementAt(0))
                {
                    return this.EndSquare + (Promote != "" ? $"={Promote}" : "");
                }
                else
                {
                    return this.StartSquare.ElementAt(0) + mid + this.EndSquare + (Promote != "" ? $"={Promote}" : "");
                }
            }

            if (this.PieceName == "k" || this.PieceName == "K")
            {
                if (StartSquare == "e1")
                {
                    if (EndSquare == "g1")
                    {
                        return "O-O";
                    }
                    else if (EndSquare == "c1")
                    {
                        return "O-O-O";
                    }
                }

                if (StartSquare == "e8")
                {
                    if (EndSquare == "g8")
                    {
                        return "O-O";
                    }
                    else if (EndSquare == "c8")
                    {
                        return "O-O-O";
                    }
                }
            }

            return this.PieceName?.ToUpper() + this.StartSquare + mid + this.EndSquare;
        }

        public PiecesCount GetPiecesCount()
        {
            PiecesCount count = new PiecesCount();

            var rows = FEN.Split(' ')[0].Split('/');

            foreach (var Row in rows)
            {
                foreach (var ch in Row)
                {
                    switch (ch)
                    {
                        case 'p':
                            count.BlackPawn++;
                            break;
                        case 'n':
                            count.BlackKnight++;
                            break;
                        case 'b':
                            count.BlackBishop++;
                            break;
                        case 'r':
                            count.BlackRook++;
                            break;
                        case 'q':
                            count.BlackQueen++;
                            break;
                        case 'k':
                            count.BlackKing++;
                            break;

                        case 'P':
                            count.WhitePawn++;
                            break;
                        case 'N':
                            count.WhiteKnight++;
                            break;
                        case 'B':
                            count.WhiteBishop++;
                            break;
                        case 'R':
                            count.WhiteRook++;
                            break;
                        case 'Q':
                            count.WhiteQueen++;
                            break;
                        case 'K':
                            count.WhiteKing++;
                            break;

                        default:
                            break;
                    }
                }
            }

            return count;
        }
    }

    public record PiecesCount
    {
        public int WhitePawn { get; set; }
        public int WhiteQueen { get; set; }
        public int WhiteRook { get; set; }
        public int WhiteBishop { get; set; }
        public int WhiteKnight { get; set; }
        public int WhiteKing { get; set; }

        public int BlackPawn { get; set; }
        public int BlackQueen { get; set; }
        public int BlackRook { get; set; }
        public int BlackBishop { get; set; }
        public int BlackKnight { get; set; }
        public int BlackKing { get; set; }
    }
}
