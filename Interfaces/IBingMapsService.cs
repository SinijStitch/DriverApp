namespace SearchForDriversWebApp.Interfaces
{
    public interface IBingMapsService
    {
        Task<string> GetAddressFromCoordinates(double latitude, double longitude);
        Task<double> CalculateDistance(double startLatitude, double startLongitude, double endLatitude, double endLongitude);
    }
}
