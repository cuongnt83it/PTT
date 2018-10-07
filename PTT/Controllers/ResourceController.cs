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
    public class ResourceController : BaseController
    {
        // GET: Resource
        public ActionResult Index()
        {
            ResourceDao bdDao = new ResourceDao();
            return View(bdDao.ToList());
        }

        // GET: Resource/Details/5
        public ActionResult Details(long id)
        {
            ResourceDao bdDao = new ResourceDao();
            var sl = bdDao.FindByID(id);
            if (sl.Status == true) { ViewBag.Status = "Kích hoạt"; }
            else { ViewBag.Status = "Khóa"; }
            return View(sl);
        }

        // GET: Resource/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Resource/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Resource collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // TODO: Add insert logic here

                    ResourceDao bdDao = new ResourceDao();
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

        // GET: Resource/Edit/5
        public ActionResult Edit(long id)
        {
            ResourceDao bdDao = new ResourceDao();
            var sl = bdDao.FindByID(id);
            //if (sl.Status == true) { ViewBag.Status = "Kích hoạt"; }
            //else { ViewBag.Status = "Khóa"; }
            return View(sl);
        }

        // POST: Resource/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Resource collection)
        {
            try
            { // TODO: Add update logic here
                if (ModelState.IsValid)
                {
                    // TODO: Add insert logic here

                    ResourceDao bdDao = new ResourceDao();
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

      

        // POST: Resource/Delete/5
        [HttpDelete]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                ResourceDao bdDao = new ResourceDao();

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
