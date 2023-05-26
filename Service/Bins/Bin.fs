module StorageMachine.Bins.Bin

open System.Runtime.CompilerServices
open Microsoft.AspNetCore.Http
open Giraffe
open StorageMachine.DataAccess
open Thoth.Json.Giraffe
open Thoth.Json.Net

let createBin (next: HttpFunc) (ctx: HttpContext) =
    task {
        let! inputBin = ThothSerializer.ReadBody ctx Serialization.decoderBin
        match inputBin with
        | Error e ->
            return! RequestErrors.badRequest (text e) earlyReturn ctx
        | Ok bin ->
            let dataAccess = ctx.GetService<IStockDataAccess> ()
            return! match Bin.storeBin bin dataAccess with
                    | Ok _ -> Successful.ok ( text $"Bin with id '{bin.Identifier}' has been added" ) next ctx
                    | Error e -> RequestErrors.notAcceptable (text e) earlyReturn ctx
            
    }
    
let handlers : HttpHandler =
    choose [
        POST >=> route "/bins" >=> createBin
    ]