using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RcpProcessor.Common;
using RcpProcessor.Factories;
using RcpProcessor.Interfaces;
using RcpProcessor.Models;

namespace RcpProcessor
{
    sealed class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("RCP Processor Started");
            Console.WriteLine("--------------------");

            if (args.Length == 0)
            {
                DisplayHelp();  // Show help info when no arguments provided
                return;
            }

            // Process files and display results in one flow
            ProcessAndDisplayResults(args);
        }

        // Displays usage instructions and command line help
        private static void DisplayHelp()
        {
            Console.WriteLine("RCP Processor Help:");
            Console.WriteLine("-------------------");
            Console.WriteLine("Usage:");
            Console.WriteLine("  RcpProcessor.exe [directory_path]");
            Console.WriteLine("    (Processes all *.csv files in the specified directory)");
            Console.WriteLine("  RcpProcessor.exe [file1.csv] [file2.csv] ...");
            Console.WriteLine("    (Processes the specified CSV files)");
            Console.WriteLine("\nPlease provide a directory path or one or more CSV file paths as arguments.");
        }

        // Main processing and output logic
        private static void ProcessAndDisplayResults(string[] args)
        {
            // Get files to process
            var (files, source) = GetFilesToProcess(args);  // Get list of files and description of source
            
            if (!files.Any())
            {
                Console.WriteLine("No CSV files found to process. Please check your arguments.");
                Console.WriteLine("Usage: RcpProcessor.exe [directory_path | file1.csv file2.csv ...]");
                return;
            }

            Console.WriteLine($"Processing CSV files from {source}:");
            
            // Process files and get results
            var results = files.SelectMany(file => ProcessFile(file))  // Process each file and flatten the results
                               .ToList();
            
            // Display results
            Console.WriteLine($"Total records processed: {results.Count}");
            
            if (results.Count > 0)
            {
                Console.WriteLine("\nSample of processed records:");
                results.Take(Constants.SampleRecordDisplayCount).ToList().ForEach(dzien => Console.WriteLine(dzien));  // Show sample records
            }

            Console.WriteLine("\nProcessing complete. Press any key to exit.");
            Console.ReadKey();
        }

        // Determines the list of files to process based on command line arguments
        private static (List<string> files, string source) GetFilesToProcess(string[] args)
        {
            var files = new List<string>();
            var source = string.Empty;

            // Check if the first argument is a directory
            if (Directory.Exists(args[0]))
            {
                string directoryPath = Path.GetFullPath(args[0]);  // Convert to absolute path
                files = Directory.GetFiles(directoryPath, Constants.CsvSearchPattern, SearchOption.TopDirectoryOnly).ToList();  // Get all CSV files
                source = $"the directory '{directoryPath}'";

                if (args.Length > 1)
                    Console.WriteLine("Info: The first argument was a directory. All CSV files in this directory will be processed. Subsequent arguments are ignored.");
            }
            else
            {
                // Filter valid CSV files from arguments
                files = args.Select(Path.GetFullPath)  // Convert to absolute paths
                            .Where(path => File.Exists(path) && Path.GetExtension(path).Equals(Constants.CsvFileExtension, StringComparison.OrdinalIgnoreCase))
                            .ToList();
                
                // Log warnings for invalid files
                var invalidFiles = args.Select(arg => (arg, path: Path.GetFullPath(arg)))
                                       .Where(x => !File.Exists(x.path) || !Path.GetExtension(x.path).Equals(Constants.CsvFileExtension, StringComparison.OrdinalIgnoreCase));
                
                foreach (var (arg, path) in invalidFiles)
                    Console.WriteLine($"Warning: Argument '{arg}' (resolved to '{path}') is not a valid, existing CSV file and will be skipped.");
                
                source = "the specified CSV file(s)";
            }

            return (files, source);
        }

        // Processes a single file and returns the work day records
        private static IEnumerable<DzienPracy> ProcessFile(string filePath)
        {
            var fileName = Path.GetFileName(filePath);  // Extract filename for display purposes
            Console.WriteLine($"Processing file: {fileName}");
            
            try
            {
                if (RcpReaderFactory.CreateReader(filePath) is IRcpReader reader)  // Get appropriate reader for file format
                {
                    var results = reader.ReadRcpFile(filePath).ToList();  // Read and process the file
                    Console.WriteLine($"Successfully processed {results.Count} records from {fileName}");
                    return results;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing file {fileName}: {ex.Message}");  // Log errors
            }
            
            return Enumerable.Empty<DzienPracy>();  // Return empty collection if processing failed
        }
    }
}
