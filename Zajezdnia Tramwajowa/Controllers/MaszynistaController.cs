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
            Przejazd p = db.Przejazd.Find(IdPrze);
            p.CzasOdjazdu = DateTime.Now;
            p.IDTramwaju = 0;
            p.IDTrasy = 0;
            return View(p);
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
        // GET: Przejazd/Edit/5
        public ActionResult Edytuj_przejazd(int id)
        {
            Przejazd przejazd = db.Przejazd.Single(emp => emp.IDPrzejazdu == id);
            return View(przejazd);
        }

        // POST: Przejazd/Edit/5
        [HttpPost]
        public ActionResult Edytuj_przejazd(int id, Przejazd przejazd)
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
            catch (Exception e)
            {
                MongoDBClient.InsertError(new ErrorMessage(e.InnerException.Message, DateTime.Now));
                ViewBag.Exception = e.InnerException.InnerException.Message;
                return View(przejazd);
            }
        }
    }
}