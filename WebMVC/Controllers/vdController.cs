using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebMVC.Models;

namespace WebMVC.Controllers
{
    public class vdController : ApiController
    {
        private TTTT3Entities1 db = new TTTT3Entities1();        
        // GET: api/vd
        public List<thongkeAll> Get_vd()
        {
            List<thongkeAll> thongketatca = new List<thongkeAll>();
            thongkeAll tkall = new thongkeAll();
            tkall.soluongdathu = db.PHIEUTHUs.ToList().FindAll(x => x.TRANGTHAIPHIEU == true && x.TRANGTHAIHUY == false).Count;
            tkall.soluongchuathu = db.PHIEUTHUs.ToList().FindAll(x => x.TRANGTHAIPHIEU == false && x.TRANGTHAIHUY == false).Count;
            tkall.soluongphieuhuy = db.PHIEUTHUs.ToList().FindAll(x => x.TRANGTHAIHUY == true).Count;
            tkall.soluongtong = tkall.soluongchuathu + tkall.soluongdathu + tkall.soluongphieuhuy;
            tkall.phantramdathu = (100 / (tkall.soluongchuathu + tkall.soluongdathu)) * tkall.soluongdathu;
            tkall.phantramchuathu = 100 - tkall.phantramdathu;
            tkall.tenkythu = "Tất cả kỳ thu";
            tkall.tentuyenthu = "Tất cả tuyến thu";
            tkall.tongtien = 0;
            foreach (PHIEUTHU pt in db.PHIEUTHUs.ToList())
            {
                if (pt.TRANGTHAIPHIEU == true && pt.TRANGTHAIHUY == false)
                {
                    CHITIETPHIEUTHU ct = db.CHITIETPHIEUTHUs.ToList().Find(x => x.IDPHIEU == pt.IDPHIEU);
                    if (ct != null)
                        tkall.tongtien += (int)ct.SOTIEN;
                }

            }
            thongketatca.Add(tkall);

            return thongketatca;
        }

