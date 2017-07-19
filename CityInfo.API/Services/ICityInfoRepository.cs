namespace CityInfo.API.Services
{
    using System.Collections.Generic;
    using CityInfo.API.Entities;

    public interface ICityInfoRepository
    {
        IEnumerable<City> GetCities();
        bool CityExists(int cityId);
        City GetCity(int cityId, bool includePointsOfInterest);
        IEnumerable<PointOfInterest> GetPointsOfInterestForCity(int cityId);
        PointOfInterest GetPointOfInterestForCity(int cityId, int pointOfInterestId);
        void AddPointOfInterestForCity(int cityId, PointOfInterest pointOfInterest);
        void DeletePointOfInterest(PointOfInterest pointOfInterest);
        bool Save();
    }
}
