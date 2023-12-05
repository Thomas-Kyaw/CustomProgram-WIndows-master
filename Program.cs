using CustomProgram;
using Raylib_cs;
using System.Numerics;
using System.Collections.Generic;
using static System.Reflection.Metadata.BlobBuilder;
using System.Linq;
using System.ComponentModel;
using static System.Net.Mime.MediaTypeNames;

namespace CustomProgram
{
    public class Game
    {
        static GameState currentState = GameState.MainGame;
        static GameManager gameManager;
        static Vector2[] plotPositions;
        static int plotSize = 150;
        static float scrollY = 0;
        static float scrollYSellProduceOverlay = 0;
        static float scrollYFeedOverlay = 0;
        static float scrollYInventory = 0;

        // Constants for button sizes and positions
        const int buttonWidth = 20;
        const int buttonHeight = 20;
        const int sellButtonWidth = 60;
        const int sellButtonHeight = 20;
        const int buttonSpacing = 5; // Space between buttons
        const int infoHeight = 80; // Height of the info box to fit text and buttons

        static bool isGameRunning = true;

        public static void Main()
        {
            gameManager = new GameManager("PlayerName");
            const int screenWidth = 650;
            const int screenHeight = 600;
            Raylib.InitWindow(screenWidth, screenHeight, "Farm Management Game");
            Raylib.SetTargetFPS(60);

            Raylib.SetExitKey(KeyboardKey.KEY_NULL);

            gameManager.LoadTextures();
            Color textColor = Color.WHITE;
            
            plotSize = 150;
            int plotSpacing = 20;
            int buttonSize = 50;
            int verticalSpacing = 10;

            int offsetX = (screenWidth - (3 * plotSize + 2 * plotSpacing)) / 2;
            int offsetY = (screenHeight - (3 * plotSize + 2 * plotSpacing)) / 2;

            plotPositions = new Vector2[9];
            for (int i = 0; i < 9; i++)
            {
                int row = i / 3;
                int col = i % 3;
                plotPositions[i] = new Vector2(
                    offsetX + col * (plotSize + plotSpacing),
                    offsetY + row * (plotSize + plotSpacing)
                );
            }

            Vector2[] buttonPositions = new Vector2[3];
            for (int i = 0; i < 3; i++)
            {
                buttonPositions[i] = new Vector2(
                    (i % 2 == 0) ? 10 : (screenWidth - buttonSize - 10),
                    offsetY + i * (buttonSize + verticalSpacing)
                );
            }
            while (!Raylib.WindowShouldClose() && isGameRunning)
            {
                gameManager.Update();
                string text = $"Coins: {gameManager.Player.Coins}";
                string text1 = $"Reputation: {gameManager.Player.Reputation}";
                // Check if the Escape key is pressed to return to the main game state
                if (Raylib.IsKeyPressed(KeyboardKey.KEY_ESCAPE))
                {
                    currentState = GameState.MainGame;
                }
                // Check for game over condition
                if (gameManager.Player.Reputation <= 0)
                {
                    currentState = GameState.GameOver;
                }
                if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_LEFT_BUTTON))
                {
                    Vector2 mousePoint = Raylib.GetMousePosition();
                    if (currentState == GameState.MainGame)
                    {
                        for (int i = 0; i < plotPositions.Length; i++)
                        {
                            if (Raylib.CheckCollisionPointRec(mousePoint, new Rectangle(plotPositions[i].X, plotPositions[i].Y, plotSize, plotSize)))
                            {
                                currentState = (GameState)(i+1);
                                
                                break;
                            }
                        }

                        for (int i = 0; i < buttonPositions.Length; i++)
                        {
                            if (Raylib.CheckCollisionPointRec(mousePoint, new Rectangle(buttonPositions[i].X, buttonPositions[i].Y, buttonSize, buttonSize)))
                            {
                                currentState = (GameState)(i + 10);                               
                            }
                        }
                    }
                }

                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.RAYWHITE);

