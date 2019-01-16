using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Zajezdnia_Tramwajowa;
using Zajezdnia_Tramwajowa.MongoDB;

namespace Zajezdnia_Tramwajowa.Controllers
{
    public class TrasaController : Controller
    {
        private ZajezdniaTramwajowaEntities db = new ZajezdniaTramwajowaEntities();

        // GET: Trasa
        public ActionResult Index()
        {
            return View(db.Trasa.ToList());
        }

        // GET: Trasa/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trasa trasa = db.Trasa.Find(id);
            if (trasa == null)
            {
                return HttpNotFound();
            }
            return View(trasa);
        }

        // GET: Trasa/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Trasa/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDTrasy,NazwaTrasy")] Trasa trasa)
        {
            if (ModelState.IsValid)
            {
                db.Trasa.Add(trasa);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(trasa);
        }

        // GET: Trasa/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trasa trasa = db.Trasa.Find(id);
            if (trasa == null)
            {
                return HttpNotFound();
            }
            return View(trasa);
        }

        // POST: Trasa/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Trasa trasa)
        {
            ViewBag.Exception = null;
            try
            {
                db.Entry(trasa).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException ex)
            {

                MongoDBClient.InsertError(new ErrorMessage("*** Błąd związany z blokowaniem optymistycznym ***", DateTime.Now));
                ViewBag.Exception = "Ktoś zmienił dane, proszę anuluj obecną operację w celu pobrania najnowszych danych";
                var entry = ex.Entries.Single();
                var clientValues = (Trasa)entry.Entity;
                var databaseEntry = entry.GetDatabaseValues();
                var databaseValues = (Trasa)databaseEntry.ToObject();



                if (databaseValues.NazwaTrasy != clientValues.NazwaTrasy)
                    ModelState.AddModelError("Nazwa Trasy", "Wartość w bazie danych: "
                        + databaseValues.NazwaTrasy);

                return View(trasa);
            }
            catch (Exception e)
            {
                MongoDBClient.InsertError(new ErrorMessage(e.InnerException.Message, DateTime.Now));
                ViewBag.Exception = e.InnerException.InnerException.Message;
                return View(trasa);
            }
        }

        // GET: Trasa/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trasa trasa = db.Trasa.Find(id);
            if (trasa == null)
            {
                return HttpNotFound();
            }
            return View(trasa);
        }

        // POST: Trasa/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Trasa trasa = db.Trasa.Find(id);
            db.Trasa.Remove(trasa);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult Dodaj_przystanek(int IdPrz)
        {
            Przystanek p = db.Przystanek.Find(IdPrz);
            p.NazwaPrzystanku = "Nowy przystanek";
            return View(p);
        }

        [HttpPost]
        public ActionResult Dodaj_przystanek(Przystanek przystanek)
        {
            ViewBag.Exception = null;
            try
            {
                if (ModelState.IsValid)
                {
                    db.Przystanek.Add(przystanek);
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
                przystanek.Trasa = db.Trasa.Single(t => t.IDTrasy == przystanek.IDTrasy);
                return View(przystanek);
            }
            return RedirectToAction("Details", new { id = przystanek.IDTrasy });
        }
        public ActionResult Edytuj_przystanek(int id)
        {
            Przystanek przystanek = db.Przystanek.Find(id);
            if (przystanek == null)
            {
                return HttpNotFound();
            }
            return View(przystanek);
        }

        // POST: Przystanek/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edytuj_przystanek(int id, Przystanek przystanek)
        {
            ViewBag.Exception = null;
            try
            {
                db.Entry(przystanek).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException ex)
            {

                MongoDBClient.InsertError(new ErrorMessage(ex.InnerException.Message, DateTime.Now));
                ViewBag.Exception = "Ktoś zmienił dane, proszę anuluj obecną operację w celu pobrania najnowszych danych";
                var entry = ex.Entries.Single();
                var clientValues = (Przystanek)entry.Entity;
                var databaseEntry = entry.GetDatabaseValues();
                var databaseValues = (Przystanek)databaseEntry.ToObject();



                if (databaseValues.IDTrasy != clientValues.IDTrasy)
                    ModelState.AddModelError("ID Trasy", "Wartość w bazie danych: "
                        + databaseValues.IDTrasy);
                if (databaseValues.NazwaPrzystanku != clientValues.NazwaPrzystanku)
                    ModelState.AddModelError("Nazwa Przystanku", "Wartość w bazie danych: "
                        + databaseValues.NazwaPrzystanku);

                return View(przystanek);
            }
            catch (Exception e)
            {
                MongoDBClient.InsertError(new ErrorMessage(e.InnerException.Message, DateTime.Now));
                ViewBag.Exception = e.InnerException.InnerException.Message;
                return View(przystanek);
            }
        }

        // GET: Przystanek/Delete/5
        public ActionResult Usun_przystanek(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Przystanek przystanek = db.Przystanek.Find(id);
            if (przystanek == null)
            {
                return HttpNotFound();
            }
            return View(przystanek);
        }

        // POST: Przystanek/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Usun_przystanek(int id)
        {
            Przystanek przystanek = db.Przystanek.Find(id);
            db.Przystanek.Remove(przystanek);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
