using System.Collections.Generic;
using FluentAssertions;
using Moq;
using NugetDependencyAnalysis.Finding;
using NugetDependencyAnalysis.Parsing;
using NugetDependencyAnalysis.Parsing.FileReading;
using Serilog;
using Xunit;

namespace NugetDependencyAnalysisTests.Parsing
{
    public class PackagesConfigParserTests
    {
        private const string ProjectName = "project";

        public class TestData
        {
            public TestData(string name, string packagesConfigContent, ProjectNugetsGrouping expectedProjectNugetsGrouping)
            {
                Name = name;
                PackagesConfigContent = packagesConfigContent;
                ExpectedProjectNugetsGrouping = expectedProjectNugetsGrouping;
            }

            public string Name { get; }

            public string PackagesConfigContent { get; }

            public ProjectNugetsGrouping ExpectedProjectNugetsGrouping { get; }

            public override string ToString()
            {
                return Name;
            }
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void Tests(TestData testData)
        {
            var packagesConfigFile = new PackagesConfigFile("path", ProjectName);

            var mockLogger = new Mock<ILogger>();
            var mockFileReader = new Mock<IFileReader>();
            mockFileReader
                .Setup(fileReader => fileReader.ReadFileContents(It.Is<string>(value => value == packagesConfigFile.Path)))
                .Returns(testData.PackagesConfigContent);

            var target = new PackagesConfigParser(mockLogger.Object, mockFileReader.Object);

            var actual = target.Parse(packagesConfigFile);

            actual.ProjectName.Should().Be(testData.ExpectedProjectNugetsGrouping.ProjectName);
            actual.Nugets.Should().ContainInOrder(testData.ExpectedProjectNugetsGrouping.Nugets);
        }

        public static IEnumerable<object[]> Data = new List<object[]>
        {
            new object[] 
            {
                new TestData(
                    "Empty Content", 
                    string.Empty,
                    new ProjectNugetsGrouping(ProjectName, new List<NugetPackage>()))
            },
            
            new object[]
            {
                new TestData(
                    "Dependencies",
                    @"<?xml version=""1.0"" encoding=""utf-8""?>
                      <packages>
                        <package id=""Castle.Core"" version=""4.1.1"" targetFramework=""net462"" />
                        <package id=""FluentAssertions"" version=""4.19.3"" targetFramework=""net462"" />
                      </packages>",
                    new ProjectNugetsGrouping(
                        ProjectName, 
                        new List<NugetPackage>
                        {
                            new NugetPackage("Castle.Core", "4.1.1", "net462"),
                            new NugetPackage("FluentAssertions", "4.19.3", "net462")
                        }
                    )
                )
            },

            new object[]
            {
                new TestData(
                    "Invalid XML",
                    @"abcdefg",
                    new ProjectNugetsGrouping(ProjectName, new List<NugetPackage>()))
            },

            new object[]
            {
                new TestData(
                    "Missing Attributes",
                    @"<?xml version=""1.0"" encoding=""utf-8""?>
                      <packages>
                        <package />
                      </packages>",
                    new ProjectNugetsGrouping(ProjectName, new List<NugetPackage>()))
            }
        };
    }
}
