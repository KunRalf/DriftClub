using System.Collections.Generic;

namespace PlayerHub
{
    public class PlayerData
    {
        public string PlayerName { get; set; }
        public int PlayerCash { get; set; } = 10;
        public int? PlayerCurrentCarId { get; set; }
        public List<int> OpenedCars { get; set; } = new List<int>();
    }
}