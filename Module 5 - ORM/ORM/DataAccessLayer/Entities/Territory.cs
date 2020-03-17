namespace DataAccessLayer.Entities
{
    public class Territory
    {
        public int TerritoryId { get; set; }

        public string TerritoryDescription { get; set; }

        public int RegionId { get; set; }
        
        public Region Region { get; set; }
    }
}