using CookingBook.AppData;
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
            List<Recipes> products = AppConnect.model01.Recipes.ToList();
            var sortList = new List<string>{
                "По умолчанию",
                "По убыванию",
                "По возрастанию"
            };
            ComboSort.ItemsSource = sortList;

            var categoriesFilter = new List<string> { "Все" };
            var categories = AppConnect.model01.Categories.ToList();
            foreach (var category in categories)
            {
                categoriesFilter.Add(category.CategoryName);
            }
            ComboFilter.ItemsSource = categoriesFilter;

            if (products.Count > 0)
            {
                tbCounter.Text = "Найдено " + products.Count + "товаров";
            }
            else
            {
                tbCounter.Text = "Не найдено";
            }
            listProduct.ItemsSource = products;
        }

        Recipes[] FindRecipes()
        {
            var recipe = AppData.AppConnect.model01.Recipes.ToList();
            var recipesall = recipe;
            if (TextSearch != null)
            {
                recipe = recipe.Where(x => x.RecipeName.ToLower().Contains(TextSearch.Text.ToLower())).ToList();
            }

            if (ComboFilter.SelectedIndex > 0)
            {
                recipe = recipe.Where(x => x.CategoryID == ComboFilter.SelectedIndex).ToList();
            }

            if (ComboSort.SelectedIndex > 0)
            {
                switch (ComboSort.SelectedIndex)
                {
                    case 1:
                        recipe = recipe.OrderByDescending(x => x.CookingTime).ToList();
                        break;
                    case 2:
                        recipe = recipe.OrderBy(x => x.CookingTime).ToList();

                        break;
                }
            }
            if (recipe.Count() > 0)
            {
                LabelCount.Content = "Найдено " + recipe.Count() + " из " + recipesall.Count;
            }
            else
            {
                LabelCount.Content = "Ничего не найдено ";
            }
            return recipe.ToArray();
        }
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

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
    }
}