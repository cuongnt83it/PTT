using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Model.DAO;
using Model.EF;
using PTT.Common;
using PTT.Models;

namespace PTT.Controllers
{
    [AuthorizeBusiness]
    public class CategoryController : BaseController
    {
        // GET: Category
        public ActionResult Index()
        {
            CategoryDao bdDao = new CategoryDao();
            return View(bdDao.ToList());
           
        }

        // GET: Category/Details/5
        public ActionResult Details(long id)
        {
            CategoryDao bdDao = new CategoryDao();
            var sl = bdDao.FindByID(id);
            if (sl.Status == true) { ViewBag.Status = "Kích hoạt"; }
            else { ViewBag.Status = "Khóa"; }
            return View(sl);
        }

        // GET: Category/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category collection)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    // TODO: Add insert logic here

                    CategoryDao bdDao = new CategoryDao();
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

        // GET: Category/Edit/5
        public ActionResult Edit(long id)
        {
            CategoryDao bdDao = new CategoryDao();
            return View(bdDao.FindByID(id));
        }

        // POST: Category/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category collection)
        {
            try
            {
                // TODO: Add update logic here
                if (ModelState.IsValid)
                {
                    // TODO: Add insert logic here

                    CategoryDao bdDao = new CategoryDao();
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

        
        // POST: Category/Delete/5
        [HttpDelete]
        public ActionResult Delete(long id)
        {
            try
            {
                // TODO: Add delete logic here

                CategoryDao bdDao = new CategoryDao();
                ProjectDao prDao = new ProjectDao();
                if (prDao.FindByCategory(id).Count > 0)
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
    }
}
