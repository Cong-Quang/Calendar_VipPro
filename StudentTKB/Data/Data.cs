using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Data
{
    private static Data _instance;
    private static readonly object _lock = new object();

    public static Data Gi(string url = "Data")
    {
        if (_instance == null)
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new Data();
                    FileJson.Instance.GetSetting(url);
                }
            }
        }
        return _instance;
    }

    private Data() { }

    public Setting Setting { get; set; }
    public List<School_Schedule> Schedule { get; set; } 
    public Calendar_personal Personal { get; set; }
}
