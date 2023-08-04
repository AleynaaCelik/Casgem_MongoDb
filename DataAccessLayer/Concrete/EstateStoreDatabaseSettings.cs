using DataAccessLayer.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Concrete
{
    public class EstateStoreDatabaseSettings : IEstateStoreDatabaseSettings
    {
        public string EstateCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
		public string UserCollectionName { get; set; }
	}
}
