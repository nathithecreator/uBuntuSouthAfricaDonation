using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Data.SqlClient; // Import the SqlConnection namespace
using uBuntuSouthAfrica.Areas.Identity.Data;
using uBuntuSouthAfrica.Models;

namespace uBuntuSouthAfrica.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<uBuntuSouthAfricaUser> _userManager;

        public HomeController(ILogger<HomeController> logger, UserManager<uBuntuSouthAfricaUser> userManager)
        {
            _logger = logger;
            this._userManager = userManager;
        }

        public IActionResult Index()
        {
            ViewData["UserID"] = _userManager.GetUserId(this.User);

            int netAmount = 0; // Initialize netAmount to 0

            // Connect to your database and execute the SQL query to get NetAmount
            using (SqlConnection connection = new SqlConnection("Server=tcp:djpromorosebank1.database.windows.net,1433;Initial Catalog=DJPromoWebApp;Persist Security Info=False;User ID=djnathi;Password=Mamabolo777;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;"))
            {
                connection.Open();
                string sql = "SELECT SUM(CASE WHEN MoneyType = 'income' THEN Amount ELSE 0 END) - SUM(CASE WHEN MoneyType = 'expense' THEN Amount ELSE 0 END) AS NetAmount FROM Funds";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Read the NetAmount value from the result
                            if (!reader.IsDBNull(0))
                            {
                                netAmount = (int)reader.GetInt32(0);
                            }
                        }
                    }
                }
            }

            ViewData["NetAmount"] = netAmount; // Pass NetAmount to the view

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
