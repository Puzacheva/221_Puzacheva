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

namespace _221_Puzacheva.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddUserPage.xaml
    /// </summary>
    public partial class AddUserPage : Page
    {
        private User _currentUser = new User();

        public AddUserPage(User selectedUser)
        {
            InitializeComponent();

            if (selectedUser != null)
                _currentUser = selectedUser;

            DataContext = _currentUser;
        }

        private void TextBoxLogin_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtHintLogin.Visibility = string.IsNullOrEmpty(TextBoxLogin.Text) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void TextBoxPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtHintPassword.Visibility = string.IsNullOrEmpty(TextBoxPassword.Text) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void TextBoxFIO_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtHintFIO.Visibility = string.IsNullOrEmpty(TextBoxFIO.Text) ? Visibility.Visible : Visibility.Collapsed;

        }

        private void TextBoxPhoto_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtHintPhoto.Visibility = string.IsNullOrEmpty(TextBoxPhoto.Text) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_currentUser.Login))
                errors.AppendLine("Укажите логин!");
            if (string.IsNullOrWhiteSpace(_currentUser.Password))
                errors.AppendLine("Укажите пароль!");
            if ((_currentUser.Role == null) || (ComboBoxRole.Text == ""))
                errors.AppendLine("Выберите роль!");
            else
                _currentUser.Role = ComboBoxRole.Text;
            if (string.IsNullOrWhiteSpace(_currentUser.FIO))
                errors.AppendLine("Укажите ФИО!");
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (_currentUser.ID == 0)
                Entities.GetContext().User.Add(_currentUser);

            try
            {
                Entities.GetContext().SaveChanges();
                MessageBox.Show("Данные успешно сохранены!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
            AddGrid.Children.OfType<UIElement>().ToList().ForEach(control =>
            {
                if (control is TextBox textBox) textBox.Text = string.Empty;
                else if (control is ComboBox comboBox) comboBox.SelectedIndex = -1;
            });
        }
    }
}
