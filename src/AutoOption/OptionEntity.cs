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

        public override bool Equals(object obj)
        {
            if (obj is OptionEntity other &&
                Key == other.Key &&
                Value == other.Value &&
                Display == other.Display &&
                Type == other.Type)
                return true;
            else
                return false;
        }

        public override int GetHashCode()
        {
            return
                Key.GetHashCode() ^
                Value.GetHashCode() ^
                Display.GetHashCode() ^
                Type.GetHashCode();
        }

        private sealed class OptionEntityComparer : IEqualityComparer<OptionEntity>
        {
            public bool Equals(OptionEntity x, OptionEntity y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (x is null) return false;
                if (y is null) return false;
                return x.Key == y.Key;
            }

            public int GetHashCode([DisallowNull] OptionEntity obj)
            {
                return obj.Key.GetHashCode();
            }
        }
    }
}
