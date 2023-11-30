using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;


namespace SpaceGame
{
    public enum TypeEnemy
    {
        Normal, Boss, Menu
    }
    internal class Enemy
    {

        public bool IsAlive { get; set; } // Okresla, czy wrog jest zywy
        public float Health { get; set; } // Zdrowie wroga
        public Point Position { get; set; } // Pozycja wroga
        public Window WindowC { get; set; } // Okno gry, w ktorym wrog sie porusza
        public ConsoleColor Color { get; set; } // Kolor wroga

        public TypeEnemy TypeEnemyE { get; set; } // Typ wroga

        public List<Point> PositionsEnemy { get; set; } // Lista przechowujaca pozycje czesci wroga

        enum Direction
        {
            Right, Left, Up, Down
        }


        private Direction direction; // Aktualny kierunek poruszania sie wroga


        private DateTime timeDirection; // Czas od ostatniej zmiany kierunku poruszania sie


        private float timeDirectionRandom; // Losowy czas do kolejnej zmiany kierunku


        private DateTime timeMovement; // Czas od ostatniego ruchu wroga
        public List<Bullet> Bullets { get; set; }  // Lista pociskow wrogow
        private DateTime timeShoot; // Czas od ostatniego strzalu wroga
        private float timeShootRandom; // Losowy czas do kolejnego strzału wroga
        private Ship ShipC { get; set; } // Referencja do statku gracza


        public Enemy(Point p, ConsoleColor c, Window w, TypeEnemy t, Ship shipC)
        {
            this.Position = p; // Inicjalizacja pozycji wroga
            this.Color = c; // Inicjalizacja koloru wroga
            this.TypeEnemyE = t; // Inicjalizacja typu wroga
            this.WindowC = w; // Inicjalizacja okna gry
            IsAlive = true; // Wrogowie sa inicjalnie zywi
            Health = 100; // Wartosc poczatkowego zdrowia wroga
            direction = Direction.Right; // Poczatkowy kierunek poruszania wroga (prawo)
            timeDirection = DateTime.Now; 
            timeDirectionRandom = 1000;
            timeMovement = DateTime.Now;
            PositionsEnemy = new List<Point>(); // Inicjalizacja listy przechowujacej pozycje czesci wroga
            Bullets = new List<Bullet>(); // Inicjalizacja listy pociskow wrogow
            timeShoot = DateTime.Now;
            timeShootRandom = 200;
            this.ShipC = shipC; // Przypisanie referencji do statku gracza
        }

        public void Draw() // Rysowanie wrogow
        {

            switch (TypeEnemyE)
            {
                case TypeEnemy.Normal:
                    DrawNormal();
                    break;

                case TypeEnemy.Boss:
                    DrawBoss();
                    break;

                case TypeEnemy.Menu:
                    DrawNormal();
                    break;
            }
        }


        public void DrawNormal() 
        {
            Console.ForegroundColor = Color; // Ustawienie koloru wroga
            int x = Position.X; // Pobranie pozycji X wroga
            int y = Position.Y;

            Console.SetCursorPosition(x + 1, y); // Rysowanie wroga typu Normal na ekranie
            Console.Write("▄▄");
            Console.SetCursorPosition(x, y + 1);
            Console.Write("████");
            Console.SetCursorPosition(x, y + 2);
            Console.Write("▀  ▀");

            PositionsEnemy.Clear(); // Wyczyszczenie listy pozycji czesci wroga

            PositionsEnemy.Add(new Point(x + 1, y)); // Dodanie pozycji kazdej czesci wroga do listy
            PositionsEnemy.Add(new Point(x + 2, y));
            PositionsEnemy.Add(new Point(x, y + 1));
            PositionsEnemy.Add(new Point(x + 1, y + 1));
            PositionsEnemy.Add(new Point(x + 2, y + 1));
            PositionsEnemy.Add(new Point(x + 3, y + 1));
            PositionsEnemy.Add(new Point(x, y + 2));
            PositionsEnemy.Add(new Point(x + 3, y + 2));
        }


