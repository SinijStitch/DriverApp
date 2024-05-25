using SearchForDriversWebApp.Models;

namespace SearchForDriversWebApp.Services
{
    public interface ITripService
    {
        void CreateTrip(Trip trip);
    }

    public class TripService : ITripService
    {
        private readonly DriverDbContext _context;

        public TripService(DriverDbContext context)
        {
            _context = context;
        }

        public void CreateTrip(Trip trip)
        {
            trip.Status = "В очікувані огляду";

            _context.Trips.Add(trip);
            _context.SaveChanges();
        }
    }
}
