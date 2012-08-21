using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using MusicLibrary.Model;

namespace MusicLibrary.ViewModel
{
    [Serializable()]
    public class Album : INotifyPropertyChanged
    {
        #region CTOR
        public Album()
        {

        }

        public Album(string title)
        {
            this.Title = title;
        }
        #endregion

        #region Properties
        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                this.OnPropertyChanged(() => this.Title);
            }
        }

        private ObservableCollection<Music> _firstLibraryMusics;
        public ObservableCollection<Music> FirstLibraryMusics
        {
            get { return _firstLibraryMusics; }
            set
            {
                _firstLibraryMusics = value;

                if (_firstLibraryMusics != null)
                    this.SelectedFirstLibraryMusic = _firstLibraryMusics.FirstOrDefault();

                this.OnPropertyChanged(() => this.FirstLibraryMusics);
            }
        }

        private ObservableCollection<Music> _secondLibraryMusics;
        public ObservableCollection<Music> SecondLibraryMusics
        {
            get { return _secondLibraryMusics; }
            set
            {
                _secondLibraryMusics = value;

                if (_secondLibraryMusics != null)
                    this.SelectedSecondLibraryMusic = _secondLibraryMusics.FirstOrDefault();

                this.OnPropertyChanged(() => this.SecondLibraryMusics);
            }
        }

        private Music _selectedFirstLibraryMusic;
        public Music SelectedFirstLibraryMusic
        {
            get { return _selectedFirstLibraryMusic; }
            set
            {
                _selectedFirstLibraryMusic = value;
                this.OnPropertyChanged(() => this.SelectedFirstLibraryMusic);
            }
        }

        private Music _selectedSecondLibraryMusic;
        public Music SelectedSecondLibraryMusic
        {
            get { return _selectedSecondLibraryMusic; }
            set
            {
                _selectedSecondLibraryMusic = value;
                this.OnPropertyChanged(() => this.SelectedSecondLibraryMusic);
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
