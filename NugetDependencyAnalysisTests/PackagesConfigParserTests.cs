using System.Collections.Generic;
using FluentAssertions;
using Moq;
using NugetDependencyAnalysis;
using NugetDependencyAnalysis.FileReading;
using Serilog;
using Xunit;

namespace NugetDependencyAnalysisTests
{
    public class PackagesConfigParserTests
    {
        public class TestData
        {
            public TestData(string name, string packagesConfigContent, IReadOnlyList<NugetPackage> expectedPackages)
            {
                Name = name;
                PackagesConfigContent = packagesConfigContent;
                ExpectedPackages = expectedPackages;
            }

            public string Name { get; }

            public string PackagesConfigContent { get; }

            public IReadOnlyList<NugetPackage> ExpectedPackages { get; }

            public override string ToString()
            {
                return Name;
            }
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void Tests(TestData testData)
        {
            var packagesConfigFile = new PackagesConfig("path", "project");

            var mockLogger = new Mock<ILogger>();
            var mockFileReader = new Mock<IFileReader>();
            mockFileReader
                .Setup(fileReader => fileReader.ReadFileContents(It.Is<string>(value => value == packagesConfigFile.Path)))
                .Returns(testData.PackagesConfigContent);

            var target = new PackagesConfigParser(mockLogger.Object, mockFileReader.Object);

            var actual = target.Parse(packagesConfigFile);

            actual.Should().ContainInOrder(testData.ExpectedPackages);
        }

        public static IEnumerable<object[]> Data = new List<object[]>
        {
            new object[] 
            {
                new TestData(
                    "Empty Content", 
                    string.Empty, 
                    new List<NugetPackage>())
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
                    new List<NugetPackage>
                    {
                        new NugetPackage("Castle.Core", "4.1.1", "net462"),
                        new NugetPackage("FluentAssertions", "4.19.3", "net462")
                    })
            },

            new object[]
            {
                new TestData(
                    "Invalid XML",
                    @"abcdefg",
                    new List<NugetPackage>())
            },

            new object[]
            {
                new TestData(
                    "Missing Attributes",
                    @"<?xml version=""1.0"" encoding=""utf-8""?>
                      <packages>
                        <package />
                      </packages>",
                    new List<NugetPackage>())
            }
        };
    }
}
