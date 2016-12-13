////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
//  MetaAutomation (C) 2016 by Matt Griscom.
//
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

namespace MetaAutomationClientSpLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Xml.Linq;
    using MetaAutomationBaseSpLibrary;
    using System.Globalization;

    /// <summary>
    /// Note 1 (Atomic Check aspects reflected here: Actionable Artifact, Artifact Data, Run Data)
    /// This class holds data about the check run that applies whether or not the check passes. Every artifact from a check
    ///  includes data serialized from an instance of this class.
    /// </summary>
    internal class CheckRunData : CheckData
    {
        #region publicMethods
        public CheckRunData(XElement checkRunDataElement)
        {
            if (checkRunDataElement == null)
            {
                throw new CheckInfrastructureClientException("The parameter 'checkRunDataElement' is null.");
            }

            string elementName = checkRunDataElement.Name.ToString();

            if (elementName != DataStringConstants.ElementNames.CheckRunData)
            {
                throw new CheckInfrastructureClientException(string.Format("The initializing element has name '{0}'. Expected name='{1}'", elementName, DataStringConstants.ElementNames.CheckRunData));
            }

            base.m_BaseElementForSection = checkRunDataElement;
        }

        public void AddCheckBeginTimeStamp(XDocument cra)
        {
            base.AddOrUpdateNameValuePairDataElement(
                DataStringConstants.NameAttributeValues.CheckBeginTime,
                DateTime.Now.ToUniversalTime().ToString("o", CultureInfo.InvariantCulture));
        }

        public void AddCheckEndTimeStamp(XDocument cra)
        {
            base.AddOrUpdateNameValuePairDataElement(
                DataStringConstants.NameAttributeValues.CheckEndTime,
                DateTime.Now.ToUniversalTime().ToString("o", CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Note 3 (Atomic Check aspects reflected here: Actionable Artifact, Artifact Data)
        /// Unlike class CheckFailData, there is no need of a data hierarchy, so this class only exposes one Add method 
        ///  with value of type string. 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void Add(string name, string value)
        {
            base.AddDataElement(name, value);
        }

        #endregion // publicMethods

        #region privateMethods

        private CheckRunData()
        {
        }
        #endregion // privateMethods
    }
}
