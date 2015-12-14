open Webserver

[<EntryPoint>]
let main argv =
    printfn "Listening on port 8000..."

    Webserver.listen
        "localhost"
        8000
        "404, site not found!"
        [
            "", fun _ -> "status"
            "/categories", fun _ -> "list of all categories: ..."
            "/applications", fun _ -> "list of all applications: ..."
            "/categories/{{name}}", fun x -> sprintf "requested %s" x.["name"].Value
            // ...
        ]

    0 // return an integer exit code