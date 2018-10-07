using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
namespace Model.DAO
{
   
    public class ProjectCompetitorDao
    {
        PTTDataContext db = null;

        public ProjectCompetitorDao()
        {

            db = new PTTDataContext();
        }
        public List<ProjectCompetitor> ToList()
        {
            return db.ProjectCompetitors.ToList<ProjectCompetitor>();
        }
        public long Insert(ProjectCompetitor buider)
        {
            db.ProjectCompetitors.Add(buider);
            db.SaveChanges();
            return buider.ID;
        }
        public int Delete(long projectID, long compeID)
        {
            var bd = db.ProjectCompetitors.Where(a => a.CompetiorID == compeID && a.ProjectID == projectID).SingleOrDefault();
            db.ProjectCompetitors.Remove(bd);
           
            return db.SaveChanges();
        }
        public int Delete(long id)
        {
            var bd = db.ProjectCompetitors.Where(a => a.ID == id ).SingleOrDefault();
            db.ProjectCompetitors.Remove(bd);

            return db.SaveChanges();
        }
        public int DeleteByProjectID(long id)
        {
            int i = 0;
            var lst = this.FindByID(id);
            foreach(var pc in lst)
            {
                this.Delete(pc.ID);
                i++;
            }
            return i;
        }

        public List<ProjectCompetitor> FindByID(long projectID)
        {
            var list = db.ProjectCompetitors.Where(a =>a.ProjectID == projectID).ToList<ProjectCompetitor>();
            return list;
        }
        
        public int Update(ProjectCompetitor pp)
        {
            var bd = db.ProjectCompetitors.Where(a => a.ID == pp.ID).SingleOrDefault();
            bd.CompetiorID = pp.CompetiorID;
            bd.ProjectID = bd.ProjectID;
            return    db.SaveChanges();
           
        }
    }
}
