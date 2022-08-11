using System;
using System.Windows;
using MessageBox = System.Windows.MessageBox;

namespace LinksList
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            AppConfig.netLoggerDesktop.Init();
            
            try
            {
                AppConfig.appSystem = new AppSystem(LinkGroupStackPanel);
                if (AppConfig.appSystem.DataFileExists())
                {
                    AppConfig.appSystem.LoadAllData();
                    if (AppConfig.BackgroundPath != "None")
                    {
                        AppConfig.appSystem.SetBackground(this);
                    }
                    AppConfig.appSystem.DrawAllGroups();
                }
                else
                {
                    AppConfig.appSystem.CreateDataFile();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.StackTrace, "Ошибка");
            }
        }
        
        private void AddLinkGroupButtonClick(object sender, RoutedEventArgs e)
        {
            AddGroupWindow addGroupWindow = new AddGroupWindow();
            addGroupWindow.Show();
        }

        private void SetBackgroundButtonClick(object sender, RoutedEventArgs e)
        {
            SetBackgroundWindow setBackgroundWindow = new SetBackgroundWindow() {mainWindow = this};
            setBackgroundWindow.Show();
        }

        private void RemoveBackgroundButtonClick(object sender, RoutedEventArgs e) =>
            AppConfig.appSystem?.RemoveBackground(this);
    }
}