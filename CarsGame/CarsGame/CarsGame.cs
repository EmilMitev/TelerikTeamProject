﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Media;

//create object
struct Object
{
    public int x;
    public int y;
    public char[,] c;
}

class CarsGame
{
    const int RaceWidth = 100;
    const int RaceHeight = 20;
    const int InfoPanelHeight = 10;
    const int GameWidth = RaceWidth;
    const int GameHeight = RaceHeight + InfoPanelHeight;

    #region bonus
    static char[,] bonus = new char[,]
        { 
            {'─','─','▐','█',' '},
            {'─','─','█','─','▄'},
            {'─','▐','█','▀',' '},
            {'─','▌','─','▌',' '},
            {'▐','▄','─','▐','▄'}
        };
    #endregion

    #region user car
    static char[,] userCarArr = new char[,]
    {
        {' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ' },
        {' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','_',' ',' ',' ',' ','_',' '},
        {' ',' ',' ','_','_','.','.','_','o','|',' ','\\','.','.','`','/',' '},
        {'<','.','(','_',')','_','_','_','_','_','_','(','_',')',',','.','`'},
        {' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ' }
    };
    #endregion

    #region enemy car
    static char[,] enemyCar = new char[,]
    {
        {' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ' },
        {' ','_',' ',' ',' ',' ','_',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ' },
        {' ','\\','`','.','.','/',' ','|','o','_','.','.','_','_',' ',' ',' ' },
        {'`','.',',','(','_',')','_','_','_','_','_','_','(','_',')','.','>' },
        {' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ' }

    };
    #endregion

    static List<Object> objects = new List<Object>();
    static Object userCar = new Object();
    static Object newCarObject = new Object();
    static Object newBonusObject = new Object();

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

        newCarObject.x = 40;
        newBonusObject.x = 40;
        userCar.c = userCarArr;

        ReadFromFile();

        while (true)
        {
            MoveUserCar();
            NewObject();
            if (HittingObjects() == 1)
            {
                score += 1000;
            }
            if (HittingObjects() == 2)
            {
                --livesCount;
                speed -= 10;
                score -= 500;
                Console.Beep(730, 300);
            }

            DrawInfo(GameWidth, GameHeight, livesCount, acceleration, speed);

            score += 10;
            if (score % 10000 == 0 && score != 0)
            {
                livesCount++;
            }

            if (random.Next(0, 5) == 1)
            {
                ++speed;
            }
            if (speed >= 90)
            {
                speed = 90;
            }

            Thread.Sleep(100 - speed);
            Console.Clear();

            if (EndGame())
            {
                PrintStringAfrerGameIsOver();
                EndGameSound();
                break;
            }
        }

        if (score > bestScore)
        {
            WriteIntoFile();
        }
    }

