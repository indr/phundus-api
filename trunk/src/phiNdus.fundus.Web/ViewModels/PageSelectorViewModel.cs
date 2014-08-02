using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace phiNdus.fundus.Web.ViewModels
{
    using Phundus.Core.Cqrs.Paging;

    public class PageSelectorViewModel
    {
        private readonly int _minGroupLength;

        public PageSelectorViewModel() : this(new PageResponse())
        {
        }

        public PageSelectorViewModel(PageResponse response, int minGroupLength = 1)
        {
            _minGroupLength = minGroupLength;
            ActivePage = response.Index + 1;
            RowsPerPage = response.Size;
            TotalRows = response.Total;
            TotalPages = Convert.ToInt32(Math.Ceiling((double) TotalRows/RowsPerPage));


            CalculateButtons();
        }


        public IList<int> Pages { get; set; }


        public bool IsPreviousVisible
        {
            get { return ActivePage > 1; }
        }

        public bool IsNextVisible
        {
            get { return ActivePage < TotalPages; }
        }

        public int ActivePage { get; set; }
        public int RowsPerPage { get; set; }
        public int TotalRows { get; set; }
        public int TotalPages { get; set; }

        public IList<SelectListItem> RowsPerPageOptions
        {
            get
            {
                var values = new[] {4, 8, 12, 16, 32};

                var result = new List<SelectListItem>();
                foreach (var each in values)
                {
                    result.Add(new SelectListItem
                                   {
                                       Selected = each == RowsPerPage,
                                       Text = each.ToString(),
                                       Value = each.ToString()
                                   });
                }
                return result;
            }
        }

        private void CalculateButtons()
        {
            var pages = new List<int>();

            for (var idx = 1; idx < 1 + _minGroupLength && idx <= TotalPages; idx++)
                pages.Add(idx);

            for (var idx = Math.Max(1, ActivePage - _minGroupLength);
                 idx <= ActivePage + _minGroupLength && idx <= TotalPages;
                 idx++)
                pages.Add(idx);

            for (var idx = Math.Max(1, TotalPages - _minGroupLength + 1); idx <= TotalPages; idx++)
                pages.Add(idx);

            Pages = pages.Distinct().ToList();
        }
    }
}