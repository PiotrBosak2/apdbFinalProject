using System;

namespace APDB_Project.Dtos
{
    public class CampaignCreationDto
    {
        public int IdClient { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int PricePerSquareMeter { get; set; }
        public int FromIdBuilding { get; set; }
        public int ToIdBuilding { get; set; }
    }
}