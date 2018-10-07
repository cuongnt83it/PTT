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
                return RedirectToAction("Index", "Home", null);
            }

            if (ViewBag.Project.Status > 2)
            {
                SetAlert("Dự án đã kết thúc!", Common.CommonConstant.ALERT_WARNING);
                return RedirectToAction("Index", "Home", null);
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
            List<FeedbackUser> lst = (from fb in db.Feedbacks
                                   join p in db.Projects
                                   on fb.ProjectID equals p.ProjectID
                                   where p.Status == 1 && (fb.CreateBy.Contains(us.UserName))
                                   orderby fb.FeedbackID descending
                                   select new FeedbackUser
                                   {
                                       FeedbackID = fb.FeedbackID,
                                       CreateBy=  fb.CreateBy,
                                       ChildID=  fb.ChildID,
                                       CreateDate= fb.CreateDate,
                                       Description=fb.Description,
                                       ProjectID= fb.ProjectID,
                                       UsersRead=  fb.UsersRead,
                                       FullName= us.UserName
                                      
                                   }).ToList<FeedbackUser>();

            List<FeedbackUser> lsttem = new List<FeedbackUser>();
            lsttem.AddRange(lst);
            foreach (var lt in lst)
            {
                List<FeedbackUser> lstFchil = (from fb in db.Feedbacks
                                               join p in db.Projects
                                               on fb.ProjectID equals p.ProjectID
                                               where p.Status == 1 && (fb.ChildID == lt.FeedbackID)
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
                                                   FullName = fb.CreateBy


                                          }).ToList<FeedbackUser>();
                lsttem.AddRange(lstFchil);

            }
           // var lst = new ContentDao().ListHot();

            return PartialView(lsttem);
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
    }
}