        public void DrawBoss()
        {
            Console.ForegroundColor = Color; // Ustawienie koloru wroga
            int x = Position.X; // Pobranie pozycji X wroga
            int y = Position.Y;

            Console.SetCursorPosition(x + 1, y); // Rysowanie wroga typu Boss na ekranie
            Console.Write("█▄▄▄▄█");
            Console.SetCursorPosition(x, y + 1);
            Console.Write("██ ██ ██");
            Console.SetCursorPosition(x, y + 2);
            Console.Write("█▀▀▀▀▀▀█");

            PositionsEnemy.Clear(); // Wyczyszczenie listy pozycji czesci wroga

            PositionsEnemy.Add(new Point(x + 1, y));  // Dodanie pozycji kazdej czesci wroga do listy
            PositionsEnemy.Add(new Point(x + 2, y));
            PositionsEnemy.Add(new Point(x + 3, y));
            PositionsEnemy.Add(new Point(x + 4, y));
            PositionsEnemy.Add(new Point(x + 5, y));
            PositionsEnemy.Add(new Point(x + 6, y));

            PositionsEnemy.Add(new Point(x, y + 1));
            PositionsEnemy.Add(new Point(x + 1, y + 1));
            PositionsEnemy.Add(new Point(x + 3, y + 1));
            PositionsEnemy.Add(new Point(x + 4, y + 1));
            PositionsEnemy.Add(new Point(x + 6, y + 1));
            PositionsEnemy.Add(new Point(x + 7, y + 1));

            PositionsEnemy.Add(new Point(x, y + 2));
            PositionsEnemy.Add(new Point(x + 1, y + 2));
            PositionsEnemy.Add(new Point(x + 2, y + 2));
            PositionsEnemy.Add(new Point(x + 3, y + 2));
            PositionsEnemy.Add(new Point(x + 4, y + 2));
            PositionsEnemy.Add(new Point(x + 5, y + 2));
            PositionsEnemy.Add(new Point(x + 6, y + 2));
            PositionsEnemy.Add(new Point(x + 7, y + 2));

        }


        public void Delete()
        {
            // Usuniecie wroga z ekranu
            foreach (Point p in PositionsEnemy)
            {
                Console.SetCursorPosition(p.X, p.Y);
                Console.Write(" "); // Wyczyszczenie piksela na ekranie
            }
        }

        public void Move()
        {

            if (!IsAlive)  // Jesli wrog nie jest zywy, wykonaj procedure smierci i zakoncz funkcje
            {
                Death();
                return;
            }

            int time = 30; // Co ile milisekund wrog powinien się poruszac

            if (TypeEnemyE == TypeEnemy.Boss)
                time = 20; // Boss porusza sie szybciej


            if (DateTime.Now > timeMovement.AddMilliseconds(time))
            {

                Delete(); // Usuniecie obecnej wizualizacji wroga z ekranu

                RandomDirection(); // Wylosowanie nowego kierunku ruchu wroga

                Point positionAux = Position;
                Movement(ref positionAux); // Wykonanie ruchu wroga na podstawie wylosowanego kierunku
                Collisions(positionAux); // Obsluga kolizji, ktora zapobiega wychodzeniu wroga poza obszar gry

                Draw(); // Narysowanie zaktualizowanej wizualizacji wroga na ekranie

                timeMovement = DateTime.Now;
            }

            if (TypeEnemyE != TypeEnemy.Menu) 
            {
                CreateBullets(); // Tworzenie pociskow przez wroga
                Shoot(); // Wystrzelenie pociskow w kierunku gracza
            }
        }

