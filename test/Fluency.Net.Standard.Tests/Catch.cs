using System;

namespace Fluency.Net.Standard.Tests
{
    internal static class Catch
    {
        public static Exception Exception(Action throwingAction)
        {
            try
            {
                throwingAction();
            }
            catch (Exception ex)
            {
                return ex;
            }
            return (Exception)null;
        }
    }
}