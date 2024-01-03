namespace ChessKing.Models.DataStructures.MyLinkedList;

public class MyLinkedList<T>
{
    public MyLinkedListNode<T>? Head { get; set; } = null;
    public int Count { get; set; } = 0;
    public MyLinkedList()
    {

    }

    public void Add(T data)
    {
        Count++;
        var newItem = new MyLinkedListNode<T>(data);
        if (Head == null)
        {
            Head = newItem;
            return;
        }

        if (Head.Previous == null && Head.Next == null)
        {
            newItem.Previous = Head;
            newItem.Next = Head;
            Head.Next = newItem;
            Head.Previous = newItem;
            return;
        }

        newItem.Next = Head;
        newItem.Previous = Head.Previous;

        if (Head.Previous != null)
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