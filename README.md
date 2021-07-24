# Raccoon
Convert Unity test results to [GitLab test reports](https://docs.gitlab.com/ee/ci/unit_test_reports.html).

## Prerequisites
* [.NET Core](https://docs.microsoft.com/en-us/dotnet/core/tools/global-tools).

## Installation
1. Use `dotnet tool install`.
```shell
$ dotnet tool install --global Andtech.Raccoon
```

## Usage
Invoke the tool with the `coon` command.
```shell
$ coon results.xml
<testsuites>
  <testsuite>
    <testcase name="MathTests.TestSubtraction" classname="MyTestSuite" duration="0.5" />
    <testcase name="MathTests.TestAddition" classname="MyTestSuite" duration="0.25">
      <failure message="Expected: 19; But was: 21;" />
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
