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
    public class FeedbackController : BaseController
    {
        // GET: Feedback
        public ActionResult Index(long id)
        { 
            ////Kiểm tra quyền truy cập của user
            //UserLogin us = (UserLogin)Session[CommonConstant.USER_SESSION];
            //ProjectUserDao usDao = new ProjectUserDao();
            //List<ProjectUser> lstUP = usDao.FindByProjectID(id);
            //bool inGroup = false;
            //foreach (var u in lstUP)
            //{
            //    if (u.LoginID == us.UserID)
            //    {
            //        inGroup = true;
            //        break;
            //    }
            //}
            ////Kiểm tra quyền truy cập của lạnh đạo
            //GroupUserDao gru = new GroupUserDao();
            //Guid grid = new Guid("964D283D-BEA0-4D85-B7C0-355487A5DF0C");
            //if (gru.FiindByID(grid, us.UserID) != null)
            //{
            //    inGroup = true;
            //}
            //if (!inGroup)
            //{
            //    RedirectToAction("NotiAuthorize", "Home");

            //}
            ProjectDao bdDao = new ProjectDao();
            ViewBag.Project = bdDao.FindByID(id);
            if (ViewBag.Project.Status < 1)
            {
                SetAlert("Dự án chưa được duyệt!", Common.CommonConstant.ALERT_WARNING);
                return RedirectToAction("Details", "Project", new { id = id });
            }

            if (ViewBag.Project.Status > 2)
            {
                SetAlert("Dự án đã kết thúc!", Common.CommonConstant.ALERT_WARNING);
                return RedirectToAction("Details", "Project", new { id = id });
            }
            
            //ProcessDao prcessDao = new ProcessDao();
            //ViewBag.lstprocess = prcessDao.ToListProcessUserByProjectID(id);
            //ViewBag.lstProjectProcessMessege = prcessDao.GetListProjectProcessMessege(id).ToArray<ProjectMessage>();
            //ViewBag.Messege = ViewBag.lstProjectProcessMessege.Length.ToString();
            FeedbackDao feedDao = new FeedbackDao();
            ViewBag.Feedback = feedDao.ToListFeebBackUser(id).ToArray<FeedbackUser>();
           List<FeedbackUser> fdnul = new List<FeedbackUser>();
            foreach(FeedbackUser fb in ViewBag.Feedback)
            {
                if(fb.ChildID == null)
                {
                    fdnul.Add(fb);

                }
            }
            ViewBag.FeedbackNull = fdnul.ToArray<FeedbackUser>();
            return View();
           
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(FormCollection data)
        {
            try
            {
                UserLogin us = (UserLogin)Session[CommonConstant.USER_SESSION];
                var dao = new  FeedbackDao();
                long projectID = Convert.ToInt64(data["txtID"].ToString());
                ProcessDao prcessDao = new ProcessDao();
                ViewBag.lstprocess = prcessDao.ToListProcessUserByProjectID(projectID);
                ViewBag.lstProjectProcessMessege = prcessDao.GetListProjectProcessMessege(projectID).ToArray<ProjectMessage>();
                ViewBag.Messege = ViewBag.lstProjectProcessMessege.Length.ToString();
                Feedback objFeedback= new Feedback();
               
                objFeedback.ProjectID = projectID;
                objFeedback.Description = data["txtDescription"].ToString();
                objFeedback.CreateDate = Hepper.GetDateServer();
                objFeedback.ModifiedDate = Hepper.GetDateServer();
                objFeedback.CreateBy = us.UserName;
                objFeedback.ModifiedBy = us.UserName;
                if (dao.Insert(objFeedback) > 0)
                {
                    SetAlert("Thêm thành công", "success");
                    ViewBag.Feedback = dao.ToListFeebBackUser(projectID).ToArray<FeedbackUser>();
                    List<FeedbackUser> fdnul = new List<FeedbackUser>();
                    foreach (FeedbackUser fb in ViewBag.Feedback)
                    {
                        if (fb.ChildID == null)
                        {
                            fdnul.Add(fb);

                        }
                    }
                    ViewBag.FeedbackNull = fdnul.ToArray<FeedbackUser>();
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

        [ChildActionOnly]
        public PartialViewResult TopFeedBack()
        {
            PTTDataContext db = new PTTDataContext();
            UserLogin us = (UserLogin)Session[CommonConstant.USER_SESSION];
            string usID = us.UserID.ToString();
            string sql = "SELECT distinct fb.[ProjectID] ";
            sql += "  FROM Feedback as fb ";
            sql += " join Project as pj on fb.ProjectID =pj.ProjectID ";
            sql += " Where pj.Status = 1 and fb.CreateBy='" + us.UserName+ "' ";
            //Lấy danh sách mã các dự án của user góp ý 
            var lstPjectID = db.Database.SqlQuery <long>(sql).ToList();
            string sqlPU = "Select distinct pj.ProjectID From Project as pj ";
            sqlPU += " join ProjectUser as pru on pj.ProjectID = pru.ProjectID ";
            sqlPU += " Where pj.Status = 1 and pru.LoginID = " + us.UserID.ToString();
            //Lấy danh sách mã các dự án của user tham gia dự án
            var lstPjectIDCreate = db.Database.SqlQuery<long>(sqlPU).ToList();
            List<long> lstPjID = lstPjectIDCreate;
            //Gộp tất cả dự án liên quan của user hiện tại
            foreach(long id in lstPjectID)
            {
                if (!lstPjectIDCreate.Contains(id))
                {
                    lstPjID.Add(id);
                }
            }

            List<FeedbackUser> lst = new List<FeedbackUser>();
            List<FeedbackUser> lstNotRead = new List<FeedbackUser>();
            foreach (long prID in lstPjID)
            {
                List<FeedbackUser> lt = (from fb in db.Feedbacks
                                         join u in db.Users
                                         on fb.CreateBy equals u.UserName
                                         where fb.ProjectID == prID && fb.CreateBy != us.UserName
                                         orderby fb.FeedbackID descending
                                         select new FeedbackUser
                                         {
                                             FeedbackID = fb.FeedbackID,
                                             CreateBy = fb.CreateBy,
                                             ChildID = fb.ChildID,
                                             CreateDate = fb.CreateDate,
                                             Description = fb.Description,
                                             ProjectID = fb.ProjectID,
                                             UsersRead = fb.UsersRead,
                                             FullName = u.UserName

                                         }).ToList<FeedbackUser>();
                lst.AddRange(lt);
            }
            //Lấy danh sách các tài khoản chưa đọc tin 
            foreach (FeedbackUser fu in lst)
            {
                if (fu.UsersRead != null)
                {
                    string[] listUerID = fu.UsersRead.Split('.');
                    if (!listUerID.Contains(usID))
                    {
                        lstNotRead.Add(fu);
                    }
                }
                else
                {
                    lstNotRead.Add(fu);
                }
            }
                return PartialView(lstNotRead);
        }

        [HttpPost]
        public JsonResult CreateFeedBack(long projectID, string content, long? chilID = null)
        {
            UserLogin us = (UserLogin)Session[CommonConstant.USER_SESSION];
            Feedback objFeedback = new Feedback();
            var dao = new FeedbackDao();
            objFeedback.ProjectID = projectID;
            objFeedback.Description = content;
            objFeedback.ChildID = chilID;
            objFeedback.CreateDate = Hepper.GetDateServer();
            objFeedback.ModifiedDate = Hepper.GetDateServer();
            objFeedback.CreateBy = us.UserName;
            objFeedback.ModifiedBy = us.UserName;
            var data = dao.Insert(objFeedback);
            JsonResult result = new JsonResult();
            result.Data = data;
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }
        [HttpPost]
        public JsonResult UpdateUserRead(long feedID)
        {
            UserLogin us = (UserLogin)Session[CommonConstant.USER_SESSION];
          
            var dao = new FeedbackDao();
            long data = 0;
            Feedback objFeedback = dao.FindByID(feedID);
            string usID = us.UserID.ToString();
            string sUserID = objFeedback.UsersRead;

            string[] listUerID;
            bool kt = false;

            if (sUserID == null)
            {
                sUserID = usID;
                kt = true;
            }
            else
            {
                listUerID = objFeedback.UsersRead.Split('.');
                if (!listUerID.Contains(usID))
                {
                    sUserID += "." + usID;
                    kt = true;
                }

            }
            if (kt == true)
            {
                objFeedback.UsersRead = sUserID;
                data= dao.Update(objFeedback);
            }

            JsonResult result = new JsonResult();
            result.Data = data;
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }
    }
}