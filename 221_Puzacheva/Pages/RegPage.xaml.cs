using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

namespace _221_Puzacheva.Pages
{
    /// <summary>
    /// Логика взаимодействия для RegPage.xaml
    /// </summary>
    public partial class RegPage : Page
    {
        public RegPage()
        {
            InitializeComponent();
        }

        private void TextBoxLogin_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtHintLogin.Visibility = string.IsNullOrEmpty(TextBoxLogin.Text) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void TextBoxPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtHintPassword.Visibility = string.IsNullOrEmpty(TextBoxPassword.Text) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void TextBoxPasswordConfirmed_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtHintPasswordConfirmed.Visibility = string.IsNullOrEmpty(TextBoxPasswordConfirmed.Text) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void TextBoxFIO_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtHintFIO.Visibility = string.IsNullOrEmpty(TextBoxFIO.Text) ? Visibility.Visible : Visibility.Collapsed;

        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
            RegGrid.Children.OfType<UIElement>().ToList().ForEach(control =>
            {
                if (control is TextBox textBox) textBox.Text = string.Empty;
                else if (control is ComboBox comboBox) comboBox.SelectedIndex = -1;
            });
        }

        private void ButtonReg_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            using (var db = new Entities())
            {
                if (string.IsNullOrEmpty(TextBoxLogin.Text) || string.IsNullOrEmpty(TextBoxPassword.Text)
                || string.IsNullOrEmpty(TextBoxPasswordConfirmed.Text) || string.IsNullOrEmpty(TextBoxFIO.Text))
                    errors.AppendLine("Все поля обязательны для заполнения!");

                var user = db.User
                    .AsNoTracking()
                    .FirstOrDefault(u => u.Login == TextBoxLogin.Text);
                if (user != null)
                    errors.AppendLine("Пользователь с таким логином уже существует!");

                if (errors.Length > 0)
                {
                    MessageBox.Show(errors.ToString());
                    return;
                }

                if (TextBoxPassword.Text.Length < 6)
                {
                    MessageBox.Show("Пароль должен содержать 6 или более символов!");
                    return;
                }

                bool en = true;
                bool number = false;

                for (int i = 0; i < TextBoxPassword.Text.Length; i++)
                {
                    if (TextBoxPassword.Text[i] >= 'А' && TextBoxPassword.Text[i] <= 'Я'
                        || TextBoxPassword.Text[i] >= 'а' && TextBoxPassword.Text[i] <= 'я')
                    {
                        en = false;
                    }

                    if (TextBoxPassword.Text[i] >= '0' && TextBoxPassword.Text[i] <= '9')
                    {
                        number = true;
                    }
                }

                if (!en)
                {
                    MessageBox.Show("Используйте только английскую раскладку для пароля!");
                    return;
                }

                if (!number)
                {
                    MessageBox.Show("Пароль должен содержать хотя бы одну цифру!");
                    return;
                }

                if (TextBoxPassword.Text != TextBoxPasswordConfirmed.Text)
                {
                    MessageBox.Show("Пароли не совпадают!");
                    return;
                }

                User userObject = new User
                {
                    FIO = TextBoxFIO.Text,
                    Login = TextBoxLogin.Text,
                    Password = GetHash(TextBoxPassword.Text),
                    Role = ComboBoxRole.Text
                };
                db.User.Add(userObject);
                db.SaveChanges();
            }
            MessageBox.Show("Новый пользователь успешно зарегистрирован!");
        }
        
        public static string GetHash(string password)
        {
            using (var hash = SHA1.Create())
            {
                return string.Concat(hash.ComputeHash(Encoding.UTF8.GetBytes(password)).Select(x
                    => x.ToString("X2")));
            }
        }
    }
}
   
