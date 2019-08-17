//  ----------------------------------------------------------------------------------------------------------------------
//  <copyright file="ITestInterface.cs" company="James Consulting LLC">
//    Copyright (c) 2019 All Rights Reserved
//  </copyright>
//  <author>Rudy James</author>
//  <summary>
// 
//  </summary>
//  ----------------------------------------------------------------------------------------------------------------------

using System.Threading.Tasks;

namespace AspectCentral.Abstractions.Tests
{
    /// <summary>
    ///     The Interface interface.
    /// </summary>
    internal interface ITestInterface
    {
        /// <summary>
        ///     The get class by id.
        /// </summary>
        /// <param name="id">
        ///     The id.
        /// </param>
        /// <returns>
        ///     The <see cref="Task" />.
        /// </returns>
        Task<MyUnitTestClass> GetClassByIdAsync(int id);

        /// <summary>
        ///     The test.
        /// </summary>
        /// <param name="x">
        ///     The x.
        /// </param>
        /// <param name="y">
        ///     The y.
        /// </param>
        /// <param name="myUnitTestClass">
        ///     The my class.
        /// </param>
        void Test(int x, string y, MyUnitTestClass myUnitTestClass);

        /// <summary>
        ///     The test async.
        /// </summary>
        /// <param name="x">
        ///     The x.
        /// </param>
        /// <param name="y">
        ///     The y.
        /// </param>
        /// <param name="myUnitTestClass">
        ///     The my class.
        /// </param>
        /// <returns>
        ///     The <see cref="Task" />.
        /// </returns>
        Task TestAsync(int x, string y, MyUnitTestClass myUnitTestClass);

        /// <summary>
        /// Gets the <typeparam name=""></typeparam>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        MyUnitTestClass GetClassById(int id);
    }
}