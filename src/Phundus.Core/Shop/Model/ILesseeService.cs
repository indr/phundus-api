namespace Phundus.Shop.Model
{
    using Common.Domain.Model;

    public interface ILesseeService
    {        
        Lessee GetById(LesseeId lesseeId);
    }
}