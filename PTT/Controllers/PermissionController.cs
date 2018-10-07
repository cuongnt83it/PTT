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
    public class PermissionController : BaseController
    {

        // GET: Admin/Permistion
        public ActionResult Index(string id)
        {
            PTTDataContext db = new PTTDataContext();
            var lstPB = from p in db.Permissions
                        join b in db.Businesses
                        on p.BusinessID equals b.BusinessID
                        where p.BusinessID ==id
                        select new PermissionBusiness
                        {
                            BusinessID = b.BusinessID,
                            PermissionID = p.PermissionID,
                            BusinessName = b.BusinessName,
                            Name = p.Name,
                            Description = p.Description

                        };
            ViewBag.PB = lstPB;
            return View();
        }
        // GET: Admin/Permistion/Edit/5
        public ActionResult Edit(int id)
        {
            PermissionDao dao = new PermissionDao();
            var pm = dao.GetPermistionByID(id);
            ViewBag.BusinessID = pm.BusinessID;
            return View(pm);
        }

        // POST: Admin/Permistion/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Permission mode)
        {
            string id = mode.BusinessID;
            try
            {
                if (ModelState.IsValid)
                {
                    PermissionDao dao = new PermissionDao();
                    
                    
                    dao.Update(mode);
                    if (dao.Update(mode)> 0)
                    {
                        SetAlert("Cập nhật thành công", "success");
                        return RedirectToAction("Index", "Permission", new { id = id });
                    }
                    else
                    {
                        SetAlert("Không thành công", "danger");
                    }
                    return RedirectToAction("Index", "Permission", new { id = id });
                }
                else
                {
                    SetAlert("Không thành công", "danger");
                    return RedirectToAction("Index", "Permission", new { id = id});
                }

            }
            catch
            {
                return RedirectToAction("Index", "Permission", new { id = mode.BusinessID });
            }
        }
        // POST: Group/Delete/5
        [HttpDelete]
        public ActionResult Delete(long id)
        {
            try
            {
                // TODO: Add delete logic here

                PermissionDao bdDao = new PermissionDao();

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