using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.WinControls.UI;
using Telerik.Web.UI;

namespace TrackingSystem
{

    public partial class Orders : System.Web.UI.Page
    {
        string connectionString = ConfigurationManager.ConnectionStrings["TrackingSystemConnectionString"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                RadGrid.Rebind();
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
        protected void ClearBttn_Click(object sender, EventArgs e)
        {

            SPidtxt.Text = "";
            EPidtxt.Text = "";
            StartDateTimetxt.Text = "";
            StatusOptions.SelectedItem.Text = "";

            EndDateTimetxt.Text = "";
           

 




            RadGrid.Rebind();
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
        private string GetCountryID(string countryName)
        {
            string airportName = "";
            string query = "SELECT IDKey FROM Countries where CountryName = @CountryName;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.AddWithValue("@CountryName", countryName);


                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            airportName = id.ToString();

                        }
                    }

                }
            }
            return airportName;
        }
        protected void SearchBttn_Click(object sender, EventArgs e)
        {
            RadGrid.Rebind();
        }
        private DataTable GetMessages()
        {

            string SPid = SPidtxt.Text;
            string EPid = EPidtxt.Text;
            if (SPid != "")
            {
                SPid = GetCountryID(SPid);

            }
            if (EPid != "")
            {
                EPid = GetCountryID(EPid);

            }
            string StartDateTime = StartDateTimetxt.Text;
            string EndDateTime = EndDateTimetxt.Text;
            string Status = StatusOptions.SelectedItem.Text;

            DataTable dataTable = new DataTable();


            string query = "SELECT * FROM Orders where SPid like @SPid and  EPid like @EPid and StartDateTime like @StartDateTime and (EndDateTime like @EndDateTime or EndDateTime  Is Null ) and Status like @Status;";
           

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SPid ","%" + SPid + "%");
                    command.Parameters.AddWithValue("@EPid", "%" +  EPid + "%");
                    command.Parameters.AddWithValue("@StartDateTime", "%" + StartDateTime + "%");
                    command.Parameters.AddWithValue("@EndDateTime", "%" +  EndDateTime + "%");
                    command.Parameters.AddWithValue("@Status", "%" + Status + "%");
                    connection.Open();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }

            return dataTable;
        }
        protected void RadGrid_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "ViewDetails" && e.Item is GridDataItem)
            {
                GridDataItem item = (GridDataItem)e.Item;
                string IDKey = item["IDKey"].Text;

                Response.Redirect($"Tracking.aspx?IDKey={IDKey}");
            }
        }
    }
}