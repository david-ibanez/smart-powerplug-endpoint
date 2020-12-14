namespace SmartPowerplugEndpoint.Handlers.Api

open System.IO

open Microsoft.AspNetCore.Http
open FSharp.Control.Tasks.V2.ContextInsensitive
open Giraffe
open SmartPowerplugEndpoint.Models
open MongoDB.Driver

module Led =

    let db = MongoClient("mongodb+srv://root:eiCxU8nRYcmYLYyF@cluster0.ibzwj.mongodb.net/proto?retryWrites=true&w=majority").GetDatabase "proto"

    let get next ctx =
        db.GetCollection<Led>("leds").Find(Builders<Led>.Filter.Eq((fun x -> x.Id), 0)).Limit(1).ToEnumerable()
        |> Seq.toList
        |> function
            | [s] when s.On -> text "1"
            | [s] -> text "0"
            | _ -> text "404"
        <| next <| ctx
    
    let post next (ctx: HttpContext) =
        match ctx.TryGetQueryStringValue "set" with
        | Some "1" ->
            db.GetCollection<Led>("leds").InsertOne({Id = 0; On = true})
            text "OK"
        | Some "0" ->
            db.GetCollection<Led>("leds").InsertOne({Id = 0; On = false})
            text "OK"
        | Some _ -> text "NOK"
        | None -> text "NOK"
        <| next <| ctx
    
    let put next (ctx: HttpContext) =
        match ctx.TryGetQueryStringValue "set" with
        | Some "1" ->
            db.GetCollection<Led>("leds").UpdateOne(
                Builders<Led>.Filter.Eq((fun x -> x.Id), 0),
                Builders<Led>.Update.Set((fun x -> x.On), true))
            |> ignore
            text "OK"
        | Some "0" ->
            db.GetCollection<Led>("leds").UpdateOne(
                Builders<Led>.Filter.Eq((fun x -> x.Id), 0),
                Builders<Led>.Update.Set((fun x -> x.On), false))
            |> ignore
            text "OK"
        | Some _ -> text "NOK"
        | None -> text "NOK"
        <| next <| ctx
