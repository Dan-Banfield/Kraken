using Avalonia.Controls;

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

        public STATUS CURRENT_STATUS = STATUS.READY;

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

        #endregion

        public MainWindow()
        {
            InitializeComponent();
        }

        #region Methods

        #endregion
    }
}