        // POST: api/vd
        public IEnumerable<thongkeAll> Post(ThongKe tk)
        {
            List<PHIEUTHU> dsPhieuthu = db.PHIEUTHUs.ToList();
            soluong thongkesoluong = new soluong();
            thongkesoluong.dsphieuthu = new List<PHIEUTHU>();
            int demsoluong = 0;
            int tinhtongtien = 0;
            int flag = 0;
            //Nhập đủ ngày
            if (tk.NgayBatDau.Year != 0001 && tk.NgayKetThuc.Year != 0001)
            {
                flag = 0;
                foreach (PHIEUTHU pt in dsPhieuthu)
                {

                    DateTime ngaycapnhat = (DateTime)pt.NGAYCAPNHAT;
                    int kq = DateTime.Compare(new DateTime(tk.NgayBatDau.Year, tk.NgayBatDau.Month, tk.NgayBatDau.Day),
                        new DateTime(ngaycapnhat.Year, ngaycapnhat.Month, ngaycapnhat.Day));
                    int kq1 = DateTime.Compare(new DateTime(tk.NgayKetThuc.Year, tk.NgayKetThuc.Month, tk.NgayKetThuc.Day),
                        new DateTime(ngaycapnhat.Year, ngaycapnhat.Month, ngaycapnhat.Day));

                    if (kq <= 0 && kq1 >= 0)
                    {
                        demsoluong++;
                        CHITIETPHIEUTHU ct = db.CHITIETPHIEUTHUs.Where(x => x.IDPHIEU == pt.IDPHIEU).FirstOrDefault();
                        if (ct != null)
                        {
                            tinhtongtien += (int)ct.SOTIEN;
                            thongkesoluong.ngay = pt.NGAYCAPNHAT + "";
                            pt.KHACHHANG = db.KHACHHANGs.ToList().Find(c => c.IDKHACHHANG == pt.IDKHACHHANG);
                            pt.KHACHHANG.TUYENTHU = db.TUYENTHUs.ToList().Find(c => c.IDTUYENTHU == pt.KHACHHANG.IDTUYENTHU);
                            pt.KHACHHANG.TUYENTHU.KHACHHANGs = null;
                            pt.KHACHHANG.TUYENTHU.XAPHUONG = db.XAPHUONGs.ToList().Find(x => x.IDXAPHUONG == pt.KHACHHANG.TUYENTHU.IDXAPHUONG);
                            pt.KHACHHANG.TUYENTHU.XAPHUONG.QUANHUYEN = db.QUANHUYENs.ToList().Find(x => x.IDQUANHUYEN == pt.KHACHHANG.TUYENTHU.XAPHUONG.IDQUANHUYEN);
                            pt.KYTHU = db.KYTHUs.ToList().Find(x => x.IDKYTHU == pt.IDKYTHU);
                            pt.KYTHU.PHIEUTHUs = null;
                            pt.KHACHHANG.TUYENTHU.XAPHUONG.QUANHUYEN.XAPHUONGs = null;
                            List<PHANQUYENTUYENTHU> qtts = db.PHANQUYENTUYENTHUs.Where(x => x.IDTUYENTHU == pt.KHACHHANG.IDTUYENTHU).ToList();
                            pt.KHACHHANG.TUYENTHU.PHANQUYENTUYENTHUs = qtts;
                            foreach (PHANQUYENTUYENTHU qtt in qtts)
                            {

                                qtt.NHANVIEN = db.NHANVIENs.SingleOrDefault(x => x.IDNHANVIEN == qtt.IDNHANVIEN);
                            }
                            thongkesoluong.dsphieuthu.Add(pt);
                            flag = 1;
                        }

                    }
                }
                if (flag == 0)
                {
                    List<thongkeAll> tall = new List<thongkeAll>();
                    return tall;
                }
            }
            //nhập ngày bắt đầu trống ngày kthuc
            if (tk.NgayBatDau.Year != 0001 && tk.NgayKetThuc.Year == 0001)
            {
                flag = 0;
                foreach (PHIEUTHU pt in dsPhieuthu)
                {

                    DateTime ngaycapnhat = (DateTime)pt.NGAYCAPNHAT;
                    int kq = DateTime.Compare(new DateTime(tk.NgayBatDau.Year, tk.NgayBatDau.Month, tk.NgayBatDau.Day),
                        new DateTime(ngaycapnhat.Year, ngaycapnhat.Month, ngaycapnhat.Day));

                    if (kq <= 0)
                    {
                        demsoluong++;
                        CHITIETPHIEUTHU ct = db.CHITIETPHIEUTHUs.Where(x => x.IDPHIEU == pt.IDPHIEU).FirstOrDefault();
                        if (ct != null)
                        {
                            tinhtongtien += (int)ct.SOTIEN;
                            thongkesoluong.ngay = pt.NGAYCAPNHAT + "";
                            pt.KHACHHANG = db.KHACHHANGs.ToList().Find(c => c.IDKHACHHANG == pt.IDKHACHHANG);
                            pt.KHACHHANG.TUYENTHU = db.TUYENTHUs.ToList().Find(c => c.IDTUYENTHU == pt.KHACHHANG.IDTUYENTHU);
                            pt.KHACHHANG.TUYENTHU.KHACHHANGs = null;
                            pt.KHACHHANG.TUYENTHU.XAPHUONG = db.XAPHUONGs.ToList().Find(x => x.IDXAPHUONG == pt.KHACHHANG.TUYENTHU.IDXAPHUONG);
                            pt.KHACHHANG.TUYENTHU.XAPHUONG.QUANHUYEN = db.QUANHUYENs.ToList().Find(x => x.IDQUANHUYEN == pt.KHACHHANG.TUYENTHU.XAPHUONG.IDQUANHUYEN);
                            pt.KYTHU = db.KYTHUs.ToList().Find(x => x.IDKYTHU == pt.IDKYTHU);
                            pt.KYTHU.PHIEUTHUs = null;
                            pt.KHACHHANG.TUYENTHU.XAPHUONG.QUANHUYEN.XAPHUONGs = null;
                            List<PHANQUYENTUYENTHU> qtts = db.PHANQUYENTUYENTHUs.Where(x => x.IDTUYENTHU == pt.KHACHHANG.IDTUYENTHU).ToList();
                            pt.KHACHHANG.TUYENTHU.PHANQUYENTUYENTHUs = qtts;
                            foreach (PHANQUYENTUYENTHU qtt in qtts)
                            {

                                qtt.NHANVIEN = db.NHANVIENs.SingleOrDefault(x => x.IDNHANVIEN == qtt.IDNHANVIEN);
                            }
                            thongkesoluong.dsphieuthu.Add(pt);
                            flag = 1;
                        }

                    }
                }
                if (flag == 0)
                {
                    List<thongkeAll> tall = new List<thongkeAll>();
                    return tall;
                }
            }
            //nhap ngay ket thuc trong ngay bat dau
            if (tk.NgayBatDau.Year == 0001 && tk.NgayKetThuc.Year != 0001)
            {
                flag = 0;
                foreach (PHIEUTHU pt in dsPhieuthu)
                {

                    DateTime ngaycapnhat = (DateTime)pt.NGAYCAPNHAT;
                    int kq1 = DateTime.Compare(new DateTime(tk.NgayKetThuc.Year, tk.NgayKetThuc.Month, tk.NgayKetThuc.Day),
                       new DateTime(ngaycapnhat.Year, ngaycapnhat.Month, ngaycapnhat.Day));

                    if (kq1 >= 0)
                    {
                        demsoluong++;
                        CHITIETPHIEUTHU ct = db.CHITIETPHIEUTHUs.Where(x => x.IDPHIEU == pt.IDPHIEU).FirstOrDefault();
                        if (ct != null)
                        {
                            tinhtongtien += (int)ct.SOTIEN;
                            thongkesoluong.ngay = pt.NGAYCAPNHAT + "";
                            pt.KHACHHANG = db.KHACHHANGs.ToList().Find(c => c.IDKHACHHANG == pt.IDKHACHHANG);
                            pt.KHACHHANG.TUYENTHU = db.TUYENTHUs.ToList().Find(c => c.IDTUYENTHU == pt.KHACHHANG.IDTUYENTHU);
                            pt.KHACHHANG.TUYENTHU.KHACHHANGs = null;
                            pt.KHACHHANG.TUYENTHU.XAPHUONG = db.XAPHUONGs.ToList().Find(x => x.IDXAPHUONG == pt.KHACHHANG.TUYENTHU.IDXAPHUONG);
                            pt.KHACHHANG.TUYENTHU.XAPHUONG.QUANHUYEN = db.QUANHUYENs.ToList().Find(x => x.IDQUANHUYEN == pt.KHACHHANG.TUYENTHU.XAPHUONG.IDQUANHUYEN);
                            pt.KYTHU = db.KYTHUs.ToList().Find(x => x.IDKYTHU == pt.IDKYTHU);
                            pt.KYTHU.PHIEUTHUs = null;
                            pt.KHACHHANG.TUYENTHU.XAPHUONG.QUANHUYEN.XAPHUONGs = null;
                            List<PHANQUYENTUYENTHU> qtts = db.PHANQUYENTUYENTHUs.Where(x => x.IDTUYENTHU == pt.KHACHHANG.IDTUYENTHU).ToList();
                            pt.KHACHHANG.TUYENTHU.PHANQUYENTUYENTHUs = qtts;
                            foreach (PHANQUYENTUYENTHU qtt in qtts)
                            {

                                qtt.NHANVIEN = db.NHANVIENs.SingleOrDefault(x => x.IDNHANVIEN == qtt.IDNHANVIEN);
                            }
                            thongkesoluong.dsphieuthu.Add(pt);
                            flag = 1;
                        }

                    }
                }
                if (flag == 0)
                {
                    List<thongkeAll> tall = new List<thongkeAll>();
                    return tall;
                }
            }

            //id quan

            if (tk.idQuan != 0)
            {
                int dem = thongkesoluong.dsphieuthu.Count;
                flag = 0;
                if (dem != 0)
                {

                    List<PHIEUTHU> temp_phieuthu = thongkesoluong.dsphieuthu;
                    thongkesoluong.dsphieuthu = new List<PHIEUTHU>();
                    demsoluong = 0;
                    tinhtongtien = 0;

                    foreach (PHIEUTHU pt in temp_phieuthu)
                    {
                        if (tk.idQuan == pt.KHACHHANG.TUYENTHU.XAPHUONG.IDQUANHUYEN)
                        {
                            CHITIETPHIEUTHU ct = db.CHITIETPHIEUTHUs.Where(x => x.IDPHIEU == pt.IDPHIEU).FirstOrDefault();
                            if (ct != null)
                            {
                                demsoluong++;
                                tinhtongtien += (int)ct.SOTIEN;
                                thongkesoluong.ngay = pt.NGAYCAPNHAT + "";
                                pt.KHACHHANG.TUYENTHU.XAPHUONG.QUANHUYEN.XAPHUONGs = null;
                                List<PHANQUYENTUYENTHU> qtts = db.PHANQUYENTUYENTHUs.Where(x => x.IDTUYENTHU == pt.KHACHHANG.IDTUYENTHU).ToList();
                                pt.KHACHHANG.TUYENTHU.PHANQUYENTUYENTHUs = qtts;
                                foreach (PHANQUYENTUYENTHU qtt in qtts)
                                {

                                    qtt.NHANVIEN = db.NHANVIENs.SingleOrDefault(x => x.IDNHANVIEN == qtt.IDNHANVIEN);
                                }
                                flag = 1;
                                thongkesoluong.dsphieuthu.Add(pt);
                            }

                        }
                    }

                }
                else   //idquan khi ds phieu=null
                {
                    foreach (PHIEUTHU pt in dsPhieuthu)
                    {


                        CHITIETPHIEUTHU ct = db.CHITIETPHIEUTHUs.Where(x => x.IDPHIEU == pt.IDPHIEU).FirstOrDefault();
                        if (ct != null)
                        {

                            thongkesoluong.ngay = pt.NGAYCAPNHAT + "";
                            pt.KHACHHANG = db.KHACHHANGs.ToList().Find(c => c.IDKHACHHANG == pt.IDKHACHHANG);
                            pt.KHACHHANG.TUYENTHU = db.TUYENTHUs.ToList().Find(c => c.IDTUYENTHU == pt.KHACHHANG.IDTUYENTHU);
                            pt.KHACHHANG.TUYENTHU.KHACHHANGs = null;
                            pt.KHACHHANG.TUYENTHU.XAPHUONG = db.XAPHUONGs.ToList().Find(x => x.IDXAPHUONG == pt.KHACHHANG.TUYENTHU.IDXAPHUONG);
                            pt.KHACHHANG.TUYENTHU.XAPHUONG.QUANHUYEN = db.QUANHUYENs.ToList().Find(x => x.IDQUANHUYEN == pt.KHACHHANG.TUYENTHU.XAPHUONG.IDQUANHUYEN);
                            pt.KYTHU = db.KYTHUs.ToList().Find(x => x.IDKYTHU == pt.IDKYTHU);
                            pt.KYTHU.PHIEUTHUs = null;
                            pt.KHACHHANG.TUYENTHU.XAPHUONG.QUANHUYEN.XAPHUONGs = null;
                            pt.KHACHHANG.PHIEUTHUs = null;
                            List<PHANQUYENTUYENTHU> qtts = db.PHANQUYENTUYENTHUs.Where(x => x.IDTUYENTHU == pt.KHACHHANG.IDTUYENTHU).ToList();
                            pt.KHACHHANG.TUYENTHU.PHANQUYENTUYENTHUs = qtts;
                            foreach (PHANQUYENTUYENTHU qtt in qtts)
                            {

                                qtt.NHANVIEN = db.NHANVIENs.SingleOrDefault(x => x.IDNHANVIEN == qtt.IDNHANVIEN);
                            }
                            if (pt.KHACHHANG.TUYENTHU.XAPHUONG.IDQUANHUYEN == tk.idQuan)
                            {
                                tinhtongtien += (int)ct.SOTIEN;
                                demsoluong++;
                                thongkesoluong.dsphieuthu.Add(pt);
                                flag = 1;
                            }
                        }
                    }
                }
                if (flag == 0)
                {
                    List<thongkeAll> tall = new List<thongkeAll>();
                    return tall;
                }

            }

            //id xa
            if (tk.idXa != 0)
            {
                flag = 0;
                int dem = thongkesoluong.dsphieuthu.Count;
                if (dem != 0)
                {

                    List<PHIEUTHU> temp_phieuthu = thongkesoluong.dsphieuthu;
                    thongkesoluong.dsphieuthu = new List<PHIEUTHU>();
                    demsoluong = 0;
                    tinhtongtien = 0;

                    foreach (PHIEUTHU pt in temp_phieuthu)
                    {
                        if (tk.idXa == pt.KHACHHANG.TUYENTHU.XAPHUONG.IDXAPHUONG)
                        {
                            CHITIETPHIEUTHU ct = db.CHITIETPHIEUTHUs.Where(x => x.IDPHIEU == pt.IDPHIEU).FirstOrDefault();
                            if (ct != null)
                            {
                                demsoluong++;
                                tinhtongtien += (int)ct.SOTIEN;
                                thongkesoluong.ngay = pt.NGAYCAPNHAT + "";
                                List<PHANQUYENTUYENTHU> qtts = db.PHANQUYENTUYENTHUs.Where(x => x.IDTUYENTHU == pt.KHACHHANG.IDTUYENTHU).ToList();
                                pt.KHACHHANG.TUYENTHU.PHANQUYENTUYENTHUs = qtts;
                                foreach (PHANQUYENTUYENTHU qtt in qtts)
                                {

                                    qtt.NHANVIEN = db.NHANVIENs.SingleOrDefault(x => x.IDNHANVIEN == qtt.IDNHANVIEN);
                                }
                                pt.KHACHHANG.TUYENTHU.XAPHUONG.QUANHUYEN.XAPHUONGs = null;
                                thongkesoluong.dsphieuthu.Add(pt);
                                flag = 1;
                            }

                        }
                    }

                }
                else   //idxa khi ds phieu=null
                {
                    foreach (PHIEUTHU pt in dsPhieuthu)
                    {
                        CHITIETPHIEUTHU ct = db.CHITIETPHIEUTHUs.Where(x => x.IDPHIEU == pt.IDPHIEU).FirstOrDefault();
                        if (ct != null)
                        {

                            thongkesoluong.ngay = pt.NGAYCAPNHAT + "";
                            pt.KHACHHANG = db.KHACHHANGs.ToList().Find(c => c.IDKHACHHANG == pt.IDKHACHHANG);
                            pt.KHACHHANG.TUYENTHU = db.TUYENTHUs.ToList().Find(c => c.IDTUYENTHU == pt.KHACHHANG.IDTUYENTHU);
                            pt.KHACHHANG.TUYENTHU.KHACHHANGs = null;
                            pt.KHACHHANG.TUYENTHU.XAPHUONG = db.XAPHUONGs.ToList().Find(x => x.IDXAPHUONG == pt.KHACHHANG.TUYENTHU.IDXAPHUONG);
                            pt.KHACHHANG.TUYENTHU.XAPHUONG.QUANHUYEN = db.QUANHUYENs.ToList().Find(x => x.IDQUANHUYEN == pt.KHACHHANG.TUYENTHU.XAPHUONG.IDQUANHUYEN);
                            pt.KYTHU = db.KYTHUs.ToList().Find(x => x.IDKYTHU == pt.IDKYTHU);
                            pt.KYTHU.PHIEUTHUs = null;
                            pt.KHACHHANG.TUYENTHU.XAPHUONG.QUANHUYEN.XAPHUONGs = null;
                            pt.KHACHHANG.PHIEUTHUs = null;
                            List<PHANQUYENTUYENTHU> qtts = db.PHANQUYENTUYENTHUs.Where(x => x.IDTUYENTHU == pt.KHACHHANG.IDTUYENTHU).ToList();
                            pt.KHACHHANG.TUYENTHU.PHANQUYENTUYENTHUs = qtts;
                            foreach (PHANQUYENTUYENTHU qtt in qtts)
                            {

                                qtt.NHANVIEN = db.NHANVIENs.SingleOrDefault(x => x.IDNHANVIEN == qtt.IDNHANVIEN);
                            }
                            if (pt.KHACHHANG.TUYENTHU.XAPHUONG.IDXAPHUONG == tk.idXa)
                            {
                                tinhtongtien += (int)ct.SOTIEN;
                                demsoluong++;
                                thongkesoluong.dsphieuthu.Add(pt);
                                flag = 1;
                            }
                        }


                    }
                }
                if (flag == 0)
                {
                    List<thongkeAll> tall = new List<thongkeAll>();
                    return tall;
                }

            }
            //tuyen
            if (tk.idtuyen != 0)
            {
                flag = 0;
                int dem = thongkesoluong.dsphieuthu.Count;
                if (dem != 0)
                {

                    List<PHIEUTHU> temp_phieuthu = thongkesoluong.dsphieuthu;
                    thongkesoluong.dsphieuthu = new List<PHIEUTHU>();
                    demsoluong = 0;
                    tinhtongtien = 0;

                    foreach (PHIEUTHU pt in temp_phieuthu)
                    {
                        if (tk.idtuyen == pt.KHACHHANG.TUYENTHU.IDTUYENTHU)
                        {
                            CHITIETPHIEUTHU ct = db.CHITIETPHIEUTHUs.Where(x => x.IDPHIEU == pt.IDPHIEU).FirstOrDefault();
                            if (ct != null)
                            {
                                demsoluong++;
                                tinhtongtien += (int)ct.SOTIEN;
                                thongkesoluong.ngay = pt.NGAYCAPNHAT + "";
                                pt.KHACHHANG.TUYENTHU.XAPHUONG.QUANHUYEN.XAPHUONGs = null;
                                List<PHANQUYENTUYENTHU> qtts = db.PHANQUYENTUYENTHUs.Where(x => x.IDTUYENTHU == pt.KHACHHANG.IDTUYENTHU).ToList();
                                pt.KHACHHANG.TUYENTHU.PHANQUYENTUYENTHUs = qtts;
                                foreach (PHANQUYENTUYENTHU qtt in qtts)
                                {

                                    qtt.NHANVIEN = db.NHANVIENs.SingleOrDefault(x => x.IDNHANVIEN == qtt.IDNHANVIEN);
                                }
                                flag = 1;
                                thongkesoluong.dsphieuthu.Add(pt);
                            }

                        }
                    }
                }
                else   
                {
                    foreach (PHIEUTHU pt in dsPhieuthu)
                    {
                        CHITIETPHIEUTHU ct = db.CHITIETPHIEUTHUs.Where(x => x.IDPHIEU == pt.IDPHIEU).FirstOrDefault();
                        if (ct != null)
                        {

                            thongkesoluong.ngay = pt.NGAYCAPNHAT + "";
                            pt.KHACHHANG = db.KHACHHANGs.ToList().Find(c => c.IDKHACHHANG == pt.IDKHACHHANG);
                            pt.KHACHHANG.TUYENTHU = db.TUYENTHUs.ToList().Find(c => c.IDTUYENTHU == pt.KHACHHANG.IDTUYENTHU);
                            pt.KHACHHANG.TUYENTHU.KHACHHANGs = null;
                            pt.KHACHHANG.TUYENTHU.XAPHUONG = db.XAPHUONGs.ToList().Find(x => x.IDXAPHUONG == pt.KHACHHANG.TUYENTHU.IDXAPHUONG);
                            pt.KHACHHANG.TUYENTHU.XAPHUONG.QUANHUYEN = db.QUANHUYENs.ToList().Find(x => x.IDQUANHUYEN == pt.KHACHHANG.TUYENTHU.XAPHUONG.IDQUANHUYEN);
                            pt.KYTHU = db.KYTHUs.ToList().Find(x => x.IDKYTHU == pt.IDKYTHU);
                            pt.KYTHU.PHIEUTHUs = null;
                            pt.KHACHHANG.TUYENTHU.XAPHUONG.QUANHUYEN.XAPHUONGs = null;
                            pt.KHACHHANG.PHIEUTHUs = null;
                            List<PHANQUYENTUYENTHU> qtts = db.PHANQUYENTUYENTHUs.Where(x => x.IDTUYENTHU == pt.KHACHHANG.IDTUYENTHU).ToList();
                            pt.KHACHHANG.TUYENTHU.PHANQUYENTUYENTHUs = qtts;
                            foreach (PHANQUYENTUYENTHU qtt in qtts)
                            {

                                qtt.NHANVIEN = db.NHANVIENs.SingleOrDefault(x => x.IDNHANVIEN == qtt.IDNHANVIEN);
                            }
                            if (pt.KHACHHANG.TUYENTHU.IDTUYENTHU == tk.idtuyen)
                            {
                                tinhtongtien += (int)ct.SOTIEN;
                                demsoluong++;
                                thongkesoluong.dsphieuthu.Add(pt);
                                flag = 1;
                            }
                        }
                    }
                }
                if (flag == 0)
                {
                    List<thongkeAll> tall = new List<thongkeAll>();
                    return tall;
                }

            }
            //kythu
            if (tk.tenkythu != null)
            {
                flag = 0;
                int dem = thongkesoluong.dsphieuthu.Count;
                KYTHU kYTHU = db.KYTHUs.SingleOrDefault(x => x.TENKYTHU == tk.tenkythu);
                if(kYTHU == null)
                {
                    List<thongkeAll> tall = new List<thongkeAll>();
                    return tall;
                }
                if (dem != 0)
                {

                    List<PHIEUTHU> temp_phieuthu = thongkesoluong.dsphieuthu;
                    thongkesoluong.dsphieuthu = new List<PHIEUTHU>();
                    demsoluong = 0;
                    tinhtongtien = 0;

                    foreach (PHIEUTHU pt in temp_phieuthu)
                    {
                        if (kYTHU.IDKYTHU == pt.IDKYTHU && kYTHU != null)
                        {
                            CHITIETPHIEUTHU ct = db.CHITIETPHIEUTHUs.Where(x => x.IDPHIEU == pt.IDPHIEU).FirstOrDefault();
                            if (ct != null)
                            {
                                demsoluong++;
                                tinhtongtien += (int)ct.SOTIEN;
                                thongkesoluong.ngay = pt.NGAYCAPNHAT + "";
                                List<PHANQUYENTUYENTHU> qtts = db.PHANQUYENTUYENTHUs.Where(x => x.IDTUYENTHU == pt.KHACHHANG.IDTUYENTHU).ToList();
                                pt.KHACHHANG.TUYENTHU.PHANQUYENTUYENTHUs = qtts;
                                foreach (PHANQUYENTUYENTHU qtt in qtts)
                                {

                                    qtt.NHANVIEN = db.NHANVIENs.SingleOrDefault(x => x.IDNHANVIEN == qtt.IDNHANVIEN);
                                }
                                pt.KHACHHANG.TUYENTHU.XAPHUONG.QUANHUYEN.XAPHUONGs = null;

                                flag = 1;
                                thongkesoluong.dsphieuthu.Add(pt);
                            }

                        }
                    }
                }
                else   //idkythukhi ds phieu=null
                {
                    foreach (PHIEUTHU pt in dsPhieuthu)
                    {
                        CHITIETPHIEUTHU ct = db.CHITIETPHIEUTHUs.Where(x => x.IDPHIEU == pt.IDPHIEU).FirstOrDefault();
                        if (ct != null)
                        {

                            thongkesoluong.ngay = pt.NGAYCAPNHAT + "";
                            pt.KHACHHANG = db.KHACHHANGs.ToList().Find(c => c.IDKHACHHANG == pt.IDKHACHHANG);
                            pt.KHACHHANG.TUYENTHU = db.TUYENTHUs.ToList().Find(c => c.IDTUYENTHU == pt.KHACHHANG.IDTUYENTHU);
                            pt.KHACHHANG.TUYENTHU.KHACHHANGs = null;
                            pt.KHACHHANG.TUYENTHU.XAPHUONG = db.XAPHUONGs.ToList().Find(x => x.IDXAPHUONG == pt.KHACHHANG.TUYENTHU.IDXAPHUONG);
                            pt.KHACHHANG.TUYENTHU.XAPHUONG.QUANHUYEN = db.QUANHUYENs.ToList().Find(x => x.IDQUANHUYEN == pt.KHACHHANG.TUYENTHU.XAPHUONG.IDQUANHUYEN);
                            pt.KYTHU = db.KYTHUs.ToList().Find(x => x.IDKYTHU == pt.IDKYTHU);
                            pt.KYTHU.PHIEUTHUs = null;
                            pt.KHACHHANG.TUYENTHU.XAPHUONG.QUANHUYEN.XAPHUONGs = null;
                            pt.KHACHHANG.PHIEUTHUs = null;
                            List<PHANQUYENTUYENTHU> qtts = db.PHANQUYENTUYENTHUs.Where(x => x.IDTUYENTHU == pt.KHACHHANG.IDTUYENTHU).ToList();
                            pt.KHACHHANG.TUYENTHU.PHANQUYENTUYENTHUs = qtts;
                            foreach (PHANQUYENTUYENTHU qtt in qtts)
                            {

                                qtt.NHANVIEN = db.NHANVIENs.SingleOrDefault(x => x.IDNHANVIEN == qtt.IDNHANVIEN);
                            }

                            if (pt.IDKYTHU == kYTHU.IDKYTHU)
                            {
                                tinhtongtien += (int)ct.SOTIEN;
                                demsoluong++;
                                thongkesoluong.dsphieuthu.Add(pt);
                                flag = 1;
                            }
                        }
                    }
                }
                if (flag == 0)
                {
                    List<thongkeAll> tall = new List<thongkeAll>();
                    return tall;
                }
            }


            var kt_tong = (from p in thongkesoluong.dsphieuthu
                           from k in db.KYTHUs
                           where p.IDKYTHU == k.IDKYTHU
                           from h in db.KHACHHANGs
                           where h.IDKHACHHANG == p.IDKHACHHANG
                           from ct in db.CHITIETPHIEUTHUs
                           where ct.IDPHIEU == p.IDPHIEU
                           group p by new { k.IDKYTHU, h.IDTUYENTHU, k.TENKYTHU }
                         into grp
                           from t in db.TUYENTHUs
                           where t.IDTUYENTHU == grp.Key.IDTUYENTHU
                           select new thongkeAll
                           {
                               idnhanvien = t.IDTUYENTHU,
                               tenkythu = grp.Key.TENKYTHU,
                               tentuyenthu = t.TENTUYENTHU,
                               soluongtong = grp.Count(),
                               tongtien = 0,
                               lpq = t.PHANQUYENTUYENTHUs.ToList()
                           }).ToList();

            var kt_pt = (from p in thongkesoluong.dsphieuthu
                         from k in db.KYTHUs
                         where p.IDKYTHU == k.IDKYTHU
                         from h in db.KHACHHANGs
                         where h.IDKHACHHANG == p.IDKHACHHANG
                         from ct in db.CHITIETPHIEUTHUs
                         where ct.IDPHIEU == p.IDPHIEU
                         where p.TRANGTHAIPHIEU == true && p.TRANGTHAIHUY == false
                         group p by new { k.IDKYTHU, h.IDTUYENTHU, k.TENKYTHU }
                         into grp
                         from t in db.TUYENTHUs
                         where t.IDTUYENTHU == grp.Key.IDTUYENTHU
                         select new thongkeAll
                         {
                             idnhanvien = t.IDTUYENTHU,
                             tenkythu = grp.Key.TENKYTHU,
                             tentuyenthu = t.TENTUYENTHU,
                             nhanvienthu = db.PHANQUYENTUYENTHUs.FirstOrDefault(x => x.IDTUYENTHU == t.IDTUYENTHU).NHANVIEN.HOTEN,
                             soluongdathu = grp.Count(),
                             tongtien = grp.Sum(c => c.CHITIETPHIEUTHUs.FirstOrDefault().SOTIEN),
                             //lpq = t.PHANQUYENTUYENTHUs.ToList()
                         }).ToList();

            var kt_pt1 = (from p in thongkesoluong.dsphieuthu
                          from k in db.KYTHUs
                          where p.IDKYTHU == k.IDKYTHU
                          from h in db.KHACHHANGs
                          where h.IDKHACHHANG == p.IDKHACHHANG
                          from ct in db.CHITIETPHIEUTHUs
                          where ct.IDPHIEU == p.IDPHIEU
                          where p.TRANGTHAIPHIEU == false && p.TRANGTHAIHUY == false
                          group p by new { k.IDKYTHU, h.IDTUYENTHU, k.TENKYTHU }
                         into grp
                          from t in db.TUYENTHUs
                          where t.IDTUYENTHU == grp.Key.IDTUYENTHU
                          select new thongkeAll
                          {
                              idnhanvien = t.IDTUYENTHU,
                              tenkythu = grp.Key.TENKYTHU,
                              tentuyenthu = t.TENTUYENTHU,
                              soluongchuathu = grp.Count(),
                          }).ToList();
            var kt_pt2 = (from p in thongkesoluong.dsphieuthu
                          from k in db.KYTHUs
                          where p.IDKYTHU == k.IDKYTHU
                          from h in db.KHACHHANGs
                          where h.IDKHACHHANG == p.IDKHACHHANG
                          from ct in db.CHITIETPHIEUTHUs
                          where ct.IDPHIEU == p.IDPHIEU
                          where p.TRANGTHAIHUY == true
                          group p by new { k.IDKYTHU, h.IDTUYENTHU, k.TENKYTHU }
                          into grp
                          from t in db.TUYENTHUs
                          where t.IDTUYENTHU == grp.Key.IDTUYENTHU
                          select new thongkeAll
                          {
                              idnhanvien = t.IDTUYENTHU,
                              tenkythu = grp.Key.TENKYTHU,
                              tentuyenthu = t.TENTUYENTHU,
                              soluongphieuhuy = grp.Count(),
                          }).ToList();

            List<thongkeAll> kt_pt3 = new List<thongkeAll>();

            foreach (thongkeAll item in kt_tong)
            {
                foreach (thongkeAll item3 in kt_pt)
                {
                    if (item.idnhanvien == item3.idnhanvien)
                    {
                        item.soluongdathu = item3.soluongdathu;
                        item.nhanvienthu = item3.nhanvienthu;
                        item.tongtien = item3.tongtien;
                    }
                }
                foreach (thongkeAll item1 in kt_pt1)
                {
                    if (item.idnhanvien == item1.idnhanvien)
                    {
                        item.soluongchuathu = item1.soluongchuathu;

                    }
                }
                foreach (thongkeAll item2 in kt_pt2)
                {
                    if (item.idnhanvien == item2.idnhanvien)
                    {
                        item.soluongphieuhuy = item2.soluongphieuhuy;
                    }
                }
            }
            foreach (thongkeAll item in kt_tong)
            {
                item.phantramdathu = (100 / (item.soluongchuathu + item.soluongdathu)) * item.soluongdathu;
                item.phantramchuathu = 100 - item.phantramdathu;
                if (item.lpq != null)
                {
                    foreach (PHANQUYENTUYENTHU pq in item.lpq)
                    {
                        pq.NHANVIEN.PHANQUYENTUYENTHUs = null;
                        pq.TUYENTHU = null;
                    }
                }

            }


            thongkesoluong.soluongthu = demsoluong;
            thongkesoluong.tongtien = tinhtongtien;


            IEnumerable<thongkeAll> kt_pts = kt_tong.OrderBy(p => p.tenkythu).ThenBy(p => p.tentuyenthu);
            return kt_pts;
        }
    }
}