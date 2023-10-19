using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using uBuntuSouthAfrica.Pages.Donates;
using System.Data.SqlClient;

namespace uBuntuSouthAfrica.Pages.Donates
{
    public class GoodsDonateIndexModel : PageModel
    {
        public List<GoodsInfo> listDonate = new List<GoodsInfo>();
        public void OnGet()
        {
            try
            {
                string connectionString = "Server=tcp:djpromorosebank1.database.windows.net,1433;Initial Catalog=DJPromoWebApp;Persist Security Info=False;User ID=djnathi;Password=Mamabolo777;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;";

                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);
                builder.TrustServerCertificate = true;
                connectionString = builder.ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT ID, DonorName, NumberOfItems, Category, ItemDescription FROM GoodsDonations";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                GoodsInfo goods = new GoodsInfo();
                                goods.id = "" + reader.GetInt32(0).ToString();
                                goods.DonorName = reader.GetString(1);
                                goods.NumberOfItems = reader.GetInt32(2).ToString();
                                goods.Category = reader.GetString(3);
                                goods.ItemDescription = reader.GetString(4);

                                listDonate.Add(goods);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
                // You might want to handle the exception or log it appropriately instead of writing to the console.
            }
        }
    }
  
    public class GoodsInfo
    {
        public string id;
        public string DonorName;
        public string date;
        public string NumberOfItems;
        public string Category;
        public string ItemDescription;
    }

}
