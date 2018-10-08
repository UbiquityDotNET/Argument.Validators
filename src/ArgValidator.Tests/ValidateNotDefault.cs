// <copyright file="UnitTest1.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ubiquity.ArgValidators;

namespace ArgValidator.Tests
{
    [TestClass]
    public class ValidateNotDefault
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void With_default_of_valuetype_throws( )
        {
            Validators.ValidateNotDefault( default( int ), "name" );
        }

        [TestMethod]
        public void With_nondefault_valuetype_succeeds( )
        {
           Validators.ValidateNotDefault( 12345678, "name" );
        }

        [TestMethod]
        public void With_nondefault_valuetype_returns_input_value( )
        {
            Assert.AreEqual( 12345678, Validators.ValidateNotDefault( 12345678, "name" ) );
        }

        [TestMethod]
        [ExpectedException( typeof( ArgumentNullException ) )]
        public void With_default_of_reftype_throws( )
        {
            object obj = null;
            Validators.ValidateNotDefault( obj, "name" );
        }

        [TestMethod]
        public void With_nondefault_refype_succeeds( )
        {
            var x = new NotSupportedException( );
            Validators.ValidateNotDefault( x, "name" );
        }

        [TestMethod]
        public void With_nondefault_reftype_returns_input_value( )
        {
            var x = new NotSupportedException( );
            Assert.AreSame( x, Validators.ValidateNotDefault( x, "name" ) );
        }
    }
}
