namespace phiNdus.fundus.Web.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
    using Microsoft.Practices.ServiceLocation;
    using phiNdus.fundus.Domain.Entities;
    using phiNdus.fundus.Domain.Repositories;

    public class SettingsViewModelBase : ViewModelBase
    {
        private readonly IDictionary<string, Setting> _settings = new Dictionary<string, Setting>();

        protected void Load(string key, Action<int, string> action)
        {
            var setting = _settings.Values.FirstOrDefault(p => p.Key.EndsWith(key));
            if (setting != null)
                action(setting.Version, setting.StringValue);
        }
    }

    public class MailTemplateSettingsViewModel : ViewModelBase
    {
        private IList<MailTemplateSettingViewModel> _items = new List<MailTemplateSettingViewModel>();

        public IList<MailTemplateSettingViewModel> Items
        {
            get { return _items; }
            set { _items = value; }
        }

        public void Load()
        {
            Items.Add(new MailTemplateSettingViewModel("mail.templates.user-account-validation"));
            Items.Add(new MailTemplateSettingViewModel("mail.templates.user-account-created"));
            Items.Add(new MailTemplateSettingViewModel("mail.templates.user-locked-out"));
            Items.Add(new MailTemplateSettingViewModel("mail.templates.user-unlocked"));
            Items.Add(new MailTemplateSettingViewModel("mail.templates.user-change-email-validation"));
            Items.Add(new MailTemplateSettingViewModel("mail.templates.user-reset-password"));
            Items.Add(new MailTemplateSettingViewModel("mail.templates.order-received"));
            Items.Add(new MailTemplateSettingViewModel("mail.templates.order-approved"));
            Items.Add(new MailTemplateSettingViewModel("mail.templates.order-rejected"));
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
        public string BodyPlain { get; set; }

        [Required]
        public int BodyHtmlVersion { get; set; }

        [DisplayName("HTML")]
        [AllowHtml]
        public string BodyHtml { get; set; }

        private void Load(string keyspace)
        {
            var settings = ServiceLocator.Current.GetInstance<ISettingRepository>().FindByKeyspace(keyspace);
            Keyspace = keyspace;
            LoadSubject(settings.Values.FirstOrDefault(p => p.Key.EndsWith("subject")));
            LoadBodyPlain(settings.Values.FirstOrDefault(p => p.Key.EndsWith("body")));
            LoadBodyHtml(settings.Values.FirstOrDefault(p => p.Key.EndsWith("html-body")));
        }

        private void LoadSubject(Setting setting)
        {
            if (setting == null) return;
            SubjectVersion = setting.Version;
            Subject = setting.StringValue;
        }

        private void LoadBodyPlain(Setting setting)
        {
            if (setting == null) return;
            BodyPlainVersion = setting.Version;
            BodyPlain = setting.StringValue;
        }

        private void LoadBodyHtml(Setting setting)
        {
            if (setting == null) return;
            BodyHtmlVersion = setting.Version;
            BodyHtml = setting.StringValue;
        }

        public void SaveChanges()
        {
            var repo = ServiceLocator.Current.GetInstance<ISettingRepository>();

            var subject = repo.FindByKey(Keyspace + ".subject");
            if (subject != null && subject.Version != SubjectVersion)
                throw new Exception("Die Daten wurden in der zwischenzeit verändert.");
            if (subject == null)
                subject = new Setting(Keyspace + ".subject");
            subject.StringValue = Subject;
            repo.Add(subject);

            var bodyPlain = repo.FindByKey(Keyspace + ".body");
            if (bodyPlain != null && bodyPlain.Version != BodyPlainVersion)
                throw new Exception("Die Daten wurden in der zwischenzeit verändert.");
            if (bodyPlain == null)
                bodyPlain = new Setting(Keyspace + ".body");
            bodyPlain.StringValue = BodyPlain;
            repo.Add(bodyPlain);

            var bodyHtml = repo.FindByKey(Keyspace + ".html-body");
            if (bodyHtml != null && bodyHtml.Version != BodyHtmlVersion)
                throw new Exception("Die Daten wurden in der zwischenzeit verändert.");
            if (bodyHtml == null)
                bodyHtml = new Setting(Keyspace + ".html-body");
            bodyHtml.StringValue = BodyHtml;
            repo.Add(bodyHtml);
        }
    }
}