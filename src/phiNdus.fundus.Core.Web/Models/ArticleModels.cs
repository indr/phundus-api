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

    public class ArticleModel : ModelBase
    {
        protected IArticleService ArticleService
        {
            get { return IoC.Resolve<IArticleService>(); }
        }

        public ArticleModel()
        {
            Item = new ArticleDto();
        }

        public ArticleModel(int id)
        {
            Item = ArticleService.GetArticle(SessionId, id);
        }

        public ArticleModel(int? id)
        {
            if (id.HasValue)
                Item = ArticleService.GetArticle(SessionId, id.Value);
            else
                Item = new ArticleDto();
        }

        public void Create()
        {
            ArticleService.CreateArticle(SessionId, Item);
        }

        public void Update()
        {
            ArticleService.UpdateArticle(SessionId, Item);
        }

        public void Delete()
        {
            ArticleService.DeleteArticle(SessionId, Item);
        }

        public ArticleDto Item { get; set; }

        public DtoProperty AddPropertyById(int propertyId)
        {
            var properties = ArticleService.GetProperties(SessionId);
            foreach (var each in properties)
                if (each.Id == propertyId)
                {
                    var result = new DtoProperty
                                     {
                                         Caption = each.Caption,
                                         DataType = each.DataType,
                                         Value = null,
                                         ValueId = 0,
                                         PropertyId = each.Id
                                     };
                    Item.AddProperty(result);
                    return result;
                }
            return null;
        }

        public void AddDiscriminatorById(int propertyId)
        {
            var property = AddPropertyById(propertyId);
            if (property != null)
                property.IsDiscriminator = true;
        }


        public void RemovePropertyById(int propertyId)
        {
            DtoProperty property = null;
            foreach (var each in Item.Properties)
                if (each.PropertyId == propertyId)
                {
                    property = each;
                    break;
                }

            if (property != null)
                Item.Properties.Remove(property);
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