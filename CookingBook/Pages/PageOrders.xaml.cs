using CookingBook.AppData;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CookingBook.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageOrders.xaml
    /// </summary>
    public partial class PageOrders : Page
    {
        public PageOrders()
        {
            InitializeComponent();
            LoadOrders();
        }

        private void LoadOrders()
        {
            // Проверяем, что пользователь авторизован
            if (AppConnect.CurrentUser == null)
            {
                MessageBox.Show("Пользователь не авторизован", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                tbEmpty.Visibility = Visibility.Visible;
                tbEmpty.Text = "Необходимо авторизоваться";
                return;
            }

            try
            {
                // Получаем ID текущего пользователя
                int currentUserId = AppConnect.CurrentUser.AuthorID;

                // Загружаем заказы пользователя
                var orders = AppData.AppConnect.model01.Orders
                    .Where(o => o.AuthorID == currentUserId)  // Используем сохраненное значение
                    .OrderByDescending(o => o.OrderDate)
                    .ToList();

                lvOrders.ItemsSource = orders;
                tbEmpty.Visibility = orders.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке заказов: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                tbEmpty.Visibility = Visibility.Visible;
                tbEmpty.Text = "Ошибка загрузки данных";
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
