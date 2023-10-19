using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using uBuntuSouthAfrica.Pages.Donates;
using System.Reflection.PortableExecutable;
using System.Data.SqlClient;

namespace uBuntuSouthAfrica.Pages.Donates
{
    public class EditMoneyModel : PageModel
    {
        public DonateMoneyInfo donateMoneyInfo = new DonateMoneyInfo();
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
                    String sql = "SELECT * FROM MoneyDonations WHERE id=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                donateMoneyInfo.id = "" + reader.GetInt32(0).ToString();
                                donateMoneyInfo.donorName = reader.GetString(1);
                                donateMoneyInfo.date = reader.GetDateTime(2).ToString();
                                donateMoneyInfo.amount = "" + reader.GetDecimal(3).ToString();

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
            donateMoneyInfo.id = Request.Query["id"];
            donateMoneyInfo.donorName = Request.Form["name"];
            donateMoneyInfo.amount = Request.Form["amount"];
         

            if (donateMoneyInfo.donorName.Length == 0 || 
                donateMoneyInfo.amount.Length == 0)
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
                    String sql = "UPDATE MoneyDonations SET DonorName=@donorName, Amount=@amount WHERE ID=@id";


                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", donateMoneyInfo.id);
                        command.Parameters.AddWithValue("@donorName", donateMoneyInfo.donorName);
                        command.Parameters.AddWithValue("@amount", donateMoneyInfo.amount);
                      

                        command.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            Response.Redirect("/Identity/Donates/MoneyIndex");


        }
    }
}
