using OCP;

namespace OC.Files.Cache
{
/**
 * In-memory cache with a capacity limit to keep memory usage in check
 *
 * Uses a simple FIFO expiry mechanism
 */
    public class CappedMemoryCache : ICache, ArrayAccess {

    private int capacity;
    private  cache = [];

    public CappedMemoryCache(int capacity = 512) {
        this.capacity = capacity;
    }

    public function hasKey(key) {
        return isset(this.cache[key]);
    }

    public object get(string key) {
        return isset(this.cache[key]) ? this.cache[key] : null;
    }

    public void set(string key, object value, int ttl = 0) {
        if (is_null(key)) {
            this.cache[] = value;
        } else {
            this.cache[key] = value;
        }
        this.garbageCollect();
    }

    public function remove(key) {
        unset(this.cache[key]);
        return true;
    }

    public function clear(prefix = '') {
        this.cache = [];
        return true;
    }

    public function offsetExists(offset) {
        return this.hasKey(offset);
    }

    public function &offsetGet(offset) {
        return this.cache[offset];
    }

    public function offsetSet(offset, value) {
        this.set(offset, value);
    }

    public function offsetUnset(offset) {
        this.remove(offset);
    }

    public function getData() {
        return this.cache;
    }


    private function garbageCollect() {
        while (count(this.cache) > this.capacity) {
            reset(this.cache);
            key = key(this.cache);
            this.remove(key);
        }
    }
    }

}