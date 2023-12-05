using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomProgram
{
    public class GameManager
    {
        public Player Player { get; private set; }
        public Shop Shop { get; private set; }

        public ITimeProvider timeProvider;

        public List<Plot> plots;

        public Texture2D defaultTexture;
        public Texture2D backgroundTexture;
        public Texture2D cowPlotTexture;
        public Texture2D chickenPlotTexture;
        public Texture2D pigPlotTexture;
        public Texture2D sheepPlotTexture;
        public Texture2D goatPlotTexture;
        public Texture2D shopAnimalTexture;
        public Texture2D buyFeedTexture;
        public Texture2D sellMarketTexture;
        public Texture2D inventoryTexture;
        public Texture2D cowFeedTexture;
        public Texture2D pigFeedTexture;
        public Texture2D sheepFeedTexture;
        public Texture2D goatFeedTexture;
        public Texture2D chickenFeedTexture;
        public Texture2D cowTexture;
        public Texture2D chickenTexture;
        public Texture2D pigTexture;
        public Texture2D sheepTexture;
        public Texture2D goatTexture;
        public Texture2D beefTexture;

        public Texture2D milkTexture;
        public Texture2D porkTexture;
        public Texture2D goatMeatTexture;
        public Texture2D goatMilkTexture;
        public Texture2D chickenMeatTexture;
        public Texture2D eggTexture;
        public Texture2D woolTexture;
        public Texture2D lambTexture;
        public GameManager(string playerName)
        {
            timeProvider = new RealTimeProvider();
            Player = new Player(playerName);
            Shop = new Shop();
            plots = new List<Plot>();
            InitializeGame();
        }

        private void InitializeGame()
        {
            InitializePlayer();
            InitializeShop();
            InitializePlot();
            // Any other game setup can go here.
        }

        private void InitializePlayer()
        {
            // Set up the player's starting resources, inventory, etc.
            Player.Coins = 10000; // Starting coins
            Player.Reputation = 100;                    

        }

        private void InitializeShop()
        {
            // Use the stock from the Shop constructor to populate items for sale
            Shop.PopulateWithStartingStock(timeProvider);
            // More shop setup...
        }
        private void InitializePlot()
        {
            int plotSize = 150;

            Plot cowPlot = new Plot(PlotType.CowPlot, Player) { Size = plotSize };
            plots.Add(cowPlot);
            Plot chickenPlot = new Plot(PlotType.ChickenPlot, Player) { Size = plotSize };
            plots.Add(chickenPlot);
            Plot pigPlot = new Plot(PlotType.PigPlot, Player) { Size = plotSize };
            plots.Add(pigPlot);
            Plot goatPlot = new Plot(PlotType.GoatPlot, Player) { Size = plotSize };
            plots.Add(goatPlot);
            Plot sheepPlot = new Plot(PlotType.SheepPlot, Player) { Size = plotSize };
            plots.Add(sheepPlot);
            Player.Plots.Add(cowPlot);
            Player.Plots.Add(chickenPlot);
            Player.Plots.Add(pigPlot);
            Player.Plots.Add(goatPlot);
            Player.Plots.Add(sheepPlot);
        }
        public void LoadTextures()
        {
             defaultTexture = Raylib.LoadTexture("assets/BackGroundGrass.png");
             backgroundTexture = Raylib.LoadTexture("assets/BackGroundGrass.png");
             cowPlotTexture = Raylib.LoadTexture("assets/CowPlot.png");
             chickenPlotTexture = Raylib.LoadTexture("assets/ChickenPlot.png");
             pigPlotTexture = Raylib.LoadTexture("assets/PigPlot.png");
             goatPlotTexture = Raylib.LoadTexture("assets/GoatPlot.png");
             sheepPlotTexture = Raylib.LoadTexture("assets/SheepPlot.png");
             shopAnimalTexture = Raylib.LoadTexture("assets/ShopAnimal.png");
             buyFeedTexture = Raylib.LoadTexture("assets/BuyFeed.png");
             sellMarketTexture = Raylib.LoadTexture("assets/SellMarket.png");
             inventoryTexture = Raylib.LoadTexture("assets/Inventory.png");
             cowFeedTexture = Raylib.LoadTexture("assets/Radish.png");
             pigFeedTexture = Raylib.LoadTexture("assets/Taro.png");
             sheepFeedTexture = Raylib.LoadTexture("assets/Pea.png");
             goatFeedTexture = Raylib.LoadTexture("assets/Carrot.png");
             chickenFeedTexture = Raylib.LoadTexture("assets/Corn.png");
             cowTexture = Raylib.LoadTexture("assets/Cow.png");
             chickenTexture = Raylib.LoadTexture("assets/Chicken.png");
             pigTexture = Raylib.LoadTexture("assets/Pig.png");
             goatTexture = Raylib.LoadTexture("assets/Goat.png");
             sheepTexture = Raylib.LoadTexture("assets/Sheep.png");

            beefTexture = Raylib.LoadTexture("assets/Beef.png");
            milkTexture = Raylib.LoadTexture("assets/CowMilk.png");
            porkTexture = Raylib.LoadTexture("assets/Pork.png");
            goatMeatTexture = Raylib.LoadTexture("assets/GoatMeat.png");
            goatMilkTexture = Raylib.LoadTexture("assets/GoatMilk.png");
            eggTexture = Raylib.LoadTexture("assets/Egg.png");
            chickenMeatTexture = Raylib.LoadTexture("assets/ChickenMeat.png");
            woolTexture = Raylib.LoadTexture("assets/Wool.png");
            lambTexture = Raylib.LoadTexture("assets/Lamb.png");

        }

        public void UnloadTextures()
        {
            // Unload textures
            Raylib.UnloadTexture(defaultTexture);
            Raylib.UnloadTexture(backgroundTexture);
            Raylib.UnloadTexture(cowPlotTexture);
            Raylib.UnloadTexture(chickenPlotTexture);
            Raylib.UnloadTexture(pigPlotTexture);
            Raylib.UnloadTexture(goatPlotTexture);
            Raylib.UnloadTexture(sheepPlotTexture);
            Raylib.UnloadTexture(shopAnimalTexture);
            Raylib.UnloadTexture(buyFeedTexture);
            Raylib.UnloadTexture(sellMarketTexture);
            Raylib.UnloadTexture(inventoryTexture);
            Raylib.UnloadTexture(cowFeedTexture);
            Raylib.UnloadTexture(goatFeedTexture);
            Raylib.UnloadTexture(chickenFeedTexture);
            Raylib.UnloadTexture(sheepFeedTexture);
            Raylib.UnloadTexture(pigFeedTexture);
            Raylib.UnloadTexture(cowTexture);
            Raylib.UnloadTexture(goatTexture);
            Raylib.UnloadTexture(chickenTexture);
            Raylib.UnloadTexture(sheepTexture);
            Raylib.UnloadTexture(pigTexture);

            Raylib.UnloadTexture(beefTexture);
            Raylib.UnloadTexture(milkTexture);
            Raylib.UnloadTexture(porkTexture);
            Raylib.UnloadTexture(goatMeatTexture);
            Raylib.UnloadTexture(goatMilkTexture);
            Raylib.UnloadTexture(chickenMeatTexture);
            Raylib.UnloadTexture(eggTexture);
            Raylib.UnloadTexture(woolTexture);
            Raylib.UnloadTexture(lambTexture);
        }

        public void Update()
        {
            // Update logic for each plot
            foreach (var plot in Player.Plots)
            {
                plot.UpdateAnimals();
            }
        }

        public void Render()
        {
            
        }
    }

}
