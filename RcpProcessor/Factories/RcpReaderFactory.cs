using System;
using System.IO;
using RcpProcessor.Interfaces;

namespace RcpProcessor.Factories
{
    // Factory class that creates appropriate reader objects based on CSV file format
    public static class RcpReaderFactory
    {
        // Cache for common reader types to avoid repeated instantiation
        private static readonly IRcpReader s_rcp1Reader = new Readers.Rcp1Reader();  // Static singleton for RCP1 format reader
        private static readonly IRcpReader s_rcp2Reader = new Readers.Rcp2Reader();  // Static singleton for RCP2 format reader

        // Public API to get the appropriate reader for a specific file path
        public static IRcpReader? CreateReader(string filePath)
        {
            return DetermineReaderFromContent(filePath);  // Delegate to helper method that examines file content
        }

        // Examines file content to determine which reader type to use
        private static IRcpReader? DetermineReaderFromContent(string filePath)
        {
            try
            {
                // Open file with read-only access and shared read permission for performance
                using var reader = new StreamReader(new FileStream(
                    filePath, FileMode.Open, FileAccess.Read, FileShare.Read, Common.Constants.FileReadBufferSize));
                
                // Return null if the file is empty
                if (reader.ReadLine() is not string line || string.IsNullOrEmpty(line))
                    return null;
                
                // Split the first line using CSV delimiter with a max count to avoid excessive splitting
                var parts = line.Split(Common.Constants.CsvSeparator, Common.Constants.MaxCsvSplitCount);
                
                // Check for rcp1 format (which has 5 columns)
                if (parts.Length >= Common.Constants.Rcp1MinColumnCount)
                    return s_rcp1Reader;  // Return the cached RCP1 reader instance
                
                // Check for rcp2 format (which has 4 columns and the last one is WE/WY)
                if (parts.Length >= Common.Constants.Rcp2MinColumnCount && IsRcp2Format(parts[Common.Constants.EventTypeIndex]))
                    return s_rcp2Reader;  // Return the cached RCP2 reader instance
            }
            catch (Exception ex)
            {
                // Log any errors but don't crash the application
                Console.WriteLine($"Error analyzing {Path.GetFileName(filePath)}: {ex.Message}");
            }
            
            // If no matching format is found, log and return null
            Console.WriteLine($"Unknown format in file {Path.GetFileName(filePath)}. Skipping.");
            return null;
        }
        
        // Helper method to check if a value matches RCP2 format event types (WE/WY)
        private static bool IsRcp2Format(string value) => 
            value.AsSpan().Trim().Equals(Common.Constants.EntryEventType, StringComparison.OrdinalIgnoreCase) ||  // Check for entry event (WE)
            value.AsSpan().Trim().Equals(Common.Constants.ExitEventType, StringComparison.OrdinalIgnoreCase);    // Check for exit event (WY)
    }
}