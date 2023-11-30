using System;
using System.Drawing;
using System.Threading;

namespace SpaceGame
{
    class Program
    {
        static Window window;
        static Ship ship;
        static Enemy enemy1;
        static Enemy enemy2;
        static Enemy enemy3;
        static Enemy boss;
        static bool play = false; // Flaga określająca, czy gra jest w trakcie
        static bool active = true; // Flaga określająca, czy program jest aktywny (menu lub gra)
        static bool showEnemies2And3 = false; // Flaga określająca, czy pokazać wrogów 2 i 3 po zniszczeniu wroga 1
        static bool showFinalBoss = false; // Flaga określająca, czy pokazać bossa po zniszczeniu wrogów 2 i 3
        static void Main(string[] args)
        {

            Start(); // Rozpoczęcie gry
            Game();  // Rozpoczęcie logiki gry

        }

        static void Start()  // Inicjalizacja gry
        {
            window = new Window( // Inicjalizacja okna gry
                Console.LargestWindowWidth,
                Console.LargestWindowHeight,
                ConsoleColor.Black,
                new Point(3, 1), // Pozycja lewego górnego rogu okna gry w konsoli (od trzeciej kolumny i pierwszego wiersza konsoli)
                new Point(118, 28)); // Pozycja prawego dolnego rogu okna (szerokość 118 znaków i wysokość 28 wierszy)


            window.DrawFrame(); // Rysowanie ramki okna


            ship = new Ship(  // Inicjalizacja statku gracza
                new Point(58, 23),
                ConsoleColor.White,
                window);

            // Inicjalizacja wrogów
            enemy1 = new Enemy(new Point(50, 10), ConsoleColor.Yellow, window, TypeEnemy.Normal, ship);
            enemy2 = new Enemy(new Point(30, 12), ConsoleColor.DarkGray, window, TypeEnemy.Normal, ship);
            enemy3 = new Enemy(new Point(40, 8), ConsoleColor.Cyan, window, TypeEnemy.Normal, ship);
            boss = new Enemy(new Point(70, 10), ConsoleColor.Magenta, window, TypeEnemy.Boss, ship);


            ship.Enemies.Add(enemy1); // Dodanie wrogów do listy wroga statku
            ship.Enemies.Add(enemy2);
            ship.Enemies.Add(enemy3);
            ship.Enemies.Add(boss);


        }

        static void Game()   // Logika gry
        {

            while (active)
            {
                window.Menu(); // Wyświetlenie menu gry
                window.Keyboard(ref active, ref play); // Obsługa klawiatury do wyboru opcji w menu

                while (play)   // Logika gry w trybie rozgrywki
                {
                    if (!enemy1.IsAlive && !showEnemies2And3)
                    {
                        Thread.Sleep(1700); // Oczekiwanie po zniszczeniu wroga 1 przed pojawieniem się wrogów 2 i 3
                        showEnemies2And3 = true;

                        window.SecondLevel(); // Wyświetlenie informacji o poziomie gry
                        ship.Health += 20; // Dodatkowe życie dla gracza

                        if (ship.Health > 100)
                        {
                            ship.Health = 100;
                        }

                        Ship.SuperShot += 25;  // Dodatkowe punkty super-strzału

                        if (Ship.SuperShot > 100)
                        {
                            Ship.SuperShot = 100;
                        }

                        // Poruszanie i aktualizacja informacji o wrogach 2 i 3
                        enemy2.Move();
                        enemy2.Information(80);
                        enemy3.Move();
                        enemy3.Information(100);
                    }

                    if (!enemy2.IsAlive && !enemy3.IsAlive && !showFinalBoss)
                    {
                        Thread.Sleep(1500); // Oczekiwanie po zniszczeniu wrogów 2 i 3 przed pojawieniem się bossa
                        showFinalBoss = true;
                        window.Danger(); // Wyświetlenie informacji o poziomie gry


                        ship.Health += 40; // Dodatkowe życie dla gracza

                        if (ship.Health > 100)
                        {
                            ship.Health = 100;
                        }

                        Ship.SuperShot += 30;

                        if (Ship.SuperShot > 100)
                        {
                            Ship.SuperShot = 100;
                        }

                        // Poruszanie i aktualizacja informacji o bossie
                        boss.Move();
                        boss.Information(100);
                    }

                    if (showFinalBoss)
                    {
                        boss.Move(); // Logika gry związana z bossem
                        boss.Information(100);
                    }
                    else if (showEnemies2And3) // Logika gry związana z wrogami 2 i 3
                    {
                        enemy2.Move();
                        enemy2.Information(80);

                        enemy3.Move();
                        enemy3.Information(100);
                    }
                    else
                    {
                        enemy1.Move(); // Logika gry związana z wrogiem 1
                        enemy1.Information(100);
                    }

                    // Poruszanie statkiem i strzelanie
                    ship.Move(2);
                    ship.Shoot();


                    // Sprawdzenie warunków końca gry (śmierć gracza lub zniszczenie bossa)
                    if (ship.Health <= 0)
                    {
                        play = false;
                        ship.Dead();
                        Restart();
                    }

                    if (!boss.IsAlive)
                    {
                        play = false;
                        window.End();
                        Restart();
                    }
                }
            }

        }

        static void Restart()   // Restart gry
        {
            Console.Clear();
            Thread.Sleep(100);
            window.DrawFrame();

            // Przywrócenie początkowych ustawień gry
            ship.Health = 100;
            Ship.SuperShot = 0;
            ship.Bullets.Clear();

            enemy1.Health = 100;
            enemy1.IsAlive = true;

            enemy2.Health = 100;
            enemy2.IsAlive = true;

            enemy3.Health = 100;
            enemy3.IsAlive = true;

            boss.Health = 100;
            boss.IsAlive = true;
            boss.PositionsEnemy.Clear();

            // Zresetowanie flagi showEnemies2And3 i showFinalBoss, aby rozpocząć poziomy od nowa
            showEnemies2And3 = false;
            showFinalBoss = false;
            
        }
    }
}
