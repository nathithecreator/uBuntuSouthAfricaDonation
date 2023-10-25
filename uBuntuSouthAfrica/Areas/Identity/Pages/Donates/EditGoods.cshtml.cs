using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace uBuntuSouthAfrica.Pages.Donates
{
    public class EditGoodsModel : PageModel
    {
        public GoodsInfo GoodsInfo = new GoodsInfo();
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
                    string sql = "SELECT * FROM GoodsDonations WHERE id=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                GoodsInfo.id = reader.GetInt32(0).ToString();
                                GoodsInfo.DisasterName = reader.GetString(1);
                                GoodsInfo.DonorName = reader.GetString(2);
                                GoodsInfo.date = reader.GetDateTime(3).ToString();
                                GoodsInfo.NumberOfItems = reader.GetInt32(4).ToString();
                                GoodsInfo.Category = reader.GetString(5);
                                GoodsInfo.ItemDescription = reader.GetString(6);
                                GoodsInfo.GoodsCost = reader.GetInt32(7);
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
            GoodsInfo.id = Request.Form["id"];
            GoodsInfo.DisasterName = Request.Form["disastername"];
            GoodsInfo.DonorName = Request.Form["name"];
            GoodsInfo.date = Request.Form["date"];
            GoodsInfo.NumberOfItems = Request.Form["numberOfItems"];
            GoodsInfo.Category = Request.Form["category"];
            GoodsInfo.ItemDescription = Request.Form["description"];
            GoodsInfo.GoodsCost = int.Parse(Request.Form["goodscost"]);

            if (GoodsInfo.DonorName.Length == 0 || GoodsInfo.DisasterName.Length == 0 ||
                GoodsInfo.Category.Length == 0 ||
                GoodsInfo.ItemDescription.Length == 0)
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
                    string sql = "UPDATE GoodsDonations SET DisasterName=@DisasterName, DonorName=@DonorName, NumberOfItems=@NumberOfItems, Category=@Category, ItemDescription=@ItemDescription, GoodsCost=@GoodsCost, Date=GETDATE() WHERE ID=@id";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", GoodsInfo.id);
                        command.Parameters.AddWithValue("@DisasterName", GoodsInfo.DisasterName);
                        command.Parameters.AddWithValue("@DonorName", GoodsInfo.DonorName);
                        command.Parameters.AddWithValue("@NumberOfItems", GoodsInfo.NumberOfItems);
                        command.Parameters.AddWithValue("@Category", GoodsInfo.Category);
                        command.Parameters.AddWithValue("@ItemDescription", GoodsInfo.ItemDescription);
                        command.Parameters.AddWithValue("@GoodsCost", GoodsInfo.GoodsCost);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            Response.Redirect("/Identity/Donates/GoodsDonateIndex");
        }
    }
}
