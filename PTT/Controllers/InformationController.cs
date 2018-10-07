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
    public class InformationController : BaseController
    {
        PTTDataContext db = null;
        // GET: Information
        public ActionResult Index()
        {
            return View();
        }
        
        // GET: Information/Details/5
        public ActionResult Manager()
        {
            InformationDao bdDao = new InformationDao();

            return View(bdDao.ToList());
        }
        public ActionResult Shared()
        {
            InformationDao bdDao = new InformationDao();

            return View(bdDao.ToListShared());
        }
        public ActionResult Jobs()
        {
            UserLogin us = (UserLogin)Session[CommonConstant.USER_SESSION];
            InformationDao bdDao = new InformationDao();
            db = new PTTDataContext();


            //ViewBag.Info = from inf in db.Information
            //               join uinfo in db.InforUsers on inf.InformationID equals uinfo.InforID
            //               orderby inf.CreateDate ascending
            //               where uinfo.LoginID== us.UserID
            //               select new Information
            //               {
            //                   Address = inf.Address,
            //                   BuilderID = inf.BuilderID,
            //                   ContratorID = inf.ContratorID,
            //                   CreateBy = inf.CreateBy,
            //                   CreateDate = inf.CreateDate,
            //                   DateLine = inf.DateLine,
            //                   Description = inf.Description,
            //                   InformationID = inf.InformationID,
            //                   ModifiedBy = inf.ModifiedBy,
            //                   ModifiedDate = inf.ModifiedDate,
            //                   Name = inf.Name,
            //                   Note = inf.Note,
            //                   Status = inf.Status,
            //                   SupplierID = inf.SupplierID

            //               };
            List<Information> listInfUser = new List<Information>();

            var listInfo = db.Information.ToList();
            var listUser = db.InforUsers.Where(u => u.LoginID == us.UserID).ToList();
            foreach(var inf in listInfo){
                foreach(var u in listUser)
                {
                    if(u.InforID== inf.InformationID)
                    {
                        listInfUser.Add(inf);

                    }
                }
            }
            ViewBag.Info = listInfUser;
            return View();
        }

      

        // GET: Information/Create
        public ActionResult Create()
        {
            SetViewBag();
          
           
            SetUserBag();
           
            SetViewSupplier();
            return View();
        }

        // POST: Information/Create
        [HttpPost]
        public ActionResult Delete(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Information/Edit/5
        public ActionResult Edit(long id)
        {
            InformationDao dbDao = new InformationDao();
            SetViewBag();
            FeedbackInforDao feedDao = new FeedbackInforDao();
            ViewBag.Feedback = feedDao.ToListFeebBackUser(id).ToArray<FeedbacInfokUser>();
            ViewBag.Infomation = dbDao.FindByID(id);
            SetViewSupplier(ViewBag.Infomation.SupplierID);
            SetStatus(ViewBag.Infomation.Status);
            InforUserDao usDao = new InforUserDao();
            List<InforUser> lstUP = usDao.FindByInforID(ViewBag.Infomation.InformationID);
            List<string> lstUPlogin = new List<string>();
            foreach (var pUs in lstUP)
            {
                //string sLogin = pUs.LoginID.ToString();
                lstUPlogin.Add(pUs.LoginID.ToString());
            }
            SetUserBag(lstUPlogin.ToArray<string>());

            ContratorDao contrDao = new ContratorDao();
            Contrator objConTra = contrDao.FindByID(ViewBag.Infomation.ContratorID);
            string str = "<p><b>Tên chủ đầu tư: </b>" + objConTra.ContraName + "</p>";
            str += "<p><b>Địa chỉ: </b>" + objConTra.Address + "</p>";
            str += "<p><b>Thông tin liên hệ: </b>" + objConTra.FullName + "<b> &nbsp;&nbsp;&nbsp;  Điện thoại: </b>" + objConTra.Phone + "</p>";
            ViewBag.PrContraDetail = str;

            ViewBag.PrContraCode = objConTra.ContratorID;
            BuilderDao buiDao = new BuilderDao();
            Builder objBuilder = buiDao.FindByID(ViewBag.Infomation.BuilderID);
            ViewBag.PrBuiderCode = objBuilder.BuilderID;
            str = "";
            str = "<p><b>Tên nhà thầu: </b>" + objBuilder.BuilderName + "</p>";
            str += "<p><b>Địa chỉ: </b>" + objBuilder.Address + "</p>";
            str += "<p><b>Thông tin liên hệ: </b>" + objBuilder.FullName + "<b>&nbsp;&nbsp; &nbsp; Điện thoại: </b>" + objBuilder.Phone + "</p>";
            ViewBag.BuiderDetail = str;


            return View();
        }
        // GET: Information/Edit/5
        public ActionResult Details(long id)
        {
            InformationDao dbDao = new InformationDao();
            SetViewBag();
            FeedbackInforDao feedDao = new FeedbackInforDao();
            ViewBag.Feedback = feedDao.ToListFeebBackUser(id).ToArray<FeedbacInfokUser>();
            ViewBag.Infomation = dbDao.FindByID(id);
            SetViewSupplier(ViewBag.Infomation.SupplierID);
            SetStatus(ViewBag.Infomation.Status);
            InforUserDao usDao = new InforUserDao();
            List<InforUser> lstUP = usDao.FindByInforID(ViewBag.Infomation.InformationID);
            List<string> lstUPlogin = new List<string>();
            foreach (var pUs in lstUP)
            {
                //string sLogin = pUs.LoginID.ToString();
                lstUPlogin.Add(pUs.LoginID.ToString());
            }
            SetUserBag(lstUPlogin.ToArray<string>());

            ContratorDao contrDao = new ContratorDao();
            Contrator objConTra = contrDao.FindByID(ViewBag.Infomation.ContratorID);
            string str = "<p><b>Tên chủ đầu tư: </b>" + objConTra.ContraName + "</p>";
            str += "<p><b>Địa chỉ: </b>" + objConTra.Address + "</p>";
            str += "<p><b>Thông tin liên hệ: </b>" + objConTra.FullName + "<b> &nbsp;&nbsp;&nbsp;  Điện thoại: </b>" + objConTra.Phone + "</p>";
            ViewBag.PrContraDetail = str;

            ViewBag.PrContraCode = objConTra.ContratorID;
            BuilderDao buiDao = new BuilderDao();
            Builder objBuilder = buiDao.FindByID(ViewBag.Infomation.BuilderID);
            ViewBag.PrBuiderCode = objBuilder.BuilderID;
            str = "";
            str = "<p><b>Tên nhà thầu: </b>" + objBuilder.BuilderName + "</p>";
            str += "<p><b>Địa chỉ: </b>" + objBuilder.Address + "</p>";
            str += "<p><b>Thông tin liên hệ: </b>" + objBuilder.FullName + "<b>&nbsp;&nbsp; &nbsp; Điện thoại: </b>" + objBuilder.Phone + "</p>";
            ViewBag.BuiderDetail = str;


            return View();
        }

        // POST: Information/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( FormCollection data)
        {
            try
            {
                InformationDao dbDao = new InformationDao();
                long id = Convert.ToInt64(data["hdIDInfor"].ToString());
                Information objProject = dbDao.FindByID(id);
                FeedbackInforDao feedDao = new FeedbackInforDao();
                ViewBag.Feedback = feedDao.ToListFeebBackUser(id).ToArray<FeedbacInfokUser>();
                // string cityID = data["CityID"].ToString();
                // SetViewBag(cityID);
                // long categoryID = Convert.ToInt64(data["CategoryID"].ToString());
                //long priceID = Convert.ToInt64(data["PriceID"].ToString());
                string name = data["Name"].ToString();
                string address = data["Address"].ToString();
                string contratorID = data["txtContratorID"].ToString();
                string builderID = data["txtBuilder"].ToString();
                string Note = data["txtNote"].ToString();
                string[] members = data.GetValues("drbMember");

                SetUserBag(members);

                objProject.Address = address;
                
              
                objProject.ContratorID = (new ContratorDao().FindByCode(contratorID.Trim()).ID);
                objProject.BuilderID = (new BuilderDao().FindByCode(builderID.Trim()).ID);
              //  objProject.SupplierID = Convert.ToInt64(data["drlSupplier"]);
                UserLogin us = (UserLogin)Session[CommonConstant.USER_SESSION];


                int iStatus = Convert.ToInt32(data["drlStatus"].ToString());
                // long iSupplierID = Convert.ToInt64(data["drlSupplier"].ToString());
                objProject.Status = iStatus;

                objProject.ModifiedDate = Hepper.GetDateServer();
                objProject.Description = data["txtDescription"].ToString();
                objProject.ModifiedBy = us.UserName;
                objProject.Note = Note;
                objProject.Name = name;

              long infoID=  dbDao.Update(objProject);
                    //thêm danh sách nhóm vào trong dự án
                InforUserDao prUSDao = new InforUserDao();
                //Xóa nhóm thuộc dự án
                prUSDao.Delete(objProject.InformationID);
               // InforUser objPrUS = new InforUser();
                //objPrUS.InforID = infoID;
                //objPrUS.LoginID = us.UserID;
                //objPrUS.IsAdmin = true;
                //prUSDao.Insert(objPrUS);
                foreach (string sUsID in members)
                {
                    long usID = Convert.ToInt64(sUsID);
                    if (usID != us.UserID)
                    {
                        InforUser objPrUSM = new InforUser();
                        objPrUSM.InforID = infoID;
                        objPrUSM.LoginID = usID;
                        objPrUSM.IsAdmin = false;
                        prUSDao.Insert(objPrUSM);
                    }
                }
                SetAlert("Cập nhật thành công", "success");
                return RedirectToAction("Manager", "Information");
              
            }
            catch
            {
                SetAlert("Không cập nhật được", "danger");
                return View();
            }
        }

        // GET: Information/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Information/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( FormCollection data)
        {
            try
            {
                SetViewBag();
             //  /  string[] members = data.GetValues("drbMember");

                //SetUserBag(members);
                string name = data["Name"].ToString();
                string address = data["Address"].ToString();
                string contratorID = data["txtContratorID"].ToString();
                string builderID = data["txtBuilder"].ToString();
                
                //string IsGroup = data["IsGroup"].ToString();
                //string IsPublic = data["IsPublic"].ToString();




                UserLogin us = (UserLogin)Session[CommonConstant.USER_SESSION];
                InformationDao bdDao = new InformationDao();
                Information objProject = new Information();
               // long iSupplierID = Convert.ToInt64(data["drlSupplier"].ToString());
               // objProject.SupplierID = iSupplierID;
                objProject.CreateDate = Hepper.GetDateServer();
                objProject.ModifiedDate = Hepper.GetDateServer();
                objProject.CreateBy = us.UserName;
                objProject.ModifiedBy = us.UserName;
                objProject.Name = name;
                objProject.Description = data["txtDescription"].ToString();
                objProject.Address = address;
              
                objProject.ContratorID = (new ContratorDao().FindByCode(contratorID.Trim()).ID);
                objProject.BuilderID = (new BuilderDao().FindByCode(builderID.Trim()).ID);
              
                objProject.Status = 0;
                objProject.DateLine = Hepper.GetDateServer();
                //Thêm dự án vào CSDL
                long projectID = bdDao.Insert(objProject);

                SetAlert("Thêm thành công", "success");

                return RedirectToAction("Index", "Home");
            }
            catch
            {
                SetAlert("Không thêm được", "danger");
                return View();
            }
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
        public void SetStatus(int? selectedId = null)
        {
            var selectList = new SelectList(
                       new List<SelectListItem>
                       {
                                 new SelectListItem {Text = "Đợi duyệt chia sẻ", Value = "0"},
                                 new SelectListItem {Text = "Đã duyệt", Value = "1"},
                             new SelectListItem {Text = "Đã phân công", Value = "2"},
                             new SelectListItem {Text = "Kết thúc", Value = "3"},
                              new SelectListItem {Text = "Không duyệt", Value = "4"},
                       }, "Value", "Text",selectedId);


            ViewBag.Status = selectList;
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
        public void SetUserBag(string[] selectedId = null)
        {
            var dao = new UserDao();
            ViewBag.Member = new MultiSelectList(dao.ToList(), "LoginID", "FullName", selectedId);
        }
    }
}
