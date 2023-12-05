using Raylib_cs;
using System.Numerics;

namespace CustomProgram
{
    public class Feed:IBuyable
    {
        /// <summary>
        /// name of the feed
        /// </summary>
        public string name{ get; set; }
        /// <summary>
        /// price to purchase from shop
        /// </summary>
        public float purchasePrice{ get; set; }
        /// <summary>
        /// type of the feed
        /// </summary>
        private FeedType type{get;set;}
        /// <summary>
        /// for testing purposes
        /// </summary>
        private string imagePath;
        /// <summary>
        /// Constructor for Feed
        /// </summary>
        /// <param name="_name"></param>
        /// <param name="_purchasePrice"></param>
        /// <param name="_type"></param>
        public Feed(string _name, float _purchasePrice, FeedType _type)
        {
            name = _name;
            purchasePrice = _purchasePrice;
            type = _type;
            AssignImagePath(_type);
        }
        /// <summary>
        /// Read only Property for type of feed
        /// </summary>
        public FeedType Type
        { get { return type; } }
        /// <summary>
        /// assigning the test image path based on the type
        /// </summary>
        /// <param name="type"></param>
        private void AssignImagePath(FeedType type)
        {
            switch (type)
            {
                case FeedType.CowFeed:
                    imagePath = "assets/Radish.png";
                    break;
                case FeedType.ChickenFeed:
                    imagePath = "assets/Corn.png";
                    break;
                case FeedType.PigFeed:
                    imagePath = "assets/Taro.png";
                    break;
                case FeedType.GoatFeed:
                    imagePath = "assets/Carrot.png";
                    break;
                case FeedType.SheepFeed:
                    imagePath = "assets/Pea.png";
                    break;
                default:
                    imagePath = "assets/Default.png";
                    break;
            }
        }   
        /// <summary>
        /// read only property for image path
        /// </summary>
        public string ImagePath
        {
            get { return imagePath; }
        }
    }
}
