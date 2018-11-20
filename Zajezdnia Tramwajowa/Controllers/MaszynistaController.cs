using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Zajezdnia_Tramwajowa.Controllers
{
    public class MaszynistaAndPrzejazdy
    {
        public Maszynista m { get; set; }
        public IEnumerable<Przejazd> Przejazdy { get; set; }
    }
    public class MaszynistaAndPrzejazd
    {
        public Maszynista m { get; set; }
        public Przejazd p { get; set; }
    }
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
            List<Przejazd> p = db.Przejazd.ToList();
            p.RemoveAll(r => !r.IDMaszynisty.Equals(id));
            MaszynistaAndPrzejazdy d = new MaszynistaAndPrzejazdy { m = db.Maszynista.Find(id), Przejazdy = p };
            return View(d);
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
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(m).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(m);
            }
            catch
            {
                return View();
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
        public ActionResult Dodaj_przejazd()
        {
            return View();
        }
    }
}
