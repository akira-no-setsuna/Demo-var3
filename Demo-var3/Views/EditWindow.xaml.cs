using Microsoft.IdentityModel.Tokens;
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
using System.Windows.Shapes;

namespace Demo_var3.Views
{
    /// <summary>
    /// Логика взаимодействия для EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        private BuildMaterialsEntities _db = new BuildMaterialsEntities();
        private Product _product;
        private bool _isNew;
        
        public EditWindow(string article)
        {
            InitializeComponent();
            FillComboBox();

            _isNew = article.IsNullOrEmpty();

            if (!_isNew)
                LoadProduct(article);
        }

        private void FillComboBox()
        {
            using (var db = new BuildMaterialsEntities())
            {
                CategoryCb.ItemsSource = db.Category.ToList();
                ManufacturerCb.ItemsSource = db.Manufacturer.ToList();
            }
        }

        private void LoadProduct(string article)
        {
            try
            {
                _product = _db.Product.FirstOrDefault(x => x.Article == article);

                ArticleTb.Text = _product.Article;
                NameTb.Text = _product.Name;
                CategoryCb.SelectedIndex = (int)_product.ProductLinks.CategoryId;
                DescriptionTb.Text = _product.Description;
                ManufacturerCb.SelectedIndex = (int)_product.ProductLinks.ManufacturerId;
                CostTb.Text = _product.Cost.ToString();
                SaleTb.Text = _product.Sale.ToString();
                QuantityTb.Text = _product.Quantity.ToString();
                ProductImg.Source = _product.Photo.Get();

                ArticleTb.Background = Brushes.Gray;
                ArticleTb.IsReadOnly = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка соеденения с БД!", "Ошибка");
                return;
            }

            
        }

        private void SelectImgButton_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new Microsoft.Win32.OpenFileDialog() { Filter = "Изображения|*.jpg;*.jpeg;*.png" };
            dlg.ShowDialog();
            if(dlg.FileName != null)
                ProductImg.Source = new BitmapImage(new Uri(dlg.FileName));
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (
                ArticleTb.Text.IsNullOrEmpty() ||
                NameTb.Text.IsNullOrEmpty() ||
                DescriptionTb.Text.IsNullOrEmpty() ||
                CostTb.Text.IsNullOrEmpty() ||
                SaleTb.Text.IsNullOrEmpty() ||
                QuantityTb.Text.IsNullOrEmpty()
            )
            {
                MessageBox.Show("Все поля должны быть заполнены!", "Ошибка");
                return;
            }

            if (!ArticleTb.Text.ToLower().All(c => (c >= 'a' && c <= 'z') || (c >= '0' && c <= '9')))
            {
                MessageBox.Show("Артикул должен состоять из латиницы и/или цифр!", "Ошибка");
                return;
            }


            if (!int.TryParse(CostTb.Text, out int cost) || cost < 0)
            {
                MessageBox.Show("Цена должна быть натуральным числом!", "Ошибка");
                return;
            }

            if (!int.TryParse(SaleTb.Text, out int sale) || sale < 0)
            {
                MessageBox.Show("Скидка должна быть натуральным числом!", "Ошибка");
                return;
            }


            if (sale > 100)
            {
                MessageBox.Show("Скидка должна быть не более 100%!", "Ошибка");
                return;
            }

            if (!int.TryParse(QuantityTb.Text, out int quantity) || quantity < 0)
            {
                MessageBox.Show("Кол-во должно быть натуральным числом!", "Ошибка");
                return;
            }

            if (!(NameTb.Text.Length <= 500))
            {
                MessageBox.Show("Наименование должно быть длинной  не более 500 символов!", "Ошибка");
                return;
            }

            if (!(DescriptionTb.Text.Length <= 500))
            {
                MessageBox.Show("Описание должно быть длинной  не более 500 символов!", "Ошибка");
                return;
            }

            if (!(CostTb.Text.Length <= 100))
            {
                MessageBox.Show("Цена должна быть длинной  не более 100 символов!", "Ошибка");
                return;
            }

            try
            {
                if (_isNew) _db.Product.Add(_product);

                _product.Article = ArticleTb.Text;
                _product.Name = NameTb.Text;
                _product.ProductLinks.CategoryId = CategoryCb.SelectedIndex;
                _product.Description = DescriptionTb.Text;
                _product.ProductLinks.ManufacturerId = ManufacturerCb.SelectedIndex;
                _product.Cost = cost;
                _product.Sale = sale;
                _product.Quantity = quantity;

                string oldPhoto = _product.Photo;

                _product.Photo = ProductImg.Source.Save();
                oldPhoto.Delete();

                _db.SaveChanges();

                DialogResult = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка соеденения с БД!", "Ошибка");
                return;
            }
        }
    }
}
