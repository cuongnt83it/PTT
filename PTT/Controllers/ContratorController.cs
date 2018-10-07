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
    public class ContratorController : BaseController
    {
        // GET: Contrator
        public ActionResult Index()
        {
            ContratorDao bdDao = new ContratorDao();
            return View(bdDao.ToList());
            
        }

        // GET: Contrator/Details/5
        public ActionResult Details(long id)
        {
            ContratorDao bdDao = new ContratorDao();
            return View(bdDao.FindByID(id));
        }

        // GET: Contrator/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Contrator/Create
        // POST: Builders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Contrator collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // TODO: Add insert logic here

                    ContratorDao bdDao = new ContratorDao();
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

        // GET: Contrator/Edit/5
        public ActionResult Edit(long id)
        {
            ContratorDao bdDao = new ContratorDao();
            return View(bdDao.FindByID(id));
        }

        // POST: Contrator/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Contrator collection)
        {
            try
            {
                // TODO: Add update logic here
                if (ModelState.IsValid)
                {
                    // TODO: Add insert logic here

                    ContratorDao bdDao = new ContratorDao();
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



        // POST: Contrator/Delete/5
        [HttpDelete]
        public ActionResult Delete(long id)
        {
            try
            {
                // TODO: Add delete logic here

                ContratorDao bdDao = new ContratorDao();
                ProjectDao prDao = new ProjectDao();
                if (prDao.FindByContrator(id).Count > 0)
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
                return View();
            }
        }
    }
}
