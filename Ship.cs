using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;


namespace SpaceGame
{
    internal class Ship
    {
        public float Health { get; set; } // Zdrowie statku gracza

        public Point Position { get; set; } // Aktualna pozycja statku gracza

        public ConsoleColor Color { get; set; } // Kolor statku

        public Window WindowC { get; set; } // Referencja do obiektu okna gry

        public List<Point> PositionsShip { get; set; }  // Lista zawierająca pozycje rysowania statku na ekranie

        public List<Bullet> Bullets { get; set; }  // Lista kul wystrzelonych przez statek gracza

        public static float SuperShot { get; set; } // Wartość określająca dostępność super-strzału

        public List<Enemy> Enemies { get; set; } // Lista wrogów, którzy są przeciwnikami gracza

        public ConsoleColor ColorAux { get; set; } // Kolor statku po kolizji

        public DateTime TimeCollision { get; set; } // Data i czas ostatniej kolizji statku



        public Ship(Point p, ConsoleColor c, Window w)
        {
            this.Position = p; // Inicjalizacja pozycji statku gracza
            this.Color = c; // Inicjalizacja koloru statku gracza 
            this.WindowC = w; // Przypisanie referencji do obiektu okna gry
            Health = 100; // Inicjalizacja początkowego zdrowia statku
            PositionsShip = new List<Point>(); // Inicjalizacja listy pozycji rysowania statku
            Bullets = new List<Bullet>(); // Inicjalizacja listy kul wystrzelonych przez statek
            Enemies = new List<Enemy>(); // Inicjalizacja listy wrogów, którzy są przeciwnikami gracza
            this.ColorAux = c; // Kolor statku po kolizji
            this.TimeCollision = DateTime.Now; // Ustalenie daty i czasu ostatniej kolizji jako aktualny czas
        }
        public void Draw()
        {

            if (DateTime.Now > TimeCollision.AddMilliseconds(1000)) // Sprawdzamy, czy upłynęło więcej niż 1000 milisekund od ostatniej kolizji
            {
                Console.ForegroundColor = Color; // Jeśli tak, ustawiamy kolor statku na jego podstawowy kolor
            }
            else
            {
                Console.ForegroundColor = ColorAux; // W przeciwnym razie ustawiamy kolor wroga
            }
            int x = Position.X;
            int y = Position.Y;

            Console.SetCursorPosition(x + 3, y); // Ustawienie kursora na odpowiedniej pozycji oraz rysowanie fragmentu statku
            Console.Write("A");
            Console.SetCursorPosition(x + 1, y + 1);
            Console.Write("<{x}>");
            Console.SetCursorPosition(x, y + 2);
            Console.Write("± W W ±");

            PositionsShip.Clear(); // Czyszczenie listy pozycji statku gracza


            PositionsShip.Add(new Point(x + 3, y)); // Dodanie punktów rysowania statku do listy

            PositionsShip.Add(new Point(x + 1, y + 1));
            PositionsShip.Add(new Point(x + 2, y + 1));
            PositionsShip.Add(new Point(x + 3, y + 1));
            PositionsShip.Add(new Point(x + 4, y + 1));
            PositionsShip.Add(new Point(x + 5, y + 1));

            PositionsShip.Add(new Point(x, y + 2));
            PositionsShip.Add(new Point(x + 2, y + 2));
            PositionsShip.Add(new Point(x + 4, y + 2));
            PositionsShip.Add(new Point(x + 6, y + 2));

        }


        public void Delete()
        {
            foreach (Point p in PositionsShip) // Iterujemy przez listę pozycji statku gracza
            {
                Console.SetCursorPosition(p.X, p.Y);
                Console.Write(" "); // Ustawiamy kursor na pozycji i usuwamy znak statku z ekranu
            }
        }

        public void Keyboard(ref Point distance, int speed)
        {
            ConsoleKeyInfo key = Console.ReadKey(true); // Odczytujemy naciśnięty klawisz

            // Obsługa klawiszy W, S, A, D do poruszania statkiem
            if (key.Key == ConsoleKey.W)
                distance = new Point(0, -1);
            if (key.Key == ConsoleKey.S)
                distance = new Point(0, 1);
            if (key.Key == ConsoleKey.D)
                distance = new Point(1, 0);
            if (key.Key == ConsoleKey.A)
                distance = new Point(-1, 0);

            distance.X *= speed; // Skalowanie odległości przez prędkość statku
            distance.Y *= speed;


            if (key.Key == ConsoleKey.RightArrow)  // Obsługa strzałów z klawiszy strzałek
            {
                Bullet bullet = new Bullet( // Tworzenie nowego pocisku w prawo od statku gracza
                    new Point(Position.X + 6, Position.Y + 2),
                    ConsoleColor.White,
                    BulletType.Normal);

                Bullets.Add(bullet);

            }
            if (key.Key == ConsoleKey.LeftArrow)
            {
                Bullet bullet = new Bullet( // Tworzenie nowego pocisku w lewo od statku gracza
                    new Point(Position.X, Position.Y + 2),
                    ConsoleColor.White,
                    BulletType.Normal);

                Bullets.Add(bullet); // Dodaj nowy pocisk do listy pocisków gracza

            }
            if (key.Key == ConsoleKey.UpArrow)
            {
                if (SuperShot >= 100)  // Tworzenie specjalnego pocisku w górę od statku gracza, jeśli SuperShot jest wystarczająco naładowany
                {
                    Bullet bullet = new Bullet(
                        new Point(Position.X + 2, Position.Y - 2),
                        ConsoleColor.White,
                        BulletType.Special);

                    Bullets.Add(bullet);

                    SuperShot = 0; // Resetowanie licznika


                }
            }

        }

