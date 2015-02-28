using System;
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
    const int RaceWidth = 100;
    const int RaceHeight = 20;
    const int InfoPanelHeight = 10;
    const int GameWidth = RaceWidth;
    const int GameHeight = RaceHeight + InfoPanelHeight;

    #region user car
    static char[,] userCarArr = new char[,]
    {
        {' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','_',' ',' ',' ',' ','_',' '},
        {' ',' ',' ','_','_','.','.','_','o','|',' ','\\','.','.','`','/',' '},
        {'<','.','(','_',')','_','_','_','_','_','_','(','_',')',',','.','`'}
    };
    #endregion

    #region enemy car
    static char[,] enemyCar = new char[,]
    {
        {' ','_',' ',' ',' ',' ','_',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ' },
        {' ','\\','`','.','.','/',' ','|','o','_','.','.','_','_',' ',' ',' ' },
        {'`','.',',','(','_',')','_','_','_','_','_','_','(','_',')','.','>' }

    };
    #endregion

    #region bonus
    static char[,] bonus = new char[,]
    {
        {' ',' ',' ',' ',',','d','8','8','.','8','8','b',',',' ',' ',' ',' ' },
        {' ',' ',' ',' ',' ','`','Y','8','8','8','Y','\'',' ',' ',' ',' ',' ' },
        {' ',' ',' ',' ',' ',' ',' ','`','Y','\'',' ',' ',' ',' ',' ',' ',' ' }

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
                PrintStringAfrerGameIsOver();
                break;
            }

            MoveUserCar();

            NewObject();

            if (HittingCars())
            {
                --livesCount;
            }

            foreach (Object car in objects)
            {
                PrintOnPosition(enemyCar, car.y, car.x, ConsoleColor.Red);
            }
            if (random.Next(0, 5) == 1)
            {
                ++speed;
            }
            if (speed >= 90)
            {
                speed = 90;
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
              !!~  ~:~!! :~!$!#$$$$$$$$$$8X:                
             :!~::!H!<   ~.U$X!?R$$$$$$$$MM!
             ~!~!!!!~~ .:XW$$$U!!?$$$$$$RMM!                Your score is: {0}
               !:~~~ .:!M'T#$$$$WX??#MRRMMM!                Best score is: {1}
               ~?WuxiW*`   `'#$$$$8!!!!??!!!
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
             :!~::!H!<   ~.U$X!?R$$$$$$$$MM!
             ~!~!!!!~~ .:XW$$$U!!?$$$$$$RMM!                Your score is: {0}
               !:~~~ .:!M'T#$$$$WX??#MRRMMM!                Best score is: {1}
               ~?WuxiW*`   `'#$$$$8!!!!??!!!
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

        PrintOnPosition(userCar.c, userCar.y, userCar.x, ConsoleColor.Cyan);
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
        int[] laneY = { 0, userCarArr.GetLength(0), userCarArr.GetLength(0) * 2, userCarArr.GetLength(0) * 3 };
        int randomIndexLaneY = random.Next(0, laneY.Length);
        if (chance < 90)
        {
            Object newCar = new Object();
            newCar.color = ConsoleColor.Yellow;
            newCar.x = 0;
            newCar.y = laneY[randomIndexLaneY];
            newCar.c = enemyCar;
            if ((newObject.x - newCar.x) >= 30)
            {
                objects.Add(newCar);
            }
        }
        MovingEnemyCars();
    }

    static bool HittingCars()
    {
        if ((newObject.y == userCar.y) && (newObject.x + userCarArr.GetLength(1) >= userCar.x))
        {
            speed += acceleration;
            objects.Clear();
            Console.Clear();
            Console.SetCursorPosition(80, newObject.y);
            Console.WriteLine(" o   ,");
            Console.SetCursorPosition(80, newObject.y + 1);
            Console.WriteLine("|   / _    ");
            Console.SetCursorPosition(80, newObject.y + 2);
            Console.WriteLine("@%_C R A/S H !  ");
            Console.SetCursorPosition(80, newObject.y + 3);
            Console.WriteLine("#@o___ ¯@@   ");
            Console.SetCursorPosition(80, newObject.y + 4);
            Console.WriteLine(" @%  ¯¯¯");
            Console.SetCursorPosition(80, newObject.y + 5);
            Console.WriteLine("|  o");
            Thread.Sleep(200);
            return true;
        }
        return false;
    }

    static void MovingEnemyCars()
    {
        List<Object> newList = new List<Object>();
        for (int i = 0; i < objects.Count; i++)
        {
            Object moveEnemyCar = objects[i];
            newObject.x = moveEnemyCar.x + 4;
            newObject.y = moveEnemyCar.y;
            newObject.c = moveEnemyCar.c;
            HittingCars();
            newObject.color = moveEnemyCar.color;
            if (newObject.x < GameWidth - userCarArr.GetLength(1))
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
