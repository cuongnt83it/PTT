using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
using System.Data.SqlClient;

namespace Model.DAO
{
    public class InformationDao
    {
        PTTDataContext db = null;

        public InformationDao()
        {

            db = new PTTDataContext();
        }
        public List<Information> ToList()
        {
            return db.Information.OrderByDescending(c => c.ModifiedDate).ToList<Information>();
        }
        public List<Information> ToListShared()
        {
            return db.Information.Where(f => f.Status > 0 && f.Status < 3).OrderBy(c => c.ModifiedDate).ToList<Information>();
        }
        public List<Information> ToListJod(long loginID)
        {
            var sql = (from inf in db.Information
                       join iu in db.InforUsers on inf.InformationID equals iu.InforID
                       where iu.LoginID == loginID
                       select new Information()
                       {
                           Address = inf.Address,
                           BuilderID = inf.BuilderID,
                           ContratorID = inf.ContratorID,
                           CreateBy = inf.CreateBy,
                           CreateDate = inf.CreateDate,
                           DateLine = inf.DateLine,
                           Description = inf.Description,
                           InformationID = inf.InformationID,
                           ModifiedBy = inf.ModifiedBy,
                           ModifiedDate = inf.ModifiedDate,
                           Name = inf.Name,
                           Note = inf.Note,
                           Status = inf.Status,
                           SupplierID = inf.SupplierID
                       }).ToList();



            return sql;
        }
        public long Insert(Information buider)
        {
            db.Information.Add(buider);
            db.SaveChanges();
            return buider.InformationID;
        }

        public long Delete(long ID)
        {
            //Xóa thông tin chia sẻ bảng Feedback Information 
            var fb = new FeedbackInforDao();
            fb.DeleteByFeedbackID(ID);
            var bd = db.Information.Find(ID);
            db.Information.Remove(bd);
            db.SaveChanges();
            return bd.InformationID;
        }

        public List<Information> FindByBuider(long biulderID)
        {

            return db.Information.Where(a => a.BuilderID == biulderID).ToList<Information>();
        }
        public List<Information> FindBySupplier(long supplierID)
        {

            return db.Information.Where(a => a.SupplierID == supplierID).ToList<Information>();
        }
        public List<Information> FindByContrator(long contratorID)
        {

            return db.Information.Where(a => a.ContratorID == contratorID).ToList<Information>();
        }


        public Information FindByID(long ID)
        {

            return db.Information.Find(ID);
        }
        public long Update(Information buider)
        {
            var bd = db.Information.Find(buider.InformationID);
            bd.InformationID = buider.InformationID;
            bd.Name = buider.Name;
            //bd.CreateBy = buider.CreateBy;
            //bd.CreateDate = buider.CreateDate;

            bd.Address = buider.Address;

            bd.BuilderID = buider.BuilderID;

            bd.ContratorID = buider.ContratorID;
            bd.DateLine = buider.DateLine;



            bd.Note = buider.Note;

            bd.Status = buider.Status;
            bd.SupplierID = buider.SupplierID;


            bd.Description = buider.Description;
            bd.ModifiedBy = buider.ModifiedBy;
            bd.ModifiedDate = buider.ModifiedDate;

            db.SaveChanges();
            return buider.InformationID;
        }
    }
}
