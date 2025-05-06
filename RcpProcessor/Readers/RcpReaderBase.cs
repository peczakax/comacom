using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using RcpProcessor.Common;
using RcpProcessor.Interfaces;
using RcpProcessor.Models;

namespace RcpProcessor.Readers
{
    public abstract class RcpReaderBase : IRcpReader
    {
        // Common interface implementation that all readers share
        public IEnumerable<DzienPracy> ReadRcpFile(string filePath)
        {
            return ProcessFile(filePath);
        }

        // Abstract method to be implemented by specific readers
        protected abstract IEnumerable<DzienPracy> ProcessFile(string filePath);

        // Common method for reading lines from a file
        protected IEnumerable<string[]> ReadFileLines(string filePath, int minPartsCount)
        {
            foreach (var line in File.ReadLines(filePath))
            {
                if (string.IsNullOrWhiteSpace(line) || !line.Contains(Constants.CsvSeparator))
                    continue;

                string[] parts;
                try
                {
                    parts = line.Split(Constants.CsvSeparator);
                    if (parts.Length < minPartsCount)
                        continue;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing line: {line}. Error: {ex.Message}");
                    continue;
                }

                yield return parts;
            }
        }
        
        // Common method to generate employee key
        protected string GenerateEmployeeKey(string employeeCode, DateTime date)
        {
            return $"{employeeCode}{Constants.KeySeparator}{date.ToString(Constants.KeyDateFormat)}";
        }
        
        // Common method to parse date
        protected DateTime ParseDate(string dateString)
        {
            return DateTime.ParseExact(dateString, Constants.DateFormat, CultureInfo.InvariantCulture);
        }

        // Common method to parse time
        protected TimeSpan ParseTime(string timeString)
        {
            return TimeSpan.Parse(timeString);
        }
    }
}