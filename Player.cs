using Raylib_cs;
using System.Numerics;

namespace CustomProgram
{
    public class Player
    {
        /// <summary>
        /// name of the player
        /// </summary>
        private string name;
        /// <summary>
        /// Player's inventory
        /// </summary>
        private Inventory inventory;
        /// <summary>
        /// Player's coins
        /// </summary>
        private float coins;
        /// <summary>
        /// Player's reputation
        /// </summary>
        public int reputation;
        /// <summary>
        /// Plots owned by player
        /// </summary>
        private List<Plot> plots;
        /// <summary>
        /// Constructor for Player
        /// </summary>
        /// <param name="_name"></param>
        public Player(string _name)
        {
            name = _name;
            inventory = new Inventory();
            coins = 10000; // Initial amount of coins
            reputation = 100; // Initial reputation
            plots = new List<Plot>();
        }
        /// <summary>
        /// Read Only property for Inventory
        /// </summary>
        public Inventory Inventory
        {
            get{return inventory;}
        }
        /// <summary>
        /// Property for coins
        /// </summary>
        public float Coins
        {
            get{return coins;}
            set{coins = value;}
        }
        /// <summary>
        /// property for reputation
        /// </summary>
        public int Reputation
        {
            get{return reputation;}
            set{reputation = value;}
        }
        /// <summary>
        /// read only property for plots
        /// </summary>
        public List<Plot> Plots
        { get { return plots;} }
        /// <summary>
        /// adding a plot to player's plost
        /// </summary>
        /// <param name="plot"></param>
        public void AddPlot(Plot plot)
        {
            plots.Add(plot);
        }
        /// <summary>
        /// removing a plot to player's plot only for testing purposes
        /// </summary>
        /// <param name="plot"></param>
        //test purpose
        public void RemovePlot(Plot plot)
        {
            plots.Remove(plot);
        }

        
    }
}
