using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
namespace Model.DAO
{
    public class ProductDao
    {
        PTTDataContext db = null;

        public ProductDao()
        {

            db = new PTTDataContext();
        }
        public List<Product> ToList()
        {
            return db.Products.OrderBy(c => c.DisplayOrder).ToList<Product>();
        }
        public List<Product> ToListActive()
        {
            return db.Products.Where(c=>c.Status==true).OrderBy(c => c.DisplayOrder).ToList<Product>();
        }
        public long Insert(Product buider)
        {
            db.Products.Add(buider);
            db.SaveChanges();
            return buider.ProductID;
        }
        public long Delete(long ID)
        {
            var bd = db.Products.Find(ID);
            db.Products.Remove(bd);
            db.SaveChanges();
            return bd.ProductID;
        }
        public Product FindByID(long ID)
        {
            
             return db.Products.Find(ID);
        }
        public long Update(Product buider)
        {
            var bd = db.Products.Find(buider.ProductID);
            bd.ProductID = buider.ProductID;
            bd.Name = buider.Name;
            bd.Code = buider.Code;
            //bd.CreateBy = buider.CreateBy;
            //bd.CreateDate = buider.CreateDate;
            bd.MetaTite = buider.MetaTite;
            bd.DisplayOrder = buider.DisplayOrder;
            bd.Status = buider.Status;
            bd.Price = buider.Price;
            bd.Discount = buider.Discount;
            bd.Description = buider.Description;
            bd.ModifiedBy = buider.ModifiedBy;
            bd.ModifiedDate = buider.ModifiedDate;
          
            db.SaveChanges();
            return buider.ProductID;
        }
    }
}
