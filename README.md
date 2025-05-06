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

# Good Practices

## Code Organization

- **Clean Architecture**: The project follows a layered architecture with clear separation of concerns:
  - `Interfaces`: Define contracts for system components
  - `Models`: Contain data structures
  - `Readers`: Implement file processing logic
  - `Factories`: Create appropriate objects based on context
  - `Common`: House shared constants and utilities
  - `Services`: Provide business logic functionality

## Development Standards

- **SOLID Principles**:
  - **Single Responsibility**: Each class has a single purpose (e.g., `Rcp1Reader` for RCP1 format processing)
  - **Open/Closed**: The reader system is open for extension (new formats) but closed for modification
  - **Liskov Substitution**: All readers implement the `IRcpReader` interface correctly
  - **Interface Segregation**: Interfaces are focused and minimal
  - **Dependency Inversion**: High-level modules depend on abstractions (e.g., `IRcpReader`)

## Object-Oriented Practices

- **Inheritance**: Base classes like `RcpReaderBase` provide common functionality that specific readers inherit
- **Abstraction**: Using abstract classes and interfaces (`IRcpReader`) to hide implementation details
- **Encapsulation**: Private methods and data are hidden within classes (e.g., `ProcessLine` in `Rcp1Reader`)
- **Polymorphism**: Different reader types share the same interface but implement processing differently
- **Design Patterns**:
  - **Factory Pattern**: `RcpReaderFactory` creates appropriate reader instances based on file format
  - **Template Method**: Base reader class defines the algorithm skeleton with specific steps implemented by subclasses
  - **Strategy Pattern**: Different reader strategies are selected at runtime
- **Class Responsibility**:
  - Classes have well-defined purposes (readers, models, factories)
  - Properties and methods have clear, focused responsibilities
- **Cohesion and Coupling**:
  - High cohesion: Classes contain strongly related functionality
  - Low coupling: Dependencies are minimized through interfaces and abstractions
- **Immutability**: Using immutable data objects where appropriate (e.g., `DzienPracy` model)
- **Composition over Inheritance**: Favoring object composition for flexible designs

## Error Handling

- Use meaningful exception messages that explain what went wrong
- Implement graceful error recovery where possible
- Log failures for diagnostic purposes without exposing sensitive information

## Performance Considerations

- Use buffered reads with appropriate buffer sizes for file operations
- Implement lazy evaluation with `IEnumerable<T>` to process large files efficiently
- Cache frequently used objects (e.g., reader instances in the factory)
- Use appropriate file access modes for performance (FileShare.Read)

## Testing

- Write unit tests for each component
- Follow AAA pattern (Arrange, Act, Assert)
- Test edge cases (empty files, malformed data)
- Use mock objects for dependencies

## Contributing Guidelines

- Always build and test locally before committing changes
- Follow existing naming conventions and code style
- Update documentation when changing functionality
- Add comments that explain "why" rather than "what"
- Add appropriate XML documentation for public APIs

## Git Best Practices

- Write meaningful commit messages that explain the purpose of changes
- Use feature branches for new functionality
- Keep commits focused on a single logical change
- Perform code reviews before merging to main branch

