# Argument.Validators
Common annotation and argument validation support

>NOTE
> v7.00 changed the name and namespace for these validators to conform
> to common standards for the Ubiquity.NET organization.


### Nuget
![Nuget](https://img.shields.io/nuget/dt/Ubiquity.ArgValidators.svg)

## Build Status
[![CI-Build](https://github.com/UbiquityDotNET/Argument.Validators/actions/workflows/pr-build.yml/badge.svg)](https://github.com/UbiquityDotNET/Argument.Validators/actions/workflows/pr-build.yml)
[![Release-Build](https://github.com/UbiquityDotNET/Argument.Validators/actions/workflows/release-build.yml/badge.svg)](https://github.com/UbiquityDotNET/Argument.Validators/actions/workflows/release-build.yml)


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

[Complete documentation](https://ubiquitydotnet.github.io/Argument.Validators/) on all validators is available online
