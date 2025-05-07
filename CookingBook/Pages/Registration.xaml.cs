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
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Page
    {
        public Registration()
        {
            InitializeComponent();
        }

        private void btnReg_Click(object sender, RoutedEventArgs e)
        {
            if (AppData.AppConnect.model01.Authors.Count(x => x.Login.ToString() == txtLogin.Text) > 0)
            {
                MessageBox.Show("Пользователь с таким логином уже есть!",
                    "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            try
            {
            Authors userObj = new Authors()
            {
                AuthorName = txtLogin.Text,
                Login = txtLogin.Text,
                Password = tbPass.Password,
                DateBirth = dateB.SelectedDate.Value,
                Stazh = txtstazh.Text,
                Mail = txtmail.Text,
                Phone = txtphone.Text
            };
                AppData.AppConnect.model01.Authors.Add(userObj);
                AppData.AppConnect.model01.SaveChanges();
                MessageBox.Show("Данные успешно добавлены!",
                    "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch
            {
                MessageBox.Show("Ошибка при добавлении данных!",
                    "Уведомление", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frmMain.GoBack();
        }
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (tbPass.Password != btnPassP.Password)
            {
                btnReg.IsEnabled = false;
                tbPass.Background = Brushes.LightCoral;
                tbPass.BorderBrush = Brushes.Red;
            }
            else
            {
                btnReg.IsEnabled = true;
                tbPass.Background = Brushes.LightGreen; ;
                tbPass.BorderBrush = Brushes.Green;
            }
        }


    }
}
