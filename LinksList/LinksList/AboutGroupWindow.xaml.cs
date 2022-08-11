using System.Runtime.InteropServices.ObjectiveC;
using System.Windows;
using System.Windows.Controls;

namespace LinksList;

public partial class AboutGroupWindow : Window
{
    public LinkGroup? linkGroup;

    public AboutGroupWindow()
    {
        InitializeComponent();
        
        if (AppConfig.BackgroundPath != "None")
        {
            AppConfig.appSystem?.SetBackground(this);
        }
    }

    private void LoadAllLinks()
    {
        TextBox linkTextBox;
        
        for (int i = 0; i < linkGroup.LinksList.Count; i++)
        {
            linkTextBox = new TextBox();
            
            linkTextBox.Margin = new Thickness(30, 10, 30, 0);
            linkTextBox.Text = linkGroup.LinksList[i];
            linkTextBox.FontSize = 20;
            linkTextBox.Height = 30;
            linkTextBox.VerticalContentAlignment = VerticalAlignment.Center;
            linkTextBox.IsReadOnly = true;
            linkTextBox.MouseDoubleClick += (sender, args) =>
            {
                TextBox textBox = (TextBox)sender;
                Clipboard.SetText(textBox.Text);
                MessageBox.Show("Ссылка успешно скопирована!");
            };

            stackPanel.Children.Add(linkTextBox);
        }
    }

    private void CloseButtonClick(object sender, RoutedEventArgs e) =>
        Close();

    private void AboutGroupWindowLoaded(object sender, RoutedEventArgs e)
    {
        Title = $"{linkGroup.Header} - Просмотр группы";
        HeaderTextBox.Text = linkGroup.Header;
        LoadAllLinks();
    }
}