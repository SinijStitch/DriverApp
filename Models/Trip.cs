using System.ComponentModel.DataAnnotations.Schema;

namespace SearchForDriversWebApp.Models
{
    public class Trip
    {
        public int Id { get; set; }
        public string DepartureLocation { get; set; }
        public string ArrivalLocation { get; set; }
        public double Distance { get; set; }
        public DateTime? DateOfTrip { get; set; }
        public string Status { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public virtual ICollection<Assignment> Assignments { get; set; }
    }
}
