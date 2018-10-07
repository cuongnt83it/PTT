using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
namespace Model.DAO
{
    public class GroupDao
    {
        PTTDataContext db = null;

        public GroupDao()
        {

            db = new PTTDataContext();
        }
        public List<Group> ToList()
        {
            return db.Groups.ToList<Group>();
        }
        public bool Insert(Group buider)
        {
            try
            {
                buider.GroupID = Guid.NewGuid();
                db.Groups.Add(buider);
                db.SaveChanges();
                return true;

            }

            catch
            {
                return false;
            }


        }
        public bool Delete(Guid ID)
        {
            try
            {
                var bd = db.Groups.SingleOrDefault(a => a.GroupID == ID);
                db.Groups.Remove(bd);
                db.SaveChanges();
                return true;

            }

            catch
            {
                return false;
            }
           
          
        }
        public Group FindByID(Guid ID)
        {

            return db.Groups.Where(a => a.GroupID == ID).SingleOrDefault();
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
