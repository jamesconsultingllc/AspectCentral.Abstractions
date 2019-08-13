// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoggingAspectRegistrationBuilderExtensionsTests.cs" company="CBRE">
//   
// </copyright>
//  <summary>
//   The logging aspect registration builder extensions tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using Microsoft.Extensions.DependencyInjection;

namespace AspectCentral.Abstractions.Tests.Logging
{
    /// <summary>
    /// The logging aspect registration builder extensions tests.
    /// </summary>
    [TestClass]
    public class LoggingAspectRegistrationBuilderExtensionsTests
    {
        /// <summary>
        /// The add logging aspect null builder throws argument null exception.
        /// </summary>
        [TestMethod]
        public void AddLoggingAspectNullBuilderThrowsArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => default(IAspectRegistrationBuilder).AddLoggingAspect());
        }

        /// <summary>
        /// The add logging aspect registers all methods when no methods are given.
        /// </summary>
        [TestMethod]
        public void AddLoggingAspectRegistersAllMethodsWhenNoMethodsAreGiven()
        {
            var builder = new ServiceCollection().AddAspectSupport().AddTransient<ITestInterface, MyTestInterface>().AddLoggingAspect();

            var aspects = builder.AspectConfigurationProvider.ConfigurationEntries[0].GetAspects().ToArray();
            Assert.AreEqual(typeof(MyTestInterface), builder.AspectConfigurationProvider.ConfigurationEntries[0].ServiceDescriptor.ImplementationType);
            Assert.AreEqual(1, aspects.Length);
            Assert.AreEqual(LoggingAspectFactory.LoggingAspectFactoryType, aspects[0].AspectFactoryType);
        }
    }
}