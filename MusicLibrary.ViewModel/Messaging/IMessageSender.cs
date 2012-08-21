using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MusicLibrary.ViewModel.Messaging
{
    public interface IMessageSender
    {
        event ViewMessageHandler SendMessageEvent;

        void SendMessage(ViewMessage message);
    }
}
