using System;
using System.Threading;

class Tetris
{
    static int boardWidth = 10;
    static int boardHeight = 20;
    static char[,] board = new char[boardHeight, boardWidth];
    static int currentX = boardWidth / 2;
    static int currentY = 0;
    static char tetromino = 'O';

    static char[,] tetrominoShape =
    {
        { ' ', 'O', ' ', ' ' },
        { ' ', 'O', ' ', ' ' },
        { ' ', 'O', 'O', ' ' },
        { ' ', ' ', ' ', ' ' }
    };

    static void Main()
    {
        Console.WindowHeight = boardHeight + 1;
        Console.WindowWidth = boardWidth * 2;
        Console.BufferHeight = Console.WindowHeight;
        Console.BufferWidth = Console.WindowWidth;
        Console.CursorVisible = false;

        InitializeGame();

        while (true)
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.LeftArrow:
                        MoveTetromino(-1, 0);
                        break;
                    case ConsoleKey.RightArrow:
                        MoveTetromino(1, 0);
                        break;
                    case ConsoleKey.DownArrow:
                        if (!MoveTetromino(0, 1))
                        {
                            // Si no se puede mover más abajo, detén el juego.
                            return;
                            //Console.ReadKey();
                        }
                        break;
                }
            }

            while (true)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;
                    switch (key)
                    {
                        case ConsoleKey.LeftArrow:
                            MoveTetromino(-1, 0);
                            break;
                        case ConsoleKey.RightArrow:
                            MoveTetromino(1, 0);
                            break;
                        case ConsoleKey.DownArrow:
                            if (!MoveTetromino(0, 1))
                            {
                                // Si no se puede mover más abajo, detén el juego.
                                return;
                            }
                            break;
                        case ConsoleKey.R:
                            RotateTetromino();
                            break;
                    }
                }

                Thread.Sleep(300); // Controla la velocidad de caída del tetromino.
                Console.ReadKey();
            }
        }

    }

    static void InitializeGame()
    {
        InitializeBoard();
        InitializeTetromino();
        DrawBoard();
        RotateTetromino();
    }

    static void InitializeBoard()
    {
        for (int y = 0; y < boardHeight; y++)
        {
            for (int x = 0; x < boardWidth; x++)
            {
                board[y, x] = ' ';
            }
        }
    }

    static void InitializeTetromino()
    {
        for (int y = 0; y < tetrominoShape.GetLength(0); y++)
        {
            for (int x = 0; x < tetrominoShape.GetLength(1); x++)
            {
                if (tetrominoShape[y, x] == 'O')
                {
                    board[y, x + currentX] = tetromino;
                }
            }
        }
    }

    static void DrawBoard()
    {
        Console.SetCursorPosition(0, 0);
        for (int y = 0; y < boardHeight; y++)
        {
            for (int x = 0; x < boardWidth; x++)
            {
                Console.Write(board[y, x]);
                Console.Write(' ');
            }
            Console.WriteLine();
        }
    }

    static bool MoveTetromino(int deltaX, int deltaY)
    {
        // Borra la posición actual del tetromino en el tablero.
        for (int y = 0; y < tetrominoShape.GetLength(0); y++)
        {
            for (int x = 0; x < tetrominoShape.GetLength(1); x++)
            {
                if (tetrominoShape[y, x] == 'O')
                {
                    board[y + currentY, x + currentX] = ' ';
                }
            }
        }

        // Calcula la nueva posición.
        int newX = currentX + deltaX;
        int newY = currentY + deltaY;

        // Verifica si la nueva posición es válida.
        if (IsMoveValid(newX, newY))
        {
            // Actualiza la posición del tetromino.
            currentX = newX;
            currentY = newY;

            // Dibuja el tetromino en la nueva posición.
            for (int y = 0; y < tetrominoShape.GetLength(0); y++)
            {
                for (int x = 0; x < tetrominoShape.GetLength(1); x++)
                {
                    if (tetrominoShape[y, x] == 'O')
                    {
                        board[y + currentY, x + currentX] = tetromino;
                    }
                }
            }

            // Dibuja el tablero actualizado.
            DrawBoard();

            return true;
        }

        return false;
    }

    static bool IsMoveValid(int newX, int newY)
    {
        for (int y = 0; y < tetrominoShape.GetLength(0); y++)
        {
            for (int x = 0; x < tetrominoShape.GetLength(1); x++)
            {
                if (tetrominoShape[y, x] == 'O')
                {
                    int boardX = x + newX;
                    int boardY = y + newY;

                    if (boardX < 0 || boardX >= boardWidth || boardY >= boardHeight)
                    {
                        return false; // Movimiento inválido
                        //Se utilizan las flechas para realizar el movimiento sensillo, por el momento selo se puede bajas
                    }
                }
            }
        }

        return true;
    }
    static void RotateTetromino()
    {
        // Copia la forma actual del tetromino antes de la rotación.
        char[,] originalShape = new char[tetrominoShape.GetLength(1), tetrominoShape.GetLength(0)];

        for (int y = 0; y < tetrominoShape.GetLength(0); y++)
        {
            for (int x = 0; x < tetrominoShape.GetLength(1); x++)
            {
                originalShape[x, y] = tetrominoShape[y, x];
            }
        }

        // Limpia la posición actual del tetromino en el tablero.
        for (int y = 0; y < tetrominoShape.GetLength(0); y++)
        {
            for (int x = 0; x < tetrominoShape.GetLength(1); x++)
            {
                if (tetrominoShape[y, x] == 'O')
                {
                    board[y + currentY, x + currentX] = ' ';
                }
            }
        }

        // Calcula la nueva forma del tetromino después de la rotación.
        int newWidth = originalShape.GetLength(0);
        int newHeight = originalShape.GetLength(1);
        char[,] newShape = new char[newHeight, newWidth];

        for (int y = 0; y < newHeight; y++)
        {
            for (int x = 0; x < newWidth; x++)
            {
                newShape[y, x] = originalShape[newHeight - 1 - y, x];
            }
        }

        // Actualiza la forma del tetromino y verifica si la rotación es válida.
        tetrominoShape = newShape;
        if (!IsMoveValid(currentX, currentY))
        {
            // Si la rotación no es válida, restaura la forma original del tetromino.
            tetrominoShape = originalShape;
        }

        // Dibuja el tetromino en la nueva posición.
        for (int y = 0; y < tetrominoShape.GetLength(0); y++)
        {
            for (int x = 0; x < tetrominoShape.GetLength(1); x++)
            {
                if (tetrominoShape[y, x] == 'O')
                {
                    board[y + currentY, x + currentX] = tetromino;
                }
            }
        }

        // Dibuja el tablero actualizado.
        DrawBoard();
    }

}
