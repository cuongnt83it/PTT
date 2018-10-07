using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
namespace Model.DAO
{
    public class PriceDao
    {
        PTTDataContext db = null;

        public PriceDao()
        {

            db = new PTTDataContext();
        }
        public List<Price> ToList()
        {
            return db.Prices.OrderBy(c => c.DisplayOrder).ToList<Price>();
        }
        public long Insert(Price buider)
        {
            db.Prices.Add(buider);
            db.SaveChanges();
            return buider.PriceID;
        }
        public long Delete(long ID)
        {
            var bd = db.Prices.Find(ID);
            db.Prices.Remove(bd);
            db.SaveChanges();
            return bd.PriceID;
        }
        public Price FindByID(long ID)
        {
            
             return db.Prices.Find(ID);
        }
        public long Update(Price buider)
        {
            var bd = db.Prices.Find(buider.PriceID);
            bd.PriceID = buider.PriceID;
            bd.Name = buider.Name;
            //bd.CreateBy = buider.CreateBy;
            //bd.CreateDate = buider.CreateDate;
            bd.MetaTite = buider.MetaTite;
            bd.DisplayOrder = buider.DisplayOrder;
            bd.Status = buider.Status;
            bd.PriceEnd = buider.PriceEnd;
            bd.PriceStart = buider.PriceStart;
            bd.ModifiedBy = buider.ModifiedBy;
            bd.ModifiedDate = buider.ModifiedDate;
          
            db.SaveChanges();
            return buider.PriceID;
        }
    }
}
