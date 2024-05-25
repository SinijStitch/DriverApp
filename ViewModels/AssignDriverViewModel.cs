using SearchForDriversWebApp.Models;

namespace SearchForDriversWebApp.ViewModels
{
    public class AssignDriverViewModel
    {
        public int TripId { get; set; }
        public string DepartureLocation { get; set; }
        public string ArrivalLocation { get; set; }
        public string Distance { get; set; }
        public List<User> AvailableDrivers { get; set; }
    }
}
