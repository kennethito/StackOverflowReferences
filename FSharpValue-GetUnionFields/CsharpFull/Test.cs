using Microsoft.FSharp.Core;
using Microsoft.FSharp.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CsharpFull
{
    static class Test
    {
        static Tuple<UnionCaseInfo, object[]> TestIt()
        {
            var option = new FSharpOption<int>(123);

            var bindingFlags = new FSharpOption<BindingFlags>(BindingFlags.Public);

            return FSharpValue.GetUnionFields(option, option.GetType(), bindingFlags);
        }
    }
}
