using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Andtech.Raccoon
{

	public class Program
	{

		public static void Main(string[] args)
		{
			var sourcePath = args.Length >= 1 ? args[0] : null;
			var destinationPath = args.Length >= 2 ? args[1] : null;
			
			var root = new XElement("testsuites");

			var testSuites = XDocument.Load(sourcePath)
				.Descendants("test-suite")
				.Where(x => x.Attribute("type").Value == "TestFixture");
			foreach (var testSuite in testSuites)
			{
				var suiteName = testSuite.Attribute("fullname").Value;
				var testSuiteNode = new XElement("testsuite");
				root.Add(testSuiteNode);

				var testCases = testSuite
					.Descendants("test-case");
				foreach (var testCase in testCases)
				{
					var testCaseNode = new XElement("testcase");
					testCaseNode.SetAttributeValue("name", testCase.Attribute("fullname").Value);
					testCaseNode.SetAttributeValue("classname", suiteName);
					testCaseNode.SetAttributeValue("time", testCase.Attribute("duration").Value);
					testSuiteNode.Add(testCaseNode);

					var hasFailure = testCase.Elements("failure").Any();
					if (hasFailure)
					{
						var failure = testCase.Element("failure");
						var statusString = testCase.Attributes().Any(IsErrorLabel) ? "error" : "failure";

						var statusNode = new XElement(statusString);
						testCaseNode.Add(statusNode);

						statusNode.Value = failure.Element("message").Value.Trim();

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

			if (string.IsNullOrEmpty(destinationPath))
			{
				Console.WriteLine(root);
			}
			else
			{
				var directory = Path.GetDirectoryName(destinationPath);
				if (Directory.Exists(directory))
				{
					Directory.CreateDirectory(directory);
				}
				File.WriteAllText(destinationPath, root.ToString());
			}
		}
	}
}
