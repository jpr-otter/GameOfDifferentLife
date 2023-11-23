using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace GameOfDifferentLife
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            cells = new Rectangle[amountOfCellsX, amountOfCellsY];

            //timer.Interval = TimeSpan.FromSeconds(1 / SpeedSlider.Value);
            //GameInfos($"Cells alive: {totalAlive}\nCells dead: {totalDead}");
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            UpdateCells();
        }
        private void SpeedSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            
            timer.Interval = TimeSpan.FromSeconds(1 / SpeedSlider.Value);
        }

        private bool isGridCreated = false;
        private bool drawboxIsChecked = false;
        private bool eraseboxIsChecked = false;
        private bool mousePressed = false;
        private bool timerIsRunning = false;
        private bool isGliderMode = false;
        int userSetSize;

        int amountOfCellsX = 40;
        int amountOfCellsY = 40;
        int totalAlive = 0;
        int totalDead = 0;

        Rectangle[,] cells; //= new Rectangle[amountOfCellsX, amountOfCellsY];
        DispatcherTimer timer = new DispatcherTimer();
        Random randomNumber = new Random();
        private double cellCanvasWidth;
        private double cellCanvasHeight;

        public void SetSize_Click(object sender, RoutedEventArgs e)
        {
            // need to find out how to set the new size chosen by the player
            bool isInteger = int.TryParse(Size.Text, out userSetSize);
            if (isInteger && userSetSize > 2)
            {
                amountOfCellsX = userSetSize;
                amountOfCellsY = userSetSize;
            }
            else
            {
                LogMessage("Please input an integer greater than 2.");
            }           

        }              

        private void LogMessage(string message)
        {
            ConsoleTextBox.Clear();
            ConsoleTextBox.Text += message + "\n";               
        }
        private void GameInfos (string message)
        {
            // what other information about the game could be presented to inform the player?
            CellCountTextBox.Clear();            
            CellCountTextBox.Text += message + "\n";
        }

        private void IsPainting()
        {
            // need to find a solution for printing larger patters of alive cells to the canvas.
            // click a beacon or glider, and let i hover above the canvas, click to make it final and paint it onto the canvas
            for (int rows = 0; rows < amountOfCellsX; rows++)
            {
                for (int cols = 0; cols < amountOfCellsY; cols++)
                {
                    Rectangle cell = new Rectangle();
                    var cellCanvasWidth = theCanvas.ActualWidth / amountOfCellsX;
                    var cellCanvasHeight = theCanvas.ActualWidth / amountOfCellsY;
                    double cellSizeReduction = 2.0;

                    cell.Width = (cellCanvasWidth) - cellSizeReduction;
                    cell.Height = (cellCanvasHeight) - cellSizeReduction;
                    cell.Fill = Brushes.WhiteSmoke;
                    theCanvas.Children.Add(cell);
                    Canvas.SetLeft(cell, cols * cellCanvasWidth);
                    Canvas.SetTop(cell, rows * cellCanvasHeight);

                    cell.MouseDown += Cell_MouseDown;
                    cell.MouseMove += Cell_MouseMove;
                    cell.MouseUp += Cell_MouseUp;

                    cells[rows, cols] = cell;

                }
            }
        }

        private void randomPainting()
        {
            // dont know how to implement the random painter. lost track of how to do it!
            for (int rows = 0; rows < amountOfCellsX; rows++)
            {
                for (int cols = 0; cols < amountOfCellsY; cols++)
                {
                    Rectangle cell = new Rectangle();
                    var cellCanvasWidth = theCanvas.ActualWidth / amountOfCellsX;
                    var cellCanvasHeight = theCanvas.ActualWidth / amountOfCellsY;
                    double cellSizeReduction = 2.0;

                    cell.Width = (cellCanvasWidth) - cellSizeReduction;
                    cell.Height = (cellCanvasHeight) - cellSizeReduction;
                    cell.Fill = randomNumber.Next(0, 2) == 1 ? Brushes.WhiteSmoke : Brushes.Black;
                    theCanvas.Children.Add(cell);
                    Canvas.SetLeft(cell, cols * cellCanvasWidth);
                    Canvas.SetTop(cell, rows * cellCanvasHeight);

                    //cell.MouseDown += Cell_MouseDown;
                    //cell.MouseMove += Cell_MouseMove;
                    //cell.MouseUp += Cell_MouseUp;

                    //cells[rows, cols] = cell;

                }
            }
        }

        private void UpdateCells()
        {
            int[,] amountOfNeighbours = new int[amountOfCellsX, amountOfCellsY];            

            int[] offSetX = { -1, -1, -1, 0, 0, 1, 1, 1 };
            int[] offSetY = { -1, 0, 1, -1, 1, -1, 0, 1 };

            for (int rows = 0; rows < amountOfCellsX; rows++)
            {
                for (int cols = 0; cols < amountOfCellsY; cols++)
                {
                    int neighbourCounter = 0;
                    for (int i = 0; i < 8; i++)
                    {
                        int neighbourX = (rows + offSetX[i] + amountOfCellsX) % amountOfCellsX;
                        int neighbourY = (cols + offSetY[i] + amountOfCellsY) % amountOfCellsY;
                        if (cells[neighbourX, neighbourY].Fill == Brushes.Black)
                        {
                            neighbourCounter++;                           
                        }
                    }
                    amountOfNeighbours[rows, cols] = neighbourCounter;
                }
            }

            for (int rows = 0; rows < amountOfCellsX; rows++)
            {
                for (int cols = 0; cols < amountOfCellsY; cols++)
                {
                    if (amountOfNeighbours[rows, cols] < 2 || amountOfNeighbours[rows, cols] > 3)
                    {
                        cells[rows, cols].Fill = Brushes.WhiteSmoke;                        
                    }
                    else if (amountOfNeighbours[rows, cols] == 3)
                    {
                        cells[rows, cols].Fill = Brushes.Black;
                        totalAlive++;
                    }
                }
            }
            // dont know where to put this? and i need to figure out how to count the cells in a way that makes sense
            // probably better to count cells that are alive or come to life and count only those a dead that have been alive before
            //GameInfos($"Cells alive: {totalAlive}\nCells dead: {totalDead}");

        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (isGridCreated)
            {
                LogMessage("Grid has already been created.");
                return;
            }
            IsPainting();            
            isGridCreated = true;
        }

        private void Cell_MouseDown(object sender, MouseButtonEventArgs e)
        {

            ((Rectangle)sender).Fill = (((Rectangle)sender).Fill == Brushes.WhiteSmoke) ? Brushes.Black : Brushes.WhiteSmoke;
            //(((Rectangle)sender).Fill == Brushes.WhiteSmoke) ? totalAlive++ : totalDead++;
            if (((Rectangle)sender).Fill == Brushes.WhiteSmoke){
                totalDead++;
            }
            else 
            { 
                totalAlive++;
                //totalAlive--;
            }
            mousePressed = true;

            GameInfos($"Cells alive: {totalAlive}\nCells dead: {totalDead}");

            //throw new NotImplementedException();
        }

        private void Draw_Click(object sender, RoutedEventArgs e)
        {

        }
        private void Cell_MouseMove(object sender, MouseEventArgs e)
        {
            if (drawboxIsChecked && !eraseboxIsChecked && mousePressed)
            {
                //((Rectangle)sender).Fill = (((Rectangle)sender).Fill == Brushes.WhiteSmoke) ? Brushes.Black : Brushes.WhiteSmoke;
                ((Rectangle)sender).Fill = Brushes.Black;
                totalAlive++;

            }
            else if (eraseboxIsChecked && !drawboxIsChecked && mousePressed)
            {
                ((Rectangle)sender).Fill = Brushes.WhiteSmoke;
            }
        }
        private void Cell_MouseUp(object sender, MouseButtonEventArgs e)
        {
            mousePressed = false;
        }

        private void GliderButton_Click(object sender, RoutedEventArgs e)
        {
            isGliderMode = true;
        }

        private void BeaconButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Erase_Checked(object sender, RoutedEventArgs e)
        {
            eraseboxIsChecked = true;
            if (eraseboxIsChecked)
            {
                LogMessage("You are in 'erase' mode");
                return;
            }
        }

        private void Erase_Unchecked(object sender, RoutedEventArgs e)
        {
            eraseboxIsChecked = false;
            if (!eraseboxIsChecked)
            {
                ConsoleTextBox.Clear();
                return;
            }
        }

        private void Draw_Checked(object sender, RoutedEventArgs e)
        {
            drawboxIsChecked = true;
            if (drawboxIsChecked)
            {
                LogMessage("You are in 'draw' mode");
                return;
            }
            //isDrawing = true;
        }
        private void Draw_Unchecked(object sender, RoutedEventArgs e)
        {
            drawboxIsChecked = false;
            mousePressed = false;
            if (!drawboxIsChecked)
            {
                ConsoleTextBox.Clear();
                return;
            }
        }
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            theCanvas.Children.Clear();

            IsPainting();
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Forward_Click(object sender, RoutedEventArgs e)
        {
            UpdateCells();
        }

        private void Cycle_Click(object sender, RoutedEventArgs e)
        {                     
            if (!timerIsRunning)
            {
                timer.Tick += Timer_Tick;
                timer.Start();
                timerIsRunning = true;
                (sender as Button).Content = "Stop Cycle";
                LogMessage("You are running cycles.");
            }
            else
            {
                timer.Stop();
                timer.Tick -= Timer_Tick;
                (sender as Button).Content = "Start Cycle";
                LogMessage("You stopped the cycles.");
                timerIsRunning = false;
            }            
        }

        private void RandomPattern_Click(object sender, RoutedEventArgs e)
        {
            randomPainting();            
        }
        //private void Canvas_MouseMove(object sender, MouseEventArgs e)
        //{
        //    if (isGliderMode)
        //    {
        //        // Get mouse position
        //        Point mousePos = e.GetPosition(theCanvas);

        //        // Calculate cell position
        //        int cellX = (int)(mousePos.X / cellCanvasWidth);
        //        int cellY = (int)(mousePos.Y / cellCanvasHeight);

        //        // Draw temporary glider shape at cell position
        //        DrawGlider(cellX, cellY, Brushes.Gray);
        //    }
        //}
        //private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    if (isGliderMode)
        //    {
        //        // Get mouse position
        //        Point mousePos = e.GetPosition(theCanvas);

        //        // Calculate cell position
        //        int cellX = (int)(mousePos.X / cellCanvasWidth);
        //        int cellY = (int)(mousePos.Y / cellCanvasHeight);

        //        // Draw permanent glider shape at cell position
        //        DrawGlider(cellX, cellY, Brushes.Black);

        //        // Exit glider mode
        //        isGliderMode = false;
        //    }
        //}
        //private void DrawGlider(int x, int y, Brush color)
        //{
        //    // Draw glider shape at (x, y)
        //    // You need to check the boundaries of the grid
        //    cells[x + 1, y].Fill = color;
        //    cells[x + 2, y + 1].Fill = color;
        //    cells[x, y + 2].Fill = color;
        //    cells[x + 1, y + 2].Fill = color;
        //    cells[x + 2, y + 2].Fill = color;
        //}
    }
}
