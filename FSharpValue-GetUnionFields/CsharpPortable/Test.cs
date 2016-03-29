using Microsoft.FSharp.Core;
using Microsoft.FSharp.Reflection;
using System;
using System.Reflection;

namespace CsharpPortable
{
    static class Test
    {
        static Tuple<UnionCaseInfo, object[]> TestIt()
        {
            var option = new FSharpOption<int>(123);

            throw new NotImplementedException();

            //Does not compile, however the F# version does
            //return FSharpValue.GetUnionFields(option, option.GetType());
        }
    }
}
