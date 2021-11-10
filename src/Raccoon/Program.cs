using CommandLine;
using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Andtech.Raccoon
{

	public class Program
	{

		internal class Options
		{
			[Value(0, HelpText = "The file containing the test results.", Required = false)]
			public string InputFilePath { get; set; }
			[Option('o', "output", HelpText = "Write to file instead of stdout")]
			public string OutputFilePath { get; set; }
		}

		public static void Main(string[] args) => Parser.Default.ParseArguments<Options>(args).WithParsed(OnParse);

		static void OnParse(Options options)
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
				if (Directory.Exists(directory))
				{
					Directory.CreateDirectory(directory);
				}
				File.WriteAllText(options.OutputFilePath, rootElement.ToString());
			}
		}
	}
}
