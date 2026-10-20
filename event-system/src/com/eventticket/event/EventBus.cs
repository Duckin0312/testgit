using System;
using System.Collections.Generic;
using EventTicket.Interfaces;

namespace EventTicket.Event
{
    public sealed class EventBus
    {
        public static readonly EventBus Instance = new EventBus();

        private readonly object _lock = new object();

        // Map chua cac listener theo tung kieu Event (Type)
        private readonly Dictionary<Type, List<Action<object>>> _danhSachListener = new();

        private EventBus() { }

        public static EventBus GetInstance()
        {
            return Instance;
        }

        // Dang ky mot Listener lang nghe mot loai Event cu the
        public void DangKy<T>(IEventListener<T> listener) where T : class
        {
            if (listener == null) return;
            lock (_lock)
            {
                var eventType = typeof(T);
                if (!_danhSachListener.TryGetValue(eventType, out var list))
                {
                    list = new List<Action<object>>();
                    _danhSachListener[eventType] = list;
                }
                list.Add(e => listener.OnEvent((T)e));
            }
        }

        // Phat (Publish) su kien den tat ca cac Listener da dang ky
        public void PhatSuKien<T>(T evt) where T : class
        {
            if (evt == null) return;
            lock (_lock)
            {
                if (_danhSachListener.TryGetValue(evt.GetType(), out var listeners))
                {
                    foreach (var listener in listeners)
                    {
                        try
                        {
                            listener(evt);
                        }
                        catch (global::System.Exception e)
                        {
                            Console.WriteLine("[EventBus] Loi khi xu ly event: " + e.Message);
                        }
                    }
                }
            }
        }
    }
}
