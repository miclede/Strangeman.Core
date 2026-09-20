namespace System.Runtime.CompilerServices
{
    public class IsExternalInit
    {
        //https://stackoverflow.com/questions/64749385/predefined-type-system-runtime-compilerservices-isexternalinit-is-not-defined
        //The compiler throws this error because we're compiling a .NET 5 code against older .NET Framework version. See his message below:
        //Thanks for taking the time to file this feedback issue. Unfortunately this is not a bug. The IsExternalInit type is only included in the net5.0 (and future) target frameworks.
        //When compiling against older target frameworks you will need to manually define this type.
    }
}