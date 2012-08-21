using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace MusicLibrary.Model
{
    [Serializable()]
    public class MusicsLibrary
    {
        #region Properties
        public IDictionary<string, List<Music>> Albums { get; private set; }
        #endregion

        #region Fields
        [field: NonSerialized()]
        private KeyValuePair<string, List<Music>> currentAlbum;
        #endregion

        #region CTOR
        public MusicsLibrary()
        {
            this.Albums = new Dictionary<string, List<Music>>();
        }
        #endregion

        #region Public Methods
        public void AddAlbum(Music music)
        {
            this.FindCurrentAlbum(music.Album);
            this.currentAlbum.Value.Add(music);
        }

        public IEnumerable<Music> GetAlbum(string albumName)
        {
            albumName = albumName.ToUpper();
            if (this.Albums.ContainsKey(albumName))
            {
               return this.Albums[albumName];
            }

            return null;
        }

        public void RemoveAlbum(string albumName)
        { 
            albumName = albumName.ToUpper();
            if (this.Albums.ContainsKey(albumName))
            {
                this.Albums.Remove(albumName);
            }
        }

        public bool ContainsAlbum(string albumName)
        {
            return this.Albums.ContainsKey(albumName.ToUpper());
        }
        #endregion

        #region Private Methods
        private void FindCurrentAlbum(string albumName)
        {
            albumName = albumName.ToUpper();

            if (!albumName.Equals(this.currentAlbum.Key))
            {
                if (this.Albums.ContainsKey(albumName))
                {
                    this.currentAlbum = new KeyValuePair<string, List<Music>>
                    (
                       albumName,
                       this.Albums[albumName]
                    );
                }
                else
                {
                    this.currentAlbum = new KeyValuePair<string, List<Music>>
                    (
                       albumName,
                       new List<Music>()
                    );

                    this.Albums.Add(this.currentAlbum);
                }
            }
        }
        #endregion
    }
}
