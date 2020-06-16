using System;
using System.Collections.Generic;
using APDB_Project.Domain;

namespace APDB_Project.Dtos
{
    public class CampaignDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double PricePerSquareMeter { get; set; }
        public  Building FromBuilding { get; set; } 
        public  Building ToBuilding { get; set; } 
        public  ClientDto Client { get; set; }
        public  ICollection<BannerDto> Banners { get; set; }

    }
}