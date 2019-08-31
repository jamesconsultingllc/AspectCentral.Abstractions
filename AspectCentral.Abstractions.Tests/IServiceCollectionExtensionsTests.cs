//  ----------------------------------------------------------------------------------------------------------------------
//  <copyright file="IServiceCollectionExtensionsTests.cs" company="James Consulting LLC">
//    Copyright (c) 2019 All Rights Reserved
//  </copyright>
//  <author>Rudy James</author>
//  <summary>
// 
//  </summary>
//  ----------------------------------------------------------------------------------------------------------------------

using System;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AspectCentral.Abstractions.Tests
{
    // ReSharper disable once InconsistentNaming
    public class IServiceCollectionExtensionsTests
    {
        private IServiceCollection serviceCollection;

        public IServiceCollectionExtensionsTests()
        {
            serviceCollection = new ServiceCollection();
        }
        
        [Fact]
        public void RegisterAspectsThrowsArgumentNullExceptionWhenServiceCollectionNull()
        {
            Assert.Throws<ArgumentNullException>("serviceCollection",() => default(IServiceCollection).RegisterAspects<IAspect>());
        }

        [Fact]
        public void RegisterAspectsSucceeds()
        {
            serviceCollection.RegisterAspects<IAspect>();
            serviceCollection.Count.Should().Be(1);
        }
    }
}