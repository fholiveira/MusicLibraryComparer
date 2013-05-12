using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Input;
using System.IO;
using System.Collections.ObjectModel;
using MusicLibrary.ViewModel.Messaging;
using MusicLibrary.Model;

namespace MusicLibrary.ViewModel
{
    public class LoadLibraries : INotifyPropertyChanged, IMessageSender
    {
        #region Commands declaration
        public ICommand SelectFirstLibrary { get; private set; }
        public ICommand SelectSecondLibrary { get; private set; }
        public ICommand CompareLibraries { get; private set; }
        public ICommand SelectComparison { get; private set; }
        public ICommand LoadComparison { get; private set; }
        #endregion

        #region Properties
        private string _firstLibrary;
        public string FirstLibrary
        {
            get { return _firstLibrary; }
            set
            {
                _firstLibrary = value;
                this.OnPropertyChanged(() => this.FirstLibrary);
            }
        }

        private string _secondLibrary;
        public string SecondLibrary
        {
            get { return _secondLibrary; }
            set
            {
                _secondLibrary = value;
                this.OnPropertyChanged(() => this.SecondLibrary);
            }
        }

        private string _comparisonFile;
        public string ComparisonFile
        {
            get { return _comparisonFile; }
            set
            {
                _comparisonFile = value;
                this.OnPropertyChanged(() => this.ComparisonFile);
            }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                this.OnPropertyChanged(() => this.IsLoading);
            }
        }
        #endregion

        #region Fields
        IList<Album> albuns;
        #endregion

        #region CTOR
        public LoadLibraries()
        {
            this.SelectFirstLibrary = new RelayCommand(SelectFirstLibrary_Executed);
            this.SelectSecondLibrary = new RelayCommand(SelectSecondLibrary_Executed);

            this.CompareLibraries = new RelayCommand(LoadLibraries_Executed, LoadLibraries_CanExecute);

            this.SelectComparison = new RelayCommand(SelectComparison_Executed);
            this.LoadComparison = new RelayCommand(LoadComparison_Executed, LoadComparison_CanExecute);
        }
        #endregion

        #region Commands

        #region Execution
        private void SelectFirstLibrary_Executed(object parameter)
        {
            this.FirstLibrary = this.SelectDirectory();
        }

        private void SelectSecondLibrary_Executed(object parameter)
        {
            this.SecondLibrary = this.SelectDirectory();
        }

        private void SelectComparison_Executed(object parameter)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "Arquivo de comparação|*.xml";
            dialog.Title = "Carregar comparação";
            dialog.Multiselect = false;
            dialog.ShowDialog();

            this.ComparisonFile = dialog.FileName;
        }

        private void LoadComparison_Executed(object parameter)
        {
            ObservableCollection<Album> albuns = null;
            BackgroundWorker bg = new BackgroundWorker();

            bg.DoWork += new DoWorkEventHandler(
                (s, e) =>
                {
                    albuns = SerializationManager.JsonLoad<ObservableCollection<Album>>(this.ComparisonFile);
                });

            bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(
                (s, e) =>
                {
                    this.StartLibraryComparer(albuns);
                    this.IsLoading = false;
                    this.SendMessage(ViewMessage.Close);
                });

            bg.RunWorkerAsync();
            this.IsLoading = true;
        }

        private void LoadLibraries_Executed(object parameter)
        {
            BackgroundWorker bg = new BackgroundWorker();

            bg.DoWork += new DoWorkEventHandler(LoadingLibraries_DoWork);

            bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(
                (s, e) =>
                {
                    this.StartLibraryComparer(new ObservableCollection<Album>(this.albuns.OrderBy(a => a.Title)));
                    this.IsLoading = false;
                    this.SendMessage(ViewMessage.Close);
                });

            bg.RunWorkerAsync();
            this.IsLoading = true;
        }
        #endregion

        #region Can execution

        private bool LoadLibraries_CanExecute(object parameter)
        {
            return (Directory.Exists(this.FirstLibrary) && Directory.Exists(this.SecondLibrary) && !this.IsLoading);
        }

        private bool LoadComparison_CanExecute(object parameter)
        {
            return (File.Exists(this.ComparisonFile) && !this.IsLoading);
        }

        #endregion

        #endregion

        #region Private Methods
        private void LoadingLibraries_DoWork(object sender, DoWorkEventArgs e)
        {
            var library1 = new LibraryIOManager().ReadAllMusic(this.FirstLibrary);
            var library2 = new LibraryIOManager().ReadAllMusic(this.SecondLibrary);

            this.albuns = new List<Album>();

            foreach (var albumAtual in library1.Albums)
            {
                var tituloDoAlbum = albumAtual.Value.First().Album;

                if (library2.ContainsAlbum(tituloDoAlbum))
                {
                    var segundoAlbum = library2.GetAlbum(tituloDoAlbum);


                    var musicasRepetidas = segundoAlbum.Select(m => m.Nome).Intersect(albumAtual.Value.Select(m => m.Nome));
                    if (musicasRepetidas.Any())
                    {
                        var novoAlbum = new Album(tituloDoAlbum)
                         {
                             FirstLibraryMusics = new ObservableCollection<Music>(albumAtual.Value.Where(m => musicasRepetidas.Contains(m.Nome)).OrderBy(m => m.Nome)),
                             SecondLibraryMusics = new ObservableCollection<Music>(segundoAlbum.Where(m => musicasRepetidas.Contains(m.Nome)).OrderBy(m => m.Nome)),
                         };

                        library2.RemoveAlbum(novoAlbum.Title);

                        this.albuns.Add(novoAlbum);
                    }
                }
            }
        }

        private void StartLibraryComparer(ObservableCollection<Album> albuns)
        {
            LibraryComparer libraryComparer = new LibraryComparer(albuns);
            Messaging.ViewCatalog.RegisterView(libraryComparer);
            libraryComparer.SendMessage(ViewMessage.Show);
        }

        private string SelectDirectory()
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();

            return dialog.SelectedPath;
        }
        #endregion

        #region INotifyPropertyChanged implementation
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
    }
}
