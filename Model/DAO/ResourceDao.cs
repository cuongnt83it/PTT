using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
namespace Model.DAO
{
    public class ResourceDao
    {
        PTTDataContext db = null;

        public ResourceDao()
        {

            db = new PTTDataContext();
        }
        public List<Resource> ToList()
        {
            return db.Resources.OrderBy(c => c.DisplayOrder).ToList<Resource>();
        }
        public long Insert(Resource buider)
        {
            db.Resources.Add(buider);
            db.SaveChanges();
            return buider.ResourceID;
        }
        public long Delete(long ID)
        {
            var bd = db.Resources.Find(ID);
            db.Resources.Remove(bd);
            db.SaveChanges();
            return bd.ResourceID;
        }
        public Resource FindByID(long ID)
        {
            
             return db.Resources.Find(ID);
        }
        public long Update(Resource buider)
        {
            var bd = db.Resources.Find(buider.ResourceID);
            bd.ResourceID= buider.ResourceID;
            bd.Name = buider.Name;
            //bd.CreateBy = buider.CreateBy;
            //bd.CreateDate = buider.CreateDate;
            bd.DisplayOrder = buider.DisplayOrder;
            bd.MetaTite = buider.MetaTite;
            bd.Status = buider.Status;
           
            bd.ModifiedBy = buider.ModifiedBy;
            bd.ModifiedDate = buider.ModifiedDate;
          
            db.SaveChanges();
            return buider.ResourceID;
        }
    }
}
