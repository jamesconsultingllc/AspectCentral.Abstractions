// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProfilingAspectRegistrationBuilderExtensionsTests.cs" company="CBRE">
//   
// </copyright>
//  <summary>
//   The profiling aspect registration builder extensions tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using Microsoft.Extensions.DependencyInjection;

namespace AspectCentral.Abstractions.Tests.Profiling
{
    /// <summary>
    ///     The profiling aspect registration builder extensions tests.
    /// </summary>
    [TestClass]
    public class ProfilingAspectRegistrationBuilderExtensionsTests
    {
        /// <summary>
        ///     The add profiling aspect null builder throws argument null exception.
        /// </summary>
        [TestMethod]
        public void AddProfilingAspectNullBuilderThrowsArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => default(IAspectRegistrationBuilder).AddProfilingAspect());
        }

        /// <summary>
        ///     The add profiling aspect registers all methods when no methods are given.
        /// </summary>
        [TestMethod]
        public void AddProfilingAspectRegistersAllMethodsWhenNoMethodsAreGiven()
        {
            var builder = new ServiceCollection().AddAspectSupport().AddTransient<ITestInterface, MyTestInterface>().AddProfilingAspect();

            var aspects = builder.AspectConfigurationProvider.ConfigurationEntries.Last().GetAspects().ToArray();
            Assert.AreEqual(ProfilingAspectFactory.ProfilingAspectFactoryType, aspects[0].AspectFactoryType);
        }
    }
}