                if (currentState == GameState.MainGame)
                {
                    // Draw the background texture tiled
                    for (int y = 0; y < screenHeight; y += gameManager.backgroundTexture.Height)
                    {
                        for (int x = 0; x < screenWidth; x += gameManager.backgroundTexture.Width)
                        {
                            Raylib.DrawTexture(gameManager.backgroundTexture, x, y, Color.WHITE);
                        }
                    }
                    
                    Texture2D[] textures = { gameManager.cowPlotTexture, gameManager.chickenPlotTexture, gameManager.pigPlotTexture, gameManager.goatPlotTexture, gameManager.sheepPlotTexture, gameManager.shopAnimalTexture, gameManager.buyFeedTexture, gameManager.sellMarketTexture, gameManager.inventoryTexture };
                    string[] plotNames = { "Cow", "Chicken", "Pig", "Goat", "Sheep", "Shop Animals", "Buy Feed", "Sell Produce", "Inventory" };

                    for (int i = 0; i < plotPositions.Length; i++)
                    {
                        Raylib.DrawTextureEx(textures[i], plotPositions[i], 0.0f, plotSize / (float)textures[i].Width, Color.WHITE);
                        Raylib.DrawText(text, 480, 10, 20, Color.BLACK);
                        Raylib.DrawText(text1, 480, 30, 20, Color.BLACK);
                        Vector2 textSize = Raylib.MeasureTextEx(Raylib.GetFontDefault(), plotNames[i], 20, 1);
                        Raylib.DrawText(plotNames[i],
                                        (int)(plotPositions[i].X + (plotSize - textSize.X) / 2),
                                        (int)(plotPositions[i].Y + plotSize - textSize.Y - 10),
                                        20,
                                        textColor);
                    }

                    string[] buttonNames = { "Bag", "Animal", "Feed" };
                    Color[] buttonColors = { Color.GRAY, Color.LIGHTGRAY, Color.DARKGRAY };
                    for (int i = 0; i < buttonPositions.Length; i++)
                    {
                        Raylib.DrawRectangle((int)buttonPositions[i].X, (int)buttonPositions[i].Y, buttonSize, buttonSize, buttonColors[i]);
                        Vector2 textSize = Raylib.MeasureTextEx(Raylib.GetFontDefault(), buttonNames[i], 20, 1);
                        Raylib.DrawText(buttonNames[i],
                                        (int)(buttonPositions[i].X + (buttonSize - textSize.X) / 2),
                                        (int)(buttonPositions[i].Y + (buttonSize - textSize.Y) / 2),
                                        20,
                                        textColor);
                    }

                    RenderPlotsAndAnimals();
                }
                else if(currentState == GameState.GameOver)
                {
                    DrawGameOverScreen();
                    if (!isGameRunning) // Check if the game should continue running
                    {
                        break; // Exit the loop if the game is over
                    }
                }
                else
                {
                    // Render the overlay for the current game state
                    DrawOverlay(currentState);
                }

