using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MusicLibrary.ViewModel.Messaging;
using MusicLibrary.ViewModel;

namespace MusicLibrary.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [ViewModel(typeof(LibraryComparer))]
    public partial class LibraryComparerView : Window, IMessageListener
    {
        public LibraryComparerView()
        {
            InitializeComponent();
        }

        #region IMessageListener implementation
        public void OnMessageArrive(ViewMessage message)
        {
            switch (message)
            {
                case ViewMessage.Show:
                    this.Show();
                    break;
                case ViewMessage.Hide:
                    this.Hide();
                    break;
                case ViewMessage.Close:
                    this.Close();
                    ViewCatalog.UnregisterView(this);
                    break;
            }
        }

        public void Initialize(IMessageSender sender)
        {
            this.DataContext = sender;
            sender.SendMessageEvent +=new ViewMessageHandler(OnMessageArrive);
        }
        #endregion
    }
}
