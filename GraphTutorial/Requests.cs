using System;
using System.Collections.Generic;
using System.Text;
using GraphTutorial.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using Directory = System.IO.Directory;

namespace GraphTutorial
{
    public static class Requests
    {
        public static void ListCalendarEvents()
        {
            var events = GraphHelper.GetEventsAsync().Result;

            Console.WriteLine("Events:");

            foreach (var calendarEvent in events)
            {
                Console.WriteLine();
                Console.WriteLine($"  Subject: {calendarEvent.Subject}");
                Console.WriteLine($"  Organizer: {calendarEvent.Organizer.EmailAddress.Name}");
                Console.WriteLine($"  Start: {FormatDateTimeTimeZone(calendarEvent.Start)}");
                Console.WriteLine($"  End: {FormatDateTimeTimeZone(calendarEvent.End)}");
                Console.WriteLine($"  Description: {calendarEvent.BodyPreview}");
            }
        }

        public static string CreateEvent(Event @event)
        {
            var eventId = GraphHelper.ScheduleAnEvent(@event).GetAwaiter().GetResult();

            if (!EventContains(eventId))
                throw new EventIsNonExistentException("Event was not created!");

            return eventId;
        }

        public static bool EventContains(string eventId)
        {
            var events = GraphHelper.GetEventsAsync().Result;

            foreach (var @event in events)
            {
                if (@event.Id == eventId)
                    return true;
            }

            return false;
        }

        private static string FormatDateTimeTimeZone(Microsoft.Graph.DateTimeTimeZone value)
        {
            // Get the timezone specified in the Graph value
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(value.TimeZone);
            // Parse the date/time string from Graph into a DateTime
            var dateTime = DateTime.Parse(value.DateTime);

            // Create a DateTimeOffset in the specific timezone indicated by Graph
            var dateTimeWithTZ = new DateTimeOffset(dateTime, timeZone.BaseUtcOffset)
                .ToLocalTime();

            return dateTimeWithTZ.ToString("g");
        }

        public static IConfigurationRoot LoadAppSettings()
        {
            var appConfig = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .Build();

            // Check for required settings
            if (string.IsNullOrEmpty(appConfig["appId"]) ||
                // Make sure there's at least one value in the scopes array
                string.IsNullOrEmpty(appConfig["scopes:0"]))
            {
                return null;
            }

            return appConfig;
        } // App configuration settings (app id, permissions)
    }
}
