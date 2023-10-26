using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
            if (Request.Query.ContainsKey("id"))
            {
                donateGoodsInfo.id = Request.Query["id"];
            }

            if (Request.Query.ContainsKey("disasterName"))
            {
                donateGoodsInfo.DisasterName = Request.Query["disasterName"];
            }
        }

        public void OnPost()
        {
            donateGoodsInfo.id = Request.Form["id"];
            donateGoodsInfo.DisasterName = Request.Form["disastername"];
            donateGoodsInfo.DonorName = string.IsNullOrEmpty(Request.Form["name"]) ? "Anonymous" : Request.Form["name"];
            string category = Request.Form["category"];
            string otherCategory = Request.Form["othercategory"];
            donateGoodsInfo.ItemDescription = Request.Form["description"];
            donateGoodsInfo.NumberOfItems = Request.Form["numberOfItems"];
            donateGoodsInfo.GoodsCost = int.Parse(Request.Form["goodscost"]);

            if (string.IsNullOrEmpty(donateGoodsInfo.ItemDescription) || string.IsNullOrEmpty(donateGoodsInfo.NumberOfItems))
            {
                errorMessage = "All fields are required";
                return;
            }

            string selectedCategory = string.IsNullOrEmpty(category) ? otherCategory : category;

            try
            {
                string connectionString = "Server=tcp:djpromorosebank1.database.windows.net,1433;Initial Catalog=DJPromoWebApp;Persist Security Info=False;User ID=djnathi;Password=Mamabolo777;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlTransaction transaction = connection.BeginTransaction()) // Start a transaction
                    {
                        try
                        {
                            // Insert into GoodsDonation
                            string goodsSql = "INSERT INTO GoodsDonation (DisasterName, DonorName, Category, ItemDescription, NumberOfItems, GoodsCost) VALUES (@disasterName, @donorName, @category, @itemDescription, @numOfItems, @goodsCost);";
                            using (SqlCommand goodsCommand = new SqlCommand(goodsSql, connection, transaction))
                            {
                                goodsCommand.Parameters.AddWithValue("@disasterName", donateGoodsInfo.DisasterName);
                                goodsCommand.Parameters.AddWithValue("@donorName", donateGoodsInfo.DonorName);
                                goodsCommand.Parameters.AddWithValue("@category", selectedCategory);
                                goodsCommand.Parameters.AddWithValue("@itemDescription", donateGoodsInfo.ItemDescription);
                                goodsCommand.Parameters.AddWithValue("@numOfItems", donateGoodsInfo.NumberOfItems);
                                goodsCommand.Parameters.AddWithValue("@goodsCost", donateGoodsInfo.GoodsCost);
                                goodsCommand.ExecuteNonQuery();
                            }

                            // Insert into Funds
                            string fundsSql = "INSERT INTO Funds (DonorName, DisasterType, DisasterName, Amount, MoneyType) VALUES (@donorName, 'Goods', @disasterName, @goodsCost, 'expense');";
                            using (SqlCommand fundsCommand = new SqlCommand(fundsSql, connection, transaction))
                            {
                                fundsCommand.Parameters.AddWithValue("@disasterName", donateGoodsInfo.DisasterName);
                                fundsCommand.Parameters.AddWithValue("@donorName", donateGoodsInfo.DonorName);
                                fundsCommand.Parameters.AddWithValue("@goodsCost", donateGoodsInfo.GoodsCost);
                                fundsCommand.ExecuteNonQuery();
                            }

                            transaction.Commit(); // Commit the transaction if both insertions are successful
                            successMessage = "New Donations Added Successfully";
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback(); // Roll back the transaction in case of an error
                            errorMessage = "Error inserting data: " + ex.Message;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = "Database connection error: " + ex.Message;
            }

            Response.Redirect("/Identity/Donates/GoodsDonateIndex");
        }
    }
}
