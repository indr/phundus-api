namespace phiNdus.fundus.Domain.Entities
{
    public class OrganizationMembership : EntityBase
    {
        public virtual User User { get; set; }
        public virtual Organization Organization { get; set; }
        public virtual int Role { get; set; }
        public virtual bool IsLocked { get; protected set; }

        public virtual void Lock()
        {
            // TODO: Audit
            // TODO: E-Mail an Benutzer senden
            IsLocked = true;
        }

        public virtual void Unlock()
        {
            // TODO: Audit
            // TODO: E-Mail an Benutzer senden
            IsLocked = false;
        }
    }
}