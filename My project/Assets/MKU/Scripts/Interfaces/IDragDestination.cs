namespace MKU.Scripts.Interfaces
{
    public interface IDragDestination<T> where T : class
    {

        int MaxAccetable(T item);

        void AddItems(T item, int number);
    }
}