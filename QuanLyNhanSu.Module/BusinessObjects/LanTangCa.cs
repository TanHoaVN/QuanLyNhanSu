﻿using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyNhanSu.Module.BusinessObjects
{
    [DefaultClassOptions]
    [Persistent(@"LanTangCa")]
    [XafDisplayName("Lần Tăng Ca")]
    [Appearance("ngayDuyet", BackColor = "red", FontColor = "white", Context = "ListView", TargetItems = "nguoiTangCa", Criteria = "ngayDuyet = null")]
    public class LanTangCa:XPLiteObject
    {
        public LanTangCa(Session session) : base(session) { }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        int fId;
        [Key(true)]
        [XafDisplayName("STT")]
        public int Id
        {
            get { return fId; }
            set { SetPropertyValue("Id", ref fId, value); }
        }
        NhanVien fNguoiTangCa;
        [XafDisplayName("Nguời Tăng Ca")]
        [Association(@"NhanVien-LanTangCa")]
        public NhanVien nguoiTangCa
        {
            get { return fNguoiTangCa; }
            set { SetPropertyValue("nguoiTangCa", ref fNguoiTangCa, value); }
        }
        DateTime fNgayTao;
        [XafDisplayName("Ngày Tạo")]
        [ModelDefault("DisplayFormat", "{0:dd/MM/yyyy}")]
        [ModelDefault("EditMask", "dd/MM/yyyy")]
        public DateTime ngayTao
        {
            get { return fNgayTao; }
            set { SetPropertyValue("ngayTao", ref fNgayTao, value); }
        }
        DateTime fNgayTangCa;
        [XafDisplayName("Ngày Tăng Ca")]
        [ModelDefault("DisplayFormat","{0:dd/MM/yyyy}")]
        [ModelDefault("EditMask", "dd/MM/yyyy")]
        public DateTime ngayTangCa
        {
            get { return fNgayTangCa; }
            set { SetPropertyValue("ngayTangCa", ref fNgayTangCa, value); }
        }
        LoaiTangCa fLoaiTangCa;
        [XafDisplayName("Loại Tăng Ca")]
        public LoaiTangCa loaiTangCa
        {
            get { return fLoaiTangCa; }
            set { SetPropertyValue("loaiTangCa", ref fLoaiTangCa, value); }
        }
        [XafDisplayName("Hệ Số Nhân Giờ")]
        public double heSoNhanGio
        {
            get
            {
                if(!Equals(this.loaiTangCa,null))
                {
                    return this.loaiTangCa.heSoNhanGio;
                }
                else
                {
                    return 0;
                }
            }
        }
        DateTime fThoiGianBatDau;
        [XafDisplayName("Thời Gian Bắt Đàu")]
        [ModelDefault("DisplayFormat", "{0:HH:mm}")]
        [ModelDefault("EditMask", "HH:mm")]
        public DateTime thoiGianBatDau
        {
            get { return fThoiGianBatDau; }
            set { SetPropertyValue("thoiGianBatDau", ref fThoiGianBatDau, value); }
        }
        DateTime fThoiGianKetThuc;
        [XafDisplayName("Thời Gian Kết Thúc")]
        [ModelDefault("DisplayFormat", "{0:HH:mm}")]
        [ModelDefault("EditMask", "HH:mm")]
        public DateTime thoiGianKetThuc
        {
            get { return fThoiGianKetThuc; }
            set { SetPropertyValue("thoiGianKetThuc", ref fThoiGianKetThuc, value); }
        }
        NguoiDung fNguoiDuyet;
        [XafDisplayName("Người Duyệt")]
        [ModelDefault("AllowEdit", "false")]

        public NguoiDung nguoiDuyet
        {
            get { return fNguoiDuyet; }
            set { SetPropertyValue("nguoiDuyet", ref fNguoiDuyet, value); }
        }
        DateTime? fNgayDuyet;
        [XafDisplayName("Ngày Duyệt")]
        [ModelDefault("AllowEdit", "false")]

        public DateTime? ngayDuyet
        {
            get { return fNgayDuyet; }
            set { SetPropertyValue("ngayDuyet", ref fNgayDuyet, value); }
        }
        [XafDisplayName("Tổng Thời Gian Tăng Ca")]
        public double thoiGianTangCa
        {
            get
            {
                double thoiGian = 0;
                TimeSpan tongThoiGian = TimeSpan.Zero;
                if(!Equals(this.thoiGianKetThuc, null))
                {
                    tongThoiGian = this.thoiGianKetThuc - this.thoiGianBatDau;
                    thoiGian = (tongThoiGian.TotalMinutes / 60) * this.heSoNhanGio;
                }
                return thoiGian;
            }
        }
        string fLyDo;
        [XafDisplayName("Lý Do")]
        public string lyDo
        {
            get { return fLyDo; }
            set { SetPropertyValue("lyDo", ref fLyDo, value); }
        }
        string fGhiChu;
        [XafDisplayName("Ghi Chú")]
        public string ghiChu
        {
            get { return fGhiChu; }
            set { SetPropertyValue("ghiChu", ref fGhiChu, value); }
        }
        GioCong fgioCong;
        [XafDisplayName("Giờ Công")]
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [Association(@"GiocCong-LanTangCa")]
        public GioCong gioCong
        {
            get { return fgioCong; }
            set { SetPropertyValue("gioCong", ref fgioCong, value); }
        }
    }
}
