using Raylib_cs;
using System.Collections.Generic;
using System.Numerics;

namespace CustomProgram
{
    public class Cow : Animal
    {
        private static Random random = new Random();
        /// <summary>
        /// Constructor for Cow
        /// </summary>
        /// <param name="_name"></param>
        /// <param name="_purchasePrice"></param>
        /// <param name="_defaultImagePath"></param>
        /// <param name="_timeProvider"></param>
        public Cow(string _name, float _purchasePrice, string _defaultImagePath, ITimeProvider _timeProvider):base(_name, _purchasePrice, _defaultImagePath, _timeProvider)
        {
            health = 150f;
            produceTimer = 20f;
        }
        /// <summary>
        /// update health hunger and produce
        /// </summary>
        public override void Update()
        {
            double currentTime = base.timeProvider.GetCurrentTime();
            if (currentTime - lastHungerUpdateTime >= hungerDecrementTime && isAlive)
            {
                hunger -= 9f; // original value is 9f
                lastHungerUpdateTime = currentTime;
                if (hunger <= 0)
                {
                    health -= 11f; //original value is 11f
                    hunger = 0;
                }
            }

            if (currentTime - lastProduceUpdateTime >= produceTimer && isAlive)
            {
                ProduceItem();
                lastProduceUpdateTime = currentTime;
            }

            if (health <= 0 && isAlive)
            {
                health = 0;
                Die();
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
                    ? new Produce("Milk", 30f, ProduceType.Milk)
                    : new Produce("Beef", 55f, ProduceType.Beef);

                produces.Add(produce);

                // Debug statement
                //Console.WriteLine($"Cow '{name}' produced {produce.name} at {DateTime.Now}. Current Produce Count: {produces.Count}");
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
                if(hunger >= 80)
                {
                    health = 150f;
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
            return feedType == FeedType.CowFeed;
        }
        /// <summary>
        /// This method will return the amount hunger increased by when fed
        /// </summary>
        /// <param name="feedType"></param>
        /// <returns></returns>
        protected override float GetFeedValue(FeedType feedType)
        {
            return 27f; // The amount hunger is increased by when fed
        }
    }
}
