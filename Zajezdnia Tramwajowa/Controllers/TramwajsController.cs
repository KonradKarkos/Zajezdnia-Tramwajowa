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
    public class TramwajsController : Controller
    {
        private ZajezdniaTramwajowaEntities db = new ZajezdniaTramwajowaEntities();

        // GET: Tramwajs
        public ActionResult Index()
        {
            return View(db.Tramwaj.ToList());
        }

        // GET: Tramwajs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tramwajs/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDTramwaju,ModelPojazdu,RowVersion")] Tramwaj tramwaj)
        {
            if (ModelState.IsValid)
            {
                db.Tramwaj.Add(tramwaj);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tramwaj);
        }

        // GET: Tramwajs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tramwaj tramwaj = db.Tramwaj.Find(id);
            if (tramwaj == null)
            {
                return HttpNotFound();
            }
            return View(tramwaj);
        }

        // POST: Tramwajs/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Tramwaj tramwaj)
        {
            ViewBag.Exception = null;

            try
            {
                db.Entry(tramwaj).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException ex)
            {

                MongoDBClient.InsertError(new ErrorMessage("*** Błąd związany z blokowaniem optymistycznym ***", DateTime.Now));
                ViewBag.Exception = "Ktoś zmienił dane, proszę anuluj obecną operację w celu pobrania najnowszych danych";
                var entry = ex.Entries.Single();
                var clientValues = (Tramwaj)entry.Entity;
                var databaseEntry = entry.GetDatabaseValues();
                var databaseValues = (Tramwaj)databaseEntry.ToObject();



                if (databaseValues.ModelPojazdu != clientValues.ModelPojazdu)
                    ModelState.AddModelError("Model pojazdu", "Wartość w bazie danych: "
                        + databaseValues.ModelPojazdu);

                return View(tramwaj);
            }
        }

        // GET: Tramwajs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tramwaj tramwaj = db.Tramwaj.Find(id);
            if (tramwaj == null)
            {
                return HttpNotFound();
            }
            return View(tramwaj);
        }

        // POST: Tramwajs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tramwaj tramwaj = db.Tramwaj.Find(id);
            db.Tramwaj.Remove(tramwaj);
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
