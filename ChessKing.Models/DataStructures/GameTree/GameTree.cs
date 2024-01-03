namespace ChessKing.Models.DataStructures.GameTree;

public class GameTree
{
    public GameTreeNode Root { get; set; }
    public GameTreeNode Active { get; set; }
    public GameTree(string fen)
    {
        Root = new GameTreeNode()
        {
            FEN = fen
        };

        Active = Root;
    }

    public void NewMove(GameTreeNode move)
    {
        var copyhead = Active.Children.Head;
        for (int i = 0; i < Active.Children.Count; i++)
        {
            var item = copyhead.Data;

            if (item.StartSquare == move.StartSquare && item.EndSquare == move.EndSquare)
            {
                Active = item;
                return;
            }

            copyhead = copyhead.Next;
        }

        Active.Children.Add(move);
        Active = move;
    }

    public string GetActiveFen()
    {
        return Active.FEN;
    }
}