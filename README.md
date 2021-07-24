# Raccoon
Convert Unity test results to [GitLab test reports](https://docs.gitlab.com/ee/ci/unit_test_reports.html).

## Prerequisites
1. Add the `AndtechGames` NuGet registry.
```shell
$ dotnet nuget add source --username USERNAME --password PERSONAL_ACCESS_TOKEN --store-password-in-clear-text --name github/andtechgames "https://nuget.pkg.github.com/AndtechGames/index.json"
```
2. Install `.NET Core`.

## Installation
1. Install **Raccoon** with `dotnet tool`.
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
