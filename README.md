# Argument.Validators
Common annotation and argument validation support

### Nuget
![Nuget](https://img.shields.io/nuget/dt/Ubiquity.ArgValidators.svg)

## Build Status
[![Build status](https://ci.appveyor.com/api/projects/status/2tm3k19g98piya52/branch/master?svg=true)](https://ci.appveyor.com/project/UbiquityDotNet/argument-validators/branch/master)


## Examples

```C#
using namespace Ubiquity.ArgValidators
//...
public static object Load( string path, object foo, int bar )
{
    path.ValidateNotNullOrWhiteSpace( nameof( path ) );
    foo.ValidateNotNull( nameof( foo ) );
    bar.ValidateRange( 3, 5, nameof( bar ) );

    //...
}
```

