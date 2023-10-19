using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
            String id = Request.Query["id"];

            try
            {
                String connectionString = "Server=tcp:djpromorosebank1.database.windows.net,1433;Initial Catalog=DJPromoWebApp;Persist Security Info=False;User ID=djnathi;Password=Mamabolo777;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;";
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);
                builder.TrustServerCertificate = true;
                connectionString = builder.ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM Disasters WHERE id=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                disasterInfo.id = "" + reader.GetInt32(0).ToString();
                                disasterInfo.Date = reader.GetDateTime(1).ToString();
                                disasterInfo.DisasterName = reader.GetString(2);
                                disasterInfo.Location = reader.GetString(3);
                                disasterInfo.Description = reader.GetString(4);
                                disasterInfo.DonationType = reader.GetString(5);

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
            disasterInfo.id = Request.Query["id"];
            disasterInfo.Date = Request.Form["name"];
            disasterInfo.DisasterName = Request.Form["DisasterName"];
            disasterInfo.Location = Request.Form["Location"];
            disasterInfo.Description = Request.Form["Description"];
            disasterInfo.DonationType = Request.Form["DonationType"];


            if (disasterInfo.DisasterName.Length == 0 ||
                disasterInfo.Location.Length == 0 ||
                disasterInfo.Description.Length == 0 ||  disasterInfo.DonationType.Length == 0)
            {
                errorMessage = "All the field are required";
                return;
            }

            try
            {
                String connectionString = "Server=tcp:djpromorosebank1.database.windows.net,1433;Initial Catalog=DJPromoWebApp;Persist Security Info=False;User ID=djnathi;Password=Mamabolo777;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;";
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);
                builder.TrustServerCertificate = true;
                connectionString = builder.ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "UPDATE Disasters SET DisasterName=@DisasterName, Location=@Location, Description=@Description, DonationType=@DonationType, Date=GETDATE() WHERE ID=@id";


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
