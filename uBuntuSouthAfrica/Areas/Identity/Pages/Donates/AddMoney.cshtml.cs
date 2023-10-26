using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.DotNet.Scaffolding.Shared.Project;
using System;
using System.Collections.Generic;
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
            if (Request.Query.ContainsKey("id"))
            {
                donateMoneyInfo.id = Request.Query["id"];
            }

            if (Request.Query.ContainsKey("disasterName"))
            {
                donateMoneyInfo.disasterName = Request.Query["disasterName"];
            }
        }

        public void OnPost()
        {
            donateMoneyInfo.donorName = Request.Form["name"];

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

            donateMoneyInfo.disasterName = Request.Form["disastername"];

            // Check if donorName is empty or null, and set it to "Anonymous" if true
            if (string.IsNullOrEmpty(donateMoneyInfo.donorName))
            {
                donateMoneyInfo.donorName = "Anonymous";
            }

            // Set up the SQL connection string
            string connectionString = "Server=tcp:djpromorosebank1.database.windows.net,1433;Initial Catalog=DJPromoWebApp;Persist Security Info=False;User ID=djnathi;Password=Mamabolo777;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Insert into "MoneyDonations" table
                        string moneyDonationsSql = "INSERT INTO MoneyDonations (DisasterName, DonorName, Amount) VALUES (@donorDisaster, @donorName, @amount);";
                        using (SqlCommand moneyDonationsCommand = new SqlCommand(moneyDonationsSql, connection, transaction))
                        {
                            moneyDonationsCommand.Parameters.AddWithValue("@donorDisaster", donateMoneyInfo.disasterName);
                            moneyDonationsCommand.Parameters.AddWithValue("@donorName", donateMoneyInfo.donorName);
                            moneyDonationsCommand.Parameters.AddWithValue("@amount", donateMoneyInfo.amount);

                            moneyDonationsCommand.ExecuteNonQuery();
                        }

                        // Insert into "Funds" table
                        string fundsSql = "INSERT INTO Funds (DonorName, DisasterType, DisasterName, Amount, MoneyType) VALUES (@donorName, 'Money' , @donorDisaster, @amount, 'expense');";
                        using (SqlCommand fundsCommand = new SqlCommand(fundsSql, connection, transaction))
                        {
                            fundsCommand.Parameters.AddWithValue("@donorName", donateMoneyInfo.donorName);
                            fundsCommand.Parameters.AddWithValue("@donorDisaster", donateMoneyInfo.disasterName);
                            fundsCommand.Parameters.AddWithValue("@amount", donateMoneyInfo.amount);

                            fundsCommand.ExecuteNonQuery();
                        }

                        // Commit the transaction if both insertions are successful
                        transaction.Commit();

                        // Clear the form fields
                        donateMoneyInfo.donorName = "";
                        donateMoneyInfo.amount = 0; // Reset amount
                        donateMoneyInfo.disasterName = "";

                        successMessage = "New Donations Added Successfully";
                    }
                    catch (Exception ex)
                    {
                        // If there's an exception, roll back the transaction to ensure data consistency
                        transaction.Rollback();
                        errorMessage = ex.Message;
                    }
                }
            }

            // Redirect to the MoneyIndex page
            Response.Redirect("/Identity/Donates/MoneyIndex");
        }
    }
}
