﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

//създаване на обект
struct Object
{
    //тези данни показват:
    public int x; //къде по Х координатата
    public int y; //Къде по У координатата
    public char c; // какво тук е символ нашето ще е масив от символи
    public ConsoleColor color; //какъв цвят
}

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
    static char[,] enemyCar = new char[,]
    {
        { '#','#' },{ '#','#' },{ '#','#' },{ '#','#' },{ '#','#' },{ '#','#' },{ '#','#' },{ '#','#' }
    };
    static int currUserCarRow = 0;
    static int currUserCarCol = 43;
    static int newCarRow = 0;
    static int newCarCol = 0;
    static int count = 0;

    static Random random = new Random();

    static void Main()
    {
        double speed;
        double acceleration;
        int playfieldWidth;
        int livesCount;

        Console.Title = "Car Race F1";
        Console.WindowWidth = GameWidth;
        Console.BufferWidth = GameWidth;
        Console.WindowHeight = GameHeight + 1;
        Console.BufferHeight = GameHeight + 1;

        while (true)
        {
            MoveUserCar();
            NewCar(newCarRow, newCarCol);
            newCarCol = newCarCol + 2;
            HittingCars();
            if (HittingCars())
            {
                //трябва да се изчистят всичките колички дето падат
            }
            else
            {
                //PrintOnPosition();
            }
            //foreach (Object car in objects)
            //{
            //    //PrintOnPosition();
            //}
            DrawInfo();
        }
    }

    //Този метод го ползваме за принтиране на количките и текста на зададена позиция
    static void PrintOnPosition(char[,] figure, int row, int col, ConsoleColor color)
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

    //Метод, който принтира
    static void Print(int row, int col, object data)
    {
        Console.SetCursorPosition(col, row);
        Console.Write(data);
    }

    //след като свършат животите ползваме този метод.
    static void PrintStringOnPosition(int x, int y, string str, ConsoleColor color = ConsoleColor.Gray)
    {

    }

    // това е колата на user-a
    static void MoveUserCar()
    {
        if (Console.KeyAvailable)
        {
            var key = Console.ReadKey();
            if (key.Key == ConsoleKey.UpArrow)
            {
                MoveUserCarUp();
            }
            else if (key.Key == ConsoleKey.DownArrow)
            {
                MoveUserCarDown();
            }
        }

        PrintOnPosition(userCar, currUserCarRow, currUserCarCol, ConsoleColor.Cyan);
    }

    // премества колата на user-a надолу
    static void MoveUserCarDown()
    {
        if (currUserCarRow + 8 - 1 < RaceHeight - 8)
        {
            currUserCarRow = currUserCarRow + 8;
        }
    }

    // премества колата на user-a нагоре
    static void MoveUserCarUp()
    {
        if (currUserCarRow > 1)
        {
            currUserCarRow = currUserCarRow - 8;
        }
    }

    //този е за създаване на нова кола която пада
    static void NewCar(int row, int col)
    {
        if (col > RaceWidth - 2)
        {
            col = 0;
            newCarCol = 0;
            if (row < 16)
            {
                row += 8;
                newCarRow += 8;
            }


        }

        PrintOnPosition(enemyCar, row, col, ConsoleColor.Red);
    }

    //проверява дали количките са се ударили
    static bool HittingCars()
    {

        return true;// трябва да се създаде някаква промелнива която да връща bool дали са се ударили или не.
    }

    static void OldCar()
    {

    }

    //този е за принтиране на инфото за животите, ускорението и т.н.
    static void DrawInfo()
    {
        Thread.Sleep(50);
        Console.Clear();
    }
}