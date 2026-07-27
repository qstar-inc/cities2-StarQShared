using System;

namespace StarQ.Shared.Types
{
    public class ModData : IComparable<ModData>
    {
        public string modId;
        public string modName;
        public string modVersion;

        public int CompareTo(ModData other)
        {
            if (other == null)
                return 1;

            return modId.CompareTo(other.modId);
        }

        public override bool Equals(object obj)
        {
            if (obj is not ModData other)
                return false;
            return modId == other.modId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(modId, modName, modVersion);
        }
    }
}
