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
    }
}
