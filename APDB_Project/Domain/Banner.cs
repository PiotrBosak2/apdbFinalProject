namespace APDB_Project.Domain
{
    public class Banner
    {
        public int IdBanner { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int IdCampaign { get; set; }
        public double Area { get; set; }
        public virtual Campaign Campaign { get; set; }
    }
}