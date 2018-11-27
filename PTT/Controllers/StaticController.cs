using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.EF;
using PTT.Common;
using PTT.Models;
using Model.DAO;


namespace PTT.Controllers
{
    [AuthorizeBusiness]
    public class StaticController : BaseController
    {
        // GET: Static
        public ActionResult Project()
        {
            PTTDataContext db = new PTTDataContext();
            SetCatagoryBag();

            SetStatusBag();
            ViewBag.Project = null;
            ViewBag.sdate = Hepper.GetDateServer().ToString("MM/dd/yyyy");
            ViewBag.edate = Hepper.GetDateServer().ToString("MM/dd/yyyy");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Project(FormCollection data)
        {
            PTTDataContext db = new PTTDataContext();
            long iStatus = Convert.ToInt32(data["drlStatus"].ToString());
            long iCategory = Convert.ToInt32(data["drlCategory"].ToString());
            DateTime sDate = Convert.ToDateTime(data["dtStart"].ToString());
            ViewBag.sdate = sDate.ToString("MM/dd/yyyy");
            
            DateTime eDate = Convert.ToDateTime(data["dtEnd"].ToString());
            ViewBag.edate = eDate.ToString("MM/dd/yyyy");
            SetStatusBag(iStatus);
            SetCatagoryBag(iCategory);
            switch (iStatus) {
                case -1:
                    if (iCategory == 0)
                    {
                        ViewBag.Project = db.V_Project_Builer_Contrator.Where(a=>  ((a.StartDate>=sDate && a.StartDate<=eDate) || (a.StartDate >= sDate && a.EndDate >=eDate)));

                    }
                    else {
                        ViewBag.Project = db.V_Project_Builer_Contrator.Where(a => a.CategoryID == iCategory && ((a.StartDate >= sDate && a.StartDate <= eDate) || (a.StartDate >= sDate && a.EndDate >= eDate))).ToList();
                    }
                   
                    break;
                  
                case 3:
                    if (iCategory == 0)
                    {
                        ViewBag.Project = db.V_Project_Builer_Contrator.Where(a => a.Status == 3 && ((a.StartDate >= sDate && a.StartDate <= eDate) || (a.StartDate >= sDate && a.EndDate >= eDate))).ToList();

                    }
                    else
                    {
                        ViewBag.Project = db.V_Project_Builer_Contrator.Where(a => a.Status == 3 && a.CategoryID == iCategory && ((a.StartDate >= sDate && a.StartDate <= eDate) || (a.StartDate >= sDate && a.EndDate >= eDate))).ToList();
                       
                    }
                   
                    break;
                case 4:
                    if (iCategory == 0)
                    {
                        ViewBag.Project = db.V_Project_Builer_Contrator.Where(a => a.Status == 4&&((a.StartDate >= sDate && a.StartDate <= eDate) || (a.StartDate >= sDate && a.EndDate >= eDate))).ToList();

                    }
                    else
                    {
                        ViewBag.Project = db.V_Project_Builer_Contrator.Where(a => a.Status == 4 && a.CategoryID == iCategory && ((a.StartDate >= sDate && a.StartDate <= eDate) || (a.StartDate >= sDate && a.EndDate >= eDate))).ToList();

                    }
                   
                    break;

                default:
                    ViewBag.Project = db.V_Project_Builer_Contrator.ToList();
                    break;

            }

        
            ViewBag.Member = db.V_Project_Users.Where(a=>((a.StartDate >= sDate && a.StartDate <= eDate) || (a.StartDate >= sDate && a.EndDate >= eDate))).ToList();

            string sqlSupplier = string.Format("Select * From V_Project_Supplier Where (StartDate >= '{0}' AND StartDate <= '{1}' ) OR (StartDate >= '{2}' AND EndDate>='{3}')", sDate, eDate, sDate, eDate);
            ViewBag.Supplier = db.Database.SqlQuery<V_Project_Supplier>(sqlSupplier).ToList();

            string sqlBuiler = string.Format("Select * From V_Project_Builder Where (StartDate >= '{0}' AND StartDate <= '{1}' ) OR (StartDate >= '{2}' AND EndDate>='{3}')", sDate, eDate, sDate, eDate);
            ViewBag.Builer = db.Database.SqlQuery<V_Project_Builder>(sqlBuiler).ToList();
            string sqlContrator = string.Format("Select * From V_Project_Contrator Where (StartDate >= '{0}' AND StartDate <= '{1}' ) OR (StartDate >= '{2}' AND EndDate>='{3}')", sDate, eDate ,sDate, eDate);
            ViewBag.Contrator = db.Database.SqlQuery<V_Project_Contrator>(sqlContrator).ToList();
            ViewBag.Product = db.Products.ToList();
            ViewBag.ProductProject = db.V_Project_Products.Where(a => ((a.StartDate >= sDate && a.StartDate <= eDate) || (a.StartDate >= sDate && a.EndDate >= eDate))).ToList();
            return View();
        }
        public ActionResult Users()
        {
            PTTDataContext db = new PTTDataContext();
            SetCatagoryBag();
            SetUserBag();
            SetStatusBag();
            ViewBag.Project = null;
            ViewBag.sdate = Hepper.GetDateServer().ToString("MM/dd/yyyy");
            ViewBag.edate = Hepper.GetDateServer().ToString("MM/dd/yyyy");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Users(FormCollection data)
        {
            PTTDataContext db = new PTTDataContext();
            long iStatus = Convert.ToInt32(data["drlStatus"].ToString());
            long iCategory = Convert.ToInt32(data["drlCategory"].ToString());
            long userID= Convert.ToInt32(data["drlUser"].ToString());
            DateTime sDate = Convert.ToDateTime(data["dtStart"].ToString());
            ViewBag.sdate = sDate.ToString("MM/dd/yyyy");
            SetUserBag(userID);
            DateTime eDate = Convert.ToDateTime(data["dtEnd"].ToString());
            ViewBag.edate = eDate.ToString("MM/dd/yyyy");
            SetStatusBag(iStatus);
            SetCatagoryBag(iCategory);
            switch (iStatus)
            {
                case -1:
                    if (iCategory == 0)
                    {
                        ViewBag.Member = db.V_Project_Users.Where(a => a.LoginID==userID&& ((a.StartDate >= sDate && a.StartDate <= eDate) || (a.StartDate >= sDate && a.EndDate >= eDate)));

                    }
                    else
                    {
                        ViewBag.Member = db.V_Project_Users.Where(a => a.LoginID == userID && a.CategoryID == iCategory && ((a.StartDate >= sDate && a.StartDate <= eDate) || (a.StartDate >= sDate && a.EndDate >= eDate))).ToList();
                    }

                    break;

                case 3:
                    if (iCategory == 0)
                    {
                        ViewBag.Member = db.V_Project_Users.Where(a => a.LoginID == userID && a.Status == 3 && ((a.StartDate >= sDate && a.StartDate <= eDate) || (a.StartDate >= sDate && a.EndDate >= eDate))).ToList();

                    }
                    else
                    {
                        ViewBag.Member = db.V_Project_Users.Where(a => a.LoginID == userID && a.Status == 3 && a.CategoryID == iCategory && ((a.StartDate >= sDate && a.StartDate <= eDate) || (a.StartDate >= sDate && a.EndDate >= eDate))).ToList();

                    }

                    break;
                case 4:
                    if (iCategory == 0)
                    {
                        ViewBag.Member = db.V_Project_Users.Where(a => a.LoginID == userID && a.Status == 4 && ((a.StartDate >= sDate && a.StartDate <= eDate) || (a.StartDate >= sDate && a.EndDate >= eDate))).ToList();

                    }
                    else
                    {
                        ViewBag.Member = db.V_Project_Users.Where(a => a.LoginID == userID && a.Status == 4 && a.CategoryID == iCategory && ((a.StartDate >= sDate && a.StartDate <= eDate) || (a.StartDate >= sDate && a.EndDate >= eDate))).ToList();

                    }

                    break;

                default:
                    ViewBag.Member = db.V_Project_Users.ToList();
                    break;

            }
            ViewBag.Project = db.V_Project_Builer_Contrator.Where(a => ((a.StartDate >= sDate && a.StartDate <= eDate) || (a.StartDate >= sDate && a.EndDate >= eDate))).ToList();
            string sqlSupplier = string.Format("Select * From V_Project_Supplier Where (StartDate >= '{0}' AND StartDate <= '{1}' ) OR (StartDate >= '{2}' AND EndDate>='{3}')", sDate, eDate, sDate, eDate);
            ViewBag.Supplier = db.Database.SqlQuery<V_Project_Supplier>(sqlSupplier).ToList();

            string sqlBuiler = string.Format("Select * From V_Project_Builder Where (StartDate >= '{0}' AND StartDate <= '{1}' ) OR (StartDate >= '{2}' AND EndDate>='{3}')", sDate, eDate, sDate, eDate);
            ViewBag.Builer = db.Database.SqlQuery<V_Project_Builder>(sqlBuiler).ToList();
            string sqlContrator = string.Format("Select * From V_Project_Contrator Where (StartDate >= '{0}' AND StartDate <= '{1}' ) OR (StartDate >= '{2}' AND EndDate>='{3}')", sDate, eDate, sDate, eDate);
            ViewBag.Contrator = db.Database.SqlQuery<V_Project_Contrator>(sqlContrator).ToList();
            ViewBag.Product = db.Products.ToList();
            ViewBag.ProductProject = db.V_Project_Products.Where(a => ((a.StartDate >= sDate && a.StartDate <= eDate) || (a.StartDate >= sDate && a.EndDate >= eDate))).ToList();
            return View();
        }
        public ActionResult Supplier()
        {
            PTTDataContext db = new PTTDataContext();
            SetCatagoryBag();
            SetSupplierBag();
            SetStatusBag();
            ViewBag.Project = null;
            ViewBag.sdate = Hepper.GetDateServer().ToString("MM/dd/yyyy");
            ViewBag.edate = Hepper.GetDateServer().ToString("MM/dd/yyyy");
            return View();
           
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Supplier(FormCollection data)
        {
            PTTDataContext db = new PTTDataContext();
            long iStatus = Convert.ToInt32(data["drlStatus"].ToString());
            long iCategory = Convert.ToInt32(data["drlCategory"].ToString());
            long supplierID = Convert.ToInt32(data["drlSupplier"].ToString());
            DateTime sDate = Convert.ToDateTime(data["dtStart"].ToString());
            ViewBag.sdate = sDate.ToString("MM/dd/yyyy");
            SetSupplierBag(supplierID);
            DateTime eDate = Convert.ToDateTime(data["dtEnd"].ToString());
            ViewBag.edate = eDate.ToString("MM/dd/yyyy");
            SetStatusBag(iStatus);
            SetCatagoryBag(iCategory);
            switch (iStatus)
            {
                case -1:
                    if (iCategory == 0)
                    {
                        ViewBag.Supplier = db.V_Project_Supplier.Where(a => a.SupplierID == supplierID && ((a.StartDate >= sDate && a.StartDate <= eDate) || (a.StartDate >= sDate && a.EndDate >= eDate)));

                    }
                    else
                    {
                        ViewBag.Supplier = db.V_Project_Supplier.Where(a => a.SupplierID == supplierID && a.CategoryID == iCategory && ((a.StartDate >= sDate && a.StartDate <= eDate) || (a.StartDate >= sDate && a.EndDate >= eDate))).ToList();
                    }

                    break;

                case 3:
                    if (iCategory == 0)
                    {
                        ViewBag.Supplier = db.V_Project_Supplier.Where(a => a.SupplierID == supplierID && a.Status == 3 && ((a.StartDate >= sDate && a.StartDate <= eDate) || (a.StartDate >= sDate && a.EndDate >= eDate))).ToList();

                    }
                    else
                    {
                        ViewBag.Supplier = db.V_Project_Supplier.Where(a => a.SupplierID == supplierID && a.Status == 3 && a.CategoryID == iCategory && ((a.StartDate >= sDate && a.StartDate <= eDate) || (a.StartDate >= sDate && a.EndDate >= eDate))).ToList();

                    }

                    break;
                case 4:
                    if (iCategory == 0)
                    {
                        ViewBag.Supplier = db.V_Project_Supplier.Where(a => a.SupplierID == supplierID && a.Status == 4 && ((a.StartDate >= sDate && a.StartDate <= eDate) || (a.StartDate >= sDate && a.EndDate >= eDate))).ToList();

                    }
                    else
                    {
                        ViewBag.Supplier = db.V_Project_Supplier.Where(a => a.SupplierID == supplierID && a.Status == 4 && a.CategoryID == iCategory && ((a.StartDate >= sDate && a.StartDate <= eDate) || (a.StartDate >= sDate && a.EndDate >= eDate))).ToList();

                    }

                    break;

                default:
                    ViewBag.Supplier = db.V_Project_Supplier.ToList();
                    break;

            }
            ViewBag.Member = db.V_Project_Users.Where(a => ((a.StartDate >= sDate && a.StartDate <= eDate) || (a.StartDate >= sDate && a.EndDate >= eDate))).ToList();
            ViewBag.Project = db.V_Project_Builer_Contrator.Where(a => ((a.StartDate >= sDate && a.StartDate <= eDate) || (a.StartDate >= sDate && a.EndDate >= eDate))).ToList();
            //string sqlSupplier = string.Format("Select * From V_Project_Supplier Where (StartDate >= '{0}' AND StartDate <= '{1}' ) OR (StartDate >= '{2}' AND EndDate>='{3}')", sDate, eDate, sDate, eDate);
            //ViewBag.Supplier = db.Database.SqlQuery<V_Project_Supplier>(sqlSupplier).ToList();

            string sqlBuiler = string.Format("Select * From V_Project_Builder Where (StartDate >= '{0}' AND StartDate <= '{1}' ) OR (StartDate >= '{2}' AND EndDate>='{3}')", sDate, eDate, sDate, eDate);
            ViewBag.Builer = db.Database.SqlQuery<V_Project_Builder>(sqlBuiler).ToList();
            string sqlContrator = string.Format("Select * From V_Project_Contrator Where (StartDate >= '{0}' AND StartDate <= '{1}' ) OR (StartDate >= '{2}' AND EndDate>='{3}')", sDate, eDate, sDate, eDate);
            ViewBag.Contrator = db.Database.SqlQuery<V_Project_Contrator>(sqlContrator).ToList();
            //   ViewBag.Supplier = db.V_Project_Supplier.Where(a => ((a.StartDate >= sDate && a.StartDate <= eDate) || (a.StartDate >= sDate && a.EndDate >= eDate))).ToList();
            ViewBag.Product = db.Products.ToList();
            ViewBag.ProductProject = db.V_Project_Products.Where(a => ((a.StartDate >= sDate && a.StartDate <= eDate) || (a.StartDate >= sDate && a.EndDate >= eDate))).ToList();
            return View();
        }
        public void SetUserBag(long? selectedId = 0)
        {
            var dao = new UserDao();
            var lst = dao.ToList();
            User objUS = dao.FindByID(3);
            lst.Remove(objUS);
            //User objUser = new User();
            //objUser.LoginID = 0;
            //objUser.FullName = "Chọn hết";
            //lst.Add(objUser);
            ViewBag.User = new SelectList(lst, "LoginID", "FullName", selectedId);
        }
        public void SetCatagoryBag(long? selectedId = 0)
        {
            var dao = new CategoryDao();
            var lst = dao.ToList();
            Category objCate = new Category();
            objCate.CategoryID = 0;
            objCate.Name = "Chọn hết";
            lst.Add(objCate);
            ViewBag.CategoryID = new SelectList(lst, "CategoryID", "Name", selectedId);
        }
    
    public void SetSupplierBag(long? selectedId = 0)
    {
        var dao = new SupplierDao();
        var lst = dao.ToList();
        //Category objCate = new Category();
        //objCate.CategoryID = 0;
        //objCate.Name = "Chọn hết";
        //lst.Add(objCate);
        ViewBag.Suppliercate = new SelectList(lst, "ID", "SupplierName", selectedId);
    }
    public void SetStatusBag(long? selectedId = 0)
        {

            var selectList = new SelectList(
                new List<SelectListItem>
                {
                         new SelectListItem {Text = "Tất cả", Value = "-1"},
                                 new SelectListItem {Text = "Dự án hoàn thành", Value = "3"},
                             new SelectListItem {Text = "Dự án không hoàn thành", Value = "4"},
                }, "Value", "Text");

            ViewBag.Status = new SelectList(selectList, "Value", "Text", selectedId);
        }
    }
}