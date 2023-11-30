using System;
using System.Collections.Generic;
using System.Drawing;


namespace SpaceGame
{
    public enum BulletType
    {
        Normal, Special, Enemy
    }
    internal class Bullet
    {
        public Point Position { get; set; } // Pozycja pocisku

        public ConsoleColor Color { get; set; }  // Kolor pocisku

        public BulletType BulletTypeB { get; set; } // Typ pocisku


        public List<Point> PositionsBullet { get; set; } // Lista pozycji dla pocisku 


        private DateTime time; // Zmienna do sledzenia czasu

        public Bullet(Point p, ConsoleColor c, BulletType b) 
        {
            this.Position = p; // Inicjalizacja pozycji pocisku
            this.Color = c; // Inicjalizacja koloru pocisku
            this.BulletTypeB = b; // Inicjalizacja typu pocisku
            PositionsBullet = new List<Point>(); // Inicjalizacja listy pozycji pocisku 
            time = DateTime.Now; // Inicjalizacja zmiennej czasu na aktualna date i godzine
        }

        public void Draw()
        {

            Console.ForegroundColor = Color; // Ustawienie konsoli na uzywanie koloru pocisku
            int x = Position.X;
            int y = Position.Y;


            PositionsBullet.Clear(); // Wyczyść liste pozycji pocisku


            switch (BulletTypeB)
            {
                case BulletType.Normal:
                    Console.SetCursorPosition(x, y);
                    Console.Write("o");
                    PositionsBullet.Add(new Point(x, y)); // Dodaj pozycje pocisku do listy
                    break;

                case BulletType.Special:
                    Console.SetCursorPosition(x + 1, y);
                    Console.Write("_");
                    Console.SetCursorPosition(x, y + 1);
                    Console.Write("( )");
                    Console.SetCursorPosition(x + 1, y + 2);
                    Console.Write("W");

                    PositionsBullet.Add(new Point(x + 1, y));
                    PositionsBullet.Add(new Point(x, y + 1));
                    PositionsBullet.Add(new Point(x + 2, y + 1));
                    PositionsBullet.Add(new Point(x + 1, y + 2)); // Dodaj pozycje pocisku do listy
                    break;


                case BulletType.Enemy:
                    Console.SetCursorPosition(x, y);
                    Console.Write("█");

                    PositionsBullet.Add(new Point(x, y)); // Dodaj pozycje pocisku do listy
                    break;

            }
        }

        public void Delete()
        {
            foreach (Point p in PositionsBullet)
            {
                Console.SetCursorPosition(p.X, p.Y);
                Console.Write(" "); // Wyczysc pozycje pocisku z ekranu
            }
        }


        public bool Move(int speed, int limit, List<Enemy> enemies)
        {

            if (DateTime.Now > time.AddMilliseconds(35))
            {
                Delete(); // Wyczysc pocisk z ekranu


                switch (BulletTypeB)
                {

                    case BulletType.Normal:
                        Position = new Point(Position.X, Position.Y - speed);
                        if (Position.Y <= limit)
                            return true; // Jesli pocisk przekroczy limit ekranu 

                        foreach (Enemy e in enemies)
                        {
                            foreach (Point p in e.PositionsEnemy)
                            {
                                if (p.X == Position.X && p.Y == Position.Y)
                                {
                                    e.Health -= 7; // Odejmij punkty zdrowia wroga
                                    Ship.SuperShot += 5; // Zwieksz licznik super strzalow statku

                                    if (e.Health <= 0)
                                    {
                                        e.Health = 0;
                                        e.IsAlive = false;
                                        e.Death(); // Jesli wrogowi skoncza sie punkty zdrowia, oznacz go jako martwego i wykonaj odpowiednia akcje
                                    }
                                    return true; // Jesli pocisk trafil w wroga
                                }
                            }
                        }
                        break;


                    case BulletType.Special:
                        Position = new Point(Position.X, Position.Y - speed);
                        if (Position.Y <= limit)
                            return true; // Jesli pocisk przekroczy limit ekranu

                        foreach (Enemy e in enemies)
                        {
                            foreach (Point p in e.PositionsEnemy)
                            {
                                foreach (Point pB in PositionsBullet)
                                {
                                    if (p.X == pB.X && p.Y == pB.Y)
                                    {
                                        e.Health -= 20;
                                        Ship.SuperShot += 10;
                                        if (e.Health <= 0)
                                        {
                                            e.Health = 0;
                                            e.IsAlive = false;
                                            e.Death();
                                        }
                                        return true; // Zwroc true, jesli pocisk trafil w wroga
                                    }
                                }
                            }
                        }
                        break;
                }

                Draw(); // Narysuj pocisk na nowo

                time = DateTime.Now;
            }

            return false; // Zwroc false, jesli pocisk nadal jest w ruchu
        }

        public bool Move(int speed, int limit, Ship ship)
        {

            if (DateTime.Now > time.AddMilliseconds(20))
            {
                Delete(); // Wyczysc pocisk z ekranu

                Position = new Point(Position.X, Position.Y + speed);
                if (Position.Y >= limit)
                    return true; // Jesli pocisk przekroczy limit ekranu


                foreach (Point p in ship.PositionsShip)
                {
                    if (p.X == Position.X && p.Y == Position.Y) // Jak strzal trafi w statek
                    {

                        ship.Health -= 4; // Odejmij punkty zdrowia statku
                        ship.ColorAux = Color; // Zapisz kolor statku po kolizji
                        ship.TimeCollision = DateTime.Now; // Zapisz czas kolizji statku
                        return true;
                    }
                }

                Draw();

                time = DateTime.Now;
            }

            return false;
        }

    }
}
//  Lista PositionsBullet sluzy do zarzadzania pozycjami i kolizjami pociskow specjalnych, ktore skladaja sie z kilku czesci
