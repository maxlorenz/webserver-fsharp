# webserver-fsharp

A simple webserver in F#. It's purpose is to deliver WMI data in JSON for monitoring tools.

## Example usage
``` fsharp
	open JSON
	open Webserver
	open System.Diagnostics
	
	let cpu = new PerformanceCounter("Processor", "% Processor Time", "_Total")
	let ram = new PerformanceCounter("Memory", "% Committed Bytes In Use", "")
	
	let getJSON() = 
	    let cpuValue = (double)(cpu.NextValue())
	    let ramValue = (double)(ram.NextValue())
	    JObject [ ("cpu", cpuValue |> JNumber); ("ram", ramValue |> JNumber) ] |> JSON.toString
	
	[<EntryPoint>]
	let main argv =
	
	    let route path =
	        match path with
	        | "/info" -> getJSON()
	        | _ -> "404"
	
	    // 8080 is the port
	    // route is function that takes the url and returns some text to send to the client
	    Webserver.listenLocal 8080 route
	        
	    0 // return an integer exit code
```