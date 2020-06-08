using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using QLKho.Models;
using QLKho.ViewModel;

namespace QLKho.Controllers
{
    public class SanPhamsController : Controller
    {
        

        // GET: SanPhams
        public ActionResult Index()
        {
            var db = new QLKhoContextEF();
            var lsSP = db.SanPhams.ToList();
            var lsSanPhamViewModel = from sp in db.SanPhams.ToList()
                                     join tg in db.Sp_LoaiSp
                                     on sp.MaSanPham equals tg.MaSanPham
                                     join tl in db.LoaiSanPhams
                                     on tg.MaLoaiSanPham equals tl.MaLoaiSanPham
                                     into gj
                                     from subpet in gj.DefaultIfEmpty()
                                     select new SanPhamViewModel
                                     {
                                         MaSanPham = sp.MaSanPham,
                                         DonGia = sp.DonGia,
                                         SoLuong = sp.SoLuong,
                                         TenSanPham = sp.TenSanPham,
                                         TenTheLoai = subpet.TenLoaiSanPham,
                                         NgaySua = sp.NgaySua
                                     };
                                     
            return View(lsSanPhamViewModel.ToList());
        }

        // GET: SanPhams/Details/5
        public ActionResult Details(string id)
        {
            var db = new QLKhoContextEF();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        // GET: SanPhams/Create
        public ActionResult Create()
        {
            var db = new QLKhoContextEF();
            List<LoaiSanPham> cate = db.LoaiSanPhams.ToList();

            // Tạo SelectList
            SelectList cateList = new SelectList(cate, "MaLoaiSanPham", "TenLoaiSanPham");

            // Set vào ViewBag
            ViewBag.CategoryList = cateList;
            return View();
        }

        // POST: SanPhams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaSanPham,TenSanPham,SoLuong,DonGia")] SanPham sanPham, string THELOAI)
        {
            var db = new QLKhoContextEF();
            if (ModelState.IsValid)
            {
                sanPham.NgayTao = DateTime.Now;
                sanPham.NgaySua = DateTime.Now;
                db.SanPhams.Add(sanPham);

                var SP_TL = new Sp_LoaiSp();
                SP_TL.MaSanPham = sanPham.MaSanPham;
                SP_TL.MaLoaiSanPham = THELOAI;

                db.Sp_LoaiSp.Add(SP_TL);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sanPham);
        }

        // GET: SanPhams/Edit/5
        public ActionResult Edit(string id)
        {
            var db = new QLKhoContextEF();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        // POST: SanPhams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaSanPham,TenSanPham,SoLuong,DonGia,NgayTao,NgaySua")] SanPham sanPham)
        {
            var db = new QLKhoContextEF();
            if (ModelState.IsValid)
            {
                db.Entry(sanPham).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sanPham);
        }

        // GET: SanPhams/Delete/5
        public ActionResult Delete(string id)
        {
            var db = new QLKhoContextEF();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        // POST: SanPhams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            var db = new QLKhoContextEF();
            SanPham sanPham = db.SanPhams.Find(id);
            db.SanPhams.Remove(sanPham);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            var db = new QLKhoContextEF();
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
