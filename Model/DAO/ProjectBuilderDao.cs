using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
namespace Model.DAO
{
   
    public class ProjectBuilderDao
    {
        PTTDataContext db = null;

        public ProjectBuilderDao()
        {

            db = new PTTDataContext();
        }
        public List<ProjectBuilder> ToList()
        {
            return db.ProjectBuilders.ToList<ProjectBuilder>();
        }
        public long Insert(ProjectBuilder buider)
        {
            db.ProjectBuilders.Add(buider);
            db.SaveChanges();
            return buider.ID;
        }
        public int Delete(long projectID, long BuilderID)
        {
            var bd = db.ProjectBuilders.Where(a => a.BuilderID == BuilderID && a.ProjectID == projectID).SingleOrDefault();
            db.ProjectBuilders.Remove(bd);
           
            return db.SaveChanges();
        }
        public int Delete(long id)
        {
            var bd = db.ProjectBuilders.Where(a => a.ID == id ).SingleOrDefault();
            db.ProjectBuilders.Remove(bd);

            return db.SaveChanges();
        }
        public int DeleteByProjectID(long id)
        {
            ProjectBuilderDao cmpDao = new ProjectBuilderDao();
            int i = 0;
            var lst = this.FindByID(id);
            foreach(var pc in lst)
            {
                this.Delete(pc.ID);
                i++;
            }
            return i;
        }

        public List<ProjectBuilder> FindByID(long projectID)
        {
            var list = db.ProjectBuilders.Where(a =>a.ProjectID == projectID).ToList<ProjectBuilder>();
            return list;
        }
        
        public int Update(ProjectBuilder pp)
        {
            var bd = db.ProjectBuilders.Where(a => a.ID == pp.ID).SingleOrDefault();
            bd.BuilderID = pp.BuilderID;
            bd.ProjectID = bd.ProjectID;
            return    db.SaveChanges();
           
        }
    }
}
