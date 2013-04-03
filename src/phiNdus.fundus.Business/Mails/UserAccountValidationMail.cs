using System;
using System.Net.Mail;
using phiNdus.fundus.Domain.Entities;
using phiNdus.fundus.Domain.Settings;
using Rhino.Commons;

namespace phiNdus.fundus.Business.Mails
{
    using System.Configuration;
    using Domain.Infrastructure;
    using piNuts.phundus.Infrastructure.Rhino;

    public class UserChangeEmailValidationMail : BaseMail
    {
        public UserChangeEmailValidationMail()
            : base(Settings.Mail.Templates.UserChangeEmailValidationMail)
        {
            
        }

        public void Send(User user)
        {
            Send(user.Membership.RequestedEmail);
        }

        public UserChangeEmailValidationMail For(User user)
        {
            Guard.Against<ArgumentNullException>(user == null, "user");

            Model = new
            {
                Settings = Settings.GetSettings(),
                Urls = new Urls(Config.ServerUrl),
                User = user
            };
            //DataContext.Add("User", user);
            //DataContext.Add("Membership", user.Membership);
            return this;
        }
    }

    public class UserResetPasswordMail : BaseMail
    {
        public UserResetPasswordMail()
            : base(Settings.Mail.Templates.UserResetPasswordMail)
        {
            
        }

        public void Send(User user)
        {
            Send(user.Membership.Email);
        }

        public UserResetPasswordMail For(User user, string password)
        {
            Guard.Against<ArgumentNullException>(user == null, "user");

            Model = new
            {
                Settings = Settings.GetSettings(),
                Urls = new Urls(Config.ServerUrl),
                Password = password,
                User = user
            };
            return this;
        }
    }

    public class UserAccountValidationMail : BaseMail
    {
        public UserAccountValidationMail() : base(Settings.Mail.Templates.UserAccountValidation)
        {
        }

        public void Send(User user)
        {
            Send(user.Membership.Email);
        }

        public UserAccountValidationMail For(User user)
        {
            Guard.Against<ArgumentNullException>(user == null, "user");

            Model = new
                        {
                            Settings = Settings.GetSettings(),
                            Urls = new Urls(Config.ServerUrl),
                            User = user
                        };
            //DataContext.Add("User", user);
            //DataContext.Add("Membership", user.Membership);
            return this;
        }
    }

    public class UserUnlockedMail : BaseMail
    {
        public UserUnlockedMail()
            : base(Settings.Mail.Templates.UserUnlocked)
        {
        }

        public UserUnlockedMail Send(User user)
        {
            Send(user.Membership.Email);
            return this;
        }

        public UserUnlockedMail For(User user)
        {
            Guard.Against<ArgumentNullException>(user == null, "user");

            Model = new
            {
                Settings = Settings.GetSettings(),
                Urls = new Urls(Config.ServerUrl),
                User = user
            };
            //DataContext.Add("User", user);
            //DataContext.Add("Membership", user.Membership);
            return this;
        }

        public new void Send(string address)
        {
            base.Send(address);
        }
    }

    public class UserLockedOutMail : BaseMail
    {
        public UserLockedOutMail()
            : base(Settings.Mail.Templates.UserLockedOut)
        {
        }

        public UserLockedOutMail Send(User user)
        {
            Send(user.Membership.Email);
            return this;
        }

        public UserLockedOutMail For(User user)
        {
            Guard.Against<ArgumentNullException>(user == null, "user");

            Model = new
            {
                Settings = Settings.GetSettings(),
                Urls = new Urls(Config.ServerUrl),
                User = user
            };
            //DataContext.Add("User", user);
            //DataContext.Add("Membership", user.Membership);
            return this;
        }

        public new void Send(string address)
        {
            base.Send(address);
        }
    }

    
    public class UserAccountCreatedMail : BaseMail
    {
        public UserAccountCreatedMail()
            : base(Settings.Mail.Templates.UserAccountCreated)
        {
        }

        public void Send(User user)
        {
            Send(user.Membership.Email);
        }

        public UserAccountCreatedMail For(User user)
        {
            Guard.Against<ArgumentNullException>(user == null, "user");

            Model = new
                        {
                            Settings = Settings.GetSettings(),
                            Urls = new Urls(Config.ServerUrl),
                            User = user
                        };
            //DataContext.Add("User", user);
            //DataContext.Add("Membership", user.Membership);
            return this;
        }

        public new void Send(string address)
        {
            base.Send(address);
        }
    }

    public class OrderReceivedMail : BaseMail
    {
        public OrderReceivedMail()
            : base(Settings.Mail.Templates.OrderReceived)
        {
        }

        public OrderReceivedMail For(Order order)
        {
            Model = new
                        {
                            Settings = Settings.GetSettings(),
                            Urls = new Urls(Config.ServerUrl),
                            User = order.Reserver,
                            Order = order
                        };
            
            Attachments.Add(new Attachment(order.GeneratePdf(),
                                           String.Format("Order-{0}.pdf", order.Id),
                                           "application/pdf"));
            
            return this;
        }

        public OrderReceivedMail Send(User user)
        {
            Send(user.Membership.Email);
            return this;
        }

        public new OrderReceivedMail Send(string address)
        {
            base.Send(address);
            return this;
        }
    }

    public class OrderRejectedMail : BaseMail
    {
        public OrderRejectedMail()
            : base(Settings.Mail.Templates.OrderRejected)
        {
        }

        public OrderRejectedMail For(Order order)
        {
            Model = new
            {
                Settings = Settings.GetSettings(),
                Urls = new Urls(Config.ServerUrl),
                User = order.Reserver,
                Order = order
            };

            Attachments.Add(new Attachment(order.GeneratePdf(),
                                           String.Format("Order-{0}.pdf", order.Id),
                                           "application/pdf"));

            return this;
        }

        public void Send(User user)
        {
            Send(user.Membership.Email);
        }
    }

    public class OrderApprovedMail : BaseMail
    {
        public OrderApprovedMail()
            : base(Settings.Mail.Templates.OrderApproved)
        {
        }

        public OrderApprovedMail For(Order order)
        {
            Model = new
            {
                Settings = Settings.GetSettings(),
                Urls = new Urls(Config.ServerUrl),
                User = order.Reserver,
                Order = order
            };

            Attachments.Add(new Attachment(order.GeneratePdf(),
                                           String.Format("Order-{0}.pdf", order.Id),
                                           "application/pdf"));
            
            return this;
        }

        public void Send(User user)
        {
            Send(user.Membership.Email);
        }
    }
}