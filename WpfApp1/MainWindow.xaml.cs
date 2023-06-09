﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public Item currentItem { get; set; }
        public List<Item> images { get; set; }
        public List<Item> Items { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            currentItem = new Item();
            var itemProvider = new ItemProvider();
            Items = new List<Item>();
            foreach (var name in Environment.GetLogicalDrives())
            {
                var temp = itemProvider.GetItems(name);
                Items.AddRange(temp);
               
            }//C:\\ tez dziala ale laduje sie wieki 
            DataContext = this;


        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string info)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(info));
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            currentItem = new Item();
            OnPropertyChanged("currentItem");
            ListBox.Items.Clear();
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.ShowDialog();
            string sFileName = dialog.SelectedPath;
            var itemProvider = new ItemProvider();
            var temp = itemProvider.GetItems(sFileName);
            images = new List<Item>();
            DirectoryItem directory = (DirectoryItem)temp[0];
            foreach (var item in directory.Items) {
                if (item.IsJPG)
                {
                    images.Add(item);
                    AddImagesToBlocks(item);
                }
            }
           

        }
        private void AddImagesToBlocks(Item tt)
        {
            var border = new Border();
            border.Background = Brushes.White;
            border.BorderBrush = Brushes.Gray;
            border.BorderThickness = new Thickness(1);
            border.Margin = new Thickness(5);

            var temp = new ListBoxItem();
            temp.DataContext = tt;
            temp.MouseLeftButtonUp += ListBox_MouseLeftButtonDown;

            var grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            var image = new Image();
            image.Stretch = Stretch.Uniform;

            if (tt.IsJPG)
            {
                var uri = new Uri(tt.Path);
                var bitmap = new BitmapImage(uri);
                image.Source = bitmap;
            }

            var textBlock = new TextBlock();
            textBlock.Text = tt.Name;
            textBlock.VerticalAlignment = VerticalAlignment.Center;

            grid.Children.Add(image);
            Grid.SetRow(image, 0);
            grid.Children.Add(textBlock);
            Grid.SetRow(textBlock, 1);

            temp.Content = grid;

            border.Child = temp;

            // Apply shadow effect to the border
            var dropShadowEffect = new DropShadowEffect();
            dropShadowEffect.Color = Colors.Black;
            dropShadowEffect.Opacity = 0.5;
            dropShadowEffect.Direction = 270;
            dropShadowEffect.ShadowDepth = 5;
            dropShadowEffect.BlurRadius = 10;
            border.Effect = dropShadowEffect;

            ListBox.Items.Add(border);

        }

        private void ListBox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            currentItem =(Item) (((ListBoxItem) sender).DataContext);
            OnPropertyChanged("currentItem");
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {

            this.IsEnabled = false;

           
            SlideShowWindow slideshowWindow = new SlideShowWindow(images,TransitionEffect.Horizontal);
            slideshowWindow.Owner = this;
            slideshowWindow.ShowDialog();
            this.IsEnabled = true;

        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;


            SlideShowWindow slideshowWindow = new SlideShowWindow(images, TransitionEffect.Vertical);
            slideshowWindow.Owner = this;
            slideshowWindow.ShowDialog();
            this.IsEnabled = true;
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;


            SlideShowWindow slideshowWindow = new SlideShowWindow(images, TransitionEffect.Opacity);
            slideshowWindow.Owner = this;
            slideshowWindow.ShowDialog();
            this.IsEnabled = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxItem typeItem = (ComboBoxItem)ComboBox.SelectedItem;
            if((string)typeItem.Content== "Opacity")
            {
                this.IsEnabled = false;


                SlideShowWindow slideshowWindow = new SlideShowWindow(images, TransitionEffect.Opacity);
                slideshowWindow.Owner = this;
                slideshowWindow.ShowDialog();
                this.IsEnabled = true;
            }
            else if ((string)typeItem.Content == "Verticaly")
            {
                this.IsEnabled = false;


                SlideShowWindow slideshowWindow = new SlideShowWindow(images, TransitionEffect.Vertical);
                slideshowWindow.Owner = this;
                slideshowWindow.ShowDialog();
                this.IsEnabled = true;
            }
            else if ((string)typeItem.Content == "Horizontaly")
            {
                this.IsEnabled = false;


                SlideShowWindow slideshowWindow = new SlideShowWindow(images, TransitionEffect.Horizontal);
                slideshowWindow.Owner = this;
                slideshowWindow.ShowDialog();
                this.IsEnabled = true;
            }
            

        }
    }

    public class Item
    {
        public string NoFile { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public bool IsJPG = false;
        public string Width { get; set; }
        public string Height { get; set; }
        public string Size  { get; set; }
    public override string ToString()
        {
            if (Name == null)
                return "NO FILE PROVIDED";
            else
                return "name: " ;
        }
        public Item()
        {
            NoFile = "No file Sselected!";
        }
    }
    public class FileItem : Item
    {
        
       
    }
    public class DirectoryItem : Item
    {
        public List<Item> Items { get; set; }

        public DirectoryItem()
        {
            Items = new List<Item>();
        }
    }
    public class ItemProvider
    {
        public List<Item> GetItems(string path, int depth=1)
        {
            var items = new List<Item>();
            var dir = new DirectoryItem();
            dir.Name= path;
            dir.Path= path;
            items.Add(dir);

            var dirInfo = new DirectoryInfo(path);
            try
            {
                foreach (var directory in dirInfo.GetDirectories())
                {
                    var item = new DirectoryItem
                    {
                        Name = directory.Name,
                        Path = directory.FullName
                        

                    };
                    if (depth < 3)
                        item.Items = GetItems(directory.FullName, depth + 1);
                    dir.Items.Add(item);
                }

                foreach (var file in dirInfo.GetFiles())
                {
                    var item = new FileItem
                    {
                        Name = file.Name,
                        Path = file.FullName,
                        Size = (((float)file.Length)/1000).ToString()
                         
                    };
                    if (file.Extension == ".jpg")
                    {
                        item.IsJPG = true;
                        var uri = new Uri(item.Path);
                        var bitmap = new BitmapImage(uri);
                        Image image = new Image();
                        image.Source = new ImageSourceConverter().ConvertFromString(item.Path) as ImageSource;
                        item.Width = bitmap.PixelWidth.ToString();
                        item.Height = bitmap.PixelHeight.ToString();
                        item.NoFile = null;

                    }
                    dir.Items.Add(item);
                }
            }
            catch { }
            return items;
        }
    }
    public enum TransitionEffect
    {
        None,
        Horizontal,
        Vertical,
        Opacity
    }
}
