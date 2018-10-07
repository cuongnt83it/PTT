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
    public class FeedbackInforController : BaseController
    {
        // GET: FeedbackInfor
        public ActionResult Index(long id)
        {
            InformationDao bdDao = new InformationDao();
            ViewBag.Project = bdDao.FindByID(id);
            FeedbackInforDao feedDao = new FeedbackInforDao();
            ViewBag.Feedback = feedDao.ToListFeebBackUser(id).ToArray<FeedbacInfokUser>();
            List<FeedbacInfokUser> fdnul = new List<FeedbacInfokUser>();
            foreach (FeedbacInfokUser fb in ViewBag.Feedback)
            {
                if (fb.ChildID == null)
                {
                    fdnul.Add(fb);

                }
            }
            ViewBag.FeedbackNull = fdnul.ToArray<FeedbacInfokUser>();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(FormCollection data)
        {
            try
            {
                UserLogin us = (UserLogin)Session[CommonConstant.USER_SESSION];
                var dao = new FeedbackInforDao();
                long projectID = Convert.ToInt64(data["txtID"].ToString());
               
                FeedbackInfor objFeedback = new FeedbackInfor();

                objFeedback.InformationID = projectID;
                objFeedback.Description = data["txtDescription"].ToString();
                objFeedback.CreateDate = Hepper.GetDateServer();
                objFeedback.ModifiedDate = Hepper.GetDateServer();
                objFeedback.CreateBy = us.UserName;
                objFeedback.ModifiedBy = us.UserName;
                if (dao.Insert(objFeedback) > 0)
                {
                    SetAlert("Thêm thành công", "success");
                    ViewBag.Feedback = dao.ToListFeebBackUser(projectID).ToArray<FeedbacInfokUser>();
                    List<FeedbacInfokUser> fdnul = new List<FeedbacInfokUser>();
                    foreach (FeedbacInfokUser fb in ViewBag.Feedback)
                    {
                        if (fb.ChildID == null)
                        {
                            fdnul.Add(fb);

                        }
                    }
                    ViewBag.FeedbackNull = fdnul.ToArray<FeedbacInfokUser>();
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
            List<FeedbacInfokUser> lst = (from fb in db.FeedbackInfors
                                      join p in db.Information
                                      on fb.InformationID equals p.InformationID
                                      where p.Status == 1 && (fb.CreateBy.Contains(us.UserName))
                                      orderby fb.FeedbackID descending
                                      select new FeedbacInfokUser
                                      {
                                          FeedbackID = fb.FeedbackID,
                                          CreateBy = fb.CreateBy,
                                          ChildID = fb.ChildID,
                                          CreateDate = fb.CreateDate,
                                          Description = fb.Description,
                                          InformationID = fb.InformationID,
                                          UsersRead = fb.UsersRead,
                                          FullName = us.UserName

                                      }).ToList<FeedbacInfokUser>();

            List<FeedbacInfokUser> lsttem = new List<FeedbacInfokUser>();
            lsttem.AddRange(lst);
            foreach (var lt in lst)
            {
                List<FeedbacInfokUser> lstFchil = (from fb in db.FeedbackInfors
                                               join p in db.Information
                                               on fb.InformationID equals p.InformationID
                                               where p.Status == 1 && (fb.ChildID == lt.FeedbackID)
                                               orderby fb.FeedbackID descending
                                               select new FeedbacInfokUser
                                               {
                                                   FeedbackID = fb.FeedbackID,
                                                   CreateBy = fb.CreateBy,
                                                   ChildID = fb.ChildID,
                                                   CreateDate = fb.CreateDate,
                                                   Description = fb.Description,
                                                   InformationID = fb.InformationID,
                                                   UsersRead = fb.UsersRead,
                                                   FullName = fb.CreateBy


                                               }).ToList<FeedbacInfokUser>();
                lsttem.AddRange(lstFchil);

            }
            // var lst = new ContentDao().ListHot();

            return PartialView(lsttem);
        }
        [HttpPost]
        public JsonResult CreateFeedBack(long inforID, string content, long? chilID = null)
        {
            UserLogin us = (UserLogin)Session[CommonConstant.USER_SESSION];
            FeedbackInfor objFeedback = new FeedbackInfor();
            var dao = new FeedbackInforDao();
            objFeedback.InformationID = inforID;
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