using CookingBook.AppData;
using Microsoft.Win32;
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

namespace CookingBook.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageCart.xaml
    /// </summary>
    public partial class PageCart : Page
    {
        private Authors currentUser;
        private Entities db = AppConnect.GetContext();

        public PageCart(Authors user)
        {
            InitializeComponent();
            currentUser = user;
            LoadCart();
        }

        private void LoadCart()
        {
            var cartItems = db.Cart.Where(c => c.AuthorID == currentUser.AuthorID).ToList();
            lvCart.ItemsSource = cartItems;

            decimal total = cartItems.Sum(c => c.Recipes.Price * c.Quantity);
            tbTotal.Text = $"Итого: {total} руб.";
        }

        private void BtnRemove_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as Button)?.Tag as Cart;
            if (item != null)
            {
                db.Cart.Remove(item);
                db.SaveChanges();
                LoadCart();
            }
        }

        private void BtnCheckout_Click(object sender, RoutedEventArgs e)
        {
            var cartItems = db.Cart.Where(c => c.AuthorID == currentUser.AuthorID).ToList();
            if (!cartItems.Any())
            {
                MessageBox.Show("Корзина пуста!");
                return;
            }

            decimal totalAmount = cartItems.Sum(c => c.Recipes.Price * c.Quantity);
            var order = new Orders
            {
                AuthorID = currentUser.AuthorID,
                OrderDate = DateTime.Now,
                TotalAmount = totalAmount,
                Status = "Новый",
                DeliveryAddress = "Адрес по умолчанию",
                Phone = "Телефон по умолчанию"
            };
            db.Orders.Add(order);
            db.SaveChanges();

            // Добавляем позиции
            foreach (var cart in cartItems)
            {
                var item = new OrderItems
                {
                    OrderID = order.OrderID,
                    RecipeID = cart.RecipeID,
                    Quantity = cart.Quantity,
                    Price = cart.Recipes.Price
                };
                db.OrderItems.Add(item);
            }

            // Сохраняем и очищаем корзину
            db.Cart.RemoveRange(cartItems);
            db.SaveChanges();

            ExportReceiptToCSV(order);

            MessageBox.Show("Заказ оформлен!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            LoadCart();
        }

        private void ExportReceiptToCSV(Orders order)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "CSV файлы (*.csv)|*.csv",
                FileName = $"order_{order.OrderID}_{DateTime.Now:yyyyMMddHHmmss}.csv"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                var sb = new StringBuilder();
                sb.AppendLine("Рецепт;Количество;Цена за единицу;Сумма");

                foreach (var item in order.OrderItems)
                {
                    sb.AppendLine($"{item.Recipes.RecipeName};{item.Quantity};{item.Price};{item.Quantity * item.Price}");
                }

                sb.AppendLine($";;;Итого:;{order.TotalAmount}");

                File.WriteAllText(saveFileDialog.FileName, sb.ToString(), Encoding.UTF8);
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }
    }
}
