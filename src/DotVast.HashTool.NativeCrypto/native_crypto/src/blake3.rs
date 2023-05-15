use crate::hasher::impl_hasher;
use blake3::Hasher;

impl_hasher!("blake3", Hasher);
