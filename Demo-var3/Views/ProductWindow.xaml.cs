using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Data.Entity;
using Demo_var3.DTOs;

namespace Demo_var3.Views
{
    /// <summary>
    /// Логика взаимодействия для ProductWindow.xaml
    /// </summary>
    public partial class ProductWindow : Window
    {
        public ProductWindow()
        {
            InitializeComponent();
            LoadProducts();
            FillComboBox();

            FullNameLabel.Content = Session.FullName;
            ToolsPanel.Visibility = Session.isStaff ? Visibility.Visible : Visibility.Collapsed;
        }

        private void FillComboBox()
        {
            using (var db = new BuildMaterialsEntities())
            {
                var query = db.Manufacturer
                .Select(t => t.Name) .ToList();
                
                foreach (var item in query)
                    FiltrationCb.Items.Add(item);
            }
        }

        private void LoadProducts()
        {
            using(var db = new BuildMaterialsEntities())
            {

                string search = SearchTb.Text.ToLower();

                var query = string.IsNullOrEmpty(search) ?
                    db.Product :
                    db.Product.Where(t =>
                        t.Article.ToLower().Contains(search)||
                        t.Name.ToLower().Contains(search) ||
                        t.Description.ToLower().Contains(search) ||
                        t.ProductLinks.Category.Name.ToLower().Contains(search) ||
                        t.ProductLinks.Manufacturer.Name.ToLower().Contains(search)
                    );

                switch (SortCb.SelectedIndex)
                {
                    case 0:
                        query = query.OrderBy(t => t.Name);
                        break;
                    case 1:
                        query = query.OrderBy(t => t.Cost);
                        break;
                    case 2:
                        query = query.OrderBy(t => t.Quantity);
                        break;
                    case 3:
                        query = query.OrderBy(t => t.Sale);
                        break;
                    case 4:
                        query = query.OrderByDescending(t => t.Cost);
                        break;
                    case 5:
                        query = query.OrderByDescending(t => t.Quantity);
                        break;
                    case 6:
                        query = query.OrderByDescending(t => t.Sale);
                        break;
                    default:
                        break;
                }

                if (FiltrationCb.SelectedIndex != 0)
                    query = query.Where(t => t.ProductLinks.Manufacturer.Name.ToLower().Contains((string)FiltrationCb.SelectedValue));

                var view = query
                .Select(t => new ProductDTO
                {
                    Article = t.Article ?? "Отсутствует",
                    Name = t.Name ?? "Отсутствует",
                    Category = t.ProductLinks.Category.Name ?? "Отсутствует",
                    Description = t.Description ?? "Отсутствует",
                    Manufacturer = t.ProductLinks.Manufacturer.Name ?? "Отсутствует",
                    Cost = (int)t.Cost,
                    Sale = (int)t.Sale,
                    Quantity = (int)t.Quantity,
                    Photo = t.Photo ?? "Отсутствует",
                })
                .ToList();

                CardsContainer.ItemsSource = view;
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            new Login().Show();
            this.Close();
        }

        private void SearchTb_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            LoadProducts();
        }

        private void SortCb_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if(CardsContainer != null)
                LoadProducts();
        }

        private void FiltrationCb_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (CardsContainer != null)
                LoadProducts();
        }

        private void CardsContainer_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            using (var db = new BuildMaterialsEntities())
            {
              
                if (Session.isAdmin &&
                    CardsContainer.SelectedItem is ProductDTO p)
                    if (new EditWindow(p.Article).ShowDialog() == true ) LoadProducts();
            }
                
        }
    }
}
