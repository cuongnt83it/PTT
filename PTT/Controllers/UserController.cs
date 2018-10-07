using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.DAO;
using Model.EF;
using PTT.Common;
using PTT.Models;

namespace PTT.Controllers
{
    [AuthorizeBusiness]
    public class UserController : BaseController
    {
        // GET: User
        public ActionResult Index()
        {
            UserDao bdDao = new UserDao();
            SetGroupViewBag();
            return View(bdDao.ToList());
        }

        // GET: User/Details/5
        public ActionResult Details(long id)
        {
            UserDao bdDao = new UserDao();
            var sl = bdDao.FindByID(id);

            return View(sl);
        }

        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // TODO: Add insert logic here

                    UserDao bdDao = new UserDao();
                    if (bdDao.GetUserByUserName(collection.UserName) != null)
                    {
                        ModelState.AddModelError("", "Tài khoản đã tồn tại!");
                        return View();
                    }
                    // UserLogin us = (UserLogin)Session[CommonConstant.USER_SESSION];
                    collection.CreatedDate = Hepper.GetDateServer();
                    collection.LastChangedPassword = Hepper.GetDateServer();
                    collection.LastLogIn = Hepper.GetDateServer();
                    collection.Password = Hepper.MD5Hash(collection.UserName + collection.Password);
                    //collection.ModifiedBy = us.UserName;
                    if (bdDao.Insert(collection) > 0)
                    {
                        SetAlert("Thêm thành công", "success");
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        SetAlert("Không thêm được", "danger");
                    }
                }
                return View();
            }
            catch
            {
                SetAlert("Không thêm được", "danger");
                return View();
            }
        }

        // GET: User/Edit/5
        public ActionResult Edit(long id)
        {
            UserDao bdDao = new UserDao();
            var sl = bdDao.FindByID(id);
            //if (sl.Status == true) { ViewBag.Status = "Kích hoạt"; }
            //else { ViewBag.Status = "Khóa"; }
            return View(sl);
        }

        public new ActionResult Profile()
        {
            UserLogin us = (UserLogin)Session[CommonConstant.USER_SESSION];
            UserDao bdDao = new UserDao();
            var sl = bdDao.FindByID(us.UserID);
            //if (sl.Status == true) { ViewBag.Status = "Kích hoạt"; }
            //else { ViewBag.Status = "Khóa"; }
            return View(sl);
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public new ActionResult Profile(User collection)
        {
            try
            { //TODO: Add update logic here
                if (ModelState.IsValid)
                {
                    // TODO: Add insert logic here
                    UserDao bdDao = new UserDao();
                    if (Session[CommonConstant.USER_SESSION] == null)
                    {
                        return RedirectToAction("Index", "Login");
                    }
                    UserLogin us = (UserLogin)Session[CommonConstant.USER_SESSION];

                    collection.Password = Hepper.MD5Hash(collection.UserName + collection.Password);
                    if (bdDao.Update(collection) > 0)
                    {
                        SetAlert("Sửa thành công", "success");
                        return RedirectToAction("Index","Home");
                    }
                    else
                    {
                        SetAlert("Không sửa được", "danger");
                    }
                }
                return View();

            }
            catch
            {
                SetAlert("Không sửa được", "danger");
                return View();
            }
        }
        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User collection)
        {
            try
            { //TODO: Add update logic here
                if (ModelState.IsValid)
                {
                    // TODO: Add insert logic here
                    UserDao bdDao = new UserDao();
                    UserLogin us = (UserLogin)Session[CommonConstant.USER_SESSION];

                    collection.Password = Hepper.MD5Hash(collection.UserName + collection.Password);
                    if (bdDao.Update(collection) > 0)
                    {
                        SetAlert("Sửa thành công", "success");
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        SetAlert("Không sửa được", "danger");
                    }
                }
                return View();

            }
            catch
            {
                SetAlert("Không sửa được", "danger");
                return View();
            }
        }
        // POST: Group/Delete/5
        [HttpDelete]
        public ActionResult Delete(long id)
        {
            try
            {
                // TODO: Add delete logic here

                UserDao bdDao = new UserDao();
                ProjectDao prDao = new ProjectDao();
                if (prDao.FindByUser(id).Count > 0)
                {

                    SetAlert("Đang sử dụng không được phép xóa", Common.CommonConstant.ALERT_DANGER);
                    return RedirectToAction("Index");
                }
                bdDao.Delete(id);
                // SetAlert("Xóa thành công", "success");
                return RedirectToAction("Index");
            }
            catch
            {
                // SetAlert("Không xóa được", "danger");
                return View();
            }
        }
        public void SetGroupViewBag(Guid[] selectedId = null)
        {
            var dao = new GroupDao();
            ViewBag.Group = new MultiSelectList(dao.ToList(), "GroupID", "GroupName", selectedId);
           
        }
        [HttpPost]
        public JsonResult UpdateGroup(long userID, Guid[] groups)
        {

            var dao = new GroupUserDao();
            dao.DeleteByLoginID(userID);
            foreach (Guid g in groups)
            {
                GroupUser gu = new GroupUser();
                gu.GroupID = g;
                gu.LoginID = userID;
                dao.Insert(gu);
            }
            
            JsonResult result = new JsonResult();
            result.Data = "success";
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }
        [HttpPost]
        public JsonResult ListGroupUser(long userID)
        {

            var dao = new GroupUserDao();
            List<GroupUserGrand> lstgr = new List<GroupUserGrand>();
            var list = dao.FindByLoginID(userID);
            var lstGroup =new GroupDao().ToList();
            foreach (var gr in lstGroup)
            {
                GroupUserGrand objGr = new GroupUserGrand();
                objGr.GroupID = gr.GroupID;
                objGr.GroupName = gr.GroupName;
                objGr.isGranted = false;
                foreach(var ug in list)
                {
                    if(ug.GroupID == gr.GroupID)
                    {
                        objGr.isGranted = true;
                        
                    }
                }
                lstgr.Add(objGr);

            }
            

            JsonResult result = new JsonResult();

            result.Data = lstgr;
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

    }
}
