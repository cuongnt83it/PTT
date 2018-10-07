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
    public class CompetitorController : BaseController
    {
        // GET: Competitor
        public ActionResult Index()
        {
           CompetitorDao bdDao = new CompetitorDao();
            return View(bdDao.ToList());
        }

        // GET: Competitor/Details/5
        public ActionResult Details(long id)
        {
            CompetitorDao bdDao = new CompetitorDao();
            return View(bdDao.FindByID(id));
        }

        // GET: Competitor/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Competitor/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Competitor collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // TODO: Add insert logic here

                    CompetitorDao bdDao = new CompetitorDao();
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

        // GET: Competitor/Edit/5
        public ActionResult Edit(long id)
        {
            CompetitorDao bdDao = new CompetitorDao();
            return View(bdDao.FindByID(id));
        }

        // POST: Competitor/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Competitor collection)
        {
            try
            {
                // TODO: Add update logic here
                if (ModelState.IsValid)
                {
                    // TODO: Add insert logic here

                    CompetitorDao bdDao = new CompetitorDao();
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

        // GET: Competitor/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Competitor/Delete/5
      
        [HttpDelete]
        public ActionResult Delete(long id)
        {
            try
            {  // TODO: Add delete logic here

                CompetitorDao bdDao = new CompetitorDao();
                ProjectDao prDao = new ProjectDao();
                if (prDao.FindByCompetitor(id).Count > 0)
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
