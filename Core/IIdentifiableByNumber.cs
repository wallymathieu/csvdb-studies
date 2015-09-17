using With;
using SomeBasicCsvApp.Core.Entities;


namespace SomeBasicCsvApp.Core
{
    public interface IIdentifiableByNumber
    {
    }
    public static class IIdentifiableByNumbers
    {
        public static int GetId(this IIdentifiableByNumber self){
            return Switch.On<IIdentifiableByNumber,int>(self)
                .Case((Customer c) => c.Id)
                .Case((Order c) => c.Id)
                .Case((Product c) => c.Id)
                .Value();
        }
    }
}
