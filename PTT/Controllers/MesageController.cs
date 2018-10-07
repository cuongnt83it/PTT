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
            var ms = bdDao.FindByID(id);
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

            var list = new ContentDao().ListHot();
            
            return PartialView(list);
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
    }
}
