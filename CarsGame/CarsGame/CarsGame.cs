﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

//create object
struct Object
{
    public int x;
    public int y;
    public char[,] c;
    public ConsoleColor color;
}

class CarsGame
{
    const int RaceWidth = 150;
    const int RaceHeight = 28;
    const int InfoPanelHeight = 10;
    const int GameWidth = RaceWidth;
    const int GameHeight = RaceHeight + InfoPanelHeight;

    #region user car
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
    #endregion

    #region enemy car
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
    #endregion

    static List<Object> objects = new List<Object>();
    static Object userCar = new Object();
    static Object newObject = new Object();

    static Random random = new Random();

    static int speed = 0;
    static int acceleration = 2;
    static int livesCount = 3;
    static int score = 0;
    static int bestScore = 0;

    static void Main()
    {
        Console.Title = "Car Race F1";
        Console.WindowWidth = GameWidth;
        Console.BufferWidth = GameWidth;
        Console.WindowHeight = GameHeight + 1;
        Console.BufferHeight = GameHeight + 1;

        newObject.x = 40;
        userCar.c = userCarArr;

        ReadFromFile();
        

        while (true)
        {
            if (EndGame())
            {
                PrintStringOnPosition();
                break;
            }

            MoveUserCar();

            NewObject();

            if (HittingCars())
            {
                --livesCount;
            }
            if (score % 1500 == 0)
            {
                livesCount++;
            }
            foreach (Object car in objects)
            {
                PrintOnPosition(enemyCar, car.y, car.x, ConsoleColor.Red);
            }

            if (speed >= 90)
            {
                speed = 90;
            }
            else
            {
                ++speed;
            }

            score += 10;

            DrawInfo(GameWidth, GameHeight, livesCount, acceleration, speed);

            Thread.Sleep(100 - speed);
            Console.Clear();
        }

        if (score > bestScore)
        {
            WriteIntoFile();
        }
    }

