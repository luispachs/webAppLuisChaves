using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAppluisChaves.Models;
using WebAppluisChaves.ViewModels;
using System.Text.Json;

namespace WebAppluisChaves.Controllers
{
    public class ClientsController : Controller
    {
        private readonly BillingContext context;
        public ClientsController( BillingContext context)
        {
            this.context = context;
        }
        
        public async Task<IActionResult> Index()
        {
            var data = from user in context.Users
                        join city in context.Cities on user.City equals city.Id
                        join state in context.States on city.StateId equals state.Id
                        select new { user.Id,user.BusinessName,user.Phone,user.Nit,user.LocalAddress,city.CityName,state.StateName };
            context.Database.CloseConnection();
            return View(data.ToList());
        }


        [HttpGet]
        public async Task<IActionResult> newUser()
        {
            UserViewModel userViewModel = new UserViewModel();
            List<State> states = await this.context.States.ToListAsync();
            this.context.Database.CloseConnection();
            userViewModel.states = states;
            return View(userViewModel);
        }

        [HttpPost()]
        public IActionResult create()
        {
            var form = Request.Form;
            if (form == null)
            {
                return View("index");
            }
            var user = new User();
            user.Nit = int.Parse(form["user-nit"]);
            user.BusinessName = form["user-name"];
            user.Phone = form["user-phone"];
            user.LocalAddress = form["user-address"];
            user.City = int.Parse(form["city"]);
            context.Add(user);
            context.SaveChanges();

            return Redirect("/Home");
        }

        [HttpGet()]
        public IActionResult edit(int id) {

            var user = context.Users.FirstOrDefault(x => x.Id == id);
            var states = context.States.ToList();
            context.Database.CloseConnection();
            if (user == null)
            {
                return RedirectToAction("Index");
            }
            UserViewModel userViewModel = new UserViewModel();
            userViewModel.user = user;
            userViewModel.states = states;
            return View("NewUser",userViewModel);
        }
        [HttpPost()]
        public  IActionResult editAction()
        {   
            var form = Request.Form;
            if(form == null)
            {
                return Redirect("/Clients/");
            }
       
            int id = int.Parse(form["id"]);
            var user =  context.Users.FirstOrDefault(u => u.Id == id);
            if(user == null)
            {
                return Redirect("/Clients/");
            }
            if(user.Nit != int.Parse(form["user-nit"])){
                user.Nit = int.Parse(form["user-nit"]);
            }
            if(user.BusinessName != form["user-name"]) {
                user.BusinessName = form["user-name"];
            }
            if(user.LocalAddress!= form["user-address"])
            {
                user.LocalAddress = form["user-address"];
            }
            if (form["city"]!= "")
            {
                if (user.City != int.Parse(form["city"]))
                {
                    user.City = int.Parse(form["city"]);
                }
            }
           
            context.Update(user);
            context.SaveChanges();
            context.Database.CloseConnection();

            return Redirect("/Clients/");
        }

        [HttpGet()]
        public IActionResult delete( int id) {
            var user = context.Users.FirstOrDefault(u=>u.Id == id);
            if(user == null)
            {
                Redirect("/Clients/");
            }
            context.Remove(user);
            context.SaveChanges();
            context.Database.CloseConnection();
            return Redirect("/Clients/");
        }




        [HttpPost()]
        [Route("/Clients/nit")]
        public IActionResult getClient([FromBody] JsonDocument _data)
        {
            int nit = int.Parse(_data.RootElement.GetProperty("nit").GetString());
            var item = from user in context.Users
                       join city in context.Cities on user.City equals city.Id
                       join state in context.States on city.StateId equals state.Id
                       where user.Nit == nit
                       select new { user.Id, user.BusinessName,user.Nit, user.LocalAddress, user.Phone, city.CityName, state.StateName };
            Dictionary<string, object> data = new Dictionary<string,object>();
            data.Add("user", item.First());
            return this.Json(data);
        }
    }
 
}
