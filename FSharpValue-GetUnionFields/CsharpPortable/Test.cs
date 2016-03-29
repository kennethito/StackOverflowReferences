using Microsoft.FSharp.Core;
using Microsoft.FSharp.Reflection;

namespace CsharpPortable
{
    static class Test
    {
        static FSharpOption<int> TestIt()
        {
            var option = new FSharpOption<int>(123);

            //Does not compile, however the F# version does
            return FSharpValue.GetUnionFields(option, option.GetType());
        }
    }
}
