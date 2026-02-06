using HoatDongSinhVien.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace HoatDongSinhVien.Infrastructure
{
    [HtmlTargetElement("div", Attributes = "page-model")]
    public class ActivityPageLinkTagHelper : TagHelper
    {
        private readonly IUrlHelperFactory _urlHelperFactory;

        public ActivityPageLinkTagHelper(IUrlHelperFactory urlHelperFactory)
        {
            _urlHelperFactory = urlHelperFactory;
        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext? ViewContext { get; set; }

        // ===================== PAGING =====================
        public Paging? PageModel { get; set; }

        // Razor Page name or route
        public string? PageName { get; set; }

        [HtmlAttributeName(DictionaryAttributePrefix = "page-url-")]
        public Dictionary<string, object> PageUrlValues { get; set; } = new();

        // ===================== CSS =====================
        public bool PageClassesEnabled { get; set; } = false;
        public string PageClass { get; set; } = "btn";
        public string PageClassNormal { get; set; } = "btn-outline-secondary";
        public string PageClassSelected { get; set; } = "btn-primary";
        public string PageClassDisabled { get; set; } = "disabled";

        // ===================== FILTERS =====================
        public DateTime? NgayToChucFilter { get; set; }
        public string? LinhVucFilter { get; set; }
        public string? TenHoatDongFilter { get; set; }
        public string? KhoaFilter { get; set; }
        public string? IDHoatDong {  get; set; }
        public string? SelectedHocKy { get; set; }
        public string? SelectedLop {  get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (ViewContext == null || PageModel == null || string.IsNullOrEmpty(PageName))
                return;

            IUrlHelper urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);
            TagBuilder container = new TagBuilder("div");

            int totalPages = PageModel.TotalPages;
            int currentPage = Math.Min(PageModel.CurrentPage, totalPages);

            // ================= PREVIOUS =================
            container.InnerHtml.AppendHtml(
                CreateNavigationTag(urlHelper, "« Previous", currentPage - 1, currentPage <= 1));

            // ================= PAGE NUMBERS =================
            int range = 2;
            int minPage = Math.Max(1, currentPage - range);
            int maxPage = Math.Min(totalPages, currentPage + range);

            if (minPage > 1)
            {
                container.InnerHtml.AppendHtml(CreatePageTag(urlHelper, 1));
                if (minPage > 2)
                    container.InnerHtml.AppendHtml(CreateEllipsisTag());
            }

            for (int i = minPage; i <= maxPage; i++)
            {
                container.InnerHtml.AppendHtml(CreatePageTag(urlHelper, i));
            }

            if (maxPage < totalPages)
            {
                if (maxPage < totalPages - 1)
                    container.InnerHtml.AppendHtml(CreateEllipsisTag());

                container.InnerHtml.AppendHtml(CreatePageTag(urlHelper, totalPages));
            }

            // ================= NEXT =================
            container.InnerHtml.AppendHtml(
                CreateNavigationTag(urlHelper, "Next »", currentPage + 1, currentPage >= totalPages));

            // ================= PAGE INFO =================
            TagBuilder info = new TagBuilder("span");
            info.AddCssClass("ms-2");
            info.InnerHtml.Append($"Page {currentPage} of {totalPages}");
            container.InnerHtml.AppendHtml(info);

            output.Content.AppendHtml(container.InnerHtml);
        }

        // =====================================================

        private TagBuilder CreatePageTag(IUrlHelper urlHelper, int pageNumber)
        {
            TagBuilder tag = new TagBuilder("a");

            var routeValues = BuildRouteValues(pageNumber);

            tag.Attributes["href"] = urlHelper.Page(PageName, routeValues);
            tag.InnerHtml.Append(pageNumber.ToString());

            if (PageClassesEnabled)
            {
                tag.AddCssClass(PageClass);
                tag.AddCssClass(pageNumber == PageModel!.CurrentPage
                    ? PageClassSelected
                    : PageClassNormal);
            }

            return tag;
        }

        private TagBuilder CreateNavigationTag(
            IUrlHelper urlHelper,
            string text,
            int targetPage,
            bool disabled)
        {
            TagBuilder tag = new TagBuilder("a");

            if (!disabled)
            {
                var routeValues = BuildRouteValues(targetPage);
                tag.Attributes["href"] = urlHelper.Page(PageName, routeValues);
            }

            tag.InnerHtml.Append(text);
            tag.AddCssClass("btn");
            tag.AddCssClass(disabled ? "btn-secondary disabled" : "btn-outline-primary");
            tag.AddCssClass(text.Contains("Previous") ? "me-1" : "ms-1");

            return tag;
        }

        private Dictionary<string, object> BuildRouteValues(int page)
        {
            var routeValues = new Dictionary<string, object>(PageUrlValues)
            {
                ["trang"] = page
            };
            if (!string.IsNullOrEmpty(IDHoatDong))
                routeValues["IDHoatDong"]= IDHoatDong;

            if (NgayToChucFilter.HasValue)
                routeValues["NgayToChucFilter"] = NgayToChucFilter.Value.ToString("yyyy-MM-dd");

            if (!string.IsNullOrEmpty(LinhVucFilter))
                routeValues["LinhVucFilter"] = LinhVucFilter;

            if (!string.IsNullOrEmpty(TenHoatDongFilter))
                routeValues["TenHoatDongFilter"] = TenHoatDongFilter;

            if (!string.IsNullOrEmpty(SelectedLop))
                routeValues["SelectedLop"] = SelectedLop;

            if (!string.IsNullOrEmpty(SelectedHocKy))
                routeValues["SelectedHocKy"] = SelectedHocKy;

            if (!string.IsNullOrEmpty(KhoaFilter))
                routeValues["KhoaFilter"] = KhoaFilter;

            return routeValues;
        }

        private TagBuilder CreateEllipsisTag()
        {
            TagBuilder span = new TagBuilder("span");
            span.AddCssClass("btn btn-light disabled");
            span.InnerHtml.Append("...");
            return span;
        }
    }
}
