using Raylib_cs;
using System.Collections.Generic;
using System.Numerics;

namespace CustomProgram
{
    public class Chicken : Animal
    {
        /// <summary>
        /// this will me used to produce egg or meat
        /// </summary>
        private static Random random = new Random();
        /// <summary>
        /// constructor for Chicken
        /// </summary>
        /// <param name="_name"></param>
        /// <param name="_purchasePrice"></param>
        /// <param name="_defaultImagePath"></param>
        /// <param name="_timeProvider"></param>
        public Chicken(string _name, float _purchasePrice, string _defaultImagePath, ITimeProvider _timeProvider):base(_name, _purchasePrice, _defaultImagePath, _timeProvider)
        {
            health = 90f;
            produceTimer = 19f;
        }
        /// <summary>
        /// update health hunger and produce
        /// </summary>
        public override void Update()
        {
            double currentTime = base.timeProvider.GetCurrentTime();
            if (currentTime - lastHungerUpdateTime >= hungerDecrementTime && isAlive)
            {
                hunger -= 6f; // Decrement hunger
                lastHungerUpdateTime = currentTime; // Reset the last update time

                if (hunger <= 0)
                {
                    health -= 6f; // Decrement health if hunger is at 0
                    hunger = 0; // Ensure hunger does not go below 0
                }
            }

            // Update produce timer
            if (currentTime - lastProduceUpdateTime >= produceTimer && isAlive)
            {
                ProduceItem(); // Produce an item
                lastProduceUpdateTime = currentTime; // Reset the last produce time
            }

            if (health <= 0 && isAlive)
            {
                    Die(); // Trigger the death of the animal only once
            }
        }
        /// <summary>
        /// method for producing Produces. The produce will be random
        /// </summary>
        public override void ProduceItem()
        {
            if (isAlive)
            {
                int produceType = random.Next(2); // Generates 0 or 1
                Produce produce = produceType == 0
                    ? new Produce("Egg", 30f, ProduceType.Egg)
                    : new Produce("ChickenMeat", 45f, ProduceType.ChickenMeat);

                produces.Add(produce);

                // Debug statement
                //Console.WriteLine($"Chicken '{name}' produced {produce.name} at {DateTime.Now}. Current Produce Count: {produces.Count}");
            }
        }
        /// <summary>
        /// this method will be called to oupdate health and hunger
        /// </summary>
        /// <param name="feedType"></param>
        public override void Feed(FeedType feedType)
        {
            if (IsCorrectFeedType(feedType))
            {
                hunger = Math.Min(hunger + GetFeedValue(feedType), 100f);
                lastHungerUpdateTime = Raylib.GetTime(); // Reset the hunger update timer after feeding
                if(hunger >= 90)
                {
                    health = 90f;
                }
            }
        }
        /// <summary>
        /// check if the feed is correct type or not
        /// </summary>
        /// <param name="feedType"></param>
        /// <returns></returns>
        protected override bool IsCorrectFeedType(FeedType feedType)
        {
            return feedType == FeedType.SheepFeed;
        }
        /// <summary>
        /// This method will return the amount hunger increased by when fed
        /// </summary>
        /// <param name="feedType"></param>
        /// <returns></returns>
        protected override float GetFeedValue(FeedType feedType)
        {
            return 33f; // The amount hunger is increased by when fed
        }
    }
}
