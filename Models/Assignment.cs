using System.ComponentModel.DataAnnotations.Schema;

namespace SearchForDriversWebApp.Models
{
    public class Assignment
    {
        public int Id { get; set; }
        public int TripId { get; set; }
        [ForeignKey("TripId")]
        public Trip Trip { get; set; }
        public int DriverId { get; set; }
        [ForeignKey("DriverId")]
        public User User { get; set; }
        public int Price { get; set; }
    }
}
