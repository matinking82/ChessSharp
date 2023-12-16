using ChessKing.Models;

namespace TestConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {

            //while (true)
            //{
            //    Console.WriteLine(Console.ReadLine().Replace("x", ""));
            //}

            //Board board = new Board("r3k2r/p2p1ppp/bpp1pq2/1B1Nn3/1b1Nn3/BPP1PQ2/P2P1PPP/R3K2R w");
            Board board = new Board();

            while (true)
            {
                Console.Clear();
                Console.WriteLine(GetBoardText(board));

                board.Move(Console.ReadLine());
            }
        }

        static string GetBoardText(Board board)
        {
            string txt = $" {board["a8"].PieceName} | {board["b8"].PieceName} | {board["c8"].PieceName} | {board["d8"].PieceName} | {board["e8"].PieceName} | {board["f8"].PieceName} | {board["g8"].PieceName} | {board["h8"].PieceName} \n" +
                $"_______________________________\n" +
                         $" {board["a7"].PieceName} | {board["b7"].PieceName} | {board["c7"].PieceName} | {board["d7"].PieceName} | {board["e7"].PieceName} | {board["f7"].PieceName} | {board["g7"].PieceName} | {board["h7"].PieceName} \n" +
                $"_______________________________\n" +
                         $" {board["a6"].PieceName} | {board["b6"].PieceName} | {board["c6"].PieceName} | {board["d6"].PieceName} | {board["e6"].PieceName} | {board["f6"].PieceName} | {board["g6"].PieceName} | {board["h6"].PieceName} \n" +
                $"_______________________________\n" +
                         $" {board["a5"].PieceName} | {board["b5"].PieceName} | {board["c5"].PieceName} | {board["d5"].PieceName} | {board["e5"].PieceName} | {board["f5"].PieceName} | {board["g5"].PieceName} | {board["h5"].PieceName} \n" +
                $"_______________________________\n" +
                         $" {board["a4"].PieceName} | {board["b4"].PieceName} | {board["c4"].PieceName} | {board["d4"].PieceName} | {board["e4"].PieceName} | {board["f4"].PieceName} | {board["g4"].PieceName} | {board["h4"].PieceName} \n" +
                $"_______________________________\n" +
                         $" {board["a3"].PieceName} | {board["b3"].PieceName} | {board["c3"].PieceName} | {board["d3"].PieceName} | {board["e3"].PieceName} | {board["f3"].PieceName} | {board["g3"].PieceName} | {board["h3"].PieceName} \n" +
                $"_______________________________\n" +
                         $" {board["a2"].PieceName} | {board["b2"].PieceName} | {board["c2"].PieceName} | {board["d2"].PieceName} | {board["e2"].PieceName} | {board["f2"].PieceName} | {board["g2"].PieceName} | {board["h2"].PieceName} \n" +
                $"_______________________________\n" +
                         $" {board["a1"].PieceName} | {board["b1"].PieceName} | {board["c1"].PieceName} | {board["d1"].PieceName} | {board["e1"].PieceName} | {board["f1"].PieceName} | {board["g1"].PieceName} | {board["h1"].PieceName} \n";
            return txt;
        }
    }
}