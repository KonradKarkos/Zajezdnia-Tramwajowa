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
    public class PrzystanekController : Controller
    {
        private ZajezdniaTramwajowaEntities db = new ZajezdniaTramwajowaEntities();

        // GET: Przystanek
        public ActionResult Index()
        {
            var przystanek = db.Przystanek.Include(p => p.Trasa);
            return View(przystanek.ToList());
        }

        // GET: Przystanek/Details/5
        public ActionResult Details(int? id)
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

        // GET: Przystanek/Create
        public ActionResult Create()
        {
            ViewBag.IDTrasy = new SelectList(db.Trasa, "IDTrasy", "NazwaTrasy");
            return View();
        }

        // POST: Przystanek/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDPrzystanku,IDTrasy,NazwaPrzystanku")] Przystanek przystanek)
        {
            if (ModelState.IsValid)
            {
                db.Przystanek.Add(przystanek);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IDTrasy = new SelectList(db.Trasa, "IDTrasy", "NazwaTrasy", przystanek.IDTrasy);
            return View(przystanek);
        }

        // GET: Przystanek/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.IDTrasy = new SelectList(db.Trasa, "IDTrasy", "NazwaTrasy", przystanek.IDTrasy);
            return View(przystanek);
        }

        // POST: Przystanek/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Przystanek przystanek)
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
        public ActionResult Delete(int? id)
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
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Przystanek przystanek = db.Przystanek.Find(id);
            db.Przystanek.Remove(przystanek);
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
    }
}
