using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using MusicLibrary.ViewModel.Messaging;
using MusicLibrary.ViewModel;

namespace MusicLibrary.View
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            this.Startup += new StartupEventHandler(App_Startup);
            //new MusicLibrary.Model.LibraryIOManager().RemoveEmptySubDirectories(@"C:\Users\fernando.oliveira\Desktop\Music");
        }

        void App_Startup(object sender, StartupEventArgs e)
        {
            ViewCatalog.Initialize(System.Reflection.Assembly.GetExecutingAssembly());

            LoadLibraries loadLibraries = new LoadLibraries();
            ViewCatalog.RegisterView(loadLibraries);
            loadLibraries.SendMessage(ViewMessage.Show);
        }
    }
}
