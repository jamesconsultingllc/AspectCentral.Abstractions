// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MyTestInterface.cs" company="James Consulting LLC">
//   
// </copyright>
// // <summary>
//   The my interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;

namespace AspectCentral.Abstractions.Tests
{
    /// <summary>
    ///     The my interface.
    /// </summary>
    internal class MyTestInterface : ITestInterface
    {
        public static readonly Type MyTestInterfaceType = typeof(MyTestInterface);
        
        /// <inheritdoc />
        public async Task<MyUnitTestClass> GetClassByIdAsync(int id)
        {
            await Task.Delay(100).ConfigureAwait(false);
            return new MyUnitTestClass(id, $"testing {id}3");
        }

        /// <inheritdoc />
        public void Test(int x, string y, MyUnitTestClass myUnitTestClass)
        {
            Console.WriteLine("testing");
        }

        /// <inheritdoc />
        public Task TestAsync(int x, string y, MyUnitTestClass myUnitTestClass)
        {
            return Task.Delay(100);
        }
    }
}