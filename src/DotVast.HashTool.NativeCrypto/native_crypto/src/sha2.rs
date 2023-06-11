use crate::hasher::impl_hasher;
use sha2::{Sha224, Sha256, Sha384, Sha512};

impl_hasher!("sha224", Sha224);
impl_hasher!("sha256", Sha256);
impl_hasher!("sha384", Sha384);
impl_hasher!("sha512", Sha512);
