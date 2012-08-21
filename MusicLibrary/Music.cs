using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace MusicLibrary.Model
{
    [Serializable()]
    public class Music : INotifyPropertyChanged
    {
        #region Tag
        private string _nome;
        public string Nome
        {
            get { return _nome; }
            set
            {
                _nome = value;
                this.OnPropertyChanged(() => this.Nome);
            }
        }


        private string _artista;
        public string Artista
        {
            get { return _artista; }
            set
            {
                _artista = value;
                this.OnPropertyChanged(() => this.Artista);
            }
        }

        private string _album;
        public string Album
        {
            get { return _album; }
            set
            {
                _album = value;
                this.OnPropertyChanged(() => this.Album);
            }
        }

        private int _faixa;
        public int Faixa
        {
            get { return _faixa; }
            set
            {
                _faixa = value;
                this.OnPropertyChanged(() => this.Faixa);
            }
        }

        private int _ano;
        public int Ano
        {
            get { return _ano; }
            set
            {
                _ano = value;
                this.OnPropertyChanged(() => this.Ano);
            }
        }

        private string _artistaDoAlbum;
        public string ArtistaDoAlbum
        {
            get { return _artistaDoAlbum; }
            set
            {
                _artistaDoAlbum = value;
                this.OnPropertyChanged(() => this.ArtistaDoAlbum);
            }
        }
        #endregion Tag

        #region File 
        private string _fileName;
        public string FileName
        {
            get { return _fileName; }
            set
            {
                _fileName = value;
                this.OnPropertyChanged(() => this.FileName);
            }
        }
        #endregion

        #region Status
        [field: NonSerialized()]
        private bool _selected;
        public bool Selected
        {
            get { return _selected; }
            set
            {
                _selected = value;
                this.OnPropertyChanged(() => this.Selected);
            }
        }
        
        private bool _deleted;
        public bool Deleted
        {
            get { return _deleted; }
            set
            {
                _deleted = value;
                this.OnPropertyChanged(() => this.Deleted);
            }
        }
        #endregion

        #region Overriden Methods
        public override string ToString()
        {
            return String.Concat(this.Artista, " - ", this.Nome);
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
