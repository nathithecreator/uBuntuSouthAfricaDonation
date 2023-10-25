using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using uBuntuSouthAfrica.Pages.Donates;
using System.Data.SqlClient;

namespace uBuntuSouthAfrica.Pages.Donates
{
    public class GoodsDonateIndexModel : PageModel
    {
        public List<GoodsInfo> listDonate = new List<GoodsInfo>();
        public int NetAmount { get; set; } // Add the NetAmount property here
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
                    string sql = "SELECT ID, DisasterName, DonorName, NumberOfItems, Category, ItemDescription, GoodsCost FROM GoodsDonations";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                GoodsInfo goods = new GoodsInfo();
                                goods.id = "" + reader.GetInt32(0).ToString();
                                goods.DisasterName = reader.GetString(1);
                                goods.DonorName = reader.GetString(2);
                                goods.NumberOfItems = reader.GetInt32(3).ToString();
                                goods.Category = reader.GetString(4);
                                goods.ItemDescription = reader.GetString(5);
                                goods.GoodsCost = reader.GetInt32(6);

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
            NetAmount = CalculateNetAmount();
        }

        public int CalculateNetAmount()
        {
            int netAmount = 0;

            // Perform your SQL query to calculate the NetAmount here
            using (SqlConnection connection = new SqlConnection("Server=tcp:djpromorosebank1.database.windows.net,1433;Initial Catalog=DJPromoWebApp;Persist Security Info=False;User ID=djnathi;Password=Mamabolo777;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;"))
            {
                connection.Open();
                string sql = "SELECT SUM(CASE WHEN MoneyType = 'income' THEN Amount ELSE 0 END) - SUM(CASE WHEN MoneyType = 'expense' THEN Amount ELSE 0 END) AS NetAmount FROM Funds";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (!reader.IsDBNull(0))
                            {
                                netAmount = reader.GetInt32(0);
                            }
                        }
                    }
                }
            }

            return netAmount;
        }
    }

    public class GoodsInfo
    {
        public string id;
        public string DisasterName;
        public string DonorName;
        public string date;
        public string NumberOfItems;
        public string Category;
        public string ItemDescription;
        public int GoodsCost;
    }

}
