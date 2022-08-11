using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace LinksList;

public partial class EditGroupWindow : Window
{
    public LinkGroup? linkGroup { get; set; }
    private List<ChangedElement> _changedElementsList = new List<ChangedElement>();
    public List<int> removedLinksList = new List<int>();
    private List<DockPanel> dockPanelsList = new List<DockPanel>();

    public EditGroupWindow()
    {
        InitializeComponent();
        
        if (AppConfig.BackgroundPath != "None")
        {
            AppConfig.appSystem?.SetBackground(this);
        }
    }

    private void CancelButtonClick(object sender, RoutedEventArgs e) =>
        Close();

    private void EditGroupWindowLoaded(object sender, RoutedEventArgs e)
    {
        Title = $"{linkGroup?.Header} - Изменить группу";
        HeaderTextBox.Text = linkGroup?.Header;
        LoadAllLinks();
    }
    
    private void LoadAllLinks()
    {
        DockPanel dockPanel;
        TextBox linkTextBox;
        Button removeLinkButton;
        
        for (int i = 0; i < linkGroup?.LinksList.Count; i++)
        {
            dockPanel = new DockPanel();
            dockPanel.Margin = new Thickness(0, 10, 0, 0);
            dockPanel.LastChildFill = true;
            
            linkTextBox = new TextBox();
            linkTextBox.Margin = new Thickness(10, 0, 30, 0);
            linkTextBox.Text = linkGroup.LinksList[i];
            linkTextBox.FontSize = 20;
            linkTextBox.Height = 30;
            linkTextBox.VerticalContentAlignment = VerticalAlignment.Center;
            linkTextBox.TextChanged += (sender, args) =>
            {
                TextBox textBox = (TextBox)sender;
                DockPanel? _dockPanel = textBox.Parent as DockPanel;
                int dockIndex = stackPanel.Children.IndexOf(_dockPanel);

                if (textBox.Text != _changedElementsList[dockIndex].PrevValue)
                {
                    _changedElementsList[dockIndex].CurrentValue = textBox.Text;
                }
            };

            removeLinkButton = new Button();
            removeLinkButton.Margin = new Thickness(20, 0, 0, 0);
            removeLinkButton.Width = 30;
            removeLinkButton.Height = 30;
            removeLinkButton.FontSize = 20;
            removeLinkButton.Content = "-";
            removeLinkButton.Click += (obj, args) =>
            {
                Button button = (Button)obj;
                DockPanel? _dockPanel = button.Parent as DockPanel;
                int index = stackPanel.Children.IndexOf(_dockPanel);
                removedLinksList.Add(index);

                if (index >= 0)
                {
                    stackPanel.Children.RemoveAt(index);
                    dockPanelsList.RemoveAt(index);
                }
            };

            _changedElementsList.Add(new ChangedElement(i, linkTextBox.Text, "None"));

            dockPanel.Children.Add(removeLinkButton);
            dockPanel.Children.Add(linkTextBox);
            dockPanelsList.Add(dockPanel);
            stackPanel.Children.Add(dockPanel);
        }
    }
    
    private void ChangeButtonClick(object sender, RoutedEventArgs e)
    {
        LinkGroup? _linkGroup = AppConfig.LinkGroupsList.FirstOrDefault(group => group?.Header == linkGroup?.Header);
        int index = AppConfig.LinkGroupsList.IndexOf(_linkGroup);

        if (HeaderTextBox.Text != linkGroup?.Header)
        {
            AppConfig.LinkGroupsList[index]!.Header = HeaderTextBox.Text;
            AppConfig.appSystem?.ChangeHeaderInJson(index, HeaderTextBox.Text);
        }
        
        for (int i = 0; i < removedLinksList.Count; i++)
        {
            if (AppConfig.LinkGroupsList[index]?.LinksList.Count >= removedLinksList[i])
            {
                AppConfig.LinkGroupsList[index]?.LinksList.RemoveAt(removedLinksList[i]);
                AppConfig.appSystem?.RemoveLinkInJson(index, i);
            }
        }

        for (int i = 0; i < stackPanel.Children.Count; i++)
        {
            if (stackPanel.Children[i].GetType() == typeof(DockPanel))
            {
                if (_changedElementsList[i].CurrentValue == "None")
                {
                    continue;
                }
                
                if (_changedElementsList[i].PrevValue != _changedElementsList[i].CurrentValue && _changedElementsList[i].PrevValue != "Ссылка")
                {
                    AppConfig.LinkGroupsList[index]!.LinksList[i] = _changedElementsList[i].CurrentValue!;
                    AppConfig.appSystem?.ChangeLinkInJson(index, i, _changedElementsList[i].CurrentValue!);
                }
                else
                {
                    AppConfig.LinkGroupsList[index]!.LinksList.Add(_changedElementsList[i].CurrentValue!);
                    AppConfig.appSystem?.AddLinkInJson(index, _changedElementsList[i].CurrentValue!);
                }
            }
        }
        
        AppConfig.appSystem?.RedrawAllGroups();

        Close();
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
        linkTextBox.TextChanged += (sender, args) =>
        {
            TextBox textBox = (TextBox)sender;
            DockPanel? _dockPanel = textBox.Parent as DockPanel;
            int dockIndex = stackPanel.Children.IndexOf(_dockPanel);
            LinkGroup? _linkGroup = AppConfig.LinkGroupsList.FirstOrDefault(group => group?.Header == linkGroup.Header);
                
            if (textBox.Text != _changedElementsList[dockIndex].CurrentValue)
            {
                _changedElementsList[dockIndex].CurrentValue = textBox.Text;
            }
        };

        Button removeLinkButton = new Button();
        removeLinkButton.Margin = new Thickness(20, 0, 0, 0);
        removeLinkButton.Width = 30;
        removeLinkButton.Height = 30;
        removeLinkButton.FontSize = 20;
        removeLinkButton.Content = "-";
        removeLinkButton.Click += (obj, args) =>
        {
            TextBox textBox;
            Button button = (Button)obj;
            DockPanel? _dockPanel = button.Parent as DockPanel;
            int index = stackPanel.Children.IndexOf(_dockPanel);
            removedLinksList.Add(index);

            if (index >= 0)
            {
                stackPanel.Children.RemoveAt(index);
                dockPanelsList.RemoveAt(index);
            }
        };
        
        _changedElementsList.Add(new ChangedElement(dockPanelsList.Count, linkTextBox.Text, "New"));

        dockPanel.Children.Add(removeLinkButton);
        dockPanel.Children.Add(linkTextBox);
        dockPanelsList.Add(dockPanel);
        stackPanel.Children.Add(dockPanel);
    }
}