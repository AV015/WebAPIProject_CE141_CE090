using MedicalStoreWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MedicalStoreWebApi.Controllers
{
    public class MedicineController : ApiController
    {
        [HttpGet]
        public IEnumerable<Medicine> Get()
        {
            using (MedicineDBContext context = new MedicineDBContext())
            {
                return context.Medicines.ToList();
            }
        }
        [HttpGet]
        public Medicine Get(int id)
        {
            using (MedicineDBContext context = new MedicineDBContext())
            {
                return context.Medicines.FirstOrDefault(e => e.Id == id);
            }
        }

        [HttpPost]
        public IHttpActionResult Post(Medicine m)
        {
            using (MedicineDBContext context = new MedicineDBContext())
            {
                context.Medicines.Add(new Medicine()
                {
                    MedicineName = m.MedicineName,
                    Stock = m.Stock,
                    Price = m.Price
                });
                
                context.SaveChanges();
                //var data = context.Medicines.ToList();
            }
            return Ok();
        }

        [HttpPut]
        public IHttpActionResult Put(Medicine m)
        {
            using (MedicineDBContext context = new MedicineDBContext())
            {
                Medicine data = context.Medicines.Where(e => e.Id == m.Id).FirstOrDefault<Medicine>();

                if (data != null)
                {
                    data.MedicineName = m.MedicineName;
                    data.Stock = m.Stock;
                    data.Price = m.Price;
                    context.SaveChanges();
                }
                else
                {
                    return NotFound();
                }

                return Ok();
            }
        }

        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            using (MedicineDBContext context = new MedicineDBContext())
            {
                Medicine m = context.Medicines.FirstOrDefault(e => e.Id == id);
                context.Medicines.Remove(m);
                context.SaveChanges();
                return Ok();
            }
        }


    }
}
