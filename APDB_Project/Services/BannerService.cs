using System.Collections.Generic;
using System.Linq;
using APDB_Project.Domain;
using APDB_Project.Dtos;
using APDB_Project.Exceptions;

namespace APDB_Project.Services
{
    public class BannerService : IBannerService
    {
        private readonly AdvertisementContext _context;
        private const float WidthOfBuilding = 1;

        public BannerService(AdvertisementContext context)
        {
            _context = context;
        }
        public Banner CreateBanner(List<Building> buildings, double pricePerSquareMeter)
        {
            var heightOfBanner = buildings.Max(b => b.Height);
            var widthOfBanner = buildings.Count * WidthOfBuilding;//we assume each building has the same width
            if (widthOfBanner == 0)
                throw new BannerWithZeroAreaException();
            var area = heightOfBanner * widthOfBanner;
            var price = area * pricePerSquareMeter;
            var banner =  new Banner
            {
                Area = area,
                Name = "some fancy name",
                Price = price
            };
            var bannerEntry = _context.Banners.Add(banner);
            return bannerEntry.Entity;
        }

        public void UpdateBanners(Campaign campaign,params Banner[] banners)
        {
            foreach (var banner in banners)
            {
                banner.Campaign = campaign;
                banner.IdCampaign = campaign.IdCampaign;
            }
            

            
        }
    }
}