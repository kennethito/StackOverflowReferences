using FS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS
{
    public class Test
    {
        public void Caller()
        {
            TestType.IrrelevantFunction();
            
            //We want to call this
            //TestType.NeedToCallThis();

            //Metadata:

            //namespace FS
            //{
            //    [Microsoft.FSharp.Core.AutoOpenAttribute]
            //    [Microsoft.FSharp.Core.CompilationMappingAttribute]
            //    public static class Extensions
            //    {
            //        public static int TestType.NeedToCallThis.Static();
            //    }
            //}

            //None of these compile
            //TestType.NeedToCallThis();
            //Extensions.TestType.NeedToCallThis.Static();
            //Extensions.TestType.NeedToCallThis();
        }
    }
}
