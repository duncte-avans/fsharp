module StorageMachine.DataAccess

open StorageMachine.Bins.Bin

/// Defines data access operations for stock functionality.
type IStockDataAccess =

    /// Retrieve all bins currently stored in the Storage Machine.
    abstract RetrieveAllBins : unit -> List<Bin>
    
    // Store a bin in the database
    abstract StoreBin : Bin -> Result<Unit, string>

