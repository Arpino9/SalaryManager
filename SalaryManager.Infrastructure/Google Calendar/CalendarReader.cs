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

            foreach (var eventItem in events)
            {
                if (String.IsNullOrEmpty(eventItem.Start.DateTime.ToString()) ||
                    eventItem.Location is null)
                {
                    continue;
                }

                var eventEntity = new CalendarEventEntity(eventItem.Summary, eventItem.Start.DateTime.Value, eventItem.End.DateTime.Value, eventItem.Location, eventItem.Description);

                CalendarReader.CalendarEvents.Add(eventEntity);
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

            var schedules = new List<Event>();

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
                        schedules.Add(eventItem);
                    }   
                }

                // 次のページのトークンを設定
                request.PageToken = events.NextPageToken;
            } while (!String.IsNullOrEmpty(request.PageToken));

            return schedules.OrderBy(x => x.Start.DateTime).ToList().AsReadOnly();
        }

        /// <summary>
        /// イベントを取得する
        /// </summary>
        /// <param name="address">住所</param>
        /// <returns>イベント</returns>
        /// <remarks>
        /// 指定された住所と一致するイベントを抽出する。
        /// </remarks>
        public static IReadOnlyList<CalendarEventEntity> GetEvents(string address)
            => CalendarReader.CalendarEvents.Any() ?
               CalendarReader.CalendarEvents.Where(x => x.Place.Contains(address)).ToList().AsReadOnly() :
               new List<CalendarEventEntity>();

        /// <summary>
        /// イベントを取得する
        /// </summary>
        /// <param name="address">住所</param>
        /// <param name="startDate">開始日付</param>
        /// <param name="endDate">終了日付</param>
        /// <returns>イベント</returns>
        /// <remarks>
        /// 指定された開始日、終了日と一致するイベントを取得する。
        /// </remarks>
        public static IReadOnlyList<CalendarEventEntity> GetEvents(DateTime startDate, DateTime endDate)
            => CalendarReader.CalendarEvents.Any() ?
               CalendarReader.CalendarEvents.Where(x => x.StartDate >= startDate &&
                                                        x.EndDate <= endDate).ToList().AsReadOnly() :
               new List<CalendarEventEntity>();

        /// <summary>
        /// イベントを取得する
        /// </summary>
        /// <param name="address">住所</param>
        /// <param name="startDate">開始日付</param>
        /// <param name="endDate">終了日付</param>
        /// <returns>イベント</returns>
        /// <remarks>
        /// 指定された住所、開始日、終了日と一致するイベントを取得する。
        /// </remarks>
        public static IReadOnlyList<CalendarEventEntity> GetEvents(string address, DateTime startDate, DateTime endDate)
            => CalendarReader.CalendarEvents.Any()?
               CalendarReader.CalendarEvents.Where(x => x.Place.Contains(address) &&
                                                        x.StartDate >= startDate &&
                                                        x.EndDate   <= endDate).ToList().AsReadOnly() : 
               new List<CalendarEventEntity>();

        /// <summary>
        /// イベントを取得する
        /// </summary>
        /// <param name="address">住所</param>
        /// <param name="startTime">開始時刻</param>
        /// <param name="endTime">終了時刻</param>
        /// <returns>イベント</returns>
        /// <remarks>
        /// 指定された住所、開始時刻、終了時刻と一致するイベントを取得する。
        /// </remarks>
        public static IReadOnlyList<CalendarEventEntity> GetEvents(string address, TimeValue startTime, TimeValue endTime)
            => CalendarReader.CalendarEvents.Any() ?
               CalendarReader.CalendarEvents.Where(x => x.Place.Contains(address) &&
                                                        x.StartDate.Hour   >= startTime.Hour &&
                                                        x.EndDate.Hour     <= endTime.Hour &&
                                                        x.StartDate.Minute >= startTime.Minute &&
                                                        x.EndDate.Minute   <= endTime.Minute).ToList().AsReadOnly() :
               new List<CalendarEventEntity>();

        /// <summary>
        /// イベントを取得する
        /// </summary>
        /// <param name="address">住所</param>
        /// <param name="startDate">開始日付</param>
        /// <param name="endDate">終了日付</param>
        /// <param name="startTime">開始時刻</param>
        /// <param name="endTime">終了時刻</param>
        /// <returns>イベント</returns>
        /// <remarks>
        /// 指定された住所、開始日、終了日と一致するイベントを取得する。
        /// </remarks>
        public static IReadOnlyList<CalendarEventEntity> GetEvents(string address, DateTime startDate, DateTime endDate, TimeValue startTime, TimeValue endTime)
            => CalendarReader.CalendarEvents.Any() ?
               CalendarReader.CalendarEvents.Where(x => x.Place.Contains(address) &&
                                                        x.StartDate        >= startDate &&
                                                        x.EndDate          <= endDate &&
                                                        x.StartDate.Hour   >= startTime.Hour &&
                                                        x.EndDate.Hour     <= endTime.Hour &&
                                                        x.StartDate.Minute >= startTime.Minute &&
                                                        x.EndDate.Minute   <= endTime.Minute).ToList().AsReadOnly() :
               new List<CalendarEventEntity>();
    }
}