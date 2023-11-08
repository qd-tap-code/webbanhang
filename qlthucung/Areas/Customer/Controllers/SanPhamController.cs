using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using qlthucung.Models;

namespace qlthucung.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class SanPhamController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly AppDbContext _context;

        public SanPhamController(AppDbContext context)
        {
            _context = context;
        }

        // GET: All SanPham
        public async Task<IActionResult> Index()
        {
            var sanpham = await _context.SanPhams.Take(12).ToListAsync();

            getSPNoiBat();
            getPhone();
            getApple();
            getSamsung();

            getPhoneOther();
            getLaptop();
            getTable();
            getGiaDung();
            getWatch();
            getPhuKienWatch();
            getPhuKienPhone();
            getPhuKienLaptop();
            getPhuKienOther();

            return View(sanpham);
        }

        //cac ham lay ra san pham
        #region
        //lay san pham noi bat
        private void getSPNoiBat()
        {
            var list = (from c in _context.SanPhams select c)
                .Take(10).ToList();
            ViewBag.getSPNoiBat = list;
        }
        //lay san pham là điện thoại id danh mục là 1
        private void getPhone()
        {
            var list = (from c in _context.SanPhams select c).Where(n => n.IdDanhmuc == 1)
                .Take(10).ToList();
            ViewBag.getPhone = list;
        }
        //lay san pham Iphone id =2
        private void getApple()
        {
            var list = (from c in _context.SanPhams select c).Where(n => n.IdDanhmuc == 2)
                .Take(10).ToList();
            ViewBag.getApple = list;
        }
        //lay san pham Samsung id = 3
        private void getSamsung()
        {
            var list = (from c in _context.SanPhams select c).Where(n => n.IdDanhmuc == 3)
                .Take(10).ToList();
            ViewBag.getSamsung = list;
        }
        private void getPhoneOther()
        {
            var list = (from c in _context.SanPhams select c).Where(n => n.IdDanhmuc == 1029)
                .Take(10).ToList();
            ViewBag.getPhoneOther = list;
        }
        //lay san pham laptop = 5
        private void getLaptop()
        {
            var list = (from c in _context.SanPhams select c).Where(n => n.IdDanhmuc == 8)
                .Take(10).ToList();
            ViewBag.getLaptop = list;
        }
        //lay san pham table
        private void getTable()
        {
            var list = (from c in _context.SanPhams select c).Where(n => n.IdDanhmuc == 1012)
                .Take(10).ToList();
            ViewBag.getTable = list;
        }
        // watch = 7
        private void getWatch()
        {
            var list = (from c in _context.SanPhams select c).Where(n => n.IdDanhmuc == 1016)
                .Take(10).ToList();
            ViewBag.getWatch = list;
        }
        private void getGiaDung()
        {
            var list = (from c in _context.SanPhams select c).Where(n => n.IdDanhmuc == 1020)
                .Take(10).ToList();
            ViewBag.getGiaDung = list;
        }
        private void getPhuKienPhone()
        {
            var list = (from c in _context.SanPhams select c).Where(n => n.IdDanhmuc == 1025)
                .Take(10).ToList();
            ViewBag.getPhuKienPhone = list;
        }
        private void getPhuKienLaptop()
        {
            var list = (from c in _context.SanPhams select c).Where(n => n.IdDanhmuc == 1026)
                .Take(10).ToList();
            ViewBag.getPhuKienLaptop = list;
        }
        private void getPhuKienOther()
        {
            var list = (from c in _context.SanPhams select c).Where(n => n.IdDanhmuc == 1027)
                .Take(10).ToList();
            ViewBag.getPhuKienOther = list;
        }
        private void getPhuKienWatch()
        {
            var list = (from c in _context.SanPhams select c).Where(n => n.IdDanhmuc == 1028)
                .Take(10).ToList();
            ViewBag.getPhuKienWatch = list;
        }
        #endregion

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var sanpham = await _context.SanPhams.FirstOrDefaultAsync(m => m.Masp == id);
            if (sanpham == null)
            {
                return NotFound();
            }

            //random san pham
            List<SanPham> products = _context.SanPhams.OrderBy(x => Guid.NewGuid()).Take(5).ToList();
            ViewBag.getSPRanDom = products;

            getThuVienAnhList(id);

            return View(sanpham);
        }

        private void getThuVienAnhList(int? id)
        {

            //sp vs thu vien anh
            List<SanPham> sanpham = _context.SanPhams.ToList();
            List<ThuVienAnh> thuvienanh = _context.ThuVienAnhs.ToList();
            var thu = from sp in sanpham
                      join tv in thuvienanh
                              on sp.Idthuvien equals tv.Idthuvien
                      where sp.Masp == id && sp.Idthuvien == tv.Idthuvien
                      select new ViewModel
                      {
                          sanpham = sp,
                          thuvienanh = tv
                      };

            ViewBag.getthuvienanh = thu;

        }

        [HttpGet]
        public async Task<IActionResult> Search(string search)
        {
            var searchProduct = from m in _context.SanPhams
                                select m;

            if (!string.IsNullOrEmpty(search))
            {
                searchProduct = searchProduct.Where(s => s.Tensp.Contains(search));
                if (!searchProduct.Any())
                {
                    TempData["nameProduct"] = search;
                    return RedirectToAction("NotFoundProduct", "SanPham");
                }
            }
            else
            {
                return RedirectToAction("NotFoundProduct", "SanPham");
            }

            TempData["nameProduct"] = search;
            return View(await searchProduct.ToListAsync());
        }

        public IActionResult NotFoundProduct()
        {
            return View();
        }

        public async Task<IActionResult> TatCaSanPham(int? pageNumber)
        {
            const int pageSize = 5;

            var products = _context.SanPhams.AsNoTracking();
            var paginatedProducts = await PaginatedList<SanPham>.CreateAsync(products, pageNumber ?? 1, pageSize);

            return View(paginatedProducts);
        }
    }
}
