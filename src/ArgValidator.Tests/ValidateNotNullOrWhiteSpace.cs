// <copyright file="ValidateNotNull.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ubiquity.ArgValidators;

/*
 ReSharper disable ExpressionIsAlwaysNull
 ReSharper disable NotResolvedInText
*/

namespace ArgValidator.Tests
{
    [TestClass]
    public class ValidateNotNullOrWhiteSpace
    {
        [TestMethod]
        [ExpectedException( typeof( ArgumentNullException ) )]
        public void With_null_throws()
        {
            Validators.ValidateNotNullOrWhiteSpace( null, "name" );
        }

        [TestMethod]
        [ExpectedException( typeof( ArgumentException ) )]
        [SuppressMessage( "StyleCop.CSharp.ReadabilityRules", "SA1122:Use string.Empty for empty strings", Justification = "explicitly not using string.Empty to ensure validation doesn't assume that" )]
        public void With_empty_string_throws( )
        {
            Validators.ValidateNotNullOrWhiteSpace( "", "name" );
        }

        [TestMethod]
        [ExpectedException( typeof( ArgumentException ) )]
        public void With_whitespace_only_throws( )
        {
            Validators.ValidateNotNullOrWhiteSpace( " \t\n\r", "name" );
        }

        [TestMethod]
        public void With_valid_string_succeeds( )
        {
            Validators.ValidateNotNullOrWhiteSpace( "x \t\n\r", "name" );
        }

        [TestMethod]
        public void With_valid_string_returns_input( )
        {
            string s = "x \t\n\r";
            string x = Validators.ValidateNotNullOrWhiteSpace( s, "name" );
            Assert.AreSame(s,x);
        }
    }
}
