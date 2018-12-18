using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zajezdnia_Tramwajowa.MongoDB;

namespace Zajezdnia_Tramwajowa.Controllers
{
    public class PrzejazdController : Controller
    {
        ZajezdniaTramwajowaEntities db = new ZajezdniaTramwajowaEntities();
        // GET: Przejazd
        public ActionResult Index()
        {
            return View(db.Przejazd.ToList());
        }

        // GET: Przejazd/Details/5
        public ActionResult Details(int id)
        {
            return View(db.Przejazd.Find(id));
        }

        // GET: Przejazd/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Przejazd/Create
        [HttpPost]
        public ActionResult Create(Przejazd przejazd)
        {
            ViewBag.Exception = null;
            try
            {
                if (ModelState.IsValid)
                {
                    db.Przejazd.Add(przejazd);
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                MongoDBClient.InsertError(new ErrorMessage(e.InnerException.Message, DateTime.Now));
                ViewBag.Exception = "Błędne dane";
                return View(przejazd);
            }
        }

        // GET: Przejazd/Edit/5
        public ActionResult Edit(int id)
        {
            Przejazd przejazd = db.Przejazd.Single(emp => emp.IDPrzejazdu == id);
            return View(przejazd);
        }

        // POST: Przejazd/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Przejazd przejazd)
        {
            ViewBag.Exception = null;
            try
            {
                db.Entry(przejazd).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException ex)
            {

                MongoDBClient.InsertError(new ErrorMessage(ex.InnerException.Message, DateTime.Now));
                ViewBag.Exception = "Ktoś zmienił dane, proszę anuluj obecną operację w celu pobrania najnowszych danych";
                var entry = ex.Entries.Single();
                var clientValues = (Przejazd)entry.Entity;
                var databaseEntry = entry.GetDatabaseValues();
                var databaseValues = (Przejazd)databaseEntry.ToObject();



                if (databaseValues.IDMaszynisty != clientValues.IDMaszynisty)
                    ModelState.AddModelError("ID Motorniczego", "Wartość w bazie danych: "
                        + databaseValues.IDMaszynisty);
                if (databaseValues.IDTramwaju != clientValues.IDTramwaju)
                    ModelState.AddModelError("ID Tramwaju", "Wartość w bazie danych: "
                        + databaseValues.IDTramwaju);
                if (databaseValues.IDTrasy != clientValues.IDTrasy)
                    ModelState.AddModelError("ID Trasy", "Wartość w bazie danych: " + databaseValues.IDTrasy);
                if (databaseValues.CzasOdjazdu != clientValues.CzasOdjazdu)
                    ModelState.AddModelError("Czas Odjazdu", "Wartość w bazie danych: " + databaseValues.CzasOdjazdu);

                return View(przejazd);
            }
        }

        // GET: Przejazd/Delete/5
        public ActionResult Delete(int id)
        {
            return View(db.Przejazd.Find(id));
        }

        // POST: Przejazd/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Przejazd p)
        {
            try
            {
                db.Przejazd.Remove(db.Przejazd.Find(id));
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View(p);
            }
        }
    }
}
