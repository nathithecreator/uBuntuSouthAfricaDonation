using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using uBuntuSouthAfrica.Pages.Donates;
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

            if (donateGoodsInfo.ItemDescription.Length == 0 ||
                donateGoodsInfo.NumberOfItems.Length == 0)
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

                    using (SqlCommand command = new SqlCommand(sql, connection, transaction))
                    {
                        command.Parameters.AddWithValue("@disasterName", disasterName);
                        command.Parameters.AddWithValue("@donorName", donateGoodsInfo.DonorName);
                        command.Parameters.AddWithValue("@category", selectedCategory);
                        command.Parameters.AddWithValue("@itemDescription", donateGoodsInfo.ItemDescription);
                        command.Parameters.AddWithValue("@numOfItems", donateGoodsInfo.NumberOfItems);
                        command.Parameters.AddWithValue("@goodsCost", goodsCost);

                        // Execute the SQL command
                        command.ExecuteNonQuery();
                    }

                    // Commit the transaction if the insertion is successful
                    transaction.Commit();

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
    }
    catch (Exception ex)
    {
        // If there's an exception, roll back the transaction to ensure data consistency
        errorMessage = ex.Message;
    }

    Response.Redirect("/Identity/Donates/GoodsDonateIndex");
}
    }
}
