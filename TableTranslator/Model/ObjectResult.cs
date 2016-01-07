using System.Collections.Generic;

namespace TableTranslator.Model
{
    public class ObjectResult<T>
    {
        public T Object { get; set; }
        public Dictionary<string, object> DataBag { get; set; }

        public ObjectResult()
        {
            this.DataBag = new Dictionary<string, object>();
        }
    }
}