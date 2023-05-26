/// Serialization and deserialization for types/values of the Stock component.
module StorageMachine.Stock.Serialization

open Thoth.Json.Net
open StorageMachine
open Common
open Stock
    
/// JSON deserialization of a part number.
let decoderPartNumber : Decoder<PartNumber> =
    Decode.string
    |> Decode.andThen (fun s ->
        match PartNumber.make s with
        | Ok partNumber -> Decode.succeed partNumber
        | Error validationMessage -> Decode.fail validationMessage
    )

/// JSON serialization of a stock product.
let encoderProduct : Encoder<Product> = fun product ->
    Encode.object [
        "part_number", match product with
                        // Why does this work?
                        // Why does it not work without the argument?
                        | Product(PartNumber partNumber) -> Encode.string partNumber
    ]

/// JSON serialization of a complete products overview.
let encoderProductsOverview : Encoder<ProductsOverview> = fun productsOverview ->
    Encode.seq [
        for (product, quantity) in productsOverview do
            yield Encode.object [
                "product", encoderProduct product
                "total", Encode.int quantity
            ]
    ]