        public void Movement(ref Point positionAux)
        {
            // Obliczenie nowej pozycji wroga w zaleznoci od aktualnego kierunku
            switch (direction)
            {
                case Direction.Right:
                    positionAux.X += 1; // Przesuń pozycję wroga o 1 w prawo
                    break;
                case Direction.Left:
                    positionAux.X -= 1;
                    break;
                case Direction.Up:
                    positionAux.Y -= 1; // Przesuń pozycję wroga o 1 w górę
                    break;
                case Direction.Down:
                    positionAux.Y += 1;
                    break;
            }
        }


        public void Collisions(Point positionAux)  // Obsługa kolizji w przypadku wychodzenia poza granice ekranu
        {
            int width = 3; // Zakres w jakim wrogowie mogą poruszać sie na ekranie
            if (TypeEnemyE == TypeEnemy.Boss)
                width = 7;

            int lowerLimit = WindowC.UpperLimit.Y + 15; // 15 pikseli poniżej górnej granicy obszaru gry

            if (TypeEnemyE == TypeEnemy.Menu)
                lowerLimit = WindowC.LowerLimit.Y - 1; // 1 piksel powyżej dolnej granicy obszaru gry 



            if (positionAux.X <= WindowC.UpperLimit.X) // Sprawdzenie, czy wrog nie opuścił obszaru gry
            {
                direction = Direction.Right;
                positionAux.X = WindowC.UpperLimit.X + 1;
            }
            if (positionAux.X + width >= WindowC.LowerLimit.X)
            {
                direction = Direction.Left;
                positionAux.X = WindowC.LowerLimit.X - 1 - width;
            }
            if (positionAux.Y <= WindowC.UpperLimit.Y)
            {
                direction = Direction.Down;
                positionAux.Y = WindowC.UpperLimit.Y + 1;
            }
            if (positionAux.Y + 2 >= lowerLimit)
            {
                direction = Direction.Up;
                positionAux.Y = lowerLimit - 2;
            }

            Position = positionAux; // Aktualizowaie pozycji na podstawie nowej, aby uwzględnić dokonane korekty w celu uniknięcia opuszczenia obszaru 

        }


        public void RandomDirection() // Losowa zmiana kierunku poruszania się wroga
        {
            // Warunek sprawdza, czy wystarczył już czas na losową zmianę kierunku ruchu w poziomie
            if (DateTime.Now > timeDirection.AddMilliseconds(timeDirectionRandom)
                && (direction == Direction.Right || direction == Direction.Left))
            {

                Random random = new Random();
                int randomNumer = random.Next(1, 5);

                switch (randomNumer) // Losowanie liczby od 1 do 4, aby określić nowy kierunek
                {
                    case 1: direction = Direction.Right; break;

                    case 2: direction = Direction.Left; break;

                    case 3: direction = Direction.Up; break;

                    case 4: direction = Direction.Down; break;
                }
                // Aktualizacja czasu i czasu losowania dla kolejnej zmiany kierunku
                timeDirection = DateTime.Now;
                timeDirectionRandom = random.Next(1000, 2000);

            }
            if (DateTime.Now > timeDirection.AddMilliseconds(50) // Warunek sprawdza, czy wystarczył już czas na losową zmianę kierunku w pionie
                && (direction == Direction.Up || direction == Direction.Down))
            {

                Random random = new Random();
                int randomNumer = random.Next(1, 3);

                switch (randomNumer)
                {
                    case 1: direction = Direction.Right; break;

                    case 2: direction = Direction.Left; break; // Przypisanie losowego kierunku w pionie
                }

                timeDirection = DateTime.Now; 

            }
        }
        public void CreateBullets()
        {

            if (DateTime.Now > timeShoot.AddMilliseconds(timeShootRandom)) // Warunek sprawdza, czy wystarczył już czas na stworzenie nowego pocisku
            {
                Random random = new Random();

                if (TypeEnemyE == TypeEnemy.Normal) // Dla wrogów typu "Normal", tworzony jest pocisk z pozycją odpowiednio przesuniętą w prawo
                {
                    Bullet bullet = new Bullet(
                        new Point(Position.X + 1, Position.Y + 2),
                        Color,
                        BulletType.Enemy);

                    Bullets.Add(bullet); // Dodanie nowego pocisku do listy pocisków wroga
                    timeShootRandom = random.Next(200, 500); // Losowanie czasu, po jakim zostanie stworzony kolejny pocisk
                }
                if (TypeEnemyE == TypeEnemy.Boss) // Dla wrogów typu "Boss", tworzony jest inny rodzaj pocisku z pozycją przesuniętą bardziej w prawo
                {
                    Bullet bullet = new Bullet(
                        new Point(Position.X + 4, Position.Y + 2),
                        Color,
                        BulletType.Enemy);

                    Bullets.Add(bullet);  // Dodanie nowego pocisku do listy pocisków wroga
                    timeShootRandom = random.Next(100, 150); // Losowanie czasu, po jakim zostanie stworzony kolejny pocisk
                }

                timeShoot = DateTime.Now; // Aktualizacja czasu ostatniego stworzenia pocisk
            }

        }

