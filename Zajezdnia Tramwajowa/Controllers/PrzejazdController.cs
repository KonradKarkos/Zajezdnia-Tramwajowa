using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Zajezdnia_Tramwajowa.Controllers
{
    public class PrzejazdController : Controller
    {
        // GET: Przejazd
        public ActionResult Index()
        {
            return View();
        }

        // GET: Przejazd/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Przejazd/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Przejazd/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                   
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Przejazd/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Przejazd/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Przejazd/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Przejazd/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
