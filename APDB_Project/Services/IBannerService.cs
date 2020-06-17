using System.Collections.Generic;
using APDB_Project.Domain;
using APDB_Project.Dtos;

namespace APDB_Project.Services
{
    public interface IBannerService
    {
        public Banner CreateBanner(List<Building> buildings,double pricePerSquareMeter);
        public void UpdateBanners(Campaign campaign,params Banner[] banners);
    }
}