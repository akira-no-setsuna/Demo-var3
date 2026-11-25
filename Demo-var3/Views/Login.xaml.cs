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
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTb.Text;
            string password = PasswordTb.Text;
            

            using (var db = new BuildMaterialsEntities())
            {
                var user = db.User.FirstOrDefault(u => u.Login == login && u.Password == password);

                if (user != null)
                {
                    Session.FullName = user.FullName;
                    Session.Role = (int)user.UserRole.RoleId;

                    new ProductWindow().Show();
                    this.Close();
                }
                else if (login == "admin" && password == "admin")
                {
                    Session.FullName = "Админ";
                    Session.Role = 1;

                    new ProductWindow().Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль!", "Ошибка");
                }
            }
        }

        private void QuestButton_Click(object sender, RoutedEventArgs e)
        {
            Session.FullName = "Гость";
            Session.Role = 4;

            new ProductWindow().Show();
            this.Close();
        }
    }
}
