using SearchForDriversWebApp.Models;
using System.ComponentModel.DataAnnotations;

namespace SearchForDriversWebApp.ViewModels
{
    public class TripModel
    {

        [Required(ErrorMessage = "Не вказаний DepartureLocation")]
        public string DepartureLocation { get; set; }


        [Required(ErrorMessage = "Не вказаний пароль")]
        public string ArrivalLocation { get; set; }
        [Required(ErrorMessage = "Не вказана дата поїздки")]
        public DateTime DateOfTrip { get; set; }

        [Required(ErrorMessage = "Не вказаний Distance")]
        public double Distance { get; set; }
    }
}
