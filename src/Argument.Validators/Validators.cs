// <copyright file="Validators.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;
using Ubiquity.ArgValidators.Properties;

namespace Ubiquity.ArgValidators
{
    /// <summary>Parameter validation extension set.</summary>
    public static class Validators
    {
        /// <summary>Verifies that a value is not equal to the default for the type.</summary>
        /// <typeparam name="T">Type of value to test for.</typeparam>
        /// <param name="value">Value to test.</param>
        /// <param name="paramName">Name of the parameter for the argument exception generated.</param>
        /// <returns><paramref name="value"/> for fluent design usage.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is the default value for value types or <see lang="null"/> for reference types.</exception>
        [DebuggerStepThrough]
        public static T ValidateNotDefault<T>( [ValidatedNotNull] this T value, string paramName )
        {
            // reference types default is null, for value types this compares to a boxed value
            if( ReferenceEquals(value, default))
            {
                throw new ArgumentNullException( paramName );
            }

            return !EqualityComparer<T>.Default.Equals( value, default! ) ? value : throw new ArgumentNullException( paramName );
        }

        /// <summary>Verifies an object isn't null.</summary>
        /// <typeparam name="T">Type of value to test for.</typeparam>
        /// <param name="value">Object instance to check.</param>
        /// <param name="paramName">Name of the parameter for the exception if <paramref name="value"/> is <see lang="null"/>.</param>
        /// <returns><paramref name="value"/> for fluent design usage.</returns>
        [DebuggerStepThrough]
        public static T ValidateNotNull<T>( [ValidatedNotNull] this T value, string paramName )
            where T : class
        {
            return value ?? throw new ArgumentNullException( paramName );
        }

        /// <summary>Verifies an IntPtr isn't null.</summary>
        /// <param name="value">value to check.</param>
        /// <param name="paramName">Name of the parameter for the exception if <paramref name="value"/> is <see lang="null"/>.</param>
        /// <returns><paramref name="value"/> for fluent design usage.</returns>
        [DebuggerStepThrough]
        public static IntPtr ValidateNotNull( this IntPtr value, string paramName )
        {
            return value != IntPtr.Zero ? value : throw new ArgumentNullException( paramName );
        }

        /// <summary>Verifies an UIntPtr isn't null.</summary>
        /// <param name="value">value to check.</param>
        /// <param name="paramName">Name of the parameter for the exception if <paramref name="value"/> is <see lang="null"/>.</param>
        /// <returns><paramref name="value"/> for fluent design usage.</returns>
        [DebuggerStepThrough]
        [CLSCompliant(false)]
        public static UIntPtr ValidateNotNull( this UIntPtr value, string paramName )
        {
            return value != UIntPtr.Zero ? value : throw new ArgumentNullException( paramName );
        }

        /// <summary>Validates a parameter string is not null or white space.</summary>
        /// <param name="value">string to test.</param>
        /// <param name="paramName">Name of the parameter for the exception if the <paramref name="value"/> is <see lang="null"/> or whitespace.</param>
        /// <returns><paramref name="value"/> for fluent design usage.</returns>
        /// <remarks>
        /// This validation provides a distinct exception or exception message depending on the input <paramref name="value"/>.
        /// </remarks>
        /// <exception cref="ArgumentNullException">If <paramref name="value"/> is <see lang="null"/>.</exception>
        /// <exception cref="ArgumentException">If the string is empty or all whitespace (<see cref="Exception.Message"/> message will indicate which condition triggered the exception).</exception>
        [DebuggerStepThrough]
        public static string ValidateNotNullOrWhiteSpace( [ValidatedNotNull] this string value, string paramName )
        {
            if( value == null )
            {
                throw new ArgumentNullException( paramName );
            }

            if( value.Length == 0 || ( ( value.Length == 1 ) && ( value[ 0 ] == '\0' ) ) )
            {
                throw new ArgumentException( Resources.String_Must_not_be_empty, paramName );
            }

            return !string.IsNullOrWhiteSpace( value )
                ? value
                : throw new ArgumentException( Resources.String_must_not_be_whitespace, paramName );
        }

