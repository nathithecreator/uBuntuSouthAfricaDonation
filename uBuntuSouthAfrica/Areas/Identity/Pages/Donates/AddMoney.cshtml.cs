using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using uBuntuSouthAfrica.Pages.Donates;
using System;
using System.Data.SqlClient;

namespace uBuntuSouthAfrica.Pages.Donates
{
    public class AddMoneyModel : PageModel
    {
        public DonateMoneyInfo donateMoneyInfo = new DonateMoneyInfo();
        public string errorMessage = "";
        public string successMessage = "";

        public void OnGet()
        {
        }

        public void OnPost()
        {
            donateMoneyInfo.id = Request.Form["id"];
            donateMoneyInfo.donorName = Request.Form["name"];
            donateMoneyInfo.amount = Request.Form["amount"];

            // Check if donorName is empty or null, and set it to anonymous if true
            if (string.IsNullOrEmpty(donateMoneyInfo.donorName))
            {
                donateMoneyInfo.donorName = "Anonymous";
            }

            if (string.IsNullOrEmpty(donateMoneyInfo.amount))
            {
                errorMessage = "An amount is required";
                return;
            }

            // Save the new donation into the database
            try
            {
                string connectionString = "Server=tcp:djpromorosebank1.database.windows.net,1433;Initial Catalog=DJPromoWebApp;Persist Security Info=False;User ID=djnathi;Password=Mamabolo777;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;";
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);
                builder.TrustServerCertificate = true;
                connectionString = builder.ConnectionString;
                
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO MoneyDonations (DonorName, Amount) VALUES (@donorName, @amount);";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@donorName", donateMoneyInfo.donorName);
                        command.Parameters.AddWithValue("@amount", donateMoneyInfo.amount);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            donateMoneyInfo.donorName = "";
            donateMoneyInfo.amount = "";
           
            successMessage = "New Donation Added Successfully";

            Response.Redirect("/Identity/Donates/MoneyIndex");
        }
    }
}
