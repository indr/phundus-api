namespace Phundus.Shop.Model
{
    using Common.Domain.Model;

    public interface ILessorService
    {
        Lessor GetById(LessorId lessorId);
    }
}