using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebMVC.Models;

namespace WebMVC.Controllers
{
    public class TKnhanvienController : ApiController
    {
        private TTTT3Entities1 db = new TTTT3Entities1();       

        // POST: api/TKnhanvien
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
                        // else return new soluong();



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
                else   //idtuyenkhi ds phieu=null
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
                if (kYTHU == null)
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
                    //return thongketatca();
                }
            }

            //List<thongkeAll> tt = new List<thongkeAll>();
            // tong so phieu cua 1 tuyen thu
            var kt_tong = (from p in thongkesoluong.dsphieuthu
                           from k in db.KYTHUs
                           where p.IDKYTHU == k.IDKYTHU
                           from h in db.KHACHHANGs
                           where h.IDKHACHHANG == p.IDKHACHHANG
                           from ct in db.CHITIETPHIEUTHUs
                           where ct.IDPHIEU == p.IDPHIEU
                           where p.TRANGTHAIHUY == false
                           group p by new { k.IDKYTHU, k.TENKYTHU, h.IDTUYENTHU }
                            into grp
                           from t in db.TUYENTHUs
                           where t.IDTUYENTHU == grp.Key.IDTUYENTHU
                           select new thongkeAll
                           {
                               idnhanvien = t.IDTUYENTHU,
                               tenkythu = grp.Key.TENKYTHU,
                               tentuyenthu = t.TENTUYENTHU,
                               //nhanvienthu = n.HOTEN,
                               soluongtong = grp.Count(),
                               tongtien = 0,
                               //lpq = t.PHANQUYENTUYENTHUs.ToList()
                           }).ToList();
            // tong so phieu da thu cua tung nhan vien trong 1 tuyen
            var kt_pt = (from p in thongkesoluong.dsphieuthu
                         from k in db.KYTHUs
                         where p.IDKYTHU == k.IDKYTHU
                         from h in db.KHACHHANGs
                         where h.IDKHACHHANG == p.IDKHACHHANG
                         from ct in db.CHITIETPHIEUTHUs
                         where ct.IDPHIEU == p.IDPHIEU
                         where p.TRANGTHAIPHIEU == true && p.TRANGTHAIHUY == false
                         group p by new { k.IDKYTHU,h.IDTUYENTHU, p.IDNGUOITHU, k.TENKYTHU }
                         into grp
                         from t in db.TUYENTHUs
                         where t.IDTUYENTHU == grp.Key.IDTUYENTHU
                         from n in db.NHANVIENs
                         where n.IDNHANVIEN == grp.Key.IDNGUOITHU
                         select new thongkeAll
                         {
                             idnhanvien = t.IDTUYENTHU,
                             tenkythu = grp.Key.TENKYTHU,
                             tentuyenthu = t.TENTUYENTHU,
                             nhanvienthu = db.NHANVIENs.SingleOrDefault(x => x.IDNHANVIEN == grp.Key.IDNGUOITHU).HOTEN,
                             soluongdathu = grp.Count(),
                             tongtien = grp.Sum(c => c.CHITIETPHIEUTHUs.FirstOrDefault().SOTIEN),
                         }).ToList();

            var kt_pt1 = (from p in thongkesoluong.dsphieuthu
                          from k in db.KYTHUs
                          where p.IDKYTHU == k.IDKYTHU
                          from h in db.KHACHHANGs
                          where h.IDKHACHHANG == p.IDKHACHHANG
                          from ct in db.CHITIETPHIEUTHUs
                          where ct.IDPHIEU == p.IDPHIEU
                          where p.TRANGTHAIPHIEU == false && p.TRANGTHAIHUY == false
                          group p by new { k.IDKYTHU, k.TENKYTHU, h.IDTUYENTHU }
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
            

            List<thongkeAll> kt_pt3 = new List<thongkeAll>();

            foreach (thongkeAll item in kt_pt)
            {
                foreach (thongkeAll item3 in kt_tong)
                {
                    if (item.idnhanvien == item3.idnhanvien)
                    {
                        item.soluongtong = item3.soluongtong;
                    }
                }
                foreach (thongkeAll item1 in kt_pt1)
                {
                    if (item.idnhanvien == item1.idnhanvien)
                    {
                        item.soluongchuathu = item1.soluongchuathu;

                    }
                }
                
            }
            foreach (thongkeAll item in kt_pt)
            {
                item.phantramdathu = (100 / item.soluongtong) * item.soluongdathu;
                item.phantramchuathu = 100 - item.phantramdathu;
            }


            thongkesoluong.soluongthu = demsoluong;
            thongkesoluong.tongtien = tinhtongtien;


            IEnumerable<thongkeAll> kt_pts = kt_pt.OrderBy(p => p.tenkythu).ThenBy(p => p.tentuyenthu);
            return kt_pts;
        }
    }
}
