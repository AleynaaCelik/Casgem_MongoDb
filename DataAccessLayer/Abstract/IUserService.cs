using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Abstract
{
	public interface IUserService
	{
		List<User> Get();
		User Get(string id);
		List<User> GetByFilter(string? userName);
		User Create(User user);
		void Update(string id, User user);
		void Remove(string id);
	}
}
