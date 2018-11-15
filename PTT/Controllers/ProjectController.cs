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
            //ProjectDao bdDao = new ProjectDao();
            //ViewBag.ProjectGroup = bdDao.ToList();
            UserLogin user = (UserLogin)Session[CommonConstant.USER_SESSION];
            db = new PTTDataContext();
            ViewBag.ProjectGroup = from pr in db.Projects
                                   join us in db.Users on pr.CreateBy equals us.UserName
                                   orderby pr.ProjectID ascending
                                   //where pr.Status == 0
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
        [HttpGet]
        public ActionResult End(long id)
        {
            ProjectDao bdDao = new ProjectDao();
            SetViewBag();
            ViewBag.Project = bdDao.FindByID(id);
            SetViewBagCity(ViewBag.Project.CityID);
         
            SetViewBagDistrict(ViewBag.Project.DistrictID, ViewBag.Project.CityID);
            SetCatagoryBag(ViewBag.Project.CategoryID);
            SetResourceIDViewBag(ViewBag.Project.ResourceID);
            SetPriceIDViewBag(ViewBag.Project.PriceID);
          
            UserLogin us = (UserLogin)Session[CommonConstant.USER_SESSION];
            ProjectUserDao puDao = new ProjectUserDao();

            //Kiểm tra quyền truy cập của lạnh đạo
            bool inGroup = false;
            //GroupUserDao gru = new GroupUserDao();
            ////Kiểm tra theo user có thuộc nhóm lãnh đạo kho
            //Guid grid = new Guid("964D283D-BEA0-4D85-B7C0-355487A5DF0C");
            //if (gru.FiindByID(grid, us.UserID) != null)
            //{
            //    inGroup = true;
            //}
            //Kiểm tra theo user có thuộc nhóm trong dự án ko
            ProjectUser objPU = puDao.FindByID(us.UserID, id);
            if (objPU != null)
            {
                inGroup = true;

            }
            if (inGroup == false)
            {
                SetAlert("Bạn không có quyền yêu cầu kết thúc dự án", Common.CommonConstant.ALERT_DANGER);
                return RedirectToAction("Details", "Project", new { id = id });
            }
            if (ViewBag.Project.Status != 1)
            {
                SetAlert("Dự đã kết thúc!", Common.CommonConstant.ALERT_WARNING);
                return RedirectToAction("Details", "Project", new { id = id });
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


            //Lấy danh sách nhà cung ứng 
            ProjectSupplierDao psDao = new ProjectSupplierDao();
            List<ProjectSupplier> lsPSP = psDao.FindByID(ViewBag.Project.ProjectID);

            List<string> lstPS = new List<string>();
            foreach (var sid in lsPSP)
            {
                //string sLogin = pUs.LoginID.ToString();
                lstPS.Add(sid.SupplierID.ToString());
            }
            SetViewSupplier(lstPS.ToArray<string>());


            //Lấy danh sách chủ đầu tư
            ProjectContratorDao pctrDao = new ProjectContratorDao();
            List<ProjectContrator> lstPCTR = pctrDao.FindByID(ViewBag.Project.ProjectID);

            List<string> lstPCT = new List<string>();
            foreach (var contrid in lstPCTR)
            {
                //string sLogin = pUs.LoginID.ToString();
                lstPCT.Add(contrid.ContratorID.ToString());
            }
            SetContratorViewBag(lstPCT.ToArray<string>());

            //Lấy danh sách nhà thầu thi công
            ProjectBuilderDao pbdDao = new ProjectBuilderDao();
            List<ProjectBuilder> lsPBD = pbdDao.FindByID(ViewBag.Project.ProjectID);

            List<string> lstPD = new List<string>();
            foreach (var bdid in lsPBD)
            {
                //string sLogin = pUs.LoginID.ToString();
                lstPD.Add(bdid.BuilderID.ToString());
            }
            SetBuilderViewBag(lstPD.ToArray<string>());
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
            ViewBag.Messege = prcessDao.CountProcessMessage(id).ToString();
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
            SetViewBagCity(ViewBag.Project.CityID);
          
            SetViewBagDistrict(ViewBag.Project.DistrictID, ViewBag.Project.CityID);
            SetCatagoryBag(ViewBag.Project.CategoryID);
            SetResourceIDViewBag(ViewBag.Project.ResourceID);
            SetPriceIDViewBag(ViewBag.Project.PriceID);
           
            ProjectUserDao usDao = new ProjectUserDao();
            List<ProjectUser> lstUP = usDao.FindByProjectID(ViewBag.Project.ProjectID);
            List<string> lstUPlogin = new List<string>();
            foreach (var pUs in lstUP)
            {
                //string sLogin = pUs.LoginID.ToString();
                lstUPlogin.Add(pUs.LoginID.ToString());
            }
            SetUserBag(lstUPlogin.ToArray<string>());

            //Lấy danh sách nhà cung ứng 
            ProjectSupplierDao psDao = new ProjectSupplierDao();
            List<ProjectSupplier> lsPSP = psDao.FindByID(ViewBag.Project.ProjectID);

            List<string> lstPS = new List<string>();
            foreach (var sid in lsPSP)
            {
                //string sLogin = pUs.LoginID.ToString();
                lstPS.Add(sid.SupplierID.ToString());
            }
            SetViewSupplier(lstPS.ToArray<string>());


            //Lấy danh sách chủ đầu tư
            ProjectContratorDao pctrDao = new ProjectContratorDao();
            List<ProjectContrator> lstPCTR = pctrDao.FindByID(ViewBag.Project.ProjectID);

            List<string> lstPCT = new List<string>();
            foreach (var contrid in lstPCTR)
            {
                //string sLogin = pUs.LoginID.ToString();
                lstPCT.Add(contrid.ContratorID.ToString());
            }
            SetContratorViewBag(lstPCT.ToArray<string>());

            //Lấy danh sách nhà thầu thi công
            ProjectBuilderDao pbdDao = new ProjectBuilderDao();
            List<ProjectBuilder> lsPBD = pbdDao.FindByID(ViewBag.Project.ProjectID);

            List<string> lstPD = new List<string>();
            foreach (var bdid in lsPBD)
            {
                //string sLogin = pUs.LoginID.ToString();
                lstPD.Add(bdid.BuilderID.ToString());
            }
            SetBuilderViewBag(lstPD.ToArray<string>());

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
            ViewBag.Messege = prcessDao.CountProcessMessage(id).ToString();
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
                return RedirectToAction("ProjectUserWait", "Home", null);
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
            SetViewBagCity(ViewBag.Project.CityID);

            SetViewBagDistrict(ViewBag.Project.DistrictID, ViewBag.Project.CityID);
            SetCatagoryBag(ViewBag.Project.CategoryID);
            SetResourceIDViewBag(ViewBag.Project.ResourceID);
            SetPriceIDViewBag(ViewBag.Project.PriceID);

          
            ProjectUserDao usDao = new ProjectUserDao();
            List<ProjectUser> lstUP = usDao.FindByProjectID(ViewBag.Project.ProjectID);
            List<string> lstUPlogin = new List<string>();
            foreach (var pUs in lstUP)
            {
                //string sLogin = pUs.LoginID.ToString();
                lstUPlogin.Add(pUs.LoginID.ToString());
            }
            SetUserBag(lstUPlogin.ToArray<string>());

            //Lấy danh sách nhà cung ứng 
            ProjectSupplierDao psDao = new ProjectSupplierDao();
            List<ProjectSupplier> lsPSP = psDao.FindByID(ViewBag.Project.ProjectID);

            List<string> lstPS = new List<string>();
            foreach (var sid in lsPSP)
            {
                //string sLogin = pUs.LoginID.ToString();
                lstPS.Add(sid.SupplierID.ToString());
            }
            SetViewSupplier(lstPS.ToArray<string>());


            //Lấy danh sách chủ đầu tư
            ProjectContratorDao pctrDao = new ProjectContratorDao();
            List<ProjectContrator> lstPCTR = pctrDao.FindByID(ViewBag.Project.ProjectID);

            List<string> lstPCT = new List<string>();
            foreach (var contrid in lstPCTR)
            {
                //string sLogin = pUs.LoginID.ToString();
                lstPCT.Add(contrid.ContratorID.ToString());
            }
            SetContratorViewBag(lstPCT.ToArray<string>());

            //Lấy danh sách nhà thầu thi công
            ProjectBuilderDao pbdDao = new ProjectBuilderDao();
            List<ProjectBuilder> lsPBD = pbdDao.FindByID(ViewBag.Project.ProjectID);

            List<string> lstPD = new List<string>();
            foreach (var bdid in lsPBD)
            {
                //string sLogin = pUs.LoginID.ToString();
                lstPD.Add(bdid.BuilderID.ToString());
            }
            SetBuilderViewBag(lstPD.ToArray<string>());

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
            ViewBag.Messege = prcessDao.CountProcessMessage(id).ToString();
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
           
            ProjectUserDao usDao = new ProjectUserDao();
            List<ProjectUser> lstUP = usDao.FindByProjectID(ViewBag.Project.ProjectID);
            List<string> lstUPlogin = new List<string>();
            foreach (var pUs in lstUP)
            {
                //string sLogin = pUs.LoginID.ToString();
                lstUPlogin.Add(pUs.LoginID.ToString());
            }
            SetUserBag(lstUPlogin.ToArray<string>());

            //Lấy danh sách nhà cung ứng 
            ProjectSupplierDao psDao = new ProjectSupplierDao();
            List<ProjectSupplier> lsPSP = psDao.FindByID(ViewBag.Project.ProjectID);

            List<string> lstPS = new List<string>();
            foreach (var sid in lsPSP)
            {
                //string sLogin = pUs.LoginID.ToString();
                lstPS.Add(sid.SupplierID.ToString());
            }
            SetViewSupplier(lstPS.ToArray<string>());


            //Lấy danh sách chủ đầu tư
            ProjectContratorDao pctrDao = new ProjectContratorDao();
            List<ProjectContrator> lstPCTR = pctrDao.FindByID(ViewBag.Project.ProjectID);

            List<string> lstPCT = new List<string>();
            foreach (var contrid in lstPCTR)
            {
                //string sLogin = pUs.LoginID.ToString();
                lstPCT.Add(contrid.ContratorID.ToString());
            }
            SetContratorViewBag(lstPCT.ToArray<string>());

            //Lấy danh sách nhà thầu thi công
            ProjectBuilderDao pbdDao = new ProjectBuilderDao();
            List<ProjectBuilder> lsPBD = pbdDao.FindByID(ViewBag.Project.ProjectID);

            List<string> lstPD = new List<string>();
            foreach (var bdid in lsPBD)
            {
                //string sLogin = pUs.LoginID.ToString();
                lstPD.Add(bdid.BuilderID.ToString());
            }
            SetBuilderViewBag(lstPD.ToArray<string>());

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
            ViewBag.Messege = prcessDao.CountProcessMessage(id).ToString();
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
            SetViewBagCity(ViewBag.Project.CityID);
           
            SetViewBagDistrict(ViewBag.Project.DistrictID, ViewBag.Project.CityID);
            SetCatagoryBag(ViewBag.Project.CategoryID);
            SetResourceIDViewBag(ViewBag.Project.ResourceID);
            SetPriceIDViewBag(ViewBag.Project.PriceID);
          
            ProjectUserDao usDao = new ProjectUserDao();
            List<ProjectUser> lstUP = usDao.FindByProjectID(ViewBag.Project.ProjectID);
            List<string> lstUPlogin = new List<string>();
            foreach (var pUs in lstUP)
            {
                //string sLogin = pUs.LoginID.ToString();
                lstUPlogin.Add(pUs.LoginID.ToString());
            }
            SetUserBag(lstUPlogin.ToArray<string>());


            //Lấy danh sách nhà cung ứng 
            ProjectSupplierDao psDao = new ProjectSupplierDao();
            List<ProjectSupplier> lsPSP = psDao.FindByID(ViewBag.Project.ProjectID);

            List<string> lstPS = new List<string>();
            foreach (var sid in lsPSP)
            {
                //string sLogin = pUs.LoginID.ToString();
                lstPS.Add(sid.SupplierID.ToString());
            }
            SetViewSupplier(lstPS.ToArray<string>());


            //Lấy danh sách chủ đầu tư
            ProjectContratorDao pctrDao = new ProjectContratorDao();
            List<ProjectContrator> lstPCTR = pctrDao.FindByID(ViewBag.Project.ProjectID);

            List<string> lstPCT = new List<string>();
            foreach (var contrid in lstPCTR)
            {
                //string sLogin = pUs.LoginID.ToString();
                lstPCT.Add(contrid.ContratorID.ToString());
            }
            SetContratorViewBag(lstPCT.ToArray<string>());

            //Lấy danh sách nhà thầu thi công
            ProjectBuilderDao pbdDao = new ProjectBuilderDao();
            List<ProjectBuilder> lsPBD = pbdDao.FindByID(ViewBag.Project.ProjectID);

            List<string> lstPD = new List<string>();
            foreach (var bdid in lsPBD)
            {
                //string sLogin = pUs.LoginID.ToString();
                lstPD.Add(bdid.BuilderID.ToString());
            }
            SetBuilderViewBag(lstPD.ToArray<string>());

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
            ViewBag.Messege = prcessDao.CountProcessMessage(id).ToString();
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
            SetViewBagCity(ViewBag.Project.CityID);
          
            SetViewBagDistrict(ViewBag.Project.DistrictID, ViewBag.Project.CityID);
            SetCatagoryBag(ViewBag.Project.CategoryID);
            SetResourceIDViewBag(ViewBag.Project.ResourceID);
            SetPriceIDViewBag(ViewBag.Project.PriceID);
           
            ProjectUserDao usDao = new ProjectUserDao();
            List<ProjectUser> lstUP = usDao.FindByProjectID(ViewBag.Project.ProjectID);
            List<string> lstUPlogin = new List<string>();
            foreach (var pUs in lstUP)
            {
                //string sLogin = pUs.LoginID.ToString();
                lstUPlogin.Add(pUs.LoginID.ToString());
            }
            SetUserBag(lstUPlogin.ToArray<string>());
            //Lấy danh sách nhà cung ứng 
            ProjectSupplierDao psDao = new ProjectSupplierDao();
            List<ProjectSupplier> lsPSP = psDao.FindByID(ViewBag.Project.ProjectID);

            List<string> lstPS = new List<string>();
            foreach (var sid in lsPSP)
            {
                //string sLogin = pUs.LoginID.ToString();
                lstPS.Add(sid.SupplierID.ToString());
            }
            SetViewSupplier(lstPS.ToArray<string>());


            //Lấy danh sách chủ đầu tư
            ProjectContratorDao pctrDao = new ProjectContratorDao();
            List<ProjectContrator> lstPCTR = pctrDao.FindByID(ViewBag.Project.ProjectID);

            List<string> lstPCT = new List<string>();
            foreach (var contrid in lstPCTR)
            {
                //string sLogin = pUs.LoginID.ToString();
                lstPCT.Add(contrid.ContratorID.ToString());
            }
            SetContratorViewBag(lstPCT.ToArray<string>());

            //Lấy danh sách nhà thầu thi công
            ProjectBuilderDao pbdDao = new ProjectBuilderDao();
            List<ProjectBuilder> lsPBD = pbdDao.FindByID(ViewBag.Project.ProjectID);

            List<string> lstPD = new List<string>();
            foreach (var bdid in lsPBD)
            {
                //string sLogin = pUs.LoginID.ToString();
                lstPD.Add(bdid.BuilderID.ToString());
            }
            SetBuilderViewBag(lstPD.ToArray<string>());

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
            ViewBag.Messege = prcessDao.CountProcessMessage(id).ToString();
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
            SetViewBagCity(ViewBag.Project.CityID);
          
            SetViewBagDistrict(ViewBag.Project.DistrictID, ViewBag.Project.CityID);
            SetCatagoryBag(ViewBag.Project.CategoryID);
            SetResourceIDViewBag(ViewBag.Project.ResourceID);
            SetPriceIDViewBag(ViewBag.Project.PriceID);
          
            ProjectUserDao usDao = new ProjectUserDao();
            List<ProjectUser> lstUP = usDao.FindByProjectID(ViewBag.Project.ProjectID);
            List<string> lstUPlogin = new List<string>();
            foreach (var pUs in lstUP)
            {
                //string sLogin = pUs.LoginID.ToString();
                lstUPlogin.Add(pUs.LoginID.ToString());
            }
            SetUserBag(lstUPlogin.ToArray<string>());

            //Lấy danh sách nhà cung ứng 
            ProjectSupplierDao psDao = new ProjectSupplierDao();
            List<ProjectSupplier> lsPSP = psDao.FindByID(ViewBag.Project.ProjectID);

            List<string> lstPS = new List<string>();
            foreach (var sid in lsPSP)
            {
                //string sLogin = pUs.LoginID.ToString();
                lstPS.Add(sid.SupplierID.ToString());
            }
            SetViewSupplier(lstPS.ToArray<string>());


            //Lấy danh sách chủ đầu tư
            ProjectContratorDao pctrDao = new ProjectContratorDao();
            List<ProjectContrator> lstPCTR = pctrDao.FindByID(ViewBag.Project.ProjectID);

            List<string> lstPCT = new List<string>();
            foreach (var contrid in lstPCTR)
            {
                //string sLogin = pUs.LoginID.ToString();
                lstPCT.Add(contrid.ContratorID.ToString());
            }
            SetContratorViewBag(lstPCT.ToArray<string>());

            //Lấy danh sách nhà thầu thi công
            ProjectBuilderDao pbdDao = new ProjectBuilderDao();
            List<ProjectBuilder> lsPBD = pbdDao.FindByID(ViewBag.Project.ProjectID);

            List<string> lstPD = new List<string>();
            foreach (var bdid in lsPBD)
            {
                //string sLogin = pUs.LoginID.ToString();
                lstPD.Add(bdid.BuilderID.ToString());
            }
            SetBuilderViewBag(lstPD.ToArray<string>());

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
            ViewBag.Messege = prcessDao.CountProcessMessage(id).ToString();
            FeedbackDao feedDao = new FeedbackDao();
            ViewBag.Feedback = feedDao.ToListByProjectID(id).Count.ToString();
            return View();
        }

        // GET: Project/Create
        public ActionResult Create()
        {
            SetViewBag();
            SetViewBagCity();
            SetViewBagDistrict();
            SetViewBagCitySuplier();
            SetCatagoryBag();
            SetPriceIDViewBag();
            SetResourceIDViewBag();
            SetUserBag();
            SetCompetitorViewBag();
            SetContratorViewBag();
            SetViewSupplier();
            SetBuilderViewBag();
            ViewBag.Name = "";
            ViewBag.Address = "";
            ViewBag.EndCreate = "";
         //   ViewBag.ContratorID = "";
         //   ViewBag.BuilderID = "";
            return View();
        }

        // POST: Project/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormCollection data)
        {

            try

            {
                //SetViewSupplier();
                //SetContratorViewBag();
                //SetBuilderViewBag();
                //SetCompetitorViewBag();
                if (ModelState.IsValid)
                {

                    bool kt = true;
                    string cityID = data["drlCityID"].ToString();
                   
                    SetViewBag();
                    SetViewBagCity(cityID);
       
                    long categoryID = Convert.ToInt64(data["drlCategoryID"].ToString());
                    SetCatagoryBag(categoryID);
                    long priceID = Convert.ToInt64(data["drlPriceID"].ToString());
                    SetPriceIDViewBag(priceID);
                    SetViewBagDistrict(data["drlDistrict"], data["drlCityID"]); 
                    SetResourceIDViewBag(Convert.ToInt64(data["drlResourceID"]));
                    SetPriceIDViewBag(Convert.ToInt64(data["drlPriceID"]));
                    string[] members = data.GetValues("drbMember");
                  
                    //Danh sách  đối thủ cạnh tranh được chọn
                    string[] lstCompetitors = data.GetValues("drlCompetitor");
                    SetCompetitorViewBag(lstCompetitors);

                    SetUserBag(members);
                    //Danh sách nhà cung ứng
                    string[] iSupplierID = data.GetValues("drlSupplier");
                    SetViewSupplier(iSupplierID);

                    //Danh sách nhà thầu thi công
                    string[] lstBuilders = data.GetValues("drlBuilder");
                    SetBuilderViewBag(lstBuilders);
                    //Danh sách chủ đầu tư
                    string[] lstContrators = data.GetValues("drlContrator");
                    SetContratorViewBag(lstContrators);

                    //  string projectCode = data["txtCode"].ToString();
                    var dao = new ProjectDao();

                    string projectCode = dao.GenaraCode("BPTTT", 5);
                    string name = data["Name"].ToString();
                    string address = data["Address"].ToString();
                    
                 
                  //  string builderID = data["txtBuilder"].ToString();
                    string[] lstproductID = data.GetValues("cblProduct");
                   

                    ViewBag.Address = address;
                    ViewBag.Name = name;
                    ViewBag.EndCreate = data["EndCreate"].ToString();
                    //   ViewBag.ContratorID = contratorID;
                    //  ViewBag.BuilderID = lstBuilders;
                    //string IsGroup = data["IsGroup"].ToString();
                    //string IsPublic = data["IsPublic"].ToString();

                    //Kiểm tra nhà cung ứng
                    if (iSupplierID == null)
                    {
                        kt = false;
                        ModelState.AddModelError("", "Bạn phải chọn nhà cung ứng!");

                    }
                    if (lstContrators == null)
                    {
                        kt = false;
                        ModelState.AddModelError("", "Bạn phải chọn chủ đầu tư!");

                    }
                    if (lstBuilders == null)
                    {
                        kt = false;
                        ModelState.AddModelError("", "Bạn phải chọn nhà thầu thi công!");

                    }

                    List<long> lstProductIDProject = new List<long>();
                    if (lstproductID == null)
                    {
                        kt = false;
                        ModelState.AddModelError("", "Bạn chưa chọn sản phẩm của dự án!");

                    }
                    else
                    {
                        //Kiểm tra dữ liệu của chiết khấu đã nhập chưa
                        try
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
                                lstProductIDProject.Add(productID);
                            }
                        }
                        catch
                        {
                            kt = false;
                            ModelState.AddModelError("", "Nhập giá và chiết khấu kiểu số tương ứng với sản phẩm dự án!");
                        }

                    }
                  
                    if (lstCompetitors != null)
                    {
                        foreach (string scpID in lstCompetitors)
                        {
                            //Lấy danh sách các sản phẩm của từng đối thủ được chọn rồi lấy chiết khấu của sản phẩm đấy
                            string[] lstProdctCom;
                            string drlProuctCom = "cblProduct" + scpID;
                            lstProdctCom = data.GetValues(drlProuctCom);
                            List<long> lstProductIDCompare = new List<long>();
                            try
                            {
                                if (lstProdctCom.Length > 0)
                                {
                                    double discount = 0;
                                    double discountVAT = 0;
                                    long prID = 0;
                                    foreach (string sProID in lstProdctCom)
                                    {
                                        prID = Convert.ToInt64(sProID);
                                        lstProductIDCompare.Add(prID);
                                        string txtDiscount = "txtDiscount" + scpID + "_" + sProID;
                                        string txtDiscountVAT = "txtDiscountVAT" + scpID + "_" + sProID;
                                        if (data[txtDiscountVAT].ToString() != null)
                                        {
                                            discountVAT = Convert.ToDouble(data[txtDiscountVAT].ToString());
                                        }
                                        if (data[txtDiscount].ToString() != null)
                                        {
                                            discount = Convert.ToDouble(data[txtDiscount].ToString());
                                        }
                                    }
                                    if (Hepper.compareList(lstProductIDProject, lstProductIDCompare) == false)
                                    {
                                        kt = false;
                                        ModelState.AddModelError("", "Bạn phải chọn sản phẩm của đối thủ cạnh tranh giống sản phẩm của dự án!");
                                        break;
                                    }
                                }
                            }
                            catch
                            {
                                kt = false;
                                ModelState.AddModelError("", "Nhập giá và chiết khấu kiểu số tương ứng với sản phẩm của đối thủ cạnh tranh!");
                            }



                        }

                    }

                  


                    UserLogin us = (UserLogin)Session[CommonConstant.USER_SESSION];
                    ProjectDao bdDao = new ProjectDao();
                    Project objProject = new Project();

                  //  objProject.SupplierID = iSupplierID;
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
                    objProject.ResourceID = Convert.ToInt64(data["drlResourceID"].ToString());
                    objProject.CategoryID = categoryID;
                    objProject.CityID = cityID;
                    objProject.DistrictID = data["drlDistrict"].ToString();
                    objProject.Description = data["txtDescription"].ToString();
                    objProject.Address = address;
                    objProject.Code = projectCode;
                    objProject.EndCreate = Convert.ToDateTime(data["EndCreate"].ToString());
                    objProject.Status = 0;
                    objProject.DateLine = Hepper.GetDateServer();
                    objProject.StartDate = Hepper.GetDateServer();
                    objProject.Value = 0;
                    //Thêm dự án vào CSDL
                    if (kt == true)
                    {
                       // objProject.ContratorID = objContra.ID;
                      //  objProject.BuilderID = (objBuider.ID);
                        long projectID = bdDao.Insert(objProject);

                        //thêm danh sách nhóm vào trong dự án
                        ProjectUserDao prUSDao = new ProjectUserDao();
                        ProjectUser objPrUS = new ProjectUser();
                        objPrUS.ProjectID = projectID;
                        objPrUS.LoginID = us.UserID;
                        objPrUS.IsAdmin = true;
                        prUSDao.Insert(objPrUS);
                        if (members != null)
                        {
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
                        }
                        //Thêm danh sách nhà cung ứng
                        if (iSupplierID != null)
                        {
                            ProjectSupplierDao prsDB = new ProjectSupplierDao();
                            foreach (string spID in iSupplierID)
                            {
                                ProjectSupplier objPS = new ProjectSupplier();
                                objPS.ProjectID = projectID;
                                objPS.SupplierID = long.Parse(spID);
                                prsDB.Insert(objPS);
                            }
                        }
                        //Thêm danh sách chủ đầu tư
                        if (lstContrators != null)
                        {
                            ProjectContratorDao pcDB = new ProjectContratorDao();
                            foreach (string ctID in lstContrators)
                            {
                                ProjectContrator objPS = new ProjectContrator();
                                objPS.ProjectID = projectID;
                                objPS.ContratorID = long.Parse(ctID);
                                pcDB.Insert(objPS);
                            }
                        }
                        //Thêm danh sách nhà thầu thi công
                        if (lstBuilders != null)
                        {
                            ProjectBuilderDao pbDB = new ProjectBuilderDao();
                            foreach (string bdID in lstBuilders)
                            {
                                ProjectBuilder objPS = new ProjectBuilder();
                                objPS.ProjectID = projectID;
                                objPS.BuilderID = long.Parse(bdID);
                                pbDB.Insert(objPS);
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

                        if (lstCompetitors != null)
                        {

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
            if (ViewBag.Project.Status >= 2)
            {
                SetAlert("Dự án đã được duyệt!", Common.CommonConstant.ALERT_WARNING);
                return RedirectToAction("Details", "Project", new { id = id });
            }


            SetViewBag();
            SetViewBagCity(ViewBag.Project.CityID);
            SetViewBagDistrict(ViewBag.Project.DistrictID, ViewBag.Project.CityID);
            SetCatagoryBag(ViewBag.Project.CategoryID);
            SetResourceIDViewBag(ViewBag.Project.ResourceID);
            SetPriceIDViewBag(ViewBag.Project.PriceID);
           
            ProjectUserDao usDao = new ProjectUserDao();
            List<ProjectUser> lstUP = usDao.FindByProjectID(ViewBag.Project.ProjectID);
            List<string> lstUPlogin = new List<string>();
            foreach (var pUs in lstUP)
            {
                //string sLogin = pUs.LoginID.ToString();
                lstUPlogin.Add(pUs.LoginID.ToString());
            }
            SetUserBag(lstUPlogin.ToArray<string>());


            //Lấy danh sách nhà cung ứng 
            ProjectSupplierDao psDao = new ProjectSupplierDao();
            List<ProjectSupplier> lsPSP = psDao.FindByID(ViewBag.Project.ProjectID);

            List<string> lstPS = new List<string>();
            foreach (var sid in lsPSP)
            {
                //string sLogin = pUs.LoginID.ToString();
                lstPS.Add(sid.SupplierID.ToString());
            }
            SetViewSupplier(lstPS.ToArray<string>());


            //Lấy danh sách chủ đầu tư
            ProjectContratorDao pctrDao = new ProjectContratorDao();
            List<ProjectContrator> lstPCTR = pctrDao.FindByID(ViewBag.Project.ProjectID);

            List<string> lstPCT = new List<string>();
            foreach (var contrid in lstPCTR)
            {
                //string sLogin = pUs.LoginID.ToString();
                lstPCT.Add(contrid.ContratorID.ToString());
            }
            SetContratorViewBag(lstPCT.ToArray<string>());

            //Lấy danh sách nhà thầu thi công
            ProjectBuilderDao pbdDao = new ProjectBuilderDao();
            List<ProjectBuilder> lsPBD = pbdDao.FindByID(ViewBag.Project.ProjectID);

            List<string> lstPD = new List<string>();
            foreach (var bdid in lsPBD)
            {
                //string sLogin = pUs.LoginID.ToString();
                lstPD.Add(bdid.BuilderID.ToString());
            }
            SetBuilderViewBag(lstPD.ToArray<string>());
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
            ViewBag.Messege = prcessDao.CountProcessMessage(id).ToString();
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

              
               
                string[] lstContrators = data.GetValues("drlContrator");
                SetContratorViewBag(lstContrators);

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
                if (ModelState.IsValid)
                {
                    bool kt = true;


                    Project objProject = bdDao.FindByID(id);
                    string cityID = data["drlCityID"].ToString();
              
                 
                    SetViewBagCity(cityID);
                    long categoryID = Convert.ToInt64(data["drlCategoryID"].ToString());
                    SetCatagoryBag(categoryID);
                    long priceID = Convert.ToInt64(data["drlPriceID"].ToString());
                    SetPriceIDViewBag(priceID);
                    SetViewBagDistrict(data["drlCityID"], data["drlCityID"]);
                    SetResourceIDViewBag(Convert.ToInt64(data["drlResourceID"]));
                    SetPriceIDViewBag(Convert.ToInt64(data["drlPriceID"]));


                    //Danh sách  đối thủ cạnh tranh được chọn
                    string[] lstCompetitors = data.GetValues("drlCompetitor");
                    SetCompetitorViewBag(lstCompetitors);

                    string[] members = data.GetValues("drbMember");
                    SetUserBag(members);
                    //Danh sách nhà cung ứng
                    string[] iSupplierID = data.GetValues("drlSupplier");
                    SetViewSupplier(iSupplierID);

                    //Danh sách nhà thầu thi công
                    string[] lstBuilders = data.GetValues("drlBuilder");
                    SetBuilderViewBag(lstBuilders);
                    //Danh sách chủ đầu tư

                    //string projectCode = data["txtCode"].ToString();
                    string name = data["Name"].ToString();
                    string address = data["Address"].ToString();
                  
                    string[] lstproductID = data.GetValues("cblProduct");
                    //string IsGroup = data["IsGroup"].ToString();
                    //string IsPublic = data["IsPublic"].ToString();

                    //Kiểm tra nhà cung ứng
                    if (iSupplierID == null)
                    {
                        kt = false;
                        ModelState.AddModelError("", "Bạn phải chọn nhà cung ứng!");

                    }
                    if (lstContrators == null)
                    {
                        kt = false;
                        ModelState.AddModelError("", "Bạn phải chọn chủ đầu tư!");

                    }
                    if (lstBuilders == null)
                    {
                        kt = false;
                        ModelState.AddModelError("", "Bạn phải chọn nhà thầu thi công!");

                    }



                    List<long> lstProductIDProject = new List<long>();
                    if (lstproductID == null)
                    {
                        kt = false;
                        ModelState.AddModelError("", "Bạn chưa chọn sản phẩm của dự án!");

                    }
                    else
                    {
                        //Kiểm tra dữ liệu của chiết khấu đã nhập chưa
                        try
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
                                lstProductIDProject.Add(productID);
                            }
                        }
                        catch
                        {
                            kt = false;
                            ModelState.AddModelError("", "Nhập giá và chiết khấu kiểu số tương ứng với sản phẩm dự án!");
                        }

                    }



             
                    //Kiểm tra sản phẩm của đối thủ cạnh tranh đã chọn

                    if (lstCompetitors != null)
                    {
                        foreach (string scpID in lstCompetitors)
                        {
                            //Lấy danh sách các sản phẩm của từng đối thủ được chọn rồi lấy chiết khấu của sản phẩm đấy
                            string[] lstProdctCom;
                            string drlProuctCom = "cblProduct" + scpID;
                            lstProdctCom = data.GetValues(drlProuctCom);
                            List<long> lstProductIDCompare = new List<long>();
                            try
                            {
                                if (lstProdctCom.Length > 0)
                                {
                                    double discount = 0;
                                    double discountVAT = 0;
                                    long prID = 0;
                                    foreach (string sProID in lstProdctCom)
                                    {
                                        prID = Convert.ToInt64(sProID);
                                        lstProductIDCompare.Add(prID);
                                        string txtDiscount = "txtDiscount" + scpID + "_" + sProID;
                                        string txtDiscountVAT = "txtDiscountVAT" + scpID + "_" + sProID;
                                        if (data[txtDiscountVAT].ToString() != null)
                                        {
                                            discountVAT = Convert.ToDouble(data[txtDiscountVAT].ToString());
                                        }
                                        if (data[txtDiscount].ToString() != null)
                                        {
                                            discount = Convert.ToDouble(data[txtDiscount].ToString());
                                        }
                                    }
                                    if (Hepper.compareList(lstProductIDProject, lstProductIDCompare) == false)
                                    {
                                        kt = false;
                                        ModelState.AddModelError("", "Bạn phải chọn sản phẩm của đối thủ cạnh tranh giống sản phẩm của dự án!");
                                        break;
                                    }
                                }
                            }
                            catch
                            {
                                kt = false;
                                ModelState.AddModelError("", "Nhập giá và chiết khấu kiểu số tương ứng với sản phẩm của đối thủ cạnh tranh!");
                            }



                        }

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
                        objProject.ResourceID = Convert.ToInt64(data["drlResourceID"].ToString());
                        objProject.CategoryID = categoryID;
                        objProject.CityID = cityID;
                        objProject.DistrictID = data["drlCityID"].ToString();
                        objProject.Address = address;
                        //objProject.Code = projectCode;
                       
                    //    objProject.SupplierID = iSupplierID;
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
                        if (members != null)
                        {
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
                        }

                        //Thêm danh sách nhà cung ứng
                        if (iSupplierID != null)
                        {

                            ProjectSupplierDao prsDB = new ProjectSupplierDao();
                            //Xóa danh sách nhà cung ứng thuộc dự án
                            prsDB.DeleteByProjectID(objProject.ProjectID);
                            foreach (string spID in iSupplierID)
                            {
                                ProjectSupplier objPS = new ProjectSupplier();
                                objPS.ProjectID = projectID;
                                objPS.SupplierID = long.Parse(spID);
                                prsDB.Insert(objPS);
                            }
                        }
                        //Thêm danh sách chủ đầu tư
                        if (lstContrators != null)
                        {
                            ProjectContratorDao pcDB = new ProjectContratorDao();
                            //Xóa danh sách chủ đầu tư thuộc dự án
                            pcDB.DeleteByProjectID(objProject.ProjectID);
                            foreach (string ctID in lstContrators)
                            {
                                ProjectContrator objPS = new ProjectContrator();
                                objPS.ProjectID = projectID;
                                objPS.ContratorID = long.Parse(ctID);
                                pcDB.Insert(objPS);
                            }
                        }
                        //Thêm danh sách nhà thầu thi công
                        if (lstBuilders != null)
                        {
                            ProjectBuilderDao pbDB = new ProjectBuilderDao();
                            //Xóa danh nhà thầu thi công thuộc dự án
                            pbDB.DeleteByProjectID(objProject.ProjectID);
                            foreach (string bdID in lstBuilders)
                            {
                                ProjectBuilder objPS = new ProjectBuilder();
                                objPS.ProjectID = projectID;
                                objPS.BuilderID = long.Parse(bdID);
                                pbDB.Insert(objPS);
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

                        ProjectCompetitorDao prcDao = new ProjectCompetitorDao();
                        //Xóa danh sách các đối thủ cạnh tranh của dự án
                        prcDao.DeleteByProjectID(objProject.ProjectID);

                        if (lstCompetitors != null)
                        {
                            //Lấy danh sách chiết khấu giá của sản phẩm mà đối thủ cạnh tranh
                            List<ProjectCompetitor> lstprojectConpetitor = new List<ProjectCompetitor>();

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
                                            // compdtDao.Delete(objProCom.ID);

                                        }
                                    }

                                }
                            }

                            //Thêm sản phẩm và chiết khấu của đối thủ vào CSDL


                            foreach (CompetiorProduct objCPDT in lstComProduct)
                            {
                                compdtDao.Insert(objCPDT);
                            }

                        }


                        SetAlert("Cập nhật thành công", "success");
                        return RedirectToAction("Details", "Project", new { id = id });
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
            SetViewBag();
            SetViewBagCity(ViewBag.Project.CityID);
            SetViewBagDistrict(ViewBag.Project.DistrictID, ViewBag.Project.CityID);
           
            SetCatagoryBag(ViewBag.Project.CategoryID);
            SetResourceIDViewBag(ViewBag.Project.ResourceID);
            SetPriceIDViewBag(ViewBag.Project.PriceID);
       
            ProjectUserDao usDao = new ProjectUserDao();
            List<ProjectUser> lstUP = usDao.FindByProjectID(ViewBag.Project.ProjectID);
            List<string> lstUPlogin = new List<string>();
            foreach (var pUs in lstUP)
            {
                //string sLogin = pUs.LoginID.ToString();
                lstUPlogin.Add(pUs.LoginID.ToString());
            }
            SetUserBag(lstUPlogin.ToArray<string>());

            //Lấy danh sách nhà cung ứng 
            ProjectSupplierDao psDao = new ProjectSupplierDao();
            List<ProjectSupplier> lsPSP = psDao.FindByID(ViewBag.Project.ProjectID);

            List<string> lstPS = new List<string>();
            foreach (var sid in lsPSP)
            {
                //string sLogin = pUs.LoginID.ToString();
                lstPS.Add(sid.SupplierID.ToString());
            }
            SetViewSupplier(lstPS.ToArray<string>());


            //Lấy danh sách chủ đầu tư
            ProjectContratorDao pctrDao = new ProjectContratorDao();
            List<ProjectContrator> lstPCTR = pctrDao.FindByID(ViewBag.Project.ProjectID);

            List<string> lstPCT = new List<string>();
            foreach (var contrid in lstPCTR)
            {
                //string sLogin = pUs.LoginID.ToString();
                lstPCT.Add(contrid.ContratorID.ToString());
            }
            SetContratorViewBag(lstPCT.ToArray<string>());

            //Lấy danh sách nhà thầu thi công
            ProjectBuilderDao pbdDao = new ProjectBuilderDao();
            List<ProjectBuilder> lsPBD = pbdDao.FindByID(ViewBag.Project.ProjectID);

            List<string> lstPD = new List<string>();
            foreach (var bdid in lsPBD)
            {
                //string sLogin = pUs.LoginID.ToString();
                lstPD.Add(bdid.BuilderID.ToString());
            }
            SetBuilderViewBag(lstPD.ToArray<string>());

           

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
            ViewBag.Messege = prcessDao.CountProcessMessage(id).ToString();
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

                ProjectUserDao usDao = new ProjectUserDao();
                List<ProjectUser> lstUP = usDao.FindByProjectID(ViewBag.Project.ProjectID);
                List<string> lstUPlogin = new List<string>();
                foreach (var pUs in lstUP)
                {
                    //string sLogin = pUs.LoginID.ToString();
                    lstUPlogin.Add(pUs.LoginID.ToString());
                }
                SetUserBag(lstUPlogin.ToArray<string>());
                

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
                if (ModelState.IsValid)
                {
                    bool kt = true;

                    Project objProject = bdDao.FindByID(id);
                    string cityID = data["drlCityID"].ToString();
                    SetViewBagCity(cityID);
                 
                    long categoryID = Convert.ToInt64(data["drlCategoryID"].ToString());
                    SetCatagoryBag(categoryID);
                    long priceID = Convert.ToInt64(data["drlPriceID"].ToString());
                    SetPriceIDViewBag(priceID);
                    SetViewBagDistrict(data["drlDistrict"], data["drlCityID"]);
                    SetResourceIDViewBag(Convert.ToInt64(data["drlResourceID"]));
                    SetPriceIDViewBag(Convert.ToInt64(data["drlPriceID"]));
                           
                    //Danh sách  đối thủ cạnh tranh được chọn
                    string[] lstCompetitors = data.GetValues("drlCompetitor");
                    SetCompetitorViewBag(lstCompetitors);

                    string[] members = data.GetValues("drbMember");
                    SetUserBag(members);
                    //Danh sách nhà cung ứng
                    string[] iSupplierID = data.GetValues("drlSupplier");
                    SetViewSupplier(iSupplierID);

                    //Danh sách nhà thầu thi công
                    string[] lstBuilders = data.GetValues("drlBuilder");
                    SetBuilderViewBag(lstBuilders);
                    //Danh sách chủ đầu tư
                    string[] lstContrators = data.GetValues("drlContrator");
                    SetContratorViewBag(lstContrators);

                    //  string projectCode = data["txtCode"].ToString();
                    string name = data["Name"].ToString();
                    string address = data["Address"].ToString();
                   
                    string[] lstproductID = data.GetValues("cblProduct");


                    //Kiểm tra nhà cung ứng
                    if (iSupplierID == null)
                    {
                        kt = false;
                        ModelState.AddModelError("", "Bạn phải chọn nhà cung ứng!");

                    }
                    if (lstContrators == null)
                    {
                        kt = false;
                        ModelState.AddModelError("", "Bạn phải chọn chủ đầu tư!");

                    }
                    if (lstBuilders == null)
                    {
                        kt = false;
                        ModelState.AddModelError("", "Bạn phải chọn nhà thầu thi công!");

                    }

                    List<long> lstProductIDProject = new List<long>();
                    if (lstproductID == null)
                    {
                        kt = false;
                        ModelState.AddModelError("", "Bạn chưa chọn sản phẩm của dự án!");

                    }
                    else
                    {
                        //Kiểm tra dữ liệu của chiết khấu đã nhập chưa
                        try
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
                                lstProductIDProject.Add(productID);
                            }
                        }
                        catch
                        {
                            kt = false;
                            ModelState.AddModelError("", "Nhập giá và chiết khấu kiểu số tương ứng với sản phẩm dự án!");
                        }

                    }



                  
                    //Kiểm tra sản phẩm của đối thủ cạnh tranh đã chọn

                    if (lstCompetitors != null)
                    {
                        foreach (string scpID in lstCompetitors)
                        {
                            //Lấy danh sách các sản phẩm của từng đối thủ được chọn rồi lấy chiết khấu của sản phẩm đấy
                            string[] lstProdctCom;
                            string drlProuctCom = "cblProduct" + scpID;
                            lstProdctCom = data.GetValues(drlProuctCom);
                            List<long> lstProductIDCompare = new List<long>();
                            try
                            {
                                if (lstProdctCom.Length > 0)
                                {
                                    double discount = 0;
                                    double discountVAT = 0;
                                    long prID = 0;
                                    foreach (string sProID in lstProdctCom)
                                    {
                                        prID = Convert.ToInt64(sProID);
                                        lstProductIDCompare.Add(prID);
                                        string txtDiscount = "txtDiscount" + scpID + "_" + sProID;
                                        string txtDiscountVAT = "txtDiscountVAT" + scpID + "_" + sProID;
                                        if (data[txtDiscountVAT].ToString() != null)
                                        {
                                            discountVAT = Convert.ToDouble(data[txtDiscountVAT].ToString());
                                        }
                                        if (data[txtDiscount].ToString() != null)
                                        {
                                            discount = Convert.ToDouble(data[txtDiscount].ToString());
                                        }
                                    }
                                    if (Hepper.compareList(lstProductIDProject, lstProductIDCompare) == false)
                                    {
                                        kt = false;
                                        ModelState.AddModelError("", "Bạn phải chọn sản phẩm của đối thủ cạnh tranh giống sản phẩm của dự án!");
                                        break;
                                    }
                                }
                            }
                            catch
                            {
                                kt = false;
                                ModelState.AddModelError("", "Nhập giá và chiết khấu kiểu số tương ứng với sản phẩm của đối thủ cạnh tranh!");
                            }



                        }

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
                        objProject.ResourceID = Convert.ToInt64(data["drlResourceID"].ToString());
                        objProject.CategoryID = categoryID;
                        objProject.CityID = cityID;
                        objProject.DistrictID = data["drlDistrict"].ToString();
                        objProject.Description = data["txtDescription"].ToString();
                        objProject.Note = data["txtNote"].ToString();
                        objProject.NotePass = data["txtNotePass"].ToString();
                        objProject.Address = address;
                        //  objProject.Code = projectCode;
                        //objProject.ContratorID = objContra.ID;
                        //objProject.BuilderID = objBuider.ID;
                      //i  objProject.SupplierID = iSupplierID;
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
                        if (members != null)
                        {
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
                        }

                        //Thêm danh sách nhà cung ứng
                        if (iSupplierID != null)
                        {

                            ProjectSupplierDao prsDB = new ProjectSupplierDao();
                            //Xóa danh sách nhà cung ứng thuộc dự án
                            prsDB.DeleteByProjectID(objProject.ProjectID);
                            foreach (string spID in iSupplierID)
                            {
                                ProjectSupplier objPS = new ProjectSupplier();
                                objPS.ProjectID = projectID;
                                objPS.SupplierID = long.Parse(spID);
                                prsDB.Insert(objPS);
                            }
                        }
                        //Thêm danh sách chủ đầu tư
                        if (lstContrators != null)
                        {
                            ProjectContratorDao pcDB = new ProjectContratorDao();
                            //Xóa danh sách chủ đầu tư thuộc dự án
                            pcDB.DeleteByProjectID(objProject.ProjectID);
                            foreach (string ctID in lstContrators)
                            {
                                ProjectContrator objPS = new ProjectContrator();
                                objPS.ProjectID = projectID;
                                objPS.ContratorID = long.Parse(ctID);
                                pcDB.Insert(objPS);
                            }
                        }
                        //Thêm danh sách nhà thầu thi công
                        if (lstBuilders != null)
                        {
                            ProjectBuilderDao pbDB = new ProjectBuilderDao();
                            //Xóa danh nhà thầu thi công thuộc dự án
                            pbDB.DeleteByProjectID(objProject.ProjectID);
                            foreach (string bdID in lstBuilders)
                            {
                                ProjectBuilder objPS = new ProjectBuilder();
                                objPS.ProjectID = projectID;
                                objPS.BuilderID = long.Parse(bdID);
                                pbDB.Insert(objPS);
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
                        ProjectCompetitorDao prcDao = new ProjectCompetitorDao();
                        //Xóa danh sách các đối thủ cạnh tranh của dự án
                        prcDao.DeleteByProjectID(objProject.ProjectID);
                        //Lấy danh sách chiết khấu giá của sản phẩm mà đối thủ cạnh tranh
                        if (lstCompetitors != null)
                        {
                            List<ProjectCompetitor> lstprojectConpetitor = new List<ProjectCompetitor>();


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
                                            // compdtDao.Delete(objProCom.ID);

                                        }
                                    }

                                }
                            }

                            //Thêm sản phẩm và chiết khấu của đối thủ vào CSDL


                            foreach (CompetiorProduct objCPDT in lstComProduct)
                            {
                                compdtDao.Insert(objCPDT);
                            }
                        }

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

            //var condao = new ContratorDao();
            //var builderdao = new BuilderDao();
            var productdao = new ProductDao();

            //ViewBag.Contrator = new SelectList(condao.ToListActive(), "ContratorID", "ContraName", selectedId);

        //    ViewBag.Builder = new SelectList(builderdao.ToListActive(), "BuilderID", "BuilderName", selectedId);
            ViewBag.Products = productdao.ToListActive();
        }
        public void SetViewBagCity(string selectedId = null)
        {
            var dao = new CityDao();
            ViewBag.CityID = new SelectList(dao.ToList(), "CityID", "Name", selectedId);

        }
        public void SetViewBagCitySuplier(string selectedId = null)
        {
            var dao = new CityDao();
            ViewBag.CityIDSupplier = new SelectList(dao.ToList(), "CityID", "Name", selectedId);

        }
        public void SetCatagoryBag(long? selectedId = null)
        {
            var dao = new CategoryDao();
            ViewBag.CategoryID = new SelectList(dao.ToList(), "CategoryID", "Name", selectedId);
        }
        public void SetUserBag(string[] selectedId = null)
        {
            var dao = new UserDao();
            User objUS = dao.FindByID(3);
            var lst = dao.ToList();
            lst.Remove(objUS);
            ViewBag.Member = new MultiSelectList(lst, "LoginID", "FullName", selectedId);
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
        public void SetBuilderViewBag(string[] selectedId = null)
        {
            var dao = new BuilderDao();
            ViewBag.Builder = new MultiSelectList(dao.ToListActive(), "ID", "BuilderName", selectedId);
            ViewBag.ListBuilder = dao.ToListActive();
        }
        public void SetContratorViewBag(string[] selectedId = null)
        {
            var dao = new ContratorDao();
            ViewBag.Contrator = new MultiSelectList(dao.ToListActive(), "ID", "ContraName", selectedId);
            ViewBag.ListContrator = dao.ToListActive();
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
        public void SetViewSupplier(string[] selectedId = null, string cityid = null, string districtid = null)
        {
            var dao = new SupplierDao();
            if (cityid != null)
            {
                ViewBag.Supplier = new MultiSelectList(dao.FindByDistrist(cityid, districtid), "ID", "SupplierName", selectedId);
            }
            else
                ViewBag.Supplier = new MultiSelectList(dao.ToList(), "ID", "SupplierName", selectedId);
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
