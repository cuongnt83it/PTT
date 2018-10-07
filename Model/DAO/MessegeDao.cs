using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
namespace Model.DAO
{
    public class MessegeDao
    {
        PTTDataContext db = null;

        public MessegeDao()
        {

            db = new PTTDataContext();
        }
        public List<Messege> ToList()
        {
            return db.Messeges.OrderBy(c => c.ProcessID).ToList<Messege>();
        }
        public List<Messege> ToListByProjectID(long processID)
        {
            return db.Messeges.Where(c => c.ProcessID== processID).ToList<Messege>();
        }
        public long Insert(Messege buider)
        {
            db.Messeges.Add(buider);
            db.SaveChanges();
            return buider.ID;
        }
        public long Delete(long ID)
        {
            var bd = db.Messeges.Find(ID);
            db.Messeges.Remove(bd);
            db.SaveChanges();
            return bd.ID;
        }
        public Messege FindByID(long ID)
        {
            
             return db.Messeges.Find(ID);
        }
        public long Update(Messege buider)
        {
            var bd = db.Messeges.Find(buider.ID);
            bd.ProcessID = buider.ProcessID;
            bd.UsersRead = buider.UsersRead;
            //bd.CreateBy = buider.CreateBy;
            //bd.CreateDate = buider.CreateDate;
            bd.Description = buider.Description;
          
           
            
            bd.ModifiedBy = buider.ModifiedBy;
            bd.ModifiedDate = buider.ModifiedDate;
          
            db.SaveChanges();
            return buider.ID;
        }
    }
}
