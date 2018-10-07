using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
namespace Model.DAO
{
   
    public class ProjectProductDao
    {
        PTTDataContext db = null;

        public ProjectProductDao()
        {

            db = new PTTDataContext();
        }
        public List<ProjectProduct> ToList()
        {
            return db.ProjectProducts.ToList<ProjectProduct>();
        }
        public int Insert(ProjectProduct buider)
        {
            db.ProjectProducts.Add(buider);
           
            return db.SaveChanges();
        }
        public int Delete(long projectID, long productID)
        {
            var bd = db.ProjectProducts.Where(a => a.ProductID == productID && a.ProjectID == projectID).SingleOrDefault();
            db.ProjectProducts.Remove(bd);
           
            return db.SaveChanges();
        }
        public int Delete(long projectID)
        {
            var lst = this.FindByID(projectID);
            int i = 0;
            foreach (var pp in lst)
            {
                Delete(pp.ProjectID, pp.ProductID);
                i++;
            }
           
            return i;
        }
        public List<ProjectProduct> FindByID(long projectID)
        {
            var list = db.ProjectProducts.Where(a =>a.ProjectID == projectID).ToList<ProjectProduct>();
            return list;
        }
        
        public int Update(ProjectProduct pp)
        {
            var bd = db.ProjectProducts.Where(a => a.ProductID == pp.ProductID && a.ProjectID == pp.ProjectID).SingleOrDefault();
            bd.Price = pp.Price;
            bd.Discount = bd.Discount;
            bd.DiscountVAT = bd.DiscountVAT;
            return    db.SaveChanges();
           
        }
    }
}
