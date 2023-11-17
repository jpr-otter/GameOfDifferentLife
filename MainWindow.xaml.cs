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

        private void IsPainting()
        {
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

            int[] dx = { -1, -1, -1, 0, 0, 1, 1, 1 };
            int[] dy = { -1, 0, 1, -1, 1, -1, 0, 1 };

            for (int rows = 0; rows < amountOfCellsX; rows++)
            {
                for (int cols = 0; cols < amountOfCellsY; cols++)
                {
                    int neighbourCounter = 0;
                    for (int i = 0; i < 8; i++)
                    {
                        int nx = (rows + dx[i] + amountOfCellsX) % amountOfCellsX;
                        int ny = (cols + dy[i] + amountOfCellsY) % amountOfCellsY;
                        if (cells[nx, ny].Fill == Brushes.Black)
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
                    }
                }
            }
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
            mousePressed = true;

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
                    /*
                    for (int rows = 0; rows < amountOfCellsX; rows++)
                    {
                        for (int cols = 0; cols < amountOfCellsY; cols++)
                        {
                            int neighbourCounter = 0;

                            int rowsAbove = rows - 1;
                            if(rowsAbove < 0)
                            {
                                rowsAbove = amountOfCellsY - 1;
                            }

                            int rowsBelow = rows + 1;
                            if (rowsBelow  >= amountOfCellsY)
                            {
                                rowsBelow = 0;
                            }

                            int colsLeft = cols - 1;
                            if (colsLeft < 0)
                            {
                                colsLeft = amountOfCellsX -1;
                            }

                            int colsRight = cols + 1;
                            if (colsRight >= amountOfCellsX)
                            {
                                colsRight = 0;
                            }

                            if (cells[rowsAbove, colsLeft].Fill == Brushes.Black)
                            {
                                neighbourCounter++;
                            }
                            if (cells[rowsAbove, cols].Fill == Brushes.Black)
                            {
                                neighbourCounter++;
                            }
                            if (cells[rowsAbove, colsRight].Fill == Brushes.Black)
                            {
                                neighbourCounter++;
                            }

                            if (cells[rows, colsLeft].Fill == Brushes.Black)
                            {
                                neighbourCounter++;
                            }
                            if (cells[rows, colsRight].Fill == Brushes.Black)
                            {
                                neighbourCounter++;
                            }

                            if (cells[rowsBelow, colsLeft].Fill == Brushes.Black)
                            {
                                neighbourCounter++;
                            }
                            if (cells[rowsBelow, cols].Fill == Brushes.Black)
                            {
                                neighbourCounter++;
                            }
                            if (cells[rowsBelow, colsRight].Fill == Brushes.Black)
                            {
                                neighbourCounter++;
                            }

                            amountOfNeighbours[rows, cols] = neighbourCounter;
                        }
                    }
                    */
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
