using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
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
using tpiPracticeClasses;

namespace tpiPracticeGUI.Frames
{
    /// <summary>
    /// Interaction logic for LoginPhaseUsercontrol.xaml
    /// </summary>
    public partial class LoginPhaseUsercontrol : UserControl
    {
        public LoginPhaseUsercontrol()
        {
            

            InitializeComponent();
        }

        private async void submitButton_Click(object sender, RoutedEventArgs e)
        {
            string username = usernameTextbox.Text;
            string password = passwordTextbox.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Il faut qu'un nom d'utilisateur et mot de passe soient entrées", "Erreur de connection", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {

                User user = new User();
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

                HttpClient client = new HttpClient();

                var response = await client.GetAsync($"{Globals.apiURL}/User");

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    response.EnsureSuccessStatusCode();
                    
                    string responseBody = await response.Content.ReadAsStringAsync();

                    User[] users = JsonSerializer.Deserialize<User[]>(responseBody)!;

                    foreach (User currentUser in users)
                    {
                        if (
                            currentUser.name == user.name &&
                            currentUser.password == user.password
                        )
                        {
                            Globals.currentLoggedUser = currentUser;
                            Globals.phase = Globals.Phase.groupchatListing;
                            return;
                        }
                    }
                    MessageBox.Show("Vous n'êtes pas connecté!");
                }
                
            }
        }

        private void usernameTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(usernameTextbox.Text))
            {
                usernamePlaceholderLabel.Visibility = Visibility.Hidden;
            }
            else
            {
                usernamePlaceholderLabel.Visibility = Visibility.Visible;
            }
        }

        private void passwordTextbox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(passwordTextbox.Password))
            {
                passwordPlaceholderLabel.Visibility = Visibility.Hidden;
            }
            else
            {
                passwordPlaceholderLabel.Visibility = Visibility.Visible;
            }
        }

        private void Label_MouseEnter(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void Label_MouseLeave(object sender, MouseEventArgs e)
        {
            Cursor = default;
        }

        private void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Globals.phase = Globals.Phase.signup;
        }

    }
}
