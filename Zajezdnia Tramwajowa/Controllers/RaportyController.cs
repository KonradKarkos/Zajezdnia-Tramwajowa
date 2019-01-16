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
    public class RaportyController : Controller
    {
        private ZajezdniaTramwajowaEntities db = new ZajezdniaTramwajowaEntities();

        // GET: Raporty
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NajczesciejUzywane()
        {
            return View(db.uzyciePrzystankows.OrderByDescending(o => o.Ilosc_Przejazdow).ToList().Take(5));
        }

        public ActionResult PrzejazdyMiesieczne()
        {
            Dictionary<Maszynista, int> Zestawienie = new Dictionary<Maszynista, int>();
            IEnumerable<Maszynista> m = db.Maszynista.ToList();
            string sqlQuery = "SELECT [dbo].[iloscPrzejazdow] (0,1)";
            foreach (var c in m)
            {
                sqlQuery = "SELECT [dbo].[iloscPrzejazdow] (" + c.IDMaszynisty + ")";
                Zestawienie.Add(c, db.Database.SqlQuery<int>(sqlQuery).FirstOrDefault());
            }
            return View(Zestawienie);
        }

        public ActionResult PrzejazdyMotorniczy()
        {
            Dictionary<Maszynista, int[]> Zestawienie = new Dictionary<Maszynista, int[]>();
            IEnumerable<Maszynista> m = db.Maszynista.ToList();
            int rok = DateTime.Now.Year;
            int[] dane;
            int? liczba;
            int miesiac = DateTime.Now.Month;
            foreach (var c in m)
            {
                dane = new int[3];
                liczba = db.Database.SqlQuery<int?>("SELECT [dbo].[totalTrips] (" + c.IDMaszynisty + "," + rok + ")").FirstOrDefault();
                dane[0] = liczba.Equals(null) ? 0 : (int)liczba;
                liczba = db.Database.SqlQuery<int?>("SELECT [dbo].[minTripsInYear] (" + c.IDMaszynisty + "," + rok + ")").FirstOrDefault();
                dane[1] = liczba.Equals(null) ? 0 : (int)liczba;
                liczba = db.Database.SqlQuery<int?>("SELECT [dbo].[maxTripsInYear] (" + c.IDMaszynisty + "," + rok + ")").FirstOrDefault() / miesiac;
                dane[2] = liczba.Equals(null) ? 0 : (int)liczba;
                Zestawienie.Add(c, dane);
            }
            return View(Zestawienie);
        }

        public ActionResult NajTramwaje()
        {
            Dictionary<int, int> Zestawienie = new Dictionary<int, int>();
            var tramwaj = db.Tramwaj.ToList();
            var przejazd = db.Tramwaj.ToList();


            var list = tramwaj.Join(przejazd,
                                    tramwaj_w => tramwaj_w.IDTramwaju,
                                    przejazd_w => przejazd_w.IDTramwaju,
                                    (tramwaj_w, przejazd_w) => new
                                    {
                                        tramwaj_w,
                                        przejazd_w
                                    }
                                        ).GroupBy(x => new { x.tramwaj_w.IDTramwaju }).Select(g => new { IDTramwaju = g.Key.IDTramwaju, Ilosc = g.Sum(x => x.tramwaj_w.Przejazd.Count) }).OrderByDescending(x => x.Ilosc).ToList().Take(5);

            foreach (var element in list)
            {
                Zestawienie.Add(element.IDTramwaju, element.Ilosc);
            }
            return View(Zestawienie);
        }
    }
}
