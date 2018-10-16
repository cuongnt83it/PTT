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
    public class SupplierController : BaseController
    {
        // GET: Supplier
        public ActionResult Index()
        {
            SupplierDao bdDao = new SupplierDao();
            return View(bdDao.ToList());
        }

        // GET: Supplier/Details/5
        public ActionResult Details(long id)
        {
            SupplierDao bdDao = new SupplierDao();
            var sl = bdDao.FindByID(id);
            if (sl.Status == true) { ViewBag.Status = "Kích hoạt"; }
            else { ViewBag.Status = "Khóa"; }
            var dao = new CityDao();
            var ct = dao.FindByID(sl.CityID);
            ViewBag.CityName = ct[0].Name;
            var disDao = new DistrictDao();
            var dist = disDao.FindByID(sl.CityID, sl.DistrictID);
            ViewBag.DistName = dist.Name;
            return View(sl);
        }

        // GET: Supplier/Create
        public ActionResult Create()
        {
            SetViewBag();
            SetViewBagDistrict();
            return View();
        }

        // POST: Supplier/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Supplier collection)
        {
            try
            {


                SetViewBag(collection.CityID);
               SetViewBagDistrict(collection.DistrictID, collection.CityID);
                if (ModelState.IsValid)
                {
                    // TODO: Add insert logic here

                    SupplierDao bdDao = new SupplierDao();
                    UserLogin us = (UserLogin)Session[CommonConstant.USER_SESSION];
                    collection.SupplierID = bdDao.GenaraCode("CU", 4);
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

        // GET: Supplier/Edit/5
        public ActionResult Edit(long id)
        {
            SupplierDao bdDao = new SupplierDao();
            var sp = bdDao.FindByID(id);
            SetViewBag(sp.CityID);
            SetViewBagDistrict(sp.DistrictID, sp.CityID);
            return View(sp);
        }

        // POST: Supplier/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Supplier collection)
        {
            try
            {
                SetViewBag(collection.CityID);
                SetViewBagDistrict(collection.DistrictID, collection.CityID);
                // TODO: Add update logic here
                if (ModelState.IsValid)
                {
                    // TODO: Add insert logic here

                    SupplierDao bdDao = new SupplierDao();
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


        // POST: Supplier/Delete/5
        [HttpDelete]
        public ActionResult Delete(long id)
        {
            try
            {
                // TODO: Add delete logic here

                SupplierDao bdDao = new SupplierDao();

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
        public void SetViewBag(string selectedId = null)
        {
            var dao = new CityDao();
            ViewBag.CityID = new SelectList(dao.ToList(), "CityID", "Name", selectedId);
        }
        public void SetViewBagDistrict(string selectedId = null,string cityid =null)
        {
            var dao = new DistrictDao();
            if (cityid != null)
            {
                ViewBag.DistrictID = new SelectList(dao.FindByID(cityid), "DistrictCode", "Name", selectedId);
            }else
            ViewBag.DistrictID = new SelectList(dao.ToList(), "DistrictCode", "Name", selectedId);
        }

        [HttpPost]
        public JsonResult ChangeDistrict(string cityid)
        {

            var dao = new DistrictDao();
            var list = dao.FindByID(cityid);
            JsonResult result = new JsonResult();
            result.Data = list;
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }
    }
}
