using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
using System.Security.Cryptography;
using tpiPracticeClasses;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Net;

namespace tpiPracticeGUI.Frames
{
    /// <summary>
    /// Interaction logic for SignupPhaseUsercontrol.xaml
    /// </summary>
    public partial class SignupPhaseUsercontrol : UserControl
    {
        public SignupPhaseUsercontrol()
        {
            InitializeComponent();
        }

        private void Label_MouseEnter(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        private void Label_MouseLeave(object sender, MouseEventArgs e)
        {
            this.Cursor = default;
        }

        private void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Globals.phase = Globals.Phase.login;
        }

        private async void connectionButton_Click(object sender, RoutedEventArgs e)
        {
            if (
                string.IsNullOrEmpty(usernameTextbox.Text) ||
                string.IsNullOrEmpty(emailTextbox.Text) ||
                string.IsNullOrEmpty(passwordTextbox.Password) ||
                string.IsNullOrEmpty(passwordConfirmationTextbox.Password)
            )
            {
                MessageBox.Show("Il faut que le nom d'utilisateur, adresse email ainsi que le mot de passe soient entrées", "Erreur de création de compte", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (passwordConfirmationTextbox.Password != passwordTextbox.Password)
                {
                    MessageBox.Show("Il faut que le mot de passe de confirmation soit identique à celui du mot de passe", "Erreur de création de compte", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                User user = new User();
                user.email = emailTextbox.Text;
                user.name = usernameTextbox.Text;

                using (SHA256 sha256 = SHA256.Create())
                {
                    Encoding encoding = Encoding.ASCII;

                    byte[] cryptedPasswordResult = sha256.ComputeHash(encoding.GetBytes(passwordTextbox.Password));

                    user.password = "";

                    foreach (byte b in cryptedPasswordResult)
                    {
                        user.password += b;
                    }
                }

                var postData = new
                {
                    userId = user.userId,
                    password = user.password,
                    email = user.email,
                    name = user.name
                };

                string jsonString = JsonSerializer.Serialize(postData);
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                HttpClient client = new HttpClient();
                var response = await client.PostAsync($"{Globals.apiURL}/User", content);

                if (response.StatusCode == HttpStatusCode.Created)
                {
                    Globals.phase = Globals.Phase.login;
                }
                else
                {
                    MessageBox.Show($"{response.StatusCode}: {response.Content}");
                }

            }
        }

        private void usernameTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(usernameTextbox.Text))
            {
                usernamePlaceholderLabel.Visibility = Visibility.Visible;
            }
            else
            {
                usernamePlaceholderLabel.Visibility = Visibility.Hidden;
            }
        }

        private void emailTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(emailTextbox.Text))
            {
                emailPlaceholderLabel.Visibility = Visibility.Visible;
            }
            else
            {
                emailPlaceholderLabel.Visibility = Visibility.Hidden;
            }
        }

        private void passwordTextbox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(passwordTextbox.Password))
            {
                passwordPlaceholderLabel.Visibility = Visibility.Visible;
            }
            else
            {
                passwordPlaceholderLabel.Visibility = Visibility.Hidden;
            }
        }

        private void passwordConfirmationTextbox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(passwordConfirmationTextbox.Password))
            {
                passwordConfirmationPlaceholderLabel.Visibility = Visibility.Visible;
            }
            else
            {
                passwordConfirmationPlaceholderLabel.Visibility = Visibility.Hidden;
            }
        }
    }
}
