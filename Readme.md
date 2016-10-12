Simple server to mimic a key/value store.

first commit:

- no server tests (not even run it so far)
- simple global lock on cache
- started with simple string cache, then implemented a LRU as put/get would essentially refresh the timestamp
- minimal tests added for LRU cache