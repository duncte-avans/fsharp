module StorageMachine.Bins.Serialization

open StorageMachine.Bins.Bin
open Thoth.Json.Net
open StorageMachine
open Common

/// JSON serialization of a bin.
let encoderBin : Encoder<Bin> = fun bin ->
    Encode.object [
        "binIdentifier", (let (BinIdentifier identifier) = bin.Identifier in Encode.string identifier)
        "content", (Encode.option (fun (PartNumber partNumber) -> Encode.string partNumber) bin.Content)
    ]

/// JSON deserialization of a bin identifier.
let decoderBinIdentifier : Decoder<BinIdentifier> =
    Decode.string
    |> Decode.andThen (fun s ->
        match BinIdentifier.make s with
        | Ok binIdentifier -> Decode.succeed binIdentifier
        | Error validationMessage -> Decode.fail validationMessage
    )

let decoderBinContent : Decoder<PartNumber> =
    Decode.string
    |> Decode.andThen (fun s ->
        match PartNumber.make s with
        | Ok pn -> Decode.succeed pn
        | Error validationMessage -> Decode.fail validationMessage
    )

// JSON deserialization of a bin.
let decoderBin : Decoder<Bin> =
    Decode.object (fun obj ->
            {
                Bin.Identifier = obj.Required.Field "binIdentifier" decoderBinIdentifier
                Content = obj.Optional.Field "content" decoderBinContent
            }
        )
    
// EXERCISE: Is this decoder in the right place (in the architecture) here?