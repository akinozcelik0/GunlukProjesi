using Gunluk.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Gunluk.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class GonderilerController : Controller
    {
        private readonly UygulamaDbContext _db;

        public GonderilerController(UygulamaDbContext db)
        {
            _db = db;
        }

        public IActionResult Index(string? durum)
        {
            ViewBag.Mesaj = durum == "eklendi" ? "Gonderi Basariyla Olusturuldu":
                            durum == "duzenlendi" ? "Gonderiniz Basariyla Guncellendi":
                            durum == "silindi" ? "Gonderi Basariyla Silindi" : null;

            return View(_db.Gonderiler.Include(x => x.Kategori).ToList());
        }

        //YENI-----------=-=-=-=-=-=-=
        public IActionResult Yeni()
        {
            KategorileriYukle();
            return View("Yonet");
        }

        private void KategorileriYukle()
        {
            ViewBag.Kategoriler = _db.Kategoriler
                .Select(x => new SelectListItem(){ Value = x.Id.ToString(), Text = x.Ad }).ToList();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Yeni(GonderiViewModel vm)
        {

            if (ModelState.IsValid)
            {
                var gonderi = new Gonderi()
                {
                    Baslik = vm.Baslik,
                    Icerik = vm.Icerik,
                    KategoriId = vm.KategoriId!.Value
                };
                _db.Gonderiler.Add(gonderi);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index), new {durum = "eklendi"});
            }
            KategorileriYukle();
            return View("Yonet");
        }

        //DUZENLE-----------=-=-=-=-=-=-=
        public IActionResult Duzenle(int id)
        {
            var gonderi = _db.Gonderiler.Find(id);

            if (gonderi == null)
                return NotFound();

            var vm = new GonderiViewModel()
            {
                Id = gonderi.Id,
                Baslik = gonderi.Baslik,
                Icerik = gonderi.Icerik,
                KategoriId = gonderi.KategoriId

            };

            KategorileriYukle();
            return View("Yonet",vm);
        }



        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Duzenle(GonderiViewModel vm)
        {

            if (ModelState.IsValid)
            {
                var gonderi = _db.Gonderiler.Find(vm.Id);

                if (gonderi == null)
                {
                    return NotFound();
                }

                gonderi.Baslik = vm.Baslik;
                gonderi.Icerik = vm.Icerik;
                gonderi.DegistirilmeZamani = DateTime.Now;
                gonderi.KategoriId = vm.KategoriId!.Value;
                
                _db.SaveChanges();
                return RedirectToAction(nameof(Index), new { durum = "duzenlendi" });
            }
            KategorileriYukle();
            return View("Yonet");
        }

        [HttpPost,ValidateAntiForgeryToken]
        public IActionResult Sil(int id)
        {
            var gonderi = _db.Gonderiler.Find(id);

            if (gonderi == null)
            {
                return NotFound();
            }

            _db.Gonderiler.Remove(gonderi);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index), new { durum = "silindi" });
        }





    }
}
