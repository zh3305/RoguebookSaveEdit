using Abrakam.Data.Runs;

namespace RoguebookSaveEdit.model
{
    public class CardData : baseCard
    {

        public int id
        {
            get => cardId;
            set => cardId = value;
        }

        public string orderId { get; set; }
        public string heroType { get; set; }

        /**
status
event
ally
creature
talent
gem
treasure
perk
positionStatus
hero
consumable
globalStatus
tournamentSeal
         */
        public string type { get; set; }
        public string faeria { get; set; }
        public string scope { get; set; }
        public string price { get; set; }
        public string tags { get; set; }
        // public string name_cn { get; set; }
        // public string text_cn { get; set; }
    }
}