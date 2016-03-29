module Test

open Microsoft.FSharp.Reflection

let TestIt() = 
    let option = Some(123)
    
    FSharpValue.GetUnionFields(option, option.GetType())

