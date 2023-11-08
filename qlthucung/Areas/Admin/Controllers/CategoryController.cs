using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using qlthucung.Models;
using System.Linq;
using System.Threading.Tasks;

namespace qlthucung.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        // Hiển thị danh sách danh mục
        public async Task<IActionResult> Index()
        {
            var danhMucList = await _context.DanhMucs.ToListAsync();

            return View(danhMucList);
        }

        // Hiển thị trang tạo mới danh mục
        public IActionResult Create()
        {
            return View();
        }

        // Xử lý tạo mới danh mục
        [HttpPost]
        public async Task<IActionResult> Create(DanhMuc danhMuc)
        {
            if (ModelState.IsValid)
            {
                _context.Add(danhMuc);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(danhMuc);
        }

        // Hiển thị trang chỉnh sửa danh mục
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var danhMuc = await _context.DanhMucs.FindAsync(id);

            if (danhMuc == null)
            {
                return NotFound();
            }

            return View(danhMuc);
        }

        // Xử lý chỉnh sửa danh mục
        [HttpPost]
        public async Task<IActionResult> Edit(int id, DanhMuc danhMuc)
        {
            if (id != danhMuc.IdDanhmuc)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(danhMuc);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DanhMucExists(danhMuc.IdDanhmuc))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(danhMuc);
        }

        // Hiển thị trang xóa danh mục
        public IActionResult Xoa(string id)
        {
            // Chuyển đổi Guid thành string trước khi sử dụng


            if (id == null)
            {
                return NotFound();
            }

            DanhMuc danhMuc = _context.DanhMucs.Find(id);

            if (danhMuc == null)
            {
                return NotFound();
            }

            return View(danhMuc);
        }


        [HttpPost, ActionName("Xoa")]
        [ValidateAntiForgeryToken]
        public IActionResult XoaConfirmed(string id)
        {
            DanhMuc danhMuc = _context.DanhMucs.Find(id);

            if (danhMuc == null)
            {
                return NotFound();
            }

            _context.DanhMucs.Remove(danhMuc);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

    private bool DanhMucExists(int id)
    {
        return _context.DanhMucs.Any(e => e.IdDanhmuc == id);
    }
}
}
