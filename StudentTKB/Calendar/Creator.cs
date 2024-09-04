using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using System;
public class Creator
{
    private readonly CalendarService _service;

    public Creator(CalendarService service)
    {
        _service = service;
    }
    /// <summary>
    /// Tạo sự kiện mới với các tham số tùy chỉnh.
    /// </summary>
    /// <param name="summary">Tiêu đề của sự kiện.</param>
    /// <param name="location">Địa điểm của sự kiện.</param>
    /// <param name="description">Mô tả của sự kiện.</param>
    /// <param name="startDateTime">Thời gian bắt đầu của sự kiện (ISO 8601 format).</param>
    /// <param name="endDateTime">Thời gian kết thúc của sự kiện (ISO 8601 format).</param>
    /// <param name="timeZone">Múi giờ của sự kiện.</param>
    /// <returns>Đối tượng Event đã được tạo.</returns>
    public Event CreateEvent(string summary, string location, string description, DateTime startDateTime, DateTime endDateTime, string timeZone = "Asia/Ho_Chi_Minh")
    {
        var newEvent = new Event()
        {
            Summary = summary,
            Location = location,
            Description = description,
            Start = new EventDateTime()
            {
                DateTime = startDateTime,
                TimeZone = timeZone,
            },
            End = new EventDateTime()
            {
                DateTime = endDateTime,
                TimeZone = timeZone,
            }
        };

        return newEvent;
    }
    /// <summary>
    /// Thêm sự kiện vào lịch.
    /// </summary>
    /// <param name="calendarId">ID của lịch.</param>
    /// <param name="newEvent">Đối tượng Event muốn thêm vào lịch.</param>
    /// <returns>Đối tượng Event đã được thêm vào lịch.</returns>
    public Event InsertEvent(string calendarId, Event newEvent)
    {
        var request = _service.Events.Insert(newEvent, calendarId);
        return request.Execute();
    }
}
