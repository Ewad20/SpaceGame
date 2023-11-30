using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;

namespace SpaceGame
{
    internal class Window
    {
        public int Width { get; set; } // Szerokość okna gry
        public int Height { get; set; } // Wysokość okna gry
        public ConsoleColor Color { get; set; } // Kolor tekstu na konsoli
        public Point UpperLimit { get; set; } // Górny limit obszaru gry
        public Point LowerLimit { get; set; } // Dolny limit obszaru gry

        private Enemy _enemy1; // Prywatne pole przechowujące wroga 
        private Enemy _enemy2;


        private List<Bullet> _bullets; // Prywatna lista przechowująca pociski
        private int selectedMenuItem = 0; // Aktualnie wybrany element w menu gry

        public Window(int w, int h, ConsoleColor c, Point upperL, Point loweL) 
        {

            this.Width = w; // Szerokość okna gry
            this.Height = h; // Wysokość okna gry
            this.Color = c; //  Kolor tekstu na konsoli
            this.UpperLimit = upperL; //  Górny limit obszaru gry
            this.LowerLimit = loweL; // Dolny limit obszaru gry

            Init();
        }

        private void Init()
        {

            Console.SetWindowSize(Width, Height); // Rozmiar okna konsoli na podstawie wartości które zostały przekazane jako parametry konstruktora
            Console.Title = "Space Game"; //  Ustawia tytuł okna konsoli 
            Console.BackgroundColor = Color; // Ustawia kolor tła konsoli
            Console.CursorVisible = false; // Ukrywa kursor w oknie konsoli
            Console.Clear();
            _enemy1 = new Enemy(new Point(50, 10), ConsoleColor.DarkYellow, this, TypeEnemy.Menu, null);
            _enemy2 = new Enemy(new Point(100, 30), ConsoleColor.DarkGray, this, TypeEnemy.Menu, null);
            _bullets = new List<Bullet>(); // Lista przechowująca pociski używane w grze

        }

        public void DrawFrame()
        {
            Console.ForegroundColor = ConsoleColor.White; // Kolor ramki

            for (int i = UpperLimit.X; i <= LowerLimit.X; i++) // Rysowanie linii poziomych ramki
            {


                Console.SetCursorPosition(i, UpperLimit.Y); // Ustawienie kursora i rysowanie linii poziomej na górnej krawędzi
                Console.Write("═");

                Console.SetCursorPosition(i, LowerLimit.Y); // Na dole
                Console.Write("═");
            }

            for (int i = UpperLimit.Y; i <= LowerLimit.Y; i++) // Rysowanie linii pionowych ramki
            {
                Console.SetCursorPosition(UpperLimit.X, i); // Ustawienie kursora i rysowanie linii pionowej na lewej krawędzi
                Console.Write("║");

                Console.SetCursorPosition(LowerLimit.X, i); // Prawej krawędzi
                Console.Write("║");
            }
            // Rysowanie rogów ramki
            // Lewy górny róg
            Console.SetCursorPosition(UpperLimit.X, UpperLimit.Y);
            Console.Write("╔");

            Console.SetCursorPosition(UpperLimit.X, LowerLimit.Y);
            Console.Write("╚");    // Lewy dolny róg

            Console.SetCursorPosition(LowerLimit.X, UpperLimit.Y);
            Console.Write("╗");


            Console.SetCursorPosition(LowerLimit.X, LowerLimit.Y);
            Console.Write("╝");
        }


        public void Danger()
        {
            Console.Clear();     // Wyczyść konsolę 
            Thread.Sleep(100); // Poczekaj 100 milisekund
            DrawFrame();    // // Narysuj ramkę na nowo po wyczyszczeniu konsoli

            // Wyświetl napis na środku ekranu
            Console.SetCursorPosition(LowerLimit.X / 2 - 6, LowerLimit.Y - 18);
            Title("Level completed", 100);

            // Wyświetl informację o zdobytych bonusach
            Console.SetCursorPosition(LowerLimit.X / 2 - 5, LowerLimit.Y - 16);
            Title("You have received:", 100 );

            Thread.Sleep(500);  // Poczekaj 500 milisekund

            // Wyświetl bonusy z opóźnieniem
            Console.SetCursorPosition(LowerLimit.X / 2 - 6, LowerLimit.Y - 13);
            Console.Write("+ 40 Life");

            Thread.Sleep(250);
            Console.SetCursorPosition(LowerLimit.X / 2 - 6, LowerLimit.Y - 11);
            Console.Write("+ 30 Super Shot");

            Thread.Sleep(2000);  // Poczekaj 2 sekundy
            for (int i = 0; i < 6; i++)
            { 
                Console.ForegroundColor = ConsoleColor.Red;     // Wyświetl komunikat "BE CAREFUL!!" migający na czerwono
                Console.SetCursorPosition(LowerLimit.X / 2 - 2, LowerLimit.Y - 7);
                Console.Write("BE CAREFUL!!");
                Thread.Sleep(250);
                Console.SetCursorPosition(LowerLimit.X / 2 - 2, LowerLimit.Y - 7);
                Console.Write("         ");   // Wyczyść napis - odświeżanie tylko elemntow ktore ulegly zmianie
                Thread.Sleep(200);

            }

            Console.Clear(); 
            Thread.Sleep(100); 
            DrawFrame();
        }


