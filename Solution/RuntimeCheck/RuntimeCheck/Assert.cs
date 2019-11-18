using System;


namespace RuntimeCheck
{
    /// <summary>
    /// Assertions are used to specify conditions that must be met at a location in the program. This is a powerful tool to catch errors early.
    /// Furthermore they serve as documentation for the code.
    /// Traditionally assertions are removed when the development has completed in order to imporve performance. 
    /// Even though in a final product assertions should not occure anymore i believe that many applications 
    /// can afford keeping the the assertions alive. 
    /// In contrast to the method Assert of the class System.Debug the class Assert only generates an exception in case of a violated assertion. 
    /// This is basically the same as with NUnit. But i do not want ot include NUnit in all my project and i 
    /// </summary>
    public static class Assert
    {
        public static void True(bool condition, string message, params object[] args)
        {
            if (!condition)
            {
                throw new ViolatedAssertionException(string.Format( message, args));
            }
        }

    }


}
