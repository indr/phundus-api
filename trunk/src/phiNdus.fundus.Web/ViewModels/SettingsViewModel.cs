using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using phiNdus.fundus.Domain.Entities;
using phiNdus.fundus.Domain.Repositories;
using Rhino.Commons;

namespace phiNdus.fundus.Web.ViewModels
{
    public class SettingsViewModelBase : ViewModelBase
    {
        protected IDictionary<string, Setting> _settings = new Dictionary<string, Setting>();

        protected void Load(string key, Action<int, string> action)
        {
            var setting = _settings.Values.FirstOrDefault(p => p.Key.EndsWith(key));
            if (setting != null)
                action(setting.Version, setting.StringValue);
        }
    }

    public class GeneralSettingsViewModel : SettingsViewModelBase
    {
        [Required]
        public string Keyspace { get; set; }

        [DisplayName("Server-URL")]
        [Required]
        public string ServerUrl { get; set; }

        public int ServerUrlVersion { get; set; }

        [DisplayName("E-Mail-Adresse des Administrators")]
        [Required]
        public string AdminEmailAddress { get; set; }

        public int AdminEmailAddressVersion { get; set; }

        public void Load()
        {
            using (UnitOfWork.Start())
            {
                Keyspace = "common";
                _settings = IoC.Resolve<ISettingRepository>().FindByKeyspace(Keyspace);

                Load("server-url", (version, value) =>
                {
                    ServerUrlVersion = version;
                    ServerUrl = value;
                });
                Load("admin-email-address", (version, value) =>
                {
                    AdminEmailAddressVersion = version;
                    AdminEmailAddress = value;
                });
            }
        }

        public void SaveChanges()
        {
            var repo = IoC.Resolve<ISettingRepository>();

            var serverUrl = repo.FindByKey(Keyspace + ".server-url");
            if (serverUrl != null && serverUrl.Version != ServerUrlVersion)
                throw new Exception("Die Daten wurden in der zwischenzeit verändert.");
            if (serverUrl == null)
                serverUrl = new Setting(Keyspace + ".server-url");
            serverUrl.StringValue = ServerUrl;
            repo.SaveOrUpdate(serverUrl);

            var adminEmailAddress = repo.FindByKey(Keyspace + ".admin-email-address");
            if (adminEmailAddress != null && adminEmailAddress.Version != AdminEmailAddressVersion)
                throw new Exception("Die Daten wurden in der zwischenzeit verändert.");
            if (adminEmailAddress == null)
                adminEmailAddress = new Setting(Keyspace + ".admin-email-address");
            adminEmailAddress.StringValue = AdminEmailAddress;
            repo.SaveOrUpdate(adminEmailAddress);
        }
    }

    public class SmtpSettingsViewModel : SettingsViewModelBase
    {
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
            var repo = IoC.Resolve<ISettingRepository>();
            
            var subject = repo.FindByKey(Keyspace + ".host");
            if (subject != null && subject.Version != HostVersion)
                throw new Exception("Die Daten wurden in der zwischenzeit verändert.");
            if (subject == null)
                subject = new Setting(Keyspace + ".host");
            subject.StringValue = Host;
            repo.SaveOrUpdate(subject);

            var userName = repo.FindByKey(Keyspace + ".user-name");
            if (userName != null && userName.Version != UserNameVersion)
                throw new Exception("Die Daten wurden in der zwischenzeit verändert.");
            if (userName == null)
                userName = new Setting(Keyspace + ".user-name");
            userName.StringValue = UserName;
            repo.SaveOrUpdate(userName);

            var password = repo.FindByKey(Keyspace + ".password");
            if (password != null && password.Version != PasswordVersion)
                throw new Exception("Die Daten wurden in der zwischenzeit verändert.");
            if (password == null)
                password = new Setting(Keyspace + ".password");
            password.StringValue = Password;
            repo.SaveOrUpdate(password);

            var from = repo.FindByKey(Keyspace + ".from");
            if (from != null && from.Version != FromVersion)
                throw new Exception("Die Daten wurden in der zwischenzeit verändert.");
            if (from == null)
                from = new Setting(Keyspace + ".from");
            from.StringValue = From;
            repo.SaveOrUpdate(from);
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
            using (UnitOfWork.Start())
            {
                var settings = IoC.Resolve<ISettingRepository>().FindByKeyspace(keyspace);
                Keyspace = keyspace;
                LoadSubject(settings.Values.FirstOrDefault(p => p.Key.EndsWith("subject")));
                LoadBodyPlain(settings.Values.FirstOrDefault(p => p.Key.EndsWith("body")));
                LoadBodyHtml(settings.Values.FirstOrDefault(p => p.Key.EndsWith("html-body")));
            }
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
            var repo = IoC.Resolve<ISettingRepository>();

            var subject = repo.FindByKey(Keyspace + ".subject");
            if (subject != null && subject.Version != SubjectVersion)
                throw new Exception("Die Daten wurden in der zwischenzeit verändert.");
            if (subject == null)
                subject = new Setting(Keyspace + ".subject");
            subject.StringValue = Subject;
            repo.SaveOrUpdate(subject);

            var bodyPlain = repo.FindByKey(Keyspace + ".body");
            if (bodyPlain != null && bodyPlain.Version != BodyPlainVersion)
                throw new Exception("Die Daten wurden in der zwischenzeit verändert.");
            if (bodyPlain == null)
                bodyPlain = new Setting(Keyspace + ".body");
            bodyPlain.StringValue = BodyPlain;
            repo.SaveOrUpdate(bodyPlain);

            var bodyHtml = repo.FindByKey(Keyspace + ".html-body");
            if (bodyHtml != null && bodyHtml.Version != BodyHtmlVersion)
                throw new Exception("Die Daten wurden in der zwischenzeit verändert.");
            if (bodyHtml == null)
                bodyHtml = new Setting(Keyspace + ".html-body");
            bodyHtml.StringValue = BodyHtml;
            repo.SaveOrUpdate(bodyHtml);
        }
    }
}