        public void Information() // Wyświetl informacje o zdrowiu gracza i ładowaniu super-strzału na górnym pasku ekranu
        {

            if (Health < 25)
                Console.ForegroundColor = ConsoleColor.Red; // Napis zmienia sie na czerwono jak poziom życia spadnie poniżej 25%
            else
                Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(WindowC.UpperLimit.X, WindowC.UpperLimit.Y - 1);
            Console.Write("HEALTH: " + (int)Health + " %  ");

            if (SuperShot >= 100) // Jak super strzał jest gotowy do wytrzelenia to napis staje sie zielony
                Console.ForegroundColor = ConsoleColor.Green;
            else
                Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(WindowC.UpperLimit.X + 16, WindowC.UpperLimit.Y - 1);
            Console.Write("SUPER SHOT: " + (int)SuperShot + " %  ");
            if (SuperShot >= 100)
                SuperShot = 100;


        }

        public void Shoot() // Obsługuje strzelanie gracza i przemieszczanie pocisków
        {
            for (int i = 0; i < Bullets.Count; i++)
            {
                if (Bullets[i].Move(1, WindowC.UpperLimit.Y, Enemies))
                {
                    Bullets.Remove(Bullets[i]);  // Usuń pocisk, jeśli przekroczy górny limit okna lub trafi w wroga
                }
            }
        }
        public void Move(int speed) // Obsługuje ruch statku gracza, zarządzanie kolizjami i aktualizację wyświetlanego stanu statku
        {
            if (Console.KeyAvailable)
            {
                Delete();

                Point distance = new Point();
                Keyboard(ref distance, speed); // Pobiera zmianę pozycji na podstawie klawiszy naciśniętych przez gracza
                Collisions(distance); // Sprawdza kolizje z granicami okna

            }

            Draw(); // Rysuje statek na nowo po zmianie pozycji
            Information(); // Wyświetla informacje na pasku stanu
        }


        public void Collisions(Point distance) // Sprawdza kolizje i aktualizuje pozycję statku gracza w określonych granicach okna gry
                                               // Jeśli statek wykroczy poza te granice, zostaje przeniesiony na najbliższą poprawną pozycję
        {

            Point positionAux = new Point(Position.X + distance.X, Position.Y + distance.Y); // Tworzy tymczasową pozycję statku, uwzględniając przesunięcie od klawiszy


            // Sprawdzanie i aktualizowanie pozycji statku w zależności od granic okna
            if (positionAux.X <= WindowC.UpperLimit.X)
                positionAux.X = WindowC.UpperLimit.X + 1;
            if (positionAux.X + 6 >= WindowC.LowerLimit.X)
                positionAux.X = WindowC.LowerLimit.X - 7;
            if (positionAux.Y <= (WindowC.UpperLimit.Y) + 15)
                positionAux.Y = (WindowC.UpperLimit.Y + 1) + 15;
            if (positionAux.Y + 2 >= WindowC.LowerLimit.Y)
                positionAux.Y = WindowC.LowerLimit.Y - 3;


            Position = positionAux; // Aktualizuje pozycję statku
        }


        public void Dead() // Metoda wywoływana w przypadku śmierci statku gracza
        {
            Console.ForegroundColor = Color;

            Thread.Sleep(100); // Opóźnienie przed wyświetleniem efektu "śmierci" statku


            foreach (Point p in PositionsShip)   // Wyświetla efekt "śmierci" statku
            {
                Console.SetCursorPosition(p.X, p.Y);
                Console.Write("O");
                Thread.Sleep(250);
            }

            foreach (Point p in PositionsShip) // Usuwa efekt "śmierci" statku
            {
                Console.SetCursorPosition(p.X, p.Y);
                Console.Write(" ");
                Thread.Sleep(250);
            }

            Thread.Sleep(1000);  // Opóźnienie przed wyświetleniem komunikatu "Game Over"
            Console.Clear();


            Console.SetCursorPosition(WindowC.LowerLimit.X / 2 - 6, WindowC.LowerLimit.Y - 17);
            WindowC.Title(" Game Over", 200);  // Wyświetla komunikat "Game Over" w środku ekranu

            Thread.Sleep(500); // Opóźnienie przed wyświetleniem informacji o powrocie do gry

            Console.SetCursorPosition(WindowC.LowerLimit.X / 2 - 12, WindowC.LowerLimit.Y - 6);
            Console.Write("Press SPACE to return...");

            Thread.Sleep(500);
            ConsoleKeyInfo keyInfo;
            do
            {
                keyInfo = Console.ReadKey(true);
            } while (keyInfo.Key != ConsoleKey.Spacebar);

            Console.Clear();
        }
    }
}
