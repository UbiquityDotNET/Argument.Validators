// <copyright file="ValidatedNotNullAttribute.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>

using System;

namespace Ubiquity.ArgValidators
{
    /// <summary>Marker Attribute to inform CodeAnalysis that a parameter is validated as non-null in a method.</summary>
    [AttributeUsage( AttributeTargets.Parameter )]
    public sealed class ValidatedNotNullAttribute
        : Attribute
    {
    }
}
