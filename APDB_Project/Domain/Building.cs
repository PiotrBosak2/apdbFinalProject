using System.Collections;
using System.Collections.Generic;

namespace APDB_Project.Domain
{
    public class Building
    {
        public int IdBuilding { get; set; }
        public string Street{ get; set; }
        public int StreetNumber { get; set; }
        public string City { get; set; }
        public float Height { get; set; }
        public virtual ICollection<Campaign> FromBuildingCampaigns { get; set; }
        public virtual ICollection<Campaign> ToBuildingCampaigns { get; set; }
        
        

    }

}