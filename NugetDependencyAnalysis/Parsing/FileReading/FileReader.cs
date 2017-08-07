using System.IO;

namespace NugetDependencyAnalysis.Parsing.FileReading
{
    class FileReader : IFileReader
    {
        public string ReadFileContents(string filePath)
        {
            return File.ReadAllText(filePath);
        }
    }
}
