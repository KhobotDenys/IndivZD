using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace AnimatedShapesApp
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer;
        private Random random;
        private int animationSpeed = 50; // Задайте швидкість анімації тут (в мілісекундах)

        public MainWindow()
        {
            InitializeComponent();
            InitializeTimer();
            random = new Random();
        }

        private void InitializeTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(animationSpeed);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            MoveShapes();
        }

        private void MoveShapes()
        {
            foreach (UIElement shape in canvas.Children)
            {
                double newX = Canvas.GetLeft(shape) + random.Next(-5, 6); // Випадкове зміщення по X
                double newY = Canvas.GetTop(shape) + random.Next(-5, 6); // Випадкове зміщення по Y

                if (newX < 0) newX = 0;
                if (newY < 0) newY = 0;
                if (newX + shape.RenderSize.Width > canvas.ActualWidth) newX = canvas.ActualWidth - shape.RenderSize.Width;
                if (newY + shape.RenderSize.Height > canvas.ActualHeight) newY = canvas.ActualHeight - shape.RenderSize.Height;

                Canvas.SetLeft(shape, newX);
                Canvas.SetTop(shape, newY);
            }
        }

        private void AddShape_Click(object sender, RoutedEventArgs e)
        {
            if (ShapeTypeComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a shape from the dropdown list.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Shape shape = null;

            switch (ShapeTypeComboBox.SelectedItem)
            {
                case ComboBoxItem rectangleItem when rectangleItem.Content.ToString() == "Rectangle":
                    shape = new Rectangle();
                    break;
                case ComboBoxItem ellipseItem when ellipseItem.Content.ToString() == "Ellipse":
                    shape = new Ellipse();
                    break;
                case ComboBoxItem triangleItem when triangleItem.Content.ToString() == "Triangle":
                    shape = CreateTriangle();
                    break;
                default:
                    break;
            }

            if (shape != null)
            {
                shape.Width = 50;
                shape.Height = 50;
                shape.Fill = Brushes.Blue;

                canvas.Children.Add(shape);
                Canvas.SetLeft(shape, random.Next((int)canvas.ActualWidth));
                Canvas.SetTop(shape, random.Next((int)canvas.ActualHeight));
            }
        }



        private Polygon CreateTriangle()
        {
            PointCollection points = new PointCollection();
            points.Add(new Point(0, 50));
            points.Add(new Point(50, 50));
            points.Add(new Point(25, 0));

            Polygon triangle = new Polygon();
            triangle.Points = points;
            triangle.Fill = Brushes.Green;

            return triangle;
        }

        private void ChangeColor_Click(object sender, RoutedEventArgs e)
        {
            foreach (UIElement shape in canvas.Children)
            {
                if (shape is Shape)
                {
                    SolidColorBrush brush = new SolidColorBrush(Color.FromRgb((byte)random.Next(256), (byte)random.Next(256), (byte)random.Next(256)));
                    (shape as Shape).Fill = brush;
                }
            }
        }

        private void ChangeSpeed_Click(object sender, RoutedEventArgs e)
        {
            if (animationSpeed > 10) // Мінімальна швидкість
                animationSpeed -= 10;
            timer.Interval = TimeSpan.FromMilliseconds(animationSpeed);
        }

        private void ShapeTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
