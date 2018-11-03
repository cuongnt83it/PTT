using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
using System.Data.SqlClient;

namespace Model.DAO
{
    public class ProjectDao
    {
        PTTDataContext db = null;

        public ProjectDao()
        {

            db = new PTTDataContext();
        }
        public List<Project> ToList()
        {
            return db.Projects.OrderBy(c => c.DisplayOrder).ToList<Project>();
        }
        public long Insert(Project buider)
        {
            db.Projects.Add(buider);
            db.SaveChanges();
            return buider.ProjectID;
        }
        public string GenaraCode(string str, int lengh)
        {
            return db.Database.SqlQuery<string>("exec proc_t_GenaraProjectCode @Code,@len", new SqlParameter("@Code", str), new SqlParameter("@len", lengh)).SingleOrDefault();
        }
        public long Delete(long ID)
        {
            //Xóa project USER
            ProjectUserDao prUerDB = new ProjectUserDao();
            prUerDB.Delete(ID);

            //Xóa sản phẩm và của dự án
            ProjectProductDao prProductDB = new ProjectProductDao();
            prProductDB.Delete(ID);

            //Xóa danh sách đối thủ cạnh tranh, sản phẩm của đối thủ cạnh tranh
            ProjectCompetitorDao prCompetitorDB = new ProjectCompetitorDao();
            prCompetitorDB.DeleteByProjectID(ID);

            // Xóa cập nhật tiến độ của dự án
            ProcessDao processDB = new ProcessDao();
            processDB.DeleteByProject(ID);

            //Xóa đóng góp ý kiến
            FeedbackDao fbDB = new FeedbackDao();
            fbDB.DeleteByProject(ID);

            var bd = db.Projects.Find(ID);
            db.Projects.Remove(bd);
            db.SaveChanges();
            return bd.ProjectID;
        }
        public List<ProjectUser> FindByUser(long ID)
        {

            return db.ProjectUsers.Where(a => a.LoginID == ID).ToList<ProjectUser>();
        }
        public List<Project> FindByBuider(long biulderID)
        {

            return db.Projects.Where(a => a.BuilderID == biulderID).ToList<Project>();
        }
        public List<Project> FindBySupplier(long supplierID)
        {

            return db.Projects.Where(a => a.SupplierID == supplierID).ToList<Project>();
        }
        public List<Project> FindByContrator(long contratorID)
        {

            return db.Projects.Where(a => a.ContratorID == contratorID).ToList<Project>();
        }
        public List<Project> FindByCategory(long CategorID)
        {

            return db.Projects.Where(a => a.CategoryID == CategorID).ToList<Project>();
        }
        public List<Project> FindByPrice(long PriceID)
        {

            return db.Projects.Where(a => a.PriceID == PriceID).ToList<Project>();
        }
        public List<ProjectCompetitor> FindByCompetitor(long competitorID)
        {

            return db.ProjectCompetitors.Where(a => a.CompetiorID == competitorID).ToList<ProjectCompetitor>();
        }
        public Project FindByID(long ID)
        {
            
             return db.Projects.Find(ID);
        }
        public long Update(Project buider)
        {
            var bd = db.Projects.Find(buider.ProjectID);
            bd.ProjectID = buider.ProjectID;
            bd.Name = buider.Name;
            //bd.CreateBy = buider.CreateBy;
            //bd.CreateDate = buider.CreateDate;
            bd.MetaTite = buider.MetaTite;
            bd.DisplayOrder = buider.DisplayOrder;
            bd.Status = buider.Status;
            bd.PriceID = buider.PriceID;
            bd.Address = buider.Address;

            bd.BuilderID = buider.BuilderID;
            bd.ResourceID = buider.ResourceID;
            bd.CategoryID = buider.CategoryID;
            bd.CityID = buider.CityID;
            bd.Code = buider.Code;
            bd.ContratorID = buider.ContratorID;
            bd.DateLine = buider.DateLine;
            bd.DistrictID = buider.DistrictID;
            bd.EndCreate = buider.EndCreate;
            bd.EndDate = buider.EndDate;
            bd.IsGroup = buider.IsGroup;
            bd.IsPublic = buider.IsPublic;
          
            bd.MetaTite = buider.MetaTite;
            bd.Note = buider.Note;
            bd.NotePass = buider.NotePass;
            bd.StartDate = buider.StartDate;
            bd.Status = buider.Status;
            bd.SupplierID = buider.SupplierID;
            bd.Value = buider.Value;

            bd.Description = buider.Description;
            bd.ModifiedBy = buider.ModifiedBy;
            bd.ModifiedDate = buider.ModifiedDate;
          
            db.SaveChanges();
            return buider.ProjectID;
        }
    }
}
