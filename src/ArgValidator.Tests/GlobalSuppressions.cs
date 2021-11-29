// <copyright file="GlobalSuppressions.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>

/* This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.
*/

using System;
using System.Diagnostics.CodeAnalysis;

[assembly: CLSCompliant( false )]

[assembly: SuppressMessage(
    "Language Usage Opportunities",
    "RECS0015:If an extension method is called as static method convert it to method syntax",
    Justification = "Provides clarity for tests to be explicit"
    )]

[assembly: SuppressMessage( "StyleCop.CSharp.DocumentationRules", "SA1652:Enable XML documentation output", Justification = "Unit tests" )]
[assembly: SuppressMessage( "StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "Unit Tests" )]
