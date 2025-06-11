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
    /// Логика взаимодействия для PageRecipes.xaml
    /// </summary>
    public partial class PageRecipes : Page
    {
        public PageRecipes()
        {
            InitializeComponent();
            LoadRecipes();
            LoadFiltersAndSorting();
        }

        private void LoadRecipes()
        {
            var recipes = AppConnect.model01.Recipes.ToList();
            listProduct.ItemsSource = recipes;
            UpdateCounter(recipes.Count);
        }

        private void LoadFiltersAndSorting()
        {
            // Загрузка категорий для фильтра
            var categories = new List<string> { "Все" };
            categories.AddRange(AppConnect.model01.Categories.Select(c => c.CategoryName));
            ComboFilter.ItemsSource = categories;

            // Загрузка вариантов сортировки
            ComboSort.ItemsSource = new List<string>
            {
                "По умолчанию",
                "По времени (убывание)",
                "По времени (возрастание)"
            };
        }

        private void UpdateCounter(int count)
        {
            tbCounter.Text = count > 0 ? $"Найдено {count} рецептов" : "Ничего не найдено";
        }

        private List<Recipes> FindRecipes()
        {
            var recipes = AppConnect.model01.Recipes.ToList();

            // Фильтрация по поиску
            if (!string.IsNullOrWhiteSpace(TextSearch.Text))
            {
                recipes = recipes.Where(r => r.RecipeName.ToLower()
                                             .Contains(TextSearch.Text.ToLower()))
                                             .ToList();
            }

            // Фильтрация по категории
            if (ComboFilter.SelectedIndex > 0)
            {
                recipes = recipes.Where(r => r.Categories.CategoryName ==
                                           ComboFilter.SelectedItem.ToString())
                                           .ToList();
            }

            // Сортировка
            switch (ComboSort.SelectedIndex)
            {
                case 1:
                    recipes = recipes.OrderByDescending(r => r.CookingTime).ToList();
                    break;
                case 2:
                    recipes = recipes.OrderBy(r => r.CookingTime).ToList();
                    break;
            }

            UpdateCounter(recipes.Count);
            return recipes;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditPages(new Recipes()));
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (listProduct.SelectedItem is Recipes selectedRecipe)
            {
                NavigationService.Navigate(new AddEditPages(selectedRecipe));
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите рецепт для редактирования",
                              "Внимание",
                              MessageBoxButton.OK,
                              MessageBoxImage.Warning);
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (listProduct.SelectedItem is Recipes selectedRecipe)
            {
                var result = MessageBox.Show($"Удалить рецепт '{selectedRecipe.RecipeName}'?",
                                           "Подтверждение удаления",
                                           MessageBoxButton.YesNo,
                                           MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        AppConnect.model01.Recipes.Remove(selectedRecipe);
                        AppConnect.model01.SaveChanges();
                        LoadRecipes();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении: {ex.Message}",
                                      "Ошибка",
                                      MessageBoxButton.OK,
                                      MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите рецепт для удаления",
                                "Внимание",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
            }
        }

        private void ComboFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            listProduct.ItemsSource = FindRecipes();
        }

        private void TextSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            listProduct.ItemsSource = FindRecipes();
        }

        private void ComboSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            listProduct.ItemsSource = FindRecipes();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Можно добавить дополнительную логику при выборе элемента
        }
        private void BtnAddToCart_Click(object sender, RoutedEventArgs e)
        {
            var recipe = (sender as Button)?.Tag as Recipes;
            if (recipe != null)
            {
                var newCart = new Cart
                {
                    RecipeID = recipe.RecipeID,
                    AuthorID = AppConnect.CurrentUser.AuthorID,
                    Quantity = 1,
                    DateAdded = DateTime.Now
                };

                using (var context = new Entities())
                {
                    context.Cart.Add(newCart);
                    context.SaveChanges();
                }

                MessageBox.Show($"\"{recipe.RecipeName}\" добавлен в корзину!");
            }
        }

        private void BtnAddToFavorites_Click(object sender, RoutedEventArgs e)
        {
            var recipe = (sender as Button)?.Tag as Recipes;
            if (recipe != null)
            {
                using (var context = new Entities())
                {
                    int currentAuthorId = AppConnect.CurrentUser.AuthorID; // предполагается, что ты где-то хранишь текущего пользователя

                    // Проверка — уже добавлен в избранное?
                    bool alreadyExists = context.Favorites.Any(f => f.AuthorID == currentAuthorId && f.RecipeID == recipe.RecipeID);
                    if (alreadyExists)
                    {
                        MessageBox.Show("Этот рецепт уже в избранном.", "Избранное", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }

                    var favorite = new Favorites
                    {
                        AuthorID = currentAuthorId,
                        RecipeID = recipe.RecipeID,
                        DateAdded = DateTime.Now
                    };

                    context.Favorites.Add(favorite);
                    context.SaveChanges();
                }

                MessageBox.Show($"\"{recipe.RecipeName}\" добавлен в избранное!", "Избранное", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void BtnExportCsv_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "CSV файлы (*.csv)|*.csv",
                FileName = "export_recipes.csv"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                var sb = new StringBuilder();
                sb.AppendLine("Название;Описание;Категория;Время приготовления (мин)");

                foreach (var recipe in listProduct.Items.Cast<Recipes>())
                {
                    string name = recipe.RecipeName?.Replace(";", ",") ?? "";
                    string desc = recipe.Description?.Replace(";", ",") ?? "";
                    string category = recipe.Categories?.CategoryName?.Replace(";", ",") ?? "";
                    string time = recipe.CookingTime.ToString() ?? "";

                    sb.AppendLine($"{name};{desc};{category};{time}");
                }

                File.WriteAllText(saveFileDialog.FileName, sb.ToString(), Encoding.UTF8);
                MessageBox.Show("Файл успешно сохранён:\n" + saveFileDialog.FileName, "Экспорт CSV", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BtnCart_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PageCart(AppConnect.CurrentUser));
        }

        private void BtnFavorites_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PageFavorites());
        }

        private void BtnOrders_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PageOrders());
        }

        private void BtnAbout_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PageAbout());
        }

    }
}