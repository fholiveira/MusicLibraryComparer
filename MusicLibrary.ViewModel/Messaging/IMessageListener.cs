using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MusicLibrary.ViewModel.Messaging
{
    public interface IMessageListener
    {
        void Initialize(IMessageSender sender);
        void OnMessageArrive(ViewMessage message);
    }
}