        public void Shoot() // Iteruje przez listę pocisków wroga 
        {
            for (int i = 0; i < Bullets.Count; i++)
            {

                if (Bullets[i].Move(1, WindowC.LowerLimit.Y, ShipC))// Jeśli pocisk przekroczył dolny limit obszaru grylub trafił w statek gracza
                {
                    Bullets.Remove(Bullets[i]); // Pocisk jest usuwany z listy
                }
            }
        }
        public void Information(int distanceX) // Informacje o stanie wroga
        {

            Console.ForegroundColor = Color; //  Kolor tekstu na kolor wroga
            Console.SetCursorPosition(WindowC.UpperLimit.X + distanceX, WindowC.UpperLimit.Y - 1); 
            Console.Write("ENEMY: " + (int)Health + " %  ");
        }

        public void Death() // Smierc wrogow
        {
            if (TypeEnemyE == TypeEnemy.Normal)
            {
                NormalDeath();
            }
            if (TypeEnemyE == TypeEnemy.Boss)
            {
                BossDeath();
            }
        }


        public void NormalDeath() // Smierc w przypadku wroga normalnego 
        {
            Console.ForegroundColor = ConsoleColor.White; // Kolor tekstu

            int x = Position.X; // Zapisanie pozycji wroga na planszy
            int y = Position.Y;

            Console.SetCursorPosition(x + 1, y); // Wyświetlenie animacji śmierci wroga w postaci znaków ASCII
            Console.Write("▄▄Zzz");
            Console.SetCursorPosition(x, y + 1);
            Console.Write("████");
            Console.SetCursorPosition(x, y + 2);
            Console.Write("▀  ▀");


            PositionsEnemy.Clear(); // Usuniecie wroga


            foreach (Bullet b in Bullets)
            {
                b.Delete();
            }

            Bullets.Clear(); // Usunięcie pocisków
        }


        public void BossDeath()
        {
            Console.ForegroundColor = Color; //  Kolor tekstu na kolor wroga


            foreach (Point p in PositionsEnemy) // Iteracja przez wszystkie pozycje wroga i zamiana na inne znaki
            {
                Console.SetCursorPosition(p.X, p.Y);
                Console.Write("▓");
                Thread.Sleep(150); // Oczekiwanie 200ms miedzy kolejnymi krokami animacji
            }
            foreach (Point p in PositionsEnemy) // Czyszczenie pozycji wroga
            {
                Console.SetCursorPosition(p.X, p.Y);
                Console.Write(" ");
                Thread.Sleep(100);
            }
            PositionsEnemy.Clear(); // Usuniecie bossa


            foreach (Bullet b in Bullets)
            {
                b.Delete(); // Usuniecie pociskow
            }
            Console.ForegroundColor = ConsoleColor.White; // Przywrócenie koloru tekstu na biały


        }
    }
}

