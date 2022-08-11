using System.IO;
using System.Linq;
using System.Windows;
using Newtonsoft.Json;
using MessageBox = System.Windows.MessageBox;

namespace LinksList;

public partial class LinkGroupCard
{

    public LinkGroupCard()
    {
        InitializeComponent();
    }

    private void SeeGroupButtonClick(object sender, RoutedEventArgs e)
    {
        Json? json;
        using (StreamReader reader = new StreamReader("data.json"))
        {
            string text = reader.ReadToEnd();
            json = JsonConvert.DeserializeObject<Json>(text);
            reader.Close();
        }
        
        LinkGroup? linkGroup = json!.LinkGroupList.FirstOrDefault(linkGroup => linkGroup?.Header == Header.Text);
        
        AboutGroupWindow aboutGroupWindow = new AboutGroupWindow() {linkGroup = linkGroup!};
        aboutGroupWindow.Show();
    }

    private void ChangeGroupButtonClick(object sender, RoutedEventArgs e)
    {
        Json? json;
        using (StreamReader reader = new StreamReader("data.json"))
        {
            string text = reader.ReadToEnd();
            json = JsonConvert.DeserializeObject<Json>(text);
            reader.Close();
        }
        
        LinkGroup? linkGroup = json!.LinkGroupList.FirstOrDefault(linkGroup => linkGroup?.Header == Header.Text);
        EditGroupWindow editGroupWindow = new EditGroupWindow() { linkGroup = linkGroup! };
        editGroupWindow.Show();
    }
    
    private void RemoveGroupButtonClick(object sender, RoutedEventArgs e)
    {
        MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить данную группу ссылок?", "Подтверждение", MessageBoxButton.YesNo);

        if (result == MessageBoxResult.Yes)
        {
            LinkGroup? linkGroup = AppConfig.LinkGroupsList.FirstOrDefault(linkGroup => linkGroup?.Header == Header.Text);
            int index = AppConfig.LinkGroupsList.IndexOf(linkGroup);

            AppConfig.LinkGroupsList.RemoveAt(index);
            AppConfig.DrawedLinkGroup.Remove(index);
            AppConfig.appSystem?.RemoveGroup(linkGroup?.Header);
        }
    }
}