namespace Phundus.Shop.Orders.Model
{
    using System.Collections.Generic;
    using Common;
    using Common.Domain.Model;

    /// <summary>
    /// Vermieter
    /// </summary>
    public class Lessor : ValueObject
    {
        public Lessor(LessorId lessorId, string name)
        {
            AssertionConcern.AssertArgumentNotNull(lessorId, "LessorId must be provided.");
            AssertionConcern.AssertArgumentNotEmpty(name, "Name must be provided.");

            LessorId = lessorId;
            Name = name;
        }

        protected Lessor()
        {
        }

        public LessorId LessorId { get; private set; }

        public string Name { get; private set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return LessorId;
        }
    }
}