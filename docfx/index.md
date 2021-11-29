# Ubiquity.ArgValidators
This is a simple library to enable common argument validation in .NET code

## Quick Start Notes:
1. Install the Ubiquity.ArgValidators NuGet Package
2. Start using the validators in your project!

## Examples

```C#
using namespace Ubiquity.NET.ArgValidators
//...
public static object Load( string path, object foo, int bar )
{
    path.ValidateNotNullOrWhiteSpace( nameof( path ) );
    foo.ValidateNotNull( nameof( foo ) );
    bar.ValidateRange( 3, 5, nameof( bar ) );

    //...
}
```

For more details see the full [API documentation](api/index.md)
