using Raylib_cs;
using System.Numerics;

namespace CustomProgram
{
    public class Produce:ISellable
    {
        /// <summary>
        /// name of the produce
        /// </summary>
        public string name{ get; set; }
        /// <summary>
        /// selling price of the produce
        /// </summary>
        public float sellPrice{ get; set; }
        /// <summary>
        /// type of produce
        /// </summary>
        private ProduceType type;
        /// <summary>
        /// image for testing purpose
        /// </summary>
        private string imagePath;
        /// <summary>
        /// Constructor for produce
        /// </summary>
        /// <param name="_name"></param>
        /// <param name="_sellPrice"></param>
        /// <param name="_type"></param>
        public Produce(string _name, float _sellPrice, ProduceType _type)
        {
            name = _name;
            sellPrice = _sellPrice;
            type = _type;
            AssignImagePath(_type);
        }
        /// <summary>
        /// assigning image for test
        /// </summary>
        /// <param name="type"></param>
        private void AssignImagePath(ProduceType type)
        {
            switch (type)
            {
                case ProduceType.Beef:
                    imagePath = "assets/Beef.png";
                    break;
                case ProduceType.Milk:
                    imagePath = "assets/CowMilk.png";
                    break;
                case ProduceType.Pork:
                    imagePath = "assets/Pork.png";
                    break;
                case ProduceType.GoatMeat:
                    imagePath = "assets/GoatMeat.png";
                    break;
                case ProduceType.GoatMilk:
                    imagePath = "assets/GoatMilk.png";
                    break;
                case ProduceType.ChickenMeat:
                    imagePath = "assets/ChickenMeat.png";
                    break;
                case ProduceType.Egg:
                    imagePath = "assets/Egg.png";
                    break;
                case ProduceType.Wool:
                    imagePath = "assets/Wool.png";
                    break;
                case ProduceType.Lamb:
                    imagePath = "assets/Lamb.png";
                    break;
                default:
                    imagePath = "assets/Default.png";
                    break;
            }
        }
        /// <summary>
        /// read only property
        /// </summary>
        public string ImagePath
        {
            get { return imagePath; }
        }
        /// <summary>
        /// read only property for type
        /// </summary>
        public ProduceType Type
        {
            get { return type; }
        }
    }
}
