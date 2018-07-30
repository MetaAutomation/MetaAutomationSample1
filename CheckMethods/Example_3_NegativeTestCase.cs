////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
//  MetaAutomation (C) 2018 by Matt Griscom.
//
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

namespace CheckMethods
{
    using MetaAutomationClientSpLibrary;
    using System;

    public class Example_3_NegativeTestCase
    {
        [CheckMethod(CheckMethodName = "ExampleNegativeTestCase", CheckMethodGuid = "29DB9DEC-7A3E-4723-A97E-884EE8888D13")]
        public void ExampleNegativeTestCase()
        {
            try
            {
                int divisor = 1;

                Check.Step("Set up check.", delegate
                {
                    divisor = DateTime.Now.Millisecond / 10;
                    divisor = divisor % 4;
                    divisor = (divisor > 0) ? 0 : 1;

                    Check.SetCustomDataCheckStep("divisor", divisor.ToString());
                });

                Check.Step("Measure and verify the expected failure", delegate
                {
                    bool expectedFailure = false;

                    try
                    {
                        // The following line throws a DivideByZeroException 75% of the time,
                        //  and the remaining 25% does nothing interesting.
                        int result = 1 / divisor;
                    }
                    catch (DivideByZeroException)
                    {
                        // Add code here for additional verifications as needed:
                        //  is this really the expected failure?
                        expectedFailure = true;
                    }

                    if (!expectedFailure)
                    {
                        throw new CheckFailException("The expected failure didn't happen, so this negative test fails.");
                    }
                });
            }
            catch (Exception ex)
            {
                Check.ReportFailureData(ex);
            }
        }
    }
}
