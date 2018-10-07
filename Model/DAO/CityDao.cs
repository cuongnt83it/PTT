using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
namespace Model.DAO
{
   
    public class CityDao
    {
        PTTDataContext db = null;

        public CityDao()
        {

            db = new PTTDataContext();
        }
        public List<City> ToList()
        {
            return db.Cities.ToList<City>();
        }
        public int Insert(City buider)
        {
            db.Cities.Add(buider);
            
            return db.SaveChanges();
        }
        public int Delete(string CityID)
        {
            var bd = db.Cities.Find(CityID);
            db.Cities.Remove(bd);
        
            return db.SaveChanges();
        }
        public List<City> FindByID(string CityID)
        {
            var list = db.Cities.Where(a => a.CityID == CityID).ToList<City>();
             return list;
        }
        
        public int Update(City buider)
        {
            var bd = db.Cities.Find(buider.CityID);
          
        return    db.SaveChanges();
           
        }
    }
}
