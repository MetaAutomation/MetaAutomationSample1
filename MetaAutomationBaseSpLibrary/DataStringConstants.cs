////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
//  MetaAutomation (C) 2016 by Matt Griscom.
//
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

namespace MetaAutomationBaseSpLibrary
{
    /// <summary>
    /// This class contains all of the strings used for the XML data.
    /// </summary>
    public static class DataStringConstants
    {
        public static class ElementNames
        {
            // base elements to define a check run launch (CRL) or check run artifact (CRA)
            public const string CheckRunLaunch = "CheckRunLaunch";
            public const string CheckRunArtifact = "CheckRunArtifact";

            // required at least as empty elements, in this document order
            public const string CheckRunData = "CheckRunData";
            public const string CheckCustomData = "CheckCustomData";
            public const string CheckFailData = "CheckFailData";
            public const string CompleteCheckStepInfo = "CompleteCheckStepInfo";

            // other elements
            public const string DataElement = "DataElement";
            public const string CheckStepInformation = "CheckStep"; 
        }

        public static class AttributeNames
        {
            public const string Name = "Name";
            public const string Value = "Value";
            public const string TimeElapsed = "msTimeElapsed";
            public const string TimeLimit = "msTimeLimit";
        }

        public static class NameAttributeValues
        {
            public const string CheckJobSpecGuid = "CheckJobSpecGuid";
            public const string CheckJobRunGuid = "CheckJobRunGuid";
            public const string CheckRunGuid = "CheckRunGuid";
            public const string CheckMethodName = "CheckMethodName";

            public const string CheckMethodGuid = "CheckMethodGuid";

            public const string CheckBeginTime = "CheckBeginTime";
            public const string CheckEndTime = "CheckEndTime";
        }

        public static class StatusString
        {
            public static string DefaultServiceSuccessMessage = "Success.";
        }

        public static class NumericConstants
        {
            public static uint DefaultTimeoutMilliseconds = 30000; // 30 seconds
        }
    }
}
