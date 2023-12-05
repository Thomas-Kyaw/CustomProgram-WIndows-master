using Raylib_cs;
using System.Collections.Generic;
using System.Numerics;

namespace CustomProgram
{
    public class Plot
    {
        /// <summary>
        /// Player who owns the plot
        /// </summary>
        private Player owner;
        /// <summary>
        /// size of the plot which is only for testin and UI aspects
        /// </summary>
        private int size;
        /// <summary>
        /// position of the plot
        /// </summary>
        private Vector2 position;
        /// <summary>
        /// List of animals that will be in the plot
        /// </summary>
        protected List<Animal> animals { get; set; }
        /// <summary>
        /// Amount of animals you can add to the plot
        /// </summary>
        private int animalLimit { get; set; }
        /// <summary>
        /// type of plot which means type of animal you can add
        /// </summary>
        private PlotType type { get; set; }
        /// <summary>
        /// construcotr for Plot
        /// </summary>
        /// <param name="_type"></param>
        /// <param name="_owner"></param>
        public Plot(PlotType _type,  Player _owner)
        {
            this.owner = _owner;
            type = _type;
            animals = new List<Animal>();
            SetAnimalLimitAndImagePath(_type);
        }
        /// <summary>
        /// Properties
        /// </summary>
        public List<Animal> Animals
        {
            get{return animals;}
        }
        public Vector2 Posiiton
        {
            get { return position; }
            set { position = value; }
        }
        public int Size
        {
            get { return size; }
            set { size = value; }
        }
        public PlotType Type
        {
            get { return type; }
        }
        /// <summary>
        /// method for setting the amount of animal you can put in based on the animal type
        /// </summary>
        /// <param name="type"></param>
        private void SetAnimalLimitAndImagePath(PlotType type)
        {
            switch (type)
            {
                case PlotType.CowPlot:
                    animalLimit = 10;
                    break;
                case PlotType.ChickenPlot:
                    animalLimit = 20;
                    break;
                case PlotType.SheepPlot:
                    animalLimit = 7;
                    break;
                case PlotType.GoatPlot:
                    animalLimit = 8;
                    break;
                case PlotType.PigPlot:
                    animalLimit = 12;
                    break;
                default:
                    animalLimit = 5;
                    break;
            }
        }
        /// <summary>
        /// adding animal to the plot
        /// </summary>
        /// <param name="animal"></param>
        /// <returns></returns>
        public bool AddAnimal(Animal animal)
        {
            if (Animals.Count >= animalLimit || !IsCorrectAnimalType(animal))
            {
                return false;
            }

            animal.OnDeath += Animal_OnDeath;
            animals.Add(animal);
            return true;
        }
        /// <summary>
        /// event method the will trigger when the animal health reaches 0
        /// </summary>
        /// <param name="animal"></param>
        private void Animal_OnDeath(Animal animal)
        {
            owner.Reputation -= 7; // Decrease the player's reputation by 5
            owner.Reputation = Math.Max(0, owner.Reputation); // Ensure reputation doesn't go below 0
            RemoveAnimal(animal); // Remove the dead animal
        }
        /// <summary>
        /// to check if the animal type matches the plot type
        /// </summary>
        /// <param name="animal"></param>
        /// <returns></returns>
        private bool IsCorrectAnimalType(Animal animal)
        {
            // Logic to check if the animal type matches the plot type
            switch (type)
            {
                case PlotType.CowPlot:
                    return animal is Cow;
                case PlotType.ChickenPlot:
                    return animal is Chicken;
                case PlotType.PigPlot:
                    return animal is Pig;
                case PlotType.SheepPlot:
                    return animal is Sheep;
                case PlotType.GoatPlot:
                    return animal is Goat;
                default:
                    return false;
            }
        }
        /// <summary>
        /// method for updating the animals inside this plot
        /// </summary>
        public void UpdateAnimals()
        {     
            foreach (var animal in new List<Animal>(animals)) // Using a copy to safely modify the original list
            {
                animal.Update();
                TransferProducesToPlayer(animal);
                if (!animal.IsAlive)
                {
                    Console.WriteLine($"Removing dead animal: {animal.name}");
                    RemoveAnimal(animal);
                }
            }
        }
        /// <summary>
        /// removing an animal from plot. it will be triggered on animal death
        /// </summary>
        /// <param name="animal"></param>
        public void RemoveAnimal(Animal animal)
        {
            Console.WriteLine($"Removing animal: {animal.name} from plot {Type}");
            animals.Remove(animal);
            animal.OnDeath -= RemoveAnimal; // Unsubscribe from the event
                                            // Additional cleanup if necessary
        }

        /// <summary>
        /// the Produce from the animal in this plot will be added to player inventory
        /// </summary>
        /// <param name="animal"></param>
        private void TransferProducesToPlayer(Animal animal)
        {
            foreach (var produce in animal.Produces)
            {
                owner.Inventory.AddSellableItem(produce);
                //Console.WriteLine($"Transferring produce to player: {produce.name}"); // Debug
            }
            animal.ClearProduces();
        }

    }
}
