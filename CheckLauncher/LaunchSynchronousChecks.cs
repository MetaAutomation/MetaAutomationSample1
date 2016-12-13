////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
//  MetaAutomation (C) 2016 by Matt Griscom.
//
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using MetaAutomationBaseSpLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace CheckLauncher
{
    class LaunchSynchronousChecks
    {
        private const string CheckMapPath = @"..\..\Artifacts";
        private const string CheckMapFile = "CheckMap.xml";
        private const string CheckElementName = "Check";
        private const string DirectoryName = "DirectoryName";
        private const string CurrentCheckRunArtifact = "CurrentCheckRunArtifact";

        static void Main(string[] args)
        {
            try
            {
                RunChecks();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.WriteLine();
                Console.WriteLine("Hit Enter to continue...");
                Console.Read();
            }

        }

        static void RunChecks()
        {
            try
            {
                XDocument checkMap = XDocument.Load(Path.Combine(CheckMapPath, CheckMapFile));

                string xpathToFindFileName = string.Format(
                    "{0}[@{1}='{2}']",
                    DataStringConstants.ElementNames.DataElement,
                    DataStringConstants.AttributeNames.Name,
                    CurrentCheckRunArtifact);

                string xpathToFindDirectoryName = string.Format(
                    "{0}[@{1}='{2}']",
                    DataStringConstants.ElementNames.DataElement,
                    DataStringConstants.AttributeNames.Name,
                    DirectoryName);


                var checkElementsIterator = checkMap.Descendants(CheckElementName);

                // launch each check in sequence
                foreach (XElement checkElement in checkElementsIterator)
                {
                    XElement fileNameElement = checkElement.XPathSelectElement(xpathToFindFileName);
                    XAttribute fileNameValueAttribute = fileNameElement.Attribute(DataStringConstants.AttributeNames.Value);
                    string fileName = fileNameValueAttribute.Value;

                    XElement directoryNameElement = checkElement.XPathSelectElement(xpathToFindDirectoryName);
                    XAttribute directoryNameValueAttribute = directoryNameElement.Attribute(DataStringConstants.AttributeNames.Value);
                    string directoryName = directoryNameValueAttribute.Value;

                    if (string.IsNullOrWhiteSpace(directoryName))
                    {
                        throw new ApplicationException("'directoryName' is missing or zero length.");
                    }

                    if (string.IsNullOrWhiteSpace(fileName))
                    {
                        throw new ApplicationException("'fileName' is missing or zero length.");
                    }

                    string pathAndFileName = Path.GetFullPath(Path.Combine(CheckMapPath, directoryName, fileName));

                    // The next line runs the check synchronously, based on the artifact from the last check run
                    string newArtifactFileName = MetaAutomationLauncherSpLibrary.CheckArtifactFiles.RunCheck(pathAndFileName);

                    // The next line updates the CheckMap XDocument with the file name that is the new artifact of the check run
                    fileNameValueAttribute.Value = newArtifactFileName;
                }

                checkMap.Save(Path.Combine(CheckMapPath, CheckMapFile));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
            finally
            {
                Console.WriteLine("finally...");
            }
        }
    }
}
