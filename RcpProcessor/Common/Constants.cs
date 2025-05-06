using System;

namespace RcpProcessor.Common
{
    /// <summary>
    /// Centralized constants storage for RcpProcessor
    /// </summary>
    public static class Constants
    {
        // Buffer size for file operations
        public const int FileReadBufferSize = 4096;
        
        // CSV format constants
        public const char CsvSeparator = ';';
        public const int MaxCsvSplitCount = 6;
        
        // File format column counts
        public const int Rcp1MinColumnCount = 5;
        public const int Rcp2MinColumnCount = 4;
        
        // CSV column indices (0-based)
        public const int EmployeeCodeIndex = 0;
        public const int DateIndex = 1;
        public const int TimeIndex = 2;
        public const int Rcp1ExitTimeIndex = 3;
        public const int EventTypeIndex = 3;  // Same index as exit time in Rcp1 format
        
        // Event type values
        public const string EntryEventType = "WE";
        public const string ExitEventType = "WY";
        
        // Date and time formats
        public const string DateFormat = "yyyy-MM-dd";
        public const string KeyDateFormat = "yyyyMMdd";
        
        // Key separator for employee daily records
        public const char KeySeparator = '_';
        
        // File operations constants
        public const string CsvFileExtension = ".csv";
        public const string CsvSearchPattern = "*.csv";
        
        // Display limits
        public const int SampleRecordDisplayCount = 10;
    }
}