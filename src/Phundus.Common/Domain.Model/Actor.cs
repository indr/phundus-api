namespace Phundus.Common.Domain.Model
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
      
    [DataContract]
    public class Actor : ValueObject
    {
        public Actor(Guid actorGuid, string emailAddress, string fullName)
        {
            AssertionConcern.AssertArgumentNotEmpty(actorGuid, "User guid must be provided.");
            AssertionConcern.AssertArgumentNotEmpty(emailAddress, "Email address must be provided.");
            AssertionConcern.AssertArgumentNotEmpty(fullName, "Full name must be provided.");
            ActorGuid = actorGuid;
            EmailAddress = emailAddress;
            FullName = fullName;
        }

        protected Actor()
        {
        }

        [DataMember(Order = 1)]        
        protected Guid ActorGuid { get; set; }        

        [DataMember(Order = 2)]        
        public string EmailAddress { get; protected set; }

        [DataMember(Order = 3)]                
        public string FullName { get; protected set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return ActorGuid;
        }

        public Actor ToActor()
        {
            return new Actor(ActorGuid, EmailAddress, FullName);
        }
    }
}