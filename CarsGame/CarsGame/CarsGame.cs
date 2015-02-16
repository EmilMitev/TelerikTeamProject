using System;
using System.Collections.Generic;
using System.Threading;

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
    static int newCarRow = 0;
    static int newCarCol = 0;
    static Random rand = new Random();

    static char[,] enemyCar = new char[,]
    {
            { ' ',' ',' ',' ',' ',' ',' ',' ','_','_','_',' ',' ',' ',' ',' ',' ','_','_','_',' ',' ',' ',' ',' ',' ',' ' },
            { ' ',' ',' ','_',' ',' ',' ','[','_',' ','_',']',' ',' ',' ',' ','[','_',' ','_',']',' ',' ',' ',' ',' ',' ' },
            { ' ',' ','/',' ','|',' ',' ','_','_','$','_','_','_','_','_','_','_','_','S','_','_',' ',' ','|','\\',' ',' ' },
            { ' ','+','|',' ','+','+',']',' ',' ','_','_','_','_',' ',' ',' ',' ',' ',' ',' ',' ','\\','-','|',' ','\\',' ' },
            { ' ','|','<','o','o','o','>','(','0','_','_','_','_','<','|',' ','|','>','-','-','-','>','>','>','>','>',' ' },
            { ' ','+','|',' ','+','+',']','_','_',' ','_','_','_','_','_','_','_','_',' ','_','_','/','-','|',' ','/',' ' },
            { ' ',' ','\\','_','|',' ',' ',' ','_','$','_',' ',' ',' ',' ',' ',' ','_','S','_',' ',' ',' ','|','/',' ',' ' },
            { ' ',' ',' ',' ',' ',' ',' ','[','_','_','_',']',' ',' ',' ',' ','[','_','_','_',']',' ',' ',' ',' ',' ',' ' }
    };

    static void Main()
    {
        double speed;
        double acceleration;
        int playfieldWidth;
        int livesCount;


        Console.BufferHeight = Console.WindowHeight = 35;
        Console.BufferWidth = Console.WindowWidth = 170;

        while (true)
        {
            MoveUserCar();
            NewCar(newCarRow, newCarCol);
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
            Console.Clear();

        }
    }

    //Този метод го ползваме за принтиране на количките и текста
    static void PrintOnPosition(int x, int y, char c, ConsoleColor color = ConsoleColor.Gray)
    {

    }

    //след като свършат животите ползваме този метод.
    static void PrintStringOnPosition(int x, int y, string str, ConsoleColor color = ConsoleColor.Gray)
    {

    }

    // това е колата на user-a
    static void MoveUserCar()
    {

    }

    //този е за създаване на нова кола която пада
    static void NewCar(int row, int col)
    {
        for (int x = 0; x < enemyCar.GetLength(1); x++)
        {
            for (int y = 0; y < enemyCar.GetLength(0); y++)
            {
                Console.SetCursorPosition(x + col, y + row);
                Console.Write(enemyCar[y, x]);
            }
        }
        newCarCol = newCarCol + 2;
        if (newCarCol >= 142)
        {
            newCarCol = 0;

            switch (rand.Next(3))
            {
                case 1: newCarRow = 0; break;
                case 2: newCarRow = 8; break;
                case 3: newCarRow = 16; break;
            }
        }

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
    }
}