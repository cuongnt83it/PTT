using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
namespace Model.DAO
{
    public class BuilderDao
    {
        PTTDataContext db = null;

        public BuilderDao()
        {

            db = new PTTDataContext();
        }
        public List<Builder> ToList()
        {
            return db.Builders.ToList<Builder>();
        }
        public List<Builder> ToListActive()
        {
            return db.Builders.Where(c=>c.Status==true).ToList<Builder>();
        }
        public long Insert(Builder buider)
        {
            db.Builders.Add(buider);
            db.SaveChanges();
            return buider.ID;
        }
        public long Delete(long ID)
        {
            var bd = db.Builders.Find(ID);
            db.Builders.Remove(bd);
            db.SaveChanges();
            return bd.ID;
        }
        public Builder FindByID(long ID)
        {
            
             return db.Builders.Find(ID);
        }
        public Builder FindByCode(string code)
        {
            var ct = db.Builders.SingleOrDefault<Builder>(c => c.BuilderID == code&&c.Status==true);
            return ct;
        }
        public long Update(Builder buider)
        {
            var bd = db.Builders.Find(buider.ID);
            bd.BuilderID = buider.BuilderID;
            bd.FullName = buider.FullName;
            //bd.CreateBy = buider.CreateBy;
            //bd.CreateDate = buider.CreateDate;
            bd.Email = buider.Email;
            bd.BuilderName = buider.BuilderName;
            bd.Address = buider.Address;
            bd.Status = buider.Status;
            bd.Image = buider.Image;
            bd.ModifiedBy = buider.ModifiedBy;
            bd.ModifiedDate = buider.ModifiedDate;
            bd.Phone = buider.Phone;
            db.SaveChanges();
            return buider.ID;
        }
    }
}
