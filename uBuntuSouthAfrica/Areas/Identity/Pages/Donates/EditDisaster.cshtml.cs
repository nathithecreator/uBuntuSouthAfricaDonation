using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Data.SqlClient;

namespace uBuntuSouthAfrica.Pages.Donates
{
    public class EditDisasterModel : PageModel
    {
        public DisasterInfo disasterInfo = new DisasterInfo();
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
                    string sql = "SELECT * FROM Disasters WHERE ID=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                disasterInfo.id = reader.GetInt32(0).ToString();
                                disasterInfo.DisasterName = reader.GetString(1);
                                disasterInfo.Location = reader.GetString(2);
                                disasterInfo.Description = reader.GetString(3);
                                disasterInfo.DonationType = reader.GetString(4);
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
            disasterInfo.id = Request.Form["id"];
            disasterInfo.DisasterName = Request.Form["DisasterName"];
            disasterInfo.Location = Request.Form["Location"];
            disasterInfo.Description = Request.Form["Description"];
            disasterInfo.DonationType = Request.Form["DonationType"];

            if (string.IsNullOrEmpty(disasterInfo.DisasterName) || string.IsNullOrEmpty(disasterInfo.Location) || string.IsNullOrEmpty(disasterInfo.Description) || string.IsNullOrEmpty(disasterInfo.DonationType))
            {
                errorMessage = "All the fields are required";
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
                    string sql = "UPDATE Disasters SET DisasterName=@DisasterName, Location=@Location, Description=@Description, DonationType=@DonationType WHERE ID=@id";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", disasterInfo.id);
                        command.Parameters.AddWithValue("@DisasterName", disasterInfo.DisasterName);
                        command.Parameters.AddWithValue("@Location", disasterInfo.Location);
                        command.Parameters.AddWithValue("@Description", disasterInfo.Description);
                        command.Parameters.AddWithValue("@DonationType", disasterInfo.DonationType);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            Response.Redirect("/Identity/Donates/DisasterIndex");
        }
    }
}
