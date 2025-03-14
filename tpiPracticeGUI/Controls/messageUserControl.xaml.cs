using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
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
using tpiPracticeGUI.Frames;
using tpiPracticeGUI.Windows;

namespace tpiPracticeGUI.Controls
{
    /// <summary>
    /// Interaction logic for messageUserControl.xaml
    /// </summary>

    public partial class messageUserControl : UserControl
    {
        private Message? message;

        public messageUserControl()
        {
            InitializeComponent();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            messageContentTextBlock.DataContext = this;

            message = (this.DataContext as Message)!;

            messageContentTextBlock.Text = message!.content;
            
            HttpClient client = new HttpClient();
            var response = await client.GetAsync($"{Globals.apiURL}/User");

            if (response.StatusCode == HttpStatusCode.OK)
            {

                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                User[] users = JsonSerializer.Deserialize<User[]>(responseBody)!;

                User? correspondingUser = users.ToList().FirstOrDefault(x => x.userId == message.userId);

                if (correspondingUser != null)
                {
                    usernameLabel.Content = correspondingUser.name;
                }

                bool isEditable = message.userId == Globals.currentLoggedUser!.userId;

                editionButton.Visibility = isEditable ? Visibility.Visible : Visibility.Collapsed;
                removeButton.Visibility = isEditable ? Visibility.Visible : Visibility.Collapsed;
            }
            else
            {
                MessageBox.Show("La liste des utilisateur n'a pas pu etre recupérée", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void editionButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            messageEditionWindow messageEditionWindow = new messageEditionWindow(message);
            messageEditionWindow.ShowDialog();
            messageContentTextBlock.Text = (this.DataContext as Message).content;
        }

        private async void removeButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            HttpClient client = new HttpClient();
            var result = await client.DeleteAsync($"{Globals.apiURL}/Message/{message!.messageId}");

            if (result.StatusCode != System.Net.HttpStatusCode.NoContent)
            {
                MessageBox.Show("Le groupchat n'a pas pu être supprimé", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Globals.currentMessageList.Remove(message);
            message = null;
        }
    }
}
