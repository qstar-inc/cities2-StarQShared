using Unity.Collections;
using Unity.Entities;

namespace StarQ.Shared.Extensions
{
    public static class HashMapHelper
    {
        public static NativeHashMap<FixedString64Bytes, Entity> CreateHashMapStringEntity(
            int cap = 1024
        ) => new(cap, Allocator.Persistent);

        public static NativeHashMap<FixedString64Bytes, int> CreateHashMapStringInt(
            int cap = 512
        ) => new(cap, Allocator.Persistent);
    }
}
