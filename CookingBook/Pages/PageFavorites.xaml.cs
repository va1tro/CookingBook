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
    /// Логика взаимодействия для PageFavorites.xaml
    /// </summary>
    public partial class PageFavorites : Page
    {
        private readonly Entities context = new Entities();
        private readonly int currentAuthorId = AppConnect.CurrentUser.AuthorID;

        public PageFavorites()
        {
            InitializeComponent();
            LoadFavorites();
        }

        private void LoadFavorites()
        {
            var favorites = context.Favorites
                .Where(f => f.AuthorID == currentAuthorId)
                .OrderByDescending(f => f.DateAdded)
                .ToList();

            listFavorites.ItemsSource = favorites;

            tbEmpty.Visibility = favorites.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        private void BtnRemoveFavorite_Click(object sender, RoutedEventArgs e)
        {
            var favorite = (sender as Button)?.Tag as Favorites;
            if (favorite != null)
            {
                var confirm = MessageBox.Show($"Удалить \"{favorite.Recipes.RecipeName}\" из избранного?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (confirm == MessageBoxResult.Yes)
                {
                    context.Favorites.Remove(favorite);
                    context.SaveChanges();
                    LoadFavorites();
                }
            }
        }
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }
    }
}