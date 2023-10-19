using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using uBuntuSouthAfrica.Pages.Donates;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace uBuntuSouthAfrica.Pages.Donates
{
    public class DisasterIndexModel : PageModel
    {
        public List<DisasterInfo> listDonate = new List<DisasterInfo>();
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
                    string sql = "SELECT ID, DisasterName, Location, Description, DonationType FROM Disasters";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DisasterInfo disasters = new DisasterInfo();
                                disasters.id = "" + reader.GetInt32(0).ToString();
                                disasters.DisasterName = reader.GetString(1);
                                disasters.Location = reader.GetString(2);
                                disasters.Description = reader.GetString(3);
                                disasters.DonationType = reader.GetString(4);

                           

                                listDonate.Add(disasters);
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
    public class DisasterInfo
    {
        public string id;
        public string Date;
        public string DisasterName;
        public string Location;
        public string Description;
        public string DonationType;
    }
}
