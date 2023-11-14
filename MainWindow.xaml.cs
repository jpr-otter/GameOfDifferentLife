using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
            
        }

        private bool isGridCreated = false;        
        private bool drawboxIsChecked = false;
        private bool eraseboxIsChecked = false;
        private bool mousePressed = false;
        
        private void LogMessage(string message)
        {
            ConsoleTextBox.Clear();
            ConsoleTextBox.Text += message + "\n";
        }

        private void IsPainting()
        {
            const int amountOfCellsX = 20;
            const int amountOfCellsY = 20;

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
        }
        
        private void Erase_Unchecked(object sender, RoutedEventArgs e)
        {
            eraseboxIsChecked = false;
        }

        private void Draw_Checked(object sender, RoutedEventArgs e)
        {
            drawboxIsChecked = true;
            //isDrawing = true;
        }
        private void Draw_Unchecked(object sender, RoutedEventArgs e)
        {
            drawboxIsChecked = false;            
            mousePressed = false; 
        }
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            theCanvas.Children.Clear();

            IsPainting();     
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {

        }

        
    }
}
