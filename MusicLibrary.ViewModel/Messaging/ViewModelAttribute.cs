using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MusicLibrary.ViewModel.Messaging
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class ViewModelAttribute : Attribute
    {
        public ViewModelAttribute(Type viewModelType)
        {
            this.ViewModelType = viewModelType;
        }

        public Type ViewModelType { get; private set; }
    }
}
