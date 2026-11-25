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

            FullNameLabel.Content = Session.FullName;
        }

        private void LoadProducts()
        {
            using(var db = new BuildMaterialsEntities())
            {
                var query = db.Product
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

                CardsContainer.ItemsSource = query;
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            new Login().Show();
            this.Close();
        }
    }
}
