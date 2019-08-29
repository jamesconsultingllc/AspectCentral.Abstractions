//  ----------------------------------------------------------------------------------------------------------------------
//  <copyright file="TestAspectRegistrationBuilder.cs" company="James Consulting LLC">
//    Copyright (c) 2019 All Rights Reserved
//  </copyright>
//  <author>Rudy James</author>
//  <summary>
// 
//  </summary>
//  ----------------------------------------------------------------------------------------------------------------------

using System;
using AspectCentral.Abstractions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AspectCentral.Abstractions.Tests
{
    public class TestAspectRegistrationBuilder : AspectRegistrationBuilder
    {
        /// <inheritdoc />
        public TestAspectRegistrationBuilder(IServiceCollection services, IAspectConfigurationProvider aspectConfigurationProvider) : base(services, aspectConfigurationProvider)
        {
        }

        /// <inheritdoc />
        public override object InvokeCreateFactory(IServiceProvider serviceProvider, AspectConfiguration aspectConfiguration)
        {
            throw new NotImplementedException();
        }
    }
}