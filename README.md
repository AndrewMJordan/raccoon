## Setup

### Prerequisites
* [.NET Core](https://docs.microsoft.com/en-us/dotnet/core/tools/global-tools)
* Make sure [NuGet Gallery](https://nuget.org) is registered as a source in your NuGet configuration (it is by default).

```shell
$ dotnet nuget list source
Registered Sources:
  1.  nuget.org [Enabled]
      https://api.nuget.org/v3/index.json
```

### Installation
1. Use `dotnet tool install`.
```shell
$ dotnet tool install --global Andtech.Raccoon
```

### Uninstallation
1. Use `dotnet tool uninstall`.
```shell
$ dotnet tool uninstall --global Andtech.Raccoon
```

## Usage
```
raccoon <file> [-o|--output <path>]
```

| Parameter | Description | Remarks |
| --- | --- | --- |
| `file` | The XML file containing the test results. | If omitted, XML will be read from `stdin`. |
| `-o`<br>`--output` | The file to write to. | If omitted, the converted XML will be printed to `stdout`. |

## Examples
Suppose `Unity.exe` is in `PATH`. Run the following to create test results at `results.xml`:

```
$ Unity.exe -projectPath MyProject -runTests -testResults results.xml
```

Now use `raccoon` to convert the XML:

```
$ raccoon results.xml
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

Or `cat` XML test results to `raccoon`.

```
$ cat results.xml | raccoon
<testsuites>
  <testsuite>
    <testcase name="MathTests.TestSubtraction" classname="MyTestSuite" time="0.5" />
  ...
</testsuites>
```

`raccoon` can also write directly to a file.

```
$ cat results.xml | raccoon --output results-gitlab.xml
```
