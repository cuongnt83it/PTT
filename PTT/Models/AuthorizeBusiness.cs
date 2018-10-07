using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model.EF;
using Model.DAO;
using PTT.Common;
using PTT.Models;
using System.Web.Mvc;
using System.Web.Routing;

namespace PTT.Models
{
    public class AuthorizeBusiness:ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //base.OnActionExecuting(filterContext);
            UserDao usDao = new UserDao();
            if (HttpContext.Current.Session[CommonConstant.USER_SESSION] == null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { Controller = "Login", Action = "Index" }));
                return;
            }
            var usl = (UserLogin)HttpContext.Current.Session[CommonConstant.USER_SESSION];
            //Lấy action name hiện tại
            string actionname = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName + "Controller-" + filterContext.ActionDescriptor.ActionName;
            //Nếu user thuộc nhóm Admin thì được vào không cần kiểm tra
            if (usDao.isAdmin(usl.UserID)){
                return;
            }
            //Lấy danh sách quyền của uer
            List<string> listPermistion = usDao.HasPermistionUser(usl.UserID);
            if (!listPermistion.Contains(actionname))
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { Controller = "Home", Action = "NotiAuthorize" }));
                return;
            }

        }
    }
}