using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

class CarsGame
{
    const int RaceWidth = 70;
    const int RaceHeight = 28;
    const int InfoPanelHeight = 10;
    const int GameWidth = RaceWidth;
    const int GameHeight = RaceHeight + InfoPanelHeight;
    static char[,] userCar = new char[,]
    {
            { ' ',' ',' ',' ',' ',' ',' ',' ','_','_','_',' ',' ',' ',' ',' ',' ','_','_','_',' ',' ',' ',' ',' ',' ',' ' },
            { ' ',' ',' ',' ',' ',' ',' ','[','_',' ','_',']',' ',' ',' ',' ','[','_',' ','_',']',' ',' ',' ','_',' ',' ' },
            { ' ',' ','/','|',' ',' ','_','_','_','$','_','_','_','_','_','_','_','_','S','_',' ',' ',' ','|',' ','\\',' ' },
            { ' ','/',' ','|','-','/',' ',' ',' ',' ',' ',' ',' ',' ','_','_','_','_',' ',' ','[','+','+','|',' ','|','+' },
            { '<','<','<','<','<','-','-','-','<','|',' ',' ','|','>','_','_','_','_','O',')','<','o','o','o','>','|',' ' },
            { ' ','\\',' ','|','-','\\','_','_','_',' ','_','_','_','_','_','_','_','_',' ','_','[','+','+','|',' ','|','+' },
            { ' ',' ','\\','|',' ',' ',' ',' ','_','$','_',' ',' ',' ',' ',' ',' ','_','S','_',' ',' ',' ','|','_','/',' ' },
            { ' ',' ',' ',' ',' ',' ',' ','[','_','_','_',']',' ',' ',' ',' ','[','_','_','_',']',' ',' ',' ',' ',' ',' ' }

    };
    static char[,] obstacle = new char[,]
    {
        { '#','#' },{ '#','#' },{ '#','#' },{ '#','#' },{ '#','#' },{ '#','#' },{ '#','#' },{ '#','#' }
    };
    static int currUserCarRow = 0;
    static int currUserCarCol = 43;
    static int obstRow = 0;
    static int obstCol = 0;

    static Random random = new Random();

    static void PrintFigure(char[,] figure, int row, int col, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        for (int x = 0; x < figure.GetLength(0); x++)
        {
            for (int y = 0; y < figure.GetLength(1); y++)
            {
                Print(row + x, col + y, figure[x, y]);
            }
        }
    }



    static void Print(int row, int col, object data)
    {
        Console.SetCursorPosition(col, row);
        Console.Write(data);
    }

    static void Main()
    {
        Console.Title = "Car Race F1";
        Console.WindowWidth = GameWidth;
        Console.BufferWidth = GameWidth;
        Console.WindowHeight = GameHeight + 1;
        Console.BufferHeight = GameHeight + 1;

        while (true)
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.UpArrow)
                {
                    if (currUserCarRow > 1)
                    {
                        currUserCarRow = currUserCarRow - 8;
                    }
                }
                else if (key.Key == ConsoleKey.DownArrow)
                {
                    if (currUserCarRow + 8 - 1 < RaceHeight - 8)
                    {
                        currUserCarRow = currUserCarRow + 8;
                    }
                }
            }
            if (obstCol < RaceWidth - 2)
            {
                obstCol = obstCol + 2;
            }
            else
            {
                obstCol = 0;
                if (obstRow < 16)
                {
                    obstRow = obstRow + 8;
                }
                else
                {
                    obstRow = obstRow - 8;
                }
            }
            PrintFigure(userCar, currUserCarRow, currUserCarCol, ConsoleColor.Cyan);
            PrintFigure(obstacle, obstRow, obstCol, ConsoleColor.Red);
            Thread.Sleep(50);
            Console.Clear();
        }

    }
}

