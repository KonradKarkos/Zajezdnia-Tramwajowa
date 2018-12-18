using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Zajezdnia_Tramwajowa;

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
        public ActionResult Edit([Bind(Include = "IDTrasy,NazwaTrasy")] Trasa trasa)
        {
            if (ModelState.IsValid)
            {
                db.Entry(trasa).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(trasa);
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
    }
}
