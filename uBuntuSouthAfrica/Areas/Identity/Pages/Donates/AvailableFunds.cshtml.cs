using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace uBuntuSouthAfrica.Areas.Identity.Pages.Donates
{
    public class AvailableFundsModel : PageModel
    {
        public List<BalanceInfo> listBalance = new List<BalanceInfo>();
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
                    string sql = "SELECT ID, DonorName, DisasterType, DisasterName, Amount, MoneyType FROM Funds";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                BalanceInfo funds = new BalanceInfo();
                                funds.id = "" + reader.GetInt32(0).ToString();
                                funds.DonorName = reader.GetString(1);
                                funds.DisasterType = reader.GetString(2);
                                funds.DisasterName = reader.GetString(3);
                                funds.Amount = reader.GetString(4);
                                funds.MoneyType = reader.GetString(5);

                                listBalance.Add(funds);
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
    public class BalanceInfo
    {
        public string id;
        public string DonorName;
        public string DisasterType;
        public string DisasterName;
        public string Amount;  
        public string MoneyType;
    }

}
