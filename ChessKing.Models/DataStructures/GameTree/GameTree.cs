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
        Active.Children.Add(move);
        Active = move;
    }

    public string GetActiveFen()
    {
        return Active.FEN;
    }
}