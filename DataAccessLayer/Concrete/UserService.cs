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
	public class UserService : IUserService
	{
		private readonly IMongoCollection<User> _users;

		public UserService(IEstateStoreDatabaseSettings settings, IMongoClient mongoClient)
		{
			var database = mongoClient.GetDatabase(settings.DatabaseName);
			_users = database.GetCollection<User>(settings.UserCollectionName);
		}

		public User Create(User user)
		{
			user.Id = ObjectId.GenerateNewId().ToString();
			_users.InsertOne(user);
			return user;
		}

		public List<User> Get()
		{
			return _users.Find(user => true).ToList();
		}

		public User Get(string id)
		{
			return _users.Find(user => user.Id == id).FirstOrDefault();
		}

		public List<User> GetByFilter(string? userName)
		{
			var filterBuilder = Builders<User>.Filter;
			var filter = filterBuilder.Empty;

			if (!string.IsNullOrEmpty(userName))
			{
				filter = filter & filterBuilder.Where(user => user.UserName.ToLower().Contains(userName.ToLower()));
			}

			return _users.Find(filter).ToList();
		}

		public void Remove(string id)
		{
			_users.DeleteOne(user => user.Id == id);
		}

		public void Update(string id, User user)
		{
			_users.ReplaceOne(user => user.Id == id, user);
		}
	}
}