    static void WriteIntoFile()
    {
        try
        {
            StreamWriter streamWriter = new StreamWriter("bestScore.txt", false);
            streamWriter.Write(score);
            streamWriter.Close();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }

    static void ReadFromFile()
    {
        try
        {
            StreamReader reader = new StreamReader("bestScore.txt");
            bestScore = int.Parse(reader.ReadLine());
            reader.Close();
        }
        catch (FileNotFoundException ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }

    //Print cars and text at position on console
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

    static void Print(int row, int col, object data)
    {
        Console.CursorVisible = false;
        Console.SetCursorPosition(col, row);
        Console.Write(data);
    }

    //After live is over write game over
    static void PrintStringOnPosition(ConsoleColor color = ConsoleColor.Red)
    {
        Console.Clear();
        Console.WriteLine(@"                     .... NO! ...                  ... MNO! ...
                       ..... MNO!! ...................... MNNOO! ...
                     ..... MMNO! ......................... MNNOO!! .
                    ..... MNOONNOO!   MMMMMMMMMMPPPOII!   MNNO!!!! .
                     ... !O! NNO! MMMMMMMMMMMMMPPPOOOII!! NO! ....
                        ...... ! MMMMMMMMMMMMMPPPPOOOOIII! ! ...
                       ........ MMMMMMMMMMMMPPPPPOOOOOOII!! .....
                       ........ MMMMMOOOOOOPPPPPPPPOOOOMII! ...
                        ....... MMMMM..    OPPMMP    .,OMI! ....
                         ...... MMMM::   o.,OPMP,.o   ::I!! ...
                             .... NNM:::.,,OOPM!P,.::::!! ....
                              .. MMNNNNNOOOOPMO!!IIPPO!!O! .....
                             ... MMMMMNNNNOO:!!:!!IPPPPOO! ....
                               .. MMMMMNNOOMMNNIIIPPPOO!! ......
                              ...... MMMONNMMNNNIIIOO!..........
                           ....... MN MOMMMNNNIIIIIO! OO ..........
                        ......... MNO! IiiiiiiiiiiiI OOOO ...........
                      ...... NNN.MNO! . O!!!!!!!!!O . OONO NO! ........
                       .... MNNNNNO! ...OOOOOOOOOOO .  MMNNON!........
                       ...... MNNNNO! .. PPPPPPPPP .. MMNON!........
                          ...... OO! ................. ON! .......
                             ................................
");
        Console.WriteLine(@"    $$$$$$\   $$$$$$\  $$\      $$\ $$$$$$$$\        $$$$$$\  $$\    $$\ $$$$$$$$\ $$$$$$$\  
    $$  __$$\ $$  __$$\ $$$\    $$$ |$$  _____|      $$  __$$\ $$ |   $$ |$$  _____|$$  __$$\ 
    $$ /  \__|$$ /  $$ |$$$$\  $$$$ |$$ |            $$ /  $$ |$$ |   $$ |$$ |      $$ |  $$ |
    $$ |$$$$\ $$$$$$$$ |$$\$$\$$ $$ |$$$$$\          $$ |  $$ |\$$\  $$  |$$$$$\    $$$$$$$  |
    $$ |\_$$ |$$  __$$ |$$ \$$$  $$ |$$  __|         $$ |  $$ | \$$\$$  / $$  __|   $$  __$$< 
    $$ |  $$ |$$ |  $$ |$$ |\$  /$$ |$$ |            $$ |  $$ |  \$$$  /  $$ |      $$ |  $$ |
    \$$$$$$  |$$ |  $$ |$$ | \_/ $$ |$$$$$$$$\        $$$$$$  |   \$  /   $$$$$$$$\ $$ |  $$ |
     \______/ \__|  \__|\__|     \__|\________|       \______/     \_/    \________|\__|  \__|

                                                                                          ");
        if (score > bestScore)
        {
            Console.WriteLine("Your score is: {0}", score);
            Console.WriteLine("Best score is: {0}", score);
        }
        else
        {
            Console.WriteLine("Your score is: {0}", score);
            Console.WriteLine("Best score is: {0}", bestScore);
        }


    }

    static void MoveUserCar()
    {
        userCar.x = 120;
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

    static void MoveUserCarDown()
    {
        if (userCar.y + 8 - 1 < RaceHeight - 8)
        {
            userCar.y = userCar.y + 8;
        }
    }

    static void MoveUserCarUp()
    {
        if (userCar.y > 1)
        {
            userCar.y = userCar.y - 8;
        }
    }

    static void NewObject()
    {
        int chance = random.Next(0, 100);
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

    static bool HittingCars()
    {
        if ((newObject.y == userCar.y) && (newObject.x + 25 >= userCar.x))
        {
            speed += acceleration;
            objects.Clear();
            return true;
        }
        return false;
    }

    static void OldCar()
    {
        List<Object> newList = new List<Object>();
        for (int i = 0; i < objects.Count; i++)
        {
            Object oldCar = objects[i];
            newObject.x = oldCar.x + 10;
            newObject.y = oldCar.y;
            newObject.c = oldCar.c;
            HittingCars();
            newObject.color = oldCar.color;
            if (newObject.x < GameWidth - 26)
            {
                newList.Add(newObject);
            }
        }
        objects = newList;
    }

    static void DrawInfo(int gameWidth, int gameHeight, double LiveCounter, double Acceleration, double Speed)
    {
        int x = 0, y = 0;
        string str = string.Empty;
        ConsoleColor color = ConsoleColor.Gray;

        x = gameWidth / 2;
        y = gameHeight - 6;

        Console.ForegroundColor = color;

        if (!EndGame())
        {
            Console.SetCursorPosition(x, y);
            Console.Write("Lives remaining: " + LiveCounter);
            Console.SetCursorPosition(x, y + 1);
            Console.Write("Score: " + score);
            Console.SetCursorPosition(x, y + 2);
            Console.Write("Best score: " + bestScore);
            Console.SetCursorPosition(0, y);
            Console.Write("Current Speed: " + Speed);
            Console.SetCursorPosition(0, y + 1);
            Console.Write("Current Acceleration: " + Acceleration);
            Console.WriteLine();
        }
    }

    static bool EndGame()
    {
        if (livesCount <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}