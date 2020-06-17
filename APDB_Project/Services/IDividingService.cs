using System.Collections.Generic;
using APDB_Project.Domain;
using APDB_Project.Dtos;
using Castle.Core;

namespace APDB_Project.Services
{
    public interface IDividingService
    {

        public Pair<List<Building>,List<Building>> DivideBuildings(Pair<Building,Building> twoBuildings);
    }
}