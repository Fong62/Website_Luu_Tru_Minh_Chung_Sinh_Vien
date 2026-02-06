using HoatDongSinhVien.Models.Data;
using HoatDongSinhVien.Models;
using HoatDongSinhVien.Models.ViewModels; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HoatDongSinhVien.Pages.Student
{
    public class XemDanhSachHoatDongModel : PageModel
    {
        private readonly HoatDongSinhVienDbcontext _context;

        public XemDanhSachHoatDongModel(HoatDongSinhVienDbcontext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public DateTime? NgayToChucFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public string LinhVucFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public string TenHoatDongFilter { get; set; }

        public IList<HoatDong> HoatDongs { get; set; }

        public IList<LinhVuc> LinhVucList { get; set; }

        public Paging PagingInfo{ get; set; }

        public IActionResult OnGet(int trang = 1)
        {
            Console.WriteLine($"Page requested: {trang}");

            LinhVucList = _context.LinhVucs.ToList();
            var query = _context.HoatDongs.Include(h => h.LinhVuc).AsQueryable();
            
            // Áp dụng các bộ lọc
            if (NgayToChucFilter.HasValue)
            {
                query = query.Where(h => h.NgayToChuc.Date == NgayToChucFilter.Value.Date);
            }

            if (!string.IsNullOrEmpty(LinhVucFilter))
            {
                query = query.Where(h => h.LinhVuc.IDLinhVuc == LinhVucFilter);
            }

            if (!string.IsNullOrEmpty(TenHoatDongFilter))
            {
                query = query.Where(h => h.TenHoatDong.Contains(TenHoatDongFilter));
            }

          
            int totalItems = query.Count();

            PagingInfo = new Paging
            {
                TotalItems = totalItems,
                ItemPerPage = 6,
                CurrentPage = trang
            };

            HoatDongs = query.Skip((trang - 1) * PagingInfo.ItemPerPage)
                             .Take(PagingInfo.ItemPerPage)
                             .ToList();

            this.NgayToChucFilter = NgayToChucFilter;
            this.LinhVucFilter = LinhVucFilter;
            this.TenHoatDongFilter = TenHoatDongFilter;

            return Page();
        }
    }
}
