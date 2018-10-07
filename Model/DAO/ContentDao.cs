using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
namespace Model.DAO
{
    public class ContentDao
    {
        PTTDataContext db = null;

        public ContentDao()
        {

            db = new PTTDataContext();
        }
        public List<Model.EF.Content> ToList()
        {
            return db.Contents.ToList<Model.EF.Content>();
        }


      
        public List<Model.EF.Content> ListHot()
        {
            return db.Contents.Where(c => c.TopHot==true && c.Status==true).ToList<Model.EF.Content>();
        }
        public List<Model.EF.Content> ListActive()
        {
            return db.Contents.Where(c =>  c.Status == true).ToList<Model.EF.Content>();
        }
        public long Insert(Model.EF.Content buider)
        {
            db.Contents.Add(buider);
            db.SaveChanges();
            return buider.ContentID;
        }
        public long Delete(long ID)
        {
            var bd = db.Contents.Find(ID);
            db.Contents.Remove(bd);
            db.SaveChanges();
            return bd.ContentID;
        }
        public Content FindByID(long ID)
        {
            
             return db.Contents.Find(ID);
        }
        public long Update(Model.EF.Content buider)
        {
            var bd = db.Contents.Find(buider.ContentID);
         //   bd.ContentID = buider.BuilderID;
            bd.Name = buider.Name;
            
            //bd.CreateBy = buider.CreateBy;
            //bd.CreateDate = buider.CreateDate;
            bd.Description = buider.Description;
            bd.Detail = buider.Detail;
            bd.Image = buider.Image;
            bd.Status = buider.Status;
            bd.MetaTite = buider.MetaTite;
            bd.ModifiedBy = buider.ModifiedBy;
            bd.ModifiedDate = buider.ModifiedDate;
            bd.Status= buider.Status;
            bd.TopHot = buider.TopHot;
            db.SaveChanges();
            return buider.ContentID;
        }
    }
}
