namespace ME.ECS.Collections.MemoryAllocator {

    public static class Helpers {

        public static int NextPot(int n) {

            --n;
            n |= n >> 1;
            n |= n >> 2;
            n |= n >> 4;
            n |= n >> 8;
            n |= n >> 16;
            ++n;
            return n;

        }

    }

}