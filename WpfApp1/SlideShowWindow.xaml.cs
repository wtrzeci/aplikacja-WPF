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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WpfApp1
{
    /// <summary>
    /// Logika interakcji dla klasy SlideShowWindow.xaml
    /// </summary>
    public partial class SlideShowWindow : Window
    {
        DoubleAnimation animation;
        int WindowWidth = 1024;
        int WindowHeight = 768;
        public int startAnim = 0;
        public TransitionEffect SelectedEffect { get; set; }
        public List<Item> Images { get; set; }
        public int currentIndex;
        public DispatcherTimer timer;
        int innitialTimer = 5;
        int currentTimer = 1;
        public SlideShowWindow(List<Item> images, TransitionEffect transition)
        {
            Images = new List<Item>(images);
            InitializeComponent();
            currentIndex = 0;
            SelectedEffect = transition;
           
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(innitialTimer); //tu ustawaimy ile sekund czekamy
            timer.Tick += Timer_Tick;

           
            timer.Start();


            Item currentImage = Images[currentIndex];

            var uri = new Uri(currentImage.Path);
            var bitmap = new BitmapImage(uri);
            Image image = new Image();
            image.Source = new ImageSourceConverter().ConvertFromString(currentImage.Path) as ImageSource;
            imageControl = image;

            image.Opacity = 1;

            ImageGrid.Children.Clear();


            Grid.SetRow(image, 0);
            Grid.SetColumn(image, 0);
            ImageGrid.Children.Add(image);
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            //timer wziety ze stacka :)
            
           
            if (currentIndex >= Images.Count)
            {
                currentIndex = 0;
                this.Close();
               
            }
            startAnim++;
            if (startAnim % 2 == 1) {
                DisplayCurrentImage(SelectedEffect);
                currentIndex++;
            }
            else
            {

                Item currentImage = Images[currentIndex];

                var uri = new Uri(currentImage.Path);
                var bitmap = new BitmapImage(uri);
                Image image = new Image();
                image.Source = new ImageSourceConverter().ConvertFromString(currentImage.Path) as ImageSource;
                imageControl = image;
                
                image.Opacity = 1; 
                                  
                ImageGrid.Children.Clear();

               
                Grid.SetRow(image, 0); 
                Grid.SetColumn(image, 0); 
                ImageGrid.Children.Add(image);
            }
        }
        private void DisplayCurrentImage(TransitionEffect animationType)
        {
            if (Images.Count > 0 && currentIndex >= 0 && currentIndex < Images.Count)
            {
                Item currentImage = Images[currentIndex];

                var uri = new Uri(currentImage.Path);
                var bitmap = new BitmapImage(uri);
                Image image = new Image();
                image.Source = new ImageSourceConverter().ConvertFromString(currentImage.Path) as ImageSource;
                imageControl = image;
                
                image.Opacity = 0; 
                
                switch (animationType)
                {
                    case TransitionEffect.Horizontal:
                        DoubleAnimation asdas = new DoubleAnimation
                        {
                            From = 0, // Start with zero opacity
                            To = 1,
                            Duration = TimeSpan.FromSeconds(1) // Adjust the duration as needed
                        };
                        image.BeginAnimation(Image.OpacityProperty, asdas);
                        DoubleAnimation yAnimation = new DoubleAnimation
                        {
                            From = 0,
                            To = 1000,
                            Duration = new Duration(TimeSpan.FromSeconds(3)),
                        };
                        TranslateTransform translateTransform = new TranslateTransform();
                        image.RenderTransform = translateTransform;
                        translateTransform.BeginAnimation(TranslateTransform.YProperty, yAnimation);
                        animation = yAnimation;
                        break;

                    case TransitionEffect.Vertical:
                        DoubleAnimation qwdedqw = new DoubleAnimation
                        {
                            From = 0, 
                            To = 1,
                            Duration = TimeSpan.FromSeconds(1) 
                        };
                        image.BeginAnimation(Image.OpacityProperty, qwdedqw);
                        DoubleAnimation asdassada = new DoubleAnimation
                        {
                            From = 0,
                            To = 1000,
                            Duration = new Duration(TimeSpan.FromSeconds(3)),
                        };
                        TranslateTransform asdasdasd = new TranslateTransform();
                        image.RenderTransform = asdasdasd;
                        asdasdasd.BeginAnimation(TranslateTransform.XProperty, asdassada);
                        animation = asdassada;
                        break;

                    case TransitionEffect.Opacity:
                        DoubleAnimation opacityAnimation = new DoubleAnimation
                        {
                            From = 0,
                            To = 1,
                            Duration = TimeSpan.FromSeconds(1) 
                        };
                        image.BeginAnimation(Image.OpacityProperty, opacityAnimation);
                        animation = opacityAnimation;
                        break;

                    default:
                        break;
                }

                
                ImageGrid.Children.Clear();

                
                Grid.SetRow(image, 0); 
                Grid.SetColumn(image, 0); 
                ImageGrid.Children.Add(image);
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void MenuItem_Click1(object sender, RoutedEventArgs e)
        {
            if (currentTimer < 10)
            {
                currentTimer = Int32.MaxValue - 10;
                timer.Interval = TimeSpan.FromSeconds(1000);
            }
            else
            {
                currentTimer = innitialTimer;
                timer.Interval = TimeSpan.FromSeconds(innitialTimer);
            }
           
        }
    }
}
