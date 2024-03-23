using System;
using System.Collections.Generic;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using SalaryManager.Domain.Entities;
using SalaryManager.Domain.Modules.Helpers;
using SalaryManager.Infrastructure.XML;

namespace SalaryManager.Infrastructure.Google_Calendar
{
    /// <summary>
    /// Google Calendar 読込
    /// </summary>
    public sealed class CalendarReader
    {
        private static List<CalendarEventEntity> calendarEvents = new List<CalendarEventEntity>();

        public static void Read()
        {
            calendarEvents.Clear();

            var path = XMLLoader.FetchPrivateKeyPath_Calendar();
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            // Google Calendar APIの認証
            string[] scopes = { CalendarService.Scope.CalendarReadonly };
            
            GoogleCredential credential;

            using (var stream = new System.IO.FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(scopes);
            }

            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Google Calendar API Sample",
            });

            // カレンダーのイベントを取得
            Events events = service.Events.List(XMLLoader.FetchCalendarId()).Execute();
            
            Console.WriteLine("Upcoming events:");

            if (events is null || events.Items.Count == 0) 
            {
                Console.WriteLine("No upcoming events found.");
                return;
            }

            foreach (var eventItem in events.Items)
            {
                if (String.IsNullOrEmpty(eventItem.Start.DateTime.ToString()))
                {
                    continue;
                }

                var eventEntity = new CalendarEventEntity(eventItem.Summary, eventItem.Start.DateTime.Value, eventItem.End.DateTime.Value, eventItem.Location, eventItem.Description);

                calendarEvents.Add(eventEntity);

                //Console.WriteLine("{0} ({1})", eventItem.Summary, start);
            }

            Console.Read();
        }
    }
}