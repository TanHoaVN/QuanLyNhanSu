﻿using DevExpress.Data.Filtering;
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
    [Persistent(@"CheckInOut")]
    [DefaultClassOptions]
    [XafDisplayName("Giờ Chấm Công")]
    public class CheckInOut : XPLiteObject
    {
        public CheckInOut(Session session) : base(session) { }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        protected override void OnSaved()
        {
            base.OnSaved();
            try
            {
                NhanVien nhanVien = Session.FindObject<NhanVien>(new BinaryOperator("MaChamCong", this.MaChamCong));
                this.nguoiChamCong = nhanVien;
                Session.CommitTransaction();
            }
            catch { }
        }
        protected override void OnLoaded()
        {
            base.OnLoaded();
            if (!Equals(this.MaChamCong, null) && (Equals(this.nguoiChamCong, null)))
            {
                try
                {
                    NhanVien nhanVien = Session.FindObject<NhanVien>(new BinaryOperator("MaChamCong", this.MaChamCong));
                    this.nguoiChamCong = nhanVien;
                    Session.CommitTransaction();
                }
                catch { }
            }
        }
        int fId;
        [Key(true)]
        [XafDisplayName("STT")]
        public int Id
        {
            get { return fId; }
            set { SetPropertyValue("Id", ref fId, value); }
        }
        NhanVien fNguoiChamCong;
        [XafDisplayName("Người Chấm Công")]
        [Association(@"NhanVien-CheckInOut")]
        public NhanVien nguoiChamCong
        {
            get
            {
                return fNguoiChamCong;
            }
            set { SetPropertyValue("nguoiChamCong", ref fNguoiChamCong, value); }
        }
        int fMaChamCong;
        [XafDisplayName("Mã Chấm Công")]
        public int MaChamCong
        {
            get { return fMaChamCong; }
            set { SetPropertyValue("MaChamCong", ref fMaChamCong, value); }
        }
        DateTime fNgayCham;
        [XafDisplayName("Ngày Chấm")]
        [ModelDefault("DisplayFormat", "{0:dd/MM/yyyy}")]
        [ModelDefault("EditMask", "dd/MM/yyyy")]
        public DateTime NgayCham
        {
            get
            {
                return fNgayCham;
            }
            set { SetPropertyValue("NgayCham", ref fNgayCham, value); }
        }
        DateTime fGioCham;
        [XafDisplayName("Giờ Chấm")]
        [ModelDefault("DisplayFormat", "{0:HH:mm}")]
        [ModelDefault("EditMask", "HH:mm")]
        public DateTime GioCham
        {
            get
            {
                return fGioCham;
            }
            set { SetPropertyValue("GioCham", ref fGioCham, value); }
        }
        public enum LoaiGio
        {
            [XafDisplayName("Giờ Vào Đầu Ca")] vaodauca = 0,
            [XafDisplayName("Giờ Ra Vào Giữa Ca")] ravaogiuaca = 1,
            //[XafDisplayName("Giờ Vào Giữa Ca")] vaogiuaca = 2,
            [XafDisplayName("Giờ Tan Ca")] tanca = 2,
            [XafDisplayName("Không xác định")] khongxacdinh = 3

        }
        [XafDisplayName("Loại Chấm Công")]
        [ModelDefault("AllowEdit", "false")]
        public LoaiGio? loaiChamCong
        {
            get
            {
                if (!Equals(this.nguoiChamCong, null))
                {
                    return kiemTraChamCong(this.nguoiChamCong, this.GioCham);
                }
                else
                {
                    return null;
                }
            }
        }
        int? fIdMCC;
        [XafDisplayName("Số thứ tự máy")]
        public int? idMCC
        {
            get { return fIdMCC; }
            set { SetPropertyValue("idMCC", ref fIdMCC, value); }
        }
        [XafDisplayName("Tên Máy")]
        public string TenMay
        {
            get
            {
                if (!Equals(idMCC, null))
                {
                    MayChamCong mayChamCong = Session.GetObjectByKey<MayChamCong>(this.idMCC);
                    return mayChamCong.tenMCCC;
                }
                else
                {
                    return null;
                }
            }
        }
        GioCong fgioCong;
        [XafDisplayName("Giờ Công")]
        [Association(@"GioCong-CheckInOut")]
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        public GioCong gioCong
        {
            get { return fgioCong; }
            set { SetPropertyValue("gioCong", ref fgioCong, value); }
        }

        /*  Đây là chương trình kiểm tra loại chấm công
         *  input: Nhân viên, thời gian chấm công
         *  Author: Đình Tri
         *  Ngày: 08/05/2019
         */

        private LoaiGio kiemTraChamCong(NhanVien nhanVien, DateTime thoiGianCham)
        {
            DateTime thoiGianVaoCa = nhanVien.caLamViec.thoiGianVao;
            //DateTime thoiGianRaGiuaCa = nhanVien.caLamViec.thoiGianRaGiuaCa;
            //DateTime thoiGianVaoGiuaCa = nhanVien.caLamViec.thoiGianVaoGiuaCa;
            DateTime thoiGianTanCa = nhanVien.caLamViec.thoiGianTanCa;
            if ((thoiGianCham.Hour >= (thoiGianVaoCa.Hour - 1)) && (thoiGianCham.Hour <= (thoiGianVaoCa.Hour + 1)))
            {
                return LoaiGio.vaodauca;
            }
            else if ((thoiGianCham.Hour >= (thoiGianTanCa.Hour - 1)) && (thoiGianCham.Hour <= (thoiGianTanCa.Hour + 1)))
            {
                return LoaiGio.tanca;
            }
            else if ((thoiGianCham.Hour >= (thoiGianVaoCa.Hour + 1)) && (thoiGianCham.Hour <= (thoiGianTanCa.Hour - 1)))
            {
                return LoaiGio.ravaogiuaca;
            }
            else
            {
                return LoaiGio.khongxacdinh;
            }
        }
    }
}
