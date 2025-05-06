using System;
using System.Collections.Generic;
using RcpProcessor.Models;

namespace RcpProcessor.Interfaces
{
    // Interface defining the contract that all RCP (employee work time) file readers must implement
    public interface IRcpReader
    {
        // Method to read work day records from a specific file path
        IEnumerable<DzienPracy> ReadRcpFile(string filePath);
    }
}