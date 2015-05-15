module Webserver

open System.Net
open System.Text
open System.IO

let listen (local: bool) (port: int) (getResponse: string -> string) =

    let host = sprintf "http://%s:%i/" (if local then "localhost" else "*") port 

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

        let json = getResponse request.Url.LocalPath |> asBytes
        let output = response.OutputStream

        response.ContentLength64 <- (int64)json.Length

        output.Write(json, 0, json.Length)
        output.Close()
    
    listener.Stop()

let listenLocal = listen true
let listenPublic = listen false