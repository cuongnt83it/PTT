using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
namespace Model.DAO
{
    public class CategoryDao
    {
        PTTDataContext db = null;

        public CategoryDao()
        {

            db = new PTTDataContext();
        }
        public List<Category> ToList()
        {
            return db.Categories.OrderBy(c => c.DisplayOrder).ToList<Category>();
        }
        public long Insert(Category buider)
        {
            db.Categories.Add(buider);
            db.SaveChanges();
            return buider.CategoryID;
        }
        public long Delete(long ID)
        {
            var bd = db.Categories.Find(ID);
            db.Categories.Remove(bd);
            db.SaveChanges();
            return bd.CategoryID;
        }
        public Category FindByID(long ID)
        {
            
             return db.Categories.Find(ID);
        }
        public long Update(Category buider)
        {
            var bd = db.Categories.Find(buider.CategoryID);
            bd.CategoryID = buider.CategoryID;
            bd.Name = buider.Name;
            //bd.CreateBy = buider.CreateBy;
            //bd.CreateDate = buider.CreateDate;
            bd.MetaTite = buider.MetaTite;
            bd.DisplayOrder = buider.DisplayOrder;
            bd.Status = buider.Status;
           
            bd.ModifiedBy = buider.ModifiedBy;
            bd.ModifiedDate = buider.ModifiedDate;
          
            db.SaveChanges();
            return buider.CategoryID;
        }
    }
}
