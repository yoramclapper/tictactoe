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
            toMove (enum Marker)        : X or O
            result (enum GameResult)    : None, Xwins, Owins, Draw
            board (Array)               : index refers to field and value represents marker: 0 (empty), 1 (X), -1 (O)

            Indexed board:

                |   0   |   1   |   2   |
                |-------|-------|-------|
                |   3   |   4   |   5   |
                |-------|-------|-------|
                |   6   |   7   |   8   |
        
        Method 'GamePlay' implements the game and runs in 'Main'
        */

    private const int BOARD_SIZE = 9;
    private Marker toMove;
    private GameResult result;
    private int[] board = new int[BOARD_SIZE];

    private void InitGame()
    {
        this.toMove = Marker.X; // Player 'X' starts the game
        this.result = GameResult.None;
        this.board = new int[BOARD_SIZE]; // Default value of array is 0
    }

    private string? PromptInput()
    {
        return Console.ReadLine();
    }

    private int ValidateInput(string? inputString)
    {
        /*
            Checks if input from prompt is a valid move
            Returns validation code:
                0   : Move from input is legal
                1   : Invalid input - Field index is out of range
                2   : Invalid input - Field is non-empty
                3   : Invalid input - Input can't be parsed to int
                -1  : Player entered 'exit' to abort game
        */

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

    private void FeedbackOnInput(int validationCode)
    {
        Console.WriteLine("\n");
        switch(validationCode)
        {
            case 0:
                Console.WriteLine("Enter square-index (0-8) to mark square on board:");
                break;
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
                Console.WriteLine("Unkown validation code: {0}", validationCode);
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
    
    private void DoMove(int inputField)
    {
        UpdateBoard(inputField);
        UpdateTurn();
    }

    private void CheckResult()
    {
        // Check rows and columns for win
        int horizontalWinFlag = 0; // Set to 1 when 3-in-a-row on row or column
        for (int i = 0; i < 3; i++)
        {
            int row = this.board[3*i] + this.board[3*i+1] + this.board[3*i+2];
            int col = this.board[i] + this.board[i+3] + this.board[i+6];
            
            if (row == 3 || col == 3)
            {
                this.result = GameResult.Xwins;
                horizontalWinFlag = 1;
            }
            
            else if (row == -3 || col == -3)
            {
                this.result = GameResult.Owins;
                horizontalWinFlag = 1;
            }
        }

        // Check diagonals for win
        int diagonalWinFlag = 0; // Set to 1 when 3-in-a-row on a diagonal
        int diag1 = this.board[0] + this.board[4] + this.board[8];
        int diag2 = this.board[6] + this.board[4] + this.board[2];
        
        if (diag1 == 3 || diag2 == 3)
        {
            this.result = GameResult.Xwins;
            diagonalWinFlag = 1;
        }

        else if (diag1 == -3 || diag2 == -3)
        {
            this.result = GameResult.Owins;
            diagonalWinFlag = 1;
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

        if (emptyFieldFlag == 0 && horizontalWinFlag == 0 && diagonalWinFlag == 0)
        {
            this.result = GameResult.Draw;
        }
    }

    private string FormatMarker (int i)
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

    private string FormatResult()
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

    private void DisplayGame()
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

    public void PlayGame()
    {
        Console.WriteLine("Start new game\n");
        this.InitGame(); // Initialize new game
        this.DisplayGame();

        // Continues until result on board or no empty squares left
        int moveCounter = 0;
        while (this.result == GameResult.None) // && moveCounter < BOARD_SIZE )
        {
            FeedbackOnInput(0);
            string? input = this.PromptInput();
            int validationCode = this.ValidateInput(input);
            
            // Continues to prompt for move until valid move or 'exit' is entered
            while (validationCode > 0)
            {
                this.FeedbackOnInput(validationCode);
                input = this.PromptInput();
                validationCode = this.ValidateInput(input);
            }
            
            // Player entered 'exit' => game is aborted
            if (validationCode == -1)
            {
                break;
            }

            // Process entered move...
            this.DoMove(int.Parse(input));
            this.CheckResult();
            this.DisplayGame();
            ++moveCounter;
        }
        Console.WriteLine("\nGame ended");
    }

    static void Main()
    {
        Game TicTacToe = new();
        TicTacToe.PlayGame();
    }
}
