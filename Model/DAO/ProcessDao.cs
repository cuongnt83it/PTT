using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
namespace Model.DAO
{
    public class ProcessDao
    {
        PTTDataContext db = null;

        public ProcessDao()
        {

            db = new PTTDataContext();
        }
        public List<Process> ToList()
        {
            return db.Processes.OrderBy(c => c.ProjectID).ToList<Process>();
        }
        public List<Process> ToListByProjectID(long projectID)
        {
            return db.Processes.Where(c => c.ProjectID==projectID).ToList<Process>();
        }
        public List<ProcessUser> ToListProcessUserByProjectID(long projectID)
        {
            List<ProcessUser> lst = (from pr in db.Processes
                                     join us in db.Users
                                     on pr.CreateBy equals us.UserName
                                     where pr.ProjectID == projectID 
                                     orderby pr.ProcessID descending
                                     select new ProcessUser
                                     {
                                         ProcessID = pr.ProcessID,
                                         CreateBy = pr.CreateBy,
                                         CreateDate = pr.CreateDate,
                                         Description = pr.Description,
                                         FullName = us.FullName,
                                         Image = us.Image,
                                         ModifiedBy = pr.ModifiedBy,
                                         ModifiedDate = pr.ModifiedDate,
                                         Name = pr.Name,
                                         Phone = us.Phone,
                                         ProjectID = pr.ProjectID,
                                         UserName = us.UserName

                                     }

                                     ).ToList<ProcessUser>();
            return lst;
           
        }
        public long Insert(Process buider)
        {
            db.Processes.Add(buider);
            db.SaveChanges();
            return buider.ProcessID;
        }
       public long CountProcessMessage(long projectID)
        {
            long msgNumber = 0;
            msgNumber = db.Processes.Where(p => p.ProjectID == projectID).ToList<Process>().Count();
            msgNumber += GetListProjectProcessMessege(projectID).Count();
            return msgNumber;
        }
        public List<ProjectMessage> GetListProjectProcessMessege(long projectID)
        {
            List<ProjectMessage> lstProjectMessege = new List<ProjectMessage>();
            lstProjectMessege = (from p in db.Processes
                                  join m in db.Messeges
                                  on p.ProcessID equals m.ProcessID
                                  join u in db.Users
                                  on m.CreateBy equals u.UserName
                                  where p.ProjectID == projectID
                                  select new ProjectMessage
                                  {
                                      ChildID = m.ChildID,
                                      ContentMsg = m.Description,
                                      Description = p.Description,
                                      CreateBy = p.CreateBy,
                                      CreateByMsg = m.CreateBy,
                                      CreateDate = p.CreateDate,
                                      CreateDateMsg = m.CreateDate,
                                      ID = m.ID,
                                      ModifiedBy = p.ModifiedBy,
                                      ModifiedByMsg = m.ModifiedBy,
                                      ModifiedDate = p.ModifiedDate,
                                      ModifiedDateMsg = p.ModifiedDate,
                                      Name = p.Name,
                                      ProcessID = p.ProcessID,
                                      ProjectID = p.ProjectID,
                                      UsersRead = m.UsersRead,
                                      FullName =u.FullName,
                                      Image =u.Image,
                                      Phone = u.Phone
                                    

                                 }).OrderBy(a=>a.ProcessID).ToList<ProjectMessage>();
            return lstProjectMessege;

        }
        public long Delete(long ID)
        {
            var bd = db.Processes.Find(ID);
            db.Processes.Remove(bd);
            db.SaveChanges();
            return bd.ProcessID;
        }
        public void DeleteByProject(long ID)
        {
            var lst= db.Processes.Where(p => p.ProjectID == ID).ToList();
            foreach (var p in lst)
            {
                this.Delete(p.ProcessID);
            }

        }
        public Process FindByID(long ID)
        {
            
             return db.Processes.Find(ID);
        }
        public long Update(Process buider)
        {
            var bd = db.Processes.Find(buider.ProcessID);
            bd.ProjectID = buider.ProjectID;
            bd.Name = buider.Name;
            //bd.CreateBy = buider.CreateBy;
            //bd.CreateDate = buider.CreateDate;
            bd.Name = buider.Name;
          
            bd.Description = buider.Description;
            
            bd.ModifiedBy = buider.ModifiedBy;
            bd.ModifiedDate = buider.ModifiedDate;
          
            db.SaveChanges();
            return buider.ProcessID;
        }
    }
}
