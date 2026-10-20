namespace EventTicket.Interfaces
{
    // "in" (contravariance) tuong duong voi "? super T" cua Java,
    // cho phep IEventListener<object> dung nhu IEventListener<VeDaBanEvent>
    public interface IEventListener<in T>
    {
        void OnEvent(T evt);
    }
}
