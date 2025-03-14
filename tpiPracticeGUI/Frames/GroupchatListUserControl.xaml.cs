using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using tpiPracticeClasses;
using tpiPracticeGUI.Controls;
using tpiPracticeGUI.Windows;

namespace tpiPracticeGUI.Frames
{
    /// <summary>
    /// Interaction logic for GroupchatListUserControl.xaml
    /// </summary>
    public partial class GroupchatListUserControl : UserControl
    {

        Task pollingCode;

        bool isPollingRuning = false;

        CancellationTokenSource tokenSource2 = new CancellationTokenSource();
        CancellationToken ct;

        public GroupchatListUserControl()
        {
            ct = tokenSource2.Token;

            InitializeComponent();

            groupchatListBox.ItemsSource = Globals.getGroupChatList;
            Globals.OnGroupchatRemove += onGroupchasListChanged;
            Globals.OnGroupchatEdit += onGroupchasListChanged;
            Globals.OnGroupchatAdd += onGroupchasListChanged;

            groupchatListBox.ItemsSource = Globals.getGroupChatList;
            pollingCode = new Task(async () => {

                ct.ThrowIfCancellationRequested();

                if (ct.IsCancellationRequested)
                {
                    // Clean up here, then...
                    ct.ThrowIfCancellationRequested();
                }

                while (isPollingRuning == true)
                {
                    HttpClient httpClient = new HttpClient();

                    var response = await httpClient.GetAsync($"{Globals.apiURL}/Groupchat");

                    if (response.StatusCode == HttpStatusCode.OK)
                    {

                        response.EnsureSuccessStatusCode();
                        string responseBody = await response.Content.ReadAsStringAsync();


                        Groupchat[] groupchats = JsonSerializer.Deserialize<Groupchat[]>(responseBody)!;

                        List<Groupchat> tempGroupchat = groupchats.ToList();

                        if (Globals.isSameAsCachedGroupchatList(tempGroupchat) == false)
                        {
                            App.Current?.Dispatcher.Invoke((Action)delegate {
                                Globals.getGroupChatList.Clear();
                            });

                            foreach (Groupchat groupchat in groupchats)
                            {
                                Globals.addGroupchat(groupchat);
                            }

                            App.Current?.Dispatcher.Invoke((Action)delegate {
                                groupchatListBox.ItemsSource = Globals.getGroupChatList;
                            });
                        }
                    }
                    await Task.Delay(200);
                    
                }
            }, tokenSource2.Token);
            
        }

        private void onGroupchasListChanged(object? sender, EventArgs e)
        {
        }

        private async void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

            if (this.IsVisible == true)
            {

                header.setHeaderText(Globals.currentLoggedUser!.name);

                if (isPollingRuning == false)
                {
                    isPollingRuning = true;
                    pollingCode.Start();
                }

            }
            else
            {
            }
        }

        private void groupchatAdditionButton_Click(object sender, RoutedEventArgs e)
        {
            groupchatAdditionWindow groupchatAdditionWindow = new groupchatAdditionWindow();
            groupchatAdditionWindow.ShowDialog();
        }

        private void header_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void groupchatListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
    }
}
