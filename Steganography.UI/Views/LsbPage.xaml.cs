using System;
using System.Collections.Generic;
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
using Microsoft.Win32;

namespace Steganography.UI.Views
{
    /// <summary>
    /// Interaction logic for LsbPage.xaml
    /// </summary>
    public partial class LsbPage : Page
    {
        public LsbPage()
        {
            InitializeComponent();
        }

        private void OpenImageButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Title = "Select a picture",
                Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                         "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                         "Portable Network Graphic (*.png)|*.png"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                OriginalImage.Source = new BitmapImage(new Uri(openFileDialog.FileName));
            }
        }

        private void ReadMessageFromFileButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Title = "Select file",
                Filter = "Text files| *.txt"
            };
            if (openFileDialog.ShowDialog() != true) return;
            using (var stream = new StreamReader(openFileDialog.FileName))
            {
                MessageTextBox.Text = stream.ReadToEnd();
            }
        }

        private void SaveMessageFromFileButton_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Title = "Select file",
                Filter = "Text files| *.txt"
            };
            if (saveFileDialog.ShowDialog() != true) return;
            using (var stream = new StreamWriter(saveFileDialog.FileName))
            {
                stream.Write(MessageTextBox.Text);
            }
        }
    }
}
