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
2. Compares all the `packages.config` files and outputs for nuget packages that have multiple versions or target frameworks referenced what projects they are referenced in.

#### Example

```
NugetDependencyAnalysis.exe C:\repos
[21:14:05 INF] Package1 has different frameworks targetted ["net46", "net45"] across different projects
[21:14:05 INF] Package1 net46 referenced in projects ["ProjectA"]
[21:14:05 INF] Package1 net45 referenced in projects ["ProjectB"]
[21:14:05 INF] Package2 has multiple versions ["1.1.603", "1.2.1"] referenced in different projects
[21:14:05 INF] Package2 1.1.603 referenced in projects ["ProjectA"]
[21:14:05 INF] Package1 1.2.1 referenced in projects ["ProjectC"]
```

### License

[MIT License](https://raw.githubusercontent.com/matthewrwilton/NugetDependencyAnalysis/master/LICENSE)
