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
    static char[,] userCarArr = new char[,]
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
    static List<Object> objects = new List<Object>();
    static Object userCar = new Object();
    static Object newObject = new Object();

    static Random random = new Random();

    static int speed = 0;
    static int acceleration = 2;
    static int livesCount = 3;


    static void Main()
    {

        Console.Title = "Car Race F1";
        Console.WindowWidth = GameWidth;
        Console.BufferWidth = GameWidth;
        Console.WindowHeight = GameHeight + 1;
        Console.BufferHeight = GameHeight + 1;

        newObject.x = 40;
        userCar.c = userCarArr;

        while (true)
        {
            MoveUserCar();
            NewCar();
            speed = HittingCars();

            foreach (Object car in objects)
            {
                PrintOnPosition(enemyCar, car.y, car.x, ConsoleColor.Red);
            }
            ++speed;
            if (speed >= 90)
            {
                speed = 90;
            }
            DrawInfo(GameWidth, GameHeight, livesCount, acceleration, speed);
            Thread.Sleep(100 - speed);
            Console.Clear();
            Console.SetCursorPosition(GameWidth - 2, GameHeight - 2);
            Console.WriteLine(speed);
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
        userCar.x = 43;
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

        PrintOnPosition(userCar.c, userCar.y, userCar.x, ConsoleColor.Cyan);
    }

    // премества колата на user-a надолу
    static void MoveUserCarDown()
    {
        if (userCar.y + 8 - 1 < RaceHeight - 8)
        {
            userCar.y = userCar.y + 8;
        }
    }

    // премества колата на user-a нагоре
    static void MoveUserCarUp()
    {
        if (userCar.y > 1)
        {
            userCar.y = userCar.y - 8;
        }
    }

    //този е за създаване на нова кола която пада
    static void NewCar()
    {
        int chance = random.Next(0, 100);
        int carsOnLine = 1;
        int[] laneY = { 0, 8, 16 };
        int randomIndexLaneY = random.Next(0, laneY.Length);
        if (chance < 90)
        {
            Object newCar = new Object();
            newCar.color = ConsoleColor.Yellow;
            newCar.x = 0;
            newCar.y = laneY[randomIndexLaneY];
            newCar.c = enemyCar;
            if ((newObject.x - newCar.x) >= 40)
            {
                objects.Add(newCar);
            }
        }
        OldCar();
    }

    //проверява дали количките са се ударили
    static int HittingCars()
    {
        if ((newObject.y == userCar.y) && (newObject.x >= userCar.x))
        {
            speed += acceleration;
            objects.Clear();
        }
        return speed;
    }

    static void OldCar()
    {
        List<Object> newList = new List<Object>();
        for (int i = 0; i < objects.Count; i++)
        {
            Object oldCar = objects[i];
            newObject.x = oldCar.x + 1;
            newObject.y = oldCar.y;
            newObject.c = oldCar.c;
            HittingCars();
            newObject.color = oldCar.color;
            if (newObject.x < GameWidth - 1)
            {
                newList.Add(newObject);
            }

        }
        objects = newList;
    }

    //този е за принтиране на инфото за животите, ускорението и т.н.
    static void DrawInfo(int gameWidth, int gameHeight, double LiveCounter, double Acceleration, double Speed)
    {
        //liveCounter - брой на животи, != 0;
        int x = 0, y = 0;
        string str = string.Empty;
        ConsoleColor color = ConsoleColor.Gray;

        x = gameWidth / 2;
        y = gameHeight - 6;

        Console.ForegroundColor = color;

        if (LiveCounter > 0)
        {
            Console.SetCursorPosition(x, y);
            Console.Write("Lives remaining: " + LiveCounter);
            Console.WriteLine();
            Console.Write("Current Speed: " + Speed);
            Console.WriteLine();
            Console.Write("Current Acceleration: " + Acceleration);
            Console.WriteLine();
        }
        else
        {
            PrintStringOnPosition(x, y, "GAME OVER!");
        }

    }
}