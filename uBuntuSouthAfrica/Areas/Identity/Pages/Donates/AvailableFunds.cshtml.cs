using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace uBuntuSouthAfrica.Areas.Identity.Pages.Donates
{
    public class AvailableFundsModel : PageModel
    {
        public List<BalanceInfo> listBalance = new List<BalanceInfo>();
        public int NetAmount { get; set; }

        public void OnGet()
        {
            LoadBalanceInfo();
        }

        public void LoadBalanceInfo()
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
                                funds.id = reader.GetInt32(0).ToString();
                                funds.DonorName = reader.GetString(1);
                                funds.DisasterType = reader.GetString(2);
                                funds.DisasterName = reader.GetString(3);
                                funds.Amount = reader.GetInt32(4);
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

            // Calculate the NetAmount
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

        public class BalanceInfo
        {
            public string id;
            public string DonorName;
            public string DisasterType;
            public string DisasterName;
            public int Amount;
            public string MoneyType;
            public int NetAmount;
        }
    }
}
