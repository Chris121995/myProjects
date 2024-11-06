using System;

class TicTacToe
{
    static char[,] board = {
        { '1', '2', '3' },
        { '4', '5', '6' },
        { '7', '8', '9' }
    };

    static void DisplayBoard()
    {
        Console.Clear();
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Console.Write(" {0} ", board[i, j]);
                if (j < 2) Console.Write("|");
            }
            Console.WriteLine();
            if (i < 2) Console.WriteLine("---|---|---");
        }
    }

    static char CheckWin()
    {
        for (int i = 0; i < 3; i++)
        {
            if (board[i, 0] == board[i, 1] && board[i, 1] == board[i, 2])
                return board[i, 0];
            if (board[0, i] == board[1, i] && board[1, i] == board[2, i])
                return board[0, i];
        }

        if (board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2])
            return board[0, 0];
        if (board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0])
            return board[0, 2];

        return ' ';
    }

    static bool IsMovesLeft()
    {
        foreach (char cell in board)
        {
            if (cell != 'X' && cell != 'O')
                return true;
        }
        return false;
    }

    static int Minimax(char[,] board, int depth, bool isMaximizing)
    {
        char winner = CheckWin();
        if (winner == 'X') return -10 + depth;
        if (winner == 'O') return 10 - depth;
        if (!IsMovesLeft()) return 0;

        if (isMaximizing)
        {
            int bestScore = int.MinValue;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[i, j] != 'X' && board[i, j] != 'O')
                    {
                        char temp = board[i, j];
                        board[i, j] = 'O';
                        int score = Minimax(board, depth + 1, false);
                        board[i, j] = temp;
                        bestScore = Math.Max(bestScore, score);
                    }
                }
            }
            return bestScore;
        }
        else
        {
            int bestScore = int.MaxValue;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[i, j] != 'X' && board[i, j] != 'O')
                    {
                        char temp = board[i, j];
                        board[i, j] = 'X';
                        int score = Minimax(board, depth + 1, true);
                        board[i, j] = temp;
                        bestScore = Math.Min(bestScore, score);
                    }
                }
            }
            return bestScore;
        }
    }

    static (int, int) FindBestMove()
    {
        int bestScore = int.MinValue;
        int bestRow = -1, bestCol = -1;

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (board[i, j] != 'X' && board[i, j] != 'O')
                {
                    char temp = board[i, j];
                    board[i, j] = 'O';
                    int moveScore = Minimax(board, 0, false);
                    board[i, j] = temp;

                    if (moveScore > bestScore)
                    {
                        bestScore = moveScore;
                        bestRow = i;
                        bestCol = j;
                    }
                }
            }
        }
        return (bestRow, bestCol);
    }

    static void Main()
    {
        char currentPlayer = 'X';

        while (IsMovesLeft())
        {
            DisplayBoard();
            if (currentPlayer == 'X')
            {
                Console.Write("Spieler X, bitte wähle ein Feld (1-9): ");
                int move;
                if (int.TryParse(Console.ReadLine(), out move) && move >= 1 && move <= 9)
                {
                    int row = (move - 1) / 3;
                    int col = (move - 1) % 3;

                    if (board[row, col] != 'X' && board[row, col] != 'O')
                    {
                        board[row, col] = 'X';
                        currentPlayer = 'O';
                    }
                    else
                    {
                        Console.WriteLine("Ungültiger Zug, bitte erneut versuchen.");
                    }
                }
                else
                {
                    Console.WriteLine("Ungültige Eingabe, bitte eine Zahl zwischen 1 und 9 eingeben.");
                }
            }
            else
            {
                var (row, col) = FindBestMove();
                Console.WriteLine($"KI wählt Feld {row * 3 + col + 1}");
                board[row, col] = 'O';
                currentPlayer = 'X';
            }

            char winner = CheckWin();
            if (winner != ' ')
            {
                DisplayBoard();
                Console.WriteLine($"Spieler {winner} hat gewonnen!");
                return;
            }
        }

        DisplayBoard();
        Console.WriteLine("Unentschieden!");
    }
}
