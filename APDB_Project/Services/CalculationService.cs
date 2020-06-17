using System.Collections.Generic;
using System.Linq;
using APDB_Project.Domain;
using Castle.Core;

namespace APDB_Project.Services
{
    public class CalculationService : ICalculationService
    {
        private readonly AdvertisementContext _context;

        public CalculationService(AdvertisementContext context)
        {
            _context = context;
        }

        public Pair<List<Building>,List<Building>> DivideBuildings(Pair<Building, Building> twoBuildings)
        {
            var allBuildingsToCover = GetAllBuildingsToCover(twoBuildings);
            var highestBuilding = allBuildingsToCover
                .OrderByDescending(b => b.Height)
                .First();
            var buildingsToTheLeft = allBuildingsToCover
                .Where(b => b.IdBuilding < highestBuilding.IdBuilding).ToList();
            var buildingsToTheRight = allBuildingsToCover
                .Where(b => b.IdBuilding > highestBuilding.IdBuilding).ToList();
            var extraAreaLeft = CalculateExtraArea(buildingsToTheLeft, highestBuilding);
            var extraAreaRight = CalculateExtraArea(buildingsToTheRight, highestBuilding);
            if (extraAreaLeft < extraAreaRight)
                buildingsToTheLeft.Add(highestBuilding);
            else
                buildingsToTheRight.Add(highestBuilding);
            return new Pair<List<Building>, List<Building>>(buildingsToTheLeft, buildingsToTheRight);


            
        }
        
        private static double CalculateExtraArea(IEnumerable<Building> buildings, Building highestBuilding)
        {
            if (buildings == null)
                return 0;
            var maxHeight = highestBuilding.Height;
            return buildings.Select(b => b.Height)
                .Select(h => maxHeight - h)
                .Sum();
        }

        private List<Building> GetAllBuildingsToCover(Pair<Building, Building> buildings)
        {
            return _context.Buildings
                .Where(b => b.Street == buildings.First.Street)
                .Where(b => b.IdBuilding >= buildings.First.IdBuilding &&
                            b.IdBuilding <= buildings.Second.IdBuilding)
                .ToList();
        }
    }
}