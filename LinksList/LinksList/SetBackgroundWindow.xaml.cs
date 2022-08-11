using System.Windows;
using System.Windows.Forms;

namespace LinksList;

public partial class SetBackgroundWindow : Window
{
    private string _backgroundImage = string.Empty;
    public Window mainWindow;
    
    public SetBackgroundWindow()
    {
        InitializeComponent();
        if (AppConfig.BackgroundPath != "None")
        {
            AppConfig.appSystem.SetBackground(this);
        }
    }

    private void ChooseBackgroundImageButtonClick(object sender, RoutedEventArgs e)
    {
        var openFileDialog = new OpenFileDialog();
        openFileDialog.Title = "Browse Image Files";
        openFileDialog.Filter = "PNG Files(*.png)|*.png|JPG Files(*.jpg)|*.jpg|JPEG Files(*.jpeg)|*.jpeg";
        openFileDialog.CheckFileExists = true;
        openFileDialog.CheckPathExists = true;
        openFileDialog.ShowDialog();

        string filename = ReplaceFileName(openFileDialog.FileName);

        if (!string.IsNullOrWhiteSpace(filename))
        {
            BackgroundNameTextBox.Text = filename;
            _backgroundImage = openFileDialog.FileName;
        }
    }

    public string ReplaceFileName(string file)
    {
        string filename = "";

        if (file.LastIndexOf('/') != -1)
        {
            filename = file.Substring(file.LastIndexOf('/')).Replace("/", "");
        }
        else
        {
            filename = file.Substring(file.LastIndexOf('\\')).Replace(@"\", "");
        }

        return filename;
    }

    private void CancelButtonClick(object sender, RoutedEventArgs e) =>
        Close();

    private void ConfirmBackgroundImageButtonClick(object sender, RoutedEventArgs e)
    {
        AppConfig.BackgroundPath = _backgroundImage;
        AppConfig.appSystem.SetBackground(mainWindow);
        AppConfig.appSystem.SaveBackgroundImage();
        Close();
    }
}