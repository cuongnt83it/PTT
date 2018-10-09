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
    public class ProjectController : BaseController
    {
        PTTDataContext db = null;
        // GET: Project
        public ActionResult Index()
        {
            ProjectDao bdDao = new ProjectDao();

            return View(bdDao.ToList());
        }
        [HttpGet]
        public ActionResult End(long id)
        {
            ProjectDao bdDao = new ProjectDao();
            SetViewBag();
            ViewBag.Project = bdDao.FindByID(id);
            SetViewBag(ViewBag.Project.CityID);
            SetViewBagDistrict(ViewBag.Project.DistrictID, ViewBag.Project.CityID);
            SetCatagoryBag(ViewBag.Project.CategoryID);
            SetResourceIDViewBag(ViewBag.Project.ResourceID);
            SetPriceIDViewBag(ViewBag.Project.PriceID);
            SetViewSupplier(ViewBag.Project.SupplierID);
            if (ViewBag.Project.Status != 1)
            {
                SetAlert("Dự đã hoàn thành!", Common.CommonConstant.ALERT_WARNING);
                return RedirectToAction("Index", "Home", null);
            }
            ProjectUserDao usDao = new ProjectUserDao();
            List<ProjectUser> lstUP = usDao.FindByProjectID(ViewBag.Project.ProjectID);
            List<string> lstUPlogin = new List<string>();
            foreach (var pUs in lstUP)
            {
                //string sLogin = pUs.LoginID.ToString();
                lstUPlogin.Add(pUs.LoginID.ToString());
            }
            SetUserBag(lstUPlogin.ToArray<string>());
            ContratorDao contrDao = new ContratorDao();
            Contrator objConTra = contrDao.FindByID(ViewBag.Project.ContratorID);
            string str = "<p><b>Tên chủ đầu tư: </b>" + objConTra.ContraName + "</p>";
            str += "<p><b>Địa chỉ: </b>" + objConTra.Address + "</p>";
            str += "<p><b>Thông tin liên hệ: </b>" + objConTra.FullName + "<b> &nbsp;&nbsp;&nbsp;  Điện thoại: </b>" + objConTra.Phone + "</p>";
            ViewBag.PrContraDetail = str;
            ViewBag.PrContraCode = objConTra.ContratorID;
            BuilderDao buiDao = new BuilderDao();
            Builder objBuilder = buiDao.FindByID(ViewBag.Project.BuilderID);
            ViewBag.PrBuiderCode = objBuilder.BuilderID;
            str = "";
            str = "<p><b>Tên nhà thầu: </b>" + objBuilder.BuilderName + "</p>";
            str += "<p><b>Địa chỉ: </b>" + objBuilder.Address + "</p>";
            str += "<p><b>Thông tin liên hệ: </b>" + objBuilder.FullName + "<b>&nbsp;&nbsp; &nbsp; Điện thoại: </b>" + objBuilder.Phone + "</p>";
            ViewBag.BuiderDetail = str;
            ProjectProductDao prProdDao = new ProjectProductDao();

            ViewBag.lstProjectProdut = prProdDao.FindByID(ViewBag.Project.ProjectID);

            ProjectCompetitorDao prComDao = new ProjectCompetitorDao();
            ViewBag.lstProjectCompe = prComDao.FindByID(ViewBag.Project.ProjectID);
            List<string> lstCompeID = new List<string>();
            foreach (var compeId in ViewBag.lstProjectCompe)
            {
                lstCompeID.Add(compeId.CompetiorID.ToString());
            }
            SetCompetitorViewBag(lstCompeID.ToArray<string>());

            CompetiorProductDao comProductDao = new CompetiorProductDao();
            List<ProjectCompeProduct> lstProComProd = new List<ProjectCompeProduct>();
            foreach (var prcompe in ViewBag.lstProjectCompe)
            {
                var lstComProduct = comProductDao.FindByID(prcompe.ID);
                foreach (var comPro in lstComProduct)
                {
                    ProjectCompeProduct pcp = new ProjectCompeProduct();
                    pcp.ID = prcompe.ID;
                    pcp.CompetiorID = prcompe.CompetiorID;
                    pcp.ProjectID = prcompe.ProjectID;
                    pcp.ProductID = comPro.ProductID;
                    pcp.Discount = comPro.Discount;
                    pcp.DiscountVAT = comPro.DiscountVAT;
                    lstProComProd.Add(pcp);
                }

            }

            ViewBag.lstComeProduct = lstProComProd;
            ProcessDao prcessDao = new ProcessDao();
            ViewBag.Messege = prcessDao.GetListProjectProcessMessege(id).Count.ToString();
            FeedbackDao feedDao = new FeedbackDao();
            ViewBag.Feedback = feedDao.ToListByProjectID(id).Count.ToString();
            return View();
        }
        [HttpPost]
        public ActionResult End(FormCollection data)
        {
            ProjectDao bdDao = new ProjectDao();
            long id = Convert.ToInt64(data["hdIDProject"].ToString());

            Project objProject = bdDao.FindByID(id);
            ViewBag.Project = objProject;
            if (ViewBag.Project.Status >= 2)
            {
                SetAlert("Dự án đã được duyệt!", Common.CommonConstant.ALERT_WARNING);
                return RedirectToAction("Index", "Home", null);
            }
            SetViewBag();
            SetViewBag(ViewBag.Project.CityID);
            SetViewBagDistrict(ViewBag.Project.DistrictID, ViewBag.Project.CityID);
            SetCatagoryBag(ViewBag.Project.CategoryID);
            SetResourceIDViewBag(ViewBag.Project.ResourceID);
            SetPriceIDViewBag(ViewBag.Project.PriceID);
            SetViewSupplier(ViewBag.Project.SupplierID);
            ProjectUserDao usDao = new ProjectUserDao();
            List<ProjectUser> lstUP = usDao.FindByProjectID(ViewBag.Project.ProjectID);
            List<string> lstUPlogin = new List<string>();
            foreach (var pUs in lstUP)
            {
                //string sLogin = pUs.LoginID.ToString();
                lstUPlogin.Add(pUs.LoginID.ToString());
            }
            SetUserBag(lstUPlogin.ToArray<string>());
            ContratorDao contrDao = new ContratorDao();
            Contrator objConTra = contrDao.FindByID(ViewBag.Project.ContratorID);
            string str = "<p><b>Tên chủ đầu tư: </b>" + objConTra.ContraName + "</p>";
            str += "<p><b>Địa chỉ: </b>" + objConTra.Address + "</p>";
            str += "<p><b>Thông tin liên hệ: </b>" + objConTra.FullName + "<b> &nbsp;&nbsp;&nbsp;  Điện thoại: </b>" + objConTra.Phone + "</p>";
            ViewBag.PrContraDetail = str;
            ViewBag.PrContraCode = objConTra.ContratorID;
            BuilderDao buiDao = new BuilderDao();
            Builder objBuilder = buiDao.FindByID(ViewBag.Project.BuilderID);
            ViewBag.PrBuiderCode = objBuilder.BuilderID;
            str = "";
            str = "<p><b>Tên nhà thầu: </b>" + objBuilder.BuilderName + "</p>";
            str += "<p><b>Địa chỉ: </b>" + objBuilder.Address + "</p>";
            str += "<p><b>Thông tin liên hệ: </b>" + objBuilder.FullName + "<b>&nbsp;&nbsp; &nbsp; Điện thoại: </b>" + objBuilder.Phone + "</p>";
            ViewBag.BuiderDetail = str;

            ProjectProductDao prProdDao = new ProjectProductDao();

            ViewBag.lstProjectProdut = prProdDao.FindByID(ViewBag.Project.ProjectID);

            ProjectCompetitorDao prComDao = new ProjectCompetitorDao();
            ViewBag.lstProjectCompe = prComDao.FindByID(ViewBag.Project.ProjectID);
            List<string> lstCompeID = new List<string>();
            foreach (var compeId in ViewBag.lstProjectCompe)
            {
                lstCompeID.Add(compeId.CompetiorID.ToString());
            }
            SetCompetitorViewBag(lstCompeID.ToArray<string>());

            CompetiorProductDao comProductDao = new CompetiorProductDao();
            List<ProjectCompeProduct> lstProComProd = new List<ProjectCompeProduct>();
            foreach (var prcompe in ViewBag.lstProjectCompe)
            {
                var lstComProduct = comProductDao.FindByID(prcompe.ID);
                foreach (var comPro in lstComProduct)
                {
                    ProjectCompeProduct pcp = new ProjectCompeProduct();
                    pcp.ID = prcompe.ID;
                    pcp.CompetiorID = prcompe.CompetiorID;
                    pcp.ProjectID = prcompe.ProjectID;
                    pcp.ProductID = comPro.ProductID;
                    pcp.Discount = comPro.Discount;
                    lstProComProd.Add(pcp);
                }

            }

            ViewBag.lstComeProduct = lstProComProd;
            ProcessDao prcessDao = new ProcessDao();
            ViewBag.Messege = prcessDao.GetListProjectProcessMessege(id).Count.ToString();
            FeedbackDao feedDao = new FeedbackDao();
            ViewBag.Feedback = feedDao.ToListByProjectID(id).Count.ToString();

            //long iSupplierID = Convert.ToInt64(data["drlSupplier"].ToString());
            //objProject.SupplierID = iSupplierID;
            objProject.Value = Convert.ToDecimal(data["txtPriceProject"].ToString());
            objProject.Status = 2;
            long projectID = objProject.ProjectID;
            string[] lstproductID = data.GetValues("cblProduct");

            UserLogin us = (UserLogin)Session[CommonConstant.USER_SESSION];
            objProject.ModifiedDate = Hepper.GetDateServer();
            objProject.ModifiedBy = us.UserName;
            //Lấy danh sách sản phẩm được chọn với chiết khấu và giá kèm theo
            List<ProjectProduct> lstProductPrject = new List<ProjectProduct>();
            if (lstproductID.Length > 0)
            {
                decimal priceProduct = 0;
                double discoutProdct = 0;
                double discoutProdctVAT = 0;
                long productID = 0;
                foreach (string sprodID in lstproductID)
                {
                    productID = Convert.ToInt64(sprodID);
                    string txtPrice = "txtPrice" + sprodID;
                    string txtDiscount = "txtDiscount" + sprodID;
                    string txtDiscountVAT = "txtDiscountVAT" + sprodID;
                    if (data[txtPrice].ToString() != null)
                    {
                        priceProduct = Convert.ToDecimal(data[txtPrice].ToString());
                    }
                    if (data[txtDiscount].ToString() != null)
                    {
                        discoutProdct = Convert.ToDouble(data[txtDiscount].ToString());
                    }
                    if (data[txtDiscountVAT].ToString() != null)
                    {
                        discoutProdctVAT = Convert.ToDouble(data[txtDiscountVAT].ToString());
                    }
                    ProjectProduct objProPrd = new ProjectProduct();
                    objProPrd.ProductID = productID;
                    objProPrd.Price = priceProduct;
                    objProPrd.ProjectID = projectID;
                    objProPrd.Discount = discoutProdct;
                    objProPrd.DiscountVAT = discoutProdctVAT;
                    lstProductPrject.Add(objProPrd);
                }
            }

            //Thêm sản phẩm của dự án với giá và chiết khấu vào CSDL
            ProjectProductDao ppdtDao = new ProjectProductDao();
            //Xóa sản phẩm đã tồn tại của dự án
            ppdtDao.Delete(objProject.ProjectID);
            foreach (ProjectProduct prpd in lstProductPrject)
            {
                ppdtDao.Insert(prpd);
            }
            if (bdDao.Update(objProject) > 0)
            {

                SetAlert("Gửi thành công!", "success");
                return RedirectToAction("ProjectWaitStart", "Project", null);
            }
            else
            {
                SetAlert("Không Gửi được", "danger");
                return View();
            }
        }
        [HttpGet]
        public ActionResult PassStart(long id)
        {
            ProjectDao bdDao = new ProjectDao();
            SetViewBag();
            ViewBag.Project = bdDao.FindByID(id);
            SetViewBag(ViewBag.Project.CityID);
            SetViewBagDistrict(ViewBag.Project.DistrictID, ViewBag.Project.CityID);
            SetCatagoryBag(ViewBag.Project.CategoryID);
            SetResourceIDViewBag(ViewBag.Project.ResourceID);
            SetPriceIDViewBag(ViewBag.Project.PriceID);
            SetViewSupplier(ViewBag.Project.SupplierID);
            ProjectUserDao usDao = new ProjectUserDao();
            List<ProjectUser> lstUP = usDao.FindByProjectID(ViewBag.Project.ProjectID);
            List<string> lstUPlogin = new List<string>();
            foreach (var pUs in lstUP)
            {
                //string sLogin = pUs.LoginID.ToString();
                lstUPlogin.Add(pUs.LoginID.ToString());
            }
            SetUserBag(lstUPlogin.ToArray<string>());
            ContratorDao contrDao = new ContratorDao();
            Contrator objConTra = contrDao.FindByID(ViewBag.Project.ContratorID);
            string str = "<p><b>Tên chủ đầu tư: </b>" + objConTra.ContraName + "</p>";
            str += "<p><b>Địa chỉ: </b>" + objConTra.Address + "</p>";
            str += "<p><b>Thông tin liên hệ: </b>" + objConTra.FullName + "<b> &nbsp;&nbsp;&nbsp;  Điện thoại: </b>" + objConTra.Phone + "</p>";
            ViewBag.PrContraDetail = str;
            ViewBag.PrContraCode = objConTra.ContratorID;
            BuilderDao buiDao = new BuilderDao();
            Builder objBuilder = buiDao.FindByID(ViewBag.Project.BuilderID);
            ViewBag.PrBuiderCode = objBuilder.BuilderID;
            str = "";
            str = "<p><b>Tên nhà thầu: </b>" + objBuilder.BuilderName + "</p>";
            str += "<p><b>Địa chỉ: </b>" + objBuilder.Address + "</p>";
            str += "<p><b>Thông tin liên hệ: </b>" + objBuilder.FullName + "<b>&nbsp;&nbsp; &nbsp; Điện thoại: </b>" + objBuilder.Phone + "</p>";
            ViewBag.BuiderDetail = str;
            ProjectProductDao prProdDao = new ProjectProductDao();

            ViewBag.lstProjectProdut = prProdDao.FindByID(ViewBag.Project.ProjectID);

            ProjectCompetitorDao prComDao = new ProjectCompetitorDao();
            ViewBag.lstProjectCompe = prComDao.FindByID(ViewBag.Project.ProjectID);
            List<string> lstCompeID = new List<string>();
            foreach (var compeId in ViewBag.lstProjectCompe)
            {
                lstCompeID.Add(compeId.CompetiorID.ToString());
            }
            SetCompetitorViewBag(lstCompeID.ToArray<string>());

            CompetiorProductDao comProductDao = new CompetiorProductDao();
            List<ProjectCompeProduct> lstProComProd = new List<ProjectCompeProduct>();
            foreach (var prcompe in ViewBag.lstProjectCompe)
            {
                var lstComProduct = comProductDao.FindByID(prcompe.ID);
                foreach (var comPro in lstComProduct)
                {
                    ProjectCompeProduct pcp = new ProjectCompeProduct();
                    pcp.ID = prcompe.ID;
                    pcp.CompetiorID = prcompe.CompetiorID;
                    pcp.ProjectID = prcompe.ProjectID;
                    pcp.ProductID = comPro.ProductID;
                    pcp.Discount = comPro.Discount;
                    pcp.DiscountVAT = comPro.DiscountVAT;
                    lstProComProd.Add(pcp);
                }

            }

            ViewBag.lstComeProduct = lstProComProd;
            ProcessDao prcessDao = new ProcessDao();
            ViewBag.Messege = prcessDao.GetListProjectProcessMessege(id).Count.ToString();
            FeedbackDao feedDao = new FeedbackDao();
            ViewBag.Feedback = feedDao.ToListByProjectID(id).Count.ToString();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PassStart(FormCollection data)
        {
            ProjectDao bdDao = new ProjectDao();
            long id = Convert.ToInt64(data["hdIDProject"].ToString());
            Project objProject = bdDao.FindByID(id);
            ViewBag.Project = objProject;
            if (ViewBag.Project.Status != 0)
            {
                SetAlert("Dự án đã được duyệt!", Common.CommonConstant.ALERT_WARNING);
                return RedirectToAction("Index", "Home", null);
            }
            SetViewBag();
            SetViewBag(ViewBag.Project.CityID);
            SetViewBagDistrict(ViewBag.Project.DistrictID, ViewBag.Project.CityID);
            SetCatagoryBag(ViewBag.Project.CategoryID);
            SetResourceIDViewBag(ViewBag.Project.ResourceID);
            SetPriceIDViewBag(ViewBag.Project.PriceID);
            SetViewSupplier(ViewBag.Project.SupplierID);
            ProjectUserDao usDao = new ProjectUserDao();
            List<ProjectUser> lstUP = usDao.FindByProjectID(ViewBag.Project.ProjectID);
            List<string> lstUPlogin = new List<string>();
            foreach (var pUs in lstUP)
            {
                //string sLogin = pUs.LoginID.ToString();
                lstUPlogin.Add(pUs.LoginID.ToString());
            }
            SetUserBag(lstUPlogin.ToArray<string>());
            ContratorDao contrDao = new ContratorDao();
            Contrator objConTra = contrDao.FindByID(ViewBag.Project.ContratorID);
            string str = "<p><b>Tên chủ đầu tư: </b>" + objConTra.ContraName + "</p>";
            str += "<p><b>Địa chỉ: </b>" + objConTra.Address + "</p>";
            str += "<p><b>Thông tin liên hệ: </b>" + objConTra.FullName + "<b> &nbsp;&nbsp;&nbsp;  Điện thoại: </b>" + objConTra.Phone + "</p>";
            ViewBag.PrContraDetail = str;
            ViewBag.PrContraCode = objConTra.ContratorID;
            BuilderDao buiDao = new BuilderDao();
            Builder objBuilder = buiDao.FindByID(ViewBag.Project.BuilderID);
            ViewBag.PrBuiderCode = objBuilder.BuilderID;
            str = "";
            str = "<p><b>Tên nhà thầu: </b>" + objBuilder.BuilderName + "</p>";
            str += "<p><b>Địa chỉ: </b>" + objBuilder.Address + "</p>";
            str += "<p><b>Thông tin liên hệ: </b>" + objBuilder.FullName + "<b>&nbsp;&nbsp; &nbsp; Điện thoại: </b>" + objBuilder.Phone + "</p>";
            ViewBag.BuiderDetail = str;

            ProjectProductDao prProdDao = new ProjectProductDao();

            ViewBag.lstProjectProdut = prProdDao.FindByID(ViewBag.Project.ProjectID);

            ProjectCompetitorDao prComDao = new ProjectCompetitorDao();
            ViewBag.lstProjectCompe = prComDao.FindByID(ViewBag.Project.ProjectID);
            List<string> lstCompeID = new List<string>();
            foreach (var compeId in ViewBag.lstProjectCompe)
            {
                lstCompeID.Add(compeId.CompetiorID.ToString());
            }
            SetCompetitorViewBag(lstCompeID.ToArray<string>());

            CompetiorProductDao comProductDao = new CompetiorProductDao();
            List<ProjectCompeProduct> lstProComProd = new List<ProjectCompeProduct>();
            foreach (var prcompe in ViewBag.lstProjectCompe)
            {
                var lstComProduct = comProductDao.FindByID(prcompe.ID);
                foreach (var comPro in lstComProduct)
                {
                    ProjectCompeProduct pcp = new ProjectCompeProduct();
                    pcp.ID = prcompe.ID;
                    pcp.CompetiorID = prcompe.CompetiorID;
                    pcp.ProjectID = prcompe.ProjectID;
                    pcp.ProductID = comPro.ProductID;
                    pcp.Discount = comPro.Discount;
                    lstProComProd.Add(pcp);
                }

            }

            ViewBag.lstComeProduct = lstProComProd;
            ProcessDao prcessDao = new ProcessDao();
            ViewBag.Messege = prcessDao.GetListProjectProcessMessege(id).Count.ToString();
            FeedbackDao feedDao = new FeedbackDao();
            ViewBag.Feedback = feedDao.ToListByProjectID(id).Count.ToString();

            int iStatus = Convert.ToInt32(data["drlStatus"].ToString());
            objProject.Status = iStatus;
            objProject.NotePass = data["txtNote"].ToString();
            objProject.StartDate = Convert.ToDateTime(data["dtStart"].ToString());
            objProject.DateLine = Convert.ToDateTime(data["dtEnd"].ToString());
            UserLogin us = (UserLogin)Session[CommonConstant.USER_SESSION];
            objProject.ModifiedDate = Hepper.GetDateServer();
            objProject.ModifiedBy = us.UserName;
            if (bdDao.Update(objProject) > 0)
            {
                SetAlert("Duyệt thành công!", "success");
                return RedirectToAction("ProjectWaitStart", "Project", null);
            }
            else
            {
                SetAlert("Không thêm được", "danger");
                return View();
            }

        }


        [HttpGet]
        public ActionResult PassEnd(long id)
        {
            ProjectDao bdDao = new ProjectDao();
            SetViewBag();
            ViewBag.Project = bdDao.FindByID(id);
            SetViewBag(ViewBag.Project.CityID);
            SetViewBagDistrict(ViewBag.Project.DistrictID, ViewBag.Project.CityID);
            SetCatagoryBag(ViewBag.Project.CategoryID);
            SetResourceIDViewBag(ViewBag.Project.ResourceID);
            SetPriceIDViewBag(ViewBag.Project.PriceID);
            SetViewSupplier(ViewBag.Project.SupplierID);
            ProjectUserDao usDao = new ProjectUserDao();
            List<ProjectUser> lstUP = usDao.FindByProjectID(ViewBag.Project.ProjectID);
            List<string> lstUPlogin = new List<string>();
            foreach (var pUs in lstUP)
            {
                //string sLogin = pUs.LoginID.ToString();
                lstUPlogin.Add(pUs.LoginID.ToString());
            }
            SetUserBag(lstUPlogin.ToArray<string>());
            ContratorDao contrDao = new ContratorDao();
            Contrator objConTra = contrDao.FindByID(ViewBag.Project.ContratorID);
            string str = "<p><b>Tên chủ đầu tư: </b>" + objConTra.ContraName + "</p>";
            str += "<p><b>Địa chỉ: </b>" + objConTra.Address + "</p>";
            str += "<p><b>Thông tin liên hệ: </b>" + objConTra.FullName + "<b> &nbsp;&nbsp;&nbsp;  Điện thoại: </b>" + objConTra.Phone + "</p>";
            ViewBag.PrContraDetail = str;
            ViewBag.PrContraCode = objConTra.ContratorID;
            BuilderDao buiDao = new BuilderDao();
            Builder objBuilder = buiDao.FindByID(ViewBag.Project.BuilderID);
            ViewBag.PrBuiderCode = objBuilder.BuilderID;
            str = "";
            str = "<p><b>Tên nhà thầu: </b>" + objBuilder.BuilderName + "</p>";
            str += "<p><b>Địa chỉ: </b>" + objBuilder.Address + "</p>";
            str += "<p><b>Thông tin liên hệ: </b>" + objBuilder.FullName + "<b>&nbsp;&nbsp; &nbsp; Điện thoại: </b>" + objBuilder.Phone + "</p>";
            ViewBag.BuiderDetail = str;
            ProjectProductDao prProdDao = new ProjectProductDao();

            ViewBag.lstProjectProdut = prProdDao.FindByID(ViewBag.Project.ProjectID);

            ProjectCompetitorDao prComDao = new ProjectCompetitorDao();
            ViewBag.lstProjectCompe = prComDao.FindByID(ViewBag.Project.ProjectID);
            List<string> lstCompeID = new List<string>();
            foreach (var compeId in ViewBag.lstProjectCompe)
            {
                lstCompeID.Add(compeId.CompetiorID.ToString());
            }
            SetCompetitorViewBag(lstCompeID.ToArray<string>());

            CompetiorProductDao comProductDao = new CompetiorProductDao();
            List<ProjectCompeProduct> lstProComProd = new List<ProjectCompeProduct>();
            foreach (var prcompe in ViewBag.lstProjectCompe)
            {
                var lstComProduct = comProductDao.FindByID(prcompe.ID);
                foreach (var comPro in lstComProduct)
                {
                    ProjectCompeProduct pcp = new ProjectCompeProduct();
                    pcp.ID = prcompe.ID;
                    pcp.CompetiorID = prcompe.CompetiorID;
                    pcp.ProjectID = prcompe.ProjectID;
                    pcp.ProductID = comPro.ProductID;
                    pcp.Discount = comPro.Discount;
                    pcp.DiscountVAT = comPro.DiscountVAT;
                    lstProComProd.Add(pcp);
                }

            }

            ViewBag.lstComeProduct = lstProComProd;
            ProcessDao prcessDao = new ProcessDao();
            ViewBag.Messege = prcessDao.GetListProjectProcessMessege(id).Count.ToString();
            FeedbackDao feedDao = new FeedbackDao();
            ViewBag.Feedback = feedDao.ToListByProjectID(id).Count.ToString();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PassEnd(FormCollection data)
        {
            ProjectDao bdDao = new ProjectDao();
            long id = Convert.ToInt64(data["hdIDProject"].ToString());
            Project objProject = bdDao.FindByID(id);
            ViewBag.Project = objProject;
            if (ViewBag.Project.Status != 2)
            {
                SetAlert("Dự án đã được duyệt!", Common.CommonConstant.ALERT_WARNING);
                return RedirectToAction("Index", "Home", null);
            }
            SetViewBag();
            SetViewBag(ViewBag.Project.CityID);
            SetViewBagDistrict(ViewBag.Project.DistrictID, ViewBag.Project.CityID);
            SetCatagoryBag(ViewBag.Project.CategoryID);
            SetResourceIDViewBag(ViewBag.Project.ResourceID);
            SetPriceIDViewBag(ViewBag.Project.PriceID);
            SetViewSupplier(ViewBag.Project.SupplierID);
            ProjectUserDao usDao = new ProjectUserDao();
            List<ProjectUser> lstUP = usDao.FindByProjectID(ViewBag.Project.ProjectID);
            List<string> lstUPlogin = new List<string>();
            foreach (var pUs in lstUP)
            {
                //string sLogin = pUs.LoginID.ToString();
                lstUPlogin.Add(pUs.LoginID.ToString());
            }
            SetUserBag(lstUPlogin.ToArray<string>());
            ContratorDao contrDao = new ContratorDao();
            Contrator objConTra = contrDao.FindByID(ViewBag.Project.ContratorID);
            string str = "<p><b>Tên chủ đầu tư: </b>" + objConTra.ContraName + "</p>";
            str += "<p><b>Địa chỉ: </b>" + objConTra.Address + "</p>";
            str += "<p><b>Thông tin liên hệ: </b>" + objConTra.FullName + "<b> &nbsp;&nbsp;&nbsp;  Điện thoại: </b>" + objConTra.Phone + "</p>";
            ViewBag.PrContraDetail = str;
            ViewBag.PrContraCode = objConTra.ContratorID;
            BuilderDao buiDao = new BuilderDao();
            Builder objBuilder = buiDao.FindByID(ViewBag.Project.BuilderID);
            ViewBag.PrBuiderCode = objBuilder.BuilderID;
            str = "";
            str = "<p><b>Tên nhà thầu: </b>" + objBuilder.BuilderName + "</p>";
            str += "<p><b>Địa chỉ: </b>" + objBuilder.Address + "</p>";
            str += "<p><b>Thông tin liên hệ: </b>" + objBuilder.FullName + "<b>&nbsp;&nbsp; &nbsp; Điện thoại: </b>" + objBuilder.Phone + "</p>";
            ViewBag.BuiderDetail = str;

            ProjectProductDao prProdDao = new ProjectProductDao();

            ViewBag.lstProjectProdut = prProdDao.FindByID(ViewBag.Project.ProjectID);

            ProjectCompetitorDao prComDao = new ProjectCompetitorDao();
            ViewBag.lstProjectCompe = prComDao.FindByID(ViewBag.Project.ProjectID);
            List<string> lstCompeID = new List<string>();
            foreach (var compeId in ViewBag.lstProjectCompe)
            {
                lstCompeID.Add(compeId.CompetiorID.ToString());
            }
            SetCompetitorViewBag(lstCompeID.ToArray<string>());

            CompetiorProductDao comProductDao = new CompetiorProductDao();
            List<ProjectCompeProduct> lstProComProd = new List<ProjectCompeProduct>();
            foreach (var prcompe in ViewBag.lstProjectCompe)
            {
                var lstComProduct = comProductDao.FindByID(prcompe.ID);
                foreach (var comPro in lstComProduct)
                {
                    ProjectCompeProduct pcp = new ProjectCompeProduct();
                    pcp.ID = prcompe.ID;
                    pcp.CompetiorID = prcompe.CompetiorID;
                    pcp.ProjectID = prcompe.ProjectID;
                    pcp.ProductID = comPro.ProductID;
                    pcp.Discount = comPro.Discount;
                    lstProComProd.Add(pcp);
                }

            }

            ViewBag.lstComeProduct = lstProComProd;
            ProcessDao prcessDao = new ProcessDao();
            ViewBag.Messege = prcessDao.GetListProjectProcessMessege(id).Count.ToString();
            FeedbackDao feedDao = new FeedbackDao();
            ViewBag.Feedback = feedDao.ToListByProjectID(id).Count.ToString();

            int iStatus = Convert.ToInt32(data["drlStatus"].ToString());
            // long iSupplierID = Convert.ToInt64(data["drlSupplier"].ToString());
            objProject.Status = iStatus;
            objProject.Note = data["txtNote"].ToString();
            //  objProject.SupplierID = iSupplierID;
            // objProject.Value = Convert.ToDecimal(data["txtPriceProject"].ToString());
            objProject.EndDate = Convert.ToDateTime(data["dtEnd"].ToString());
            UserLogin us = (UserLogin)Session[CommonConstant.USER_SESSION];
            objProject.ModifiedDate = Hepper.GetDateServer();
            objProject.ModifiedBy = us.UserName;
            if (bdDao.Update(objProject) > 0)
            {
                SetAlert("Duyệt thành công!", Common.CommonConstant.ALERT_SUCCESS);
                return RedirectToAction("ProjectWaitStart", "Project", null);
            }
            else
            {
                SetAlert("Không thêm được", "danger");
                return View();
            }
        }
        public ActionResult ProjectWaitStart()

        {

            UserLogin user = (UserLogin)Session[CommonConstant.USER_SESSION];
            db = new PTTDataContext();
            ViewBag.ProjectGroup = from pr in db.Projects
                                   join us in db.Users on pr.CreateBy equals us.UserName
                                   orderby pr.ProjectID ascending
                                   where pr.Status == 0
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
        public ActionResult ProjectEnd()

        {

            UserLogin user = (UserLogin)Session[CommonConstant.USER_SESSION];
            db = new PTTDataContext();
            ViewBag.ProjectGroup = from pr in db.Projects
                                   join us in db.Users on pr.CreateBy equals us.UserName
                                   orderby pr.ProjectID ascending
                                   where pr.Status == 3
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
        public ActionResult ProjectStart()

        {

            UserLogin user = (UserLogin)Session[CommonConstant.USER_SESSION];
            db = new PTTDataContext();
            ViewBag.ProjectGroup = from pr in db.Projects
                                   join us in db.Users on pr.CreateBy equals us.UserName
                                   orderby pr.ProjectID ascending
                                   where pr.Status == 1
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
        public ActionResult ProjectStop()

        {

            UserLogin user = (UserLogin)Session[CommonConstant.USER_SESSION];
            db = new PTTDataContext();
            ViewBag.ProjectGroup = from pr in db.Projects
                                   join us in db.Users on pr.CreateBy equals us.UserName
                                   orderby pr.ProjectID ascending
                                   where pr.Status == 4
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
        public ActionResult ProjectWaitEnd()

        {

            UserLogin user = (UserLogin)Session[CommonConstant.USER_SESSION];
            db = new PTTDataContext();
            ViewBag.ProjectGroup = from pr in db.Projects
                                   join us in db.Users on pr.CreateBy equals us.UserName
                                   orderby pr.ProjectID ascending
                                   where pr.Status == 2
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
        // GET: Project/Details/5
        public ActionResult Details(long id)
        {
            ProjectDao bdDao = new ProjectDao();
            SetViewBag();
            ViewBag.Project = bdDao.FindByID(id);
            SetViewBag(ViewBag.Project.CityID);
            SetViewBagDistrict(ViewBag.Project.DistrictID, ViewBag.Project.CityID);
            SetCatagoryBag(ViewBag.Project.CategoryID);
            SetResourceIDViewBag(ViewBag.Project.ResourceID);
            SetPriceIDViewBag(ViewBag.Project.PriceID);
            SetViewSupplier(ViewBag.Project.SupplierID);
            ProjectUserDao usDao = new ProjectUserDao();
            List<ProjectUser> lstUP = usDao.FindByProjectID(ViewBag.Project.ProjectID);
            List<string> lstUPlogin = new List<string>();
            foreach (var pUs in lstUP)
            {
                //string sLogin = pUs.LoginID.ToString();
                lstUPlogin.Add(pUs.LoginID.ToString());
            }
            SetUserBag(lstUPlogin.ToArray<string>());
            ContratorDao contrDao = new ContratorDao();
            Contrator objConTra = contrDao.FindByID(ViewBag.Project.ContratorID);
            string str = "<p><b>Tên chủ đầu tư: </b>" + objConTra.ContraName + "</p>";
            str += "<p><b>Địa chỉ: </b>" + objConTra.Address + "</p>";
            str += "<p><b>Thông tin liên hệ: </b>" + objConTra.FullName + "<b> &nbsp;&nbsp;&nbsp;  Điện thoại: </b>" + objConTra.Phone + "</p>";
            ViewBag.PrContraDetail = str;
            ViewBag.PrContraCode = objConTra.ContratorID;
            BuilderDao buiDao = new BuilderDao();
            Builder objBuilder = buiDao.FindByID(ViewBag.Project.BuilderID);
            ViewBag.PrBuiderCode = objBuilder.BuilderID;
            str = "";
            str = "<p><b>Tên nhà thầu: </b>" + objBuilder.BuilderName + "</p>";
            str += "<p><b>Địa chỉ: </b>" + objBuilder.Address + "</p>";
            str += "<p><b>Thông tin liên hệ: </b>" + objBuilder.FullName + "<b>&nbsp;&nbsp; &nbsp; Điện thoại: </b>" + objBuilder.Phone + "</p>";
            ViewBag.BuiderDetail = str;
            ProjectProductDao prProdDao = new ProjectProductDao();

            ViewBag.lstProjectProdut = prProdDao.FindByID(ViewBag.Project.ProjectID);

            ProjectCompetitorDao prComDao = new ProjectCompetitorDao();
            ViewBag.lstProjectCompe = prComDao.FindByID(ViewBag.Project.ProjectID);
            List<string> lstCompeID = new List<string>();
            foreach (var compeId in ViewBag.lstProjectCompe)
            {
                lstCompeID.Add(compeId.CompetiorID.ToString());
            }
            SetCompetitorViewBag(lstCompeID.ToArray<string>());

            CompetiorProductDao comProductDao = new CompetiorProductDao();
            List<ProjectCompeProduct> lstProComProd = new List<ProjectCompeProduct>();
            foreach (var prcompe in ViewBag.lstProjectCompe)
            {
                var lstComProduct = comProductDao.FindByID(prcompe.ID);
                foreach (var comPro in lstComProduct)
                {
                    ProjectCompeProduct pcp = new ProjectCompeProduct();
                    pcp.ID = prcompe.ID;
                    pcp.CompetiorID = prcompe.CompetiorID;
                    pcp.ProjectID = prcompe.ProjectID;
                    pcp.ProductID = comPro.ProductID;
                    pcp.Discount = comPro.Discount;
                    pcp.DiscountVAT = comPro.DiscountVAT;
                    lstProComProd.Add(pcp);
                }

            }

            ViewBag.lstComeProduct = lstProComProd;
            ProcessDao prcessDao = new ProcessDao();
            ViewBag.Messege = prcessDao.GetListProjectProcessMessege(id).Count.ToString();
            FeedbackDao feedDao = new FeedbackDao();
            ViewBag.Feedback = feedDao.ToListByProjectID(id).Count.ToString();
            return View();
        }

        // GET: Project/Create
        public ActionResult Create()
        {
            SetViewBag();
            SetViewBagDistrict();
            SetCatagoryBag();
            SetPriceIDViewBag();
            SetResourceIDViewBag();
            SetUserBag();
            SetCompetitorViewBag();
            SetViewSupplier();
            return View();
        }

        // POST: Project/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormCollection data)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    bool kt = true;
                    string cityID = data["CityID"].ToString();
                    SetViewBag(cityID);
                    long categoryID = Convert.ToInt64(data["CategoryID"].ToString());
                    SetCatagoryBag(categoryID);
                    long priceID = Convert.ToInt64(data["PriceID"].ToString());
                    SetPriceIDViewBag(priceID);
                    SetViewBagDistrict(data["DistrictID"], data["CityID"]);
                    SetResourceIDViewBag(Convert.ToInt64(data["ResourceID"]));
                    SetPriceIDViewBag(Convert.ToInt64(data["PriceID"]));
                    string[] members = data.GetValues("drbMember");

                    SetUserBag(members);


                    // string a = Competitors[0].ToString();

                    //  string projectCode = data["txtCode"].ToString();
                    var dao = new ProjectDao();

                    string projectCode = dao.GenaraCode("BPTTT", 5);
                    string name = data["Name"].ToString();
                    string address = data["Address"].ToString();
                    string contratorID = data["txtContratorID"].ToString();
                    string builderID = data["txtBuilder"].ToString();
                    string[] lstproductID = data.GetValues("cblProduct");
                    //string IsGroup = data["IsGroup"].ToString();
                    //string IsPublic = data["IsPublic"].ToString();

                    //Kiem tra ma chu dau tu
                    ContratorDao contraDAO = new ContratorDao();
                    Contrator objContra = contraDAO.FindByCode(contratorID.Trim());
                    if (objContra == null)
                    {
                        kt = false;
                        ModelState.AddModelError("", "Mã chủ đầu tư không đúng!");

                    }


                    BuilderDao buiderDao = new BuilderDao();
                    Builder objBuider = buiderDao.FindByCode(builderID.Trim());
                    if (objBuider == null)
                    {
                        kt = false;
                        ModelState.AddModelError("", "Mã nhà thầu thi công không đúng!");

                    }
                    long iSupplierID = Convert.ToInt64(data["drlSupplier"].ToString());
                    SetViewSupplier(iSupplierID);
                    //Danh sách  đối thủ cạnh tranh được chọn
                    string[] lstCompetitors = data.GetValues("drlCompetitor");
                    if (lstCompetitors == null)
                    {
                        kt = false;
                        ModelState.AddModelError("", "Bạn chưa chọn đối thủ cạnh tranh!");

                    }
                    SetCompetitorViewBag(lstCompetitors);
                    UserLogin us = (UserLogin)Session[CommonConstant.USER_SESSION];
                    ProjectDao bdDao = new ProjectDao();
                    Project objProject = new Project();

                    objProject.SupplierID = iSupplierID;
                    objProject.CreateDate = Hepper.GetDateServer();
                    objProject.ModifiedDate = Hepper.GetDateServer();
                    objProject.CreateBy = us.UserName;
                    objProject.ModifiedBy = us.UserName;
                    objProject.Name = name;
                    //bool bPublic = true;
                    //if (IsPublic.Equals("false")) bPublic = false;
                    //objProject.IsPublic = bPublic;
                    //bool bGroup = true;
                    //if (IsGroup.Equals("false")) bGroup = false;
                    // objProject.IsGroup = bGroup;
                    objProject.PriceID = Convert.ToInt64(priceID);
                    objProject.ResourceID = Convert.ToInt64(data["ResourceID"].ToString());
                    objProject.CategoryID = categoryID;
                    objProject.CityID = cityID;
                    objProject.DistrictID = data["DistrictID"].ToString();
                    objProject.Description = data["txtDescription"].ToString();
                    objProject.Address = address;
                    objProject.Code = projectCode;
                    objProject.EndCreate = Convert.ToDateTime(data["EndCreate"].ToString());
                    objProject.Status = 0;
                    objProject.DateLine = Hepper.GetDateServer();
                    objProject.StartDate = Hepper.GetDateServer();
                    //Thêm dự án vào CSDL
                    if (kt)
                    {
                        objProject.ContratorID = objContra.ID;
                        objProject.BuilderID = (objBuider.ID);
                        long projectID = bdDao.Insert(objProject);

                        //thêm danh sách nhóm vào trong dự án
                        ProjectUserDao prUSDao = new ProjectUserDao();
                        ProjectUser objPrUS = new ProjectUser();
                        objPrUS.ProjectID = projectID;
                        objPrUS.LoginID = us.UserID;
                        objPrUS.IsAdmin = true;
                        prUSDao.Insert(objPrUS);
                        foreach (string sUsID in members)
                        {
                            long usID = Convert.ToInt64(sUsID);
                            if (usID != us.UserID)
                            {
                                ProjectUser objPrUSM = new ProjectUser();
                                objPrUSM.ProjectID = projectID;
                                objPrUSM.LoginID = usID;
                                objPrUSM.IsAdmin = false;
                                prUSDao.Insert(objPrUSM);
                            }
                        }
                        //Lấy danh sách sản phẩm được chọn với chiết khấu và giá kèm theo
                        List<ProjectProduct> lstProductPrject = new List<ProjectProduct>();
                        if (lstproductID.Length > 0)
                        {
                            decimal priceProduct = 0;
                            double discoutProdct = 0;
                            double discoutProdctVAT = 0;
                            long productID = 0;
                            foreach (string sprodID in lstproductID)
                            {
                                productID = Convert.ToInt64(sprodID);
                                string txtPrice = "txtPrice" + sprodID;
                                string txtDiscount = "txtDiscount" + sprodID;
                                string txtDiscountVAT = "txtDiscountVAT" + sprodID;
                                if (data[txtPrice].ToString() != null)
                                {
                                    priceProduct = Convert.ToDecimal(data[txtPrice].ToString());
                                }
                                if (data[txtDiscount].ToString() != null)
                                {
                                    discoutProdct = Convert.ToDouble(data[txtDiscount].ToString());
                                }
                                if (data[txtDiscountVAT].ToString() != null)
                                {
                                    discoutProdctVAT = Convert.ToDouble(data[txtDiscountVAT].ToString());
                                }
                                ProjectProduct objProPrd = new ProjectProduct();
                                objProPrd.ProductID = productID;
                                objProPrd.Price = priceProduct;
                                objProPrd.ProjectID = projectID;
                                objProPrd.Discount = discoutProdct;
                                objProPrd.DiscountVAT = discoutProdctVAT;
                                lstProductPrject.Add(objProPrd);
                            }
                        }

                        //Thêm sản phẩm của dự án với giá và chiết khấu vào CSDL
                        ProjectProductDao ppdtDao = new ProjectProductDao();
                        foreach (ProjectProduct prpd in lstProductPrject)
                        {
                            ppdtDao.Insert(prpd);
                        }


                        //Lấy danh sách chiết khấu giá của sản phẩm mà đối thủ cạnh tranh
                        List<ProjectCompetitor> lstprojectConpetitor = new List<ProjectCompetitor>();
                        ProjectCompetitorDao prcDao = new ProjectCompetitorDao();
                        if (lstCompetitors.Length > 0)
                        {
                            long competitorID = 0;
                            foreach (string scpID in lstCompetitors)
                            {
                                competitorID = Convert.ToInt64(scpID);

                                ProjectCompetitor objProCompt = new ProjectCompetitor();
                                objProCompt.CompetiorID = competitorID;
                                //objProCompt.ID = 0;
                                objProCompt.ProjectID = projectID;
                                // Insert vào Bang lấy được lại ID cho vào danh sách
                                objProCompt.ID = prcDao.Insert(objProCompt);
                                lstprojectConpetitor.Add(objProCompt);
                            }
                        }
                        //Kiểm tra những sản phẩm thuộc đối thủ đã chọn lấy chiết khấu của sản phẩm đấy
                        List<CompetiorProduct> lstComProduct = new List<CompetiorProduct>();
                        foreach (ProjectCompetitor objProCom in lstprojectConpetitor)
                        {
                            //Lấy danh sách các sản phẩm của từng đối thủ được chọn rồi lấy chiết khấu của sản phẩm đấy
                            string[] lstProdctCom;
                            string drlProuctCom = "cblProduct" + objProCom.CompetiorID;
                            lstProdctCom = data.GetValues(drlProuctCom);
                            if (lstProdctCom.Length > 0)
                            {
                                double discount = 0;
                                double discountVAT = 0;
                                long prID = 0;
                                foreach (string sProID in lstProdctCom)
                                {
                                    string txtDiscount = "txtDiscount" + objProCom.CompetiorID + "_" + sProID;
                                    string txtDiscountVAT = "txtDiscountVAT" + objProCom.CompetiorID + "_" + sProID;
                                    if (data[txtDiscountVAT].ToString() != null)
                                    {
                                        discountVAT = Convert.ToDouble(data[txtDiscountVAT].ToString());
                                    }
                                    if (data[txtDiscount].ToString() != null)
                                    {
                                        discount = Convert.ToDouble(data[txtDiscount].ToString());

                                        prID = Convert.ToInt64(sProID);
                                        CompetiorProduct objComProdt = new CompetiorProduct();
                                        objComProdt.Discount = discount;
                                        objComProdt.DiscountVAT = discountVAT;
                                        objComProdt.ID = objProCom.ID;
                                        objComProdt.ProductID = prID;
                                        lstComProduct.Add(objComProdt);

                                    }
                                }

                            }
                        }
                        //Thêm sản phẩm và chiết khấu của đối thủ vào CSDL
                        CompetiorProductDao compdtDao = new CompetiorProductDao();
                        foreach (CompetiorProduct objCPDT in lstComProduct)
                        {
                            compdtDao.Insert(objCPDT);
                        }


                        SetAlert("Thêm thành công", "success");
                        return RedirectToAction("Index", "Home");
                        // return View();

                    }
                    else
                    {
                        SetAlert("Không thêm được", "danger");
                        return View();
                    }

                }
                else
                {
                    SetAlert("Không thêm được", "danger");
                    return View();
                }
            }
            catch
            {
                SetAlert("Không thêm được", "danger");
                return View();
            }
        }

        // GET: Project/Edit/5
        public ActionResult Edit(long id)
        {
            ProjectDao bdDao = new ProjectDao();
            SetViewBag();
            ViewBag.Project = bdDao.FindByID(id);
            UserLogin us = (UserLogin)Session[CommonConstant.USER_SESSION];
            ProjectUserDao puDao = new ProjectUserDao();
           
            //Kiểm tra quyền truy cập của lạnh đạo
            bool inGroup = false;
            GroupUserDao gru = new GroupUserDao();
            //Kiểm tra theo user có thuộc nhóm lãnh đạo kho
            Guid grid = new Guid("964D283D-BEA0-4D85-B7C0-355487A5DF0C");
            if (gru.FiindByID(grid, us.UserID) != null)
            {
                inGroup = true;
            }
            //Kiểm tra theo user có thuộc nhóm trong dự án ko
            ProjectUser objPU = puDao.FindByID(us.UserID, id);
            if (objPU != null)
            {
                inGroup = true;
                
            }
            if (inGroup == false)
            {
                SetAlert("Bạn không có quyền sửa dự án", Common.CommonConstant.ALERT_DANGER);
                return RedirectToAction("Details", "Project", new { id = id });
            }
            if (ViewBag.Project.Status > 2)
            {
                SetAlert("Dự án đã được duyệt!", Common.CommonConstant.ALERT_WARNING);
                return RedirectToAction("Details", "Project", new { id = id });
            }
           

            SetViewBag(ViewBag.Project.CityID);
            SetViewBagDistrict(ViewBag.Project.DistrictID, ViewBag.Project.CityID);
            SetCatagoryBag(ViewBag.Project.CategoryID);
            SetResourceIDViewBag(ViewBag.Project.ResourceID);
            SetPriceIDViewBag(ViewBag.Project.PriceID);
            SetViewSupplier(ViewBag.Project.SupplierID);
            ProjectUserDao usDao = new ProjectUserDao();
            List<ProjectUser> lstUP = usDao.FindByProjectID(ViewBag.Project.ProjectID);
            List<string> lstUPlogin = new List<string>();
            foreach (var pUs in lstUP)
            {
                //string sLogin = pUs.LoginID.ToString();
                lstUPlogin.Add(pUs.LoginID.ToString());
            }
            SetUserBag(lstUPlogin.ToArray<string>());
            ContratorDao contrDao = new ContratorDao();
            Contrator objConTra = contrDao.FindByID(ViewBag.Project.ContratorID);
            string str = "<p><b>Tên chủ đầu tư: </b>" + objConTra.ContraName + "</p>";
            str += "<p><b>Địa chỉ: </b>" + objConTra.Address + "</p>";
            str += "<p><b>Thông tin liên hệ: </b>" + objConTra.FullName + "<b> &nbsp;&nbsp;&nbsp;  Điện thoại: </b>" + objConTra.Phone + "</p>";
            ViewBag.PrContraDetail = str;
            ViewBag.PrContraCode = objConTra.ContratorID;
            BuilderDao buiDao = new BuilderDao();
            Builder objBuilder = buiDao.FindByID(ViewBag.Project.BuilderID);
            ViewBag.PrBuiderCode = objBuilder.BuilderID;
            str = "";
            str = "<p><b>Tên nhà thầu: </b>" + objBuilder.BuilderName + "</p>";
            str += "<p><b>Địa chỉ: </b>" + objBuilder.Address + "</p>";
            str += "<p><b>Thông tin liên hệ: </b>" + objBuilder.FullName + "<b>&nbsp;&nbsp; &nbsp; Điện thoại: </b>" + objBuilder.Phone + "</p>";
            ViewBag.BuiderDetail = str;

            ProjectProductDao prProdDao = new ProjectProductDao();

            ViewBag.lstProjectProdut = prProdDao.FindByID(ViewBag.Project.ProjectID);

            ProjectCompetitorDao prComDao = new ProjectCompetitorDao();
            ViewBag.lstProjectCompe = prComDao.FindByID(ViewBag.Project.ProjectID);
            List<string> lstCompeID = new List<string>();
            foreach (var compeId in ViewBag.lstProjectCompe)
            {
                lstCompeID.Add(compeId.CompetiorID.ToString());
            }
            SetCompetitorViewBag(lstCompeID.ToArray<string>());

            CompetiorProductDao comProductDao = new CompetiorProductDao();
            List<ProjectCompeProduct> lstProComProd = new List<ProjectCompeProduct>();
            foreach (var prcompe in ViewBag.lstProjectCompe)
            {
                var lstComProduct = comProductDao.FindByID(prcompe.ID);
                foreach (var comPro in lstComProduct)
                {
                    ProjectCompeProduct pcp = new ProjectCompeProduct();
                    pcp.ID = prcompe.ID;
                    pcp.CompetiorID = prcompe.CompetiorID;
                    pcp.ProjectID = prcompe.ProjectID;
                    pcp.ProductID = comPro.ProductID;
                    pcp.Discount = comPro.Discount;
                    pcp.DiscountVAT = comPro.DiscountVAT;
                    lstProComProd.Add(pcp);
                }

            }

            ViewBag.lstComeProduct = lstProComProd;
            ProcessDao prcessDao = new ProcessDao();
            ViewBag.Messege = prcessDao.GetListProjectProcessMessege(id).Count.ToString();
            FeedbackDao feedDao = new FeedbackDao();
            ViewBag.Feedback = feedDao.ToListByProjectID(id).Count.ToString();
            return View();
        }

        // POST: Project/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FormCollection data)
        {
            try
            {
                ProjectDao bdDao = new ProjectDao();
                long id = Convert.ToInt64(data["hdIDProject"].ToString());
                ViewBag.Project = bdDao.FindByID(id);
                SetViewBag();
                if (ModelState.IsValid)
                {
                    bool kt = true;
                  
                   
                    Project objProject = bdDao.FindByID(id);
                    string cityID = data["CityID"].ToString();
                    SetViewBag(cityID);
                    long categoryID = Convert.ToInt64(data["CategoryID"].ToString());
                    SetCatagoryBag(categoryID);
                    long priceID = Convert.ToInt64(data["PriceID"].ToString());
                    SetPriceIDViewBag(priceID);
                    SetViewBagDistrict(data["DistrictID"], data["CityID"]);
                    SetResourceIDViewBag(Convert.ToInt64(data["ResourceID"]));
                    SetPriceIDViewBag(Convert.ToInt64(data["PriceID"]));
                    string[] members = data.GetValues("drbMember");

                    SetUserBag(members);
                 
                  
                    //string projectCode = data["txtCode"].ToString();
                    string name = data["Name"].ToString();
                    string address = data["Address"].ToString();
                    string contratorID = data["txtContratorID"].ToString();
                    string builderID = data["txtBuilder"].ToString();
                    string[] lstproductID = data.GetValues("cblProduct");
                    //string IsGroup = data["IsGroup"].ToString();
                    //string IsPublic = data["IsPublic"].ToString();

                    //Kiem tra ma chu dau tu
                    ContratorDao contraDAO = new ContratorDao();
                    Contrator objContra = contraDAO.FindByCode(contratorID.Trim());
                    if (objContra == null)
                    {
                        kt = false;
                        ModelState.AddModelError("", "Mã chủ đầu tư không đúng!");

                    }


                    BuilderDao buiderDao = new BuilderDao();
                    Builder objBuider = buiderDao.FindByCode(builderID.Trim());
                    if (objBuider == null)
                    {
                        kt = false;
                        ModelState.AddModelError("", "Mã nhà thầu thi công không đúng!");

                    }
                    long iSupplierID = Convert.ToInt64(data["drlSupplier"].ToString());
                    SetViewSupplier(iSupplierID);
                    //Danh sách  đối thủ cạnh tranh được chọn
                    string[] lstCompetitors = data.GetValues("drlCompetitor");
                    if (lstCompetitors == null)
                    {
                        kt = false;
                        ModelState.AddModelError("", "Bạn chưa chọn đối thủ cạnh tranh!");

                    }

                    // string a = Competitors[0].ToString();
                    SetCompetitorViewBag(lstCompetitors);
                    if (kt == true) { 
                

                    UserLogin us = (UserLogin)Session[CommonConstant.USER_SESSION];



                    objProject.ModifiedDate = Hepper.GetDateServer();

                    objProject.ModifiedBy = us.UserName;
                    objProject.Name = name;
                    //bool bPublic = true;
                    //if (IsPublic.Equals("false")) bPublic = false;
                    //objProject.IsPublic = bPublic;
                    //bool bGroup = true;
                    //if (IsGroup.Equals("false")) bGroup = false;
                    //objProject.IsGroup = bGroup;
                    objProject.PriceID = Convert.ToInt64(priceID);
                    objProject.ResourceID = Convert.ToInt64(data["ResourceID"].ToString());
                    objProject.CategoryID = categoryID;
                    objProject.CityID = cityID;
                    objProject.DistrictID = data["DistrictID"].ToString();
                    objProject.Address = address;
                    //objProject.Code = projectCode;
                    objProject.ContratorID = objContra.ID;
                    objProject.BuilderID = objBuider.ID;
                    objProject.SupplierID = iSupplierID;
                    // objProject.EndCreate = Convert.ToDateTime(data["EndCreate"].ToString());
                    //objProject.Status = 0;
                    // objProject.DateLine = Hepper.GetDateServer();
                    // objProject.StartDate = Hepper.GetDateServer();
                    //Thêm dự án vào CSDL
                    long projectID = bdDao.Update(objProject);

                    //thêm danh sách nhóm vào trong dự án
                    ProjectUserDao prUSDao = new ProjectUserDao();
                    //Xóa nhóm thuộc dự án
                    prUSDao.Delete(objProject.ProjectID);
                    ProjectUser objPrUS = new ProjectUser();
                    objPrUS.ProjectID = projectID;
                    objPrUS.LoginID = us.UserID;
                    objPrUS.IsAdmin = true;
                    prUSDao.Insert(objPrUS);
                    foreach (string sUsID in members)
                    {
                        long usID = Convert.ToInt64(sUsID);
                        if (usID != us.UserID)
                        {
                            ProjectUser objPrUSM = new ProjectUser();
                            objPrUSM.ProjectID = projectID;
                            objPrUSM.LoginID = usID;
                            objPrUSM.IsAdmin = false;
                            prUSDao.Insert(objPrUSM);
                        }
                    }
                    //Lấy danh sách sản phẩm được chọn với chiết khấu và giá kèm theo
                    List<ProjectProduct> lstProductPrject = new List<ProjectProduct>();
                    if (lstproductID.Length > 0)
                    {
                        decimal priceProduct = 0;
                        double discoutProdct = 0;
                        double discoutProdctVAT = 0;
                        long productID = 0;
                        foreach (string sprodID in lstproductID)
                        {
                            productID = Convert.ToInt64(sprodID);
                            string txtPrice = "txtPrice" + sprodID;
                            string txtDiscount = "txtDiscount" + sprodID;
                            string txtDiscountVAT = "txtDiscountVAT" + sprodID;
                            if (data[txtPrice].ToString() != null)
                            {
                                priceProduct = Convert.ToDecimal(data[txtPrice].ToString());
                            }
                            if (data[txtDiscount].ToString() != null)
                            {
                                discoutProdct = Convert.ToDouble(data[txtDiscount].ToString());
                            }
                            if (data[txtDiscountVAT].ToString() != null)
                            {
                                discoutProdctVAT = Convert.ToDouble(data[txtDiscountVAT].ToString());
                            }
                            ProjectProduct objProPrd = new ProjectProduct();
                            objProPrd.ProductID = productID;
                            objProPrd.Price = priceProduct;
                            objProPrd.ProjectID = projectID;
                            objProPrd.Discount = discoutProdct;
                            objProPrd.DiscountVAT = discoutProdctVAT;
                            lstProductPrject.Add(objProPrd);
                        }
                    }
                    //Thêm sản phẩm của dự án với giá và chiết khấu vào CSDL
                    ProjectProductDao ppdtDao = new ProjectProductDao();
                    //Xóa sản phẩm đã tồn tại của dự án
                    ppdtDao.Delete(objProject.ProjectID);
                    foreach (ProjectProduct prpd in lstProductPrject)
                    {
                        ppdtDao.Insert(prpd);
                    }


                    //Lấy danh sách chiết khấu giá của sản phẩm mà đối thủ cạnh tranh
                    List<ProjectCompetitor> lstprojectConpetitor = new List<ProjectCompetitor>();
                    ProjectCompetitorDao prcDao = new ProjectCompetitorDao();
                    //Xóa danh sách các đối thủ cạnh tranh của dự án
                    prcDao.DeleteByProjectID(objProject.ProjectID);
                    if (lstCompetitors.Length > 0)
                    {
                        long competitorID = 0;
                        foreach (string scpID in lstCompetitors)
                        {
                            competitorID = Convert.ToInt64(scpID);

                            ProjectCompetitor objProCompt = new ProjectCompetitor();
                            objProCompt.CompetiorID = competitorID;
                            //objProCompt.ID = 0;
                            objProCompt.ProjectID = projectID;
                            // Insert vào Bang lấy được lại ID cho vào danh sách
                            objProCompt.ID = prcDao.Insert(objProCompt);
                            lstprojectConpetitor.Add(objProCompt);
                        }
                    }

                    //Kiểm tra những sản phẩm thuộc đối thủ đã chọn lấy chiết khấu của sản phẩm đấy
                    List<CompetiorProduct> lstComProduct = new List<CompetiorProduct>();
                    CompetiorProductDao compdtDao = new CompetiorProductDao();
                    foreach (ProjectCompetitor objProCom in lstprojectConpetitor)
                    {
                        //Lấy danh sách các sản phẩm của từng đối thủ được chọn rồi lấy chiết khấu của sản phẩm đấy
                        string[] lstProdctCom;
                        string drlProuctCom = "cblProduct" + objProCom.CompetiorID;
                        lstProdctCom = data.GetValues(drlProuctCom);
                        if (lstProdctCom.Length > 0)
                        {
                            double discount = 0;
                            double discountVAT = 0;
                            long prID = 0;
                            foreach (string sProID in lstProdctCom)
                            {
                                string txtDiscount = "txtDiscount" + objProCom.CompetiorID + "_" + sProID;
                                string txtDiscountVAT = "txtDiscountVAT" + objProCom.CompetiorID + "_" + sProID;
                                if (data[txtDiscountVAT].ToString() != null)
                                {
                                    discountVAT = Convert.ToDouble(data[txtDiscountVAT].ToString());
                                }
                                if (data[txtDiscount].ToString() != null)
                                {
                                    discount = Convert.ToDouble(data[txtDiscount].ToString());

                                    prID = Convert.ToInt64(sProID);
                                    CompetiorProduct objComProdt = new CompetiorProduct();
                                    objComProdt.Discount = discount;
                                    objComProdt.DiscountVAT = discountVAT;
                                    objComProdt.ID = objProCom.ID;
                                    objComProdt.ProductID = prID;
                                    lstComProduct.Add(objComProdt);
                                    //xóa từng sản phẩm và chiết khấu của đối thủ trong bảng sản phẩm và chiết khấu của đối thủ
                                    compdtDao.Delete(objProCom.ID);

                                }
                            }

                        }
                    }

                    //Thêm sản phẩm và chiết khấu của đối thủ vào CSDL


                    foreach (CompetiorProduct objCPDT in lstComProduct)
                    {
                        compdtDao.Insert(objCPDT);
                    }

                  
                    SetAlert("Cập nhật thành công", "success");
                    return RedirectToAction("Details", "Project", new  { id = id });
                        // return View();
                    }
                    else
                    {
                        SetAlert("Không thêm được", "danger");
                        return View();
                    }
                }
                else
                {
                    SetAlert("Không thêm được", "danger");
                    return View();
                }
            }
            catch
            {
                SetAlert("Không cập nhật được", "danger");
                return View();
            }
        }

        // GET: Project/Edit/5
        public ActionResult EditMNG(long id)
        {
            ProjectDao bdDao = new ProjectDao();
            SetViewBag();
            ViewBag.Project = bdDao.FindByID(id);
            //if (ViewBag.Project.Status > 2)
            //{
            //    SetAlert("Dự án đã được duyệt!", Common.CommonConstant.ALERT_WARNING);
            //    return RedirectToAction("Index", "Home", null);
            //}
            UserLogin us = (UserLogin)Session[CommonConstant.USER_SESSION];
          

            //Kiểm tra quyền truy cập của lạnh đạo
            bool inGroup = false;
            GroupUserDao gru = new GroupUserDao();
            //Kiểm tra theo user có thuộc nhóm lãnh đạo kho
            Guid grid = new Guid("964D283D-BEA0-4D85-B7C0-355487A5DF0C");
            if (gru.FiindByID(grid, us.UserID) != null)
            {
                inGroup = true;
            }
           
            if (inGroup == false)
            {
                SetAlert("Bạn không có quyền sửa dự án", Common.CommonConstant.ALERT_DANGER);
                return RedirectToAction("Details", "Project", new { id = id });
            }
            SetViewBag(ViewBag.Project.CityID);
            SetViewBagDistrict(ViewBag.Project.DistrictID, ViewBag.Project.CityID);
            SetCatagoryBag(ViewBag.Project.CategoryID);
            SetResourceIDViewBag(ViewBag.Project.ResourceID);
            SetPriceIDViewBag(ViewBag.Project.PriceID);
            SetViewSupplier(ViewBag.Project.SupplierID);
            ProjectUserDao usDao = new ProjectUserDao();
            List<ProjectUser> lstUP = usDao.FindByProjectID(ViewBag.Project.ProjectID);
            List<string> lstUPlogin = new List<string>();
            foreach (var pUs in lstUP)
            {
                //string sLogin = pUs.LoginID.ToString();
                lstUPlogin.Add(pUs.LoginID.ToString());
            }
            SetUserBag(lstUPlogin.ToArray<string>());
            ContratorDao contrDao = new ContratorDao();
            Contrator objConTra = contrDao.FindByID(ViewBag.Project.ContratorID);
            string str = "<p><b>Tên chủ đầu tư: </b>" + objConTra.ContraName + "</p>";
            str += "<p><b>Địa chỉ: </b>" + objConTra.Address + "</p>";
            str += "<p><b>Thông tin liên hệ: </b>" + objConTra.FullName + "<b> &nbsp;&nbsp;&nbsp;  Điện thoại: </b>" + objConTra.Phone + "</p>";
            ViewBag.PrContraDetail = str;
            ViewBag.PrContraCode = objConTra.ContratorID;
            BuilderDao buiDao = new BuilderDao();
            Builder objBuilder = buiDao.FindByID(ViewBag.Project.BuilderID);
            ViewBag.PrBuiderCode = objBuilder.BuilderID;
            str = "";
            str = "<p><b>Tên nhà thầu: </b>" + objBuilder.BuilderName + "</p>";
            str += "<p><b>Địa chỉ: </b>" + objBuilder.Address + "</p>";
            str += "<p><b>Thông tin liên hệ: </b>" + objBuilder.FullName + "<b>&nbsp;&nbsp; &nbsp; Điện thoại: </b>" + objBuilder.Phone + "</p>";
            ViewBag.BuiderDetail = str;

            ProjectProductDao prProdDao = new ProjectProductDao();

            ViewBag.lstProjectProdut = prProdDao.FindByID(ViewBag.Project.ProjectID);

            ProjectCompetitorDao prComDao = new ProjectCompetitorDao();
            ViewBag.lstProjectCompe = prComDao.FindByID(ViewBag.Project.ProjectID);
            List<string> lstCompeID = new List<string>();
            foreach (var compeId in ViewBag.lstProjectCompe)
            {
                lstCompeID.Add(compeId.CompetiorID.ToString());
            }
            SetCompetitorViewBag(lstCompeID.ToArray<string>());

            CompetiorProductDao comProductDao = new CompetiorProductDao();
            List<ProjectCompeProduct> lstProComProd = new List<ProjectCompeProduct>();
            foreach (var prcompe in ViewBag.lstProjectCompe)
            {
                var lstComProduct = comProductDao.FindByID(prcompe.ID);
                foreach (var comPro in lstComProduct)
                {
                    ProjectCompeProduct pcp = new ProjectCompeProduct();
                    pcp.ID = prcompe.ID;
                    pcp.CompetiorID = prcompe.CompetiorID;
                    pcp.ProjectID = prcompe.ProjectID;
                    pcp.ProductID = comPro.ProductID;
                    pcp.Discount = comPro.Discount;
                    pcp.DiscountVAT = comPro.DiscountVAT;
                    lstProComProd.Add(pcp);
                }

            }

            ViewBag.lstComeProduct = lstProComProd;
            ProcessDao prcessDao = new ProcessDao();
            ViewBag.Messege = prcessDao.GetListProjectProcessMessege(id).Count.ToString();
            FeedbackDao feedDao = new FeedbackDao();
            ViewBag.Feedback = feedDao.ToListByProjectID(id).Count.ToString();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditMNG(FormCollection data)
        {

            try
            {
                ProjectDao bdDao = new ProjectDao();
                long id = Convert.ToInt64(data["hdIDProject"].ToString());
                ViewBag.Project = bdDao.FindByID(id);
                SetViewBag();
                if (ModelState.IsValid)
                {
                    bool kt = true;
                   
                    Project objProject = bdDao.FindByID(id);
                    string cityID = data["CityID"].ToString();
                    SetViewBag(cityID);
                    long categoryID = Convert.ToInt64(data["CategoryID"].ToString());
                    SetCatagoryBag(categoryID);
                    long priceID = Convert.ToInt64(data["PriceID"].ToString());
                    SetPriceIDViewBag(priceID);
                    SetViewBagDistrict(data["DistrictID"], data["CityID"]);
                    SetResourceIDViewBag(Convert.ToInt64(data["ResourceID"]));
                    SetPriceIDViewBag(Convert.ToInt64(data["PriceID"]));
                    SetViewSupplier(Convert.ToInt64(data["drlSupplier"]));
                    string[] members = data.GetValues("drbMember");

                    SetUserBag(members);


                    //  string projectCode = data["txtCode"].ToString();
                    string name = data["Name"].ToString();
                    string address = data["Address"].ToString();
                    string contratorID = data["txtContratorID"].ToString();
                    string builderID = data["txtBuilder"].ToString();
                    string[] lstproductID = data.GetValues("cblProduct");
                    //string IsGroup = data["IsGroup"].ToString();
                    //string IsPublic = data["IsPublic"].ToString();
                    //Kiem tra ma chu dau tu
                    ContratorDao contraDAO = new ContratorDao();
                    Contrator objContra = contraDAO.FindByCode(contratorID.Trim());
                    if (objContra == null)
                    {
                        kt = false;
                        ModelState.AddModelError("", "Mã chủ đầu tư không đúng!");

                    }


                    BuilderDao buiderDao = new BuilderDao();
                    Builder objBuider = buiderDao.FindByCode(builderID.Trim());
                    if (objBuider == null)
                    {
                        kt = false;
                        ModelState.AddModelError("", "Mã nhà thầu thi công không đúng!");

                    }
                    long iSupplierID = Convert.ToInt64(data["drlSupplier"].ToString());
                    SetViewSupplier(iSupplierID);
                    //Danh sách  đối thủ cạnh tranh được chọn
                    string[] lstCompetitors = data.GetValues("drlCompetitor");
                    if (lstCompetitors == null)
                    {
                        kt = false;
                        ModelState.AddModelError("", "Bạn chưa chọn đối thủ cạnh tranh!");

                    }

                    // string a = Competitors[0].ToString();
                    SetCompetitorViewBag(lstCompetitors);
                    if (kt == true)
                    {



                        UserLogin us = (UserLogin)Session[CommonConstant.USER_SESSION];



                        objProject.ModifiedDate = Hepper.GetDateServer();

                        objProject.ModifiedBy = us.UserName;
                        objProject.Name = name;
                        //bool bPublic = true;
                        //if (IsPublic.Equals("false")) bPublic = false;
                        //objProject.IsPublic = bPublic;
                        //bool bGroup = true;
                        //if (IsGroup.Equals("false")) bGroup = false;
                        //objProject.IsGroup = bGroup;
                        objProject.PriceID = Convert.ToInt64(priceID);
                        objProject.ResourceID = Convert.ToInt64(data["ResourceID"].ToString());
                        objProject.CategoryID = categoryID;
                        objProject.CityID = cityID;
                        objProject.DistrictID = data["DistrictID"].ToString();
                        objProject.Description = data["txtDescription"].ToString();
                        objProject.Note = data["txtNote"].ToString();
                        objProject.NotePass = data["txtNotePass"].ToString();
                        objProject.Address = address;
                        //  objProject.Code = projectCode;
                        objProject.ContratorID = objContra.ID;
                        objProject.BuilderID = objBuider.ID;
                        objProject.SupplierID = iSupplierID;
                        decimal value = 0;
                        if (data["txtPriceProject"].ToString().Length > 0)
                        {
                            value = Convert.ToDecimal(data["txtPriceProject"].ToString());
                        }
                        objProject.Value = value;
                        // objProject.EndCreate = Convert.ToDateTime(data["EndCreate"].ToString());
                        //objProject.Status = 0;
                        // objProject.DateLine = Hepper.GetDateServer();
                        // objProject.StartDate = Hepper.GetDateServer();
                        //Thêm dự án vào CSDL
                        long projectID = bdDao.Update(objProject);

                        //thêm danh sách nhóm vào trong dự án
                        ProjectUserDao prUSDao = new ProjectUserDao();
                        //Xóa nhóm thuộc dự án
                        prUSDao.Delete(objProject.ProjectID);
                        ProjectUser objPrUS = new ProjectUser();
                        objPrUS.ProjectID = projectID;
                        objPrUS.LoginID = us.UserID;
                        objPrUS.IsAdmin = true;
                        prUSDao.Insert(objPrUS);
                        foreach (string sUsID in members)
                        {
                            long usID = Convert.ToInt64(sUsID);
                            if (usID != us.UserID)
                            {
                                ProjectUser objPrUSM = new ProjectUser();
                                objPrUSM.ProjectID = projectID;
                                objPrUSM.LoginID = usID;
                                objPrUSM.IsAdmin = false;
                                prUSDao.Insert(objPrUSM);
                            }
                        }
                        //Lấy danh sách sản phẩm được chọn với chiết khấu và giá kèm theo
                        List<ProjectProduct> lstProductPrject = new List<ProjectProduct>();
                        if (lstproductID.Length > 0)
                        {
                            decimal priceProduct = 0;
                            double discoutProdct = 0;
                            double discoutProdctVAT = 0;
                            long productID = 0;
                            foreach (string sprodID in lstproductID)
                            {
                                productID = Convert.ToInt64(sprodID);
                                string txtPrice = "txtPrice" + sprodID;
                                string txtDiscount = "txtDiscount" + sprodID;
                                string txtDiscountVAT = "txtDiscountVAT" + sprodID;
                                if (data[txtPrice].ToString() != null)
                                {
                                    priceProduct = Convert.ToDecimal(data[txtPrice].ToString());
                                }
                                if (data[txtDiscount].ToString() != null)
                                {
                                    discoutProdct = Convert.ToDouble(data[txtDiscount].ToString());
                                }
                                if (data[txtDiscountVAT].ToString() != null)
                                {
                                    discoutProdctVAT = Convert.ToDouble(data[txtDiscountVAT].ToString());
                                }
                                ProjectProduct objProPrd = new ProjectProduct();
                                objProPrd.ProductID = productID;
                                objProPrd.Price = priceProduct;
                                objProPrd.ProjectID = projectID;
                                objProPrd.Discount = discoutProdct;
                                objProPrd.DiscountVAT = discoutProdctVAT;
                                lstProductPrject.Add(objProPrd);
                            }
                        }
                        //Thêm sản phẩm của dự án với giá và chiết khấu vào CSDL
                        ProjectProductDao ppdtDao = new ProjectProductDao();
                        //Xóa sản phẩm đã tồn tại của dự án
                        ppdtDao.Delete(objProject.ProjectID);
                        foreach (ProjectProduct prpd in lstProductPrject)
                        {
                            ppdtDao.Insert(prpd);
                        }


                        //Lấy danh sách chiết khấu giá của sản phẩm mà đối thủ cạnh tranh
                        List<ProjectCompetitor> lstprojectConpetitor = new List<ProjectCompetitor>();
                        ProjectCompetitorDao prcDao = new ProjectCompetitorDao();
                        //Xóa danh sách các đối thủ cạnh tranh của dự án
                        prcDao.DeleteByProjectID(objProject.ProjectID);
                        if (lstCompetitors.Length > 0)
                        {
                            long competitorID = 0;
                            foreach (string scpID in lstCompetitors)
                            {
                                competitorID = Convert.ToInt64(scpID);

                                ProjectCompetitor objProCompt = new ProjectCompetitor();
                                objProCompt.CompetiorID = competitorID;
                                //objProCompt.ID = 0;
                                objProCompt.ProjectID = projectID;
                                // Insert vào Bang lấy được lại ID cho vào danh sách
                                objProCompt.ID = prcDao.Insert(objProCompt);
                                lstprojectConpetitor.Add(objProCompt);
                            }
                        }

                        //Kiểm tra những sản phẩm thuộc đối thủ đã chọn lấy chiết khấu của sản phẩm đấy
                        List<CompetiorProduct> lstComProduct = new List<CompetiorProduct>();
                        CompetiorProductDao compdtDao = new CompetiorProductDao();
                        foreach (ProjectCompetitor objProCom in lstprojectConpetitor)
                        {
                            //Lấy danh sách các sản phẩm của từng đối thủ được chọn rồi lấy chiết khấu của sản phẩm đấy
                            string[] lstProdctCom;
                            string drlProuctCom = "cblProduct" + objProCom.CompetiorID;
                            lstProdctCom = data.GetValues(drlProuctCom);
                            if (lstProdctCom.Length > 0)
                            {
                                double discount = 0;
                                double discountVAT = 0;
                                long prID = 0;
                                foreach (string sProID in lstProdctCom)
                                {
                                    string txtDiscount = "txtDiscount" + objProCom.CompetiorID + "_" + sProID;
                                    string txtDiscountVAT = "txtDiscountVAT" + objProCom.CompetiorID + "_" + sProID;
                                    if (data[txtDiscountVAT].ToString() != null)
                                    {
                                        discountVAT = Convert.ToDouble(data[txtDiscountVAT].ToString());
                                    }
                                    if (data[txtDiscount].ToString() != null)
                                    {
                                        discount = Convert.ToDouble(data[txtDiscount].ToString());

                                        prID = Convert.ToInt64(sProID);
                                        CompetiorProduct objComProdt = new CompetiorProduct();
                                        objComProdt.Discount = discount;
                                        objComProdt.DiscountVAT = discountVAT;
                                        objComProdt.ID = objProCom.ID;
                                        objComProdt.ProductID = prID;
                                        lstComProduct.Add(objComProdt);
                                        //xóa từng sản phẩm và chiết khấu của đối thủ trong bảng sản phẩm và chiết khấu của đối thủ
                                        compdtDao.Delete(objProCom.ID);

                                    }
                                }

                            }
                        }

                        //Thêm sản phẩm và chiết khấu của đối thủ vào CSDL


                        foreach (CompetiorProduct objCPDT in lstComProduct)
                        {
                            compdtDao.Insert(objCPDT);
                        }

                        //if (bdDao.Insert(collection) > 0)
                        //{
                        //    SetAlert("Thêm thành công", "success");
                        //    return RedirectToAction("Index");
                        //}
                        //else
                        //{
                        //    SetAlert("Không thêm được", "danger");
                        //}
                        SetAlert("Cập nhật thành công", "success");
                        return RedirectToAction("Index", "Home");
                        // return View();
                    }
                    else
                    {

                        SetAlert("Không cập nhật được", "danger");
                        return View();

                    }
                }
                else
                {

                    SetAlert("Không cập nhật được", "danger");
                    return View();

                }
            }
            catch
            {
                SetAlert("Không cập nhật được", "danger");
                return View();
            }
        }

        // POST: Builders/Delete/5
        [HttpDelete]
        public ActionResult Delete(long id)
        {
            try
            {
                // TODO: Add delete logic here

                ProjectDao bdDao = new ProjectDao();

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
        public void SetViewBag(string selectedId = null)
        {
            var dao = new CityDao();
            var condao = new ContratorDao();
            var builderdao = new BuilderDao();
            var productdao = new ProductDao();
            ViewBag.CityID = new SelectList(dao.ToList(), "CityID", "Name", selectedId);
            ViewBag.Contrator = new SelectList(condao.ToListActive(), "ContratorID", "ContraName", selectedId);

            ViewBag.Builder = new SelectList(builderdao.ToListActive(), "BuilderID", "BuilderName", selectedId);
            ViewBag.Products = productdao.ToListActive();
        }
        public void SetCatagoryBag(long? selectedId = null)
        {
            var dao = new CategoryDao();
            ViewBag.CategoryID = new SelectList(dao.ToList(), "CategoryID", "Name", selectedId);
        }
        public void SetUserBag(string[] selectedId = null)
        {
            var dao = new UserDao();
            ViewBag.Member = new MultiSelectList(dao.ToList(), "LoginID", "FullName", selectedId);
        }
        public void SetResourceIDViewBag(long? selectedId = null)
        {
            var dao = new ResourceDao();
            ViewBag.ResourceID = new SelectList(dao.ToList(), "ResourceID", "Name", selectedId);
        }
        public void SetPriceIDViewBag(long? selectedId = null)
        {
            var dao = new PriceDao();
            ViewBag.PriceID = new SelectList(dao.ToList(), "PriceID", "Name", selectedId);
        }
        public void SetCompetitorViewBag(string[] selectedId = null)
        {
            var dao = new CompetitorDao();
            ViewBag.Competitor = new MultiSelectList(dao.ToListActive(), "ID", "CompetitorName", selectedId);
            ViewBag.ListCompetitor = dao.ToListActive();
        }

        public void SetViewBagDistrict(string selectedId = null, string cityid = null)
        {
            var dao = new DistrictDao();
            if (cityid != null)
            {
                ViewBag.DistrictID = new SelectList(dao.FindByID(cityid), "DistrictCode", "Name", selectedId);
            }
            else
                ViewBag.DistrictID = new SelectList(dao.ToList(), "DistrictCode", "Name", selectedId);
        }
        public void SetViewSupplier(long? selectedId = null, string cityid = null, string districtid = null)
        {
            var dao = new SupplierDao();
            if (cityid != null)
            {
                ViewBag.Supplier = new SelectList(dao.FindByDistrist(cityid, districtid), "ID", "SupplierName", selectedId);
            }
            else
                ViewBag.Supplier = new SelectList(dao.ToList(), "ID", "SupplierName", selectedId);
        }
        [HttpPost]
        public JsonResult ChangeDistrict(string cityid)
        {

            var dao = new DistrictDao();
            var list = dao.FindByID(cityid);
            JsonResult result = new JsonResult();
            result.Data = list;
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }
        [HttpPost]
        public JsonResult ChangeSupeelier(string cityid)
        {

            var dao = new SupplierDao();
            var list = dao.FindByCity(cityid);
            JsonResult result = new JsonResult();
            result.Data = list;
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        [HttpPost]
        public JsonResult CreateCode(string code, int lengh)
        {

            var dao = new ProjectDao();
            var list = dao.GenaraCode(code, lengh);
            JsonResult result = new JsonResult();
            result.Data = list;
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }
        [HttpPost]
        public JsonResult ContratorDT(string code)
        {

            var dao = new ContratorDao();
            var list = dao.FindByCode(code);
            JsonResult result = new JsonResult();
            result.Data = list;
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }
        [HttpPost]
        public JsonResult BuilderDT(string code)
        {

            var dao = new BuilderDao();
            var list = dao.FindByCode(code);
            JsonResult result = new JsonResult();
            result.Data = list;
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

    }
}
