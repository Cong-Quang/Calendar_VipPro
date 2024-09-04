using Newtonsoft.Json;
using System.IO;

public class FileJson
{
    private static FileJson _instance;
    private static readonly object _lock = new object();

    public static FileJson Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new FileJson();
                    }
                }
            }
            return _instance;
        }
    }

    private FileJson() { }

    public string Url { get; private set; }

    public void GetSetting(string url)
    {
        var settingPath = $"{url}\\setting.json";
        if (!File.Exists(settingPath))
        {
            var setting = new Setting
            {
                URL_OAuth = $"{Url}OAuth.json",
                URL_Schedule = $"{Url}Schedule.json",
                URL_Personal = $"{Url}personal.json",
                ID = new[] { "lịch 1", "lịch 2", "lịch 3" },
                Height = 720,
                Width = 1080,
                userName = "tài khoản",
                password = "Mật Khẩu"
            };
            File.WriteAllText(settingPath, JsonConvert.SerializeObject(setting));
        }
        Data.Gi().Setting = JsonConvert.DeserializeObject<Setting>(File.ReadAllText(settingPath));
    }

    public void SetSetting(string url, Setting value)
    {
        File.WriteAllText(url, JsonConvert.SerializeObject(value));
    }
}
