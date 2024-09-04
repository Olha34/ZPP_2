namespace zpp_aplikacja.Pages.Models
{
    public class Reward
    {
        public int RewardId { get; set; } // Identyfikator nagrody
        public string Name { get; set; } // Nazwa nagrody
        public int PointsRequired { get; set; } // Ilość punktów potrzebna do zdobycia nagrody
        public string Description { get; set; } // Opis nagrody
    }
}