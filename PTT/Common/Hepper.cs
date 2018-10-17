using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Security.Cryptography;
using Model.EF;
using Model.DAO;

namespace PTT.Common
{
  
    public static class Hepper
    {
      public static  PTTDataContext db = new PTTDataContext();
        public static DateTime GetDateServer()
        {
            return  db.Database.SqlQuery<DateTime> ("spGetSystemDate").SingleOrDefault();
        }
        public static bool compareList(List<long> lst1, List<long> lst2)
        {
            bool kt = true;
            if (lst1 == null && lst1 == null)
            {
                return true;
            }
            else if (lst1 == null || lst1 == null)
            {
                return false;
            }
            if (lst1.Count != lst1.Count)
                return false;
           lst1.Sort();
            lst2.Sort();
            for(int i = 0; i < lst1.Count; i++)
            {
                if (lst1[i] != lst2[i]) return false;
            }
            return kt;

        }
       public static  int thisIsMagic(int year, int month, int day)
        {
            if (month < 3)
            {
                year--;
                month += 12;
            }
            return 365 * year + year / 4 - year / 100 + year / 400 + (153 * month - 457) / 5 + day - 306;
        }
        #region "Security functions"
        public static string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            //get hash result after compute it
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits
                //for each byte
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }
        #endregion
        #region "Moeny function"
        public static string str = " ";

