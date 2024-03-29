﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
using System.Data.SqlClient;

namespace Model.DAO
{
    public class ContratorDao
    {
        PTTDataContext db = null;

        public ContratorDao()
        {

            db = new PTTDataContext();
        }
        public List<Contrator> ToList()
        {
            return db.Contrators.ToList<Contrator>();
        }
        public List<Contrator> ToListActive()
        {
            return db.Contrators.Where(c=>c.Status==true).ToList<Contrator>();
        }
        public long Insert(Contrator buider)
        {
            db.Contrators.Add(buider);
            db.SaveChanges();
            return buider.ID;
        }
        public long Delete(long ID)
        {
            var bd = db.Contrators.Find(ID);
            db.Contrators.Remove(bd);
            db.SaveChanges();
            return bd.ID;
        }
        public Contrator FindByID(long ID)
        {
            
             return db.Contrators.Find(ID);
        }
        public string GenaraCode(string str, int lengh)
        {
            return db.Database.SqlQuery<string>("exec proc_t_GenaraContratorCode @Code,@len", new SqlParameter("@Code", str), new SqlParameter("@len", lengh)).SingleOrDefault();
        }
        public Contrator FindByCode(string code)
        {
            var ct = db.Contrators.SingleOrDefault<Contrator>(c => c.ContratorID == code&&c.Status==true);
            return ct;
        }
        public Contrator FindByTaxID(string taxID)
        {
            var ct = db.Contrators.SingleOrDefault<Contrator>(c => c.TaxID == taxID );
            return ct;
        }
        public long Update(Contrator buider)
        {
            var bd = db.Contrators.Find(buider.ID);
           // bd.ContratorID = buider.ContratorID;
            bd.FullName = buider.FullName;
            bd.TaxID = buider.TaxID;
            //bd.CreateBy = buider.CreateBy;
            //bd.CreateDate = buider.CreateDate;
            bd.Email = buider.Email;
            bd.ContraName = buider.ContraName;
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
