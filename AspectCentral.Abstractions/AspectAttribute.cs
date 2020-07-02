//  ----------------------------------------------------------------------------------------------------------------------
//  <copyright file="AspectAttribute.cs" company="James Consulting LLC">
//    Copyright (c) 2019 All Rights Reserved
//  </copyright>
//  <author>Rudy James</author>
//  <summary>
// 
//  </summary>
//  ----------------------------------------------------------------------------------------------------------------------

using System;

namespace AspectCentral.Abstractions
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class AspectAttribute : Attribute
    {
    }
}