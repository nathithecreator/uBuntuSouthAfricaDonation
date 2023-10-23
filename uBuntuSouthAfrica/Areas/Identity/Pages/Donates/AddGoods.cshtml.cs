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
                    string sql = "INSERT INTO GoodsDonations (DonorName, Category, ItemDescription, NumberOfItems) VALUES (@donorName, @category, @itemDescription, @numOfItems);";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@donorName", donateGoodsInfo.DonorName);
                        command.Parameters.AddWithValue("@category", selectedCategory);
                        command.Parameters.AddWithValue("@itemDescription", donateGoodsInfo.ItemDescription);
                        command.Parameters.AddWithValue("@numOfItems", donateGoodsInfo.NumberOfItems);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            donateGoodsInfo.DonorName = "";
            donateGoodsInfo.ItemDescription = "";
            donateGoodsInfo.NumberOfItems = "";
            donateGoodsInfo.Category = selectedCategory;
            successMessage = "New Donation Added Successfully";

            Response.Redirect("/Identity/Donates/GoodsDonateIndex");
        }
    }
}
