using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace dtp8_MUD_0
{
    class Room
    {
        // Constants:
        public const int North = 0;
        public const int East = 1;
        public const int South = 2;
        public const int West = 3;
        public const int NoDoor = -1;

        // Object attributes:
        public int number;
        public string roomname = "";
        public string story = "";
        public string imageFile = "";
        public int[] adjacent = new int[4]; // adjacent[Room.North] etc.
        public Room(int num, string name)
        {
            number = num; roomname = name;
        }
        public void SetStory(string theStory)
        {
            story = theStory;
        }
        public void SetImage(string theImage)
        {
            imageFile = theImage;
        }
        public void SetDirections(int N, int E, int S, int W)
        {
            adjacent[North] = N; adjacent[East] = E; adjacent[South] = S; adjacent[West] = W;
        }
        public int GetNorth() => adjacent[North];        
        public int GetEast() => adjacent[East];
        public int GetSouth() => adjacent[South];
        public int GetWest() => adjacent[West];
    }
    class Hero
    {
        public string name;
        public int healthPoints = 100;
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Room[] labyrinth = new Room[100];
        int currentRoom;
        public MainWindow()
        {
            InitializeComponent();

            // Init Rooms here:
            Room R;
            R = new Room(0, "Startrummet");
            R.SetStory("Du står i ett rum med rött tegel. Väggarna fladdrar i facklornas sken. Du ser en hög med tyg nere till vänster. ");
            R.SetImage("ingang-stangd.png");
            R.SetDirections(N: 1, E: Room.NoDoor, S: Room.NoDoor, W: Room.NoDoor);
            labyrinth[0] = R;

            R = new Room(1, "Korsvägen");
            R.SetStory("Du står i korsväg. Det går gångar i alla riktningar. ");
            R.SetImage("vagskal.png");
            R.SetDirections(N: 2, E: 3, S: 0, W: 4);
            labyrinth[1] = R;

            R = new Room(2, "ett litet rum med en trappa norrut");
            R.SetStory("Du hör skrämmande ljud från nedre våningen");
            R.SetImage("trappa.png");
            R.SetDirections(N: Room.NoDoor, E: Room.NoDoor, S: 1, W: Room.NoDoor);
            labyrinth[2] = R;

            R = new Room(3, "Skruttigt rum med en låst dörr"); //Ändra låst till öppen om den öppnats med nyckel TBD
            R.SetStory("Dörren norrut är låst");//samma som ovan TBD
            R.SetImage("ingang-stangd.png");
            R.SetDirections(N: 3, E: Room.NoDoor, S: 1, W: Room.NoDoor);
            labyrinth[3] = R;

            R = new Room(4, "rum med monster");
            R.SetStory("HJÄLP! VÄND OM!");
            R.SetImage("septopus.png");
            R.SetDirections(N: Room.NoDoor, E: Room.NoDoor, S: 1, W: Room.NoDoor);
            labyrinth[4] = R;

            currentRoom = 0;
            DisplayCurrentRoom();
        }

        private void ApplicationKeyPress(object sender, KeyEventArgs e)
        {
            string output = "Key pressed: ";
            output += e.Key.ToString();
            // output += ", ";
            // output += AppDomain.CurrentDomain.BaseDirectory;
            KeyPressDisplay.Text = output;
            try
            {
                if (e.Key == Key.Escape)
                {
                    System.Windows.Application.Current.Shutdown();
                }
                else if (e.Key == Key.Up)
                {
                    currentRoom = labyrinth[currentRoom].GetNorth();
                    DisplayCurrentRoom();
                }
                else if (e.Key == Key.Down)
                {
                    currentRoom = labyrinth[currentRoom].GetSouth();
                    DisplayCurrentRoom();
                }
                else if (e.Key == Key.Right)
                {
                    currentRoom = labyrinth[currentRoom].GetEast();
                    DisplayCurrentRoom();
                }
                else if (e.Key == Key.Left)
                {
                    currentRoom = labyrinth[currentRoom].GetWest();
                    DisplayCurrentRoom();
                }
            }
            catch (System.IndexOutOfRangeException)
            {
                currentRoom = 0;
                DisplayCurrentRoom();
            }
        }
        private void DisplayCurrentRoom()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            Room R = labyrinth[currentRoom];
            string bitmapFileName = $"{baseDir}/{R.imageFile}";
            if (File.Exists(bitmapFileName))
            {
                Uri uri = new Uri(bitmapFileName, UriKind.RelativeOrAbsolute);
                RoomImage.Source = BitmapFrame.Create(uri);
            }
            string text = $"Du befinner dig i {R.roomname}. ";
            text += R.story+" ";
            if (R.GetNorth() != Room.NoDoor) text += "Det finns en gång norrut. ";
            if (R.GetEast() != Room.NoDoor) text += "Det finns en gång österut. ";
            if (R.GetSouth() != Room.NoDoor) text += "Det finns en gång söderut. ";
            if (R.GetWest() != Room.NoDoor) text += "Det finns en gång västerut. ";
            RoomText.Text = text;

            /* Andra texter som man kan sätta:
             * KeyAlt1.Text = "upp      gå norrut"; 
             * KeyAlt2.Text = "vänster  gå västerut"; 
             * ... etc. ...
             */
        }
    }
}
