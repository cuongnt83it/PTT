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
    public class BuildersController : BaseController
    {
        // GET: Builders
        public ActionResult Index()

        {
            BuilderDao bdDao = new BuilderDao();
            return View(bdDao.ToList());
        }

        // GET: Builders/Details/5
        public ActionResult Details(long id)
        {
            BuilderDao bdDao = new BuilderDao();
            return View(bdDao.FindByID(id));
        }

        // GET: Builders/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Builders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Builder collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // TODO: Add insert logic here

                    BuilderDao bdDao = new BuilderDao();
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

        // GET: Builders/Edit/5
        public ActionResult Edit(long id)
        {
            BuilderDao bdDao = new BuilderDao();
            return View(bdDao.FindByID(id));
        }

        // POST: Builders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Builder collection)
        {
            try
            {
                // TODO: Add update logic here
                if (ModelState.IsValid)
                {
                    // TODO: Add insert logic here

                    BuilderDao bdDao = new BuilderDao();
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



        // POST: Builders/Delete/5
        [HttpDelete]
        public ActionResult Delete(long id)
        {
            try
            {
                // TODO: Add delete logic here

                BuilderDao bdDao = new BuilderDao();
                ProjectDao prDao = new ProjectDao();
                if (prDao.FindByBuider(id).Count > 0)
                {

                    SetAlert("Đang sử dụng không được phép xóa",Common.CommonConstant.ALERT_DANGER);
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
