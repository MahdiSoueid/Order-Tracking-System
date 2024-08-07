using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Telerik.Windows.Documents.Spreadsheet.Expressions.Functions;
using System.Web.DynamicData;
using System.Diagnostics;
using System.IO;
using Telerik.WinControls;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI.com.hisoftware.api2;

namespace TrackingSystem
{
    public partial class Tracking : System.Web.UI.Page
    {
        string connectionString = ConfigurationManager.ConnectionStrings["TrackingSystemConnectionString"].ConnectionString;
        string OrderID;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {



                if (Request.QueryString["IDKey"] != null)
                {

                    OrderID = Request.QueryString["IDKey"];
                    List<List<string>> trackingData = GetOrderTrackingData(Int32.Parse(OrderID));
                    PopulateStepper(trackingData);






                }




            }
        }


        private int GetTransitCountryId(string countryAirport)
        {
            int CountryID = 0;
            string query = "SELECT IDKey FROM Countries where AirportName = @AirportName;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.AddWithValue("@AirportName", countryAirport);


                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CountryID = reader.GetInt32(0);

                        }
                    }

                }
            }
            return CountryID;
        }
        private string GetAirportFromCountry(string CountryName)
        {
            string airportName = "0";
            string query = "SELECT AirportName FROM Countries where CountryName = @CountryName;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.AddWithValue("@CountryName", CountryName);


                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            airportName = reader.GetString(0);

                        }
                    }

                }
            }
            return airportName;
        }
        private string GetTransitCountryAirport(int countryID)
        {
            string airportName = "";
            string query = "SELECT TransitAirportName FROM Countries where IDKey = @IDKey;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.AddWithValue("@IDKey", countryID);


                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            airportName = reader.GetString(0);

                        }
                    }

                }
            }
            return airportName;
        }
        private bool haveTransit(int countryID)
        {
            string haveTransit = "";
            string query = "SELECT Transit FROM Countries where IDKey = @IDKey;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.AddWithValue("@IDKey", countryID);


                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            haveTransit = reader.GetString(0);

                        }
                    }

                }
            }
            if (haveTransit == "false")
                return false;
            else
                return true;


        }

        private int[] GetOrderStartAndEnd(int OrderID)
        {
            int[] orderStartEnd = new int[2];



            string query = "SELECT SPid , EPid FROM Orders  Where IDKey = @IDKey;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.AddWithValue("@IDKey", OrderID);


                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            orderStartEnd[0] = reader.GetInt32(0);
                            orderStartEnd[1] = reader.GetInt32(1);

                        }
                    }

                }
            }
            return orderStartEnd;
        }
      
        public List<string> GetCountryAndAirportById(int id)
        {
            List<string> result = new List<string>();

            string query = "SELECT CountryName, AirportName FROM Countries WHERE IDKey = @IDKey";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IDKey", id);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result.Add(reader["CountryName"].ToString());
                            result.Add(reader["AirportName"].ToString());
                        }
                    }
                }
            }
            return result;

        }
        public string GetOrderStatus(string countryName, int orderId)
        {
            string status = "";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT Status FROM OrderTracking WHERE CountryName = @CountryName AND OrderId = @OrderId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CountryName", countryName);
                    command.Parameters.AddWithValue("@OrderId", orderId);

                    connection.Open();
                    var result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        status = result.ToString();
                    }
                }
            }

            return status;
        }
        public string[] OrderDetails(string countryName, int orderId)
        {
            string[] status = new string[3];


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT Status, ActionDateTime, ReceivedDateTime FROM OrderTracking WHERE CountryName = @CountryName AND OrderId = @OrderId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CountryName", countryName);
                    command.Parameters.AddWithValue("@OrderId", orderId);

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                           
                            status[0] = reader.IsDBNull(0) ? "Not accessed yet" : reader.GetString(0);
                            status[1] = reader.IsDBNull(1) ? "No action Taken Yet" : reader.GetString(1);
                            status[2] = reader.IsDBNull(2) ? "Not received yet" : reader.GetString(2);
                        }
                    }
                }
            }

            return status;
        }
        private List<List<string>> GetOrderTotalStops(int startCountry, int endCountry)
        {

            List<List<string>> totalStops = new List<List<string>>();
            int checkCountry = startCountry;
            bool checker = haveTransit(checkCountry);
            totalStops.Add(GetCountryAndAirportById(checkCountry));
            while (checker && checkCountry != endCountry)
            {
                List<string> countryAndAirport = GetCountryAndAirportById(checkCountry);
                checker = haveTransit(checkCountry);
                checkCountry = GetTransitCountryId(GetTransitCountryAirport(checkCountry));
                totalStops.Add(GetCountryAndAirportById(checkCountry));
            }


            return totalStops;
        }
        private List<List<string>> GetOrderTrackingData(int orderId)
        {
            int[] startend = GetOrderStartAndEnd(Int32.Parse(OrderID));
            List<List<string>> fullpath = GetOrderTotalStops(startend[0], startend[1]);

           

            return fullpath;

        }

        private void PopulateStepper(List<List<string>> trackingData)
        {
            for (int i = 0; i < trackingData.Count; i++)
            {
                List<string> row = trackingData[i];
                List<string> rowafter = trackingData[i];

                if (i != trackingData.Count - 1)
                {
                    rowafter = trackingData[i + 1];
                }

                string country = row[0].ToString();
                string countryafter = rowafter[0].ToString();

                string[] s = new string[3]; 
                s = OrderDetails(country, Int32.Parse(OrderID));
                string status;
                if (s[0] == null || s[1] == "InProgress") {
                    status = " Not accessed yet";
                }
                else
                {
                    status = s[0];
                }
                if (s[1] == null )
                {
                    s[1] = "No action Taken Yet";
                }
              
                if (s[2] == null)
                {
                    s[2] = " Not received yet";
                }
               
                string statusafter = GetOrderStatus(countryafter, Int32.Parse(OrderID));
                string airport = row[1].ToString();

                HtmlGenericControl stepDiv = new HtmlGenericControl("div");
                stepDiv.Attributes["class"] = "step";

                
                HtmlGenericControl circleLink = new HtmlGenericControl("a");
                circleLink.Attributes["href"] = "#";
                circleLink.Attributes["class"] = "circle " + GetStatusClass(status);
                circleLink.Attributes["onclick"] = $"toggleDetails('step{i}')";

               
                HtmlGenericControl icon = new HtmlGenericControl("i");
                icon.Attributes["class"] = GetIconClass(status);
                circleLink.Controls.Add(icon);

                
                HtmlGenericControl textDiv = new HtmlGenericControl("div");
                textDiv.InnerHtml = $"<strong>Country:</strong> {country}<br>";

                
                HtmlGenericControl detailsPanel = new HtmlGenericControl("div");
                detailsPanel.Attributes["id"] = $"step{i}-details";
                detailsPanel.Attributes["class"] = "details";

                
                HtmlGenericControl cardContent = new HtmlGenericControl("div");
                cardContent.Attributes["class"] = "card-content";
                cardContent.InnerHtml = $"<strong>Country:</strong> <span>{country}</span><br><strong>Status:</strong> <span>{status}</span><br><strong>Airport:</strong> <span>{airport}</span><br><strong>Received DateTime:</strong> <span>{s[2]}</span><br><strong>Action DateTime:</strong> <span>{s[1]}</span>";
                detailsPanel.Controls.Add(cardContent);

                
                stepDiv.Controls.Add(circleLink);
                stepDiv.Controls.Add(textDiv);
                stepDiv.Controls.Add(detailsPanel);

                
                if (i < trackingData.Count - 1)
                {
                    HtmlGenericControl lineDiv = new HtmlGenericControl("div");
                    lineDiv.Attributes["class"] = "line " + GetLineClass(statusafter);
                    stepDiv.Controls.Add(lineDiv);
                }

                
                stepperContainer.Controls.Add(stepDiv);
            }
        }




        private string GetStatusClass(string status)
        {
            if (status == null)
            {
                status = "";
            }
            switch (status.ToLower())
            {
                case "approved":
            return "approved";
        case "rejected":
            return "rejected";
        case "inprogress":
            return "inprogress";
        default:
            return "none";
            }
        }

        private string GetLineClass(string status)
        {
            if (status == null)
            {
                status = "";
            }
            switch (status.ToLower())
            {
                case "approved":
                    return "approved";
                case "rejected":
                    return "rejected";
                case "inprogress":
                    return "inprogress";
                case "none":
                default:
                    return "none-line";
            }
        }
        private string GetIconClass(string status)
        {
            if (status == null)
            {
                status = "";
            }
            switch (status.ToLower())
            {
                case "approved":
                    return "fas fa-check";
                case "rejected":
                    return "fas fa-times";
                case "inprogress":
                    return "fas fa-spinner";
                default:
                    return "fas fa-question";
            }
        }
        protected void RadGrid_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {


            DataTable dt = new DataTable();
            dt = GetMessages();
            dt.Columns.Add("SPName", typeof(string));
            dt.Columns.Add("EPName", typeof(string));
            foreach (DataRow row in dt.Rows)
            {
                row["SPName"] = GetCountryName((int)row["SPid"]);
                row["EPName"] = GetCountryName((int)row["EPid"]);
            }
            RadGrid.DataSource = dt;

        }
        private string GetCountryName(int countryID)
        {
            string airportName = "";
            string query = "SELECT CountryName FROM Countries where IDKey = @IDKey;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.AddWithValue("@IDKey", countryID);


                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            airportName = reader.GetString(0);

                        }
                    }

                }
            }
            return airportName;
        }
        private DataTable GetMessages()
        {

          
            DataTable dataTable = new DataTable();


            string query = "SELECT * FROM Orders where IDKey = @IDKey;";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IDKey ", OrderID);
                   
                    connection.Open();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }

            return dataTable;
        }
    }
}



    

