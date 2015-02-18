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
    public char[,] c; // какво тук е символ нашето ще е масив от символи
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
    static List<Object> objects = new List<Object>();
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
            NewCar();
            OldCar();
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
            foreach (Object car in objects)
            {
                PrintOnPosition(enemyCar, car.y, car.x, ConsoleColor.Red); 
            }
            DrawInfo();
            Thread.Sleep(70);
            Console.Clear();
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

        Console.CursorVisible = false;
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
    static void NewCar()
    {
        int chance = random.Next(0, 100);
        int carsOnLine = 1;
        int[] laneY = { 0, 8, 16 };
        int randomIndexLaneY = random.Next(0, laneY.Length);
        if (chance > 2)
        {
            carsOnLine = 0;
        }
        for (int i = 1; i <= carsOnLine; i++)
        {
            Object newCar = new Object();
            newCar.color = ConsoleColor.Yellow;
            newCar.x = 0;
            newCar.y = laneY[randomIndexLaneY];
            newCar.c = enemyCar;

            objects.Add(newCar);
        }
    }

    //проверява дали количките са се ударили
    static bool HittingCars()
    {

        return true;// трябва да се създаде някаква промелнива която да връща bool дали са се ударили или не.
    }

    static void OldCar()
    {
        List<Object> newList = new List<Object>();
        for (int i = 0; i < objects.Count; i++)
        {
            Object oldCar = objects[i];
            Object newObject = new Object();
            newObject.x = oldCar.x + 1;
            newObject.y = oldCar.y;
            newObject.c = oldCar.c;
            newObject.color = oldCar.color;
            if (newObject.x < GameWidth-1)
            {
                newList.Add(newObject);
            }

        }
        objects = newList;

    }

    //този е за принтиране на инфото за животите, ускорението и т.н.
    static void DrawInfo()
    {

    }
}