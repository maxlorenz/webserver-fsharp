module WMI

open System.Management

let getObj (scope: string) query = 
    let searcher = new ManagementObjectSearcher(scope, query)
    searcher.Get()

let propertyToTuple (property: PropertyData) = (property.Name, property.Value)
let moToProperty (mo: ManagementObject) = mo.Properties

let castMap f = Seq.cast >> Seq.map f

let getWQL scope query = 
    getObj scope query
        |> castMap moToProperty
        |> castMap Seq.cast
        |> (Seq.map >> Seq.map) propertyToTuple

