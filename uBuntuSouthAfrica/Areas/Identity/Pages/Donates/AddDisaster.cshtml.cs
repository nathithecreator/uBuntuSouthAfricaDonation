using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using uBuntuSouthAfrica.Pages.Donates;
using System;
using System.Data.SqlClient;

namespace uBuntuSouthAfrica.Pages.Donates
{
    public class AddDisasterModel : PageModel
    {
        public int NetAmount { get; set; } // Add the NetAmount property here

        public DisasterInfo disasterInfo = new DisasterInfo();
        public string errorMessage = "";
        public string successMessage = "";
        public void OnGet()
        {
        }

        public void OnPost()
        {
            disasterInfo.id = Request.Form["id"];
            disasterInfo.DisasterName = Request.Form["DisasterName"];
            disasterInfo.Location = Request.Form["Location"];
            disasterInfo.Description = Request.Form["Description"];
            disasterInfo.DonationType = Request.Form["DonationType"];

            // Check if donorName is empty or null, and set it to anonymous if true
            if (string.IsNullOrEmpty(disasterInfo.DisasterName))
            {
                errorMessage = "An amount is required";
                return;
            }

            if (string.IsNullOrEmpty(disasterInfo.Location))
            {
                errorMessage = "An amount is required";
                return;
            }

            if (string.IsNullOrEmpty(disasterInfo.Description))
            {
                errorMessage = "An amount is required";
                return;
            }

            if (string.IsNullOrEmpty(disasterInfo.DonationType))
            {
                errorMessage = "An amount is required";
                return;
            }

            // Save the new donation into the database
            try
            {
                string connectionString = "Server=tcp:djpromorosebank1.database.windows.net,1433;Initial Catalog=DJPromoWebApp;Persist Security Info=False;User ID=djnathi;Password=Mamabolo777;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;";
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);
                builder.TrustServerCertificate = true;
                connectionString = builder.ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO Disasters (DisasterName, Location, Description,DonationType) VALUES (@DisasterName, @Location,@Description, @DonationType);";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
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

            disasterInfo.DisasterName = "";
            disasterInfo.Location = "";
            disasterInfo.Description = "";
            disasterInfo.DonationType = "";
            successMessage = "New Donation Added Successfully";

            Response.Redirect("/Identity/Donates/DisasterIndex");
        }
    }
}


public class DisasterInfo
{
    public string id;
    public string DisasterName;
    public string Location;
    public string Description;
    public string DonationType;
}
