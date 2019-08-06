
using Research.Data;

namespace Research.Data
{
    /// <summary>
    /// Represents a setting
    /// </summary>
    public partial class Setting : BaseEntity
    {
        public Setting()
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="value">Value</param>
        /// <param name="storeId">Store identifier</param>
        //public Setting(string name, string value, int storeId = 0)
        //{
        //    this.Name = name;
        //    this.Value = value;
        //   // this.StoreId = storeId;
        //}

        public Setting(string name, string value)
        {
            this.Name = name;
            this.Value = value;
            // this.StoreId = storeId;
        }
        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the store for which this setting is valid. 0 is set when the setting is for all stores
        /// </summary>
       // public int StoreId { get; set; }

        /// <summary>
        /// To string
        /// </summary>
        /// <returns>Result</returns>
        public override string ToString()
        {
            return Name;
        }
    }
}