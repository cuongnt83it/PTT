using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;

namespace Model.DAO
{
   public class PermissionDao
    {
        PTTDataContext db = null;
        public PermissionDao()
        {

            db = new PTTDataContext();
        }

        public int Update(Permission entity)
        {
            var gr = db.Permissions.SingleOrDefault(x => x.PermissionID == entity.PermissionID);
            gr.Description = entity.Description;
       // gr.Name = entity.Name;


            db.SaveChanges();
            return entity.PermissionID;

        }
        public int Delete(long id)
        {
            var gr = db.Permissions.SingleOrDefault(x => x.PermissionID == id);

            db.Permissions.Remove(gr);


            return db.SaveChanges();

        }
        public List<Permission> ListPermistion()
        {
            return db.Permissions.ToList<Permission>();
        }
        public Permission GetPermistionByID(int PermissionID)
        {
            return db.Permissions.SingleOrDefault(x => x.PermissionID == PermissionID);

        }
        public List<Permission> GetPermistionByBusinessID(string BusinessID)
        {
            return db.Permissions.Where(x => x.BusinessID == BusinessID).ToList();

        }
    }
}