        public void Menu()
        {
            _enemy1.Move(); // Przesuń obiekty Enemy w celu animacji ruchu na pasku menu
            _enemy2.Move();

            // Wyświetl tytuł gry oraz opcje menu
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.SetCursorPosition(LowerLimit.X / 2 - 30, LowerLimit.Y / 2 - 10);
            Console.Write("  ____                             ____                          ");
            Console.SetCursorPosition(LowerLimit.X / 2 - 30, LowerLimit.Y / 2 - 9);
            Console.Write(" / ___|  _ __    __ _   ___  ___  / ___|  __ _  _ __ ___    ___  ");
            Console.SetCursorPosition(LowerLimit.X / 2 - 30, LowerLimit.Y / 2 - 8);
            Console.Write(" \\___ \\ | '_ \\  / _` | / __|/ _ \\| |  _  / _` || '_ ` _ \\  / _ \\ ");
            Console.SetCursorPosition(LowerLimit.X / 2 - 30, LowerLimit.Y / 2 - 7);
            Console.Write("  ___) || |_) || (_| || (__|  __/| |_| || (_| || | | | | ||  __/ ");
            Console.SetCursorPosition(LowerLimit.X / 2 - 30, LowerLimit.Y / 2 - 6);
            Console.Write(" |____/ | .__/  \\__,_| \\___|\\___| \\____| \\__,_||_| |_| |_| \\___| ");
            Console.SetCursorPosition(LowerLimit.X / 2 - 30, LowerLimit.Y / 2 - 5);
            Console.Write("        |_|                                                      ");

            // Wyświetl opcje menu i podświetl aktualnie zaznaczoną na zielono
            for (int i = 0; i < 3; i++)
            {
                Console.SetCursorPosition(LowerLimit.X / 2 - 6, LowerLimit.Y / 2 + 2 * i);
                if (i == selectedMenuItem)
                {
                    // Podświetl aktualnie zaznaczoną opcję na zielono
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }

                if (i == 0)
                {
                    Console.Write("Play");
                }
                else if (i == 1)
                {
                    Console.Write("Instruction");
                }
                else if (i == 2)
                {
                    Console.Write("Finish");
                }
            }
        }


