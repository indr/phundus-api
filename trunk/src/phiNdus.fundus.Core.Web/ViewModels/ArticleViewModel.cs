using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using phiNdus.fundus.Core.Business.Dto;

namespace phiNdus.fundus.Core.Web.ViewModels
{
    public class ArticleViewModel
    {
        private readonly PropertyDto[] _propertyDefinitions;
        private IList<DiscriminatorViewModel> _discriminators = new List<DiscriminatorViewModel>();
        private IList<PropertyValueViewModel> _propertyValues = new List<PropertyValueViewModel>();
        private IList<ArticleViewModel> _children = new List<ArticleViewModel>();

        public ArticleViewModel()
        {
        }

        public ArticleViewModel(ArticleDto article) : this(article, new PropertyDto[0])
        {
        }

        public ArticleViewModel(PropertyDto[] propertyDefinitions)
            : this(new ArticleDto(), propertyDefinitions)
        {
        }

        public ArticleViewModel(ArticleDto article, PropertyDto[] propertyDefinitions)
        {
            Id = article.Id;
            Version = article.Version;
            foreach (var each in article.Properties)
            {
                if (each.IsDiscriminator)
                    _discriminators.Add(ConvertToDiscriminatorViewModel(each));
                else
                    _propertyValues.Add(ConvertToPropertyValueViewModel(each));
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

        public static DiscriminatorViewModel ConvertToDiscriminatorViewModel(DtoProperty each)
        {
            return new DiscriminatorViewModel
                       {
                           Caption = each.Caption,
                           PropertyDefinitionId = each.PropertyId,
                           PropertyValueId = each.ValueId
                       };
        }

        public static DiscriminatorViewModel ConvertToDiscriminatorViewModel(PropertyDto each)
        {
            return ConvertToDiscriminatorViewModel(new DtoProperty
                                                       {
                                                           Caption = each.Caption,
                                                           DataType = each.DataType,
                                                           PropertyId = each.Id
                                                       });
        }

        public static PropertyValueViewModel ConvertToPropertyValueViewModel(PropertyDto each)
        {
            return ConvertToPropertyValueViewModel(new DtoProperty
                                                       {
                                                           Caption = each.Caption,
                                                           DataType = each.DataType,
                                                           PropertyId = each.Id
                                                       });
        }

        public static PropertyValueViewModel ConvertToPropertyValueViewModel(DtoProperty each)
        {
            var propertyValueViewModel = new PropertyValueViewModel();
            //switch (each.DataType)
            //{
            //    case PropertyDataType.Boolean:
            //        propertyValueViewModel = new PropertyValueViewModelEx<bool>
            //                                     {
            //                                         Value = Convert.ToBoolean(each.Value)
            //                                     };
            //        break;
            //    case PropertyDataType.Text:
            //        propertyValueViewModel = new PropertyValueViewModelEx<string>
            //                                     {
            //                                         Value = Convert.ToString(each.Value)
            //                                     };
            //        break;
            //    case PropertyDataType.Integer:
            //        propertyValueViewModel = new PropertyValueViewModelEx<int>
            //                                     {
            //                                         Value = Convert.ToInt32(each.Value)
            //                                     };
            //        break;
            //    case PropertyDataType.Decimal:
            //        propertyValueViewModel = new PropertyValueViewModelEx<double>
            //                                     {
            //                                         Value = Convert.ToDouble(each.Value)
            //                                     };
            //        break;
            //    case PropertyDataType.DateTime:
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

                result.Properties.Add(new DtoProperty
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

                result.Properties.Add(new DtoProperty
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