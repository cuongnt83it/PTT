using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
namespace Model.DAO
{
   
    public class CompetiorProductDao
    {
        PTTDataContext db = null;

        public CompetiorProductDao()
        {

            db = new PTTDataContext();
        }
        public List<CompetiorProduct> ToList()
        {
            return db.CompetiorProducts.ToList<CompetiorProduct>();
        }
        public long Insert(CompetiorProduct buider)
        {
            db.CompetiorProducts.Add(buider);
            db.SaveChanges();
            return buider.ID;
        }
        public int Delete(long productID, long ID)
        {
            var bd = db.CompetiorProducts.Where(a => a.ID == ID && a.ProductID == productID).SingleOrDefault();
            db.CompetiorProducts.Remove(bd);
           
            return db.SaveChanges();
        }
        public int Delete(long id)
        {
            var lst = db.CompetiorProducts.Where(a => a.ID == id ).ToList<CompetiorProduct>();
            int i = 0;
            foreach(var comp in lst)
            {
                this.Delete(comp.ProductID, comp.ID);
                i++;
            }
            return i;
          
            
        }
        public List<CompetiorProduct> FindByID(long id)
        {
            var list = db.CompetiorProducts.Where(a =>a.ID == id).ToList<CompetiorProduct>();   
            return list;
        }
        
        public int Update(CompetiorProduct pp)
        {
            var bd = db.CompetiorProducts.Where(a => a.ID == pp.ID&& a.ProductID ==pp.ProductID).SingleOrDefault();
            bd.Discount = pp.Discount;
            bd.DiscountVAT = pp.DiscountVAT;
            return    db.SaveChanges();
           
        }
    }
}
