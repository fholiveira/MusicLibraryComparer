using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.IO;
using MusicLibrary.ViewModel.Messaging;
using MusicLibrary.Model;

namespace MusicLibrary.ViewModel
{
    public class LibraryComparer : INotifyPropertyChanged, IMessageSender
    {
        #region Commands declaration
        public ICommand DeleteSelectedMusics { get; private set; }
        public ICommand SaveComparison { get; private set; }
        #endregion

        #region Properties
        private ObservableCollection<Album> _albuns;
        public ObservableCollection<Album> Albuns
        {
            get { return _albuns; }
            set
            {
                _albuns = value;

                if (_albuns != null)
                    this.SelectedAlbum = _albuns.FirstOrDefault();

                this.OnPropertyChanged(() => this.Albuns);
            }
        }

        private Album _selectedAlbum;
        public Album SelectedAlbum
        {
            get { return _selectedAlbum; }
            set
            {
                _selectedAlbum = value;
                this.OnPropertyChanged(() => this.SelectedAlbum);
            }
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                this.OnPropertyChanged(() => this.IsBusy);
            }
        }
        #endregion

        #region CTOR
        private LibraryComparer()
        {
            this.DeleteSelectedMusics = new RelayCommand<ObservableCollection<Music>>(DeleteSelectedMusics_Executed);
            this.SaveComparison = new RelayCommand<ObservableCollection<Album>>(SaveComparison_Executed);
        }

        public LibraryComparer(ObservableCollection<Album> albuns)
            : this()
        {
            this.Albuns = albuns;
        }
        #endregion

        #region Commands execution
        private void DeleteSelectedMusics_Executed(ObservableCollection<Music> musics)
        {
            var musicsToRemove = musics.Where(m => m.Selected).ToArray();
            foreach (var music in musicsToRemove)
            {
                if (File.Exists(music.FileName))
                {
                    Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(music.FileName, 
                        Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs, 
                        Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin);

                    if (!File.Exists(music.FileName))
                        music.Deleted = true;
                }
            }
        }

        private void SaveComparison_Executed(ObservableCollection<Album> comparation)
        {
            BackgroundWorker bg = new BackgroundWorker();

            bg.DoWork += new DoWorkEventHandler(
                (s, e) => 
                {
                    Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();
                    dialog.Filter = "Arquivo de comparação|*.xml";
                    dialog.Title = "Salvar comparação";

                    if (dialog.ShowDialog() == true)
                    {
                        SerializationManager.BinarySave(comparation, dialog.FileName);
                    }
                });

            bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(
                (s, e) =>
                {
                    this.IsBusy = false;
                });

            bg.RunWorkerAsync();
            this.IsBusy = true;
        }
        #endregion

        #region IMessageSender implementation
        public event ViewMessageHandler SendMessageEvent;

        public void SendMessage(ViewMessage message)
        {
            if (this.SendMessageEvent != null)
            {
                this.SendMessageEvent(message);
            }
        }
        #endregion

        #region INotifyPropertyChanged implementation
        [field: NonSerialized()]
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged<T>(System.Linq.Expressions.Expression<Func<T>> property)
        {
            if (PropertyChanged != null)
            {
                System.Linq.Expressions.MemberExpression memberExpression = property.Body as System.Linq.Expressions.MemberExpression;
                if (memberExpression != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs(memberExpression.Member.Name));
                }
            }
        }
        #endregion
    }
}
