﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShip
{
    class Program
    {
        static void Main(string[] args)
        {
            Grid grid = new Grid();
            
            grid.Greet();

            
            Ship ship1 = new Ship(ShipType.Submarine);
            Ship ship2 = new Ship(ShipType.Minesweeper);
            Ship ship3 = new Ship(ShipType.Cruiser);
            Ship ship4 = new Ship(ShipType.Carrier);
            Ship ship5 = new Ship(ShipType.Battleship);

           
            grid.PlaceShip(ship1);
            grid.PlaceShip(ship2);
            grid.PlaceShip(ship3);
            grid.PlaceShip(ship4);
            grid.PlaceShip(ship5);

            
            grid.PlayGame();
            

        }
        public enum PointStatus
        {
            Empty,
            Ship,
            Hit,
            Miss
        }

        class Point
        {
            
            public int x { get; set; }
            public int y { get; set; }
            public PointStatus status { get; set; }

            
            public Point(int x, int y, PointStatus status)
            {
                this.x = x;
                this.y = y;
                this.status = status;
            }

            
        }
        public enum ShipType
        {
            Carrier = 5,
            Battleship = 4,
            Cruiser = 3,
            Submarine = 3,
            Minesweeper = 2
        }

        class Ship
        {
            
            public ShipType type { get; set; }
            public List<Point> occupiedPoint { get; set; }

            public int length { get; set; }
            public bool IsDistroyed
            {
                get
                {
                    return occupiedPoint.All(x => x.status == PointStatus.Hit);
                }
            }

           
            public Ship(ShipType typeOfShip)
            {
                this.occupiedPoint = new List<Point>();
                this.type = typeOfShip;
                this.length = (int)type;
            }



        }
        public enum PlaceShipDirection
        {
            Horizontal = 1,
            Vertical = 2
        }

        class Grid
        {
            
            public string name;
            public static Point[,] Ocean { get; set; }
            public static List<Ship> ListOfShips { get; set; }
            public static bool AllShipsDestroyed
            {
                get
                {
                    return ListOfShips.All(x => x.IsDistroyed);
                }
            }
            public static int CombatRound { get; set; }
            public static int Score { get; set; }

            public Grid()
            {
                
                Ocean = new Point[10, 10];
                ListOfShips = new List<Ship>();
                

                
                for (int x = 0; x < 10; x++) 
                {
                    for (int y = 0; y < 10; y++) 
                    {
                        Ocean[x, y] = new Point(x, y, PointStatus.Empty);
                    }
                }

            }

            
            public void Greet()
            {
                Console.Write("Enter Your Name: ");
                name = Console.ReadLine();
                Console.Clear();

                Console.WriteLine("\nWelcome to Battle Ship, " + name + ", this game will make you very angry.");
                Console.WriteLine("\ntype \"HELP\" for the introduction \n or Press enter to skip the introduction:");
                string temp = Console.ReadLine();

                if (temp == "")
                    return;
                else if (temp.ToLower() == "help")
                {
                    Console.WriteLine(@"
You will play this Battleship game and absolutely love it.
First type in a X value and a Y value. Whichever the case is
it will either print a circle and if you hit, it will display a red X.
    ");
                    Console.ReadKey();
                }
            }

            
            public void PlaceShip(Ship shipToPlace)
            {
                bool yesShip = false;
                int startx = 0, starty = 0;
                int direction = 0;

                
                while (!yesShip)
                {
                    Random rng = new Random();

                    
                    startx = rng.Next(0, 10);
                    starty = rng.Next(0, 10);

                    
                    direction = rng.Next(1, 3);
                    yesShip = CanPlaceShip(shipToPlace, (PlaceShipDirection)direction, startx, starty);
                }

                
                for (int i = 0; i < shipToPlace.length; i++)
                {
                    
                    Ocean[startx, starty].status = PointStatus.Ship;
                    
                    shipToPlace.occupiedPoint.Add(Ocean[startx, starty]);

                    if ((PlaceShipDirection)direction == PlaceShipDirection.Horizontal)
                    {
                        startx++;
                    }
                    else
                        starty++;
                }
                
                ListOfShips.Add(shipToPlace);
            }

            
            public static void DisplayOcean()
            {
                
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("    0  1  2  3  4  5  6  7  8  9  X");
                Console.WriteLine("===================================");
                Console.ResetColor();

                
                for (int y = 0; y < 10; y++)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(y+"||");
                    Console.ResetColor();

                    for (int x = 0; x < 10; x++)
                    {
                        if (Ocean[x, y].status == PointStatus.Empty || Ocean[x, y].status == PointStatus.Ship)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkBlue;
                            Console.Write("[ ]");
                            Console.ResetColor();
                        }
                        else if (Ocean[x, y].status == PointStatus.Hit)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("[X]");
                            Console.ResetColor();
                        }
                        else if (Ocean[x, y].status == PointStatus.Miss)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write("[O]");
                            Console.ResetColor();
                        }
                    }
                    Console.ResetColor();
                    Console.WriteLine();
                }
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Y||");
                Console.ResetColor();
            }
            public void Target(int x, int y)
            {
                if (Ocean[x, y].status == PointStatus.Ship)
                {
                    Ocean[x, y].status = PointStatus.Hit;
                    
                }
                else if (Ocean[x, y].status == PointStatus.Empty)
                {
                    Ocean[x, y].status = PointStatus.Miss;
                    
                }
            }
            public void PlayGame()
            {
                while (!AllShipsDestroyed)
                {
                    DisplayOcean();

                    int x = -1, y = 10;
                    
                    while (x < 0 || x > 9)
                    {
                        string xException = "";
                        while (xException == "" || !("0123456789".Contains(xException)))
                        {
                            Console.WriteLine("Enter X coordinate");
                            xException = Console.ReadLine();
                        }
                        
                        x = int.Parse(xException);
                    }

                    while (y < 0 || y > 9)
                    {
                        string yException = "";
                        while (yException == "" || !("0123456789".Contains(yException)))
                        {
                            Console.WriteLine("Enter Y coordinate");
                            yException = Console.ReadLine();
                        }
                        
                        y = int.Parse(yException);
                    }
                    
                    Target(x, y);
                    CombatRound++;

                }
                DisplayOcean();
                Console.WriteLine("Congratulations you WON!");
                Console.WriteLine("It took " + CombatRound + " rounds to finish the game");
                
            }
            public bool CanPlaceShip(Ship shipToPlace, PlaceShipDirection direction, int startx, int starty)
            {
                for (int i = 0; i < shipToPlace.length; i++)
                {
                    if (Ocean[startx, starty].status == PointStatus.Ship)
                        return false;

                    if (direction == PlaceShipDirection.Vertical)
                    {
                        starty++;
                        if (starty > 9)
                            return false;
                    }
                    else if (direction == PlaceShipDirection.Horizontal)
                    {
                        startx++;
                        if (startx > 9)
                            return false;
                    }

                }
                return true;

            }

            

        }




    }
}
