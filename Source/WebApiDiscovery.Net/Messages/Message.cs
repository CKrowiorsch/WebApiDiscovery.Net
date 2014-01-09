using System;

using Newtonsoft.Json;

namespace Krowiorsch.Messages
{
    public class Message
    {
        public string TypeName { get; set; }

        public string Body { get; set; }

        public static Message FromObject(object message)
        {
            return new Message()
            {
                TypeName = message.GetType().FullName,
                Body = JsonConvert.SerializeObject(message, Formatting.None)
            };
        }

        public static Message FromJson(string message)
        {
            return JsonConvert.DeserializeObject<Message>(message);
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public object GetBodyMessage()
        {
            return JsonConvert.DeserializeObject(Body, Type.GetType(TypeName));
        }
    }
}