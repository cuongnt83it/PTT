using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
namespace Model.DAO
{
   
    public class ProjectUserDao
    {
        PTTDataContext db = null;

        public ProjectUserDao()
        {

            db = new PTTDataContext();
        }
        public List<ProjectUser> ToList()
        {
            return db.ProjectUsers.ToList<ProjectUser>();
        }
        public int Insert(ProjectUser buider)
        {
            db.ProjectUsers.Add(buider);
            
            return db.SaveChanges();
        }
        public int Delete(long ProjectID)
        {
            var lstPrUs = db.ProjectUsers.Where(a => a.ProjectID == ProjectID).ToList<ProjectUser>();
            foreach(ProjectUser prus in lstPrUs)
            {
                db.ProjectUsers.Remove(prus);
            }
            
        
            return db.SaveChanges();
        }
        public List<ProjectUser> FindByProjectID(long ProjectID)
        {
            var list = db.ProjectUsers.Where(a => a.ProjectID == ProjectID).ToList<ProjectUser>();
             return list;
        }
        public List<ProjectUser> FindByUserID(long UerID)
        {
            var list = db.ProjectUsers.Where(a => a.LoginID == UerID).ToList<ProjectUser>();
            return list;
        }
        public ProjectUser FindByID(long UerID,long projectID)
        {
            var prUS = db.ProjectUsers.Where(a => a.LoginID == UerID&& a.ProjectID==projectID).SingleOrDefault<ProjectUser>();
            return prUS;
        }
        public int Update(ProjectUser pu)
        {
            var prUS = db.ProjectUsers.Where(a => a.LoginID == pu.LoginID && a.ProjectID == pu.ProjectID).SingleOrDefault<ProjectUser>();
            prUS.IsAdmin = pu.IsAdmin;
            return    db.SaveChanges();
           
        }
    }
}
