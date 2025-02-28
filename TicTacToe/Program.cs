using System;
using System.Data;
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

    public string PromptInput()
    {
        return Console.ReadLine();
    }

    public int ValidateInput(string inputString)
    {
        try
        {
            int inputMove = int.Parse(inputString);
            if (inputMove < 0 || inputMove >= BOARD_SIZE)
            {
                return 1; // Field index does not exist
            }

            else 
            {
                if (this.board[inputMove] != 0)
                {
                    return 2; // Field is not empty
                }

                else
                {
                    return 0; // Legal move
                }
            }
        }

        catch
        {
            if (inputString == "exit")
            {
                return -1; // Abort game
            }
            else
            {
                return 3; // Invalid input given
            }    
        }
    }

    public void FeedbackOnInput(int validationCode)
    {
        switch(validationCode)
        {
            case 1:
                Console.WriteLine("Entered square is out of range, re-enter move:");
                break;
            case 2:
                Console.WriteLine("Entered square is not empty, re-enter move:");
                break;
            case 3:
                Console.WriteLine("Entered input is invalid, re-enter move:");
                break;
            default:
                Console.WriteLine("Enter square-index (0-8) to mark square on board:");
                break;
        }
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

    public void CheckResult()
    {
        // Check rows and columns for win
        for (int i = 0; i < 3; i++)
        {
            int row = this.board[3*i] + this.board[3*i+1] + this.board[3*i+2];
            int col = this.board[i] + this.board[i+3] + this.board[i+6];
            
            if (row == 3 || col == 3)
            {
                this.result = GameResult.Xwins;
            }
            
            else if (row == -3 || col == -3)
            {
                this.result = GameResult.Owins;
            }
        }

        // Check diagonals for win
        int diag1 = this.board[0] + this.board[4] + this.board[8];
        int diag2 = this.board[6] + this.board[4] + this.board[2];
        
        if (diag1 == 3 || diag2 == 3)
        {
            this.result = GameResult.Xwins;
        }

        else if (diag1 == -3 || diag2 == -3)
        {
            this.result = GameResult.Owins;
        }

        // Check if there are empty fields still
        int emptyFieldFlag = 0;
        for (int i = 0; i < BOARD_SIZE; i++)
        {
            if (this.board[i] == 0)
            {
                emptyFieldFlag = 1;
                break;
            }
        }

        if (emptyFieldFlag == 0)
        {
            this.result = GameResult.Draw;
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
            TicTacToe.FeedbackOnInput(0);
            string input = TicTacToe.PromptInput();
            int validationCode = TicTacToe.ValidateInput(input);
            while (validationCode > 0)
            {
                TicTacToe.FeedbackOnInput(validationCode);
                input = TicTacToe.PromptInput();
                validationCode = TicTacToe.ValidateInput(input);
            }
            if (validationCode == -1)
            {
                Console.WriteLine("Game aborted");
                break;
            }
            TicTacToe.DoMove(int.Parse(input));
            TicTacToe.CheckResult();
            TicTacToe.DisplayGame();
            ++moveCounter;
        }
        Console.WriteLine("Game ended");
    }
}
