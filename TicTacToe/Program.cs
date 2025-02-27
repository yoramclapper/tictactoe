using System;
using System.IO.Pipelines;

enum Marker 
{
    X,
    O
};

enum GameResult 
{
    None,
    Xwins,
    Owins,
    Draw
};


class Game
{
    /*
    TicTacToe: Paper-and-pencil game for two players who take turns marking the fields in a three-by-three grid with X or O.

    Properties:
        toMove (Enum)          : X or O
        result (Enum)          : None, Xwins, Owins, Draw
        board (List<int>)      : index refers to field and value represents marker: 0 (empty), 1 (X), -1 (O)

        Indexed board:

            |   0   |   1   |   2   |
            |-------|-------|-------|
            |   3   |   4   |   5   |
            |-------|-------|-------|
            |   6   |   7   |   8   |

    Methods:
        InputMove              : Prompts user to select field (index)
        LegalMove              : Checks if move is legal
        DoMove                 : Updates board and toMove
        CheckResult            : Checks if there is a win or draw on the board
    */

    private const int BOARD_SIZE = 9;
    private Marker toMove;
    private GameResult result;
    private int[] board = new int[BOARD_SIZE];

    public void InitGame()
    {
        // Player 'X' starts the game
        this.toMove = Marker.X;
        this.result = GameResult.None;
        this.board = new int[BOARD_SIZE]; // Default value of array is 0
    }

    private int LegalMove(string inputMoveString)
    {
        try
        {
            int inputMove = int.Parse(inputMoveString);
            if (inputMove < 0 || inputMove >= BOARD_SIZE)
            {
                return 1; // Field-index out of range
            }

            else 
            {
                if (this.board[inputMove] != 0)
                {
                    return 1; // Field is not empty
                }

                else
                {
                    return 0; // Field is legal
                }
            }
        }

        catch
        {
            return 1; // Invalid input given
        }
    }

    // TODO: Players should be able to exit the game before game is over
    public int InputMove()
    {
        string inputMoveString;
        int inputMove;

        Console.WriteLine("Enter field to place your mark: ");
        inputMoveString = Console.ReadLine();
        
        while (LegalMove(inputMoveString) > 0)
        {
            Console.WriteLine("\nEntered move is not legal, re-enter move: ");
            inputMoveString = Console.ReadLine();
        }

        inputMove = int.Parse(inputMoveString);
        return inputMove;
    }
    
    private void UpdateBoard(int inputField)
    {
        int token;
        if (this.toMove == Marker.X)
        {
            token = 1;
        }

        else
        {
            token = -1;
        }

        this.board[inputField] = token;
    }

    private void UpdateTurn()
    {
        if (this.toMove == Marker.X)
        {
            this.toMove = Marker.O;
        }

        else
        {
            this.toMove = Marker.X;
        }
    }
    
    public void DoMove(int inputField)
    {
        UpdateBoard(inputField);
        UpdateTurn();
    }

    // TODO: Make search more efficient
    public void CheckResult()
    {
        if 
        (
            this.board[0] + this.board[1] + this.board[2] == 3
            || 
            this.board[3] + this.board[4] + this.board[5] == 3
            ||
            this.board[6] + this.board[7] + this.board[8] == 3
            ||
            this.board[0] + this.board[3] + this.board[6] == 3
            || 
            this.board[1] + this.board[4] + this.board[7] == 3
            ||
            this.board[2] + this.board[5] + this.board[8] == 3
            || 
            this.board[0] + this.board[4] + this.board[8] == 3
            ||
            this.board[2] + this.board[4] + this.board[6] == 3

        )
        {
            result = GameResult.Xwins;
        }

        else if 
        (
            this.board[0] + this.board[1] + this.board[2] == -3
            || 
            this.board[3] + this.board[4] + this.board[5] == -3
            ||
            this.board[6] + this.board[7] + this.board[8] == -3
            ||
            this.board[0] + this.board[3] + this.board[6] == -3
            || 
            this.board[1] + this.board[4] + this.board[7] == -3
            ||
            this.board[2] + this.board[5] + this.board[8] == -3
            || 
            this.board[0] + this.board[4] + this.board[8] == -3
            ||
            this.board[2] + this.board[4] + this.board[6] == -3

        )
        {
            result = GameResult.Owins;
        }

        else if
        (
            this.board[0] != 0
            &&
            this.board[1] != 0
            &&
            this.board[2] != 0
            &&
            this.board[3] != 0
            &&
            this.board[4] != 0
            &&
            this.board[5] != 0
            &&
            this.board[6] != 0
            &&
            this.board[7] != 0
            &&
            this.board[8] != 0
        )
        {
            result = GameResult.Draw;
        }
    }

    public string FormatMarker (int i)
    {
        int loc = this.board[i];
        if (loc == 0) 
        {
            return " ";
        }
        
        else if (loc == 1)
        {
            return "X";
        }

        else if (loc == -1)
        {
            return "O";
        }

        else 
        {
            return "?";
        };
    }

    public string FormatResult()
    {
        string marker;
        if (this.toMove == Marker.X)
        {
            marker = "X";
        }
        
        else
        {
            marker = "O";
        }
        
        return result switch
        {
            GameResult.None => "Turn of player " + marker,
            GameResult.Xwins => "Player X wins",
            GameResult.Owins => "Player 'O' wins",
            GameResult.Draw => "The game is a draw",
            _ => "Unknown game state",
        };    
    }

    public void DisplayGame()
    {
        Console.WriteLine("\nCurrent game state: {0}\n", FormatResult());
        Console.WriteLine("Current position:\n");
        Console.WriteLine("| {0} | {1} | {2} |", FormatMarker(0), FormatMarker(1), FormatMarker(2));
        Console.WriteLine("|---|---|---|");
        Console.WriteLine("| {0} | {1} | {2} |", FormatMarker(3), FormatMarker(4), FormatMarker(5));
        Console.WriteLine("|---|---|---|");
        Console.WriteLine("| {0} | {1} | {2} |", FormatMarker(6), FormatMarker(7), FormatMarker(8));
        Console.WriteLine("\n");
    }

    static void Main()
    {
        Console.WriteLine("Start new game");
        Game TicTacToe = new();
        TicTacToe.InitGame();

        int moveCounter = 0;
        while (TicTacToe.result == GameResult.None && moveCounter < 9 )
        {
            int inputMove = TicTacToe.InputMove();
            TicTacToe.DoMove(inputMove);
            TicTacToe.CheckResult();
            TicTacToe.DisplayGame();
            ++moveCounter;
        }
        Console.WriteLine("Game ended");
    }
}
