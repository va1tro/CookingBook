using CookingBook.AppData;
using Microsoft.Win32;
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
    /// Логика взаимодействия для AddEditPages.xaml
    /// </summary>
    public partial class AddEditPages : Page
    {
        public AddEditPages()
        {
            InitializeComponent();
        }

        private Recipes _currentRecipe;

        public AddEditPages(Recipes recipe)
        {
            InitializeComponent();
            _currentRecipe = recipe ?? new Recipes();
            DataContext = _currentRecipe;

            cmbCategory.ItemsSource = AppConnect.model01.Categories.ToList();
            cmbAuthor.ItemsSource = AppConnect.model01.Authors.ToList();
        }

        private void BtnLoadImage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = new OpenFileDialog();
                //dialog.InitialDirectory = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\Images"));

                dialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif|All Files|*.*";
                dialog.Title = "Выберите изображение";

                if (dialog.ShowDialog() == true)
                {
                    string photoName = System.IO.Path.GetFileName(dialog.FileName);
                    _currentRecipe.Image= photoName;
                    MessageBox.Show("Изображение загружено: " + photoName, "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    imgPreview.Source = new BitmapImage(new Uri(dialog.FileName));
                }
                else
                {
                    MessageBox.Show("Изображение не выбрано.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке изображения: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_currentRecipe.RecipeID == 0)
                    AppConnect.model01.Recipes.Add(_currentRecipe);

                AppConnect.model01.SaveChanges();
                MessageBox.Show("Данные сохранены!");
                NavigationService.Navigate(new PageRecipes());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
    }
}
