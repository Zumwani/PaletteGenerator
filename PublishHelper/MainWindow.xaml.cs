using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using static PublishHelper.Strings;

namespace PublishHelper
{

    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        public MainWindow() =>
            InitializeComponent();

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {

            Version = IncrementVersion.Increment(project);

            FrameworkDependentStatus = InProgress;
            OnPropertyChanged();

            await Dotnet.Publish(frameworkDependentProfile, UpdatePublishProgress);

            FrameworkDependentStatus = Ready;
            SelfContainedStatus = InProgress;
            OnPropertyChanged();

            PublishText.Text += Environment.NewLine + Environment.NewLine + string.Join("", Enumerable.Repeat("-", 30)) + Environment.NewLine + Environment.NewLine;
            await Dotnet.Publish(selfContainedProfile, UpdatePublishProgress);
            
            SelfContainedStatus = Ready;
            ProgressBar.IsIndeterminate = false;
            ProgressBar.Value = ProgressBar.Maximum;
            OnPropertyChanged();

            DoneMessage.Visibility = Visibility.Visible;

        }

        void UpdatePublishProgress(string message) =>
            Application.Current?.Dispatcher?.Invoke(() =>
        {
            PublishText.Visibility = Visibility.Visible;
            PublishText.Text += message + Environment.NewLine + Environment.NewLine;
            ScrollViewer.ScrollToBottom();
        });

        public string Version { get; set; }
        public string FrameworkDependentStatus { get; set; } = Queued;
        public string SelfContainedStatus { get; set; } = Queued;

        #region INotifyPropertyChanged
        
        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged() =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(string.Empty));

        #endregion

        private void Window_Closed(object sender, EventArgs e) =>
            Dotnet.CancelPublish();

    }

}
