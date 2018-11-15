using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
namespace Model.DAO
{
   
    public class ProjectSupplierDao
    {
        PTTDataContext db = null;

        public ProjectSupplierDao()
        {

            db = new PTTDataContext();
        }
        public List<ProjectSupplier> ToList()
        {
            return db.ProjectSuppliers.ToList<ProjectSupplier>();
        }
        public long Insert(ProjectSupplier buider)
        {
            db.ProjectSuppliers.Add(buider);
            db.SaveChanges();
            return buider.ID;
        }
        public int Delete(long projectID, long supplierID)
        {
            var bd = db.ProjectSuppliers.Where(a => a.SupplierID == supplierID && a.ProjectID == projectID).SingleOrDefault();
            db.ProjectSuppliers.Remove(bd);
           
            return db.SaveChanges();
        }
        public int Delete(long id)
        {
            var bd = db.ProjectSuppliers.Where(a => a.ID == id ).SingleOrDefault();
            db.ProjectSuppliers.Remove(bd);

            return db.SaveChanges();
        }
        public int DeleteByProjectID(long id)
        {
            ProjectSupplierDao cmpDao = new ProjectSupplierDao();
            int i = 0;
            var lst = this.FindByID(id);
            foreach(var pc in lst)
            {
                this.Delete(pc.ID);
                i++;
            }
            return i;
        }

        public List<ProjectSupplier> FindByID(long projectID)
        {
            var list = db.ProjectSuppliers.Where(a =>a.ProjectID == projectID).ToList<ProjectSupplier>();
            return list;
        }
        
        public int Update(ProjectSupplier pp)
        {
            var bd = db.ProjectSuppliers.Where(a => a.ID == pp.ID).SingleOrDefault();
            bd.SupplierID = pp.SupplierID;
            bd.ProjectID = bd.ProjectID;
            return    db.SaveChanges();
           
        }
    }
}
