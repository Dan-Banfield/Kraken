using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace Kraken
{
    public partial class MainWindow : Window
    {
        #region Private Variables

        private string selectedDirectory = string.Empty;

        #endregion

        #region Global Variables

        public enum STATUS
        {
            READY,
            BUSY
        }

        public string SelectedDirectory
        {
            get
            {
                return selectedDirectory;
            }
            set
            {
                selectedDirectory = value;
                selectedDirectoryTextBox.Text = value;
            }
        }

        #endregion

        #region Event Handlers

        public void selectDirectoryButton_Click(object sender, RoutedEventArgs e) =>
            SelectDirectory();

        public void startButton_Click(object sender, RoutedEventArgs e) =>
            DeleteDuplicateFiles(SelectedDirectory);

        public void closeButton_Click(object sender, RoutedEventArgs e) => 
            Close();

        #endregion

        public MainWindow()
        {
            InitializeComponent();
        }

        #region Methods

        private async void SelectDirectory()
        {
            OpenFolderDialog ofd = new OpenFolderDialog();

            string? result = await ofd.ShowAsync(this);

            if (result != null)
                SelectedDirectory = result;

            return;
        }

        private async void DeleteDuplicateFiles(string directory)
        {
            SetStatus(STATUS.BUSY);

            int fileCount = await GetFileCount(directory);
            statusProgressBar.Maximum = fileCount;

            HashSet<string> fileHashSet = new HashSet<string>();
            string tempFileHash = "";
            
            foreach (string file in Directory.GetFiles(directory))
            {
                tempFileHash = await ComputeMD5Hash(file);

                if (fileHashSet.Contains(tempFileHash))
                {
                    File.Delete(file);
                }
                else
                {
                    fileHashSet.Add(tempFileHash);
                }

                statusProgressBar.Value += 1;
            }

            SetStatus(STATUS.READY);
        }

        private async Task<string> ComputeMD5Hash(string filePath)
        {
            string result = "";

            await Task.Run(() => 
            {
                using (MD5 md5 = MD5.Create())
                {
                    using (FileStream stream = File.OpenRead(filePath))
                    {
                        byte[] hash = md5.ComputeHash(stream);
                        result = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                    }
                }
            });

            return result;
        }

        private async Task<int> GetFileCount(string directory)
        {
            int result = 0;

            await Task.Run(() =>
            {
                result = Directory.GetFiles(directory).Count();
            });

            return result;
        }

        private void SetStatus(STATUS status)
        {
            switch (status)
            {
                case STATUS.READY:
                    statusLabel.Content = "READY";
                    startButton.IsEnabled = true;
                    break;
                case STATUS.BUSY:
                    statusLabel.Content = "BUSY";
                    startButton.IsEnabled = false;
                    break;
            }
        }

        #endregion
    }
}
