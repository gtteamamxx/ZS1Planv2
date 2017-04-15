using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ZS1Planv2.Model.Application
{
    public class MVVMMessagerService
    {
        private static Dictionary<Type, object> _RegisteredReceivers;

        public static void RegisterReceiver(Type sourcePageType, Action action)
            => _RegisterReceiver(sourcePageType, action);
        public static void RegisterReceiver<T>(Type sourcePageType, Action<T> action)
            => _RegisterReceiver(sourcePageType, action);
        public static void RegisterReceiver<T1, T2>(Type sourcePageType, Action<T1, T2> action)
            => _RegisterReceiver(sourcePageType, action);

        private static void _RegisterReceiver(Type sourcePageType, object action)
        {
            if (_RegisteredReceivers == null)
                _RegisteredReceivers = new Dictionary<Type, object>();
            if (_RegisteredReceivers.Any(p => p.Key == sourcePageType))
                return;
            _RegisteredReceivers.Add(sourcePageType, action);
        }

        public static void SendMessage(Type targetPageType, object one = null, object two = null)
        {
            List<Type> types = new List<Type>();
            types.Add(targetPageType);
            SendMessage(types, one, two);
        }

        public static void SendMessage(List<Type> pagesToReceiveType, object one = null, object two = null)
        {
            if (pagesToReceiveType.Count() == 0)
                return;

            foreach(KeyValuePair<Type, object> receiver in _RegisteredReceivers)
            {
                if (!pagesToReceiveType.Any(p => p == receiver.Key))
                    continue;
                object _event = receiver.Value;
                object[] parameters = new object[2];
                parameters[0] = one;
                parameters[1] = two;

                MethodInfo methodInfo = _event.GetType().GetMethod("Invoke");

                if (methodInfo.GetParameters().Count() == 0)
                    methodInfo.Invoke(_event, null);
                else if (methodInfo.GetParameters().Count() == 1)
                    methodInfo.Invoke(_event, parameters.Take(1).ToArray<object>());
                else
                    methodInfo.Invoke(_event, parameters);
            }
        }
    }
}
