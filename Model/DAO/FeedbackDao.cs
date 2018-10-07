using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
namespace Model.DAO
{
    public class FeedbackInforDao
    {
        PTTDataContext db = null;

        public FeedbackInforDao()
        {

            db = new PTTDataContext();
        }
        public List<FeedbackInfor> ToList()
        {
            return db.FeedbackInfors.OrderBy(c => c.FeedbackID).ToList<FeedbackInfor>();
        }
        public List<FeedbackInfor> ToListByProjectID(long InformationID)
        {
            return db.FeedbackInfors.Where(c => c.InformationID == InformationID).ToList<FeedbackInfor>();
        }
        public List<FeedbacInfokUser> ToListFeebBackUser(long InformationID)
        {
            List<FeedbacInfokUser> lst = (from f in db.FeedbackInfors
                                      join u in db.Users
                                      on f.CreateBy equals u.UserName
                                      where f.InformationID == InformationID
                                          orderby f.FeedbackID descending
                                      select new FeedbacInfokUser
                                      {
                                          InformationID = f.InformationID,
                                          ChildID = f.ChildID,
                                          CreateBy = f.CreateBy,
                                          CreateDate = f.CreateDate,
                                          Description = f.Description,
                                          FeedbackID = f.FeedbackID,
                                          FullName = u.FullName,
                                          Image = u.Image,
                                          ModifiedBy = f.ModifiedBy,
                                          ModifiedDate = f.ModifiedDate,
                                          Phone = u.Phone,
                                          UsersRead = f.UsersRead

                                      }).ToList<FeedbacInfokUser>();
                return lst;
        }
        public long Insert(FeedbackInfor buider)
        {
            db.FeedbackInfors.Add(buider);
            db.SaveChanges();
            return buider.FeedbackID;
        }
        public long Delete(long ID)
        {
            var bd = db.FeedbackInfors.Find(ID);
            db.FeedbackInfors.Remove(bd);
            db.SaveChanges();
            return bd.FeedbackID ;
        }
        public FeedbackInfor FindByID(long ID)
        {
            
             return db.FeedbackInfors.Find(ID);
        }
        public long Update(FeedbackInfor buider)
        {
            var bd = db.FeedbackInfors.Find(buider.FeedbackID);
            bd.InformationID = buider.InformationID;
            bd.UsersRead = buider.UsersRead;

            //bd.CreateBy = buider.CreateBy;
            //bd.CreateDate = buider.CreateDate;
            bd.Description = buider.Description;
            
           
            
            bd.ModifiedBy = buider.ModifiedBy;
            bd.ModifiedDate = buider.ModifiedDate;
          
            db.SaveChanges();
            return buider.FeedbackID;
        }
    }
}
