using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
namespace Model.DAO
{
    public class FeedbackDao
    {
        PTTDataContext db = null;

        public FeedbackDao()
        {

            db = new PTTDataContext();
        }
        public List<Feedback> ToList()
        {
            return db.Feedbacks.OrderBy(c => c.FeedbackID).ToList<Feedback>();
        }
        public List<Feedback> ToListByProjectID(long prjectID)
        {
            return db.Feedbacks.Where(c => c.ProjectID== prjectID).ToList<Feedback>();
        }
        public List<FeedbackUser> ToListFeebBackUser(long prjectID)
        {
            List<FeedbackUser> lst = (from f in db.Feedbacks
                                      join u in db.Users
                                      on f.CreateBy equals u.UserName
                                      where f.ProjectID == prjectID
                                      orderby f.FeedbackID descending
                                      select new FeedbackUser
                                      {
                                          ProjectID = f.ProjectID,
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

                                      }).ToList<FeedbackUser>();
                return lst;
        }
        public long Insert(Feedback buider)
        {
            db.Feedbacks.Add(buider);
            db.SaveChanges();
            return buider.FeedbackID;
        }
        public long Delete(long ID)
        {
            var bd = db.Feedbacks.Find(ID);
            db.Feedbacks.Remove(bd);
            db.SaveChanges();
            return bd.FeedbackID;
        }
        public void DeleteByProject(long projectID)
        {
            var lst = db.Feedbacks.Where(p => p.ProjectID == projectID).ToList();
            foreach(var f in lst)
            {
                this.Delete(f.FeedbackID);
            }
        }
        public Feedback FindByID(long ID)
        {
            
             return db.Feedbacks.Find(ID);
        }
        public long Update(Feedback buider)
        {
            var bd = db.Feedbacks.Find(buider.FeedbackID);
            bd.ProjectID = buider.ProjectID;
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
