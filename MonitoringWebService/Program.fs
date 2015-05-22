open JSON
open WMI
open Webserver

let rec valueToJSON (value: obj) = 
    match value with
    | :? string as s -> 
        JString (s.Replace("\"", "''").Replace("\\", "\\\\"))
    | :? bool as b -> 
        JBool b
    | :? uint8 | :? uint16 | :? uint32 | :? uint64 -> 
        JNumber (System.Double.Parse(value.ToString()))
    | :? (uint8 array) as a -> 
        JArray [ for element in a do yield valueToJSON element ]
    | :? (uint16 array) as a -> 
        JArray [ for element in a do yield valueToJSON element ]
    | :? (uint32 array) as a -> 
        JArray [ for element in a do yield valueToJSON element ]
    | :? (uint64 array) as a -> 
        JArray [ for element in a do yield valueToJSON element ]
    | :? (obj array) as a -> 
        JArray [ for element in a do yield valueToJSON element ]
    | null -> 
        JNull
    | _ -> 
        JString (value.GetType().FullName)

let objToJSON obj =
    JObject [ for (name, value) in obj do yield (name, valueToJSON value) ]

let wmiToJSON wmi =
    JArray [ for obj in wmi do yield objToJSON obj ]

[<EntryPoint>]
let main argv =

    let route (path: string) =
        match path with
        | path -> getWQL @"root\cimv2" (sprintf "select * from %s" (path.Split('/').[1])) |> wmiToJSON |> JSON.toString

    // 8080 is the port
    // route is function that takes the url and returns some text to send to the client
    Webserver.listenLocal 8080 route
        
    0 // return an integer exit code