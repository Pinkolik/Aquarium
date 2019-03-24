using System;
using System.Collections.Generic;
using System.Drawing;
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
using System.Windows.Threading;
using AquariumLogic.AquariumClass;
using AquariumLogic.FishClass;
using Point = System.Windows.Point;
using Size = System.Windows.Size;


namespace AquariumUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IAquarium aquarium;
        private DispatcherTimer refreshTimer;

        public MainWindow()
        {
            InitializeComponent();
            //ImageDrawingExample();
        }

        public void ImageDrawingExample()
        {
            // Create a DrawingGroup to combine the ImageDrawing objects.
            DrawingGroup imageDrawings = new DrawingGroup();

            // Create a 100 by 100 image with an upper-left point of (75,75). 
            ImageDrawing bigKiwi = new ImageDrawing();
            bigKiwi.Rect = new Rect(75, 75, 100, 100);
            bigKiwi.ImageSource = new BitmapImage(new Uri(@"Resources/fish1.png", UriKind.Relative));

            imageDrawings.Children.Add(bigKiwi);

            // Create a 25 by 25 image with an upper-left point of (0,150). 
            ImageDrawing smallKiwi1 = new ImageDrawing();
            smallKiwi1.Rect = new Rect(0, 150, 25, 25);
            smallKiwi1.ImageSource = new BitmapImage(new Uri(@"Resources/fish2.png", UriKind.Relative));
            imageDrawings.Children.Add(smallKiwi1);

            // Create a 25 by 25 image with an upper-left point of (150,0). 
            ImageDrawing smallKiwi2 = new ImageDrawing();
            smallKiwi2.Rect = new Rect(150, 0, 25, 25);
            smallKiwi2.ImageSource = new BitmapImage(new Uri(@"Resources/fish3.png", UriKind.Relative));
            imageDrawings.Children.Add(smallKiwi2);

            // Create a 75 by 75 image with an upper-left point of (0,0). 
            ImageDrawing wholeKiwi = new ImageDrawing();
            wholeKiwi.Rect = new Rect(0, 0, 75, 75);
            wholeKiwi.ImageSource = new BitmapImage(new Uri(@"Resources/fish4.png", UriKind.Relative));
            imageDrawings.Children.Add(wholeKiwi);

            //
            // Use a DrawingImage and an Image control to
            // display the drawings.
            //
            DrawingImage drawingImageSource = new DrawingImage(imageDrawings);

            // Freeze the DrawingImage for performance benefits.
            drawingImageSource.Freeze();

            BackgroundImage.Source = drawingImageSource;

            //Image imageControl = new Image();
            //imageControl.Stretch = Stretch.None;
            //imageControl.Source = drawingImageSource;

            //// Create a border to contain the Image control.
            //Border imageBorder = new Border();
            //imageBorder.BorderBrush = Brushes.Gray;
            //imageBorder.BorderThickness = new Thickness(1);
            //imageBorder.HorizontalAlignment = HorizontalAlignment.Left;
            //imageBorder.VerticalAlignment = VerticalAlignment.Top;
            //imageBorder.Margin = new Thickness(20);
            //imageBorder.Child = imageControl;

            //this.Background = Brushes.White;
            //this.Margin = new Thickness(20);
            //this.Content = imageBorder;
        }

        public void InitializeAquarium()
        {
            var aquariumSize = new Size(this.ActualWidth, this.ActualHeight);
            aquarium = new Aquarium(aquariumSize, new Uri("pack://application:,,,/Resources/background.png"));
            refreshTimer = new DispatcherTimer {Interval = new TimeSpan(0, 0, 0, 0, 100)};
            refreshTimer.Tick += RedrawAquarium;
            refreshTimer.Start();
        }

        //private void Background_MouseLeftButtonDown(object sender,  MouseButtonEventArgs e)
        //{
        //    if (aquarium == null) InitializeAquarium();
        //    var fish = new Fish(100, 180, 0, 20);
        //    var pos = e.GetPosition(this);
        //    var aquariumObject = new AquariumObject(new Point((int)pos.X, (int)pos.Y), new Size(10, 10), Properties.Resources.fish1);
        //    aquarium.AddFish(fish, aquariumObject);
        //}

        private void RedrawAquarium(object sender, EventArgs args)
        {
            aquarium.Iterate();

            DrawingGroup aquariumImages = new DrawingGroup();

            ImageDrawing background = new ImageDrawing();
            background.Rect = new Rect(aquarium.Size);
            background.ImageSource = new BitmapImage(aquarium.BackgroundImageUri);
            aquariumImages.Children.Add(background);

            foreach (var fish in aquarium.Fishes)
            {
                ImageDrawing fishImage = new ImageDrawing();
                fishImage.Rect = new Rect(fish.Value.Position, fish.Value.Size);
                fishImage.ImageSource = new BitmapImage(fish.Value.TextureUri);
                aquariumImages.Children.Add(fishImage);
            }

            DrawingImage drawingImageSource = new DrawingImage(aquariumImages);

            drawingImageSource.Freeze();

            BackgroundImage.Source = drawingImageSource;
        }

        private void BackgroundImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (aquarium == null) InitializeAquarium();
            var fish = new Fish(100, 180, 0, 20);
            var pos = e.GetPosition(BackgroundImage);
            var aquariumObject = new AquariumObject(new Point(pos.X, pos.Y),
                new Size(100, 100),
                new Uri("pack://application:,,,/Resources/fish1.png"));
            aquarium.AddFish(fish, aquariumObject);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeAquarium();
        }

        //private BitmapImage BitmapToImageSource(Bitmap bitmap)
        //{
        //    using (var memory = new MemoryStream())
        //    {
        //        bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
        //        memory.Position = 0;
        //        BitmapImage bitmapImage = new BitmapImage();
        //        bitmapImage.BeginInit();
        //        bitmapImage.StreamSource = memory;
        //        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
        //        bitmapImage.EndInit();

        //        return bitmapImage;
        //    }
        //}
    }
}