        public static string ToStringMoney(decimal number)
        {
            str = " ";
            string s = number.ToString("#");
            string[] so = new string[] { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string[] hang = new string[] { "", "nghìn", "triệu", "tỷ" };
            int i, j, donvi, chuc, tram;

            bool booAm = false;
            decimal decS = 0;
            //Tung addnew
            try
            {
                decS = Convert.ToDecimal(s.ToString());
            }
            catch
            {
            }
            if (decS < 0)
            {
                decS = -decS;
                s = decS.ToString();
                booAm = true;
            }
            i = s.Length;
            if (i == 0)
                str = so[0] + str;
            else
            {
                j = 0;
                while (i > 0)
                {
                    donvi = Convert.ToInt32(s.Substring(i - 1, 1));
                    i--;
                    if (i > 0)
                        chuc = Convert.ToInt32(s.Substring(i - 1, 1));
                    else
                        chuc = -1;
                    i--;
                    if (i > 0)
                        tram = Convert.ToInt32(s.Substring(i - 1, 1));
                    else
                        tram = -1;
                    i--;
                    if ((donvi > 0) || (chuc > 0) || (tram > 0) || (j == 3))
                        str = hang[j] + str;
                    j++;
                    if (j > 3) j = 1;
                    if ((donvi == 1) && (chuc > 1))
                        str = "một " + str;
                    else
                    {
                        if ((donvi == 5) && (chuc > 0))
                            str = "năm " + str;
                        else if (donvi > 0)
                            str = so[donvi] + " " + str;
                    }
                    if (chuc < 0)
                        break;
                    else
                    {
                        if ((chuc == 0) && (donvi > 0)) str = "lẻ " + str;
                        if (chuc == 1) str = "mười " + str;
                        if (chuc > 1) str = so[chuc] + " mươi " + str;
                    }
                    if (tram < 0) break;
                    else
                    {
                        if ((tram > 0) || (chuc > 0) || (donvi > 0)) str = so[tram] + " trăm " + str;
                    }
                    str = " " + str;
                }
            }
            if (booAm) str = "Âm " + str;
            return str;// = str+ "đồng chẵn";

        }
        private static string[] ChuSo = new string[10] { " không", " một", " hai", " ba", " bốn", " năm", " sáu", " bẩy", " tám", " chín" };
        private static string[] Tien = new string[6] { "", " nghìn", " triệu", " tỷ", " nghìn tỷ", " triệu tỷ" };

        // Hàm đọc số thành chữ

        public static string DocTienBangChu(long SoTien, string strTail)
        {
            int lan, i;
            long so;
            string KetQua = "", tmp = "";
            int[] ViTri = new int[6];
            if (SoTien < 0) return "Số tiền âm !";
            if (SoTien == 0) return "Không đồng !";
            if (SoTien > 0)
            {
                so = SoTien;
            }
            else
            {
                so = -SoTien;
            }
            //Kiểm tra số quá lớn
            if (SoTien > 8999999999999999)
            {
                SoTien = 0;
                return "";
            }
            ViTri[5] = (int)(so / 1000000000000000);
            so = so - long.Parse(ViTri[5].ToString()) * 1000000000000000;
            ViTri[4] = (int)(so / 1000000000000);
            so = so - long.Parse(ViTri[4].ToString()) * +1000000000000;
            ViTri[3] = (int)(so / 1000000000);
            so = so - long.Parse(ViTri[3].ToString()) * 1000000000;
            ViTri[2] = (int)(so / 1000000);
            ViTri[1] = (int)((so % 1000000) / 1000);
            ViTri[0] = (int)(so % 1000);
            if (ViTri[5] > 0)
            {
                lan = 5;
            }
            else if (ViTri[4] > 0)
            {
                lan = 4;
            }
            else if (ViTri[3] > 0)
            {
                lan = 3;
            }
            else if (ViTri[2] > 0)
            {
                lan = 2;
            }
            else if (ViTri[1] > 0)
            {
                lan = 1;
            }
            else
            {
                lan = 0;
            }
            for (i = lan; i >= 0; i--)
            {

                tmp = DocSo3ChuSo(ViTri[i]);
                KetQua += tmp;
                if (ViTri[i] != 0) KetQua += Tien[i].ToString();
                if ((i > 0) && (!string.IsNullOrEmpty(tmp))) KetQua += ",";//&& (!string.IsNullOrEmpty(tmp))
            }
            if (KetQua.Substring(KetQua.Length - 1, 1) == ",") KetQua = KetQua.Substring(0, KetQua.Length - 1);
            KetQua = KetQua.Trim() + " " + strTail;
            return KetQua.Substring(0, 1).ToUpper() + KetQua.Substring(1);
        }
        // Hàm đọc số có 3 chữ số
        private static string DocSo3ChuSo(int baso)
        {
            int tram, chuc, donvi;
            string KetQua = "";
            tram = (int)(baso / 100);
            chuc = (int)((baso % 100) / 10);
            donvi = baso % 10;
            if ((tram == 0) && (chuc == 0) && (donvi == 0)) return "";
            if (tram != 0)
            {
                KetQua += ChuSo[tram] + " trăm";
                if ((chuc == 0) && (donvi != 0)) KetQua += " linh";
            }
            if ((chuc != 0) && (chuc != 1))
            {
                KetQua += ChuSo[chuc] + " mươi";
                if ((chuc == 0) && (donvi != 0)) KetQua = KetQua + " linh";
            }
            if (chuc == 1) KetQua += " mười";
            switch (donvi)
            {
                case 1:
                    if ((chuc != 0) && (chuc != 1))
                    {
                        KetQua += " mốt";
                    }
                    else
                    {
                        KetQua += ChuSo[donvi];
                    }
                    break;
                case 5:
                    if (chuc == 0)
                    {
                        KetQua += ChuSo[donvi];
                    }
                    else
                    {
                        KetQua += " năm";
                    }
                    break;
                default:
                    if (donvi != 0)
                    {
                        KetQua += ChuSo[donvi];
                    }
                    break;
            }
            return KetQua;
        }
        #endregion
        #region "String function"
        public static string RTESate(string strText)
        {
            string tmString = "";
            tmString = strText.Trim();
            tmString = tmString.Replace(char.ConvertFromUtf32(145), char.ConvertFromUtf32(39));
            tmString = tmString.Replace(char.ConvertFromUtf32(146), char.ConvertFromUtf32(39));
            tmString = tmString.Replace("'", "&#39;");

            tmString = tmString.Replace(char.ConvertFromUtf32(147), char.ConvertFromUtf32(34));
            tmString = tmString.Replace(char.ConvertFromUtf32(148), char.ConvertFromUtf32(34));
            tmString = tmString.Replace(char.ConvertFromUtf32(10), " ");
            tmString = tmString.Replace(char.ConvertFromUtf32(13), " ");

            return tmString;
        }
     public   static string ConvertStringArrayToStringJoin(string[] array)
        {
            // Use string Join to concatenate the string elements.
            string result = string.Join(".", array);
            return result;
        }
        /// <summary>
        ///Chuan hoa ho Ten Nguyen Tam Cuong
        /// </summary>
        /// <returns></returns>

        public static string StandardString(string strInput)
        {
            string strResult = "";
            if (strInput.Length > 0)
            {
                strInput = strInput.Trim().ToLower();
                while (strInput.Contains("  "))
                    strInput = strInput.Replace("  ", " ");

                string[] arrResult = strInput.Split(' ');
                foreach (string item in arrResult)
                    strResult += item.Substring(0, 1).ToUpper() + item.Substring(1) + " ";
            }
            return strResult.TrimEnd();
        }
        //Lay danh sach theo key
        public static List<string> GetListByKey(string text, string key)
        {
            List<string> listString = new List<string>();
            if (text != null)
            {
                if (text.Substring(text.Length - 1, 1).Equals(key))
                    text = text.Remove(text.Length - 1);
                while (text.IndexOf(key) != -1)
                {
                    string str = text.Substring(0, text.IndexOf(key));
                    listString.Add(str);
                    text = text.Substring(text.IndexOf(key) + 1);

                }
                listString.Add(text);
            }



            return listString;

        }

        #endregion
        public static bool SendMail(string sMailFrom, string sDisplayName, string sPassword, string sMailTo, string sTite, string sContent)
        {
            try
            {
                // OpenFileDialog dlg = new OpenFileDialog();
                // string filename = dlg.FileName;

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer;
                if (sMailFrom.IndexOf("yahoo") >= 0)
                {
                    SmtpServer = new SmtpClient("smtp.mail.yahoo.com", 25);
                    SmtpServer.EnableSsl = false;
                }
                else
                    if (sMailFrom.IndexOf("gmail") >= 0 || sMailFrom.IndexOf("hpu.edu.vn") > 0)
                {
                    SmtpServer = new SmtpClient("smtp.gmail.com", 587);//or 465
                    SmtpServer.Credentials = new System.Net.NetworkCredential(sMailFrom, sPassword);
                    SmtpServer.EnableSsl = true;

                }
                else
                {
                    int Kytu = -1;
                    Kytu = sMailTo.IndexOf("@") + 1;
                    string smtp = sMailFrom;
                    string str = sMailFrom.Substring(0, Kytu);
                    smtp = smtp.Replace(str, "mail.");

                    SmtpServer = new SmtpClient(smtp);
                    SmtpServer.EnableSsl = false;
                }

                SmtpServer.Timeout = 20000;
                mail.From = new MailAddress(sMailFrom, sDisplayName);
                mail.To.Add(sMailTo);
                mail.Subject = sTite;
                mail.IsBodyHtml = true;
                mail.Body = sContent;
                //  Attachment attachment = new Attachment(filename);
                //  mail.Attachments.Add(attachment);

                SmtpServer.Send(mail);

            }
            catch
            {
                return false;
            }
            return true;
        }

        public static string UploadFile(System.Web.UI.WebControls.FileUpload strObject, string strPath)
        {
            string strFolderpath = "~/Upaloads/Images" + strPath;
            if (strObject.PostedFile != null)
            {
                System.Web.HttpPostedFile pFile = strObject.PostedFile;
                int iFileLen = pFile.ContentLength;
                if (iFileLen == 0)
                {
                    return string.Empty;
                }
                string strEx = System.IO.Path.GetExtension(pFile.FileName);
                switch (strEx.ToLower())
                {
                    case ".gif":
                        break;
                    case ".jpg":
                        break;
                    case ".doc":
                        break;
                    case ".ppt":
                        break;
                    case ".xls":
                        break;
                    default:
                        {
                            return string.Empty;
                        }
                }
                byte[] DataFile = new Byte[iFileLen];
                pFile.InputStream.Read(DataFile, 0, iFileLen);

                string strFileName = System.IO.Path.GetFileName(pFile.FileName);
                int iEx = 0;

                while (System.IO.File.Exists(HttpContext.Current.Server.MapPath(strFolderpath + strFileName)))
                {
                    iEx++;
                    strFileName = System.IO.Path.GetFileNameWithoutExtension(pFile.FileName) + "_" + iEx.ToString() + strEx;
                }
                System.IO.FileStream newFile = new System.IO.FileStream(HttpContext.Current.Server.MapPath(strFolderpath + strFileName), System.IO.FileMode.Create);
                newFile.Write(DataFile, 0, DataFile.Length);
                newFile.Close();
                return strFileName;
            }
            else
            {
                return string.Empty;
            }

        }

    }
}