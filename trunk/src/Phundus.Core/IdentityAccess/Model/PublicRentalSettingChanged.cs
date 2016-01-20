﻿namespace Phundus.IdentityAccess.Model
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;

    [DataContract]
    public class PublicRentalSettingChanged : DomainEvent
    {
        public PublicRentalSettingChanged(Initiator initiator, OrganizationId organizationId, bool value)
        {
            if (initiator == null) throw new ArgumentNullException("initiator");
            if (organizationId == null) throw new ArgumentNullException("organizationId");
            Initiator = initiator;
            OrganizationId = organizationId.Id;
            Value = value;
        }

        protected PublicRentalSettingChanged()
        {
        }

        [DataMember(Order = 1)]
        public Initiator Initiator { get; set; }

        [DataMember(Order = 2)]
        public Guid OrganizationId { get; set; }

        [DataMember(Order = 3)]
        public bool Value { get; set; }
    }
}