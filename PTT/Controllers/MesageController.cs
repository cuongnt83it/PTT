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
    public class MesageController : BaseController
    {
        // GET: Mesage
        public ActionResult Index()
        {
            ContentDao bdDao = new ContentDao();
            return View(bdDao.ToList());
        }

        // GET: Mesage/Details/5
        public ActionResult Details(long id)
        {
            ContentDao bdDao = new ContentDao();
            return View(bdDao.FindByID(id));
        }

        // GET: Mesage/Details/5
        public ActionResult DetailMesage(long id)
        {
            ContentDao bdDao = new ContentDao();
            Content ms = bdDao.FindByID(id);
          var list = new ContentDao().ListHot();
            list.Remove(ms);
            ViewBag.listHot = list;
            return View(ms);

        }

        // GET: Mesage/Create
        public ActionResult Create()
        {
            return View();
        }
        public ActionResult MeseageFull()

        {
            ViewBag.lstMesage = new ContentDao().ListActive();
            return View();
        }

        [ChildActionOnly]
        public PartialViewResult TopMesage()
        {
            UserLogin us = (UserLogin)Session[CommonConstant.USER_SESSION];
            string usID = us.UserID.ToString();
            var list = new ContentDao().ListHot();
            List<Content> ls = new List<Content>();
            //Lấy danh sách những tin user này chưa xem
            foreach (Content ct in list)
            {
                if(ct.UsersRead!=null) {
                    string[] listUerID = ct.UsersRead.Split('.');
                    if (!listUerID.Contains(usID))
                    {
                        ls.Add(ct);
                    }
                }
                else
                {
                    ls.Add(ct);
                }
                
            }
            return PartialView(ls);
        }

        // POST: Mesage/Create
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Content collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // TODO: Add insert logic here

                    ContentDao bdDao = new ContentDao();
                    string detail = collection.Detail;
                    UserLogin us = (UserLogin)Session[CommonConstant.USER_SESSION];
                    collection.CreateDate = Hepper.GetDateServer();
                    collection.ModifiedDate = Hepper.GetDateServer();
                    collection.CreateBy = us.UserName;
                    collection.ModifiedBy = us.UserName;
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

        // GET: Mesage/Edit/5
        public ActionResult Edit(long id)
        {
            ContentDao bdDao = new ContentDao();
            return View(bdDao.FindByID(id));
        }

        // POST: Mesage/Edit/5
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Content collection)
        {
            try
            {// TODO: Add update logic here
                if (ModelState.IsValid)
                {
                    // TODO: Add insert logic here

                    ContentDao bdDao = new ContentDao();
                    UserLogin us = (UserLogin)Session[CommonConstant.USER_SESSION];

                    collection.ModifiedDate = Hepper.GetDateServer();

                    collection.ModifiedBy = us.UserName;
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


        // POST: Mesage/Delete/5
        [HttpDelete]
        public ActionResult Delete(long id)
        {
            try
            {
                // TODO: Add delete logic here

                ContentDao bdDao = new ContentDao();

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
        [HttpPost]
        public JsonResult UpdateUserRead(long msID)
        {
            UserLogin us = (UserLogin)Session[CommonConstant.USER_SESSION];

            var dao = new ContentDao();
            long data = 0;
            Content objMS = dao.FindByID(msID);
            string usID = us.UserID.ToString();
            string sUserID = objMS.UsersRead;

            string[] listUerID;
            bool kt = false;

            if (sUserID == null)
            {
                sUserID = usID;
                kt = true;
            }
            else
            {
                listUerID = objMS.UsersRead.Split('.');
                if (!listUerID.Contains(usID))
                {
                    sUserID += "." + usID;
                    kt = true;
                }

            }
            if (kt == true)
            {
                objMS.UsersRead = sUserID;
                data = dao.Update(objMS);
            }

            JsonResult result = new JsonResult();
            result.Data = data;
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }
    }
}
