using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace CsExercise
{
    /// <summary>
    /// Interaction logic for Graph.xaml
    /// </summary>
    public partial class GraphWindow : Window
    {
        List<DailyCurrency> dailyCurrencies;
        public GraphWindow(List<DailyCurrency> dailyCurrencies)
        {
            InitializeComponent();

            this.dailyCurrencies = dailyCurrencies;

        }

        private void DrawGraph()
        {
            try
            {
                var minDate = dailyCurrencies.OrderBy(dc => dc.day).First();
                var maxDate = dailyCurrencies.OrderBy(dc => dc.day).Last();
                var dayNum = (maxDate.day - minDate.day).TotalDays + 1;

                var minCur = dailyCurrencies.OrderBy(dc => dc.currency).First();
                var maxCur = dailyCurrencies.OrderBy(dc => dc.currency).Last();
                var diff = maxCur.currency - minCur.currency;

                int colWidth = (int)mainCanvas.ActualWidth / (int)(mainCanvas.ActualWidth / 100);
                int colNum = (int)(mainCanvas.ActualWidth) / colWidth;
                int rowHeight = (int)mainCanvas.ActualHeight / (int)(mainCanvas.ActualHeight / 50);
                int rowNum = (int)(mainCanvas.ActualHeight) / rowHeight;

                var dayInterval = dayNum / (colNum - 1);
                var curInterval = diff / (rowNum - 1);

                // Columns
                for (int i = 1; i <= colNum; i++)
                {
                    var newLine = new Line();
                    newLine.Stroke = Brushes.Black;
                    newLine.X1 = i * colWidth;
                    newLine.Y1 = 0;
                    newLine.X2 = i * colWidth;
                    newLine.Y2 = mainCanvas.ActualHeight - rowHeight + 5;

                    mainCanvas.Children.Add(newLine);

                    var dateLabel = new TextBlock();
                    dateLabel.Text = minDate.day.AddDays(Math.Round((i - 1) * dayInterval)).ToShortDateString();
                    Canvas.SetLeft(dateLabel, i * colWidth - 25);
                    Canvas.SetTop(dateLabel, mainCanvas.ActualHeight - rowHeight + 5);
                    mainCanvas.Children.Add(dateLabel);
                }

                // Rows
                for (int i = 0; i < rowNum; i++)
                {
                    var newLine = new Line();
                    newLine.Stroke = Brushes.Black;
                    newLine.X1 = colWidth - 5;
                    newLine.Y1 = i * rowHeight;
                    newLine.X2 = mainCanvas.ActualWidth;
                    newLine.Y2 = i * rowHeight;

                    mainCanvas.Children.Add(newLine);

                    var curLabel = new TextBlock();
                    curLabel.Text = Math.Round(minCur.currency + i * curInterval, 2).ToString();
                    Canvas.SetLeft(curLabel, colWidth - 50);
                    Canvas.SetTop(curLabel, i * rowHeight);
                    mainCanvas.Children.Add(curLabel);
                }

                // Graph
                double prevXVal = 0;
                double prevYVal = 0;
                for (int i = 0; i < dailyCurrencies.Count; i++)
                {
                    var curDays = (dailyCurrencies[i].day - minDate.day).TotalDays;
                    var curDiff = dailyCurrencies[i].currency - minCur.currency;

                    // Point
                    Ellipse ellipse = new Ellipse();
                    ellipse.Width = 5;
                    ellipse.Height = 5;
                    ellipse.Fill = Brushes.ForestGreen;
                    ellipse.Stroke = Brushes.ForestGreen;
                    var x = (colWidth + (curDays / dayNum) * (mainCanvas.ActualWidth - colWidth));
                    var y = ((curDiff / diff) * (mainCanvas.ActualHeight - rowHeight));
                    Canvas.SetLeft(ellipse, x - (ellipse.Width / 2));
                    Canvas.SetTop(ellipse, y - (ellipse.Height / 2));

                    mainCanvas.Children.Add(ellipse);

                    // Connecting line
                    if (i > 0)
                    {
                        var line = new Line();
                        line.Stroke = Brushes.LightSteelBlue;

                        line.X1 = prevXVal;
                        line.X2 = x;
                        line.Y1 = prevYVal;
                        line.Y2 = y;

                        line.StrokeThickness = 2;
                        mainCanvas.Children.Insert(0, line);
                    }

                    prevXVal = x;
                    prevYVal = y;
                }
            }
            catch (Exception) { }
        }

        private void MainCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            DrawGraph();
        }

        private void MainCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.mainCanvas.Children.Clear();
            DrawGraph();
        }
    }
}
