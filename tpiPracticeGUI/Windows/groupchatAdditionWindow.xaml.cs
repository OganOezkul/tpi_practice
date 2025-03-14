using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
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
    /// Interaction logic for groupchatAdditionWindow.xaml
    /// </summary>
    public partial class groupchatAdditionWindow : Window
    {
        public groupchatAdditionWindow()
        {
            InitializeComponent();
        }

        private async void submissionButton_Click(object sender, RoutedEventArgs e)
        {
            Groupchat newGroupchat = new Groupchat();
            newGroupchat.Name = groupchatNameTextbox.Text;
            newGroupchat.UserId = Globals.currentLoggedUser!.userId;

            var postData = new
            {
                userId = newGroupchat.UserId,
                name = newGroupchat.Name
            };

            string jsonString = JsonSerializer.Serialize(postData);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            HttpClient httpClient = new HttpClient();
            var result = await httpClient.PostAsync($"{Globals.apiURL}/Groupchat", content);

            if (result.StatusCode == HttpStatusCode.Created)
            {
                result.EnsureSuccessStatusCode();
                string resultBody = await result.Content.ReadAsStringAsync();

                Groupchat addedGroupchat = JsonSerializer.Deserialize<Groupchat>(resultBody)!;

                Globals.addGroupchat(addedGroupchat);

                this.Close();
            }
        }

        private void groupchatNameTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string newName = groupchatNameTextbox.Text;
            submissionButton.IsEnabled = !string.IsNullOrEmpty(newName);
        }
    }
}
