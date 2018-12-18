using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zajezdnia_Tramwajowa.MongoDB;

namespace Zajezdnia_Tramwajowa.Controllers
{
    public class MaszynistaController : Controller
    {
        ZajezdniaTramwajowaEntities db = new ZajezdniaTramwajowaEntities();

        // GET: Maszynista
        public ActionResult Index()
        {
            return View(db.Maszynista.ToList());
        }

        // GET: Maszynista/Details
        public ActionResult Details(int id)
        {
            return View(db.Maszynista.Find(id));
        }

        // GET: Maszynista/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Maszynista/Create
        [HttpPost]
        public ActionResult Create(Maszynista maszynista)
        {
            ViewBag.Exception = null;
            try
            {
                db.InsertMaszynista(maszynista.Stawka, maszynista.Imie, maszynista.Nazwisko);
            }
            catch (Exception e)
            {

                String innerMessage = (e.InnerException != null)
                  ? e.InnerException.Message
                  : "Błędne dane";
                ViewBag.Exception = innerMessage;
                MongoDBClient.InsertError(new ErrorMessage(e.InnerException.Message, DateTime.Now));

                return View(maszynista);
            }
            return RedirectToAction("Index");
        }

        // GET: Maszynista/Edit/5
        public ActionResult Edit(int id)
        {
            Maszynista m = db.Maszynista.Single(emp => emp.IDMaszynisty == id);
            return View(m);
        }

        // POST: Maszynista/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Maszynista m)
        {


            ViewBag.Exception = null;

            try
            {
                db.Entry(m).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException ex)
            {

                MongoDBClient.InsertError(new ErrorMessage(ex.InnerException.Message, DateTime.Now));
                ViewBag.Exception = "Ktoś zmienił dane, proszę anuluj obecną operację w celu pobrania najnowszych danych";
                var entry = ex.Entries.Single();
                var clientValues = (Maszynista)entry.Entity;
                var databaseEntry = entry.GetDatabaseValues();
                var databaseValues = (Maszynista)databaseEntry.ToObject();



                if (databaseValues.Stawka != clientValues.Stawka)
                    ModelState.AddModelError("Stawka", "Wartość w bazie danych: "
                        + databaseValues.Stawka);
                if (databaseValues.Imie != clientValues.Imie)
                    ModelState.AddModelError("Imie", "Wartość w bazie danych: "
                        + databaseValues.Imie);
                if (databaseValues.Nazwisko != clientValues.Nazwisko)
                    ModelState.AddModelError("Nazwisko", "Wartość w bazie danych: " + databaseValues.Nazwisko);

                return View(m);
            }

        }

        // GET: Maszynista/Delete/5
        public ActionResult Delete(int id)
        {
            Maszynista m = db.Maszynista.Find(id);
            return View(m);
        }

        // POST: Maszynista/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Maszynista m)
        {
            try
            {
                db.Maszynista.Remove(db.Maszynista.Single(ma => ma.IDMaszynisty == id));
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View(m);
            }
        }
        public ActionResult Dodaj_przejazd(int IdPrze)
        {
            return View(db.Przejazd.Find(IdPrze));
        }

        [HttpPost]
        public ActionResult Dodaj_przejazd(Przejazd przejazd)
        {
            ViewBag.Exception = null;
            try
            {
                if (ModelState.IsValid)
                {
                    db.Przejazd.Add(przejazd);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                String innerMessage = (e.InnerException != null)
                  ? e.InnerException.Message
                  : "Błędne dane";
                ViewBag.Exception = innerMessage;
                MongoDBClient.InsertError(new ErrorMessage(e.InnerException.Message, DateTime.Now));
                przejazd.Maszynista = db.Maszynista.Single(ma => ma.IDMaszynisty == przejazd.IDMaszynisty);
                return View(przejazd);
            }
            return RedirectToAction("Details", new { id = przejazd.IDMaszynisty });
        }
        // GET: Maszynista/Delete/5
        public ActionResult Usun_przejazd(int id)
        {
            Przejazd m = db.Przejazd.Find(id);
            return View(m);
        }

        // POST: Maszynista/Delete/5
        [HttpPost]
        public ActionResult Usun_przejazd(int id, Przejazd m)
        {
            try
            {
                db.Przejazd.Remove(db.Przejazd.Find(id));
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View(m);
            }
        }
    }
}