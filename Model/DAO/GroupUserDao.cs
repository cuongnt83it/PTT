using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
namespace Model.DAO
{
    public class GroupUserDao
    {
        PTTDataContext db = null;

        public GroupUserDao()
        {

            db = new PTTDataContext();
        }
        public List<GroupUser> ToList()
        {
            return db.GroupUsers.ToList<GroupUser>();
        }
        public bool Insert(GroupUser buider)
        {
            try
            {
                
                db.GroupUsers.Add(buider);
                db.SaveChanges();
                return true;

            }

            catch
            {
                return false;
            }


        }
        public GroupUser FiindByID(Guid ID, long uerID)
        {
          
                return db.GroupUsers.SingleOrDefault(a => a.GroupID == ID && a.LoginID == uerID);
        }
        public bool Delete(Guid ID, long uerID)
        {
            try
            {
                var bd = db.GroupUsers.SingleOrDefault(a => a.GroupID == ID && a.LoginID == uerID);
                db.GroupUsers.Remove(bd);
                db.SaveChanges();
                return true;

            }

            catch
            {
                return false;
            }
           
          
        }
        public bool DeleteByLoginID(long uerID)
        {
            try
            {
                var listGU = FindByLoginID(uerID);
                foreach(GroupUser gu in listGU)
                {
                    db.GroupUsers.Remove(gu);

                }
                db.SaveChanges();

                return true;

            }

            catch
            {
                return false;
            }


        }
        public List<GroupUser> FindByGroupID(Guid ID)
        {

            return db.GroupUsers.Where(a => a.GroupID == ID).ToList<GroupUser>();
        }
        public List< GroupUser> FindByLoginID(long ID)
        {

            return db.GroupUsers.Where(a => a.LoginID == ID).ToList<GroupUser>();
        }
        public bool Update(Group buider)
        {
            try
            {
                var bd = db.Groups.Find(buider.GroupID);
                bd.GroupID = buider.GroupID;
                bd.GroupName = buider.GroupName;
                bd.Note = buider.GroupName;
                bd.IsAdmin = buider.IsAdmin;
                //bd.CreateBy = buider.CreateBy;
                //bd.CreateDate = buider.CreateDate;
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }


        }
    }
}
