namespace NugetDependencyAnalysis.FileReading
{
    internal interface IFileReader
    {
        string ReadFileContents(string filePath);
    }
}
