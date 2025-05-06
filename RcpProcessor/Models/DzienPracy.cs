using System;
using RcpProcessor.Common;

namespace RcpProcessor.Models
{
    // Represents an employee's work day record with entry and exit times
    public sealed class DzienPracy
    {
        public string KodPracownika { get; private set; }  // Employee identifier/code
        public DateTime Data { get; private set; }  // Date of the work day record
        public TimeSpan GodzinaWejscia { get; private set; }  // Entry time
        public TimeSpan GodzinaWyjscia { get; private set; }  // Exit time

        // Constructor that initializes all required properties
        public DzienPracy(string kodPracownika, DateTime data, TimeSpan godzinaWejscia, TimeSpan godzinaWyjscia)
        {
            KodPracownika = kodPracownika;
            Data = data;
            GodzinaWejscia = godzinaWejscia;
            GodzinaWyjscia = godzinaWyjscia;
        }
        
        // Converts the work day record to a CSV-formatted string
        public override string ToString()
        {
            return $"{KodPracownika}{Constants.CsvSeparator}{Data.ToString(Constants.DateFormat)}{Constants.CsvSeparator}{GodzinaWejscia}{Constants.CsvSeparator}{GodzinaWyjscia}";
        }
    }
}