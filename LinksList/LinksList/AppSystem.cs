using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Newtonsoft.Json;

namespace LinksList;

public class AppSystem
{
    private StackPanel? _stackPanel { get; set; }
    private Json? json;

    public AppSystem(StackPanel stackPanel)
    {
        _stackPanel = stackPanel;
    }

    public void RedrawAllGroups()
    {
        for (int i = 0; i < AppConfig.LinkGroupsList.Count; i++)
        {
            AppConfig.DrawedLinkGroup[i] = false;
        }
            
        _stackPanel?.Children.Clear();
        DrawAllGroups();
    }

    public bool DataFileExists() =>
        File.Exists("data.json");

    public void CreateDataFile()
    {
        if (DataFileExists())
        {
            MessageBox.Show("Файл data.json уже существует", "Ошибка");
            return;
        }
        
        json = new Json();
        json.Background = !string.IsNullOrWhiteSpace(AppConfig.BackgroundPath) ? AppConfig.BackgroundPath : "None";

        for (int i = 0; i < AppConfig.LinkGroupsList.Count; i++)
        {
            json.LinkGroupList.Add(AppConfig.LinkGroupsList[i]);
        }

        string _json = JsonConvert.SerializeObject(json, Formatting.Indented);

        using (StreamWriter writer = new StreamWriter("data.json", true))
        {
            writer.WriteLine(_json);
            writer.Close();
        }
    }

    public void DrawAllGroups()
    {
        LinkGroupCard card;

        for (int i = 0; i < AppConfig.LinkGroupsList.Count; i++)
        {
            card = new LinkGroupCard();
            card.Header.Text = AppConfig.LinkGroupsList[i].Header;
            card.Margin = new Thickness(30, 10, 30, 0);
            _stackPanel?.Children.Add(card);
            AppConfig.DrawedLinkGroup[i] = true;
        }
    }

