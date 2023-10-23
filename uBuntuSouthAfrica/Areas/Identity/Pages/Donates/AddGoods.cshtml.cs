using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using uBuntuSouthAfrica.Pages.Donates;
using System;
using System.Data.SqlClient;

namespace uBuntuSouthAfrica.Pages.Donates
{
    public class AddGoodsModel : PageModel
    {
        public GoodsInfo donateGoodsInfo = new GoodsInfo();
        public string errorMessage = "";
        public string successMessage = "";

        public void OnGet()
        {
        }

        public void OnPost()
        {
            donateGoodsInfo.id = Request.Form["id"];
            donateGoodsInfo.DonorName = Request.Form["name"];
            string category = Request.Form["category"];
            string otherCategory = Request.Form["othercategory"];
            donateGoodsInfo.ItemDescription = Request.Form["description"];
            donateGoodsInfo.NumberOfItems = Request.Form["numberOfItems"];

            if (string.IsNullOrEmpty(donateGoodsInfo.DonorName))
            {
                donateGoodsInfo.DonorName = "Anonymous";
            }

            if (string.IsNullOrEmpty(donateGoodsInfo.ItemDescription) || string.IsNullOrEmpty(donateGoodsInfo.NumberOfItems))
            {
                errorMessage = "All fields are required";
                return;
            }

            string selectedCategory = string.IsNullOrEmpty(category) ? otherCategory : category;

            try
            {
                string connectionString = "Server=tcp:djpromorosebank1.database.windows.net,1433;Initial Catalog=DJPromoWebApp;Persist Security Info=False;User ID=djnathi;Password=Mamabolo777;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;";
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);
                builder.TrustServerCertificate = true;
                connectionString = builder.ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO GoodsDonations (DisasterName, DonorName, Category, ItemDescription, NumberOfItems, GoodsCost) " +
                              "VALUES (@disasterName, @donorName, @category, @itemDescription, @numOfItems, @goodsCost);";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        // You forgot to start a transaction here, so I removed transaction from the SqlCommand constructor.
                        command.Parameters.AddWithValue("@disasterName", donateGoodsInfo.DisasterName); // Add disaster name
                        command.Parameters.AddWithValue("@donorName", donateGoodsInfo.DonorName);
                        command.Parameters.AddWithValue("@category", selectedCategory);
                        command.Parameters.AddWithValue("@itemDescription", donateGoodsInfo.ItemDescription);
                        command.Parameters.AddWithValue("@numOfItems", donateGoodsInfo.NumberOfItems);

                        // You forgot to add the GoodsCost parameter, so I added it
                        command.Parameters.AddWithValue("@goodsCost", donateGoodsInfo.GoodsCost);

                        // Execute the SQL command
                        command.ExecuteNonQuery();
                    }

                    // No transaction rollback is needed since we didn't start a transaction.

                    // Clear the form fields
                    donateGoodsInfo.DisasterName = "";
                    donateGoodsInfo.DonorName = "";
                    donateGoodsInfo.ItemDescription = "";
                    donateGoodsInfo.NumberOfItems = "";
                    donateGoodsInfo.GoodsCost = "";
                    donateGoodsInfo.Category = selectedCategory;
                    successMessage = "New Donations Added Successfully";
                }
            }
            catch (Exception ex)
            {
                // If there's an exception, handle it (you can log it) but no need to rollback a non-existent transaction.
                errorMessage = ex.Message;
            }

            Response.Redirect("/Identity/Donates/GoodsDonateIndex");
        }
    }
}
