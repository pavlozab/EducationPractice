using System;
using Newtonsoft.Json;

namespace Task_02
{
    /// <summary> Base Product class. </summary>
    public abstract class BaseProduct
    {
        public Guid Id { get; }

        /// <summary> Constructor for json serializer. </summary>
        [JsonConstructor]
        protected BaseProduct()
        {
            Id = Guid.NewGuid();
        }
        
        /// <summary>Returns a String which represents the object instance.</summary>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}