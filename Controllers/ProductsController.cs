using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using WebAppluisChaves.Models;
using WebAppluisChaves.ViewModels;

namespace WebAppluisChaves.Controllers
{
    public class ProductsController : Controller
    {
        private readonly BillingContext context;
        public ProductsController(BillingContext context) {
            this.context = context;
        }
        public IActionResult Index()
        {
            var products = context.Products.ToList();
            return View(products);
        }
        [HttpGet]
        public async Task<IActionResult> newProduct()
        {
            return View("newProduct");
        }
        [HttpPost]
        public async Task<IActionResult> create()
        {
            var form = Request.Form;
            if(form == null)
            {
                return RedirectToAction("newProduct");
            }
            Product product = new Product();
            product.ProductName = form["product-name"].ToString().ToLower();
            product.Price = int.Parse(form["product-price"]);
            product.Amount = int.Parse(form["product-amount"]);
            product.Discount = int.Parse(form["product-discount"]);
            context.Products.Add(product);
            context.SaveChanges();
            context.Database.CloseConnection();
            return RedirectToAction("Index");
        }

        [HttpGet()]
        public IActionResult edit(int id)
        {
            var products = context.Products.FirstOrDefault(p=> p.Id==id);
            return View("newProduct",products);
        }

        [HttpPost]
        public IActionResult editAction()
        {   var form = Request.Form;
            if(form == null)
            {
                return RedirectToAction("Index");
            }
            int id = int.Parse(form["id"]);
            var products = context.Products.FirstOrDefault(p => p.Id == id);
            if(products == null)
            {
                return RedirectToAction("Index");
            }
            if(products.ProductName!= form["product-name"].ToString().ToLower())
            {
                products.ProductName = form["product-name"].ToString().ToLower();
            }
            if (products.Price != form["product-price"])
            {
                products.Price = int.Parse(form["product-price"]);
            }
            if (products.Amount != form["product-amount"])
            {
                products.Amount = int.Parse(form["product-amount"]);
            }
            int discount = int.Parse(form["product-discount"]);
            if (discount < 0) { 
                discount =0;
            }
            if (discount>100) { 
                discount =100;
            }
            if (products.Discount != discount)
            {   
                products.Discount = discount;
            }

            context.Products.Update(products);
            context.SaveChanges();
            context.Database.CloseConnection();

            return RedirectToAction("Index");
        }

        [HttpGet()]
        public IActionResult delete(int id)
        {   
            var products = context.Products.FirstOrDefault(x => x.Id == id);
            if(products == null)
            {
                return RedirectToAction("Index");
            }
            context.Remove(products);
            context.SaveChanges();
            context.Database.CloseConnection();
            return RedirectToAction("Index");
        }

        [HttpGet()]
        [Route("/product/{name}")]
        public IActionResult getClient(string name)
        {
            Product product = context.Products.FirstOrDefault(p=> p.ProductName.Contains(name)); 
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("user",product);
            return this.Json(data);
        }
    }
}
