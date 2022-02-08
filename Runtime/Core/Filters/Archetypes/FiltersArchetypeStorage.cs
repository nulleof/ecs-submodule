#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ME.ECS.Collections;
using Unity.IL2CPP.CompilerServices;
using Il2Cpp = Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute;

#if FILTERS_STORAGE_ARCHETYPES
namespace ME.ECS.FiltersArchetype {

    [Il2Cpp(Option.NullChecks, false)]
    [Il2Cpp(Option.ArrayBoundsChecks, false)]
    [Il2Cpp(Option.DivideByZeroChecks, false)]
    public struct FiltersArchetypeStorage : IStorage {

        [Il2Cpp(Option.NullChecks, false)]
        [Il2Cpp(Option.ArrayBoundsChecks, false)]
        [Il2Cpp(Option.DivideByZeroChecks, false)]
        public struct Archetype {

            [Il2Cpp(Option.NullChecks, false)]
            [Il2Cpp(Option.ArrayBoundsChecks, false)]
            [Il2Cpp(Option.DivideByZeroChecks, false)]
            public struct CopyData : IArrayElementCopy<Archetype> {

                #if INLINE_METHODS
                [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
                #endif
                public void Copy(Archetype @from, ref Archetype to) {

                    to.CopyFrom(in from);

                }

                #if INLINE_METHODS
                [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
                #endif
                public void Recycle(Archetype item) {

                    item.Recycle();

                }

            }

            #if INLINE_METHODS
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            #endif
            private void CopyFrom(in Archetype other) {

                this.index = other.index;
                ArrayUtils.Copy(other.componentIds, ref this.componentIds);
                ArrayUtils.Copy(other.entitiesArr, ref this.entitiesArr);
                
                ArrayUtils.Copy(other.edgesToAdd, ref this.edgesToAdd);
                ArrayUtils.Copy(other.edgesToRemove, ref this.edgesToRemove);
                ArrayUtils.Copy(other.components, ref this.components);

            }

            #if INLINE_METHODS
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            #endif
            public void Recycle() {

                this.index = default;
                PoolDictionaryInt<Info>.Recycle(ref this.components);
                PoolList<int>.Recycle(ref this.componentIds);
                PoolListCopyable<int>.Recycle(ref this.entitiesArr);
                PoolDictionaryInt<int>.Recycle(ref this.edgesToAdd);
                PoolDictionaryInt<int>.Recycle(ref this.edgesToRemove);

            }

            public struct Info {

                public int index; // Index in list

            }

            public int index;
            public DictionaryInt<Info> components; // Contains componentId => Info index
            public List<int> componentIds; // Contains raw list of component ids
            public ListCopyable<int> entitiesArr; // Contains raw list of entities
            public DictionaryInt<int> edgesToAdd; // Contains edges to move from this archetype to another
            public DictionaryInt<int> edgesToRemove; // Contains edges to move from this archetype to another

            //private bool isCreated;

            #if INLINE_METHODS
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            #endif
            internal readonly bool HasAnyPair(List<FilterInternalData.Pair2> list) {

                foreach (var pair in list) {

                    if (this.Has(pair.t1) == false &&
                        this.Has(pair.t2) == false) {
                        return false;
                    }

                }

                return true;

            }

            #if INLINE_METHODS
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            #endif
            internal readonly bool HasAnyPair(List<FilterInternalData.Pair3> list) {

                foreach (var pair in list) {

                    if (this.Has(pair.t1) == false &&
                        this.Has(pair.t2) == false &&
                        this.Has(pair.t3) == false) {
                        return false;
                    }

                }

                return true;

            }

            #if INLINE_METHODS
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            #endif
            internal readonly bool HasAnyPair(List<FilterInternalData.Pair4> list) {

                foreach (var pair in list) {

                    if (this.Has(pair.t1) == false &&
                        this.Has(pair.t2) == false &&
                        this.Has(pair.t3) == false &&
                        this.Has(pair.t4) == false) {
                        return false;
                    }

                }

                return true;

            }

            #if INLINE_METHODS
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            #endif
            public readonly bool Has(int componentId) {

                return this.components.ContainsKey(componentId);

            }

            #if INLINE_METHODS
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            #endif
            public readonly bool HasAll(List<int> componentIds) {

                foreach (var item in componentIds) {

                    if (this.components.ContainsKey(item) == false) {
                        return false;
                    }

                }

                return true;

            }

            #if INLINE_METHODS
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            #endif
            public readonly bool HasNotAll(List<int> componentIds) {

                foreach (var item in componentIds) {

                    if (this.components.ContainsKey(item) == true) {
                        return false;
                    }

                }

                return true;

            }

            #if INLINE_METHODS
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            #endif
            public readonly bool HasAllExcept(List<int> componentIds, int componentId) {

                foreach (var item in componentIds) {

                    if (item == componentId) {
                        continue;
                    }

                    if (this.components.ContainsKey(item) == false) {
                        return false;
                    }

                }

                return true;

            }

            #if INLINE_METHODS
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            #endif
            public Archetype Set(ref FiltersArchetypeStorage storage, Entity entity, int componentId) {

                if (this.Has(componentId) == true) {
                    return this;
                }

                // Remove entity from current archetype
                storage.RemoveEntityFromArch(ref this, entity.id);

                // Find the edge to move
                ref var edge = ref this.edgesToAdd.GetValue(componentId, out var exist);
                if (exist == false) {
                    edge = Archetype.CreateAdd(ref storage, this.index, this.componentIds, this.components, componentId);
                }
                
                {
                    ref var arch = ref storage.allArchetypes[edge];
                    storage.AddEntityToArch(ref arch, entity.id);
                }

                // Return the new archetype we are moved to
                return storage.allArchetypes[edge];

            }

            #if INLINE_METHODS
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            #endif
            public Archetype Remove(ref FiltersArchetypeStorage storage, Entity entity, int componentId) {

                if (this.Has(componentId) == false) {
                    return this;
                }

                // Remove entity from current archetype
                storage.RemoveEntityFromArch(ref this, entity.id);

                // Find the edge to move
                ref var edge = ref this.edgesToRemove.GetValue(componentId, out var exist);
                if (exist == false) {
                    edge = Archetype.CreateRemove(ref storage, this.index, this.componentIds, this.components, componentId);
                }

                {
                    ref var arch = ref storage.allArchetypes[edge];
                    storage.AddEntityToArch(ref arch, entity.id);
                }

                // Return the new archetype we are moved to
                return storage.allArchetypes[edge];

            }

            #if INLINE_METHODS
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            #endif
            internal static int CreateAdd(ref FiltersArchetypeStorage storage, int node, List<int> componentIds, DictionaryInt<Info> components, int componentId) {

                if (storage.TryGetArchetypeAdd(componentIds, componentId, out var ar) == true) {
                    return ar;
                }

                var arch = new Archetype() {
                    //isCreated = true,
                    edgesToAdd = PoolDictionaryInt<int>.Spawn(16),
                    edgesToRemove = PoolDictionaryInt<int>.Spawn(16),
                    entitiesArr = PoolListCopyable<int>.Spawn(16),
                    componentIds = PoolList<int>.Spawn(componentIds.Count),
                    components = PoolDictionaryInt<Info>.Spawn(components.Count),
                    //componentStorage = new ComponentStorage[componentIds.Count + 1],
                };
                foreach (var c in components) {
                    arch.components.Add(c.Key, c.Value);
                }

                arch.componentIds.AddRange(componentIds);
                storage.isArchetypesDirty = true;
                var idx = storage.allArchetypes.Count;
                arch.index = idx;
                storage.allArchetypes.Add(arch);
                arch.components.Add(componentId, new Info() {
                    index = arch.componentIds.Count,
                });
                arch.componentIds.Add(componentId);
                if (node >= 0) {
                    arch.edgesToRemove.Add(componentId, node);
                }

                return idx;

            }

            #if INLINE_METHODS
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            #endif
            internal static int CreateRemove(ref FiltersArchetypeStorage storage, int node, List<int> componentIds, DictionaryInt<Info> components, int componentId) {

                if (storage.TryGetArchetypeRemove(componentIds, componentId, out var ar) == true) {
                    return ar;
                }

                var arch = new Archetype() {
                    //isCreated = true,
                    edgesToAdd = PoolDictionaryInt<int>.Spawn(16),
                    edgesToRemove = PoolDictionaryInt<int>.Spawn(16),
                    entitiesArr = PoolListCopyable<int>.Spawn(16),
                    componentIds = PoolList<int>.Spawn(componentIds.Count),
                    components = PoolDictionaryInt<Info>.Spawn(16),
                    //componentStorage = new ComponentStorage[componentIds.Count - 1],
                };
                arch.componentIds.AddRange(componentIds);
                storage.isArchetypesDirty = true;
                var idx = storage.allArchetypes.Count;
                arch.index = idx;
                storage.allArchetypes.Add(arch);
                var info = components[componentId];
                arch.componentIds.RemoveAt(info.index);
                for (var i = 0; i < arch.componentIds.Count; ++i) {
                    var cId = arch.componentIds[i];
                    arch.components.Add(cId, new Info() {
                        index = i,
                    });
                }

                if (node >= 0) {
                    arch.edgesToAdd.Add(componentId, node);
                }

                return idx;

            }

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        private void RemoveEntityFromArch(ref Archetype arch, int entityId) {

            var idx = this.GetEntityArrIndex(entityId);
            var movedEntityId = arch.entitiesArr[arch.entitiesArr.Count - 1];
            arch.entitiesArr.RemoveAtFast(idx);
            if (movedEntityId != entityId) this.SetEntityArrIndex(movedEntityId, idx);
            this.SetEntityArrIndex(entityId, -1);
            
        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        private void AddEntityToArch(ref Archetype arch, int entityId) {

            var idx = arch.entitiesArr.Count;
            arch.entitiesArr.Add(entityId);
            this.SetEntityArrIndex(entityId, idx);
            
        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        private int GetEntityArrIndex(int entityId) {

            ArrayUtils.Resize(entityId, ref this.entitiesArrIndex);
            return this.entitiesArrIndex[entityId];

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        private void SetEntityArrIndex(int entityId, int index) {

            ArrayUtils.Resize(entityId, ref this.entitiesArrIndex);
            this.entitiesArrIndex[entityId] = index;

        }

        public struct NullArchetypes {

            #if INLINE_METHODS
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            #endif
            public void Set<T>(in Entity entity) { }
            #if INLINE_METHODS
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            #endif
            public void Remove<T>(in Entity entity) { }

            #if INLINE_METHODS
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            #endif
            public void Set(in Entity entity, int componentId) { }
            #if INLINE_METHODS
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            #endif
            public void Remove(in Entity entity, int componentId) { }

            #if INLINE_METHODS
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            #endif
            public void Set(in EntitiesGroup group, int componentId) { }
            #if INLINE_METHODS
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            #endif
            public void Remove(in EntitiesGroup group, int componentId) { }

            #if INLINE_METHODS
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            #endif
            public void Set(int entityId, int componentId) { }
            #if INLINE_METHODS
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            #endif
            public void Remove(int entityId, int componentId) { }

            #if INLINE_METHODS
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            #endif
            public void Clear(in Entity entity) { }

            #if INLINE_METHODS
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            #endif
            public void Validate(int capacity) { }

            #if INLINE_METHODS
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            #endif
            public void Validate(in Entity entity) { }

            #if INLINE_METHODS
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            #endif
            public void CopyFrom(in Entity @from, in Entity to) { }

        }

        private struct Request {

            public Entity entity;
            public byte op;
            public int componentId;

        }

        [ME.ECS.Serializer.SerializeField]
        internal int forEachMode;
        [ME.ECS.Serializer.SerializeField]
        internal int root;
        [ME.ECS.Serializer.SerializeField]
        internal DictionaryULong<int> index;
        [ME.ECS.Serializer.SerializeField]
        internal ListCopyable<Archetype> allArchetypes;
        [ME.ECS.Serializer.SerializeField]
        internal List<FilterData> filters;
        [ME.ECS.Serializer.SerializeField]
        internal EntityVersions versions;
        [ME.ECS.Serializer.SerializeField]
        internal BufferArray<int> entitiesArrIndex;

        internal NullArchetypes archetypes; // Used for backward compability

        #region Entities Storage
        public int AliveCount => this.aliveCount;
        public int DeadCount => this.dead.Count;

        internal NativeBufferArray<Entity> cache;
        [ME.ECS.Serializer.SerializeField]
        internal ListCopyable<int> dead;
        [ME.ECS.Serializer.SerializeField]
        internal ListCopyable<int> deadPrepared;
        [ME.ECS.Serializer.SerializeField]
        internal ListCopyable<int> alive;
        [ME.ECS.Serializer.SerializeField]
        private int aliveCount;
        [ME.ECS.Serializer.SerializeField]
        private int nextEntityId;
        [ME.ECS.Serializer.SerializeField]
        internal bool isCreated;

        private List<Request> requests;
        private bool isArchetypesDirty;

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public void Initialize(int capacity) {

            this.entitiesArrIndex = PoolArray<int>.Spawn(capacity);

            this.cache = PoolArrayNative<Entity>.Spawn(capacity);
            this.dead = PoolListCopyable<int>.Spawn(capacity);
            this.alive = PoolListCopyable<int>.Spawn(capacity);
            this.deadPrepared = PoolListCopyable<int>.Spawn(capacity);
            this.versions = new EntityVersions();
            this.aliveCount = 0;
            this.nextEntityId = -1;
            this.isCreated = true;
            this.forEachMode = 0;
            this.isArchetypesDirty = false;

            this.requests = PoolList<Request>.Spawn(10);

            var arch = new Archetype() {
                edgesToAdd = PoolDictionaryInt<int>.Spawn(16),
                edgesToRemove = PoolDictionaryInt<int>.Spawn(16),
                entitiesArr = PoolListCopyable<int>.Spawn(16),
                componentIds = PoolList<int>.Spawn(10),
                components = PoolDictionaryInt<Archetype.Info>.Spawn(16),
                index = 0,
            };
            this.root = arch.index;
            this.index = PoolDictionaryULong<int>.Spawn(16);
            this.allArchetypes = PoolListCopyable<Archetype>.Spawn(capacity);
            this.filters = PoolList<FilterData>.Spawn(capacity);
            this.allArchetypes.Add(arch);

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public void CopyFrom(FiltersArchetypeStorage other) {

            if (this.isCreated == false || this.requests == null) {
                this.Initialize(0);
            }

            ArrayUtils.Copy(other.requests, ref this.requests);

            ArrayUtils.Copy(other.entitiesArrIndex, ref this.entitiesArrIndex);

            this.isCreated = other.isCreated;
            this.forEachMode = other.forEachMode;
            NativeArrayUtils.Copy(other.cache, ref this.cache);
            ArrayUtils.Copy(other.dead, ref this.dead);
            ArrayUtils.Copy(other.alive, ref this.alive);
            ArrayUtils.Copy(other.deadPrepared, ref this.deadPrepared);
            this.aliveCount = other.aliveCount;
            this.nextEntityId = other.nextEntityId;
            this.versions.CopyFrom(other.versions);
            this.isArchetypesDirty = other.isArchetypesDirty;

            this.root = other.root;
            ArrayUtils.Copy(other.filters, ref this.filters, new FilterData.CopyData());
            ArrayUtils.Copy(other.index, ref this.index);
            
            ArrayUtils.Copy(other.allArchetypes, ref this.allArchetypes, new Archetype.CopyData());

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public void Recycle() {

            for (int i = 0; i < this.allArchetypes.Count; ++i) {
                
                this.allArchetypes[i].Recycle();
                
            }
            
            this.versions.Recycle();
            this.versions = default;
            
            this.root = default;
            PoolDictionaryULong<int>.Recycle(ref this.index);
            PoolListCopyable<Archetype>.Recycle(ref this.allArchetypes);
            PoolList<FilterData>.Recycle(ref this.filters);
            this.isArchetypesDirty = default;

            PoolArray<int>.Recycle(ref this.entitiesArrIndex);

            PoolList<Request>.Recycle(ref this.requests);

            PoolArrayNative<Entity>.Recycle(ref this.cache);
            PoolListCopyable<int>.Recycle(ref this.dead);
            PoolListCopyable<int>.Recycle(ref this.alive);
            PoolListCopyable<int>.Recycle(ref this.deadPrepared);
            this.forEachMode = default;
            this.isCreated = false;
            this.aliveCount = 0;
            this.nextEntityId = -1;

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public ref Entity GetEntityById(int id) {

            return ref this.cache[id];

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public bool IsAlive(int id, ushort generation) {

            return this.cache[id].generation == generation;

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public bool ForEach(ListCopyable<Entity> results) {

            results.Clear();
            for (var i = 0; i < this.alive.Count; ++i) {
                results.Add(this.GetEntityById(this.alive[i]));
            }

            return true;

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public void Alloc(int count, ref EntitiesGroup group, Unity.Collections.Allocator allocator, bool copyMode) {

            var lastId = ++this.nextEntityId + count;
            NativeArrayUtils.Resize(lastId, ref this.cache);

            this.aliveCount += count;

            var from = this.nextEntityId;
            var id = this.nextEntityId;
            for (var i = 0; i < count; ++i) {
                this.cache.arr[id] = new Entity(id, 1);
                this.OnAlloc(id);
                this.alive.Add(id++);
            }

            this.versions.Reset(id);

            this.nextEntityId += count;

            var slice = new Unity.Collections.NativeSlice<Entity>(this.cache.arr, from, count);
            var array = new Unity.Collections.NativeArray<Entity>(count, allocator, Unity.Collections.NativeArrayOptions.UninitializedMemory);
            slice.CopyTo(array);
            group = new EntitiesGroup(from, from + count - 1, array, copyMode);

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public ref Entity Alloc() {

            var id = -1;
            if (this.dead.Count > 0) {

                id = this.dead[0];
                this.dead.RemoveAtFast(0);

            } else {

                id = ++this.nextEntityId;
                NativeArrayUtils.Resize(id, ref this.cache);

            }

            ++this.aliveCount;
            ref var e = ref this.cache[id];
            this.alive.Add(id);
            if (e.generation == 0) {
                e = new Entity(id, 1);
            }

            this.versions.Reset(id);

            this.OnAlloc(e.id);

            return ref e;

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public bool Dealloc(in Entity entity) {

            if (this.IsAlive(entity.id, entity.generation) == false) {
                return false;
            }

            //using (NoStackTrace.All) UnityEngine.Debug.Log("Dealloc: " + entity + ", tick: " + Worlds.current.GetCurrentTick());
            this.deadPrepared.Add(entity.id);

            return true;

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public void ApplyDead() {

            var cnt = this.deadPrepared.Count;
            if (cnt > 0) {

                for (var i = 0; i < cnt; ++i) {

                    var id = this.deadPrepared[i];
                    --this.aliveCount;
                    this.dead.Add(id);
                    this.alive.Remove(id);
                    this.OnDealloc(id);

                }

                this.deadPrepared.Clear();

            }

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        private void OnAlloc(int entityId) {

            ref var arch = ref this.allArchetypes[this.root];
            this.AddEntityToArch(ref arch, entityId);
            this.index.Add((ulong)entityId << 32, this.root);

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        private void OnDealloc(int entityId) {

            // Remove from archetype
            var key = (ulong)entityId << 32;
            var archIdx = this.index.GetValueAndRemove(key);
            ref var arch = ref this.allArchetypes[archIdx];
            this.RemoveEntityFromArch(ref arch, entityId);

        }
        #endregion

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public FilterData GetFilter(int id) {

            return this.filters[id - 1];

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        private bool TryGetArchetypeAdd(List<int> componentIds, int componentId, out int arch) {

            // Try to search archetype with componentIds + componentId contained in
            arch = default;
            for (var i = 0; i < this.allArchetypes.Count; ++i) {

                var ar = this.allArchetypes[i];
                if (ar.componentIds.Count == componentIds.Count &&
                    ar.Has(componentId) == true &&
                    ar.HasAll(componentIds) == true) {

                    arch = i;
                    return true;

                }

            }

            return false;

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        private bool TryGetArchetypeRemove(List<int> componentIds, int componentId, out int arch) {

            // Try to search archetype with componentIds except componentId contained in
            arch = default;
            for (var i = 0; i < this.allArchetypes.Count; ++i) {

                var ar = this.allArchetypes[i];
                if (ar.componentIds.Count == componentIds.Count - 1 &&
                    ar.Has(componentId) == false &&
                    ar.HasAllExcept(componentIds, componentId) == true) {

                    arch = i;
                    return true;

                }

            }

            return false;

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public bool Has<T>(in Entity entity) where T : struct {

            var key = (ulong)entity.id << 32;
            var archIdx = this.index[key];
            var arch = this.allArchetypes[archIdx];
            return arch.Has(ComponentTypes<T>.typeId);

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public void Set(in EntitiesGroup group, int componentId) {

            for (var i = group.fromId; i <= group.toId; ++i) {

                this.Set(this.GetEntityById(i), componentId);

            }

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public void Remove(in EntitiesGroup group, int componentId) {

            for (var i = group.fromId; i <= group.toId; ++i) {

                this.Remove(this.GetEntityById(i), componentId);

            }

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public void Validate<T>(in Entity entity, bool makeRequest) where T : struct {

            this.Validate(in entity, ComponentTypes<T>.typeId, makeRequest);

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public void Validate(in Entity entity, int componentId, bool makeRequest) {

            if (makeRequest == true) {

                // Add request and apply set on next UpdateFilters call
                this.AddValidateRequest(in entity, componentId);

            } else {

                if (ComponentTypesLambda.itemsSet.TryGetValue(componentId, out var lambda) == true) {
                    lambda.Invoke(entity);
                }

            }

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public void Set(in Entity entity, int componentId) {

            if (ComponentTypesLambda.itemsSet.TryGetValue(componentId, out var lambda) == true) {
                lambda.Invoke(entity);
            }

            if (this.forEachMode > 0) {

                // Add request
                this.AddSetRequest(in entity, componentId);
                return;

            }

            var key = (ulong)entity.id << 32;
            ref var archIdx = ref this.index.GetValue(key);
            ref var arch = ref this.allArchetypes[archIdx];
            archIdx = arch.Set(ref this, entity, componentId).index;

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public void Set<T>(in Entity entity) where T : struct {

            this.Set(in entity, ComponentTypes<T>.typeId);

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public void Remove(in Entity entity) { }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public void Remove(in Entity entity, int componentId) {

            if (ComponentTypesLambda.itemsRemove.TryGetValue(componentId, out var lambda) == true) {
                lambda.Invoke(entity);
            }

            if (this.forEachMode > 0) {

                // Add request
                this.AddRemoveRequest(in entity, componentId);
                return;

            }

            var key = (ulong)entity.id << 32;
            ref var archIdx = ref this.index.GetValue(key);
            ref var arch = ref this.allArchetypes[archIdx];
            archIdx = arch.Remove(ref this, entity, componentId).index;

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public void Remove<T>(in Entity entity) where T : struct {

            this.Remove(in entity, ComponentTypes<T>.typeId);

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        private void ApplyAllRequests() {

            foreach (var req in this.requests) {

                if (req.entity.IsAlive() == false) {
                    continue;
                }

                if (req.op == 1) {

                    this.Set(in req.entity, req.componentId);

                } else if (req.op == 2) {

                    this.Remove(in req.entity, req.componentId);

                } else if (req.op == 3) {

                    this.Validate(in req.entity, req.componentId, false);

                }

            }

            this.requests.Clear();

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        private void AddValidateRequest(in Entity entity, int componentId) {

            this.requests.Add(new Request() {
                entity = entity,
                componentId = componentId,
                op = 3,
            });

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        private void AddSetRequest(in Entity entity, int componentId) {

            this.requests.Add(new Request() {
                entity = entity,
                componentId = componentId,
                op = 1,
            });

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        private void AddRemoveRequest(in Entity entity, int componentId) {

            this.requests.Add(new Request() {
                entity = entity,
                componentId = componentId,
                op = 2,
            });

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public int Count(Filter filter) {

            return this.Count(this.GetFilter(filter.id));

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public int Count(FilterData filter) {

            if (this.forEachMode == 0) {
                this.UpdateFilters();
            }

            var count = 0;
            foreach (var archId in filter.archetypes) {

                var arch = this.allArchetypes[archId];
                count += arch.entitiesArr.Count;

            }

            return count;

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public void UpdateFilters() {

            if (this.forEachMode > 0) {
                return;
            }

            this.ApplyDead();
            this.ApplyAllRequests();

            if (this.isArchetypesDirty == true) {

                this.isArchetypesDirty = false;
                foreach (var item in this.filters) {

                    item.archetypes.Clear();

                    for (var i = 0; i < this.allArchetypes.Count; ++i) {

                        var arch = this.allArchetypes[i];
                        if (arch.HasAll(item.data.contains) == true &&
                            arch.HasNotAll(item.data.notContains) == true &&
                            arch.HasAnyPair(item.data.anyPair2) == true &&
                            arch.HasAnyPair(item.data.anyPair3) == true &&
                            arch.HasAnyPair(item.data.anyPair4) == true &&
                            this.CheckStaticShared(item.data.containsShared, item.data.notContainsShared) == true &&
                            this.CheckLambdas(in arch, item.data.lambdas) == true) {

                            item.archetypes.Add(i);

                        }

                    }

                }

            }

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        private bool CheckLambdas(in Archetype arch, List<int> lambdas) {

            return arch.HasAll(lambdas);

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        private bool CheckStaticShared(List<int> containsShared, List<int> notContainsShared) {

            if (containsShared.Count == 0 && notContainsShared.Count == 0) {
                return true;
            }

            var w = Worlds.current;
            for (int i = 0, count = containsShared.Count; i < count; ++i) {

                if (w.HasSharedDataBit(containsShared[i]) == false) {
                    return false;
                }

            }

            for (int i = 0, count = notContainsShared.Count; i < count; ++i) {

                if (w.HasSharedDataBit(notContainsShared[i]) == true) {
                    return false;
                }

            }

            return true;

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public bool TryGetFilter(FilterBuilder filterBuilder, out FilterData filterData) {

            filterData = default;

            for (int i = 0, cnt = this.filters.Count; i < cnt; ++i) {

                var filter = this.filters[i];
                if (this.IsEquals(filter.data.contains, filterBuilder.data.contains) == true &&
                    this.IsEquals(filter.data.notContains, filterBuilder.data.notContains) == true &&
                    this.IsEquals(filter.data.notContainsShared, filterBuilder.data.notContainsShared) == true &&
                    this.IsEquals(filter.data.containsShared, filterBuilder.data.containsShared) == true &&
                    this.IsEquals(filter.data.lambdas, filterBuilder.data.lambdas) == true) {

                    filterData = filter;
                    return true;

                }

            }

            return false;

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        private bool IsEquals(List<int> list1, List<int> list2) {

            if (list1.Count != list2.Count) {
                return false;
            }

            for (var i = 0; i < list1.Count; ++i) {

                if (list2.Contains(list1[i]) == false) {
                    return false;
                }

            }

            return true;

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public bool WillNew() {

            return this.dead.Count == 0;

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public ListCopyable<int> GetAlive() {

            return this.alive;

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public ref Entity IncrementGeneration(in Entity entity) {

            // Make this entity not alive, but not completely destroyed at this time
            this.cache[entity.id] = new Entity(entity.id, unchecked((ushort)(entity.generation + 1)));
            return ref this.cache[entity.id];

        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public void SetFreeze(bool freeze) { }

    }

}
#endif