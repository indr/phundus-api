using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using phiNdus.fundus.Core.Business.Dto;
using phiNdus.fundus.Core.Business.SecuredServices;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Web.ViewModels
{
    public class ArticleViewModel : ViewModelBase
    {
        private IList<FieldDefinitionDto> _propertyDefinitions;
        private IList<DiscriminatorViewModel> _discriminators = new List<DiscriminatorViewModel>();
        private IList<PropertyValueViewModel> _propertyValues = new List<PropertyValueViewModel>();
        private IList<ArticleViewModel> _children = new List<ArticleViewModel>();

        [Obsolete("Yay!")]
        public ArticleViewModel()
        {
        }

        public ArticleViewModel(int id)
        {
            Load(IoC.Resolve<IArticleService>().GetArticle(SessionId, id), new List<FieldDefinitionDto>(0));
        }

        public ArticleViewModel(ArticleDto article)
        {
            Load(article, new FieldDefinitionDto[0]);
        }

        public ArticleViewModel(IList<FieldDefinitionDto> propertyDefinitions)
        {
            Load(new ArticleDto(), propertyDefinitions);
        }

        public ArticleViewModel(ArticleDto article, IList<FieldDefinitionDto> propertyDefinitions)
        {
            Load(article, propertyDefinitions);
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



            foreach (var each in article.Children)
            {
                var child = new ArticleViewModel(each, propertyDefinitions);
                child.IsChild = true;
                _children.Add(child);
            }


            _propertyDefinitions = propertyDefinitions;
        }

        public bool IsDeleted { get; set; }
        public bool IsChild { get; set; }

        // fundus-14: Entfernen
        public int RemainingPropertyId { get; set; }
        
        // fundus-14: Entfernen
        public int RemainingDiscriminatorId { get; set; }

        public int Id { get; set; }
        public int Version { get; set; }

        protected PropertyValueViewModel GetProperty(int propertyDefinitionId)
        {
            return _propertyValues.SingleOrDefault(each => each.PropertyDefinitionId == propertyDefinitionId);
        }

        public object GetPropertyValue(int propertyId)
        {
            var property = GetProperty(propertyId);
            return property != null ? Convert.ToString(property.Value) : null;
        }

        public string Caption {
            get {
                var property = GetProperty(2);
                return property != null ? (string)property.Value : null;
            }
        }

        public bool IsReservable {
            get {
                var property = GetProperty(6);
                return property != null ? (bool)property.Value : false;
            }
        }

        public bool IsBorrowable {
            get {
                var property = GetProperty(7);
                return property != null ? (bool)property.Value : false;
            }
        }

        public IList<PropertyValueViewModel> PropertyValues
        {
            get { return _propertyValues; }
            set { _propertyValues = value; }
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
                return (from p in propertyDefinitions.Select(p => new SelectListItem
                                                           {
                                                               Value = p.Id.ToString(),
                                                               Text = p.Caption
                                                           })
                                 orderby p.Text select p);
                
            }
        }

        public IList<ArticleViewModel> Children
        {
            get { return _children; }
            set { _children = value; }
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
            //switch (each.DataType)
            //{
            //    case FieldDataType.Boolean:
            //        propertyValueViewModel = new PropertyValueViewModelEx<bool>
            //                                     {
            //                                         Value = Convert.ToBoolean(each.Value)
            //                                     };
            //        break;
            //    case FieldDataType.Text:
            //        propertyValueViewModel = new PropertyValueViewModelEx<string>
            //                                     {
            //                                         Value = Convert.ToString(each.Value)
            //                                     };
            //        break;
            //    case FieldDataType.Integer:
            //        propertyValueViewModel = new PropertyValueViewModelEx<int>
            //                                     {
            //                                         Value = Convert.ToInt32(each.Value)
            //                                     };
            //        break;
            //    case FieldDataType.Decimal:
            //        propertyValueViewModel = new PropertyValueViewModelEx<double>
            //                                     {
            //                                         Value = Convert.ToDouble(each.Value)
            //                                     };
            //        break;
            //    case FieldDataType.DateTime:
            //        propertyValueViewModel = new PropertyValueViewModelEx<DateTime>
            //                                     {
            //                                         Value = Convert.ToDateTime(each.Value)
            //                                     };
            //        break;
            //    default:
            //        throw new ArgumentOutOfRangeException();
            //}
            propertyValueViewModel.Value = each.Value;
            propertyValueViewModel.Caption = each.Caption;
            propertyValueViewModel.DataType = each.DataType;
            propertyValueViewModel.PropertyDefinitionId = each.PropertyId;
            propertyValueViewModel.PropertyValueId = each.ValueId;
            return propertyValueViewModel;
        }

        public ArticleDto CreateDto()
        {
            var result = new ArticleDto();
            result.Id = Id;
            result.Version = Version;

            foreach (var each in PropertyValues)
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