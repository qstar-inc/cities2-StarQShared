using System;

namespace StarQ.Shared.Types
{
    public class ModData : IComparable<ModData>
    {
        public string ModId { get; set; } = string.Empty;
        public string ModName { get; set; } = string.Empty;
        public string ModVersion { get; set; } = string.Empty;

        public int CompareTo(ModData other)
        {
            if (other == null)
                return 1;

            return ModId.CompareTo(other.ModId);
        }

        public override bool Equals(object obj)
        {
            if (obj is not ModData other)
                return false;
            return ModId == other.ModId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ModId, ModName, ModVersion);
        }
    }
}
