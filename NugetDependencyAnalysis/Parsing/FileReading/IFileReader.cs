namespace NugetDependencyAnalysis.Parsing.FileReading
{
    internal interface IFileReader
    {
        string ReadFileContents(string filePath);
    }
}
