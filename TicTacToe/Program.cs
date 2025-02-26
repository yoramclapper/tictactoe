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
        Terminate game
        Make move
        Check win
        Check draw
        Update state
        Update turn
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
    public const int BOARD_SIZE = 9;

    public bool[] emptyFields = new bool[BOARD_SIZE];
    public bool[] fieldsWithX = new bool[BOARD_SIZE];
    public bool[] fieldsWithO = new bool[BOARD_SIZE];
    public GameState state;

    public void InitGame()
    {
        // First player 'X' makes the first move
        state = GameState.XToPlay;

        // Initialize field values
        int i;
        for (i = 0; i < BOARD_SIZE; ++i)
        {
            emptyFields[i] = true;
            fieldsWithX[i] = false;
            fieldsWithO[i] = false;
        }
    }

    public int MakeMove(int i)
    {
        if (0 <= i && i < BOARD_SIZE)
        {
            if (emptyFields[i])
            {
                if (state == GameState.XToPlay)
                {
                    fieldsWithX[i] = true;
                    emptyFields[i] = false;
                    return 0;
                }

                else if (state == GameState.OToPlay)
                {
                    fieldsWithO[i] = true;
                    emptyFields[i] = false;
                    return 0;
                }

                else
                {
                    Console.WriteLine("Invalid move: move not possible during game state '{0}'", state);
                    return 1;
                }
            }

            else
            {
                Console.WriteLine("Invalid move: target field '{0}' is not empty", i);
                return 1; 
            }
        }

        else
        {
            Console.WriteLine("Invalid move: target field '{0}' does not exist", i);
            return 1;
        }
    }

    public string GetTokenOnField (int i)
    {
        if (emptyFields[i]) 
        {
            return " ";
        }
        
        else if (fieldsWithX[i])
        {
            return "X";
        }

        else if (fieldsWithO[i])
        {
            return "O";
        }

        else 
        {
            return "?";
        };
    }

    public string FormatState()
    {
            return state switch
        {
            GameState.XToPlay => "Turn of player 'X'",
            GameState.OToPlay => "Turn of player 'O'",
            GameState.XWins => "Player 'X' wins",
            GameState.OWins => "Player 'O' wins",
            GameState.Draw => "The game is a draw",
            _ => "Unknown game state",
        };    
    }

    public void DisplayGame()
    {
        Console.WriteLine("\nCurrent game state: {0}\n", FormatState());
        Console.WriteLine("Current position:\n");
        Console.WriteLine("| {0} | {1} | {2} |", GetTokenOnField(0), GetTokenOnField(1), GetTokenOnField(2));
        Console.WriteLine("|---|---|---|");
        Console.WriteLine("| {0} | {1} | {2} |", GetTokenOnField(3), GetTokenOnField(4), GetTokenOnField(5));
        Console.WriteLine("|---|---|---|");
        Console.WriteLine("| {0} | {1} | {2} |", GetTokenOnField(6), GetTokenOnField(7), GetTokenOnField(8));
    }

    static void Main()
    {
        Game TicTacToe = new();
        TicTacToe.InitGame();
        TicTacToe.DisplayGame();
    }
}
