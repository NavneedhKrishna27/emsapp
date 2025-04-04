using EventManagementSystemMerged.Data;
using EventManagementSystemMerged.Models;

namespace EventManagementSystemMerged.Repos
{
    public class LocationService
    {
        #region Enter Location Details
        public void EnterLocationDetails(Location location)
        {
            using (var context = new AppDbContext())
            {
                context.Locations.Add(location);
                context.SaveChanges();
            }
        }
        #endregion
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
        #region Update Locations
        public void UpdateLocationDetails(Location location, int id)
        {
            using (var context = new AppDbContext())
            {
                var existingLocation = context.Locations.Find(id);
                if (existingLocation != null)
                {
                    existingLocation.LocationName = location.LocationName;
                    existingLocation.Capacity = location.Capacity;
                    existingLocation.Address = location.Address;
                    existingLocation.City = location.City;
                    existingLocation.State = location.State;
                    existingLocation.Country = location.Country;
                    existingLocation.PostalCode = location.PostalCode;
                    existingLocation.PrimaryContact = location.PrimaryContact;
                    existingLocation.SecondaryContact = location.SecondaryContact;

                    context.SaveChanges();
                }
            }
        }
        #endregion
    }
}
