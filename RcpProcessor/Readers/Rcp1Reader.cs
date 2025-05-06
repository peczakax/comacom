using System;
using System.Collections.Generic;
using RcpProcessor.Common;
using RcpProcessor.Models;

namespace RcpProcessor.Readers
{
    // Implementation for parsing RCP1 format files where each line contains complete entry/exit data
    public sealed class Rcp1Reader : RcpReaderBase
    {
        // Processes the RCP1 format file and extracts work day records 
        protected override IEnumerable<DzienPracy> ProcessFile(string filePath)
        {
            var workingDays = new Dictionary<string, DzienPracy>();  // Dictionary to store unique employee-day records
            
            foreach (var parts in ReadFileLines(filePath, Constants.Rcp1MinColumnCount))
            {
                try
                {
                    ProcessLine(parts, workingDays);  // Process each valid CSV line
                }
                catch (Exception ex)
                {
                    // Log errors but continue processing remaining lines
                    Console.WriteLine($"Error processing parts: {string.Join(Constants.CsvSeparator.ToString(), parts)}. Error: {ex.Message}");
                }
            }
            
            return workingDays.Values;  // Return all work day records
        }
        
        // Processes a single line from the RCP1 file and adds it to the collection
        private void ProcessLine(string[] parts, Dictionary<string, DzienPracy> workingDays)
        {
            var employeeCode = parts[Constants.EmployeeCodeIndex];  // Get employee ID from the first column
            var date = ParseDate(parts[Constants.DateIndex]);  // Parse the date from the second column
            var entryTime = ParseTime(parts[Constants.TimeIndex]);  // Entry time is in the third column
            var exitTime = ParseTime(parts[Constants.Rcp1ExitTimeIndex]);  // Exit time is in the fourth column
            
            var key = GenerateEmployeeKey(employeeCode, date);  // Create unique key combining employee and date
            
            // Requirement 1: Each employee should have max one record per day
            if (!workingDays.ContainsKey(key))
            {
                workingDays[key] = new DzienPracy(employeeCode, date, entryTime, exitTime);  // Store the work day record
            }
        }
    }
}