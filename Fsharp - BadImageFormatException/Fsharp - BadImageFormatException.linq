<Query Kind="FSharpProgram" />

type IThingWithKey<'Key> = 
    abstract member GetKey: unit -> 'Key
    
type IRelatedThingWithKey<'Key> =
    abstract member GetKey: unit -> 'Key

type Thing = 
    {
        Prop1:int
    } 
    interface IThingWithKey<int> with
        member this.GetKey() = this.Prop1
        
type RelatedThing = 
    {
        PropA:int
    } 
    interface IRelatedThingWithKey<int> with
        member this.GetKey() = this.PropA
        
type ThingGroup<'Thing, 'Related> = 
    {
        Thing: 'Thing
        RelatedThing: 'Related option
    }
    
let WithRelatedThing relatedThing thing = 
    {
        Thing = thing
        RelatedThing = relatedThing
    }

let WithRelatedThings<'Thing, 'RelatedThing, 'Key 
                    when 'RelatedThing :> IRelatedThingWithKey<'Key> 
                    and 'Thing :> IThingWithKey<'Key> 
                    and 'Key : comparison> 
                        (relatedThings: 'RelatedThing seq ) 
                        (things: 'Thing seq) = 

    let relatedThingsMap = relatedThings |> Seq.map (fun x -> (x.GetKey(), x)) |> Map.ofSeq
    
    //This version throws a BadImageFormatException
    //let relatedThingByThing (thing:IThingWithKey<'TKey>) = thing.GetKey() |> relatedThingsMap.TryFind
    
    //This version works
    let relatedThingByThing (thing:'Thing) = thing.GetKey() |> relatedThingsMap.TryFind
        
    let withRelatedThing thing = thing |> WithRelatedThing (relatedThingByThing thing)
    things |> Seq.map withRelatedThing
    
let Run() = 
    let things = [
        { Prop1 = 1 };
        { Prop1 = 2 };
    ]
    
    let relatedThings = [
        { PropA = 1 };
        { PropA = 2 };
    ]
    
    let merged = things |> WithRelatedThings relatedThings
    
    merged.Dump()
    
Run()