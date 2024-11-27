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
    /// Логика взаимодействия для UserPage.xaml
    /// </summary>
    public partial class UserPage : Page
    {
        public UserPage()
        {
            InitializeComponent();

            ComboBoxSort.SelectedIndex = 0;
            CheckBoxUser.IsChecked = false;

            var currentUsers = Entities.GetContext().User.ToList();
            ListUser.ItemsSource = currentUsers;

            UpdateUsers();
        }

        private void TextBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateUsers();
        }

        private void ComboBoxSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateUsers();
        }

        private void CheckBoxUser_Checked(object sender, RoutedEventArgs e)
        {
            UpdateUsers();
        }

        private void CheckBoxUser_Unchecked(object sender, RoutedEventArgs e)
        {
            UpdateUsers();
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            CheckBoxUser.IsChecked = false;
            TextBoxSearch.Clear();
            ComboBoxSort.SelectedIndex = 0;
        }

        private void UpdateUsers()
        {
            var currentUsers = Entities.GetContext().User.ToList();

            //поиск ФИО (без учета регистра)
            currentUsers = currentUsers.Where(x => x.FIO.ToLower().Contains(TextBoxSearch.Text.ToLower())).ToList();

            //фильтр по роли
            if (CheckBoxUser.IsChecked.Value)
                currentUsers = currentUsers.Where(x => x.Role.Contains("Пользователь")).ToList();

            //сортировка
            if (ComboBoxSort.SelectedIndex == 0)
                ListUser.ItemsSource = currentUsers.OrderBy(x => x.FIO).ToList();
            else
                ListUser.ItemsSource = currentUsers.OrderByDescending(x => x.FIO).ToList();
        }
    }
}
