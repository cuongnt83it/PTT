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
    public class ProductController : BaseController
    {
        // GET: Product
        public ActionResult Index()
        {
            ProductDao bdDao = new ProductDao();
            return View(bdDao.ToList());
        }

        // GET: Product/Details/5
        public ActionResult Details(long id)
        {
            ProductDao bdDao = new ProductDao();
            var sl = bdDao.FindByID(id);
            if (sl.Status == true) { ViewBag.Status = "Kích hoạt"; }
            else { ViewBag.Status = "Khóa"; }
            return View(sl);
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Product/Create
        [HttpPost]
       [ValidateAntiForgeryToken]
        public ActionResult Create(Product collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // TODO: Add insert logic here

                    ProductDao bdDao = new ProductDao();
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

        // GET: Product/Edit/5
        public ActionResult Edit(long id)
        {

            ProductDao bdDao = new ProductDao();
            var sl = bdDao.FindByID(id);
            //if (sl.Status == true) { ViewBag.Status = "Kích hoạt"; }
            //else { ViewBag.Status = "Khóa"; }
            return View(sl);
        }

        // POST: Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product collection)
        {
            try
            { // TODO: Add update logic here
                if (ModelState.IsValid)
                {
                    // TODO: Add insert logic here
                    ProductDao bdDao = new ProductDao();
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



        // POST: Product/Delete/5
        [HttpDelete]
        public ActionResult Delete(long id)
        {
            try
            {
                // TODO: Add delete logic here

                ProductDao bdDao = new ProductDao();

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
