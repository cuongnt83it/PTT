using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
namespace Model.DAO
{
   
    public class ProjectContratorDao
    {
        PTTDataContext db = null;

        public ProjectContratorDao()
        {

            db = new PTTDataContext();
        }
        public List<ProjectContrator> ToList()
        {
            return db.ProjectContrators.ToList<ProjectContrator>();
        }
        public long Insert(ProjectContrator buider)
        {
            db.ProjectContrators.Add(buider);
            db.SaveChanges();
            return buider.ID;
        }
        public int Delete(long projectID, long ContratorID)
        {
            var bd = db.ProjectContrators.Where(a => a.ContratorID == ContratorID && a.ProjectID == projectID).SingleOrDefault();
            db.ProjectContrators.Remove(bd);
           
            return db.SaveChanges();
        }
        public int Delete(long id)
        {
            var bd = db.ProjectContrators.Where(a => a.ID == id ).SingleOrDefault();
            db.ProjectContrators.Remove(bd);

            return db.SaveChanges();
        }
        public int DeleteByProjectID(long id)
        {
            ProjectContratorDao cmpDao = new ProjectContratorDao();
            int i = 0;
            var lst = this.FindByID(id);
            foreach(var pc in lst)
            {
                this.Delete(pc.ID);
                i++;
            }
            return i;
        }

        public List<ProjectContrator> FindByID(long projectID)
        {
            var list = db.ProjectContrators.Where(a =>a.ProjectID == projectID).ToList<ProjectContrator>();
            return list;
        }
        
        public int Update(ProjectContrator pp)
        {
            var bd = db.ProjectContrators.Where(a => a.ID == pp.ID).SingleOrDefault();
            bd.ContratorID = pp.ContratorID;
            bd.ProjectID = bd.ProjectID;
            return    db.SaveChanges();
           
        }
    }
}
