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
        private void LogMessage(string message)
        {
            ConsoleTextBox.Text += message + "\n";
        }
        private bool isGridCreated = false;
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (isGridCreated)
            {
                LogMessage("Grid has already been created.");
                return;
            }
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

                }
            }
            isGridCreated = true;
        }

        private void Cell_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ((Rectangle)sender).Fill = (((Rectangle)sender).Fill == Brushes.WhiteSmoke) ? Brushes.Black : Brushes.WhiteSmoke;
            //throw new NotImplementedException();
        }

        private void Draw_Click(object sender, RoutedEventArgs e)
        {

        }

        private void GliderButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BeaconButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Erase_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void Draw_Checked(object sender, RoutedEventArgs e)
        {
           
        }
        
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            theCanvas.Children.Clear();

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

                }
            }            
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
