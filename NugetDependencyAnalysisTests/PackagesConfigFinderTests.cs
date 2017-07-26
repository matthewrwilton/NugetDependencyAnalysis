using System.Collections.Generic;
using FluentAssertions;
using NugetDependencyAnalysis;
using Xunit;

namespace NugetDependencyAnalysisTests
{
    public class PackagesConfigFinderTests
    {
        [Fact]
        public void Finds_PackagesConfig_In_Directory()
        {
            // TODO: replace with function to find directory.
            var testDirectory = @"C:\repos\github\matthewrwilton\NugetDependencyAnalysis\NugetDependencyAnalysisTests\_TestData";

            var target = new PackagesConfigFinder();

            var actual = target.Find(testDirectory);

            var expected = new List<PackagesConfig> {
                new PackagesConfig(@"C:\repos\github\matthewrwilton\NugetDependencyAnalysis\NugetDependencyAnalysisTests\_TestData\SampleSolution\SampleProject\packages.config", "SampleProject")
            };

            actual.Should().BeEquivalentTo(expected);
        }
    }
}