        public void Keyboard(ref bool active, ref bool play) // Monitoruje naciśnięcia klawiszy przez użytkownika w menu 
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Enter)  // Obsługuje zdarzenie naciśnięcia klawisza "Enter".
                                                  // W zależności od wybranej opcji w menu, podejmuje odpowiednie akcje
                {

                    Thread.Sleep(1000);

                    if (selectedMenuItem == 0)
                    {
                        Console.Clear(); // Rozpocznij grę: Wyczyść ekran, wyświetl intro i ustaw flagę gry na "true"
                        Thread.Sleep(150);
                        DrawFrame();
                        Introduction();
                        play = true;
                    }
                    else if (selectedMenuItem == 1)
                    {
                        play = false; // Wyświetl instrukcje gry: Ustaw flagę gry na "false" i wyświetl instrukcje
                        Instructions();
                        Console.Clear();
                        Thread.Sleep(100);
                        DrawFrame();
                    }
                    else if (selectedMenuItem == 2) 
                    {
                        active = false; // Zakończ grę: Ustaw flagę aktywności gry na "false"
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.SetCursorPosition(LowerLimit.X / 2 - 10, LowerLimit.Y / 2 + 7);
                        
                    }
                }
                else if (key.Key == ConsoleKey.UpArrow)
                {
                    // Przesuń się do góry po menu
                    if (selectedMenuItem > 0)
                    {
                        selectedMenuItem--;
                    }
                }
                else if (key.Key == ConsoleKey.DownArrow)
                {
                    // Przesuń się w dół po menu
                    if (selectedMenuItem < 2)
                    {
                        selectedMenuItem++;
                    }
                }
            }
        }
        public void Instructions()
        {
            Console.Clear(); // Wyczyść ekran, odczekaj i narysuj ramkę

            Thread.Sleep(100);

            DrawFrame();

            // Wyświetl instrukcje gry, wskazując na misję gracza i sterowanie
            Console.SetCursorPosition(LowerLimit.X / 2 - 19, LowerLimit.Y - 24);
            Console.Write("Your mission is not only to eliminate external threats,");
            Console.SetCursorPosition(LowerLimit.X / 2 - 24, LowerLimit.Y - 23);
            Console.Write("but also discover the very essence of your existence");
            Console.SetCursorPosition(LowerLimit.X / 2 - 35, LowerLimit.Y - 22);
            Console.Write("Aren't your enemies reflections of your own desires, fears and passions?");
            Console.SetCursorPosition(LowerLimit.X / 2 - 30, LowerLimit.Y - 21);
            Console.Write("What meaning does war have in the immense canvas of the cosmos?");

            Console.SetCursorPosition(LowerLimit.X / 2 - 32, LowerLimit.Y - 16);
            Console.Write("[W] Up");
            Console.SetCursorPosition(LowerLimit.X / 2 - 32, LowerLimit.Y - 14);
            Console.Write("[S] Down");
            Console.SetCursorPosition(LowerLimit.X / 2 - 32, LowerLimit.Y - 12);
            Console.Write("[A] Left");
            Console.SetCursorPosition(LowerLimit.X / 2 - 32, LowerLimit.Y - 10);
            Console.Write("[D] Right");

            Console.SetCursorPosition(LowerLimit.X - 35, LowerLimit.Y - 15);
            Console.Write("[^] Super Shot ");
            Console.SetCursorPosition(LowerLimit.X - 35, LowerLimit.Y - 13);
            Console.Write("[>] Right shot");
            Console.SetCursorPosition(LowerLimit.X - 35, LowerLimit.Y - 11);
            Console.Write("[<] Left shot");

            // Wyświetl wizualizację statku gracza
            Console.SetCursorPosition(LowerLimit.X - 56, LowerLimit.Y - 14);
            Console.Write("A");
            Console.SetCursorPosition(LowerLimit.X - 58, LowerLimit.Y - 13);
            Console.Write("<{x}>");
            Console.SetCursorPosition(LowerLimit.X - 59, LowerLimit.Y - 12);
            Console.Write("± W W ±");

            Console.SetCursorPosition(LowerLimit.X / 2 - 16, LowerLimit.Y - 4);
            Console.Write("Press any key to return...");


            Console.ReadKey(); // Oczekuj na dowolny klawisz od gracza

        }

        public void Introduction()
        {
            // Wyświetl wprowadzenie do gry z wizualizacją tekstu
            Console.SetCursorPosition(LowerLimit.X / 2 - 10, LowerLimit.Y - 17);
            Title("In my restless dreams,", 80);
            Console.SetCursorPosition(LowerLimit.X / 2 - 9, LowerLimit.Y - 15);
            Title("I see that planet...", 80);
            Thread.Sleep(1500);

            // Poczekaj 1,5 sekundy i wyczyść wprowadzenie
            Console.SetCursorPosition(LowerLimit.X / 2 - 10, LowerLimit.Y - 17);
            Console.Write("                       ");
            Console.SetCursorPosition(LowerLimit.X / 2 - 9, LowerLimit.Y - 15);
            Console.Write("                       ");


        }

        public void SecondLevel()
        {
            Console.Clear(); // Wyczyść ekran i poczekaj 
            Thread.Sleep(100);
            DrawFrame();

            Thread.Sleep(500);

            // Wyświetl informacje o ukończonym poziomie
            Console.SetCursorPosition(LowerLimit.X / 2 - 6, LowerLimit.Y - 18);
            Title("Level completed", 100);

            Console.SetCursorPosition(LowerLimit.X / 2 - 5, LowerLimit.Y - 16);
            Title("Have you received:", 100);

            Thread.Sleep(500);

            // Wyświetl informacje o zdobytych
            Console.SetCursorPosition(LowerLimit.X / 2 - 6, LowerLimit.Y - 13);
            Console.Write("+ 20 Health");

            Thread.Sleep(250);
            Console.SetCursorPosition(LowerLimit.X / 2 - 6, LowerLimit.Y - 11);
            Console.Write("+ 25 Super Shot");

            Thread.Sleep(2000);
            
            // Wyczyść ekran po wyświetleniu informacji
            Console.Clear();
            Thread.Sleep(100);
            DrawFrame();

        }

        public void Title(string text, int speed) // Speed określa jak szybko pojawią się znaki
        {
            foreach (char caracter in text) // Dla każdego znaku w tekście
            {
                Console.Write(caracter); // Wyświetl znak na ekranie
                Thread.Sleep(speed); // Poczekaj przez określony czas(opóźnienie między znakami)
            }
        }

        public void End()
        {

            Thread.Sleep(1500); // Poczekaj przez 1,5 sekundy
            Console.Clear();

            Thread.Sleep(100);  // Poczekaj przez 100 milisekund i narysuj ramkę okna

            DrawFrame();

            // Wyświetl teksty na ekranie z opóźnieniem 
            Console.SetCursorPosition(LowerLimit.X / 2 - 7, LowerLimit.Y - 21);
            Title("I do this", 200);

            Console.SetCursorPosition(LowerLimit.X / 2 - 10, LowerLimit.Y - 19);
            Title("But... at what cost?", 200);

            Thread.Sleep(300);

            // Wyświetl symbole z opóźnieniem
            Console.SetCursorPosition(LowerLimit.X - 96, LowerLimit.Y - 14);
            Title("A", 100);
            Console.SetCursorPosition(LowerLimit.X - 98, LowerLimit.Y - 13);
            Title("<{x}>", 100);
            Console.SetCursorPosition(LowerLimit.X - 99, LowerLimit.Y - 12);
            Title("± W W ±", 100);

            Thread.Sleep(1500);

        }

    }
} 