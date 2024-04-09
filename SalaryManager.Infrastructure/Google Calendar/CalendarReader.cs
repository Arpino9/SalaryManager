using System;
using System.Collections.Generic;
using System.Linq;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using SalaryManager.Domain.Entities;
using SalaryManager.Domain.ValueObjects;
using SalaryManager.Infrastructure.XML;

namespace SalaryManager.Infrastructure.Google_Calendar
{
    /// <summary>
    /// Google Calendar 読込
    /// </summary>
    public sealed class CalendarReader
    {
        /// <summary> Googleカレンダーのイベント </summary>
        private static List<CalendarEventEntity> CalendarEvents = new List<CalendarEventEntity>();

        
        private static List<Event> _events = new List<Event>();

        /// <summary> Repository </summary>
        //private static IWorkingPlaceRepository _repository;

        /// <summary>
        /// 読込
        /// </summary>
        public static void Read()
        {
            CalendarReader.CalendarEvents.Clear();

            var events = GetEvents(Initialize());

            //WorkingPlace.Create(_repository);
            var start = new TimeValue(9, 0);
            var end = new TimeValue(17, 30);
            var h = GetEvents("491-0852, 愛知県 一宮市, 大志2丁目2番1号, グルークプレイズ402", start, end);

            foreach (var eventItem in events)
            {
                if (String.IsNullOrEmpty(eventItem.Start.DateTime.ToString()))
                {
                    continue;
                }

                var eventEntity = new CalendarEventEntity(eventItem.Summary, eventItem.Start.DateTime.Value, eventItem.End.DateTime.Value, eventItem.Location, eventItem.Description);

                CalendarReader.CalendarEvents.Add(eventEntity);

                //Console.WriteLine("{0} ({1})", eventItem.Summary, start);
            }

            Console.Read();
        }

        /// <summary>
        /// 初期化
        /// </summary>
        /// <returns>カレンダー</returns>
        /// <remarrks>
        /// Googleカレンダーに接続するための初期設定を行う。
        /// </remarrks>
        private static CalendarService Initialize()
        {
            var path = XMLLoader.FetchPrivateKeyPath_Calendar();
            if (string.IsNullOrEmpty(path))
            {
                return null;
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

            return service;
        }

        /// <summary>
        /// イベントの取得
        /// </summary>
        /// <param name="service">カレンダー</param>
        /// <returns>全てのイベント</returns>
        /// <remarks>
        /// ページ1つにつき最大2500件までしか取得できないため、
        /// ページネーションを用いて全件取得できるまでループする。
        /// </remarks>
        private static IReadOnlyList<Event> GetEvents(CalendarService service)
        {
            if (service is null)
            {
                return new List<Event>();
            }

            var request = service.Events.List(XMLLoader.FetchCalendarId());
            request.MaxResults = 2500;

            // 最初のページ
            request.PageToken = null;

            _events.Clear();

            do
            {
                // イベントを取得
                Events events = request.Execute();

                if (events is null || !events.Items.Any())
                {
                    Console.WriteLine("No upcoming events found.");
                    return new List<Event>();
                }

                // イベントの処理
                foreach (var eventItem in events.Items)
                {
                    if (eventItem != null)
                    {
                        // イベントの詳細を処理
                        _events.Add(eventItem);
                    }   
                }

                // 次のページのトークンを設定
                request.PageToken = events.NextPageToken;
            } while (!String.IsNullOrEmpty(request.PageToken));

            return _events.OrderBy(x => x.Start.DateTime).ToList().AsReadOnly();
        }

        /// <summary>
        /// イベントを取得する
        /// </summary>
        /// <param name="address">住所</param>
        /// <returns>イベント</returns>
        public static IReadOnlyList<Event> GetEvents(string address)
        {
            if (!_events.Any())
            {
                return new List<Event>();
            }

            var events = _events.Where(x => !string.IsNullOrEmpty(x.Location)).ToList();

            return events.Where(x => x.Location.Contains(address)).ToList().AsReadOnly();
        }

        /// <summary>
        /// イベントを取得する
        /// </summary>
        /// <param name="address">住所</param>
        /// <param name="startTime">開始時刻</param>
        /// <param name="endTime">終了時刻</param>
        /// <returns>イベント</returns>
        public static IReadOnlyList<Event> GetEvents(string address, TimeValue startTime, TimeValue endTime)
        {
            if (!_events.Any())
            {
                return new List<Event>();
            }

            var events = _events.Where(x => !string.IsNullOrEmpty(x.Location)).ToList();

            return events.Where(x => x.Location.Contains(address) &&
                                     x.Start.DateTime.Value.Hour   >= startTime.Hour &&
                                     x.Start.DateTime.Value.Minute >= startTime.Minute &&
                                     x.End.DateTime.Value.Hour     <= endTime.Hour &&
                                     x.End.DateTime.Value.Minute   <= endTime.Minute).ToList().AsReadOnly();
        }
    }
}