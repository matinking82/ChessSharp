namespace ChessKing.Models.DataStructures.MyLinkedList;

public class MyLinkedList<T>
{
    public MyLinkedListNode<T>? Head { get; set; } = null;

    public MyLinkedList()
    {

    }

    public void Add(T data)
    {
        var newItem = new MyLinkedListNode<T>(data);
        if (Head == null)
        {
            Head = newItem;
            return;
        }

        newItem.Next = Head;
        newItem.Previous = Head.Previous;

        if (Head.Previous!=null)
        {
            Head.Previous.Next = newItem;
        }

        Head.Previous = newItem;
    }

    public bool IsEmpty()
    {
        return Head == null;
    }
}