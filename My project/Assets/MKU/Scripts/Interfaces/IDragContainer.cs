namespace MKU.Scripts.Interfaces
{
    public interface IDragContainer<T> : IDragDestination<T>, IDragSource<T> where T : class
    {
    }
}