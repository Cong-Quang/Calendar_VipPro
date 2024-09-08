using Google.Apis.Calendar.v3;
using System;
public class Handle
{
    private static Handle _instance;
    private static readonly object _lock = new object();

    public static Handle Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new Handle();
                    }
                }
            }
            return _instance;
        }
    }

    private string CalendarId_LichHoc => Data.Gi().Setting.ID[0];
    private string CalendarId_LicThi => Data.Gi().Setting.ID[1];
    private string CalendarId_CaNhan => Data.Gi().Setting.ID[2];

    private readonly CalendarService _service;

    [Obsolete]
    private Handle()
    {
        var calendarService = new Service();
        _service = calendarService.InitializeService("Data\\OAuth.json");
    }
}