    static void EndGameSound()
    {
        using (SoundPlayer player = new SoundPlayer(@"../../Sound/GameOver.wav"))
        {
            try
            {
                player.PlaySync();
            }
            catch (FileNotFoundException ex)
            {
                Console.Error.WriteLine("File not found!\n{0}", ex.Message);
            }
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

    static void PrintOnPosition(char[,] figure, int row, int col, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        for (int x = 0; x < figure.GetLength(0); x++)
        {
            for (int y = 0; y < figure.GetLength(1); y++)
            {
                Console.CursorVisible = false;
                Console.SetCursorPosition(col + y, row + x);
                Console.Write(figure[x, y]);
            }
        }
    }

    static void PrintStringAfrerGameIsOver(ConsoleColor color = ConsoleColor.Red)
    {
        Console.Clear();
        if (score > bestScore)
        {
            Console.WriteLine(@"   
                      :::!~!!!!!:.
                  .xUHWH!! !!?M88WHX:.
                .X*#M@$!!  !X!M$$$$$$WWx:.
               :!!!!!!?H! :!$!$$$$$$$$$$8X:        
              !!~  ~:~!! :~!$!#$$$$$$$$$$8X:                            _____________
             :!~::!H!<   ~.U$X!?R$$$$$$$$MM!       Your score is: {0}  (_________ _.'``''-....
             ~!~!!!!~~ .:XW$$$U!!?$$$$$$RMM!       Best score is: {1}      (___.-' _,
               !:~~~ .:!M'T#$$$$WX??#MRRMMM!                                (_(_.-'  `   
               ~?WuxiW*`   `'#$$$$8!!!!??!!!                                 (__)__,-''``'-----
             :X- M$$$$       `'T#$T~!8$WUXU~
            :%`  ~#$$$m:        ~!~ ?$$$$$$
          :!`.-   ~T$$$$8xx.  .xWW- ~''##*'
.....   -~~:<` !    ~?T#$$@@W@*?$$      /`
W$@@M!!! .!~~ !!     .:XUW$W!~ `'~:    :
#'~~`.:x%`!!  !H:   !WM$$$$Ti.: .!WUn+!`
:::~:!!`:X~ .: ?H.!u '$$$B$$$!W:U!T$$M~
.~~   :X@!.-~   ?@WTWo('*$$$W$TH$! `
Wi.~!X$?!-~    : ?$$$B$Wu('**$RM!
$R@i.~~ !     :   ~$$$$$B$$en:``
?MXT@Wx.~    :     ~'##*$$$$M~", score, score);
            Console.WriteLine(@"    
    $$$$$$\   $$$$$$\  $$\      $$\ $$$$$$$$\        $$$$$$\  $$\    $$\ $$$$$$$$\ $$$$$$$\  
    $$  __$$\ $$  __$$\ $$$\    $$$ |$$  _____|      $$  __$$\ $$ |   $$ |$$  _____|$$  __$$\ 
    $$ /  \__|$$ /  $$ |$$$$\  $$$$ |$$ |            $$ /  $$ |$$ |   $$ |$$ |      $$ |  $$ |
    $$ |$$$$\ $$$$$$$$ |$$\$$\$$ $$ |$$$$$\          $$ |  $$ |\$$\  $$  |$$$$$\    $$$$$$$  |
    $$ |\_$$ |$$  __$$ |$$ \$$$  $$ |$$  __|         $$ |  $$ | \$$\$$  / $$  __|   $$  __$$< 
    $$ |  $$ |$$ |  $$ |$$ |\$  /$$ |$$ |            $$ |  $$ |  \$$$  /  $$ |      $$ |  $$ |
    \$$$$$$  |$$ |  $$ |$$ | \_/ $$ |$$$$$$$$\        $$$$$$  |   \$  /   $$$$$$$$\ $$ |  $$ |
     \______/ \__|  \__|\__|     \__|\________|       \______/     \_/    \________|\__|  \__| ");
        }
        else
        {
            Console.WriteLine(@"   
                      :::!~!!!!!:.
                  .xUHWH!! !!?M88WHX:.
                .X*#M@$!!  !X!M$$$$$$WWx:.
               :!!!!!!?H! :!$!$$$$$$$$$$8X:        
              !!~  ~:~!! :~!$!#$$$$$$$$$$8X:       
             :!~::!H!<   ~.U$X!?R$$$$$$$$MM!                            _____________
             ~!~!!!!~~ .:XW$$$U!!?$$$$$$RMM!       Your score is: {0}  (_________ _.'``''-....
               !:~~~ .:!M'T#$$$$WX??#MRRMMM!       Best score is: {1}      (___.-' _,
               ~?WuxiW*`   `'#$$$$8!!!!??!!!                                (_(_.-'  `   
             :X- M$$$$       `'T#$T~!8$WUXU~                                 (__)__,-''``'-----
            :%`  ~#$$$m:        ~!~ ?$$$$$$
          :!`.-   ~T$$$$8xx.  .xWW- ~''##*'
.....   -~~:<` !    ~?T#$$@@W@*?$$      /`
W$@@M!!! .!~~ !!     .:XUW$W!~ `'~:    :
#'~~`.:x%`!!  !H:   !WM$$$$Ti.: .!WUn+!`
:::~:!!`:X~ .: ?H.!u '$$$B$$$!W:U!T$$M~
.~~   :X@!.-~   ?@WTWo('*$$$W$TH$! `
Wi.~!X$?!-~    : ?$$$B$Wu('**$RM!
$R@i.~~ !     :   ~$$$$$B$$en:``
?MXT@Wx.~    :     ~'##*$$$$M~", score, bestScore);
            Console.WriteLine(@"    
    $$$$$$\   $$$$$$\  $$\      $$\ $$$$$$$$\        $$$$$$\  $$\    $$\ $$$$$$$$\ $$$$$$$\  
    $$  __$$\ $$  __$$\ $$$\    $$$ |$$  _____|      $$  __$$\ $$ |   $$ |$$  _____|$$  __$$\ 
    $$ /  \__|$$ /  $$ |$$$$\  $$$$ |$$ |            $$ /  $$ |$$ |   $$ |$$ |      $$ |  $$ |
    $$ |$$$$\ $$$$$$$$ |$$\$$\$$ $$ |$$$$$\          $$ |  $$ |\$$\  $$  |$$$$$\    $$$$$$$  |
    $$ |\_$$ |$$  __$$ |$$ \$$$  $$ |$$  __|         $$ |  $$ | \$$\$$  / $$  __|   $$  __$$< 
    $$ |  $$ |$$ |  $$ |$$ |\$  /$$ |$$ |            $$ |  $$ |  \$$$  /  $$ |      $$ |  $$ |
    \$$$$$$  |$$ |  $$ |$$ | \_/ $$ |$$$$$$$$\        $$$$$$  |   \$  /   $$$$$$$$\ $$ |  $$ |
     \______/ \__|  \__|\__|     \__|\________|       \______/     \_/    \________|\__|  \__| ");
        }


    }

    static void MoveUserCar()
    {
        userCar.x = 80;
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
        PrintOnPosition(userCar.c, userCar.y, userCar.x, ConsoleColor.Magenta);
    }

    static void MoveUserCarDown()
    {
        if (userCar.y + userCarArr.GetLength(0) - 1 < 10)
        {
            userCar.y = userCar.y + userCarArr.GetLength(0);
        }
    }

    static void MoveUserCarUp()
    {
        if (userCar.y > 1)
        {
            userCar.y = userCar.y - userCarArr.GetLength(0);
        }
    }

    static void NewObject()
    {
        int chance = random.Next(0, 100);
        int[] laneY = { 0, userCarArr.GetLength(0), userCarArr.GetLength(0) * 2 };
        int randomIndexLaneY = random.Next(0, laneY.Length);
        if (chance < 10)
        {
            Object newBonus = new Object();
            newBonus.x = 0;
            newBonus.y = laneY[randomIndexLaneY];
            newBonus.c = bonus;
            if ((newCarObject.x - newBonus.x) >= 30)
            {
                objects.Add(newBonus);
            }
        }
        if (chance > 10)
        {
            Object newCar = new Object();
            newCar.x = 0;
            newCar.y = laneY[randomIndexLaneY];
            newCar.c = enemyCar;
            if ((newCarObject.x - newCar.x) >= 30)
            {
                objects.Add(newCar);
            }
        }
        MovingEnemyCars();
    }

    static void MovingEnemyCars()
    {
        List<Object> newList = new List<Object>();
        for (int i = 0; i < objects.Count; i++)
        {
            Object moveEnemyCar = objects[i];
            newCarObject.x = moveEnemyCar.x + 4;
            newCarObject.y = moveEnemyCar.y;
            newCarObject.c = moveEnemyCar.c;
            HittingObjects();
            if (moveEnemyCar.c == enemyCar)
            {
                if (newCarObject.x < GameWidth - userCarArr.GetLength(1))
                {
                    newList.Add(newCarObject);
                }
            }
            else
            {
                if (newCarObject.x < GameWidth - bonus.GetLength(1))
                {
                    newList.Add(newCarObject);
                }
            }
        }

        objects = newList;

        foreach (Object car in objects)
        {
            if (car.c == bonus)
            {
                PrintOnPosition(car.c, car.y, car.x, ConsoleColor.Green);
            }
            else
            {
                PrintOnPosition(car.c, car.y, car.x, ConsoleColor.Red);
            }
        }
    }

    static int HittingObjects()
    {
        if ((newCarObject.c == bonus) && (newCarObject.y == userCar.y) && (newCarObject.x + userCarArr.GetLength(1) >= userCar.x))
        {
            objects.Clear();
            Console.Clear();
            Console.SetCursorPosition(40, newCarObject.y);
            Console.WriteLine("      _          ");
            Console.SetCursorPosition(40, newCarObject.y + 1);
            Console.WriteLine("     /(|         ");
            Console.SetCursorPosition(40, newCarObject.y + 2);
            Console.WriteLine("    (  :               ___   ______  ______  ______    ");
            Console.SetCursorPosition(40, newCarObject.y + 3);
            Console.WriteLine("   __\\  \\  _____      /   \\ (  __  )(  __  )(  __  ) ");
            Console.SetCursorPosition(40, newCarObject.y + 4);
            Console.WriteLine(" (____)  `|      _    \\/| | | (  ) || (  ) || (  ) | ");
            Console.SetCursorPosition(40, newCarObject.y + 5);
            Console.WriteLine("(____)|   |    _| |_    | | | (  ) || (  ) || (  ) |");
            Console.SetCursorPosition(40, newCarObject.y + 6);
            Console.WriteLine(" (____).__|   |_   _| __| |_| (__) || (__) || (__) |");
            Console.SetCursorPosition(40, newCarObject.y + 7);
            Console.WriteLine("  (___)__.|___  |_|   \\____/(______)(______)(______) ");
            Thread.Sleep(500);

            return 1;
        }
        else if ((newCarObject.c == enemyCar) && (newCarObject.y == userCar.y) && (newCarObject.x + userCarArr.GetLength(1) >= userCar.x))
        {
            objects.Clear();
            Console.Clear();
            Console.SetCursorPosition(80, newCarObject.y);
            Console.WriteLine(" o   ,");
            Console.SetCursorPosition(80, newCarObject.y + 1);
            Console.WriteLine("|   / _    ");
            Console.SetCursorPosition(80, newCarObject.y + 2);
            Console.WriteLine("@%_C R A/S H !  ");
            Console.SetCursorPosition(80, newCarObject.y + 3);
            Console.WriteLine("#@o___ ¯@@   ");
            Console.SetCursorPosition(80, newCarObject.y + 4);
            Console.WriteLine(" @%  ¯¯¯");
            Console.SetCursorPosition(80, newCarObject.y + 5);
            Console.WriteLine("|  o");
            Thread.Sleep(500);
            return 2;
        }
        return 3;
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