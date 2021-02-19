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
    }
}