using EventManagementSystemMerged.Data;
using EventManagementSystemMerged.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EventManagementSystemMerged.Repos
{
    public class LocationService
    {
        //#region Enter Location Details ADO.NET
        //public DataTable EnterLocationDetails(Location location)
        //{
        //    using (SqlConnection conn = new SqlConnection("Server=localhost;Database=EventManagementSystemMerged1;Trusted_Connection=True;TrustServerCertificate=True"))
        //    {
        //        SqlCommand cmd = new SqlCommand("SELECT * FROM [Locations]", conn);
        //        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
        //        DataTable dt = new DataTable();
        //        adapter.Fill(dt);
        //        return dt;
        //    }
        //}
        //#endregion


        #region Fetch Location Details
        public List<Location> GetLocationDetails()
        {
            using (var context = new AppDbContext())
            {
                return context.Locations.ToList();
            }
        }
        #endregion


        #region Remove Locations
        public void RemoveLocationDetails(int id)
        {
            using (var context = new AppDbContext())
            {
                var remove = context.Locations.Find(id);
                if (remove != null)
                {
                    context.Locations.Remove(remove);
                    context.SaveChanges();
                }
            }
        }
        #endregion


        #region Update Locations ADO.NET
        public void UpdateLocationDetails(Location location, int id)
        {
            using (SqlConnection conn = new SqlConnection("Server=localhost;Database=EventManagementSystemMerged1;Trusted_Connection=True;TrustServerCertificate=True"))
            {
                SqlCommand cmd = new SqlCommand("Update [dbo].[Locations] set LocationName=@LocationName, Capacity=@Capacity, Address=@Address, City=@City, State=@State, Country=@Country, PostalCode=@PostalCode, PrimaryContact=@PrimaryContact, SecondaryContact=@SecondaryContact where LocationID=@LocationID", conn);

                cmd.Parameters.AddWithValue("@LocationName", location.LocationName);          
                cmd.Parameters.AddWithValue("@Capacity", location.Capacity);                  
                cmd.Parameters.AddWithValue("@Address", location.Address);                    
                cmd.Parameters.AddWithValue("@City", location.City);                          
                cmd.Parameters.AddWithValue("@State", location.State);                        
                cmd.Parameters.AddWithValue("@Country", location.Country);                    
                cmd.Parameters.AddWithValue("@PostalCode", location.PostalCode);              
                cmd.Parameters.AddWithValue("@PrimaryContact", location.PrimaryContact);
                cmd.Parameters.AddWithValue("@SecondaryContact", location.SecondaryContact);
                cmd.Parameters.AddWithValue("@LocationID", location.LocationID);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        #endregion

        #region Update Locations EFW
        //public void UpdateLocationDetails(Location location, int id)
        //{
        //    using (var context = new AppDbContext())
        //    {
        //        var existingLocation = context.Locations.Find(id);
        //        if (existingLocation != null)
        //        {
        //            existingLocation.LocationName = location.LocationName;
        //            existingLocation.Capacity = location.Capacity;
        //            existingLocation.Address = location.Address;
        //            existingLocation.City = location.City;
        //            existingLocation.State = location.State;
        //            existingLocation.Country = location.Country;
        //            existingLocation.PostalCode = location.PostalCode;
        //            existingLocation.PrimaryContact = location.PrimaryContact;
        //            existingLocation.SecondaryContact = location.SecondaryContact;

        //            context.SaveChanges();
        //        }
        //    }
        //}
        #endregion

        #region Add Location
        public void EnterLocationDetails(Location location)
        {
            using (var context = new AppDbContext())
            {
                context.Locations.Add(location);
                context.SaveChanges();
            }
        }
        #endregion
    }
}

















