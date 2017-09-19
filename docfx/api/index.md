# Ubiquity.Argvalidators
This namespace provides a set of common validators for method
arguments to keep code simpler and more readable.

## Examples

```C#
using namespace Ubiquity.ArgValidators
//...
public static object Load( string path, object foo, int bar )
{
    path.ValidateNotNullOrWhiteSpace( nameof( path ) );
    foo.ValidateNotNull( nameof( foo ) );
    bar.ValidateRange(3, 5, nameof( bar ) );

    //...
}
```


