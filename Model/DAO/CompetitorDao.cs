using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
using System.Data.SqlClient;

namespace Model.DAO
{
    public class CompetitorDao
    {
        PTTDataContext db = null;

        public CompetitorDao()
        {

            db = new PTTDataContext();
        }
        public List<Competitor> ToList()
        {
            return db.Competitors.ToList<Competitor>();
        }
        public List<Competitor> ToListActive()
        {
            return db.Competitors.Where(c => c.Status == true).ToList<Competitor>();
        }
        public long Insert(Competitor buider)
        {
            db.Competitors.Add(buider);
            db.SaveChanges();
            return buider.ID;
        }
        public long Delete(long ID)
        {
            var bd = db.Competitors.Find(ID);
            db.Competitors.Remove(bd);
            db.SaveChanges();
            return bd.ID;
        }
        public Competitor FindByID(long ID)
        {
            
             return db.Competitors.Find(ID);
        }
        public string GenaraCode(string str, int lengh)
        {
            return db.Database.SqlQuery<string>("exec proc_t_GenaraCompetitorCode @Code,@len", new SqlParameter("@Code", str), new SqlParameter("@len", lengh)).SingleOrDefault();
        }
        public long Update(Competitor buider)
        {
            var bd = db.Competitors.Find(buider.ID);
          //  bd.CompetitorID = buider.CompetitorID;
            bd.FullName = buider.FullName;
            //bd.CreateBy = buider.CreateBy;
            //bd.CreateDate = buider.CreateDate;
            bd.Email = buider.Email;
            bd.CompetitorName = buider.CompetitorName;
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
