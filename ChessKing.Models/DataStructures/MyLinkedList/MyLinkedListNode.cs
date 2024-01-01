using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessKing.Models.DataStructures.MyLinkedList
{
    public class MyLinkedListNode<T>
    {
        public T Data { get; set; }
        public MyLinkedListNode<T>? Next { get; set; } = null;
        public MyLinkedListNode<T>? Previous { get; set; } = null;

        public MyLinkedListNode(T data)
        {
            Data = data;
        }
    }
}
