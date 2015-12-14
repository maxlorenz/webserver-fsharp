# webserver-fsharp

A simple webserver in F#. It's purpose is to create a REST Api fast.

## Example usage
``` fsharp
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
```