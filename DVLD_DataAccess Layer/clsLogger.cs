using System;
using System.Diagnostics;

namespace DVLD_DataAccess_Layer
{
    public class clsLogger
    {
        private static string sourceName = "DVLD_App";
        static clsLogger()
        {
            if (!EventLog.SourceExists(sourceName))
            {
                EventLog.CreateEventSource(sourceName, "Application");
            }
        }
        /// <summary>
        /// Logs an exception to the Windows Event Viewer with the specified entry type.
        /// The default entry type is Error if not specified.
        /// </summary>
        /// <param name="ex">The exception to log.</param>
        /// <param name="entryType">The type of entry to log (e.g., Error, Warning, Information). Default is Error.</param>
        public static void Log(Exception ex, EventLogEntryType entryType = EventLogEntryType.Error)
        {
            try
            {
                // Check if the source exists, if not create it
                if (!EventLog.SourceExists(sourceName))
                {
                    EventLog.CreateEventSource(sourceName, "Application");
                }

                // Log the exception to the Windows Event Viewer
                EventLog.WriteEntry(sourceName, FormatErrorMessage(ex), entryType);
            }
            catch (Exception e)
            {
                // Handle any exceptions that occur while creating the event source
                Console.WriteLine($"Error in clsLogger.Log method: {e.Message}");
            }
        }
        private static string FormatErrorMessage(Exception ex)
        {

            // This is an example
            /*
                --- Exception Log ---
                Timestamp: 10/31/2024 2:45:32 PM
                Message: File not found.
                Inner Exception: File path was null.
                Stack Trace: at MyApp.Program.Main() in C:\Project\MyApp\Program.cs:line 24
                Source: MyApp
                -----------------------
             */

            string message =

                 $"--- Exception Log ---\n" +
                 $"Timestamp: {DateTime.Now}\n" +
                 $"Message: {ex.Message}\n" +
                 $"Inner Exception: {(ex.InnerException != null ? ex.InnerException.Message : "N/A")}\n" +
                 $"Stack Trace: {ex.StackTrace}\n" +
                 $"Source: {ex.Source}\n" +
                 $"-----------------------";

            return message;
        }
    }
}
