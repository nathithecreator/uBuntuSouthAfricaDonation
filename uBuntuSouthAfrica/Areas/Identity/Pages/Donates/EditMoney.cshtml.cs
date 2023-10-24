using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Data.SqlClient;

namespace uBuntuSouthAfrica.Pages.Donates
{
    public class EditMoneyModel : PageModel
    {
        public DonateMoneyInfo donateMoneyInfo = new DonateMoneyInfo();
        public string errorMessage = "";
        public string successMessage = "";

        public void OnGet()
        {
            string id = Request.Query["id"];

            try
            {
                string connectionString = "Server=tcp:djpromorosebank1.database.windows.net,1433;Initial Catalog=DJPromoWebApp;Persist Security Info=False;User ID=djnathi;Password=Mamabolo777;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;";
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);
                builder.TrustServerCertificate = true;
                connectionString = builder.ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM MoneyDonations WHERE id=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                donateMoneyInfo.id = reader.GetInt32(0).ToString();
                                donateMoneyInfo.donorName = reader.GetString(1);
                                donateMoneyInfo.disasterName = reader.GetString(2);
                                donateMoneyInfo.date = reader.GetDateTime(3).ToString();
                                donateMoneyInfo.amount = reader.GetDecimal(4); // Convert decimal to string
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
        }

        public void OnPost()
        {
            donateMoneyInfo.id = Request.Form["id"];
            donateMoneyInfo.donorName = Request.Form["name"];
            donateMoneyInfo.disasterName = Request.Form["disasterName"];
            // Parse the input amount as a decimal
            if (decimal.TryParse(Request.Form["amount"], out decimal parsedAmount))
            {
                donateMoneyInfo.amount = parsedAmount;
            }
            else
            {
                errorMessage = "Invalid amount format";
                return;
            }

            if (string.IsNullOrEmpty(donateMoneyInfo.donorName) || string.IsNullOrEmpty(donateMoneyInfo.disasterName))
            {
                errorMessage = "All fields are required";
                return;
            }

            try
            {
                string connectionString = "Server=tcp:djpromorosebank1.database.windows.net,1433;Initial Catalog=DJPromoWebApp;Persist Security Info=False;User ID=djnathi;Password=Mamabolo777;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;";
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);
                builder.TrustServerCertificate = true;
                connectionString = builder.ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "UPDATE MoneyDonations SET DonorName=@donorName, DisasterName=@disasterName, Amount=@amount WHERE ID=@id";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", donateMoneyInfo.id);
                        command.Parameters.AddWithValue("@donorName", donateMoneyInfo.donorName);
                        command.Parameters.AddWithValue("@disasterName", donateMoneyInfo.disasterName);
                        command.Parameters.AddWithValue("@amount", donateMoneyInfo.amount); // Parse amount to decimal

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            Response.Redirect("/Identity/Donates/MoneyIndex");
        }
    }

}
