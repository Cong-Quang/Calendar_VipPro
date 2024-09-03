using Newtonsoft.Json;
using System.IO;

public class FileJson
{
    private static FileJson instance;
    public static FileJson gI()
    {
        if (instance == null) instance = new FileJson();
        return instance;
    }
    public void GetSetting(string url)
    {
        if (!File.Exists(url))
        {
            File.WriteAllText(url, JsonConvert.SerializeObject(new Setting
            {
                API_Key = "your_api_key calendar",
                ID = new string[] { "lịch 1", "lịch 2", "lịch 3" },
                Height = 720,
                Width = 1080
            }));
        }
        Data.Gi().Setting = JsonConvert.DeserializeObject<Setting>(File.ReadAllText(url));
    }
    public void SetSetting(string url, Setting value)
    {
        File.WriteAllText(url, JsonConvert.SerializeObject(value));
    }
}
