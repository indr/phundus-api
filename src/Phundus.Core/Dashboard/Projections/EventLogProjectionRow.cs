namespace Phundus.Dashboard.Projections
{
    using System;
    using System.IO;
    using Newtonsoft.Json;

    public class ActionsProjectionRowBase
    {
        
        public virtual Guid EventGuid { get; set; }
        public virtual DateTime OccuredOnUtc { get; set; }
        public virtual string Name { get; set; }

        public virtual void SetData(object data)
        {
            var stringWriter = new StringWriter();
            var settings = new JsonSerializerSettings();
            JsonSerializer.Create(settings).Serialize(stringWriter, data);
            JsonData = stringWriter.GetStringBuilder().ToString();
        }

        public virtual string JsonData { get; protected set; }
    }

    public class EventLogProjectionRow : ActionsProjectionRowBase
    {
        public virtual string Text { get; set; }
    }
}