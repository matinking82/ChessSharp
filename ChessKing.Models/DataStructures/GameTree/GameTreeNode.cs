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

        public MyLinkedList<GameTreeNode> Children { get; set; }

        public GameTreeNode()
        {
            Children = new MyLinkedList<GameTreeNode>();
        }

    }
}
