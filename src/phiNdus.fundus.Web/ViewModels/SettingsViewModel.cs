using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using phiNdus.fundus.Domain.Entities;
using phiNdus.fundus.Domain.Repositories;
using Rhino.Commons;

namespace phiNdus.fundus.Web.ViewModels
{
    //public class SettingsViewModel : ViewModelBase
    //{
    //    public SettingsViewModel()
    //    {
    //        using (UnitOfWork.Start())
    //        {
    //            var repo = IoC.Resolve<ISettingRepository>();
    //            var settings = repo.FindByKeyspace("mail.templates");
    //            foreach (var each in settings.Values)
    //                Items.Add(new SettingViewModel(each));
    //        }
    //    }


    //    private IList<SettingViewModel> _items = new List<SettingViewModel>();
    //    public IList<SettingViewModel> Items
    //    {
    //        get { return _items; }
    //        set { _items = value; }
    //    }
    //}


    public class MailTemplateSettingsViewModel : ViewModelBase
    {
        public MailTemplateSettingsViewModel()
        {
            
        }

        public void Load()
        {
            Items.Add(new MailTemplateSettingViewModel("mail.templates.user-account-validation"));
            Items.Add(new MailTemplateSettingViewModel("mail.templates.order-approved"));
            Items.Add(new MailTemplateSettingViewModel("mail.templates.order-rejected"));
        }

        private IList<MailTemplateSettingViewModel> _items = new List<MailTemplateSettingViewModel>();
        public IList<MailTemplateSettingViewModel> Items
        {
            get { return _items; }
            set { _items = value; }
        }

        public void SaveChanges()
        {
            foreach (var each in Items)
                each.SaveChanges();
        }
    }

    public class MailTemplateSettingViewModel : ViewModelBase
    {
        public MailTemplateSettingViewModel()
        {
            
        }

        public MailTemplateSettingViewModel(string keyspace)
        {
            Load(keyspace);
        }

        private void Load(string keyspace)
        {
            using (UnitOfWork.Start())
            {
                var settings = IoC.Resolve<ISettingRepository>().FindByKeyspace(keyspace);
                Keyspace = keyspace;
                LoadSubject(settings.Values.FirstOrDefault(p => p.Key.EndsWith("subject")));
                LoadBodyPlain(settings.Values.FirstOrDefault(p => p.Key.EndsWith("body")));
            }
        }

        private void LoadBodyPlain(Setting setting)
        {
            if (setting == null) return;
            BodyPlainVersion = setting.Version;
            BodyPlain = setting.StringValue;
        }

        private void LoadSubject(Setting setting)
        {
            if (setting == null) return;
            SubjectVersion = setting.Version;
            Subject = setting.StringValue;
        }

        [Required]
        public string Keyspace { get; set; }

        [Required]
        public int SubjectVersion { get; set; }

        [DisplayName("Betreff")]
        [Required]
        public string Subject { get; set; }

        [Required]
        public int BodyPlainVersion { get; set; }

        [DisplayName("Text")]
        [Required]
        public string BodyPlain { get; set; }


        public void SaveChanges()
        {
            var repo = IoC.Resolve<ISettingRepository>();
            
            var subject = repo.FindByKey(Keyspace + ".subject");
            if (subject != null && subject.Version != SubjectVersion)
                throw new Exception("Die Daten wurden in der zwischenzeit verändert.");
            if (subject == null)
                subject = new Setting(Keyspace + ".subject");
            subject.StringValue = Subject;

            var bodyPlain = repo.FindByKey(Keyspace + ".body");
            if (bodyPlain != null && bodyPlain.Version != BodyPlainVersion)
                throw new Exception("Die Daten wurden in der zwischenzeit verändert.");
            if (bodyPlain == null)
                bodyPlain = new Setting(Keyspace + ".body");
            bodyPlain.StringValue = BodyPlain;

            repo.SaveOrUpdate(subject);
            repo.SaveOrUpdate(bodyPlain);
        }
    }
}