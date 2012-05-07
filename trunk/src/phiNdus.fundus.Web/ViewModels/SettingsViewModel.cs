using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using phiNdus.fundus.Domain.Entities;
using phiNdus.fundus.Domain.Repositories;
using Rhino.Commons;

namespace phiNdus.fundus.Web.ViewModels
{
    public class SmtpSettingsViewModel : ViewModelBase
    {
        private IDictionary<string, Setting> _settings;

        [Required]
        public string Keyspace { get; set; }

        [DisplayName("Host")]
        [Required]
        public string Host { get; set; }

        public int HostVersion { get; set; }

        [DisplayName("Login")]
        public string UserName { get; set; }

        public int UserNameVersion { get; set; }

        [DisplayName("Passwort")]
        public string Password { get; set; }

        public int PasswordVersion { get; set; }

        [DisplayName("Absender")]
        [Required]
        public string From { get; set; }

        public int FromVersion { get; set; }

        private void Load(string key, Action<int, string> action)
        {
            var setting = _settings.Values.FirstOrDefault(p => p.Key.EndsWith(key));
            action(setting.Version, setting.StringValue);
        }

        public void Load()
        {
            using (UnitOfWork.Start())
            {
                Keyspace = "mail.smtp";
                _settings = IoC.Resolve<ISettingRepository>().FindByKeyspace(Keyspace);

                Load("host", (version, value) =>
                                 {
                                     HostVersion = version;
                                     Host = value;
                                 });
                Load("user-name", (version, value) =>
                                      {
                                          UserNameVersion = version;
                                          UserName = value;
                                      });
                Load("password", (version, value) =>
                                     {
                                         PasswordVersion = version;
                                         Password = value;
                                     });
                Load("from", (version, value) =>
                                 {
                                     FromVersion = version;
                                     From = value;
                                 });
            }
        }

        public void SaveChanges()
        {
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
        [Required]
        public string BodyPlain { get; set; }

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