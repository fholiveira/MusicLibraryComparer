using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MusicLibrary.Model
{
    public class LibraryIOManager
    {
        #region Fields
        private MusicsLibrary musics;
        private List<string> errors;
        #endregion

        #region Public Methods
        public MusicsLibrary ReadAllMusic(string directory)
        {
            this.musics = new MusicsLibrary();
            this.errors = new List<string>();
            string[] directories = Directory.GetDirectories(directory);
            string[] files = null; 
            
            files = Directory.GetFiles(directory, "*.m4a");
            if (files != null && files.Length > 0)
                this.ReadMusic(files);

            files = Directory.GetFiles(directory, "*.mp3");
            if (files != null && files.Length > 0)
                this.ReadMusic(files);

            if (directories != null && directories.Length > 0)
                this.ReadAllMusic(directories);

            return this.musics;
        }

        public static Music ReadMusic(string filename)
        {
            TagLib.File file = TagLib.File.Create(filename);
            return Convert(file);
        }

        public void RemoveEmptySubDirectories(string directory)
        {
            this.errors = new List<string>();
            string[] extensions = new[] { ".m4a", ".mp3" };

            if (Directory.Exists(directory))
            {
                string[] directories = Directory.GetDirectories(directory);
                this.RemoveEmptySubDirectories(directories, extensions);
            }
            else
            {
                this.errors.Add(directory);
            }
        }
        #endregion

        #region Private Methods
        private static Music Convert(TagLib.File tag)
        {
            return new Music()
            {
                Album = tag.Tag.Album ?? "--- Desconhecido ---",
                Ano = (int)tag.Tag.Year,
                Artista = String.Join(", ", tag.Tag.Performers),
                ArtistaDoAlbum = String.Join(", ", tag.Tag.AlbumArtists),
                Nome = tag.Tag.Title,
                Faixa = (int)tag.Tag.Track,
                FileName = tag.Name,
            };
        }

        private void ReadMusic(IEnumerable<string> filenames)
        {
            TagLib.File file = null;

            foreach (var currentFile in filenames)
            {
                try
                {
                    file = TagLib.File.Create(currentFile);

                    this.musics.AddAlbum(Convert(file));
                }
                catch 
                {
                    this.errors.Add(currentFile);
                }
            }
        }

        private void ReadAllMusic(string[] directories)
        {
            foreach (var directory in directories)
            {
                string[] subDirectories = Directory.GetDirectories(directory);
                string[] files = null;

                files = Directory.GetFiles(directory, "*.m4a");
                if (files != null && files.Length > 0)
                    this.ReadMusic(files);

                files = Directory.GetFiles(directory, "*.mp3");
                if (files != null && files.Length > 0)
                    this.ReadMusic(files);

                if (directories != null && directories.Length > 0)
                    this.ReadAllMusic(subDirectories);
            }
        }

        private void RemoveEmptySubDirectories(string[] directories, string[] extensions)
        {
            foreach (var directory in directories)
            {
                string[] subDirectories = Directory.GetDirectories(directory);

                if (subDirectories.Length == 0)
                {
                    string[] files = Directory.GetFiles(directory);

                    if (files.Count(f => extensions.Contains(Path.GetExtension(f))) == 0)
                    {
                        try
                        {
                            foreach (string file in files)
                            {
                                File.Delete(file);
                            }

                            Directory.Delete(directory);
                        }
                        catch { this.errors.Add(directory); }
                    }
                }
                else
                {
                    this.RemoveEmptySubDirectories(subDirectories, extensions);
                }
            }
        }
        #endregion
    }
}
