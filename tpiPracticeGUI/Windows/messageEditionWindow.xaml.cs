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
    /// Interaction logic for messageEditionWindow.xaml
    /// </summary>
    public partial class messageEditionWindow : Window
    {

        Message message;

        public messageEditionWindow(Message message)
        {
            this.message = message;
            InitializeComponent();

            newContentTextbox.Text = message.content;
        }

        private void newContentTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            submissionButton.IsEnabled = (newContentTextbox.Text != message.content) && !string.IsNullOrEmpty(newContentTextbox.Text);
        }

        private async void submissionButton_Click(object sender, RoutedEventArgs e)
        {
            Message newMessage = message;
            message.content = newContentTextbox.Text;

            var putData = new
            {
                messageId = message.messageId,
                userId = message.userId,
                groupchatId = message.groupchatId,
                content = message.content
            };

            string jsonString = JsonSerializer.Serialize(putData);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            HttpClient httpClient = new HttpClient();
            var result = await httpClient.PutAsync($"{Globals.apiURL}/Message/{message.messageId}", content);

            if (!(result.StatusCode == HttpStatusCode.Created || result.StatusCode == HttpStatusCode.NoContent))
            {
                MessageBox.Show("La modification du message n'a pas pu être fait", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            for (int i = 0; i < Globals.currentMessageList.Count; i++)
            {
                if (Globals.currentMessageList[i].messageId == message.messageId)
                {
                    Globals.currentMessageList[i].content = message.content;
                    break;
                }
            }

            this.Close();
        }
    }
}
