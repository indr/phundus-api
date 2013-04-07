﻿using System;
using System.Collections.Generic;
using phiNdus.fundus.Business.Paging;
using phiNdus.fundus.Business.SecuredServices;

namespace phiNdus.fundus.Web.ViewModels
{
    using Domain.Entities;
    using phiNdus.fundus.Domain;
    using piNuts.phundus.Infrastructure;
    using piNuts.phundus.Infrastructure.Obsolete;

    public class ShopSearchResultViewModel : ViewModelBase
    {
        readonly ICollection<Organization> _organizations;

        public ShopSearchResultViewModel(string queryString, int? queryOrganizationId, int page, int rowsPerPage, ICollection<Organization> organizations)
        {
            Query = queryString;
            QueryOrganizationId = queryOrganizationId;
            RowsPerPage = rowsPerPage;
            Articles = new List<ArticleViewModel>();
            _organizations = organizations;

            Search(Query, QueryOrganizationId, page);
        }

        protected int RowsPerPage { get; set; }

        public string Query { get; protected set; }
        public PageSelectorViewModel PageSelectorModel { get; set; }
        public IList<ArticleViewModel> Articles { get; private set; }
        
        public IEnumerable<Organization> Organizations { get { return _organizations; }}

        public int? QueryOrganizationId { get; set; }

        protected IArticleService ArticleService
        {
            get { return GlobalContainer.Resolve<IArticleService>(); }
        }

        private void Search(string query, int? organization, int page)
        {
            var fieldDefinitions = ArticleService.GetProperties(SessionId);
            var queryResult = GlobalContainer.Resolve<IArticleService>().FindArticles(SessionId,
                    new PageRequest { Index = page - 1, Size = RowsPerPage }, query, organization);
            PageSelectorModel = new PageSelectorViewModel(queryResult.Pages);
            foreach (var each in queryResult.Items)
            {
                Articles.Add(new ArticleViewModel(each, fieldDefinitions));
            }
        }
    }
}