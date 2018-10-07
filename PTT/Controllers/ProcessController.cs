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
    public class ProcessController : BaseController
    {
        // GET: Process
        public ActionResult Index(long id)
        {
            //Kiểm tra quyền truy cập của user
            UserLogin us = (UserLogin)Session[CommonConstant.USER_SESSION];
            ProjectUserDao usDao = new ProjectUserDao();
            List<ProjectUser> lstUP = usDao.FindByProjectID(id);
            bool inGroup = false;
            foreach(var u in lstUP)
            {
                if (u.LoginID == us.UserID)
                {
                    inGroup = true;
                    break;
                }
            }
            //Kiểm tra quyền truy cập của lạnh đạo
            GroupUserDao gru = new GroupUserDao();
            Guid grid = new Guid("964D283D-BEA0-4D85-B7C0-355487A5DF0C");
            if (gru.FiindByID(grid, us.UserID )!= null){
                inGroup = true;
            }
            if (!inGroup)
            {
                RedirectToAction("NotiAuthorize", "Home");
                 
            }
            ProjectDao bdDao = new ProjectDao();
            ViewBag.Project = bdDao.FindByID(id);
            if (ViewBag.Project.Status <1)
            {
                SetAlert("Dự án chưa được duyệt!", Common.CommonConstant.ALERT_WARNING);
                return RedirectToAction("Index", "Home", null);
            }
           
            if (ViewBag.Project.Status > 2)
            {
                SetAlert("Dự án đã kết thúc!", Common.CommonConstant.ALERT_WARNING);
                return RedirectToAction("Index", "Home", null);
            }
            ProcessDao prcessDao = new ProcessDao();
            ViewBag.lstprocess = prcessDao.ToListProcessUserByProjectID(id);
            ViewBag.lstProjectProcessMessege = prcessDao.GetListProjectProcessMessege(id).ToArray<ProjectMessage>();
            ViewBag.Messege = ViewBag.lstProjectProcessMessege.Length.ToString();
            FeedbackDao feedDao = new FeedbackDao();
            ViewBag.Feedback = feedDao.ToListByProjectID(id).Count.ToString();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(FormCollection data) {
            try { 
            UserLogin us = (UserLogin)Session[CommonConstant.USER_SESSION];
            var dao = new ProcessDao();
            ViewBag.lstProjectProcessMessege = dao.GetListProjectProcessMessege(Convert.ToInt64(data["txtID"].ToString()));
            ViewBag.lstprocess = dao.ToListProcessUserByProjectID(Convert.ToInt64(data["txtID"].ToString()));
            Process objProcess = new Process();
            objProcess.Name = data["txtName"].ToString();
            objProcess.ProjectID = Convert.ToInt64( data["txtID"].ToString());
            objProcess.Description = data["txtDescription"].ToString();
            objProcess.CreateDate = Hepper.GetDateServer();
            objProcess.ModifiedDate = Hepper.GetDateServer();
            objProcess.CreateBy = us.UserName;
            objProcess.ModifiedBy = us.UserName;
            if(dao.Insert(objProcess)>0)
            {
                SetAlert("Thêm thành công", "success");
                    ViewBag.lstprocess = dao.ToListProcessUserByProjectID(Convert.ToInt64(data["txtID"].ToString()));
                    ViewBag.lstProjectProcessMessege = dao.GetListProjectProcessMessege(Convert.ToInt64(data["txtID"].ToString()));
                return RedirectToAction("Index");
               
            }
            }
            catch
            {
                SetAlert("Không thêm được", "danger");
                return View();
            }
            return View();
        }
        [HttpPost]
        public JsonResult CreateProcess(long processID, string content,long? chilID=null )
        {
            UserLogin us = (UserLogin)Session[CommonConstant.USER_SESSION];
            MessegeDao msgDao = new MessegeDao();

            Messege msg = new Messege();
            msg.ChildID = chilID;
            msg.CreateDate = Hepper.GetDateServer();
            msg.ModifiedDate = Hepper.GetDateServer();
            msg.CreateBy = us.UserName;
            msg.ModifiedBy = us.UserName;
            msg.ProcessID = processID;
            msg.Description = content;
            //var list = dao.GetListProjectProcessMessege(projectID);
            var data = msgDao.Insert(msg);
            JsonResult result = new JsonResult();
            result.Data = data;
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }
    }
}