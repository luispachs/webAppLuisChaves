using WebAppluisChaves.Models;

namespace WebAppluisChaves.ViewModels
   
{
    public class UserViewModel
    {
        public User? user { get; set; }
        public List<State>? states { get; set; }
    }
}
