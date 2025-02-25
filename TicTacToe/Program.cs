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
using System.Runtime.ExceptionServices;

enum GameState {
    XToPlay,
    OToPlay,
    XWins,
    OWins,
    Draw
};

class Game
{
    public const int BOARD_SIZE = 3;

    public int[,] board = new int[BOARD_SIZE, BOARD_SIZE];
    public GameState state;

    public void InitGame()
    {
        // First player 'X' makes the first move
        state = GameState.XToPlay;

        // Initialize empty board
        int i ;
        for (i = 0 ; i < BOARD_SIZE ; ++i)
        {
            int j ;
            for (j = 0 ; j < BOARD_SIZE ; ++j)
            {
                board[i,j] = 0;
            }
        }    
    }

    public void DisplayGame()
    {
        Console.WriteLine("\nCurrent game state: {0}\n", state);
        Console.WriteLine("Current position:\n");

        if (BOARD_SIZE == 3)
        {
            Console.WriteLine("| {0} | {1} | {2} |", board[0,0], board[0,1], board[0,2]);
            Console.WriteLine("|---|---|---|");
            Console.WriteLine("| {0} | {1} | {2} |", board[1,0], board[1,1], board[1,2]);
            Console.WriteLine("|---|---|---|");
            Console.WriteLine("| {0} | {1} | {2} |", board[2,0], board[2,1], board[2,2]);
        } 
        else 
        {
            Console.WriteLine("Board display not implemented for board-size: {0}", BOARD_SIZE);
        }
    }

    static void Main()
    {
        Game TicTacToe = new();
        TicTacToe.InitGame();
        TicTacToe.DisplayGame();
    }
}
