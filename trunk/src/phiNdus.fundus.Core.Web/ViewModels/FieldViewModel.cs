using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;
using phiNdus.fundus.Core.Business.Dto;
using phiNdus.fundus.Core.Business.SecuredServices;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Web.ViewModels
{
    public class FieldViewModel : ViewModelBase
    {
        private FieldDefinitionDto _dto = new FieldDefinitionDto();
        protected FieldDefinitionDto Dto
        {
            get { return _dto; }
            set { _dto = value; }
        }

        protected IFieldsService FieldsService
        {
            get { return IoC.Resolve<IFieldsService>(); }
        }


        public int Id
        {
            get { return Dto.Id; }
            set
            {
                if (Dto.Id == 0)
                    Dto.Id = value;
            }
        }

        public int Version
        {
            get { return Dto.Version; }
            set
            {
                if (Dto.Version == 0)
                    Dto.Version = value;
            }
        }

        [DisplayName("Name")]
        public string Caption
        {
            get { return Dto.Caption; }
            set { Dto.Caption = value; }
        }

        [DisplayName("Datentyp")]
        public int DataType
        {
            get { return Convert.ToInt32(Dto.DataType); }
            set
            {
                if (!IsSystem)
                    Dto.DataType = (FieldDataType)value;
            }
        }

        [DisplayName("Datentyp")]
        public string DataTypeAsText
        {
            get { return Convert.ToString(Dto.DataType); }
        }

        [DisplayName("Systemfeld")]
        public bool IsSystem
        {
            get { return Dto.IsSystem; }
        }

        [DisplayName("Hinzufügbar")]
        public bool IsAttachable
        {
            get { return Dto.IsAttachable; }
        }

        [DisplayName("Spalte in Tabellen")]
        public bool IsColumn
        {
            get { return Dto.IsColumn; }
            set { Dto.IsColumn = value; }
        }

        [DisplayName("Position")]
        public int Position
        {
            get { return Dto.Position; }
            set { Dto.Position = value; }
        }

        [DisplayName("Standard")]
        public bool IsDefault
        {
            get { return Dto.IsDefault; }
            set { Dto.IsDefault = value; }
        }

        public FieldViewModel Load(int id)
        {
            Dto = FieldsService.GetField(SessionId, id);
            return this;
        }

        public FieldViewModel Load(int id, int version)
        {
            Load(id);
            if (Dto.Version != version)
                throw new Exception("Tjaaaa. Zu schpät! (odr zfrüeh?!)");
            return this;
        }


        public FieldViewModel Load(FieldDefinitionDto dto)
        {
            Dto = dto;
            return this;
        }

        public void Save()
        {
            if (Dto.Id > 0)
                FieldsService.UpdateField(SessionId, Dto);
            else
                FieldsService.CreateField(SessionId, Dto);
        }

        public void Delete()
        {
            FieldsService.DeleteField(SessionId, Dto);
        }

        public IEnumerable<SelectListItem> FieldDataTypes
        {
            get
            {
                var result = new List<SelectListItem>();
                foreach (var each in Enum.GetValues(typeof(FieldDataType)))
                {
                    var item = new SelectListItem();
                    item.Value = Convert.ToString(Convert.ToInt32(each));
                    item.Text = Convert.ToString(each);
                    result.Add(item);
                }
                result.Sort((x,y) => x.Text.CompareTo(y.Text));
                return result;
            }
        }

        public FieldDefinitionDto CreateDto()
        {
            return Dto;
        }
    }
}