using Google.Apis.Calendar.v3.Data;
using Google.Apis.Calendar.v3;
using System;
public class Creator
{
    private readonly CalendarService _service;

    public Creator(CalendarService service)
    {
        _service = service;
    }

    public Event CreateEvent()
    {
        var newEvent = new Event()
        {
            Summary = "Cuộc họp dự án",
            Location = "Trực tuyến",
            Description = "Cuộc họp với đội phát triển",
            Start = new EventDateTime()
            {
                DateTime = DateTime.Parse("2024-09-05T10:00:00+07:00"),
                TimeZone = "Asia/Ho_Chi_Minh",
            },
            End = new EventDateTime()
            {
                DateTime = DateTime.Parse("2024-09-05T11:00:00+07:00"),
                TimeZone = "Asia/Ho_Chi_Minh",
            }
        };

        return newEvent;
    }

    public Event InsertEvent(string calendarId, Event newEvent)
    {
        var request = _service.Events.Insert(newEvent, calendarId);
        return request.Execute();
    }
}
