////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
//  MetaAutomation (C) 2016 by Matt Griscom.
//
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

namespace MetaAutomationClientSp
{
    using MetaAutomationBaseSpLibrary;
    using MetaAutomationClientSpLibrary;
    using System;
    using System.IO;
    using System.Reflection;
    using System.Threading;
    using System.Xml.Linq;
    using System.Xml.XPath;

    /// <summary>
    /// Manages checks from CheckRunLaunch (CRL) to CheckRunArtifact (CRA)
    /// </summary>
    public class CheckRunner
    {
        /// <summary>
        /// Runs a check to completion, or throws an exception
        /// </summary>
        /// <param name="checkRunLaunch">The check run launch (CRL) object</param>
        /// <returns>The check run artifact (CRA)</returns>
        public XDocument Run(XDocument checkRunLaunch)
        {
            string targetCheckMethodGuid = DataAccessors.GetCheckRunValue(checkRunLaunch, DataStringConstants.NameAttributeValues.CheckMethodGuid);

            Type targetType = null;
            MethodInfo targetMethod = null;
            string methodNameGivenInAttribute = string.Empty;

            string checkAssemblyName = "CheckMethods.dll";
            Assembly checkAssembly = Assembly.LoadFile(Path.Combine(Environment.CurrentDirectory, checkAssemblyName));

            this.GetMethodAndType(targetCheckMethodGuid, checkAssembly.GetTypes(), out targetMethod, out targetType, out methodNameGivenInAttribute);

            // Create instance
            object testObject = Activator.CreateInstance(targetType);

            // Initialize MetaAutomation client lib
            CheckArtifact checkArtifact = Check.CheckArtifactInstance;
            checkArtifact.InitializeCheckRunFromCheckRunLaunch(checkRunLaunch);

            string methodStepName = string.Format("Method {0}", methodNameGivenInAttribute);
            try
            {
                checkArtifact.DoStep(methodStepName, delegate
                {
                    // Run the test method synchronously
                    targetMethod.Invoke(testObject, null);
                });
            }
            catch (Exception ex)
            {
                checkArtifact.AddCheckExceptionInformation(ex);
            }


            XDocument craXdoc = checkArtifact.CompleteCheckRun();

            return craXdoc;
        }

        private void GetMethodAndType(string targetCheckMethodGuid, Type[] typesInAssembly, out MethodInfo methodInfoOut, out Type targetTypeOut, out string methodNameFromAtribute)
        {
            methodInfoOut = null;
            targetTypeOut = null;
            methodNameFromAtribute = null;

            foreach (Type type in typesInAssembly)
            {
                MethodInfo[] methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);

                foreach (MethodInfo method in methods)
                {
                    Attribute attribute = method.GetCustomAttribute(typeof(CheckMethodAttribute));

                    if (attribute != null)
                    {
                        if (attribute is CheckMethodAttribute)
                        {
                            CheckMethodAttribute checkMethodAttribute = (CheckMethodAttribute)attribute;
                            string checkMethodGuid = checkMethodAttribute.CheckMethodGuid;

                            if (targetCheckMethodGuid == checkMethodGuid)
                            {
                                methodInfoOut = method;
                                targetTypeOut = type;
                                methodNameFromAtribute = checkMethodAttribute.CheckMethodName;
                                break;
                            }
                        }
                    }
                }

                if (methodInfoOut != null)
                {
                    break;
                }
            }
        }
    }
}
