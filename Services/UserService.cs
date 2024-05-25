using SearchForDriversWebApp.Models;

namespace SearchForDriversWebApp.Services
{
    public class UserService
    {
        private static List<User> users = new List<User>
        {
            new User { Id = 1, Username = "JohnDoe", Email = "john@example.com", Phone = "123-456-7890", Role = "User" },
            // Add more users as needed
        };

        public User GetUserById(int id)
        {
            return users.Find(u => u.Id == id);
        }
    }
}
