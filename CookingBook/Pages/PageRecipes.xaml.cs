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
    }
}