        /// <summary>Verifies a parameter is within a valid range.</summary>
        /// <typeparam name="T">Type of the parameter (must be primitive type supporting comparisons).</typeparam>
        /// <param name="i">Value of the parameter to test.</param>
        /// <param name="min">Minimum allowed value.</param>
        /// <param name="max">Maximum allowed value.</param>
        /// <param name="paramName">Name of the parameter for the exception if the <paramref name="i"/> is outside the specified range.</param>
        [DebuggerStepThrough]
        public static void ValidateRange<T>( this T i, T min, T max, string paramName )
            where T : struct, IComparable<T>
        {
            if( min.CompareTo( i ) > 0 || i.CompareTo( max ) > 0 )
            {
                throw new ArgumentOutOfRangeException( paramName, i, string.Format( CultureInfo.CurrentCulture, Resources.Accepted_range_0_1_, min, max) );
            }
        }

        /// <summary>Verifies that a string matches a Regular expression pattern.</summary>
        /// <param name="value">String value to validate.</param>
        /// <param name="pattern">Pattern to use for testing <paramref name="value"/>.</param>
        /// <param name="paramName">Name of the parameter for the exception message if <paramref name="value"/> does not match the pattern.</param>
        /// <returns><see cref="Match"/> from a successful pattern match.</returns>
        [DebuggerStepThrough]
        public static Match ValidatePattern( [ValidatedNotNull] this string value, string pattern, string paramName )
        {
            if( string.IsNullOrWhiteSpace( value ) )
            {
                throw new ArgumentException( Resources.Must_not_be_null_or_whitespace, paramName );
            }

            if( string.IsNullOrWhiteSpace( pattern ) )
            {
                throw new ArgumentException( Resources.Must_not_be_null_or_whitespace, nameof( pattern ) );
            }

            var regEx = new Regex( pattern );
            var match = regEx.Match( value );
            return match.Success
                ? match
                : throw new ArgumentException( string.Format( CultureInfo.CurrentCulture, Resources.Value_does_not_conform_to_required_format_0_, pattern ), paramName );
        }

        /// <summary>Verifies that a string matches a Regular expression pattern.</summary>
        /// <param name="value">String value to validate.</param>
        /// <param name="pattern">Pattern to use for testing <paramref name="value"/>.</param>
        /// <param name="paramName">Name of the parameter for the exception message if <paramref name="value"/> does not match the pattern.</param>
        /// <returns><see cref="Match"/> from a successful pattern match.</returns>
        [DebuggerStepThrough]
        public static Match ValidatePattern( [ValidatedNotNull] this string value, Regex pattern, string paramName )
        {
            if( string.IsNullOrWhiteSpace( value ) )
            {
                throw new ArgumentException( Resources.Must_not_be_null_or_whitespace, paramName );
            }

            if( pattern == null )
            {
                throw new ArgumentNullException( nameof( pattern ) );
            }

            var match = pattern.Match( value );
            return match.Success
                ? match
                : throw new ArgumentException( string.Format( CultureInfo.CurrentCulture, Resources.Value_does_not_conform_to_required_format_0_, pattern ), paramName );
        }

        /// <summary>Validates a string length falls within a specific range.</summary>
        /// <param name="value">String to validate.</param>
        /// <param name="min">Minimum allowed number of characters in the string.</param>
        /// <param name="max">Maximum allowed number of characters in the string.</param>
        /// <param name="paramName">Name of the parameter for the exception message if <paramref name="value"/> length is outside the specified range.</param>
        /// <returns><paramref name="value"/> for fluent design usage.</returns>
        [DebuggerStepThrough]
        public static string ValidateLength( [ValidatedNotNull] this string value, int min, int max, string paramName )
        {
            if( value == null )
            {
                throw new ArgumentNullException( nameof( value ) );
            }

            return value.Length >= min && value.Length <= max
                ? value
                : throw new ArgumentException( string.Format( CultureInfo.CurrentCulture, Resources.Expected_string_with_length_in_the_range_0_1_, min, max ), paramName );
        }

        /// <summary>Validates that a value is defined on the Enum type.</summary>
        /// <typeparam name="T">Enumeration type.</typeparam>
        /// <param name="value">Value to check is defined.</param>
        /// <param name="paramName">name of the parameter owning the value.</param>
        /// <returns><paramref name="value"/> for fluent design usage.</returns>
        public static T ValidateDefined<T>( this T value, string paramName )
            where T : Enum
        {
            return Enum.IsDefined( typeof( T ), value )
                ? value
                : throw new ArgumentException( string.Format( CultureInfo.CurrentCulture, Resources.Type_0_does_not_define_an_enumerated_value_for_1_, typeof( T ).FullName, value ), paramName );
        }
    }
}
