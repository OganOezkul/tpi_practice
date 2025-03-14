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
using System.Windows.Shapes;
using tpiPracticeClasses;
using tpiPracticeGUI.Frames;

namespace tpiPracticeGUI.Windows
{
    /// <summary>
    /// Interaction logic for groupchatNameEditionWindow.xaml
    /// </summary>
    public partial class groupchatNameEditionWindow : Window
    {

        Groupchat currentlyEditingGroupchat;

        public groupchatNameEditionWindow(Groupchat groupchat)
        {
            currentlyEditingGroupchat = groupchat;
            InitializeComponent();
        }

        private void groupchatNameTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string newName = groupchatNameTextbox.Text;
            submissionButton.IsEnabled = (!string.IsNullOrEmpty(newName) && newName != currentlyEditingGroupchat.Name);
        }

        private async void submissionButton_Click(object sender, RoutedEventArgs e)
        {
            Groupchat newGroupchat = currentlyEditingGroupchat;
            newGroupchat.Name = groupchatNameTextbox.Text;

            var putData = new
            {
                grouchatId = newGroupchat.GroupchatId,
                userId = newGroupchat.UserId,
                name = newGroupchat.Name
            };

            string jsonString = JsonSerializer.Serialize(putData);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            HttpClient httpClient = new HttpClient();
            var result = await httpClient.PutAsync($"{Globals.apiURL}/Groupchat/{newGroupchat.GroupchatId}", content);

            if (!(result.StatusCode == HttpStatusCode.Created || result.StatusCode == HttpStatusCode.NoContent))
            {
                MessageBox.Show("La modification du nom du groupe chat n'a pas pu être fait", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            Globals.editGroupchatById(newGroupchat.GroupchatId, newGroupchat);
            
            this.Close();
        }
    }
}
