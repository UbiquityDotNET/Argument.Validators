// <copyright file="ValidateNotNull.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ubiquity.ArgValidators;

/*
 ReSharper disable ExpressionIsAlwaysNull
 ReSharper disable NotResolvedInText
*/

namespace ArgValidator.Tests
{
    [TestClass]
    public class ValidateNotNull
    {
        [TestMethod]
        [ExpectedException( typeof( ArgumentNullException ) )]
        public void With_null_throws()
        {
            object o = null;
            Validators.ValidateNotNull( o, "name" );
        }

        [TestMethod]
        public void With_nonnull_succeeds( )
        {
            var x = new NotSupportedException();
            Validators.ValidateNotNull( x, "name" );
        }

        [TestMethod]
        public void With_nonnull_returns_input( )
        {
            var x = new NotSupportedException( );
            Assert.AreSame(x, Validators.ValidateNotNull( x, "name" ));
        }

        [TestMethod]
        [ExpectedException( typeof( ArgumentNullException ) )]
        public void With_intpr_zero_throws( )
        {
            var o = IntPtr.Zero;
            Validators.ValidateNotNull( o, "name" );
        }

        [TestMethod]
        public void With_nonzero_intptr_succeeds( )
        {
            var x = (IntPtr)12345678;
            Validators.ValidateNotNull( x, "name" );
        }

        [TestMethod]
        public void With_nonzero_intptr_returns_input( )
        {
            var x = ( IntPtr )12345678;
            Assert.AreEqual( x, Validators.ValidateNotNull( x, "name" ) );
        }

        [TestMethod]
        [ExpectedException( typeof( ArgumentNullException ) )]
        public void With_uintpr_zero_throws( )
        {
            var o = UIntPtr.Zero;
            Validators.ValidateNotNull( o, "name" );
        }

        [TestMethod]
        public void With_nonzero_uintptr_succeeds( )
        {
            var x = ( UIntPtr )12345678;
            Validators.ValidateNotNull( x, "name" );
        }

        [TestMethod]
        public void With_nonzero_uintptr_returns_input( )
        {
            var x = ( UIntPtr )12345678;
            Assert.AreEqual( x, Validators.ValidateNotNull( x, "name" ) );
        }
    }
}
