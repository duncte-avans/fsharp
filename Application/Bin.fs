module StorageMachine.Bins.Bin

open StorageMachine.Bins.Bin
open StorageMachine.DataAccess
open StorageMachine.Stock.Stock

let storeBin (bin: Bin) (dataAccess : IStockDataAccess) =
    dataAccess.StoreBin bin