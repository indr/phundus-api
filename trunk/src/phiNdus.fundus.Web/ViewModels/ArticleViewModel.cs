using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using phiNdus.fundus.Business.Dto;
using phiNdus.fundus.Business.SecuredServices;
using phiNdus.fundus.Web.Helpers;
using phiNdus.fundus.Web.Models.CartModels;
using Rhino.Commons;

namespace phiNdus.fundus.Web.ViewModels
{
    public class ShopArticleViewModel : ArticleViewModel
    {
        private CartItemModel _cartItem = new CartItemModel();

        public ShopArticleViewModel(int id) : base(id)
        {
            CartItem.ArticleId = id;
            CartItem.Amount = 1;
            CartItem.Begin = SessionAdapter.ShopBegin;
            CartItem.End = SessionAdapter.ShopEnd;
        }

        protected ICartService CartService
        {
            get { return IoC.Resolve<ICartService>(); }
        }

        public CartItemModel CartItem
        {
            get { return _cartItem; }
            set { _cartItem = value; }
        }

        public IList<AvailabilityDto> Availabilities { get; set; }
    }

    public class ArticleViewModel : ViewModelBase
    {
        private IList<ArticleViewModel> _children = new List<ArticleViewModel>();
        private IList<DiscriminatorViewModel> _discriminators = new List<DiscriminatorViewModel>();
        private IList<PropertyValueViewModel> _editableFieldValues = new List<PropertyValueViewModel>();
        private IList<ImageDto> _images = new List<ImageDto>();
        private IList<FieldDefinitionDto> _propertyDefinitions;
        private IList<PropertyValueViewModel> _propertyValues = new List<PropertyValueViewModel>();

        public ArticleViewModel()
        {
            Load(new ArticleDto(), ArticleService.GetProperties(SessionId));
        }

        public ArticleViewModel(int id)
        {
            var articleDto = ArticleService.GetArticle(SessionId, id);
            var fieldDefinitionDtos = ArticleService.GetProperties(SessionId);
            Load(articleDto, fieldDefinitionDtos);
        }

        public ArticleViewModel(ArticleDto articleDto, IList<FieldDefinitionDto> fieldDefinitionDtos)
        {
            Load(articleDto, fieldDefinitionDtos);
        }

        protected IArticleService ArticleService
        {
            get { return IoC.Resolve<IArticleService>(); }
        }

        public bool IsDeleted { get; set; }
        public bool IsChild { get; set; }

        // fundus-14: Entfernen
        public int RemainingPropertyId { get; set; }

        // fundus-14: Entfernen
        public int RemainingDiscriminatorId { get; set; }

        public int Id { get; set; }
        public int Version { get; set; }

        public string Caption
        {
            get
            {
                var property = GetProperty(2);
                return property != null ? (string) property.Value : null;
            }
        }

        public double Price
        {
            get
            {
                var property = GetProperty(4);
                return property != null && property.Value != null ? (double) property.Value : 0.0d;
            }
        }

        public string Description
        {
            get
            {
                var property = GetProperty(8);
                return property != null ? (string) property.Value : null;
            }
        }

        public bool IsReservable
        {
            get
            {
                var property = GetProperty(6);
                return property != null ? (bool) property.Value : false;
            }
        }

        public bool IsBorrowable
        {
            get
            {
                var property = GetProperty(7);
                return property != null ? (bool) property.Value : false;
            }
        }

        public IList<PropertyValueViewModel> PropertyValues
        {
            get { return _propertyValues; }
            set { _propertyValues = value; }
        }

        public IList<PropertyValueViewModel> EditableFieldValues
        {
            get { return _editableFieldValues; }
            set { _editableFieldValues = value; }
        }

        public IList<DiscriminatorViewModel> Discriminators
        {
            get { return _discriminators; }
            set { _discriminators = value; }
        }

        public IEnumerable<SelectListItem> RemainingProperties
        {
            get
            {
                if (_propertyDefinitions == null)
                    return new List<SelectListItem>();

                var propertyDefinitions = _propertyDefinitions.ToList();
                propertyDefinitions.RemoveAll(each =>
                                              (PropertyValues.FirstOrDefault(v => v.PropertyDefinitionId == each.Id) !=
                                               null)
                                              ||
                                              (Discriminators.FirstOrDefault(d => d.PropertyDefinitionId == each.Id) !=
                                               null)
                    );
                return (from p in propertyDefinitions.Where(p => p.IsAttachable).Select(p => new SelectListItem
                                                                                                 {
                                                                                                     Value =
                                                                                                         p.Id.ToString(),
                                                                                                     Text = p.Caption
                                                                                                 })
                        orderby p.Text
                        select p);
            }
        }

