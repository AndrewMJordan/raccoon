# Raccoon
[![NuGet](https://img.shields.io/nuget/v/Andtech.Raccoon)](https://nuget.org/packages/Andtech.Raccoon)

Convert [Unity test results](https://docs.unity3d.com/Packages/com.unity.test-framework@1.1/manual/reference-command-line.html) to [GitLab test reports](https://docs.gitlab.com/ee/ci/unit_test_reports.html).

Unity generates NUnit XML test results; however, GitLab expects JUnit XML test reports. Use **Raccoon** to convert between the two formats.

## Prerequisites
* [.NET Core](https://docs.microsoft.com/en-us/dotnet/core/tools/global-tools)
* Make sure [NuGet Gallery](https://nuget.org) is registered as a source in your NuGet configuration (it is by default).

```shell
$ dotnet nuget list source
Registered Sources:
  1.  nuget.org [Enabled]
      https://api.nuget.org/v3/index.json
```

## Installation
1. Use `dotnet tool install`.
```shell
$ dotnet tool install --global Andtech.Raccoon
```

## Usage
* Invoke the tool with the `coon` command.
```shell
$ coon results.xml
<testsuites>
  <testsuite>
    <testcase name="MathTests.TestSubtraction" classname="MyTestSuite" time="0.5" />
    <testcase name="MathTests.TestAddition" classname="MyTestSuite" time="0.25">
      <failure>Expected: 19; But was: 21;</failure>
    </testcase>
    <testcase name="MathTests.Multiplication" classname="MyTestSuite" time="0.9">
      <error>System.Exception : Exception of type 'System.Exception' was thrown.</error>
    </testcase>
  </testsuite>
</testsuites>
```

Or pass a second argument to write the results to a file.

```shell
$ coon unity.xml gitlab.xml
```

## Uninstallation
1. Use `dotnet tool uninstall`.
```shell
$ dotnet tool uninstall --global Andtech.Raccoon
```
