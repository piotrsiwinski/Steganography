using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using Steganography.UI.Algorithms;

namespace Steganography.UI.Views
{
    /// <summary>
    /// Interaction logic for LsbPage.xaml
    /// </summary>
    public partial class LsbPage : Page
    {
        private BitmapImage _originalBitmapImage;
        private BitmapImage _encryptedBitmapOld;
        private Bitmap _encryptedBitmap;
        private Bitmap _originalBitmap;
        private SteganographyHelper _steganographyHelper;
        public LsbPage()
        {
            InitializeComponent();
            _steganographyHelper = new SteganographyHelper();
        }

        private Bitmap BitmapImage2Bitmap(BitmapImage bitmapImage)
        {
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder(); enc.Frames.Add(BitmapFrame.Create(bitmapImage)); enc.Save(outStream); System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);
                return new Bitmap(bitmap);
            }
        }

        public BitmapSource ToBitmapSource(Bitmap source)
        {
            BitmapSource bitSrc = null;
            var hBitmap = source.GetHbitmap();
            try { bitSrc = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions()); } catch (Exception ex) { MessageBox.Show(ex.Message); }
            return bitSrc;
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
                _originalBitmapImage = new BitmapImage(new Uri(openFileDialog.FileName));
                _originalBitmap = BitmapImage2Bitmap(_originalBitmapImage);
                OriginalImage.Source = _originalBitmapImage;
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

        private void EncryptMessageButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (_originalBitmapImage != null)
            {
                Bitmap myBitmap = BitmapImage2Bitmap(_originalBitmapImage);
                _encryptedBitmap = _steganographyHelper.EmbedText(MessageTextBox.Text, BitmapImage2Bitmap(_originalBitmapImage));
                EncryptedImage.Source = ToBitmapSource(_encryptedBitmap);
            }
        }

        private void DecryptMessageButton_OnClick(object sender, RoutedEventArgs e)
        {
            var result = _steganographyHelper.ExtractText(_encryptedBitmap);
            EncryptedMessageTextBox.Text = result;
        }

        private void SaveImageButton_OnClick(object sender, RoutedEventArgs e)
        {
            //
            var saveFileDialog = new SaveFileDialog
            {
                Title = "Select a picture",
                Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                         "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                         "Portable Network Graphic (*.png)|*.png"
            };

            if (saveFileDialog.ShowDialog() != true) return;
            _encryptedBitmap.Save(saveFileDialog.FileName);

        }

        private void DecryptMessageButton2_OnClick(object sender, RoutedEventArgs e)
        {
            var result = _steganographyHelper.ExtractText(_originalBitmap);
            EncryptedMessageTextBox.Text = result;
        }
    }
}
