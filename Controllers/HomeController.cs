using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text.Json;
using WebAppluisChaves.Models;

namespace WebAppluisChaves.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BillingContext billingContext;

        public HomeController(ILogger<HomeController> logger, BillingContext billingContext)
        {
            _logger = logger;
            this.billingContext = billingContext;
        }

        public IActionResult Index()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        
        [HttpGet()]
        [Route("/cities/{id}")]
        public async Task<IActionResult> getCities(int id)
        { 
            var cities =await  this.billingContext.Cities.Where(x => x.StateId == id).ToListAsync();
            this.billingContext.Database.CloseConnection();
            return Ok(cities);
        }

        [HttpPost()]
        [Route("/billing/new")]
        public  IActionResult billing([FromBody] JsonDocument _data)
        {   
            int userId = int.Parse(_data.RootElement.GetProperty("user").ToString());
            int total = int.Parse(_data.RootElement.GetProperty("total").ToString());
            string productList = _data.RootElement.GetProperty("listObject").ToString();
            Bill bill = new Bill();
            bill.IdUser= userId;
            bill.Amount = total;
            bill.ListObject= productList;
            this.billingContext.Bills.Add(bill);
            this.billingContext.SaveChanges();
            this.billingContext.Database.CloseConnection();
            Dictionary<string,object> data = new Dictionary<string,object>();
            data.Add("bill", bill);

            return this.Json(data);
        }
    }
}