using System;
using System.Windows;
using System.Windows.Controls;

namespace LinksList;

public partial class AddGroupWindow
{
    public AddGroupWindow()
    {
        InitializeComponent();
        if (AppConfig.BackgroundPath != "None")
        {
            AppConfig.appSystem?.SetBackground(this);
        }
    }

    private void AddText(object sender, EventArgs e)
    {
        TextBox instance = (TextBox)sender;
        if (instance.Text == instance.Tag.ToString())
        {
            instance.Text = "";
        }
    }

    private void RemoveText(object sender, EventArgs e)
    {
        TextBox instance = (TextBox)sender;
        if (string.IsNullOrWhiteSpace(instance.Text))
        {
            instance.Text = instance.Tag.ToString();
        }
    }

    private void AddLinkFieldButtonClick(object sender, RoutedEventArgs e)
    {
        DockPanel dockPanel = new DockPanel();
        dockPanel.Margin = new Thickness(0, 10, 0, 0);
        dockPanel.LastChildFill = true;
        
        TextBox linkTextBox = new TextBox();
        linkTextBox.Margin = new Thickness(10, 0, 30, 0);
        linkTextBox.Tag = "Ссылка";
        linkTextBox.Text = "Ссылка";
        linkTextBox.FontSize = 20;
        linkTextBox.Height = 30;
        linkTextBox.VerticalContentAlignment = VerticalAlignment.Center;
        linkTextBox.HorizontalContentAlignment = HorizontalAlignment.Stretch;
        linkTextBox.GotFocus += (obj, args) => AddText(obj, args);
        linkTextBox.LostFocus += (obj, args) => RemoveText(obj, args);

        Button removeLinkButton = new Button();
        removeLinkButton.Margin = new Thickness(20, 0, 0, 0);
        removeLinkButton.Width = 30;
        removeLinkButton.Height = 30;
        removeLinkButton.FontSize = 20;
        removeLinkButton.Content = "-";
        removeLinkButton.Click += (_, _) =>
        {
            int index = stackPanel.Children.IndexOf(dockPanel);
            stackPanel.Children.RemoveAt(index);
        };

        dockPanel.Children.Add(removeLinkButton);
        dockPanel.Children.Add(linkTextBox);
        stackPanel.Children.Add(dockPanel);
    }
    
    private void AddLinkGroupButtonClick(object sender, RoutedEventArgs e)
    {
        TextBox? linkTextBox;
        DockPanel? dockPanel;
        string? header = HeaderTextBox.Text;

        if (string.IsNullOrWhiteSpace(header) || header.ToLower() == "ссылка")
        {
            return;
        }
        
        LinkGroup? linkGroup = new LinkGroup(header);

        for (int i = 0; i < stackPanel.Children.Count; i++)
        {
            if (stackPanel.Children[i].GetType() == typeof(DockPanel))
            {
                dockPanel = stackPanel.Children[i] as DockPanel;
                linkTextBox = dockPanel?.Children[1] as TextBox;
                if (!string.IsNullOrWhiteSpace(linkTextBox?.Text.Trim()) && linkTextBox.Text.ToLower() != "ссылка")
                {
                    linkGroup.LinksList.Add(linkTextBox.Text);
                }
            }
        }
        
        AppConfig.appSystem?.AddGroup(linkGroup);
        Close();
    }

    private void CancelButtonClick(object sender, RoutedEventArgs e) =>
        Close();
}