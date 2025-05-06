using System;
using System.Collections.Generic;
using RcpProcessor.Common;
using RcpProcessor.Models;

namespace RcpProcessor.Readers
{
    // Implementation for parsing RCP2 format files where entry and exit times appear on separate lines
    public sealed class Rcp2Reader : RcpReaderBase
    {
        // Processes the RCP2 format file and extracts work day records
        protected override IEnumerable<DzienPracy> ProcessFile(string filePath)
        {
            var entries = new Dictionary<string, Dictionary<string, TimeSpan>>();  // Dictionary to store entry/exit times by employee-day
            
            foreach (var parts in ReadFileLines(filePath, Constants.Rcp2MinColumnCount))
            {
                try
                {
                    ProcessLine(parts, entries);  // Process each valid CSV line
                }
                catch (Exception ex)
                {
                    // Log errors but continue processing remaining lines
                    Console.WriteLine($"Error processing parts: {string.Join(Constants.CsvSeparator.ToString(), parts)}. Error: {ex.Message}");
                }
            }
            
            return CreateWorkDays(entries);  // Convert the collected times into work day records
        }
        
        // Processes a single line from the RCP2 file and adds the entry or exit time to the collection
        private void ProcessLine(string[] parts, Dictionary<string, Dictionary<string, TimeSpan>> entriesAndExits)
        {
            var employeeCode = parts[Constants.EmployeeCodeIndex];  // Get employee ID from the first column
            var date = ParseDate(parts[Constants.DateIndex]);  // Parse date from the second column
            var time = ParseTime(parts[Constants.TimeIndex]);  // Parse time from the third column
            var eventType = parts[Constants.EventTypeIndex].Trim().ToUpperInvariant();  // Get event type (WE=entry, WY=exit)

            if (eventType is not Constants.EntryEventType and not Constants.ExitEventType) // Skip invalid event types
            {
                return;
            }

            var key = GenerateEmployeeKey(employeeCode, date);  // Create unique key combining employee and date

            // Initialize tracking dictionary for this employee and day if needed
            entriesAndExits.TryAdd(key, new Dictionary<string, TimeSpan>());
            entriesAndExits[key][eventType] = time;  // Store entry or exit time by event type
        }
        
        // Creates work day records from the collected entry and exit times
        private IEnumerable<DzienPracy> CreateWorkDays(Dictionary<string, Dictionary<string, TimeSpan>> entries)
        {
            foreach (var (key, times) in entries)
            {
                // Create objects only for days with both entry and exit times
                if (!times.ContainsKey(Constants.EntryEventType) || !times.ContainsKey(Constants.ExitEventType))
                    continue;
                
                var parts = key.Split(Constants.KeySeparator);  // Split the composite key into employee code and date
                var employeeCode = parts[Constants.EmployeeCodeIndex];
                var date = DateTime.ParseExact(parts[Constants.DateIndex], Constants.KeyDateFormat, System.Globalization.CultureInfo.InvariantCulture);

                yield return new DzienPracy(employeeCode, date, times[Constants.EntryEventType], times[Constants.ExitEventType]);  // Create and return the work day record
            }
        }
    }
}