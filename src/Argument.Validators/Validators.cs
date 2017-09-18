using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace Ubiquity.ArgValidators
{
    /// <summary>Marker Atttribute to inform CodeAnalysis that a parameter is validated as non-null in a method</summary>
    [SuppressMessage( "", "SA1402", Justification = "Closely related to the static validators" )]
    [AttributeUsage( AttributeTargets.Parameter, Inherited = true, AllowMultiple = false )]
    public sealed class ValidatedNotNullAttribute
        : Attribute
    {
    }

    /// <summary>Parameter validation extension set</summary>
    public static class Validators
    {
        /// <summary>Verifies an object isn't null</summary>
        /// <param name="obj">Object instance to check</param>
        /// <param name="paramName">Name of the parameter for the exception if <paramref name="obj"/> is null</param>
        [ContractAnnotation( "obj:null => halt" )]
        public static void ValidateNotNull( [ValidatedNotNull] this object obj, [InvokerParameterName] string paramName )
        {
            if( obj == null )
            {
                throw new ArgumentNullException( paramName );
            }
        }

        /// <summary>Validates a parameter string is not null or white space</summary>
        /// <param name="str">string to test</param>
        /// <param name="paramName">Name of the parameter for the exception if the <paramref name="str"/> is null or whitespace</param>
        [ContractAnnotation( "str:null => halt" )]
        public static void ValidateNotNullOrWhiteSpace( [ValidatedNotNull] this string str, [InvokerParameterName] string paramName )
        {
            if( string.IsNullOrWhiteSpace( str ) )
            {
                throw new ArgumentException( "Must not be null or whitespace", paramName );
            }
        }

        /// <summary>Verifies a parameter is within a valid range</summary>
        /// <typeparam name="T">Type of the parameter (must be primitive type supporting comparisons)</typeparam>
        /// <param name="i">Value of the parameter to test</param>
        /// <param name="min">Minimum allowed value</param>
        /// <param name="max">Maximum allowed value</param>
        /// <param name="paramName">Name of the parameter for the exception if the <paramref name="i"/> is outside the specified range</param>
        public static void ValidateRange<T>( this T i, T min, T max, [InvokerParameterName] string paramName )
            where T : struct, IComparable<T>
        {
            if( min.CompareTo( i ) > 0 || i.CompareTo( max ) > 0 )
            {
                throw new ArgumentOutOfRangeException( paramName, i, $"Accepted range: [{min}, {max}]" );
            }
        }

        /// <summary>Verifies that a string matches a Regular expression pattern</summary>
        /// <param name="value">String value to validate</param>
        /// <param name="pattern">Pattern to use for testing <paramref name="value"/></param>
        /// <param name="paramName">Name of the parameter for the exception message if <paramref name="value"/> does not match the pattern</param>
        [ContractAnnotation( "value:null => halt" )]
        public static Match ValidatePattern( [ValidatedNotNull] this string value, [RegexPattern] string pattern, [InvokerParameterName] string paramName )
        {
            value.ValidateNotNullOrWhiteSpace( nameof( value ) );
            pattern.ValidateNotNullOrWhiteSpace( nameof( pattern ) );
            var regEx = new Regex( pattern );
            var match = regEx.Match( value );
            if( !match.Success )
            {
                throw new ArgumentException( $"Value does not conform to required format: {pattern}", paramName );
            }

            return match;
        }

        /// <summary>Verifies that a string matches a Regular expression pattern</summary>
        /// <param name="value">String value to validate</param>
        /// <param name="pattern">Pattern to use for testing <paramref name="value"/></param>
        /// <param name="paramName">Name of the parameter for the exception message if <paramref name="value"/> does not match the pattern</param>
        [ContractAnnotation( "value:null => halt" )]
        public static Match ValidatePattern( [ValidatedNotNull] this string value, Regex pattern, [InvokerParameterName] string paramName )
        {
            value.ValidateNotNullOrWhiteSpace( nameof( value ) );
            var match = pattern.Match( value );
            if( !match.Success )
            {
                throw new ArgumentException( $"Value does not conform to required format: {pattern}", paramName );
            }

            return match;
        }

        /// <summary>Validates a string length falls within a specific range</summary>
        /// <param name="value">String to validate</param>
        /// <param name="min">Minimum allowed number of characters in the string</param>
        /// <param name="max">Maximum allowed number of characters in the string</param>
        /// <param name="paramName">Name of the parameter for the exception message if <paramref name="value"/> length is outside the specified range</param>
        [ContractAnnotation( "value:null => halt" )]
        public static void ValidateLength( [ValidatedNotNull] this string value, int min, int max, [InvokerParameterName] string paramName )
        {
            value.ValidateNotNull( paramName );
            if( value.Length < min || value.Length > max )
            {
                throw new ArgumentException( $"Expected string with length in the range [{min}, {max}]", paramName );
            }
        }
    }
}
