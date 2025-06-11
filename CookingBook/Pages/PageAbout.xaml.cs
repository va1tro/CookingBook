using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
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
using ZXing;
using ZXing.Common;
using ZXing.Rendering;

namespace CookingBook.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageAbout.xaml
    /// </summary>
    public partial class PageAbout : Page
    {
        public PageAbout()
        {
            InitializeComponent();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Btn_qrcode_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var writer = new BarcodeWriter<WriteableBitmap>
                {
                    Format = BarcodeFormat.QR_CODE,
                    Options = new EncodingOptions
                    {
                        Width = 300,
                        Height = 300,
                        Margin = 1
                    },
                    Renderer = new WriteableBitmapRenderer()
                };

                // Ссылка на плейлист
                var url = @"https://music.yandex.ru/users/rud-ev/playlists/1005?utm_medium=share_link_tg";

                // Генерация QR-кода
                var qrCodeBitmap = writer.Write(url);
                imgQr.Source = qrCodeBitmap;
                Btn_qrcode.Content = "Обновить QR-код";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка генерации QR-кода: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
