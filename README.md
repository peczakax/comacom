# Legal Notice

This work constitutes an original work within the meaning of Copyright and Related Rights Act and may not be used for purposes other than evaluating the author's potential within the recruitment process in which they are participating. Commercial use without the author's consent is prohibited.

The author has no control over the use of their work by others and accepts no responsibility for any legal action taken against them in the event of any such infringement.

The author has the right to modify or delete this work at any time without notice.

# User Manual - RCP Processor

## Overview
RCP Processor is a command-line application that processes employee work time records (RCP data) from CSV files. The application supports multiple CSV formats and provides a summary of the processed records.

## System Requirements
- Windows operating system
- .NET 9.0 runtime

## Installation
1. Download the application files
2. Ensure you have .NET 9.0 runtime installed
3. No additional installation is required - the application is portable

## Usage

### Basic Operation
You can run the application using one of the following methods:

#### Process all CSV files in a directory:
```
RcpProcessor.exe [directory_path]
```

#### Process specific CSV files:
```
RcpProcessor.exe [file1.csv] [file2.csv] ...
```

### Example Commands
```
RcpProcessor.exe C:\Data\RcpFiles
RcpProcessor.exe C:\Data\rcp1.csv C:\Data\rcp2.CSV
```

### Supported File Formats
The application automatically detects and processes two CSV file formats:

1. **RCP1 Format**: CSV files with at least 5 columns
2. **RCP2 Format**: CSV files with at least 4 columns where the last column contains "WE" or "WY" entry/exit indicators

### Output
The application provides:
- Count of total records processed
- Sample display of the first 10 records processed (in the format: EmployeeCode;Date;EntryTime;ExitTime)
- Processing status for each file

## Troubleshooting

### Common Issues
- **No files processed**: Ensure you've provided valid file paths or a directory containing CSV files
- **File format error**: Check that your CSV files follow one of the supported formats
- **File access error**: Verify you have read permissions for the specified files

### Error Messages
- If a file cannot be processed, an error message will be displayed indicating the specific issue

## Support
For technical support or feature requests, please contact the development team.

