using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.IO;
using System.Threading;

class Program
{

    static void Main(string[] args)
    {
        var calendarService = new Service();
        var service = calendarService.InitializeService("Data\\OAuth.json");

        var eventCreator = new Creator(service);
        var newEvent = eventCreator.CreateEvent();

        var calendarId = "03de3fca7e4ed5844b8ac3877e1281fbc019bcf1390d2a5e7f5ad32c5f8e157d@group.calendar.google.com"; // hoặc calendar ID của bạn
        var createdEvent = eventCreator.InsertEvent(calendarId, newEvent);

        Console.WriteLine("Event created: {0}", createdEvent.HtmlLink);
    }
}
