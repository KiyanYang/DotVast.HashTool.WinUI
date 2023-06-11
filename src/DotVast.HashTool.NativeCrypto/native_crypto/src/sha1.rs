use crate::hasher::impl_hasher;
use sha1::Sha1;

impl_hasher!("sha1", Sha1);