                Raylib.EndDrawing();
            }

            gameManager.UnloadTextures();
            // De-Initialization
            Raylib.CloseWindow();
        }

        static void DrawOverlay(GameState state)
        {
            string text = $"Coins: {gameManager.Player.Coins}";
            string text1 = $"Reputation: {gameManager.Player.Reputation}";
            Raylib.DrawText(text, 480, 10, 20, Color.BLACK);
            Raylib.DrawText(text1, 480, 30, 20, Color.BLACK);
            switch (state)
            {
                case GameState.Cow:
                    DrawAnimalFeedOverlay(PlotType.CowPlot ,gameManager.Player);
                    break;
                case GameState.Chicken:
                    DrawAnimalFeedOverlay(PlotType.ChickenPlot, gameManager.Player);
                    break;
                case GameState.Pig:
                    DrawAnimalFeedOverlay(PlotType.PigPlot, gameManager.Player);
                    break;
                case GameState.Goat:
                    DrawAnimalFeedOverlay(PlotType.GoatPlot, gameManager.Player);
                    break;
                case GameState.Sheep:
                    DrawAnimalFeedOverlay(PlotType.SheepPlot, gameManager.Player);
                    break;
                case GameState.ShopAnimals:
                    // Render shop animals overlay
                    DrawShopAnimalsOverlay(gameManager.Shop, gameManager.Player);
                    break;
                case GameState.BuyFeed:
                    // Render buy feed overlay
                    DrawShopFeedOverlay(gameManager.Shop, gameManager.Player);
                    break;
                case GameState.SellProduce:
                    DrawSellProduceOverlay(gameManager.Player);
                    break;
                case GameState.Inventory:
                    DrawInventoryOverlay(gameManager.Player);
                    break;
                // TODO - add the last 3 stages and reputation text
                case GameState.Bag:
                    DrawInventoryOverlay(gameManager.Player);
                    break;
                case GameState.Feed:
                    FeedButtonOverlay(gameManager.Player);
                    break;
                case GameState.Animal:
                    AnimalButtonOverlay(gameManager.Player);
                    break;
                default:
                    break;
            }
        }
        static void RenderPlotsAndAnimals()
        {
            // Define the number of animals per row based on the plot size and desired animal size
            int animalsPerRow = 4; 

            for (int i = 0; i < gameManager.Player.Plots.Count; i++)
            {
                Plot currentPlot = gameManager.Player.Plots[i];
                Vector2 plotPosition = plotPositions[i]; // The starting position of each plot

                List<Animal> animalsInPlot = currentPlot.Animals;

                // Define the size of each animal within the plot
                float animalSize = plotSize / (animalsPerRow + 1.0f); 

                // Calculate the horizontal and vertical spacing between the animals
                float spacing = animalSize / 4.0f; // Give some space between animals

                for (int j = 0; j < animalsInPlot.Count; j++)
                {
                    int row = j / animalsPerRow;
                    int col = j % animalsPerRow;

                    // Calculate the position for each animal
                    Vector2 animalPosition = new Vector2(
                        plotPosition.X + col * (animalSize + spacing) + spacing,
                        plotPosition.Y + row * (animalSize + spacing) + spacing
                    );

                    Texture2D animalTexture = GetTextureForAnimal(animalsInPlot[j]);

                    // Draw the animal at the calculated position, scaled to the desired size
                    Raylib.DrawTextureEx(
                        animalTexture,
                        animalPosition,
                        0.0f, // No rotation
                        animalSize / 8.0f, // Scale factor to resize the 8x8 texture to the desired size
                        Color.WHITE
                    );
                }
            }
        }
        static void DrawGameOverScreen()
        {
            // Darken the background
            Raylib.DrawRectangle(0, 0, Raylib.GetScreenWidth(), Raylib.GetScreenHeight(), new Color(0, 0, 0, 180));

            // Draw "Game Over" text in the center of the screen
            string gameOverText = "Game Over";
            int fontSize = 40;
            Color textColor = Color.RED;
            Vector2 textSize = Raylib.MeasureTextEx(Raylib.GetFontDefault(), gameOverText, fontSize, 1);
            Raylib.DrawText(
                gameOverText,
                Raylib.GetScreenWidth() / 2 - (int)(textSize.X / 2),
                Raylib.GetScreenHeight() / 2 - (int)(textSize.Y / 2),
                fontSize,
                textColor
            );

            // Optionally, provide a prompt to restart or quit
            string restartText = "Press [Q] to Quit";
            int smallFontSize = 20;
            Vector2 restartTextSize = Raylib.MeasureTextEx(Raylib.GetFontDefault(), restartText, smallFontSize, 1);
            Raylib.DrawText(
                restartText,
                Raylib.GetScreenWidth() / 2 - (int)(restartTextSize.X / 2),
                Raylib.GetScreenHeight() / 2 - (int)(textSize.Y / 2) + 50,
                smallFontSize,
                Color.WHITE
            );

            if (Raylib.IsKeyPressed(KeyboardKey.KEY_Q))
            {
                isGameRunning = false;
            }
        }
        static void DrawAnimalFeedOverlay(PlotType plotType, Player player)
        {
            // Overlay background
            Raylib.DrawRectangle(0, 0, Raylib.GetScreenWidth(), Raylib.GetScreenHeight(), new Color(0, 0, 0, 128));
            int extraTopPadding = 50; // Extra space to avoid overlap with the close button
            // Fetch all animals from the selected plot type
            List<Animal> animals = player.Plots.FirstOrDefault(p => p.Type == plotType)?.Animals;

            // Ensure there are animals to display
            if (animals != null && animals.Any())
            {
                // Handle mouse wheel scroll for the feed overlay
                float mouseWheelMove = Raylib.GetMouseWheelMove();
                scrollYFeedOverlay -= mouseWheelMove * 20; // Scroll speed, adjust as necessary

                // Calculate the total content height and padding
                int itemHeight = 100; // Height of each item (animal + feed button)
                int padding = 10; // Padding between items
                float contentHeight = animals.Count * (itemHeight + padding) + padding + extraTopPadding;// Total height of the content

                // Calculate maxScroll ensuring it's not less than 0
                float maxScroll = Math.Max(0, contentHeight - Raylib.GetScreenHeight());

                // Clamp scrollY to prevent scrolling beyond the content
                scrollYFeedOverlay = Math.Clamp(scrollYFeedOverlay, 0, maxScroll);

                // Start Y position, adjusted by the current scroll position and extra top padding
                int startY = padding + extraTopPadding - (int)scrollYFeedOverlay;

                // Draw each animal's information and feed button
                foreach (var animal in animals)
                {
                    DrawAnimalInfoAndFeedButton(animal, padding, startY, player);
                    startY += itemHeight + padding; // Move to the next item position
                }
            }
            else
            {
                Raylib.DrawText("No animals in this plot type.", 50, extraTopPadding + 50, 20, Color.WHITE);
            }

            DrawCloseButton();
        }
        static void DrawAnimalInfoAndFeedButton(Animal animal, int startX, int startY, Player player)
        {
            // Background for animal info
            Rectangle infoRect = new Rectangle(startX, startY, 400, 100); // Adjust width and height as needed
            Raylib.DrawRectangleRec(infoRect, new Color(169, 169, 169, 255)); // Light grey background
            Raylib.DrawRectangleLinesEx(infoRect, 2, Color.BLACK); // Black border for the info rectangle

            // Animal information text
            string infoText = $"{animal.name} - Health: {animal.Health}, Hunger: {animal.Hunger}";
            Raylib.DrawText(infoText, startX + 10, startY + 10, 20, Color.WHITE); // Adjust text positioning as needed

            // Animal Image
            Texture2D texture = GetTextureForAnimal(animal);
            Rectangle sourceRect = new Rectangle(0, 0, texture.Width, texture.Height); // Use the full texture
            float scale = 4.0f; // Scale up the animal image, adjust this value as needed
            Rectangle destRect = new Rectangle(startX + 10, startY + 40, sourceRect.Width * scale, sourceRect.Height * scale);
            Raylib.DrawTexturePro(texture, sourceRect, destRect, new Vector2(0, 0), 0.0f, Color.WHITE);

            // Background for feed button
            Rectangle feedButtonRect = new Rectangle(startX + 250, startY + 40, 80, 30); // Adjust position and size as needed
            Raylib.DrawRectangleRec(feedButtonRect, new Color(0, 128, 0, 255)); // Green background
            Raylib.DrawRectangleLinesEx(feedButtonRect, 2, Color.BLACK); // Black border for the feed button

            // Feed button text
            Raylib.DrawText("Feed", (int)feedButtonRect.X + 20, (int)feedButtonRect.Y + 5, 20, Color.WHITE);

            if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_LEFT_BUTTON) && Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), feedButtonRect))
            {
                // Get the feed type based on the animal
                FeedType requiredFeedType = GetFeedType(animal);

                // Find the feed item in the player's inventory
                Feed feedItem = player.Inventory.BuyableItems.OfType<Feed>().FirstOrDefault(f => f.Type == requiredFeedType);

                // Check if the feed item is available in the inventory
                if (feedItem != null)
                {
                        // Feed the animal
                        animal.Feed(requiredFeedType);
                        player.Inventory.RemoveBuyableItem(feedItem);
                }
            }
        }
        static FeedType GetFeedType(Animal animal)
        {
            if (animal is Cow)
            {
                return FeedType.CowFeed;
            }
            else if (animal is Chicken)
            {
                return FeedType.ChickenFeed;
            }
            else if (animal is Goat)
            {
                return FeedType.GoatFeed;
            }
            else if (animal is Sheep)
            {
                return FeedType.SheepFeed;
            }
            else
                return FeedType.PigFeed;
        }

        // Function to draw the overlay for shopping animals
        static void DrawShopAnimalsOverlay(Shop shop, Player player)
        {
            // Handle mouse wheel scroll (reversed direction)
            float mouseWheelMove = Raylib.GetMouseWheelMove();
            scrollY -= mouseWheelMove * 100; // Now scrolling up moves up, and down moves down

            // Calculate the total content height
            float contentHeight = shop.itemsForSale.OfType<Animal>().Count() * 70;

            // Add extra space equal to one item height for padding at the bottom
            float extraPadding = 70;

            // Clamp scrollY to prevent scrolling beyond the content
            float maxScroll = Math.Max(0, contentHeight + extraPadding - Raylib.GetScreenHeight());
            scrollY = Math.Clamp(scrollY, -extraPadding, maxScroll);

            // Overlay background
            Raylib.DrawRectangle(0, 0, Raylib.GetScreenWidth(), Raylib.GetScreenHeight(), new Color(0, 0, 0, 128));

            int startY = 50 - (int)scrollY;
            int itemHeight = 60; // Increased height for better spacing
            int buttonWidth = 400; // Increased button width for better layout
            int purchaseButtonWidth = 100; // Width for the buy button
            int padding = 10; // Padding inside the button for layout

            foreach (var item in shop.itemsForSale.OfType<Animal>().ToList())
            {
                Texture2D texture = GetTextureForAnimal(item); // Method to retrieve the correct texture for the animal
                                                                           // Since we're only using the first frame, we can define the source rectangle for the first sprite
                Rectangle sourceRect = new Rectangle(0, 0, 8, 8); // Define the source rectangle for the first sprite (8x8)

                float scale = (itemHeight - 2 * padding) / sourceRect.Height; // Calculate scale to fit in button

                string itemDisplayText = $"{item.GetType().Name} - ${item.purchasePrice}";
                int textX = (int)(50 + padding + sourceRect.Width * scale + padding); // Position text after the texture

                Rectangle buttonRect = new Rectangle(50, startY, buttonWidth, itemHeight);
                Rectangle purchaseButtonRect = new Rectangle(buttonRect.X + buttonWidth - purchaseButtonWidth, startY, purchaseButtonWidth, itemHeight);
                Vector2 texturePosition = new Vector2(50 + padding, startY + padding); // Position texture inside the button

                if (startY + itemHeight > 0 && startY < Raylib.GetScreenHeight() || mouseWheelMove < 0)
                {
                    Raylib.DrawRectangleRec(buttonRect, Color.BLUE);
                    Raylib.DrawText(itemDisplayText, textX, startY + padding, 20, Color.WHITE);

                    // Draw only the first sprite from the sprite sheet
                    Rectangle destRect = new Rectangle(texturePosition.X, texturePosition.Y, sourceRect.Width * scale, sourceRect.Height * scale); // Destination rectangle for scaled sprite
                    Raylib.DrawTexturePro(texture, sourceRect, destRect, new Vector2(0, 0), 0.0f, Color.WHITE);

                    if (player.Coins >= item.purchasePrice)
                    {
                        Raylib.DrawRectangleRec(purchaseButtonRect, Color.GREEN);
                        int buyTextX = (int)(purchaseButtonRect.X + (purchaseButtonWidth - Raylib.MeasureText("Buy", 20)) / 2f);
                        int buyTextY = startY + (itemHeight - Raylib.MeasureText("Buy", 20)) / 2;
                        Raylib.DrawText("Buy", buyTextX, buyTextY, 20, Color.WHITE);

                        if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_LEFT_BUTTON) && Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), purchaseButtonRect))
                        {
                            shop.BuyItem(item, player, 1);
                            shop.itemsForSale.Remove(item); // Remove the purchased item
                        }
                    }
                }
                
                startY += itemHeight + padding; // Increase spacing for next item
            }

            DrawCloseButton();
        }

        static void AnimalButtonOverlay(Player player)
        {
            // Overlay background
            Raylib.DrawRectangle(0, 0, Raylib.GetScreenWidth(), Raylib.GetScreenHeight(), new Color(0, 0, 0, 128));

            // Combine all animals from all plots into a single list
            List<Animal> allAnimals = player.Plots.SelectMany(p => p.Animals).ToList();

            // Handle mouse wheel scroll for the overlay
            float mouseWheelMove = Raylib.GetMouseWheelMove();
            scrollYInventory -= mouseWheelMove * 100; // Scroll speed, adjust as necessary

            // Calculate the total content height and padding
            int itemHeight = 100; // Height of each item (animal info)
            int padding = 10; // Padding between items
            int extraTopPadding = 50; // Extra space to avoid overlap with the close button
            float contentHeight = allAnimals.Count * (itemHeight + padding) + padding + extraTopPadding; // Total height of the content

            // Calculate maxScroll ensuring it's not less than 0
            float maxScroll = Math.Max(0, contentHeight - Raylib.GetScreenHeight());

            // Clamp scrollY to prevent scrolling beyond the content
            scrollYInventory = Math.Clamp(scrollYInventory, 0, maxScroll);

            // Start Y position, adjusted by the current scroll position
            int startY = padding + extraTopPadding - (int)scrollYInventory;

            // Draw each animal's information
            foreach (var animal in allAnimals)
            {
                DrawAnimalInfo(animal, padding, startY);
                startY += itemHeight + padding; // Move to the next item position
            }

            // Check if there are no animals to display
            if (!allAnimals.Any())
            {
                Raylib.DrawText("No animals available.", 50, extraTopPadding + 50, 20, Color.WHITE);
            }

            DrawCloseButton();
        }

        static void DrawAnimalInfo(Animal animal, int startX, int startY)
        {
            // Background for animal info
            Rectangle infoRect = new Rectangle(startX, startY, 400, 100); // Adjust width and height as needed
            Raylib.DrawRectangleRec(infoRect, new Color(169, 169, 169, 255)); // Light grey background
            Raylib.DrawRectangleLinesEx(infoRect, 2, Color.BLACK); // Black border for the info rectangle

            // Animal information text
            string infoText = $"{animal.name} - Health: {animal.Health}, Hunger: {animal.Hunger}";
            Raylib.DrawText(infoText, startX + 10, startY + 10, 20, Color.WHITE); // Adjust text positioning as needed

            // Animal Image
            Texture2D texture = GetTextureForAnimal(animal);
            Rectangle sourceRect = new Rectangle(0, 0, texture.Width, texture.Height); // Use the full texture
            float scale = 4.0f; // Scale up the animal image, adjust this value as needed
            Rectangle destRect = new Rectangle(startX + 10, startY + 40, sourceRect.Width * scale, sourceRect.Height * scale);
            Raylib.DrawTexturePro(texture, sourceRect, destRect, new Vector2(0, 0), 0.0f, Color.WHITE);
        }


        // Main method to draw the inventory overlay
        static void DrawInventoryOverlay(Player player)
        {
            const int itemHeight = 100;
            const int padding = 10;
            const int topOffset = 50;

            // Set up for scrolling
            float mouseWheelMove = Raylib.GetMouseWheelMove();
            scrollYInventory -= mouseWheelMove * 20; // Adjust scroll speed as necessary

            // Get all items and group them by type
            var sellableItemsGrouped = player.Inventory.SellableItems
                .GroupBy(item => item.name)
                .Select(group => new
                {
                    Name = group.Key,
                    Count = group.Count(),
                    Price = group.First().sellPrice,
                    Texture = GetTextureForItem(group.Key, gameManager) 
                });

            var buyableItemsGrouped = player.Inventory.BuyableItems
                .GroupBy(item => item.name)
                .Select(group => new
                {
                    Name = group.Key,
                    Count = group.Count(),
                    Price = group.First().purchasePrice,
                    Texture = GetTextureForItem(group.Key, gameManager) 
                });

            // Combine both groups for display
            var combinedItemsGrouped = sellableItemsGrouped.Concat(buyableItemsGrouped).ToList();

            // Calculate the total content height
            float contentHeight = combinedItemsGrouped.Count * (itemHeight + padding) + padding;

            // Clamp scrollY to prevent scrolling beyond the content
            float maxScroll = Math.Max(0, contentHeight - Raylib.GetScreenHeight());
            scrollYInventory = Math.Clamp(scrollYInventory, 0, maxScroll);

            // Start Y position, adjusted by the current scroll position
            int startY = padding + topOffset - (int)scrollYInventory;

            // Draw each item's information
            foreach (var item in combinedItemsGrouped)
            {
                DrawItemInfo(item.Name, item.Count, item.Price, item.Texture, 50, startY);
                startY += itemHeight + padding; // Move to the next item position
            }

            DrawCloseButton(); 
        }

        static void FeedButtonOverlay(Player player)
        {
            // Overlay background
            Raylib.DrawRectangle(0, 0, Raylib.GetScreenWidth(), Raylib.GetScreenHeight(), new Color(0, 0, 0, 128));
            int extraTopPadding = 50; // Extra space at the top

            // Combine all animals from all plots into a single list
            List<Animal> allAnimals = player.Plots.SelectMany(p => p.Animals).ToList();

            // Handle mouse wheel scroll for the feed overlay
            float mouseWheelMove = Raylib.GetMouseWheelMove();
            scrollYFeedOverlay -= mouseWheelMove * 100; // Scroll speed, adjust as necessary

            // Calculate the total content height and padding
            int itemHeight = 100; // Height of each item (animal + feed button)
            int padding = 10; // Padding between items
            float contentHeight = allAnimals.Count * (itemHeight + padding) + padding + extraTopPadding; // Total height of the content

            // Calculate maxScroll ensuring it's not less than 0
            float maxScroll = Math.Max(0, contentHeight - Raylib.GetScreenHeight());

            // Clamp scrollY to prevent scrolling beyond the content
            scrollYFeedOverlay = Math.Clamp(scrollYFeedOverlay, 0, maxScroll);

            // Start Y position, adjusted by the current scroll position and extra top padding
            int startY = padding + extraTopPadding - (int)scrollYFeedOverlay;

            // Draw each animal's information and feed button
            foreach (var animal in allAnimals)
            {
                DrawAnimalInfoAndFeedButton(animal, padding, startY, player);
                startY += itemHeight + padding; // Move to the next item position
            }

            // Check if there are no animals to display
            if (!allAnimals.Any())
            {
                Raylib.DrawText("No animals available to feed.", 50, extraTopPadding + 50, 20, Color.WHITE);
            }

            DrawCloseButton();
        }

        // Method to get the corresponding texture for a given item name.
        // This will use the type name to decide which texture to fetch from the GameManager.
        static Texture2D GetTextureForItem(string itemName, GameManager gameManager)
        {
            if (Enum.TryParse<ProduceType>(itemName, out var produceType))
            {
                return GetTextureForProduce(produceType);
            }
            else
            {
                // Updated logic for FeedType parsing
                string feedTypeName = itemName.Substring(0, itemName.IndexOf(" Feed"));
                if (Enum.TryParse<FeedType>(feedTypeName, out var feedType))
                {
                    return GetTextureForFeed(feedType);
                }
            }

            return gameManager.defaultTexture;
        }

        static void DrawItemInfo(string name, int count, float price, Texture2D texture, int startX, int startY)
        {
            int padding = 10; // Padding between items
            int infoHeight = 100; // Increased height for additional button space

            // Adjust the height of infoRect to accommodate the buttons
            Rectangle infoRect = new Rectangle(startX, startY, Raylib.GetScreenWidth() - 2 * startX, infoHeight); // Adjusted height

            Raylib.DrawRectangleRec(infoRect, new Color(200, 200, 200, 255)); // Slightly darker background for contrast
            Raylib.DrawRectangleLinesEx(infoRect, 1, Color.BLACK); // Border for clarity

            // Item information text
            string infoText = $"{name} - Count: {count}, Price: ${price}";
            Raylib.DrawText(infoText, startX + padding, startY + padding, 20, Color.BLACK); // Draw text inside the rectangle

            // Scale factor for the texture
            float scaleFactor = 5.0f; // Adjust this value as needed to fit the texture within the infoRect

            // Position for the scaled texture
            Vector2 texturePosition = new Vector2(
                startX + infoRect.Width - (texture.Width * scaleFactor) - padding,
                startY + (infoRect.Height - (texture.Height * scaleFactor)) / 2
            );

            // Draw the scaled texture
            Raylib.DrawTextureEx(texture, texturePosition, 0, scaleFactor, Color.WHITE);
        }

        static void DrawShopFeedOverlay(Shop shop, Player player)
        {
            // Background for the overlay
            Raylib.DrawRectangle(0, 0, Raylib.GetScreenWidth(), Raylib.GetScreenHeight(), new Color(0, 0, 0, 128));

            int startY = 50;
            int itemHeight = 60; // Increased height for better spacing
            int buttonWidth = 400; // Increased button width for better layout
            int buyButtonWidth = 100; // Width for the buy button
            int padding = 10; // Padding inside the button for layout

            // Iterate over each FeedType
            foreach (FeedType feedType in Enum.GetValues(typeof(FeedType)))
            {
                Texture2D texture = GetTextureForFeed(feedType); // This method should be defined in GameManager
                float scale = CalculateFeedScaleForTexture(texture, itemHeight - 2 * padding); // Calculate scale to fit in button

                // Create a display name for the feed type
                string feedDisplayName = $"{feedType} Feed";
                // Create a new feed item for display purposes
                var feedItem = shop.CreateFeed(feedType);

                // Fetch the quantity of the feed item in the player's inventory
                int feedQuantity = player.Inventory.BuyableItems.OfType<Feed>().Count(f => f.Type == feedType);

                // Prepare texts and calculate their positions
                string priceText = $"${feedItem.purchasePrice}";
                string quantityText = $"Quantity: {feedQuantity}";

                // Calculate the positions for texts and the texture
                int priceTextX = 50 + padding;
                int quantityTextX = buttonWidth - buyButtonWidth - padding - Raylib.MeasureText(quantityText, 20); // Align to the right
                int textY = startY + padding;
                Vector2 texturePosition = new Vector2(priceTextX + Raylib.MeasureText(priceText, 20) + padding, startY + padding);

                // Rectangle positions
                Rectangle buttonRect = new Rectangle(50, startY, buttonWidth, itemHeight);
                Rectangle buyButtonRect = new Rectangle(buttonRect.X + buttonWidth - buyButtonWidth, startY, buyButtonWidth, itemHeight);

                // Draw the item button (non-clickable, just for display)
                Raylib.DrawRectangleRec(buttonRect, Color.BLUE);
                // Draw the texture scaled
                Raylib.DrawTextureEx(texture, texturePosition, 0.0f, scale, Color.WHITE);
                // Draw the price and quantity texts
                Raylib.DrawText(priceText, priceTextX, textY, 20, Color.WHITE);
                Raylib.DrawText(quantityText, quantityTextX, textY, 20, Color.WHITE);

                // Draw the "Buy" button
                int buyTextX = (int)(buyButtonRect.X + (buyButtonWidth - Raylib.MeasureText("Buy", 20)) / 2f);
                int buyTextY = startY + (itemHeight - Raylib.MeasureText("Buy", 20)) / 2;
                Raylib.DrawRectangleRec(buyButtonRect, Color.GREEN);
                Raylib.DrawText("Buy", buyTextX, buyTextY, 20, Color.WHITE);

                // Check for button click to purchase the feed
                if (player.Coins >= feedItem.purchasePrice && Raylib.IsMouseButtonPressed(MouseButton.MOUSE_LEFT_BUTTON) && Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), buyButtonRect))
                {
                    shop.BuyItem(feedItem, player, 1); // Since feed is unlimited, directly handle the purchase
                }

                startY += itemHeight + padding; // Increase the spacing for the next item
            }

            DrawCloseButton();
        }

        static Dictionary<string, int> quantitiesToSell = new Dictionary<string, int>();
        static void DrawSellProduceOverlay(Player player)
        {
            const int itemHeight = 120; // Increased height for vertical spacing
            const int padding = 10;
            const int topOffset = 50;
            const int startX = 50; // X position where the item box starts
            const int buttonStartX = startX + 300; // X position where buttons start

            // Set up for scrolling
            float mouseWheelMove = Raylib.GetMouseWheelMove();
            scrollYSellProduceOverlay -= mouseWheelMove * 20; // Adjust scroll speed as necessary

            // Get all sellable items and group them by type
            var sellableItemsGrouped = player.Inventory.SellableItems
                .OfType<Produce>() // Ensure we only deal with Produce objects
                .GroupBy(item => item.name)
                .Select(group => new
                {
                    Name = group.Key,
                    Count = group.Count(),
                    Price = group.First().sellPrice,
                    Texture = GetTextureForProduce(group.First().Type) // Now correctly accesses the Type
                }).ToList();

            // Calculate the total content height
            float contentHeight = sellableItemsGrouped.Count * (itemHeight + padding) + padding + topOffset;

            // Clamp scrollY to prevent scrolling beyond the content
            float maxScroll = Math.Max(0, contentHeight - Raylib.GetScreenHeight());
            scrollYSellProduceOverlay = Math.Clamp(scrollYSellProduceOverlay, 0, maxScroll);

            // Start Y position, adjusted by the current scroll position
            int startY = padding + topOffset - (int)scrollYSellProduceOverlay;

            // Draw each item's information
            foreach (var item in sellableItemsGrouped)
            {
                DrawItemInfo(item.Name, item.Count, item.Price, item.Texture, 50, startY);

                // Calculate positions for "To Sell" text and buttons based on the infoRect width
                int toSellTextStartX = startX + 200; // Adjust this value as needed
                int buttonsStartX = toSellTextStartX + 100; // Adjust this value as needed

                // Initialize quantity to sell if not already done
                if (!quantitiesToSell.ContainsKey(item.Name))
                {
                    quantitiesToSell[item.Name] = 0;
                }

                // Draw "+" and "-" buttons
                Rectangle plusButtonRect = new Rectangle(buttonsStartX, startY + 30, buttonWidth, buttonHeight);
                Rectangle minusButtonRect = new Rectangle(buttonsStartX + buttonWidth + buttonSpacing, startY + 30, buttonWidth, buttonHeight);
                Rectangle sellButtonRect = new Rectangle(buttonsStartX + 2 * (buttonWidth + buttonSpacing), startY + 30, sellButtonWidth + 30 , sellButtonHeight + 30);

                Raylib.DrawRectangleRec(plusButtonRect, Color.GRAY);
                Raylib.DrawRectangleRec(minusButtonRect, Color.GRAY);
                Raylib.DrawRectangleRec(sellButtonRect, Color.GREEN);

                Raylib.DrawText("+", (int)plusButtonRect.X + 10, (int)plusButtonRect.Y + 5, 20, Color.WHITE);
                Raylib.DrawText("-", (int)minusButtonRect.X + 10, (int)minusButtonRect.Y + 5, 20, Color.WHITE);
                Raylib.DrawText("Sell", (int)sellButtonRect.X + 20, (int)sellButtonRect.Y + 5, 20, Color.WHITE);

                Raylib.DrawText($"To Sell: {quantitiesToSell[item.Name]}", toSellTextStartX, startY + 30, 20, Color.WHITE);

                // Check if the buttons are clicked
                if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_LEFT_BUTTON))
                {
                    if (Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), plusButtonRect))
                    {
                        // Increase item count logic
                        if (quantitiesToSell[item.Name] < item.Count)
                        {
                            quantitiesToSell[item.Name]++;
                        }
                    }
                    else if (Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), minusButtonRect))
                    {
                        // Decrease item count logic
                        if (quantitiesToSell[item.Name] > 0)
                        {
                            quantitiesToSell[item.Name]--;
                        }
                    }
                    else if (Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), sellButtonRect))
                    {
                        // Find the specific ISellable item based on the name
                        ISellable itemToSell = player.Inventory.SellableItems
                            .OfType<Produce>()
                            .FirstOrDefault(p => p.name == item.Name);

                        if (itemToSell != null)
                        {
                            // Sell item logic
                            gameManager.Shop.SellItem(itemToSell, player, quantitiesToSell[item.Name]);
                            quantitiesToSell[item.Name] = 0; // Reset the quantity after selling
                        }
                    }
                }

                startY += itemHeight + padding; // Move to the next item position
            }

            DrawCloseButton();
        }

        // Helper function to calculate the scale factor based on desired height
        static float CalculateProduceScaleForTexture(Texture2D texture, int desiredHeight)
        {
            return desiredHeight / (float)texture.Height;
        }

        // Helper function to calculate the scale factor based on desired height
        static float CalculateFeedScaleForTexture(Texture2D texture, int desiredHeight)
        {
            return desiredHeight / (float)texture.Height; // Ensure the scale is a float
        }

        // Helper functions to get textures based on animal or feed type
        public static Texture2D GetTextureForAnimal(Animal animal)
        {
            if(animal is Cow)
            {
                return gameManager.cowTexture;
            }
            else if(animal is Chicken)
            {
                return gameManager.chickenTexture;
            }
            else if (animal is Pig)
            {
                return gameManager.pigTexture;
            }
            else if (animal is Goat)
            {
                return gameManager.goatTexture;
            }
            else if(animal is Sheep)
            {
                return gameManager.sheepTexture;
            }
            else
                return gameManager.defaultTexture;
        }

        static public Texture2D GetTextureForProduce(ProduceType type)
        {
            switch (type)
            {
                case ProduceType.Beef:
                    return gameManager.beefTexture;
                case ProduceType.Milk:
                    return gameManager.milkTexture;
                case ProduceType.Pork:
                    return gameManager.porkTexture;
                case ProduceType.GoatMilk:
                    return gameManager.goatMilkTexture;
                case ProduceType.GoatMeat:
                    return gameManager.goatMeatTexture;
                case ProduceType.ChickenMeat:
                    return gameManager.chickenMeatTexture;
                case ProduceType.Egg:
                    return gameManager.eggTexture;
                case ProduceType.Wool:
                    return gameManager.woolTexture;
                case ProduceType.Lamb:
                    return gameManager.lambTexture;
                default:
                    return gameManager.defaultTexture; // Some default texture if needed
            }
        }
        
        static public Texture2D GetTextureForPlot(Plot plot)
        {
            switch (plot.Type)
            {
                case PlotType.CowPlot:
                    return gameManager.cowPlotTexture;
                case PlotType.ChickenPlot:
                    return gameManager.chickenPlotTexture;
                case PlotType.PigPlot:
                    return gameManager.pigPlotTexture;
                case PlotType.SheepPlot:
                    return gameManager.sheepPlotTexture;
                case PlotType.GoatPlot:
                    return gameManager.goatPlotTexture;
                default:
                    return gameManager.defaultTexture; // Some default texture if needed
            }
        }

        public static Texture2D GetTextureForFeed(FeedType feedType)
        {
            switch (feedType)
            {
                case FeedType.CowFeed:
                    return gameManager.cowFeedTexture;
                case FeedType.ChickenFeed:
                    return gameManager.chickenFeedTexture;
                case FeedType.PigFeed:
                    return gameManager.pigFeedTexture;
                case FeedType.GoatFeed:
                    return gameManager.goatFeedTexture;
                case FeedType.SheepFeed:
                    return gameManager.sheepFeedTexture;
                default:
                    return gameManager.defaultTexture;
            }
        }
        static void DrawCloseButton()
        {
            // Draw the "Close" button consistently for all overlays
            Rectangle closeButtonRect = new Rectangle(10, 10, 100, 30); // Example position and size
            Raylib.DrawRectangleRec(closeButtonRect, Color.RED);
            Raylib.DrawText("Close", (int)closeButtonRect.X + 20, (int)closeButtonRect.Y + 5, 20, Color.WHITE);

            // Check if the "Close" button is clicked
            if (Raylib.IsMouseButtonReleased(MouseButton.MOUSE_LEFT_BUTTON))
            {
                if (Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), closeButtonRect))
                {
                    currentState = GameState.MainGame;
                    scrollY = 0;
                    scrollYFeedOverlay = 0;
                }
            }
        }
    }
}
