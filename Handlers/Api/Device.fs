namespace SmartPowerplugEndpoint.Handlers.Api

open Microsoft.AspNetCore.Http
open FSharp.Control.Tasks.V2.ContextInsensitive
open Giraffe
open SmartPowerplugEndpoint.Models

module Device =

    let post (next: HttpFunc) (ctx: HttpContext) =
        ()
