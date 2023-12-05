using Raylib_cs;
using System.Collections.Generic;
using System.Numerics;
using NUnit.Framework;

namespace CustomProgram
{
    public class TestTimeProvider : ITimeProvider
    {
        private double time;
        
        public void SetTime(double newTime)
        {
            time = newTime;
        }

        public double GetCurrentTime()
        {
            return time;
        }
    }

    [TestFixture]
    public class GameTests
    {
        private TestTimeProvider timeProvider;
        private Player player;

        [SetUp]
        public void Setup()
        {
            // Initialize with a mock or test time provider
            timeProvider = new TestTimeProvider(); 
            player = new Player("TestPlayer");
        }

        // Test to verify if an animal can be successfully added to a plot.
        // Ensures that the plot's animal list increments and the addition returns a true result.
        [Test]
        public void Animal_Added_To_Plot()
        {
            var plot = new Plot(PlotType.CowPlot, player);
            var animal = new Cow("Bessie", 100f, "cow.png", timeProvider);

            bool result = plot.AddAnimal(animal);

            Assert.IsTrue(result);
            Assert.AreEqual(1, plot.Animals.Count);
        }

        // Test to check if an animal's state changes to 'dead' when its health reaches zero.
        // Ensures that the UpdateAnimals method correctly updates the animal's status.
        [Test]
        public void Animal_Dies_When_Health_Reaches_Zero()
        {
            var plot = new Plot(PlotType.CowPlot, player);
            var animal = new Cow("Bessie", 100f, "cow.png", timeProvider);
            plot.AddAnimal(animal);

            animal.ForceAnimalHealth(); // Force animal's health to zero

            plot.UpdateAnimals(); // Update should trigger Die() method

            Assert.IsFalse(animal.IsAlive);
        }

        // Test to ensure that animals of the wrong type cannot be added to a specific plot type.
        // Checks if the system correctly prevents a chicken from being added to a cow plot.
        [Test]
        public void Cannot_Add_Wrong_Animal_Type_To_Plot()
        {
            var cowPlot = new Plot(PlotType.CowPlot, player);
            var chicken = new Chicken("Clucky", 50f, "chicken.png", timeProvider); // Assuming Chicken class exists

            bool result = cowPlot.AddAnimal(chicken);

            Assert.IsFalse(result, "Should not be able to add a chicken to a cow plot.");
            Assert.AreEqual(0, cowPlot.Animals.Count, "Cow plot should have no animals.");
        }

        // Test to verify if a cow produces the correct items (Milk or Beef) after a set time interval.
        // Checks if the produce is correctly added to the player's inventory.
        [Test]
        public void Cow_Produces_Correct_Produce()
        {
            var player = new Player("p1");
            var plot = new Plot(PlotType.CowPlot, player);
            var cow = new Cow("Bessie", 100f, "assets/Cow.png", timeProvider);
            player.AddPlot(plot);
            plot.AddAnimal(cow);

            // Ensure enough time has passed for the cow to produce an item
            double newTime = cow.LastProduceUpdateTime + cow.ProduceTimer + 1;
            timeProvider.SetTime(newTime);

            plot.UpdateAnimals(); // This should trigger ProduceItem() and transfer to inventory

            Assert.IsNotEmpty(player.Inventory.SellableItems, "Player's inventory should have a produce item.");
            var producedItem = player.Inventory.SellableItems[0];
            Assert.IsTrue(producedItem.name == "Milk" || producedItem.name == "Beef", 
                        "Cow should produce either Milk or Beef in the player's inventory.");
        }

        // Test to confirm if a sheep produces the correct items (Lamb or Wool) after a set time interval.
        // Ensures the correct addition of these items into the player's inventory.
        [Test]
        public void Sheep_Produces_Correct_Produce()
        {
            var player1 = new Player("p2");
            var plot = new Plot(PlotType.SheepPlot, player1);
            var sheep = new Sheep("Bessie", 100f, "assets/Sheep.png", timeProvider);
            player1.AddPlot(plot);
            plot.AddAnimal(sheep);

            // Ensure enough time has passed for the sheep to produce an item
            double newTime = sheep.LastProduceUpdateTime + sheep.ProduceTimer + 1;
            timeProvider.SetTime(newTime);

            plot.UpdateAnimals(); // This should trigger ProduceItem() and transfer to inventory

            Assert.IsNotEmpty(player1.Inventory.SellableItems, "Player's inventory should have a produce item.");
            var producedItem = player1.Inventory.SellableItems[0];
            Assert.IsTrue(producedItem.name == "Lamb" || producedItem.name == "Wool",
                        "Sheep should produce either Lamb or Wool in the player's inventory.");
        }

        // Test to ensure a pig produces the correct item (Pork) after a set time interval.
        // Verifies if the produced item is accurately transferred to the player's inventory.
        [Test]
        public void Pig_Produces_Correct_Produce()
        {
            var player1 = new Player("p2");
            var plot = new Plot(PlotType.PigPlot, player1);
            var pig = new Pig("Bessie", 100f, "assets/Pig.png", timeProvider);
            player1.AddPlot(plot);
            plot.AddAnimal(pig);

            // Ensure enough time has passed for the sheep to produce an item
            double newTime = pig.LastProduceUpdateTime + pig.ProduceTimer + 1;
            timeProvider.SetTime(newTime);

            plot.UpdateAnimals(); // This should trigger ProduceItem() and transfer to inventory

            Assert.IsNotEmpty(player1.Inventory.SellableItems, "Player's inventory should have a produce item.");
            var producedItem = player1.Inventory.SellableItems[0];
            Assert.IsTrue(producedItem.name == "Pork",
                        "Pig should produce Pork in the player's inventory.");
        }
        // Other tests...
    }

}
