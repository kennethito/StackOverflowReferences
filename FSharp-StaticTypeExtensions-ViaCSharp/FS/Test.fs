namespace FS

[<AbstractClass; Sealed>]
type TestType() = 
    static member IrrelevantFunction() = 
        0

[<AutoOpen>]
module Extensions = 
    type TestType with
        //How do we call this from C#
        static member NeedToCallThis() = 
            0

module Caller = 
    let CallIt() = 
        //F# can call it
        TestType.NeedToCallThis()