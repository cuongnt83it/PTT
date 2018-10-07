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
    public class PriceController : BaseController
    {
        // GET: Price
        public ActionResult Index()
        {
            PriceDao bdDao = new PriceDao();
            return View(bdDao.ToList());
        }

        // GET: Price/Details/5
        public ActionResult Details(long id)
        {
            PriceDao bdDao = new PriceDao();
            var sl = bdDao.FindByID(id);
            if (sl.Status == true) { ViewBag.Status = "Kích hoạt"; }
            else { ViewBag.Status = "Khóa"; }
            return View(sl);
        }

        // GET: Price/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Price/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Price collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // TODO: Add insert logic here

                    PriceDao bdDao = new PriceDao();
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

        // GET: Price/Edit/5
        public ActionResult Edit(long id)
        {
            PriceDao bdDao = new PriceDao();
            var sl = bdDao.FindByID(id);
            //if (sl.Status == true) { ViewBag.Status = "Kích hoạt"; }
            //else { ViewBag.Status = "Khóa"; }
            return View(sl);
        }

        // POST: Price/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Price collection)
        {
            try
            { // TODO: Add update logic here
                if (ModelState.IsValid)
                {
                    // TODO: Add insert logic here

                    PriceDao bdDao = new PriceDao();
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

        [HttpDelete]
        public ActionResult Delete(long id)
        {
            try
            {
                // TODO: Add delete logic here

                PriceDao bdDao = new PriceDao();
                ProjectDao prDao = new ProjectDao();
                if (prDao.FindByPrice(id).Count > 0)
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
