using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Net;
using tpiPracticeClasses;
using System.Collections.ObjectModel;
using System.Windows.Interop;
using System.Reflection.PortableExecutable;
using System.Security.Policy;
using System.Diagnostics;

namespace tpiPracticeGUI.Frames
{
    /// <summary>
    /// Interaction logic for chatRoomUSercontrol.xaml
    /// </summary>
    public partial class chatRoomUSercontrol : UserControl
    {

        Task pollingCode;

        public chatRoomUSercontrol()
        {
            InitializeComponent();
            Globals.OnGroupchatJoin += Globals_OnGroupchatJoin;

            pollingCode = new Task(async () => {
                while (true)
                {
                    HttpClient httpClient = new HttpClient();
                    var response = await httpClient.GetAsync($"{Globals.apiURL}/Message");


                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        response.EnsureSuccessStatusCode();
                        string responseBody = await response.Content.ReadAsStringAsync();

                        Message[] messageArray = JsonSerializer.Deserialize<Message[]>(responseBody)!;

                        List<Message> messageList = messageArray.ToList();

                        if (!Globals.isSameAsCachedMessagetList(messageList))
                        {
                            App.Current?.Dispatcher.Invoke((Action)delegate {
                                Globals.currentMessageList.Clear();
                            });

                            foreach (Message msg in messageList)
                            {
                                App.Current?.Dispatcher.Invoke((Action)delegate {
                                    Globals.currentMessageList.Add(msg);
                                    chatListView.ItemsSource = Globals.currentMessageList;
                                });
                            }
                        }

                    }
                    await Task.Delay(500);
                }
            });
        }

        private void Globals_OnGroupchatJoin(object? sender, EventArgs e)
        {
            Globals.currentMessageList.Clear();

            groupChatNameLabel.Content = (Globals.currentJoinedGroupchat?.Name ?? "none");

            chatListView.ItemsSource = Globals.currentMessageList;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void messageTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            sendButton.IsEnabled = !string.IsNullOrEmpty(messageTextBox.Text);
        }

        private async void sendButton_Click(object sender, RoutedEventArgs e)
        {
            Message message = new Message();
            message.content = messageTextBox.Text;
            message.userId = Globals.currentLoggedUser!.userId;
            message.groupchatId = Globals.currentJoinedGroupchat!.GroupchatId;

            var postData = new
            {
                messageId = message.messageId,
                userId = message.userId,
                groupchatId = message.groupchatId,
                content = message.content
            };

            string jsonString = JsonSerializer.Serialize(postData);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            HttpClient client = new HttpClient();
            var response = await client.PostAsync($"{Globals.apiURL}/Message", content);

            if (response.StatusCode == HttpStatusCode.Created)
            {
                Globals.currentMessageList.Add(message);
            }
            else
            {
                MessageBox.Show($"{response.StatusCode}: {response.Content}");
            }
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            Globals.currentJoinedGroupchat = null;
            Globals.currentMessageList.Clear();
            Globals.phase = Globals.Phase.groupchatListing;
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.IsVisible == true)
            {
                if (pollingCode.IsCompleted == false)
                {
                    pollingCode.Start();
                }
            }
            else
            {

            }
        }
    }
}
