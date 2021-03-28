using MedicalStoreWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MedicalStoreWebApi.Controllers
{
    [Authorize]
    public class ConsumeController : Controller
    {
        // GET: Consume
        HttpClient hc = new HttpClient();
        MedicineDBContext db = new MedicineDBContext();

        [HttpGet]
        public ActionResult Index()
        {
            Auth.auth = true;
            List<Medicine> list = new List<Medicine>();
            hc.BaseAddress = new Uri("https://localhost:44398/api/");
            var consume = hc.GetAsync("Medicine");
            consume.Wait();
            var test = consume.Result;
            if (test.IsSuccessStatusCode)
            {
                var display = test.Content.ReadAsAsync<List<Medicine>>();
                list = display.Result;
            }
            return View(list);
            
        }
        [HttpGet]
        public ActionResult Details(int id)
        {
            hc.BaseAddress = new Uri("https://localhost:44398/api/Medicine/");
            Medicine m = new Medicine();
            var consume = hc.GetAsync("Get?id=" + id.ToString());
            consume.Wait();
            var test = consume.Result;
            if (test.IsSuccessStatusCode)
            {
                var display = test.Content.ReadAsAsync<Medicine>();
                display.Wait();
                m = display.Result;
            }
            return View(m);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Medicine m)
        {
            hc.BaseAddress = new Uri("https://localhost:44398/api/Medicine/");
            var consume = hc.PostAsJsonAsync("Post", m);
            consume.Wait();
            var test = consume.Result;
            if (test.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            hc.BaseAddress = new Uri("https://localhost:44398/api/Medicine/");
            Medicine m = new Medicine();
            var consume = hc.GetAsync("Get?id=" + id.ToString());
            consume.Wait();
            var test = consume.Result;
            if (test.IsSuccessStatusCode)
            {
                var display = test.Content.ReadAsAsync<Medicine>();
                display.Wait();
                m = display.Result;
            }
            return View(m);
        }

        [HttpPost]
        public ActionResult Edit(Medicine m)
        {
            hc.BaseAddress = new Uri("https://localhost:44398/api/Medicine/");
            var consume = hc.PutAsJsonAsync("Put", m);
            consume.Wait();
            var test = consume.Result;
            if (test.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult Delete(int id)
        {
            hc.BaseAddress = new Uri("https://localhost:44398/api/Medicine/");
            var consume = hc.DeleteAsync("Delete?id=" + id.ToString());
            consume.Wait();
            var test = consume.Result;
            if (test.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return HttpNotFound();
            }
        }

        public ActionResult History()
        {
            List<Purchase> list = new List<Purchase>();
            hc.BaseAddress = new Uri("https://localhost:44398/api/");
            var consume = hc.GetAsync("Purchases");
            consume.Wait();
            var test = consume.Result;
            if (test.IsSuccessStatusCode)
            {
                var display = test.Content.ReadAsAsync<List<Purchase>>();
                list = display.Result;
            }
            return View(list);

        }

        [HttpGet]
        public ActionResult Buy()
        {
            string[] medicine = db.Medicines.Select(e => e.MedicineName).ToArray();
            ViewBag.mname = medicine;
            return View();
        }

        [HttpPost]
        public ActionResult Buy(Purchase p)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();
           
            int amount = 0, total = 0, qty = 0;
            qty = p.Quantity;
            p.UserId = 0;
            p.Date = DateTime.Now;
            amount = db.Medicines.Where(e => e.MedicineName == p.MedicineName).Select(e => e.Price).FirstOrDefault();
            total = p.Quantity * amount;
            p.Amount = total;
            Medicine data = db.Medicines.Where(e => e.MedicineName == p.MedicineName).FirstOrDefault();
            if(data != null)
            {
                data.Stock = data.Stock - qty;
                db.SaveChanges();
            }
            
            hc.BaseAddress = new Uri("https://localhost:44398/api/Purchases/");
            var consume = hc.PostAsJsonAsync("PostPurchase", p);
            consume.Wait();
            var test = consume.Result;
            if (test.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return HttpNotFound();
            }
        }
    }
}