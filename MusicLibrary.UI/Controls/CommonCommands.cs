using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using MusicLibrary.ViewModel;
using MusicLibrary.ViewModel.Messaging;

namespace MusicLibrary.View.Controls
{
    public class CommonCommands
    {
        public static ICommand Close { get; private set; }
        public static ICommand Hide { get; private set; }
        public static ICommand Show { get; private set; }

        static CommonCommands()
        {
            Close = new RelayCommand<IMessageSender>(Close_Executed);
            Hide = new RelayCommand<IMessageSender>(Hide_Executed);
            Show = new RelayCommand<IMessageSender>(Show_Executed);
        }

        private static void Close_Executed(IMessageSender parameter)
        {
            parameter.SendMessage(ViewMessage.Close);
        }

        private static void Hide_Executed(IMessageSender parameter)
        {
            parameter.SendMessage(ViewMessage.Hide);
        }

        private static void Show_Executed(IMessageSender parameter)
        {
            parameter.SendMessage(ViewMessage.Show);
        }
    }
}
