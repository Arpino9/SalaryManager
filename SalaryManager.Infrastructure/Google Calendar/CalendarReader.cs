using System;
using System.Collections.Generic;
using System.Linq;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using SalaryManager.Domain.Entities;
using SalaryManager.Domain.Exceptions;
using SalaryManager.Domain.ValueObjects;
using SalaryManager.Infrastructure.XML;

namespace SalaryManager.Infrastructure.Google_Calendar
{
    /// <summary>
    /// Google Calendar 読込
    /// </summary>
    /// <remarks>
    /// やや読込に時間がかかるため、起動時に非同期でロードしておくこと。
    /// </remarks>
    public static class CalendarReader
    {
        /// <summary> Googleカレンダーのイベント </summary>
        private static List<CalendarEventEntity> CalendarEvents = new List<CalendarEventEntity>();

        /// <summary> 取得中フラグ </summary>
        public static bool Loading;

        /// <summary>
        /// 読込
        /// </summary>
        public static void Read()
        {
            Loading = true;
            CalendarEvents.Clear();

            var events = GetEvents(Initialize());

            foreach (var eventItem in events)
            {
                if (String.IsNullOrEmpty(eventItem.Start.DateTime.ToString()) ||
                    eventItem.Location is null)
                {
                    var eventEntity2 = new CalendarEventEntity(eventItem.Summary, Convert.ToDateTime(eventItem.Start.Date), Convert.ToDateTime(eventItem.End.Date));
                    CalendarEvents.Add(eventEntity2);

                    continue;
                }

                var eventEntity = new CalendarEventEntity(eventItem.Summary, eventItem.Start.DateTime.Value, eventItem.End.DateTime.Value, eventItem.Location, eventItem.Description);

                CalendarEvents.Add(eventEntity);
            }
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

                if (events is null || events.Items.Any() == false)
                {
                    throw new DatabaseException("スケジュールの取得に失敗しました。");
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

            // 取得完了
            Loading = false;

            return schedules.OrderBy(x => x.Start.DateTime).ToList().AsReadOnly();
        }

        /// <summary> 空か </summary>
        /// <returns>リストが空であるか</returns>
        public static bool IsEmpty() => (CalendarEvents.Any() == false);

        /// <summary>
        /// イベントを取得する
        /// </summary>
        /// <param name="title">タイトル</param>
        /// <param name="startDate">開始日付</param>
        /// <returns>イベント</returns>
        /// <remarks>
        /// 指定されたタイトル、開始日と一致するイベントを取得する。
        /// </remarks>
        public static IReadOnlyList<CalendarEventEntity> FindByTitle(string title, DateTime startDate)
            => CalendarEvents.Any() ?
               CalendarEvents.Where(x => x.Title != null &&
                                         x.Title.Contains(title) &&
                                         x.StartDate == startDate).ToList().AsReadOnly() :
               new List<CalendarEventEntity>();

        /// <summary>
        /// イベントを取得する
        /// </summary>
        /// <param name="address">住所</param>
        /// <returns>イベント</returns>
        /// <remarks>
        /// 指定された住所と一致するイベントを抽出する。
        /// </remarks>
        public static IReadOnlyList<CalendarEventEntity> FindByAddress(string address)
            => CalendarEvents.Any() ?
               CalendarEvents.Where(x => x.Place != null &&
                                         x.Place.Contains(address)).ToList().AsReadOnly() :
               new List<CalendarEventEntity>();

        /// <summary>
        /// イベントを取得する
        /// </summary>
        /// <param name="startDate">開始日付</param>
        /// <param name="endDate">終了日付</param>
        /// <returns>イベント</returns>
        /// <remarks>
        /// 指定された開始日、終了日と一致するイベントを取得する。
        /// </remarks>
        public static IReadOnlyList<CalendarEventEntity> FindByDate(DateTime startDate, DateTime endDate)
            => CalendarEvents.Any() ?
               CalendarEvents.Where(x => x.StartDate >= startDate &&
                                         x.EndDate   <= endDate).ToList().AsReadOnly() :
               new List<CalendarEventEntity>();

        /// <summary>
        /// イベントを取得する
        /// </summary>
        /// <param name="startDate">開始日付</param>
        /// <param name="endDate">終了日付</param>
        /// <returns>イベント</returns>
        /// <remarks>
        /// 指定された開始日、終了日と一致するイベントを取得する。
        /// </remarks>
        public static IReadOnlyList<CalendarEventEntity> FindByDate(DateTime startDate, DateTime endDate, TimeSpan startTime)
            => CalendarEvents.Any() ?
               CalendarEvents.Where(x => x.StartDate >= startDate &&
                                         x.EndDate   <= endDate &&
                                         new TimeSpan(x.StartDate.Hour, x.StartDate.Minute, 0) >= startTime)
                             .ToList().AsReadOnly() :
               new List<CalendarEventEntity>();

        /// <summary>
        /// イベントを取得する
        /// </summary>
        /// <param name="address">住所</param>
        /// <param name="startDate">開始日付</param>
        /// <returns>イベント</returns>
        /// <remarks>
        /// 指定された住所、開始日と一致するイベントを取得する。
        /// </remarks>
        public static IReadOnlyList<CalendarEventEntity> FindByAddress(string address, DateTime startDate)
            => CalendarEvents.Any() ?
               CalendarEvents.Where(x => x.Place != null && 
                                         x.Place.Contains(address) &&
                                         x.StartDate == startDate).ToList().AsReadOnly() :
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
        public static IReadOnlyList<CalendarEventEntity> FindByAddress(string address, DateTime startDate, DateTime endDate)
            => CalendarEvents.Any()?
               CalendarEvents.Where(x => x.Place != null && 
                                         x.Place.Contains(address) &&
                                         x.StartDate >= startDate &&
                                         x.EndDate   <= endDate).ToList().AsReadOnly() : 
               new List<CalendarEventEntity>();

        /// <summary>
        /// イベントを取得する
        /// </summary>
        /// <param name="title">タイトル</param>
        /// <param name="startDate">開始日付</param>
        /// <param name="endDate">終了日付</param>
        /// <returns>イベント</returns>
        /// <remarks>
        /// 指定されたタイトル、開始日、終了日と一致するイベントを取得する。
        /// </remarks>
        public static IReadOnlyList<CalendarEventEntity> FindByTitle(string title, DateTime startDate, DateTime endDate)
            => CalendarEvents.Any() ?
               CalendarEvents.Where(x => x.Title != null &&
                                         x.Title.Contains(title) &&
                                         x.StartDate.Year  >= startDate.Year &&
                                         x.StartDate.Month >= startDate.Month &&
                                         x.StartDate.Day   >= startDate.Day &&
                                         x.EndDate.Year    <= endDate.Year &&
                                         x.EndDate.Month   <= endDate.Month &&
                                         x.EndDate.Day     <= endDate.Day ).ToList().AsReadOnly() :
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
        public static IReadOnlyList<CalendarEventEntity> FindByAddress(string address, TimeSpan startTime, TimeSpan endTime)
            => CalendarEvents.Any() ?
               CalendarEvents.Where(x => x.Place != null && 
                                         x.Place.Contains(address) &&
                                         x.StartDate.Hour   >= startTime.Hours &&
                                         x.StartDate.Minute >= startTime.Minutes &&
                                         x.EndDate.Hour     >= endTime.Hours &&
                                         x.EndDate.Minute   >= endTime.Minutes).ToList().AsReadOnly() :
               new List<CalendarEventEntity>();

        /// <summary>
        /// イベントを取得する
        /// </summary>
        /// <param name="address">住所</param>
        /// <param name="startDate">開始日付</param>
        /// <param name="endDate">終了日付</param>
        /// <param name="startTime">開始時刻</param>
        /// <returns>イベント</returns>
        /// <remarks>
        /// 指定された住所、開始日時、終了日と一致するイベントを取得する。
        /// </remarks>
        public static IReadOnlyList<CalendarEventEntity> FindByAddress(string address, DateTime startDate, DateTime endDate, TimeSpan startTime)
            => CalendarEvents.Any() ?
               CalendarEvents.Where(x => x.Place != null && 
                                         x.Place.Contains(address) &&
                                         x.StartDate.Year  >= startDate.Year &&
                                         x.StartDate.Month >= startDate.Month &&
                                         x.StartDate.Day   >= startDate.Day &&
                                         new TimeSpan(x.StartDate.Hour, x.StartDate.Minute, 0) >= new TimeSpan(startTime.Hours, startTime.Minutes, 0) &&
                                         x.EndDate.Year   <= endDate.Year &&
                                         x.EndDate.Month  <= endDate.Month &&
                                         x.EndDate.Day    <= endDate.Day).ToList().AsReadOnly() :
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
        /// 指定された住所、開始日時、終了日時と一致するイベントを取得する。
        /// </remarks>
        public static IReadOnlyList<CalendarEventEntity> FindByAddress(string address, DateTime startDate, DateTime endDate, TimeSpan startTime, TimeSpan endTime)
            => CalendarEvents.Any() ?
               CalendarEvents.Where(x => x.Place != null &&
                                         x.Place.Contains(address) &&
                                         x.StartDate.Year   >= startDate.Year &&
                                         x.StartDate.Month  >= startDate.Month &&
                                         x.StartDate.Day    >= startDate.Day &&
                                         x.StartDate.Hour   >= startTime.Hours &&
                                         x.StartDate.Minute >= startTime.Minutes &&
                                         x.EndDate.Year     <= endDate.Year &&
                                         x.EndDate.Month    <= endDate.Month &&
                                         x.EndDate.Day      <= endDate.Day &&
                                         x.EndDate.Hour     <= endTime.Hours &&
                                         x.EndDate.Minute   <= endTime.Minutes).ToList().AsReadOnly() :
               new List<CalendarEventEntity>();
    }
}