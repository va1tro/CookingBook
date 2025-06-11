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
    /// Логика взаимодействия для Autorization.xaml
    /// </summary>
    public partial class Autorization : Page
    {
        public Autorization()
        {
            InitializeComponent();
        }

        private void btnAu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var userObj = AppData.AppConnect.model01.Authors.FirstOrDefault(
                    x => x.Login == tbLogin.Text && x.Password == psbPassword.Password);

                if (userObj == null)
                {
                    MessageBox.Show("Такого пользователя нет", "Ошибка авторизации",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
                if (string.IsNullOrEmpty(tbLogin.Text) || string.IsNullOrEmpty(psbPassword.Password))
                {
                    MessageBox.Show("Введите логин и пароль", "Ошибка авторизации",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                else
                {
                    // Сохраняем текущего пользователя
                    AppConnect.CurrentUser = userObj;

                    MessageBox.Show("Здравствуйте, Автор " + userObj.AuthorName + "!",
                        "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);

                    NavigationService.Navigate(new PageRecipes());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка" + ex.Message.ToString(),
                    "Критическая ошибка приложения", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        //private void btnReg_Click(object sender, RoutedEventArgs e)
        //{
        //    MainWindow1.frmMain.Navigate(new Registration());
        //}
        private void btnReg_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Registration());
        }
    }
}
