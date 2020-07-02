//  ----------------------------------------------------------------------------------------------------------------------
//  <copyright file="MethodTypeOptions.cs" company="James Consulting LLC">
//    Copyright (c) 2019 All Rights Reserved
//  </copyright>
//  <author>Rudy James</author>
//  <summary>
// 
//  </summary>
//  ----------------------------------------------------------------------------------------------------------------------

namespace AspectCentral.Abstractions
{
    /// <summary>
    ///     The method type options.
    /// </summary>
    public enum MethodTypeOptions
    {
        /// <summary>
        ///     The sync action.
        /// </summary>
        SyncAction,

        /// <summary>
        ///     The sync function
        /// </summary>
        SyncFunction,

        /// <summary>
        ///     The async function.
        /// </summary>
        AsyncFunction,

        /// <summary>
        ///     Async action
        /// </summary>
        AsyncAction
    }
}