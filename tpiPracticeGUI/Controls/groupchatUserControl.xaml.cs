using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using tpiPracticeClasses;
using tpiPracticeGUI.Frames;
using tpiPracticeGUI.Windows;

namespace tpiPracticeGUI.Controls
{
    /// <summary>
    /// Interaction logic for groupchatUserControl.xaml
    /// </summary>
    public partial class groupchatUserControl : UserControl
    {

        private Groupchat? _groupChat;

        private Thread groupchatRemovalAction;

        public Groupchat? groupChat
        {
            get => _groupChat;
            set
            {
                _groupChat = value;
                nameLabel.Content = value?.Name;

            }
        }

        private bool isEditable;

        public groupchatUserControl()
        {
            InitializeComponent();
        }

        private void groupChatEditionIcons_MouseEnter(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void groupChatEditionIcons_MouseLeave(object sender, MouseEventArgs e)
        {
            Cursor = default;
        }

        private void editionImage_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void removeImage_MouseUp(object sender, MouseButtonEventArgs e)
        {
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try {
                nameLabel.DataContext = this;

                groupChat = (this.DataContext as Groupchat);

                isEditable = groupChat?.UserId == Globals.currentLoggedUser?.userId;

                if (isEditable)
                {
                    editionImage.Visibility = Visibility.Visible;
                    removeImage.Visibility = Visibility.Visible;
                }
            }
            catch { /* to avoid crash */ }

            groupchatRemovalAction = new Thread(async () => {
                HttpClient client = new HttpClient();
                var result = await client.DeleteAsync($"{Globals.apiURL}/Groupchat/{groupChat?.GroupchatId}");

                if (result.StatusCode != System.Net.HttpStatusCode.NoContent)
                {
                    MessageBox.Show("Le groupchat n'a pas pu être supprimé", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (groupChat != null)
                {
                    Globals.removeGroupchat(groupChat);
                }
                _groupChat = null;
            });
        }

        private void editionImage_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            groupchatNameEditionWindow groupchatNameEditionWindow = new groupchatNameEditionWindow(_groupChat ?? new Groupchat());
            groupchatNameEditionWindow.ShowDialog();
        }

        private void removeImage_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            groupchatRemovalAction.Start();
        }

        private void UserControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (groupchatRemovalAction.ThreadState != ThreadState.Running)
            {
                if (_groupChat != null)
                {
                    Globals.currentJoinedGroupchat = groupChat;
                    Globals.phase = Globals.Phase.chatting;
                }
            }

        }
    }
}
