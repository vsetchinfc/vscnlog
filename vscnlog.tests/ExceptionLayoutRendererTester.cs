using System;
using Xunit;
using NLog;

namespace vscnlog.tests
{
    public class ExceptionLayoutRendererTester
    {
        private static Logger _log = LogManager.GetCurrentClassLogger();

        [Fact]
        public void RendererFormatErrorMessage()
        {
            try
            {
                FunctionWithException("Exception!!!");
            }
            catch (System.Exception ex)
            {
                _log.Error(ex, "Error Hapend!");
                Assert.True(true);
            }
        }

        private static void FunctionWithException(string value)
        {
            try
            {
                value.Substring(100, 100);
            }
            catch (System.Exception ex)
            {
                throw new Exception("Function had exception!", ex);
            }
        }
    }
}
