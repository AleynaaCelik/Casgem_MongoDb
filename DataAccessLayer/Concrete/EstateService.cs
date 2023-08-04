using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Concrete
{
    public class EstateService : IEstateService
    {
        private readonly IMongoCollection<Estate> _estate;

        public EstateService(IEstateStoreDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _estate = database.GetCollection<Estate>(settings.EstateCollectionName);
        }
        public Estate Create(Estate estate)
        {
            estate.Id = ObjectId.GenerateNewId().ToString();
            _estate.InsertOne(estate);
            return estate;
        }

        public List<Estate> Get()
        {
            return _estate.Find(estate => true).ToList();
        }

        public Estate Get(string id)
        {
            return _estate.Find(estate => estate.Id == id).FirstOrDefault();
        }

        public List<Estate> GetByFilter(string? city, string? type, int? room, string? title, int? price, string? buildYear)
        {
            var filterBuilder = Builders<Estate>.Filter;
            var filter = filterBuilder.Empty;            

            if (!string.IsNullOrEmpty(city))
            {
                filter = filter & filterBuilder.Where(estate => estate.City.ToLower().Contains(city.ToLower()));
            }

            if (!string.IsNullOrEmpty(type))
            {
                filter = filter & filterBuilder.Where(estate => estate.Type.ToLower().Contains(type.ToLower()));
            }

            if (room.HasValue)
            {
                filter = filter & filterBuilder.Eq(estate => estate.Room, room.Value);
            }

            if (!string.IsNullOrEmpty(title))
            {
                filter = filter & filterBuilder.Where(estate => estate.Title.ToLower().Contains(title.ToLower()));
            }

            if (price.HasValue)
            {
                filter = filter & filterBuilder.Eq(estate => estate.Price, price.Value);
            }

            if (!string.IsNullOrEmpty(buildYear))
            {
                filter = filter & filterBuilder.Eq(estate => estate.BuildYear, buildYear);
            }

            return _estate.Find(filter).ToList();

        }

        public void Remove(string id)
        {
            _estate.DeleteOne(estate => estate.Id == id);
        }

        public void Update(string id, Estate estate)
        {
            _estate.ReplaceOne(estate => estate.Id == id, estate);
        }
    }
}
