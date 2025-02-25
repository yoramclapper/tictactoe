/*
Requirements:

    Board       : 3x3 (empty squares)
    Players     : 2-players represented by 'X' and 'O', respectively
    Gameplay    : Turn-based
                : Each turn player must place token on empty square of board
                : First to three-in-a-row (horizontal, vertical, diagonal) wins
                : No empty squares left and no three-in-a-row draws 

    Example board:

    0   |   O   |   O   |   X   |
        |-------|-------|-------|
    1   |   X   |   O   |       |
        |-------|-------|-------|
    2   |   X   |   O   |       |
            0       1       2

Components:

    Board (class)

    Properties:
        Game state: Play, Win, Draw
        Board representation (2D-array)
        Turn: X or O

    Methods:
        Initialize game
        Terminate game
        Make move
        Check win
        Check draw
        Update state
        Update turn
        Display board
*/

using System;

class Game
{
    public const int BOARD_SIZE = 3;

    public int[,] Board = new int[BOARD_SIZE, BOARD_SIZE];
    public int PlayerTurn;

    public void InitGame()
    {
        // Initialize empty board
        int i ;
        for (i = 0 ; i < BOARD_SIZE ; ++i)
        {
            int j ;
            for (j = 0 ; j < BOARD_SIZE ; ++j)
            {
                Board[i,j] = 0;
            }
        }

        // Player 1 makes the first move
        PlayerTurn = 1;     
    }

    public void DisplayGame()
    {
        Console.WriteLine("\nTurn of player: {0}\n", PlayerTurn);
        Console.WriteLine("Current position:\n");

        if (BOARD_SIZE == 3)
        {
            Console.WriteLine("| {0} | {1} | {2} |", Board[0,0], Board[0,1], Board[0,2]);
            Console.WriteLine("|---|---|---|");
            Console.WriteLine("| {0} | {1} | {2} |", Board[1,0], Board[1,1], Board[1,2]);
            Console.WriteLine("|---|---|---|");
            Console.WriteLine("| {0} | {1} | {2} |", Board[2,0], Board[2,1], Board[2,2]);
        } 
        else 
        {
            Console.WriteLine("Board display not implemented for size: {0}", BOARD_SIZE);
        }
    }

    static void Main()
    {
        Game TicTacToe = new();
        TicTacToe.InitGame();
        TicTacToe.DisplayGame();
    }
}
