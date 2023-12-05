using Raylib_cs;
using System.Numerics;

namespace CustomProgram
{
    public class Inventory
    {
        /// <summary>
        /// list for buyable items - Feed and animal are buyable
        /// </summary>
        private List<IBuyable> buyableItems; // Feed
        /// <summary>
        /// list for sellable items - Produce are sellable
        /// </summary>
        private List<ISellable> sellableItems; // Produce
        /// <summary>
        /// Constructor for Inventory
        /// </summary>
        public Inventory()
        {
            buyableItems = new List<IBuyable>();
            sellableItems = new List<ISellable>();
        }
        /// <summary>
        /// method for adding buyable items
        /// </summary>
        /// <param name="item"></param>
        public void AddBuyableItem(IBuyable item)
        {
            // Add item to the list of buyable items from the buyable list
            buyableItems.Add(item);
        }
        /// <summary>
        /// method for adding sellable items from the sellable list
        /// </summary>
        /// <param name="item"></param>
        public void AddSellableItem(ISellable item)
        {
            // Add item to the list of sellable items
            sellableItems.Add(item);
        }
        /// <summary>
        /// method for removing buyable items to the buyable list
        /// </summary>
        /// <param name="item"></param>
        public void RemoveBuyableItem(IBuyable item)
        {
            // Add item to the list of buyable items
            buyableItems.Remove(item);
        }
        /// <summary>
        /// method for removing sellable items from the sellable list
        /// </summary>
        /// <param name="item"></param>
        public void RemoveSellableItem(ISellable item)
        {
            // Add item to the list of sellable items
            sellableItems.Remove(item);
        }
        /// <summary>
        /// Return the quantity of the item in the inventory
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int CheckBuyableQuantity(IBuyable item)
        {
            // Return the quantity of the item in the inventory
            return buyableItems.Count; 
        }
        /// <summary>
        /// Return the quantity of the specific type of item in the inventory
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int CheckSellableQuantity(ISellable item)
        {
            // Return the quantity of the specific type of item in the inventory
            return sellableItems.Count(i => i.name == item.name);
        }
        /// <summary>
        /// Remove the specified quantity of the specific type of item from the inventory
        /// </summary>
        /// <param name="item"></param>
        /// <param name="quantity"></param>
        public void RemoveSellableItem(ISellable item, int quantity)
        {
            // Remove the specified quantity of the specific type of item from the inventory
            for (int q = 0; q < quantity; q++)
            {
                var itemToRemove = sellableItems.FirstOrDefault(i => i.name == item.name);
                if (itemToRemove != null)
                {
                    sellableItems.Remove(itemToRemove);
                }
            }
        }
        /// <summary>
        /// Read only property for sellableitems list
        /// </summary>
        public List<ISellable> SellableItems
        {
            get{return sellableItems;}
        }
        /// <summary>
        /// Read only property for buyableitems list
        /// </summary>
        public List<IBuyable> BuyableItems
        {
            get{return buyableItems;}
        }  
    }
}
