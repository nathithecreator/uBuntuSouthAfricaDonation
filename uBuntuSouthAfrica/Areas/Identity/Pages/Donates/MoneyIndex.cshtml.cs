using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace uBuntuSouthAfrica.Pages.Donates
{
    public class IndexModel : PageModel
    {
        public List<DonateMoneyInfo> listDonate = new List<DonateMoneyInfo>();

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
                    string sql = "SELECT ID, DonorName, DisasterName, Amount FROM MoneyDonations";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DonateMoneyInfo donatemoney = new DonateMoneyInfo();
                                donatemoney.ID = reader.GetInt32(0);
                                donatemoney.donorName = reader.GetString(1);
                                donatemoney.disasterName = reader.GetString(2);
                                donatemoney.amount = reader.GetDecimal(3);

                                listDonate.Add(donatemoney);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }
        }
    }

    public class DonateMoneyInfo
    {
        public string id;
        public string donorName;
        public string disasterName;
        public string date;
        public string amount;
    }
}
