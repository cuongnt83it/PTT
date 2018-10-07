using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
namespace Model.DAO
{
    public class SupplierDao
    {
        PTTDataContext db = null;

        public SupplierDao()
        {

            db = new PTTDataContext();
        }
        public List<Supplier> ToList()
        {
            return db.Suppliers.ToList();
        }
        public long Insert(Supplier buider)
        {
            db.Suppliers.Add(buider);
            db.SaveChanges();
            return buider.ID;
        }
        public long Delete(long ID)
        {
            var bd = db.Suppliers.Find(ID);
            db.Suppliers.Remove(bd);
            db.SaveChanges();
            return bd.ID;
        }
        public Supplier FindByID(long ID)
        {

            return db.Suppliers.Find(ID);
        }
        
        public List<Supplier> FindByDistrist(string cityID,string districtID)
        {
            var lst = db.Suppliers.Where(a => a.CityID == cityID && a.DistrictID == districtID).ToList<Supplier>();
            return lst;
        }
        public List<Supplier> FindByCity(string cityID)
        {
            var lst = db.Suppliers.Where(a => a.CityID == cityID ).ToList<Supplier>();
            return lst;
        }
        public long Update(Supplier buider)
        {
            var bd = db.Suppliers.Find(buider.ID);
            bd.SupplierID = buider.SupplierID;
            bd.FullName = buider.FullName;

            //bd.CreateBy = buider.CreateBy;
            //bd.CreateDate = buider.CreateDate;
            bd.CityID = buider.CityID;
            bd.DistrictID = buider.DistrictID;
            bd.Email = buider.Email;
            bd.SupplierName = buider.SupplierName;
            bd.Address = buider.Address;
            bd.Status = buider.Status;
            bd.Image = buider.Image;
            bd.ModifiedBy = buider.ModifiedBy;
            bd.ModifiedDate = buider.ModifiedDate;
            bd.Phone = buider.Phone;
            db.SaveChanges();
            return buider.ID;
        }
    }
}
