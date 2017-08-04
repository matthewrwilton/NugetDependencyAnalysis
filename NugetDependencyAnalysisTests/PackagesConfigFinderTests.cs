using System;
using System.Collections.Generic;
using System.IO;
using FluentAssertions;
using Moq;
using NugetDependencyAnalysis;
using Serilog;
using Xunit;

namespace NugetDependencyAnalysisTests
{
    public class PackagesConfigFinderTests
    {
        public PackagesConfigFinderTests()
        {
            var mockLogger = new Mock<ILogger>();
            Target = new PackagesConfigFinder(mockLogger.Object);
        }

        private string TestDirectoriesLocation => Path.Combine(Environment.CurrentDirectory, "_TestData");

        private PackagesConfigFinder Target { get; }

        [Fact]
        public void Finds_PackagesConfig_In_Directory()
        {
            var testDirectory = Path.Combine(TestDirectoriesLocation, "SampleSolution");

            var actual = Target.Find(testDirectory);

            var expected = new List<PackagesConfigFile> {
                new PackagesConfigFile(Path.Combine(TestDirectoriesLocation, @"SampleSolution\SampleProject\packages.config"), "SampleProject")
            };

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Handles_Missing_Project_Files()
        {
            var testDirectory = Path.Combine(TestDirectoriesLocation, "NoProjectFile");

            var actual = Target.Find(testDirectory);

            actual.Should().BeEmpty();
        }

        [Fact]
        public void Handles_Multiple_Project_Files()
        {
            var testDirectory = Path.Combine(TestDirectoriesLocation, "MultipleProjectFiles");

            var actual = Target.Find(testDirectory);

            actual.Should().BeEmpty();
        }

        [Fact]
        public void Handles_Non_Existent_Directory()
        {
            var actual = Target.Find(@"C:\Not_A_Real_Directory");

            actual.Should().BeEmpty();
        }
    }
}
