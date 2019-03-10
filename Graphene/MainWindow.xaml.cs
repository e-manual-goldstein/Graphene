using System.Windows;
using System.Windows.Controls;
using System;
using System.Windows.Media;
using System.Linq;
using System.Windows.Shapes;
using System.Collections.Generic;
using Graphene.Geometry;
using Graphene.Lattice;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.IO;

namespace Graphene
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Canvas Tableau { get; set; }
        public BaseGrid Grid { get; set; }
        public static MainController MainController { get; set; }
        public static DisplayProgrammaticService DisplayService { get; set; }
        public static SystemProgrammaticService SystemService { get; set; }
        public static Dictionary<string, Type> AllServices { get; set; }
        //public static DisplayController DisplayController { get; set; }
        //public static ArtefactController ArtefactController { get; set; }
        public static List<HexLine> Lights { get; set; }
        public const int UnitSize = 15;
        private static bool metronomeActive;

        public static Stopwatch Stopwatch { get; set; }
        public TextBox InputBox { get; set; }
        public static List<long> TickCounts { get; set; }

        public static double Period { get; set; }
        public static double BPM { get; set; }

        public static TextBlock Label { get; set; }

        public MainWindow()
        {
            Stopwatch = new Stopwatch();
            Stopwatch.Start();
            TickCounts = new List<long>();
            Lights = new List<HexLine>();
            MainController = new MainController();
            var initialisedServices = MainController.InitialiseServices(this);
            //DisplayService = initialisedServices.OfType<DisplayProgrammaticService>().FirstOrDefault();
            //DisplayController = new DisplayController();
            InitializeComponent();
            loadPreviousSettings();
            Tableau = new Canvas();
            Grid = new BaseGrid(Height, Width, Tableau);
            Tableau.Width = Width;
            Tableau.Height = Height;

            Tableau.Background = Brushes.LightGray;
            Content = Tableau;
            var greenLattice = new TriangularLattice(Grid, Orientation.Horizontal, LatticeTypeEnum.Green,true);
            if (Hex.ShowGridLines)
            {
                createGridBuilder(greenLattice);
            }
            var testPoint = new Ellipse();
            testPoint.Height = 20;
            testPoint.Width = 20;
            testPoint.Fill = Brushes.Chartreuse;
            Grid.AddShape(testPoint, greenLattice.Sites[20].Location);
            

          


            Label = new TextBlock() { Text = "000.00", FontSize = 15 };
            Canvas.SetLeft(Label, 500);
            Tableau.Children.Add(Label);
            
            //var label = new TextBlock() { Text = Period.ToString() };
            //label.
            //Canvas.SetLeft(label,400);
            //Tableau.Children.Add(label);
            Lights.AddRange(greenLattice.GridLines.Values);
            //var redLattice = new TriangularLattice(Grid, Orientation.Horizontal, LatticeTypeEnum.Red);
            // var blueLattice = new TriangularLattice(Grid, Orientation.Horizontal, LatticeTypeEnum.Blue);
            // var blueLattice = new TriangularLattice(Grid, 20, Orientation.Vertical, LatticeTypeEnum.Blue);
            //var redLattice = new TriangularLattice(Grid,10, Orientation.Vertical, LatticeTypeEnum.Red);

            //var cellSize = 50;
            //var gridOrigin = new Coord(cellSize * 2, cellSize * 2);
            //var hGrid = new HexGrid(15, 15, cellSize, gridOrigin);
            //DrawGrid(hGrid,addVertexIds:true);
            //Console.WriteLine(hGrid.AllVertices.Count);
            //Console.WriteLine(hGrid.AllChannels.Count);
            Closing += mainWindowClosing;
        }

        private void loadPreviousSettings()
        {
            var settings = AppSettings.Instance;
            if (settings.Custom)
            {
                Top = settings.WindowTop;
                Left = settings.WindowLeft;
            }
        }

        private void createGridBuilder(TriangularLattice lattice)
        {
            var gridBuilder = Task.Run(() =>
            {
                foreach (var site in lattice.Sites)
                {
                    //App.Current.Dispatcher.Invoke(() => lattice.AddGridLines(site.Value));
                    Thread.Sleep(1000);
                }
            });
        }

        private void mainWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            metronomeActive = false;
            saveWindowLocation(this.Top, this.Left);
        }

        private void saveWindowLocation(double top, double left)
        {
            var appSettings = AppSettings.Instance;
            appSettings.WindowTop = top;
            appSettings.WindowLeft = left;
            appSettings.Custom = true;
            AppSettings.SaveSettingsToFile();
        }

        private void resetButtonClick(object sender, RoutedEventArgs e)
        {
            TickCounts.Clear();
            if (!string.IsNullOrEmpty(InputBox.Text))
            {
                Period = 60000 / double.Parse(InputBox.Text);
                InputBox.Text = null;
                updateLabel();
            }
        }

        private void tapButtonClick(object sender, RoutedEventArgs e)
        {
            long elapsed = Stopwatch.ElapsedMilliseconds;
            Task.Run(() => recalculateBeat());
            TickCounts.Add(elapsed);
        }

        private void recalculateBeat()
        {
            if (TickCounts.Count > 1)
            {
                var last30 = TickCounts.Reverse<long>().Take(30).ToArray();
                var differences = new long[last30.Count() - 1];
                for (int i = 1; i < last30.Count(); i++)
                {
                    differences[i-1] = last30[i-1] - last30[i];
                }
                Period = (differences.Average());
            }
        }

        private void metronome()
        {

            while (metronomeActive)
            {
                Thread.Sleep((int)Period);
                BPM = 60000.0 / Period;
                Task.Run(() => flash());
                updateLabel();
                
            }
        }

        private void updateLabel()
        {
            App.Current.Dispatcher.Invoke(() => Label.Text = "Period = " + Period + "\nBPM = " + BPM.ToString("000.00"));
        }

        private void metronomeClick(object sender, RoutedEventArgs e)
        {
            metronomeActive = !metronomeActive;
            var startEvent = Task.Run(() => metronome());
            
        }

        public void DrawGrid(HexGrid hGrid, int cellSize = 50, bool addVertexIds = false)
        {
            if (addVertexIds)
                foreach (var vertex in hGrid.AllVertices)
                {
                    var ellipse = new TextBlock() { FontSize = cellSize / 5, Text = vertex.Id.ToString() };
                    Canvas.SetLeft(ellipse, vertex.Center.X);
                    Canvas.SetTop(ellipse, vertex.Center.Y);
                    Tableau.Children.Add(ellipse);
                }
            foreach (var channel in hGrid.AllChannels)
            {
                DisplayService.DrawLine(Tableau, channel.Path, Colors.Black);
            }
        }

        private void flash()
        {
            var activeLights = Lights.Where(l => l.Flash);
            if (activeLights.Any())
            {
                App.Current.Dispatcher.Invoke(() => lightsOn(activeLights));
                Thread.Sleep((int)(Period * 0.3));
                App.Current.Dispatcher.Invoke(() => lightsOff(activeLights));
            }
        }

        private void lightsOn(IEnumerable<HexLine> lights)
        {
            foreach (var light in lights)
            {
                light.SwitchOn();
            }
        }

        private void lightsOff(IEnumerable<HexLine> lights)
        {
            foreach (var light in lights)
            {
                light.SwitchOff();
            }
        }

        private void addButtons()
        {

            //int lineId = 0;
            ////greenLattice.AddGridLine(Dimension.Gamma, greenLattice.Sites[98].Location, lineId++, Grid);
            var metronomeSwitch = new Button();
            metronomeSwitch.Height = 30;
            metronomeSwitch.Width = 100;
            metronomeSwitch.Content = "On/Off";
            metronomeSwitch.Click += metronomeClick;
            Tableau.Children.Add(metronomeSwitch);

            var tapButton = new Button();
            tapButton.Height = 30;
            tapButton.Width = 100;
            tapButton.Content = "Tap";
            tapButton.Click += tapButtonClick;
            Canvas.SetLeft(tapButton, 100);
            Tableau.Children.Add(tapButton);

            var resetButton = new Button();
            resetButton.Height = 30;
            resetButton.Width = 100;
            resetButton.Content = "(Re)Set";
            resetButton.Click += resetButtonClick;
            Canvas.SetLeft(resetButton, 200);
            Tableau.Children.Add(resetButton);

            InputBox = new TextBox();
            InputBox.Height = 30;
            InputBox.Width = 80;
            Canvas.SetLeft(InputBox, 300);
            Tableau.Children.Add(InputBox);
        }
    }
}