    public void AddGroup(LinkGroup? linkGroup)
    {
        try
        {
            AppConfig.LinkGroupsList.Add(linkGroup);
            int index = AppConfig.LinkGroupsList.Count - 1;
            AppConfig.DrawedLinkGroup.Add(index, false);
            
            using (StreamReader reader = new StreamReader("data.json"))
            {
                string text = reader.ReadToEnd();
                json = JsonConvert.DeserializeObject<Json>(text);
                reader.Close();
            }

            json!.LinkGroupList.Add(linkGroup);
            
            string _json = JsonConvert.SerializeObject(json, Formatting.Indented);

            using (StreamWriter writer = new StreamWriter("data.json", false))
            {
                writer.WriteLine(_json);
                writer.Close();
            }

            DrawGroup(index);
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message, "Ошибка");
        }
    }

    public void SaveBackgroundImage()
    {
        try
        {
            using (StreamReader reader = new StreamReader("data.json"))
            {
                string text = reader.ReadToEnd();
                json = JsonConvert.DeserializeObject<Json>(text);
                reader.Close();
            }

            json!.Background = AppConfig.BackgroundPath;
        
            string _json = JsonConvert.SerializeObject(json, Formatting.Indented);

            using (StreamWriter writer = new StreamWriter("data.json", false))
            {
                writer.WriteLine(_json);
                writer.Close();
            }
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message, "Ошибка");
        }
    }

    public void SetBackground(Window window)
    {
        if (AppConfig.BackgroundPath != "None")
        {
            window.Background = new ImageBrush(new BitmapImage(new Uri(AppConfig.BackgroundPath)));
        }
        else
        {
            window.Background = Brushes.White;
        }
    }
    
    public void RemoveBackground(Window window)
    {
        MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить фоновое изображение?", "Подтверждение", MessageBoxButton.YesNo);

        if (result == MessageBoxResult.Yes)
        {
            using (StreamReader reader = new StreamReader("data.json"))
            {
                string text = reader.ReadToEnd();
                json = JsonConvert.DeserializeObject<Json>(text);
                reader.Close();
            }

            json!.Background = "None";
            AppConfig.BackgroundPath = "None";

            string _json = JsonConvert.SerializeObject(json);
            using (StreamWriter writer = new StreamWriter("data.json", false))
            {
                writer.WriteLine(_json);
                writer.Close();
            }

            SetBackground(window);
        }
    }

    public void DrawGroup(int index)
    {
        if (AppConfig.LinkGroupsList[index] is not null && AppConfig.DrawedLinkGroup.ContainsKey(index))
        {
            LinkGroupCard card = new LinkGroupCard();
            card.Header.Text = AppConfig.LinkGroupsList[index].Header;
            card.Margin = new Thickness(30, 10, 30, 0);
            _stackPanel?.Children.Add(card);
            AppConfig.DrawedLinkGroup[index] = true;
        }
    }

    public void AddLinkInJson(int groupIndex, string link)
    {
        using (StreamReader reader = new StreamReader("data.json"))
        {
            string text = reader.ReadToEnd();
            json = JsonConvert.DeserializeObject<Json>(text);
            reader.Close();
        }

        json!.LinkGroupList[groupIndex].LinksList.Add(link);
        string _json = JsonConvert.SerializeObject(json, Formatting.Indented);
                    
        using (StreamWriter writer = new StreamWriter("data.json", false))
        {
            writer.WriteLine(_json);
            writer.Close();
        }
    }

    public void ChangeLinkInJson(int groupIndex, int linkIndex, string link)
    {
        using (StreamReader reader = new StreamReader("data.json"))
        {
            string text = reader.ReadToEnd();
            json = JsonConvert.DeserializeObject<Json>(text);
            reader.Close();
        }

        json!.LinkGroupList[groupIndex].LinksList[linkIndex] = link;
        string _json = JsonConvert.SerializeObject(json, Formatting.Indented);
                    
        using (StreamWriter writer = new StreamWriter("data.json", false))
        {
            writer.WriteLine(_json);
            writer.Close();
        }
    }

    public void ChangeHeaderInJson(int groupIndex, string? header)
    {
        using (StreamReader reader = new StreamReader("data.json"))
        {
            string text = reader.ReadToEnd();
            json = JsonConvert.DeserializeObject<Json>(text);
            reader.Close();
        }

        json!.LinkGroupList[groupIndex].Header = header;
        string _json = JsonConvert.SerializeObject(json, Formatting.Indented);
                    
        using (StreamWriter writer = new StreamWriter("data.json", false))
        {
            writer.WriteLine(_json);
            writer.Close();
        }
    }

    public void RemoveLinkInJson(int groupIndex, int linkIndex)
    {
        using (StreamReader reader = new StreamReader("data.json"))
        {
            string text = reader.ReadToEnd();
            json = JsonConvert.DeserializeObject<Json>(text);
            reader.Close();
        }

        json!.LinkGroupList[groupIndex].LinksList.RemoveAt(linkIndex);
        string _json = JsonConvert.SerializeObject(json, Formatting.Indented);
                    
        using (StreamWriter writer = new StreamWriter("data.json", false))
        {
            writer.WriteLine(_json);
            writer.Close();
        }
    }

    public void RemoveGroup(string? header)
    {
        LinkGroupCard? linkGroupCard;
        for (int i = 0; i < _stackPanel?.Children.Count; i++)
        {
            linkGroupCard = _stackPanel.Children[i] as LinkGroupCard;
            if (linkGroupCard?.Header.Text == header)
            {
                _stackPanel?.Children.RemoveAt(i);
                break;
            }
        }
        
        using (StreamReader reader = new StreamReader("data.json"))
        {
            string text = reader.ReadToEnd();
            json = JsonConvert.DeserializeObject<Json>(text);
            reader.Close();
        }

        LinkGroup? linkGroup = json?.LinkGroupList.FirstOrDefault(linkGroup => linkGroup?.Header == header);
        int index = json!.LinkGroupList.IndexOf(linkGroup);
        json.LinkGroupList.RemoveAt(index);

        string _json = JsonConvert.SerializeObject(json, Formatting.Indented);
        
        using (StreamWriter writer = new StreamWriter("data.json", false))
        {
            writer.WriteLine(_json);
            writer.Close();
        }
    }

    public void LoadAllData()
    {
        try
        {
            using (StreamReader reader = new StreamReader("data.json"))
            {
                string text = reader.ReadToEnd();
                json = JsonConvert.DeserializeObject<Json>(text);

                AppConfig.BackgroundPath = json!.Background;

                for (int i = 0; i < json.LinkGroupList.Count; i++)
                {
                    AppConfig.LinkGroupsList.Add(json.LinkGroupList[i]);
                    AppConfig.DrawedLinkGroup.Add(i, false);
                }

                reader.Close();
            }
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message, "Ошибка");
        }
    }
}