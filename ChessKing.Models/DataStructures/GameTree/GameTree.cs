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

            if (item.StartSquare == move.StartSquare && item.EndSquare == move.EndSquare && item.Promote == move.Promote)
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

    public string GetAllPgn(GameTreeNode node, int move, bool white)
    {
        string res = "";
        if (white)
        {
            res += $"{move}. ";
        }

        var pgn = node.getPgn();
        if (pgn != "x")
        {
            res += node.getPgn() + " ";
        }

        if (node.Children.Count >= 2)
        {
            res += "( ";
            var copy = node.Children.Head?.Next;

            for (int i = 1; i < node.Children.Count; i++)
            {
                res += GetAllPgn(copy.Data, white ? move : move + 1, !white);
                copy = copy.Next;
            }

            res += ")";
        }

        if (node.Children.Head != null)
        {
            res += GetAllPgn(node.Children.Head.Data, white ? move : move + 1, !white);
        }

        return res;
    }
}