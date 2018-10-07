using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.EF;
using PTT.Common;
using PTT.Models;

namespace PTT.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Home
        PTTDataContext db = null;
       
        public ActionResult Index()
        {
            //SetAlert("Thử thông báo", "danger");
            db = new PTTDataContext();
            //ViewBag.ProjectPublic = from pr in db.Projects
            //                        join us in db.Users on pr.CreateBy equals us.UserName
            //                        orderby pr.ProjectID ascending
            //                        where pr.Status > 0 && pr.Status < 3 && pr.IsPublic == true
            //                        select new ProjectMember
            //                        {
            //                            ProjectID = pr.ProjectID,
            //                            Address = pr.Address,
            //                            CategoryID = pr.CategoryID,
            //                            CityID = pr.CityID,
            //                            Code = pr.Code,
            //                            CreateBy = pr.CreateBy,
            //                            CreateDate = pr.CreateDate,
            //                            DateLine = pr.DateLine,
            //                            DistrictID = pr.DistrictID,
            //                            FullName = us.FullName,
            //                            IsGroup = pr.IsGroup,
            //                            IsPublic = pr.IsPublic,
            //                            MetaTite = pr.MetaTite,
            //                            Name = pr.Name,
            //                            StartDate = pr.StartDate,
            //                            Status = pr.Status

            //                        };

            ViewBag.ProjectGroup = from pr in db.Projects
                                   join us in db.Users on pr.CreateBy equals us.UserName
                                   orderby pr.ProjectID ascending
                                   where pr.Status > 0 && pr.Status < 3 && (from pu in db.ProjectUsers where pu.ProjectID == pr.ProjectID select pu.LoginID).Count() > 1
                                   select new ProjectMember
                                   {
                                       ProjectID = pr.ProjectID,
                                       Address = pr.Address,
                                       CategoryID = pr.CategoryID,
                                       CityID = pr.CityID,
                                       Code = pr.Code,
                                       CreateBy = pr.CreateBy,
                                       CreateDate = pr.CreateDate,
                                       DateLine = pr.DateLine,
                                       DistrictID = pr.DistrictID,
                                       FullName = us.FullName,
                                       IsGroup = pr.IsGroup,
                                       IsPublic = pr.IsPublic,
                                       MetaTite = pr.MetaTite,
                                       Name = pr.Name,
                                       StartDate = pr.StartDate,
                                       Status = pr.Status

                                   };
            ViewBag.ProjectPrivate = from pr in db.Projects
                                     join us in db.Users on pr.CreateBy equals us.UserName
                                     orderby pr.ProjectID ascending
                                     where pr.Status > 0 && pr.Status<3 && (from pu in db.ProjectUsers where pu.ProjectID == pr.ProjectID select pu.LoginID).Count() == 1
                                     select new ProjectMember
                                     {
                                         ProjectID = pr.ProjectID,
                                         Address = pr.Address,
                                         CategoryID = pr.CategoryID,
                                         CityID = pr.CityID,
                                         Code = pr.Code,
                                         CreateBy = pr.CreateBy,
                                         CreateDate = pr.CreateDate,
                                         DateLine = pr.DateLine,
                                         DistrictID = pr.DistrictID,
                                         FullName = us.FullName,
                                         IsGroup = pr.IsGroup,
                                         IsPublic = pr.IsPublic,

                                         MetaTite = pr.MetaTite,
                                         Name = pr.Name,
                                         StartDate = pr.StartDate,
                                         Status = pr.Status,
                                         EndDate = pr.EndDate

                                     };

            return View();
        }

        public ActionResult ProjectUser()

        {

            UserLogin user = (UserLogin)Session[CommonConstant.USER_SESSION];
            db = new PTTDataContext();
            ViewBag.ProjectGroup = from pr in db.Projects
                                   
                                   orderby pr.ProjectID ascending
                                   where pr.Status == 1 && pr.CreateBy.Equals(user.UserName) && (from pu in db.ProjectUsers where pu.ProjectID==pr.ProjectID select pu.LoginID ).Count() ==1
                                   select new ProjectMember
                                   {
                                       ProjectID = pr.ProjectID,
                                       Address = pr.Address,
                                       CategoryID = pr.CategoryID,
                                       CityID = pr.CityID,
                                       Code = pr.Code,
                                       CreateBy = pr.CreateBy,
                                       CreateDate = pr.CreateDate,
                                       DateLine = pr.DateLine,
                                       DistrictID = pr.DistrictID,
                                       FullName = user.FullName,
                                       IsGroup = pr.IsGroup,
                                       IsPublic = pr.IsPublic,
                                       MetaTite = pr.MetaTite,
                                       Name = pr.Name,
                                       StartDate = pr.StartDate,
                                       Status = pr.Status

                                   };

            return View();
        }
        public ActionResult ProjectUserEnd()

        {

            UserLogin user = (UserLogin)Session[CommonConstant.USER_SESSION];
            db = new PTTDataContext();
            ViewBag.ProjectGroup = from pr in db.Projects
                                   join us in db.Users on pr.CreateBy equals us.UserName
                                   join pu in db.ProjectUsers on pr.ProjectID equals pu.ProjectID
                                   orderby pr.ProjectID ascending
                                   where pr.Status == 3 && pu.LoginID == user.UserID
                                   select new ProjectMember
                                   {
                                       ProjectID = pr.ProjectID,
                                       Address = pr.Address,
                                       CategoryID = pr.CategoryID,
                                       CityID = pr.CityID,
                                       Code = pr.Code,
                                       CreateBy = pr.CreateBy,
                                       CreateDate = pr.CreateDate,
                                       DateLine = pr.DateLine,
                                       DistrictID = pr.DistrictID,
                                       FullName = us.FullName,
                                       IsGroup = pr.IsGroup,
                                       IsPublic = pr.IsPublic,
                                       MetaTite = pr.MetaTite,
                                       Name = pr.Name,
                                       StartDate = pr.StartDate,
                                       Status = pr.Status,
                                       EndDate = pr.EndDate
                                       
                                      

                                   };

            return View();
        }
        public ActionResult ProjectUserNotPassStart()

        {

            UserLogin user = (UserLogin)Session[CommonConstant.USER_SESSION];
            db = new PTTDataContext();
            ViewBag.ProjectGroup = from pr in db.Projects
                                   join us in db.Users on pr.CreateBy equals us.UserName
                                   join pu in db.ProjectUsers on pr.ProjectID equals pu.ProjectID
                                   orderby pr.ProjectID ascending
                                   where pr.Status == 5 && pu.LoginID == user.UserID
                                   select new ProjectMember
                                   {
                                       ProjectID = pr.ProjectID,
                                       Address = pr.Address,
                                       CategoryID = pr.CategoryID,
                                       CityID = pr.CityID,
                                       Code = pr.Code,
                                       CreateBy = pr.CreateBy,
                                       CreateDate = pr.CreateDate,
                                       DateLine = pr.DateLine,
                                       DistrictID = pr.DistrictID,
                                       FullName = us.FullName,
                                       IsGroup = pr.IsGroup,
                                       IsPublic = pr.IsPublic,
                                       MetaTite = pr.MetaTite,
                                       Name = pr.Name,
                                       StartDate = pr.StartDate,
                                       Status = pr.Status

                                   };

            return View();
        }
        public ActionResult ProjectUserStart()

        {

            UserLogin user = (UserLogin)Session[CommonConstant.USER_SESSION];
            db = new PTTDataContext();
            ViewBag.ProjectGroup = from pr in db.Projects
                                   join us in db.Users on pr.CreateBy equals us.UserName
                                   join pu in db.ProjectUsers on pr.ProjectID equals pu.ProjectID
                                   orderby pr.ProjectID ascending
                                   where pr.Status == 0 && pu.LoginID == user.UserID
                                   select new ProjectMember
                                   {
                                       ProjectID = pr.ProjectID,
                                       Address = pr.Address,
                                       CategoryID = pr.CategoryID,
                                       CityID = pr.CityID,
                                       Code = pr.Code,
                                       CreateBy = pr.CreateBy,
                                       CreateDate = pr.CreateDate,
                                       DateLine = pr.DateLine,
                                       DistrictID = pr.DistrictID,
                                       FullName = us.FullName,
                                       IsGroup = pr.IsGroup,
                                       IsPublic = pr.IsPublic,
                                       MetaTite = pr.MetaTite,
                                       Name = pr.Name,
                                       
                                       StartDate = pr.StartDate,
                                       Status = pr.Status

                                   };

            return View();
        }
        public ActionResult ProjectUserStop()

        {

            UserLogin user = (UserLogin)Session[CommonConstant.USER_SESSION];
            db = new PTTDataContext();
            ViewBag.ProjectGroup = from pr in db.Projects
                                   join us in db.Users on pr.CreateBy equals us.UserName
                                   join pu in db.ProjectUsers on pr.ProjectID equals pu.ProjectID
                                   orderby pr.ProjectID ascending
                                   where pr.Status == 4 && pu.LoginID == user.UserID
                                   select new ProjectMember
                                   {
                                       ProjectID = pr.ProjectID,
                                       Address = pr.Address,
                                       CategoryID = pr.CategoryID,
                                       CityID = pr.CityID,
                                       Code = pr.Code,
                                       CreateBy = pr.CreateBy,
                                       CreateDate = pr.CreateDate,
                                       DateLine = pr.DateLine,
                                       DistrictID = pr.DistrictID,
                                       FullName = us.FullName,
                                       IsGroup = pr.IsGroup,
                                       IsPublic = pr.IsPublic,
                                       MetaTite = pr.MetaTite,
                                       Name = pr.Name,
                                       StartDate = pr.StartDate,
                                       Status = pr.Status

                                   };

            return View();
        }
        public ActionResult ProjectUserWait()

        {

            UserLogin user = (UserLogin)Session[CommonConstant.USER_SESSION];
            db = new PTTDataContext();
            ViewBag.ProjectGroup = from pr in db.Projects
                                   join us in db.Users on pr.CreateBy equals us.UserName
                                   join pu in db.ProjectUsers on pr.ProjectID equals pu.ProjectID
                                   orderby pr.ProjectID ascending
                                   where pr.Status == 2 && pu.LoginID == user.UserID
                                   select new ProjectMember
                                   {
                                       ProjectID = pr.ProjectID,
                                       Address = pr.Address,
                                       CategoryID = pr.CategoryID,
                                       CityID = pr.CityID,
                                       Code = pr.Code,
                                       CreateBy = pr.CreateBy,
                                       CreateDate = pr.CreateDate,
                                       DateLine = pr.DateLine,
                                       DistrictID = pr.DistrictID,
                                       FullName = us.FullName,
                                       IsGroup = pr.IsGroup,
                                       IsPublic = pr.IsPublic,
                                       MetaTite = pr.MetaTite,
                                       Name = pr.Name,
                                       StartDate = pr.StartDate,
                                       Status = pr.Status

                                   };

            return View();
        }
        //public ActionResult ProjectPublicStop()

        //{

        //    //UserLogin user = (UserLogin)Session[CommonConstant.USER_SESSION];
        //    db = new PTTDataContext();
        //    ViewBag.ProjectGroup = from pr in db.Projects
        //                           join us in db.Users on pr.CreateBy equals us.UserName
        //                           orderby pr.ProjectID ascending
        //                           where pr.Status == 4 && pr.IsPublic == true
        //                           select new ProjectMember
        //                           {
        //                               ProjectID = pr.ProjectID,
        //                               Address = pr.Address,
        //                               CategoryID = pr.CategoryID,
        //                               CityID = pr.CityID,
        //                               Code = pr.Code,
        //                               CreateBy = pr.CreateBy,
        //                               CreateDate = pr.CreateDate,
        //                               DateLine = pr.DateLine,
        //                               DistrictID = pr.DistrictID,
        //                               FullName = us.FullName,
        //                               IsGroup = pr.IsGroup,
        //                               IsPublic = pr.IsPublic,
        //                               MetaTite = pr.MetaTite,
        //                               Name = pr.Name,
        //                               StartDate = pr.StartDate,
        //                               Status = pr.Status

        //                           };

        //    return View();
        //}
        //public ActionResult ProjectPublicNotPassStart()

        //{

        //    //UserLogin user = (UserLogin)Session[CommonConstant.USER_SESSION];
        //    db = new PTTDataContext();
        //    ViewBag.ProjectGroup = from pr in db.Projects
        //                           join us in db.Users on pr.CreateBy equals us.UserName
        //                           orderby pr.ProjectID ascending
        //                           where pr.Status == 5 && pr.IsPublic == true
        //                           select new ProjectMember
        //                           {
        //                               ProjectID = pr.ProjectID,
        //                               Address = pr.Address,
        //                               CategoryID = pr.CategoryID,
        //                               CityID = pr.CityID,
        //                               Code = pr.Code,
        //                               CreateBy = pr.CreateBy,
        //                               CreateDate = pr.CreateDate,
        //                               DateLine = pr.DateLine,
        //                               DistrictID = pr.DistrictID,
        //                               FullName = us.FullName,
        //                               IsGroup = pr.IsGroup,
        //                               IsPublic = pr.IsPublic,
        //                               MetaTite = pr.MetaTite,
        //                               Name = pr.Name,
        //                               StartDate = pr.StartDate,
        //                               Status = pr.Status

        //                           };

        //    return View();
        //}
        //public ActionResult ProjectPublicStart()

        //{

        //    //UserLogin user = (UserLogin)Session[CommonConstant.USER_SESSION];
        //    db = new PTTDataContext();
        //    ViewBag.ProjectGroup = from pr in db.Projects
        //                           join us in db.Users on pr.CreateBy equals us.UserName
        //                           orderby pr.ProjectID ascending
        //                           where pr.Status == 1 && pr.IsPublic == true
        //                           select new ProjectMember
        //                           {
        //                               ProjectID = pr.ProjectID,
        //                               Address = pr.Address,
        //                               CategoryID = pr.CategoryID,
        //                               CityID = pr.CityID,
        //                               Code = pr.Code,
        //                               CreateBy = pr.CreateBy,
        //                               CreateDate = pr.CreateDate,
        //                               DateLine = pr.DateLine,
        //                               DistrictID = pr.DistrictID,
        //                               FullName = us.FullName,
        //                               IsGroup = pr.IsGroup,
        //                               IsPublic = pr.IsPublic,
        //                               MetaTite = pr.MetaTite,
        //                               Name = pr.Name,
        //                               StartDate = pr.StartDate,
        //                               Status = pr.Status

        //                           };

        //    return View();
        //}
        //public ActionResult ProjectPublicEnd()

        //{

        //    //UserLogin user = (UserLogin)Session[CommonConstant.USER_SESSION];
        //    db = new PTTDataContext();
        //    ViewBag.ProjectGroup = from pr in db.Projects
        //                           join us in db.Users on pr.CreateBy equals us.UserName
        //                           orderby pr.ProjectID ascending
        //                           where pr.Status == 3 && pr.IsPublic == true
        //                           select new ProjectMember
        //                           {
        //                               ProjectID = pr.ProjectID,
        //                               Address = pr.Address,
        //                               CategoryID = pr.CategoryID,
        //                               CityID = pr.CityID,
        //                               Code = pr.Code,
        //                               CreateBy = pr.CreateBy,
        //                               CreateDate = pr.CreateDate,
        //                               DateLine = pr.DateLine,
        //                               DistrictID = pr.DistrictID,
        //                               FullName = us.FullName,
        //                               IsGroup = pr.IsGroup,
        //                               IsPublic = pr.IsPublic,
        //                               MetaTite = pr.MetaTite,
        //                               Name = pr.Name,
        //                               StartDate = pr.StartDate,
        //                               Status = pr.Status,
        //                               EndDate = pr.EndDate

        //                           };

        //    return View();
        //}
        //public ActionResult ProjectPublic()

        //{

        //    //UserLogin user = (UserLogin)Session[CommonConstant.USER_SESSION];
        //    db = new PTTDataContext();
        //    ViewBag.ProjectGroup = from pr in db.Projects
        //                           join us in db.Users on pr.CreateBy equals us.UserName
        //                           orderby pr.ProjectID ascending
        //                           where pr.Status == 1 && pr.IsPublic == true
        //                           select new ProjectMember
        //                           {
        //                               ProjectID = pr.ProjectID,
        //                               Address = pr.Address,
        //                               CategoryID = pr.CategoryID,
        //                               CityID = pr.CityID,
        //                               Code = pr.Code,
        //                               CreateBy = pr.CreateBy,
        //                               CreateDate = pr.CreateDate,
        //                               DateLine = pr.DateLine,
        //                               DistrictID = pr.DistrictID,
        //                               FullName = us.FullName,
        //                               IsGroup = pr.IsGroup,
        //                               IsPublic = pr.IsPublic,
        //                               MetaTite = pr.MetaTite,
        //                               Name = pr.Name,
        //                               StartDate = pr.StartDate,
        //                               Status = pr.Status

        //                           };

        //    return View();
        //}
        //public ActionResult ProjectPublicWait()

        //{

        //    //UserLogin user = (UserLogin)Session[CommonConstant.USER_SESSION];
        //    db = new PTTDataContext();
        //    ViewBag.ProjectGroup = from pr in db.Projects
        //                           join us in db.Users on pr.CreateBy equals us.UserName
        //                           orderby pr.ProjectID ascending
        //                           where pr.Status == 2 && pr.IsPublic == true
        //                           select new ProjectMember
        //                           {
        //                               ProjectID = pr.ProjectID,
        //                               Address = pr.Address,
        //                               CategoryID = pr.CategoryID,
        //                               CityID = pr.CityID,
        //                               Code = pr.Code,
        //                               CreateBy = pr.CreateBy,
        //                               CreateDate = pr.CreateDate,
        //                               DateLine = pr.DateLine,
        //                               DistrictID = pr.DistrictID,
        //                               FullName = us.FullName,
        //                               IsGroup = pr.IsGroup,
        //                               IsPublic = pr.IsPublic,
        //                               MetaTite = pr.MetaTite,
        //                               Name = pr.Name,
        //                               StartDate = pr.StartDate,
        //                               Status = pr.Status

        //                           };

        //    return View();
        //}
        public ActionResult ProjectGroup()

        {

            UserLogin user = (UserLogin)Session[CommonConstant.USER_SESSION];
            db = new PTTDataContext();
            ViewBag.ProjectGroup = from pr in db.Projects
                                  
                                   join pu in db.ProjectUsers on pr.ProjectID equals pu.ProjectID

                                   orderby pr.ProjectID ascending
                                   where pr.Status == 1 && pu.LoginID == user.UserID
                                   select new ProjectMember
                                   {
                                       ProjectID = pr.ProjectID,
                                       Address = pr.Address,
                                       CategoryID = pr.CategoryID,
                                       CityID = pr.CityID,
                                       Code = pr.Code,
                                       CreateBy = pr.CreateBy,
                                       CreateDate = pr.CreateDate,
                                       DateLine = pr.DateLine,
                                       DistrictID = pr.DistrictID,
                                       FullName = user.FullName,
                                       IsGroup = pr.IsGroup,
                                       IsPublic = pr.IsPublic,
                                       MetaTite = pr.MetaTite,
                                       Name = pr.Name,
                                       StartDate = pr.StartDate,
                                       Status = pr.Status

                                   };

            return View();
        }
        public ActionResult NotiAuthorize()
        {
            SetAlert("Bạn không có quyền truy cập", "danger");
            return View();
        }
    }
}