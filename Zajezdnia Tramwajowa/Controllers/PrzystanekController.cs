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
        public ActionResult Edit([Bind(Include = "IDPrzystanku,IDTrasy,NazwaPrzystanku")] Przystanek przystanek)
        {
            if (ModelState.IsValid)
            {
                db.Entry(przystanek).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDTrasy = new SelectList(db.Trasa, "IDTrasy", "NazwaTrasy", przystanek.IDTrasy);
            return View(przystanek);
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
