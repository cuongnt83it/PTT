using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;

namespace Model.DAO
{
    public class UserDao
    {
        PTTDataContext db = null;

        public UserDao()
        {

            db = new PTTDataContext();
        }
        public long Insert(User entity)
        {
            db.Users.Add(entity);
            db.SaveChanges();
            return entity.LoginID;

        }

        public List<User> ToList()
        {
            return db.Users.ToList<User>();
        }

        public User FindByID(long id)
        {
            var us = db.Users.SingleOrDefault(x => x.LoginID == id);
            return us;
        }

        public long Delete(long ID)
        {
            var us = db.Users.SingleOrDefault(x => x.LoginID ==ID);
            db.Users.Remove(us);
            db.SaveChanges();
            return ID;

        }
        public long Update(User entity)
        {
            var us = db.Users.SingleOrDefault(x => x.LoginID == entity.LoginID);
            us.Address = entity.Address;
            us.BirthDay = entity.BirthDay;
            us.Email = entity.Email;
            us.DeadlineOfUsing = entity.DeadlineOfUsing;
            us.FullName = entity.FullName;
            us.Gender = entity.Gender;
            us.Image = entity.Image;
            us.LockedDate = entity.LockedDate;
            us.LockedReason = entity.LockedReason;
            us.LockedUser = entity.LockedUser;
            us.Password = entity.Password;
            us.Phone = entity.Phone;
            us.UserName = entity.UserName;
            db.SaveChanges();
            return entity.LoginID;

        }
        public void LastLogin(long UserID, DateTime Now)
        {
           var us= db.Users.SingleOrDefault(x => x.LoginID == UserID);
            us.LastLogIn = Now;
            db.SaveChanges();
            

        }
        public List<User> listUser()
        {
            return db.Users.ToList<User>();
        }
        public User GetUserByID(long UserID)
        {
            return db.Users.SingleOrDefault(x => x.LoginID == UserID);

        }
        public User GetUserByUserName(string UserName)
        {
            return db.Users.SingleOrDefault(x => x.UserName == UserName);
        }

       
        public int Login(string UserName, string Password)
        {
            int resuft = -2;
            var objUser = db.Users.SingleOrDefault(x => x.UserName == UserName);

            if (objUser == null)
            {
                resuft = 0;
            }
            else
            {
                if (objUser.Password == Password)
                {
                    if (objUser.LockedUser == true)
                    { resuft = -1; }
                    else
                    {
                        resuft = 1;
                    }
                }
                else
                {
                    resuft = -2;
                }


            }
            return resuft;

        }
        //Kiểm tra user có thuộc nhóm Admin ko
        public bool isAdmin(long userID)
        {
            var sqlgu = (from gu in db.GroupUsers
                         where gu.LoginID == userID
                         select gu).ToList<GroupUser>();
            if (sqlgu.Count < 0)
            {
                return false;
            }
            foreach (var GroupUser in sqlgu)
            {
                var sqlgroup = (from g in db.Groups where g.GroupID == GroupUser.GroupID select g).FirstOrDefault();
                //Nhom la admin co toan quyen.
                if (sqlgroup.IsAdmin) return true;

            }
            return false;

        }
        //Kiểm tra user có thuộc nhóm Admin ko
        public int isManager(long userID)
        {
            var sqlgu = (from gu in db.GroupUsers
                         where gu.LoginID == userID
                         select gu).ToList<GroupUser>();
            if (sqlgu.Count < 0)
            {
                return 0;
            }
            //ID nhóm manager
            Guid guIDManager = Guid.Parse("964D283D-BEA0-4D85-B7C0-355487A5DF0C");

            //ID nhóm IT
            Guid guIDIT = Guid.Parse("76974ade-0804-4dcf-9948-3c9f027a0417");
            foreach (var GroupUser in sqlgu)
            {
                var sqlgroup = (from g in db.Groups where g.GroupID == GroupUser.GroupID select g).FirstOrDefault();
                //Nhom la admin co toan quyen.
                if (sqlgroup.GroupID== guIDManager) return 1;
                if (sqlgroup.GroupID == guIDIT) return 3;
                if (sqlgroup.IsAdmin ) return 2;
            }
            return 0;

        }
        //Lấy quyền của nhóm
        public    List<string> HasPermistionGroup(Guid grID)
        {
            var listPermistionGorup = from p in db.Permissions
                                      join g in db.GrantPermissions
                                      on p.PermissionID equals g.PermissionID
                                      where g.GroupID == grID
                                      select p.Name ;
            return listPermistionGorup.ToList<string>();
        }
        //Lấy quyền  user
       public List<string> HasPermistionUser(long userID)
        {
            List<string> listPermistion= new List<string>();

            var sqlgu = (from gu in db.GroupUsers
                         where gu.LoginID == userID
                         select gu).ToList<GroupUser>();
            foreach (GroupUser gu in sqlgu)
            {
                List<string> lug = HasPermistionGroup(gu.GroupID);
                //Thêm list tên quyền vào
                listPermistion.AddRange(lug);
            }
            return listPermistion;


        }
    }
}
