using System;
using System.Collections.Generic;
using System.Linq;
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
using tpiPracticeGUI.Frames;

namespace tpiPracticeGUI.Controls
{
    /// <summary>
    /// Interaction logic for UserHeaderUserControl.xaml
    /// </summary>
    public partial class UserHeaderUserControl : UserControl
    {
        public UserHeaderUserControl()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        public void setHeaderText(string text)
        {
            usernameLabel.Content = text;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Globals.currentLoggedUser = null;
        }
    }
}
