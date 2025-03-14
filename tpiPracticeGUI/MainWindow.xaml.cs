using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup.Localizer;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using tpiPracticeClasses;
using tpiPracticeGUI.Frames;

namespace tpiPracticeGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        DispatcherTimer timer;

        public MainWindow()
        {

            timer = new DispatcherTimer();

            InitializeComponent();
            Globals.OnPhaseChanged += onPhaseChanged;
            Globals.OnUserDisconnect += Globals_OnUserDisconnect;

            timer.Interval = TimeSpan.FromSeconds(0.5);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {

        }

        private void Globals_OnUserDisconnect(object? sender, EventArgs e)
        {
            Globals.phase = Globals.Phase.login;
        }

        private void onPhaseChanged(object? sender, EventArgs e)
        {

            Globals.getGroupChatList.Clear();

            switch (Globals.phase)
            {
                case Globals.Phase.signup:
                    loginGUI.Visibility = Visibility.Collapsed;
                    groupchatListingGUI.Visibility = Visibility.Collapsed;
                    chattingUsercontrol.Visibility = Visibility.Collapsed;
                    signupGUI.Visibility = Visibility.Visible;
                    break;
                case Globals.Phase.login:
                    loginGUI.Visibility = Visibility.Visible;
                    groupchatListingGUI.Visibility = Visibility.Collapsed;
                    signupGUI.Visibility = Visibility.Collapsed;
                    chattingUsercontrol.Visibility = Visibility.Collapsed;
                    break;
                case Globals.Phase.groupchatListing:
                    loginGUI.Visibility = Visibility.Collapsed;
                    groupchatListingGUI.Visibility = Visibility.Visible;
                    signupGUI.Visibility = Visibility.Collapsed;
                    chattingUsercontrol.Visibility = Visibility.Collapsed;
                    break;
                case Globals.Phase.chatting:
                    loginGUI.Visibility = Visibility.Collapsed;
                    groupchatListingGUI.Visibility = Visibility.Collapsed;
                    signupGUI.Visibility = Visibility.Collapsed;
                    chattingUsercontrol.Visibility = Visibility.Visible;
                    break;
                default:
                    break;
            }
        }
    }
}