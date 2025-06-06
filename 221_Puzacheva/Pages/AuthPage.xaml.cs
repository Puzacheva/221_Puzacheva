﻿using System;
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
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        public AuthPage()
        {
            InitializeComponent();
        }

        private void TextBoxLogin_Changed(object sender, RoutedEventArgs e)
        {
            txtHintLogin.Visibility = Visibility.Visible;
            if (TextBoxLogin.Text.Length > 0)
            {
                txtHintLogin.Visibility = Visibility.Hidden;
            }
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            txtHintPassword.Visibility = Visibility.Visible;
            if (PasswordBox.Password.Length > 0)
            {
                txtHintPassword.Visibility = Visibility.Hidden;
            }
        }

        private void ButtonEnter_OnClick(object sender, RoutedEventArgs e)
        {
                if (string.IsNullOrEmpty(TextBoxLogin.Text) || string.IsNullOrEmpty(PasswordBox.Password))
                {
                    MessageBox.Show("Введите логин и пароль!");
                    return;
                }
                
                string hashedPassword = GetHash(PasswordBox.Password);

                using (var db = new Entities())
                {

                    var user = db.User
                        .AsNoTracking()
                        .FirstOrDefault(u => u.Login == TextBoxLogin.Text && u.Password == hashedPassword);

                    if (user == null)
                    {
                        MessageBox.Show("Пользователь с такими данными не найден!");
                        return;
                    }

                    MessageBox.Show("Пользователь успешно найден!");

                    switch (user.Role)
                    {
                        case "Администратор":
                            NavigationService?.Navigate(new AdminPage());
                            break;
                        case "Пользователь":
                            NavigationService?.Navigate(new UserPage());
                            break;
                    }
                }
            }
            
        public static string GetHash(string password)
        {
            using (var hash = SHA1.Create())
            {
                return string.Concat(hash.ComputeHash(Encoding.UTF8.GetBytes(password)).Select(x
                    => x.ToString("X2")));
            }
        }

        private void ButtonReg_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new RegPage());
        }
    }
}
   
