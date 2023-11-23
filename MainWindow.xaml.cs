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
        int userSetSize;

        const int amountOfCellsX = 40;
        const int amountOfCellsY = 40;
        int totalAlive = 0;
        int totalDead = 0;

        public void SetSize_Click(object sender, RoutedEventArgs e)
        {

            //bool isInteger = int.TryParse(Size.Text, out amountOfCellsX);
            //if (isInteger)
            //{
            //    amountOfCellsX = int.Parse(Size.Text);
            //    amountOfCellsY = amountOfCellsX;
            //}
            //else if (!isInteger || userSetSize <= 0)
            //{
            //    LogMessage("Please input a integer greater than 2.");
            //}
            //else
            //{
            //    amountOfCellsX = 20;
            //    amountOfCellsY = 20;
            //}

        }


        DispatcherTimer timer = new DispatcherTimer();

        Rectangle[,] cells = new Rectangle[amountOfCellsX, amountOfCellsY];

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
    }
}
