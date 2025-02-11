using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Data.SqlClient;
using static uBuntuSouthAfrica.Areas.Identity.Pages.Donates.AvailableFundsModel;

namespace uBuntuSouthAfrica.Areas.Identity.Pages.Donates
{
    public class AddFundsModel : PageModel
    {
        public BalanceInfo balanceInfo = new BalanceInfo();
        public string errorMessage = "";
        public string successMessage = "";

        public void OnGet()
        {
        }

        public void OnPost()
        {
            if (int.TryParse(Request.Form["amount"], out int amount))
            {
                balanceInfo.Amount = amount;

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
                        string sql = "INSERT INTO Funds (DonorName, DisasterType, DisasterName, Amount, MoneyType) VALUES ('income', 'income','income', @amount, 'income');";

                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@amount", balanceInfo.Amount);
                            command.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    errorMessage = ex.Message;
                    return;
                }

                balanceInfo.Amount = 0;  // Reset the amount to zero

                successMessage = "New Donation Added Successfully";

                Response.Redirect("/Identity/Donates/AvailableFunds");
            }
            else
            {
                errorMessage = "Invalid amount. Please enter a valid integer.";
            }
        }
    }
}
