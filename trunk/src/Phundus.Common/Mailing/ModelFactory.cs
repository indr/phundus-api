namespace Phundus.Common.Mailing
{
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq;
    using System.Reflection;
    using NHibernate.Util;

    public interface IModelFactory
    {
        object MakeModel(object data);
    }

    public class ModelFactory : IModelFactory
    {
        public dynamic MakeModel(dynamic data)
        {
            var dictionary = new Dictionary<string, object>();
            dictionary.Add("Urls", new Urls());

            var model = new ExpandoObject();
            model.AddOrOverride(dictionary);
            AddData(model, data);
            return model;
        }

        private void AddData(ExpandoObject model, object data)
        {
            if (data == null)
                return;
            var properties = data.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .ToDictionary(e => e.Name, e => e.GetValue(data, null));
            model.AddOrOverride(properties);
        }
    }
}