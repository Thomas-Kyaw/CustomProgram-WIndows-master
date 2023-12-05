namespace CustomProgram
{
    /// <summary>
    /// interface for Buyable items
    /// </summary>
    public interface IBuyable
    {
        /// <summary>
        /// name of the item
        /// </summary>
        string name { get; set;}
        /// <summary>
        /// Buying price of the item
        /// </summary>
        float purchasePrice { get; set;}
    }
    /// <summary>
    /// interface for Sellable items
    /// </summary>
    public interface ISellable
    {
        /// <summary>
        /// name of the item
        /// </summary>
        string name { get; set;}
        /// <summary>
        /// Selling price of the item
        /// </summary>
        float sellPrice { get; set;}
    }
}
