module Webserver

open System.Net
open System.Text
open System.Text.RegularExpressions
open System.IO

let compileRoute (pattern: string) =
    
    // compile pattern to regex

    // {{variable}} is used to capture variable as a regex group
    let compileRegex = Regex @"\{\{(\w+?)\}\}"
    let replacement = "(?<$1>[^/]*)"

    let routeRegex =
        compileRegex.Replace(pattern, replacement) 
        |> sprintf "^%s/?$" // Dont match partial URLs, but allow ending /
        |> Regex

    fun (x: string) -> 
        let group = routeRegex.Match x

        if group.Success then
            Some group.Groups
        else
            None

let listen (url: string) (port: int) (notFound: string) (binds: (string * (GroupCollection -> string)) seq) =

    let host = sprintf "http://%s:%i/" url port 

    let asBytes (text: string) = Encoding.UTF8.GetBytes(text)
    let listener = new HttpListener()

    listener.Prefixes.Add(host)
    listener.Start()

    while true do

        let context = listener.GetContext()
        let request = context.Request
        let response = context.Response

        // Allow foreign requests
        response.AppendHeader("Access-Control-Allow-Origin", "*");

        let mutable res = notFound

        for (route, action) in binds do
            let vars = request.Url.LocalPath |> (compileRoute route)

            if vars.IsSome then res <- action vars.Value
                                

        let bytes = asBytes res
        let output = response.OutputStream

        response.ContentLength64 <- (int64)bytes.Length

        output.Write(bytes, 0, bytes.Length)
        output.Close()
    
    listener.Stop()