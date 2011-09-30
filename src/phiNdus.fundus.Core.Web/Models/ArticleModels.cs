using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using phiNdus.fundus.Core.Business.Dto;
using phiNdus.fundus.Core.Business.SecuredServices;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Web.Models
{
    public static class ArticleDtoExtensions
    {
        private static IArticleService ArticleService
        {
            get { return IoC.Resolve<IArticleService>(); }
        }

        public static IEnumerable<SelectListItem> RemainingProperties(this ArticleDto articleDto)
        {
            var properties = ArticleService.GetProperties(HttpContext.Current.Session.SessionID).ToList();
            properties.RemoveAll(x => articleDto.Properties.SingleOrDefault(y => y.PropertyId == x.Id) != null);
            return properties.Select(p => new SelectListItem
                                                  {
                                                      Value = p.Id.ToString(),
                                                      Text = p.Caption
                                                  });
        }
    }

    public class ArticleListModel : ModelBase
    {
        private IEnumerable<PropertyDto> _headings;
        private IEnumerable<ArticleDto> _items;

        protected IArticleService ArticleService
        {
            get { return IoC.Resolve<IArticleService>(); }
        }

        public IEnumerable<ArticleDto> Items
        {
            get
            {
                if (_items == null)
                    _items = ArticleService.GetArticles(SessionId);
                return _items;
            }
        }

        public IEnumerable<PropertyDto> Headings
        {
            get
            {
                if (_headings == null)
                    _headings = ComputeHeadings();
                return _headings;
            }
        }

        private IEnumerable<PropertyDto> ComputeHeadings()
        {
            var result = ArticleService.GetProperties(SessionId);
            return result;
            /*
            var result = new List<PropertyDto>
                             {
                                 new PropertyDto
                                     {
                                         Id = 2,
                                         Caption = "Name"
                                     },
                                 new PropertyDto
                                     {
                                         Id = 5,
                                         Caption = "Erfassungsdatum"
                                     }
                             };
            return result;
            */
        }
    }
}