/// Data access implementation of the Stock component.
module StorageMachine.Stock.Stock

open StorageMachine
open StorageMachine.Bins.Bin
open Stock
open StorageMachine.SimulatedDatabase
open StorageMachine.DataAccess

/// Data access operations of the Stock component implemented using the simulated in-memory DB.
let stockPersistence = { new IStockDataAccess with
    
    member this.RetrieveAllBins () =
        SimulatedDatabase.retrieveBins ()
        |> Set.map (fun binIdentifier ->
            {
                Identifier = binIdentifier
                Content = SimulatedDatabase.retrieveStock () |> Map.tryFind binIdentifier
            }
        )
        |> Set.toList

    member this.StoreBin bin =
           let res = SimulatedDatabase.storeBin bin
           match res with
           | Ok a -> Ok a
           | Error e ->
               match e with
               | BinAlreadyStored -> Error "This bin is already stored!"
}