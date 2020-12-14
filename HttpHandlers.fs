namespace SmartPowerplugEndpoint

module HttpHandlers =

    open Microsoft.AspNetCore.Http
    open FSharp.Control.Tasks.V2.ContextInsensitive
    open Giraffe
    open SmartPowerplugEndpoint.Models

    let header name value (next: HttpFunc) (ctx: HttpContext) =
        ctx.TryGetRequestHeader name
        |> Option.map ((=) value)
        |> Option.defaultValue true
        |> function
            | true -> next ctx
            | false -> setStatusCode 400 earlyReturn ctx
    
    let allowContent mime (next: HttpFunc) (ctx: HttpContext) =
        ctx.TryGetRequestHeader "Content-Type"
        |> Option.map (fun s -> s.Split(';').[0] = mime)
        |> Option.defaultValue false
        |> function
            | true -> next ctx
            | false -> setStatusCode 413 earlyReturn ctx
    
    let allowAccept mimeType (mimeSubtype: string option) (next: HttpFunc) (ctx: HttpContext) =
        ctx.TryGetRequestHeader "Accept"
        |> Option.map (fun s ->
            s.Split ','
            |> Array.tryFind (fun s2 ->
                match s2.Split(';').[0] with
                | "*/*" -> true
                | s3 ->
                    match s3.Split('/',1) with
                    | [|mType; subtype|] ->
                           mimeType = mType
                        && (mimeSubtype.IsNone || subtype = "*" || mimeSubtype.Value = subtype)
                    | _ -> false )
            |> Option.isSome )
        |> Option.defaultValue true
        |> function
            | true -> next ctx
            | false -> setStatusCode 406 earlyReturn ctx
    
    let apiHeaders (next: HttpFunc) (ctx: HttpContext) =
        allowAccept "application" (Some "json") next ctx
    
    let verbNotAllowed verbs (next: HttpFunc) (ctx: HttpContext) =
        (setStatusCode 405 >=> setHttpHeader "Allow" verbs)
            next ctx
