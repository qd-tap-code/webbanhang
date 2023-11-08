using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using qlthucung.Models;
using Microsoft.AspNetCore.Authorization;
using qlthucung.Security;
using System;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace qlthucung.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppIdentityUser> userManager;
        private readonly RoleManager<AppIdentityRole> roleManager;
        private readonly SignInManager<AppIdentityUser> signInManager;
        private readonly AppDbContext _context;
        public AccountController(UserManager<AppIdentityUser> userManager,
            RoleManager<AppIdentityRole> roleManager,
            SignInManager<AppIdentityUser> signInManager,
            AppDbContext context)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
            _context = context;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Register register)
        {
            if (ModelState.IsValid)
            {
                if (!roleManager.RoleExistsAsync("User").Result) //staff và chỉnh role bên này
                {
                    var role = new AppIdentityRole();
                    role.Name = "User"; //staff
                    role.Description = "User can Perform CRUD Employee";
                    var roleResult = roleManager.CreateAsync(role).Result;
                }

                var user = new AppIdentityUser();
                user.UserName = register.UserName;
                user.Email = register.Email;
                user.FullName = register.FullName;
                user.BirthDate = register.BirthDate;

                var result = userManager.CreateAsync(user, register.Password).Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "User").Wait(); //"User" thay bằng staff
                    return RedirectToAction("Index", "Account");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(register);
        }
        [HttpGet]
        public IActionResult Edit(string id)
        {
            var user = userManager.FindByIdAsync(id).Result;
            if (user != null)
            {
                // Lấy dữ liệu người dùng và truyền cho view
                var model = new AspNetUser
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    FullName = user.FullName,
                    BirthDate = user.BirthDate,
                    // Thêm các trường dữ liệu khác tại đây
                };

                return View(model);
            }

            // Xử lý trường hợp không tìm thấy người dùng
            return RedirectToAction("Index", "Account");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(AspNetUser model)
        {
            if (ModelState.IsValid)
            {
                var user = userManager.FindByIdAsync(model.Id).Result;
                if (user != null)
                {
                    user.UserName = model.UserName;
                    user.Email = model.Email;
                    user.FullName = model.FullName;
                    user.BirthDate = model.BirthDate;
                    // Cập nhật các trường dữ liệu khác tại đây

                    var result = userManager.UpdateAsync(user).Result;

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Account");
                    }
                    else
                    {
                        // Xử lý lỗi nếu có
                    }
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Index()
        {
            var users = userManager.Users.ToList();
            var aspNetUsers = users.Select(u => new AspNetUser
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email,
                FullName = u.FullName,
                BirthDate = u.BirthDate,
                // Các trường dữ liệu khác nếu cần
            }).ToList();

            return View(aspNetUsers);
        }
        public IActionResult Delete(string id)
        {
            // Chuyển đổi Guid thành string trước khi sử dụng


            AspNetUser user = _context.AspNetUsers.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {
            // Chuyển đổi Guid thành string trước khi sử dụng

            AspNetUser user = _context.AspNetUsers.Find(id);
            if (user != null)
            {
                _context.AspNetUsers.Remove(user);
                _context.SaveChanges();
                return RedirectToAction("Index"); // Chuyển hướng đến trang chính hoặc trang danh sách người dùng sau khi xóa thành công.
            }
            else
            {
                return NotFound(); // Trả về NotFound nếu không tìm thấy người dùng
            }
        }
    }
}