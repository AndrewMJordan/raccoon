using CommandLine;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Andtech.Raccoon
{

	public class Program
	{

		[Verb("trigger", true)]
		internal class TriggerOptions
		{
			[Value(0, HelpText = "The file containing the test results.", Required = false)]
			public string InputFilePath { get; set; }
			[Option('o', "output", HelpText = "Write to file instead of stdout")]
			public string OutputFilePath { get; set; }
		}

		[Verb("coverage", false)]
		internal class CoverageOptions
		{
			[Value(0, HelpText = "The file containing Cobertura coverage report.", Required = false)]
			public string InputFilePath { get; set; }
			[Option("multiplier", HelpText = "Multiply the coverage value by this factor.")]
			public double Multiplier { get; set; } = 1.0;
		}

		public static void Main(string[] args) => Parser.Default.ParseArguments<TriggerOptions, CoverageOptions>(args)
			.WithParsed<TriggerOptions>(Trigger)
			.WithParsed<CoverageOptions>(Coverage);

		static void Trigger(TriggerOptions options)
		{
			XDocument document = options.InputFilePath is null ?
				XDocument.Parse(Console.In.ReadToEnd()) :
				XDocument.Load(options.InputFilePath);

			var rootElement = new XElement("testsuites");
			var testSuites = document
				.Descendants("test-suite")
				.Where(x => x.Attribute("type").Value == "TestFixture");
			foreach (var testSuite in testSuites)
			{
				var suiteName = testSuite.Attribute("fullname").Value;
				var testSuiteElement = new XElement("testsuite");
				rootElement.Add(testSuiteElement);

				var testCases = testSuite
					.Descendants("test-case");
				foreach (var testCase in testCases)
				{
					var testCaseElement = new XElement("testcase");
					testCaseElement.SetAttributeValue("name", testCase.Attribute("fullname").Value);
					testCaseElement.SetAttributeValue("classname", suiteName);
					testCaseElement.SetAttributeValue("time", testCase.Attribute("duration").Value);
					testSuiteElement.Add(testCaseElement);

					var hasFailure = testCase.Elements("failure").Any();
					if (hasFailure)
					{
						var failure = testCase.Element("failure");
						var statusString = testCase.Attributes().Any(IsErrorLabel) ? "error" : "failure";

						var statusElement = new XElement(statusString);
						testCaseElement.Add(statusElement);

						statusElement.Value = failure.Element("message").Value.Trim();

						bool IsErrorLabel(XAttribute attribute)
						{
							if (attribute.Name == "label" && attribute.Value == "Error")
							{
								return true;
							}

							return false;
						}
					}
				}
			}

			if (options.OutputFilePath is null)
			{
				Console.WriteLine(rootElement);
			}
			else
			{
				var directory = Path.GetDirectoryName(options.OutputFilePath);
				if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
				{
					Directory.CreateDirectory(directory);
				}
				File.WriteAllText(options.OutputFilePath, rootElement.ToString());
			}
		}

		static void Coverage(CoverageOptions options)
		{
			var content = File.ReadAllText(options.InputFilePath);
			var regex = Regex.Match(content, @"coverage line-rate=\""(?<linerate>.*?)\""");

			if (!regex.Success)
			{
				Environment.ExitCode = 1;
				throw new InvalidOperationException("Coverage could not be parsed!");
			}

			var coverage = double.Parse(regex.Groups["linerate"].Value);
			coverage *= options.Multiplier;
			Console.WriteLine($"Code coverage is: {coverage:.0###}");
		}
	}
}
