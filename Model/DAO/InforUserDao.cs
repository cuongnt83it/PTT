using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
namespace Model.DAO
{
   
    public class InforUserDao
    {
        PTTDataContext db = null;

        public InforUserDao()
        {

            db = new PTTDataContext();
        }
        public List<InforUser> ToList()
        {
            return db.InforUsers.ToList<InforUser>();
        }
        public int Insert(InforUser buider)
        {
            db.InforUsers.Add(buider);
            
            return db.SaveChanges();
        }
        public int Delete(long InforID)
        {
            var lstPrUs = db.InforUsers.Where(a => a.InforID == InforID).ToList<InforUser>();
            foreach(InforUser prus in lstPrUs)
            {
                db.InforUsers.Remove(prus);
            }
            
        
            return db.SaveChanges();
        }
        public List<InforUser> FindByInforID(long InforID)
        {
            var list = db.InforUsers.Where(a => a.InforID == InforID).ToList<InforUser>();
             return list;
        }
        public List<InforUser> FindByUserID(long UerID)
        {
            var list = db.InforUsers.Where(a => a.LoginID == UerID).ToList<InforUser>();
            return list;
        }
        public InforUser FindByID(long UerID,long inforID)
        {
            var prUS = db.InforUsers.Where(a => a.LoginID == UerID&& a.InforID== inforID).SingleOrDefault<InforUser>();
            return prUS;
        }
        public int Update(InforUser pu)
        {
            var prUS = db.InforUsers.Where(a => a.LoginID == pu.LoginID && a.InforID == pu.InforID).SingleOrDefault<InforUser>();
            prUS.IsAdmin = pu.IsAdmin;
            return    db.SaveChanges();
           
        }
    }
}
