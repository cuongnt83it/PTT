using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
namespace Model.DAO
{
   
    public class DistrictDao
    {
        PTTDataContext db = null;

        public DistrictDao()
        {

            db = new PTTDataContext();
        }
        public List<District> ToList()
        {
            return db.Districts.ToList<District>();
        }
        public int Insert(District buider)
        {
            db.Districts.Add(buider);
            
            return db.SaveChanges();
        }
        public int Delete(string CityID)
        {
            var bd = db.Districts.Find(CityID);
            db.Districts.Remove(bd);
            db.SaveChanges();
            return db.SaveChanges();
        }
        public List<District> FindByID(string CityID)
        {
            var list = db.Districts.Where(a => a.CityID == CityID).ToList<District>();
             return list;
        }
        public District FindByID(string CityID, string DistrictID)
        {
            return db.Districts.SingleOrDefault(a=>a.CityID==CityID && a.DistrictCode == DistrictID);
        }
        public int Update(District buider)
        {
            var bd = db.Districts.Find(buider.CityID);
          
        return    db.SaveChanges();
           
        }
    }
}
