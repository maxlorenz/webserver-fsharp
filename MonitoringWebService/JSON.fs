module JSON

type Json = 
    | JString of string
    | JNumber of double
    | JObject of (string * Json) list
    | JArray of Json list
    | JBool of bool
    | JNull

let concatList = String.concat ", "
let keyValue convert (name, value) = 
    sprintf "\"%s\": %s" name (convert value)

let rec toString (json: Json): string = 
    match json with
        | JString a -> sprintf "\"%s\"" a
        | JNumber a -> sprintf "%g" a
        | JObject a -> concatList (List.map (keyValue toString) a) |> sprintf "{ %s }"
        | JArray a -> concatList (List.map toString a) |> sprintf "[ %s ]"
        | JBool a -> if a then "true" else "false"
        | JNull -> "null"