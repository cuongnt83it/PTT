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
    public class GrantPermissionController : BaseController
    {
        // GET: GrantPermission
        public ActionResult Index(Guid id)
        {
            PTTDataContext db = new PTTDataContext();
            //Lấy tất cả các nghiệp vụ (Controler)
            var listControl = db.Businesses.AsEnumerable();
            List<SelectListItem> items = new List<SelectListItem>();

            foreach (var item in listControl)
            {
                items.Add(new SelectListItem() { Text = item.BusinessName, Value = item.BusinessID });
            }
            ViewBag.items = items;

            //Lấy danh sách quyền đã được cấp cho group
            var listgranted = from g in db.GrantPermissions
                              join p in db.Permissions on g.PermissionID equals p.PermissionID
                              where g.GroupID == id
                              select new SelectListItem() { Text = p.Description, Value = p.PermissionID.ToString() };

            ViewBag.listgranted = listgranted;
            Session["groupgrant"] = id;
            var groupgrant = db.Groups.Find(id);
            ViewBag.groupgrant = groupgrant.GroupName;
            return View();
        }
        //Lấy danh sách quyền đang được cấp cho group
        public JsonResult getPermissions(string id, Guid grouppid)
        {
            PTTDataContext db = new PTTDataContext();
            //Lấp permission của group và của bussiness
            var listGranted = (from g in db.GrantPermissions
                               join p in db.Permissions 
                               on g.PermissionID equals p.PermissionID
                               where g.GroupID == grouppid && p.BusinessID == id 
                            select new PermissionAction { PermissionID = p.PermissionID, Name = p.Name, Description = p.Description, isGranted = true });
            ////Lấp tất cả các permision của bussiness hiện tại
            var listPermission = (from p in db.Permissions
                                  where p.BusinessID == id
                                  select new PermissionAction { PermissionID = p.PermissionID, Name = p.Name, Description = p.Description, isGranted = false });
            //Lấy tất cẩ iD của permistion được cấp quyền cho nhóm ở trên
            var listPermissionID = listGranted.Select(p => p.PermissionID);

            //So sánh kiểm tra xem idpermission nào chưa có trong listGranted thì thêm vào
       var   listGt = listGranted.ToList<PermissionAction>();
            foreach (var item in listPermission)
            {
                if (!listPermissionID.Contains(item.PermissionID))
                {
                     listGt.Add(item);
                }

            }
            // return Json(new { data = listGranted.OrderBy(x => x.Description), status = true }, JsonRequestBehavior.AllowGet);
            return Json(listGt.OrderBy(x => x.Description), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public string updatePermissions(int id, Guid grouppid)
        {

            string msg = "";
            PTTDataContext db = new PTTDataContext();
            var grant = db.GrantPermissions.Where(a => a.GroupID == grouppid && a.PermissionID == id).SingleOrDefault<GrantPermission>();
            if (grant == null)
            {
                GrantPermission g = new GrantPermission();
                g.GroupID = grouppid;
                g.PermissionID = id;
                db.GrantPermissions.Add(g);
                msg = "<div class='alert alert-success'>Cấp quyền thành công!</div>";
            }
            else
            {
                db.GrantPermissions.Remove(grant);
                msg = "<div class='alert alert-danger'>Hủy quyền thành công!</div>";
            }
            db.SaveChanges();
          
            return msg;
           
        }
    }

}