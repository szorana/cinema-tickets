using CinemaTickets.Domain;
using CinemaTickets.Domain.Domain;
using CinemaTickets.Domain.Identity;
using ExcelDataReader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;

namespace CinemaTicket.Web.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserManager<TicketShopApplicationUser> _userManager;

        public UserController(UserManager<TicketShopApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("[action]")]
        public IActionResult ImportUsers(IFormFile file)
        {
            if (file != null)
            {
                //make a copy
                string pathToUpload = $"{Directory.GetCurrentDirectory()}\\files\\{file.FileName}";


                using (FileStream fileStream = System.IO.File.Create(pathToUpload))
                {
                    file.CopyTo(fileStream);

                    fileStream.Flush();
                }

                //read data from uploaded file

                List<UserImport> users = getUsersFromExcelFile(file.FileName);

                foreach (var item in users)
                {
                    var userCheck = _userManager.FindByEmailAsync(item.Email).Result;
                    if (userCheck == null)
                    {
                        var user = new TicketShopApplicationUser
                        {
                            UserName = item.Email,
                            NormalizedUserName = item.Email,
                            Email = item.Email,
                            EmailConfirmed = true,
                            PhoneNumberConfirmed = true,
                            UserCart = new Cart()
                        };
                        var result = _userManager.CreateAsync(user, item.Password).Result;
                        if (item.Role.ToLower() == "admin")
                            _userManager.AddClaimAsync(user, new Claim("UserRole", "Admin"));
                        else
                            _userManager.AddClaimAsync(user, new Claim("UserRole", "User"));
                        _userManager.UpdateAsync(user);
                    }
                    else
                    {
                        continue;
                    }
                }
            }

            return RedirectToAction("Index", "Order");
        }

        private List<UserImport> getUsersFromExcelFile(string fileName)
        {

            string pathToFile = $"{Directory.GetCurrentDirectory()}\\files\\{fileName}";

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            List<UserImport> userList = new List<UserImport>();

            using (var stream = System.IO.File.Open(pathToFile, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read())
                    {
                        userList.Add(new UserImport
                        {
                            Email = reader.GetValue(0).ToString(),
                            Password = reader.GetValue(1).ToString(),
                            Role = reader.GetValue(2).ToString(),
                        });
                    }
                }
            }

            return userList;

        }
    }
}
