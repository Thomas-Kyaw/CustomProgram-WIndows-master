using Raylib_cs;
using System.Collections.Generic;
using System.Numerics;

namespace CustomProgram
{
    public abstract class Animal:IBuyable
    {
        /// <summary>
        /// Name of the Animal
        /// </summary>
        public string name{ get; set; }
        /// <summary>
        /// Price to purchase the animal
        /// </summary>
        public float purchasePrice{ get; set; }
        /// <summary>
        /// Hunger level of the animal
        /// </summary>
        protected float hunger { get; set; } = 100f; // Starting hunger
        /// <summary>
        /// Health of the animal
        /// </summary>
        protected float health { get; set; } = 100f; // Starting health
        /// <summary>
        /// Testing purposes image path
        /// </summary>
        protected string imagePath { get; set; }
        /// <summary>
        /// List of produces the animal will produce
        /// </summary>
        protected List<Produce> produces { get; set; }
        /// <summary>
        /// Fields for updating the hunger of the animal
        /// </summary>
        protected double hungerDecrementTime = 10f; // Time in seconds to decrement hunger
        protected double lastHungerUpdateTime = Raylib.GetTime(); // Last time hunger was updated
        /// <summary>
        /// Fields for updating the produce rate of the animal
        /// </summary>
        protected double produceTimer = 15f; // Time in seconds until the animal produces an item
        protected double lastProduceUpdateTime = Raylib.GetTime(); // Last time an item was produced
        /// <summary>
        /// If the animal is alive for not
        /// </summary>
        protected bool isAlive = true;
        /// <summary>
        /// To check if the feed is correctly put in
        /// </summary>
        /// <param name="feedType"></param>
        /// <returns></returns>
        protected abstract bool IsCorrectFeedType(FeedType feedType);
        /// <summary>
        /// Field to get the value that will update the health and hunger for animal
        /// </summary>
        /// <param name="feedType"></param>
        /// <returns></returns>
        protected abstract float GetFeedValue(FeedType feedType);
        /// <summary>
        /// Field for time update
        /// </summary>
        protected ITimeProvider timeProvider;
        /// <summary>
        /// Constructor for Animal
        /// </summary>
        /// <param name="_name"></param>
        /// <param name="_purchasePrice"></param>
        /// <param name="defaultImagePath"></param>
        /// <param name="timeProvider"></param>
        public Animal(string _name, float _purchasePrice, string defaultImagePath, ITimeProvider timeProvider)
        {
            name = _name;
            purchasePrice = _purchasePrice;
            imagePath = defaultImagePath;
            produces = new List<Produce>();
            this.timeProvider = timeProvider;
        }
        /// <summary>
        /// Read Only property for IsAlive
        /// </summary>
        public bool IsAlive
        {
            get{return isAlive;}
        }
        /// <summary>
        /// Update method to be called in game loop. It wil be overriden by the child classes
        /// </summary>
        public virtual void Update()
        {
            double currentTime = timeProvider.GetCurrentTime();
            if (currentTime - lastHungerUpdateTime >= hungerDecrementTime)
            {
                hunger -= 10f; // Decrement hunger
                lastHungerUpdateTime = currentTime; // Reset the last update time

                if (hunger <= 0)
                {
                    health -= 10f; // Decrement health if hunger is at 0
                    hunger = 0; // Ensure hunger does not go below 0
                }
            }

            // Check for death
            if (health <= 0 && isAlive)
            {
                health = 0; // Ensure health doesn't go below 0
                Die();
            }
        }

        /// <summary>
        /// Animal will produce with this method
        /// </summary>
        public abstract void ProduceItem();
        /// <summary>
        /// updates hunger based on FeedType
        /// </summary>
        /// <param name="feedType"></param>
        public virtual void Feed(FeedType feedType)
        {
            // Check for correct feed type and update hunger
            if (IsCorrectFeedType(feedType))
            {
                hunger = Math.Min(hunger + 20f, 100f);
            }
        }

        /// <summary>
        /// This method will be called when the animal's health reaches 0
        /// </summary>
        public virtual void Die()
        {
            isAlive = false;
            Console.WriteLine($"Animal '{name}' has died at {DateTime.Now}."); // Debug message
            // Perform any additional cleanup specific to the animal
            OnDeath?.Invoke(this);
        }

        public void ClearProduces()
        {
            produces.Clear();
        }

        /// <summary>
        /// Delegate and event for handling death
        /// </summary>
        /// <param name="animal"></param>
        public delegate void AnimalDeathHandler(Animal animal);
        public event AnimalDeathHandler OnDeath;

        /// <summary>
        /// for testing purposes
        /// </summary>
        public void ForceAnimalHealth()
        {
            health = 0;
        }

        public void ForceHunger()
        {

        }

        public float Hunger
        {
            get { return hunger; }
        }
        public float Health
        {
            get { return health; }
        }
        public List<Produce> Produces
        {
            get{return produces;}
        }
        public double LastProduceUpdateTime
        {
            get{return lastProduceUpdateTime;}
        }
        public double ProduceTimer
        {
            get{return produceTimer;}
        }

    }

}
