

namespace CheckMethods
{
    using MetaAutomationClientSpLibrary;

    public class AtomicChecks
    {
        [CheckMethod(CheckMethodName = "AtomicCheck_Test", CheckMethodGuid = "8821B991-D2D6-4FEE-8828-0C0A33F8BC26")]
        public void AtomicCheck_Test()
        {
            try
            {
                Check.Step("Stepped sleeping.", delegate
                {
                    Check.Step("foo.", delegate
                    {
                        Check.Step("First sleep.", delegate
                        {
                            System.Threading.Thread.Sleep(200);
                        });
                        Check.Step("Second sleep.", delegate
                        {
                            System.Threading.Thread.Sleep(200);
                        });
                    });
                    Check.Step("bar.", delegate
                    {
                        Check.Step("Third sleep.", delegate
                        {
                            System.Threading.Thread.Sleep(200);
                        });
                        Check.Step("Fourth sleep.", delegate
                        {
                            System.Threading.Thread.Sleep(200);
                        });
                        Check.Step("Fifth sleep.", delegate
                        {
                            System.Threading.Thread.Sleep(200);
                        });
                    });
                });
            }
            catch (System.Exception ex)
            {
                Check.ReportFailureData(ex);
            }
        }
    }
}
