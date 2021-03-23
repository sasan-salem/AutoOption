using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace AutoOption
{
    /// <summary>
    /// Used to hold a record of Option table in DB
    /// </summary>
    public class OptionEntity
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string Display { get; set; }
        public string Type { get; set; }
        public static IEqualityComparer<OptionEntity> KeyComparer { get; } = new OptionEntityComparer();

        internal string DefaultValue =>
            Type switch
            {
                "Int16" or "UInt16" or "Int32" or "UInt32" or "Int64" or "UInt64"
                 or "Single" or "Double" or "Decimal" => "0",
                "String" => "",
                "Boolean" => "false",
                _ => ""
            };

        public override bool Equals(object obj) => obj is OptionEntity other && Key == other.Key;

        public override int GetHashCode() => Key.GetHashCode();

        private sealed class OptionEntityComparer : IEqualityComparer<OptionEntity>
        {
            public bool Equals(OptionEntity x, OptionEntity y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (x is null) return false;
                if (y is null) return false;
                return x.Key == y.Key;
            }

            public int GetHashCode([DisallowNull] OptionEntity obj) => obj.Key.GetHashCode();
        }
    }
}
