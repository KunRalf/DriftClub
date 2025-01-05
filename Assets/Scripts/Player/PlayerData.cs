using System.Collections.Generic;

namespace Player
{
    public class PlayerData
    {
        public string PlayerName { get; set; }
        public int PlayerCash { get; set; }   
        public int PlayerCurrentCarId { get; set; }
        public List<int> OpenedCars { get; set; }
    }
}