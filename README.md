# Nuget Dependency Analysis

## Compares versions of Nuget dependencies of all projects in a root directory

### Purpose

The [Manage Nuget Packages for Solution](https://docs.microsoft.com/en-us/nuget/tools/package-manager-ui#managing-packages-for-the-solution) option in Visual Studio allows you to find all version differences in a solution, but what if you have multiple solutions and want to consolidate dependency versions in them? This tool finds any differences in dependency versions in all projects in a root directory.

### Assumptions

- Each `packages.config` is located in the same directory as its project.

### How it works

```
NugetDependencyAnalysis.exe <directory to analyse>
E.g. NugetDependencyAnalysis.exe "C:\repos"
```

1. Finds all `packages.config` files located in or in a subdirectory of the given directory.
2. Compares the dependencies in all the `packages.config` files and reports for dependencies that have different versions, what the different versions are and the projects for each version.

### License

[MIT License](https://raw.githubusercontent.com/matthewrwilton/NugetDependencyAnalysis/master/LICENSE)