        public IList<ArticleViewModel> Children
        {
            get { return _children; }
            set { _children = value; }
        }

        public IList<ImageDto> Images
        {
            get { return _images; }
        }

        private void Load(ArticleDto article, IList<FieldDefinitionDto> propertyDefinitions)
        {
            Id = article.Id;
            Version = article.Version;

            // View-Models aus Properties für Felder und Diskriminatoren erstellen
            if (article.Properties.Count > 0)
            {
                foreach (var each in article.Properties)
                {
                    if (each.IsDiscriminator)
                        _discriminators.Add(ConvertToDiscriminatorViewModel(each));
                    else
                        _propertyValues.Add(ConvertToPropertyValueViewModel(each));
                }
            }
            else
            {
                // Standard-Properties hinzufügen
                foreach (var each in propertyDefinitions)
                {
                    if (each.IsDefault)
                    {
                        _propertyValues.Add(ConvertToPropertyValueViewModel(each));
                    }
                }
            }

            _propertyValues = _propertyValues.OrderBy(k => k.Position).ToList();
            _editableFieldValues = _propertyValues.Where(p => !p.IsCalculated).ToList();

            foreach (var each in article.Children)
            {
                var child = new ArticleViewModel(each, propertyDefinitions);
                child.IsChild = true;
                _children.Add(child);
            }

            _images = article.Images;

            _propertyDefinitions = propertyDefinitions;
        }

        protected PropertyValueViewModel GetProperty(int propertyDefinitionId)
        {
            return _propertyValues.SingleOrDefault(each => each.PropertyDefinitionId == propertyDefinitionId);
        }

        public object GetPropertyValue(int propertyId)
        {
            var property = GetProperty(propertyId);
            return property != null ? Convert.ToString(property.Value) : null;
        }

        public static DiscriminatorViewModel ConvertToDiscriminatorViewModel(FieldValueDto each)
        {
            return new DiscriminatorViewModel
                       {
                           Caption = each.Caption,
                           PropertyDefinitionId = each.PropertyId,
                           PropertyValueId = each.ValueId
                       };
        }

        public static DiscriminatorViewModel ConvertToDiscriminatorViewModel(FieldDefinitionDto each)
        {
            return ConvertToDiscriminatorViewModel(new FieldValueDto
                                                       {
                                                           Caption = each.Caption,
                                                           DataType = each.DataType,
                                                           PropertyId = each.Id
                                                       });
        }

        public static PropertyValueViewModel ConvertToPropertyValueViewModel(FieldDefinitionDto each)
        {
            return ConvertToPropertyValueViewModel(new FieldValueDto
                                                       {
                                                           Caption = each.Caption,
                                                           DataType = each.DataType,
                                                           PropertyId = each.Id
                                                       });
        }

        public static PropertyValueViewModel ConvertToPropertyValueViewModel(FieldValueDto each)
        {
            var propertyValueViewModel = new PropertyValueViewModel();
            propertyValueViewModel.Value = each.Value;
            propertyValueViewModel.Caption = each.Caption;
            propertyValueViewModel.DataType = each.DataType;
            propertyValueViewModel.PropertyDefinitionId = each.PropertyId;
            propertyValueViewModel.PropertyValueId = each.ValueId;
            propertyValueViewModel.IsCalculated = each.IsCalculated;
            propertyValueViewModel.Position = each.Position;
            return propertyValueViewModel;
        }

        public ArticleDto CreateDto()
        {
            var result = new ArticleDto();
            result.Id = Id;
            result.Version = Version;

            foreach (var each in EditableFieldValues)
            {
                if (each.IsDeleted)
                    continue;

                result.Properties.Add(new FieldValueDto
                                          {
                                              PropertyId = each.PropertyDefinitionId,
                                              Value = each.Value,
                                              ValueId = each.PropertyValueId
                                          });
            }

            foreach (var each in Discriminators)
            {
                if (each.IsDeleted)
                    continue;

                result.Properties.Add(new FieldValueDto
                                          {
                                              PropertyId = each.PropertyDefinitionId,
                                              ValueId = each.PropertyValueId,
                                              IsDiscriminator = true
                                          });
            }

            foreach (var each in Children)
            {
                if (each.IsDeleted)
                    continue;
                result.AddChild(each.CreateDto());
            }

            return result;
        }
    }
}