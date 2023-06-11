use crate::hasher::impl_hasher;
use ripemd::{Ripemd128, Ripemd160, Ripemd256, Ripemd320};

impl_hasher!("ripemd128", Ripemd128);
impl_hasher!("ripemd160", Ripemd160);
impl_hasher!("ripemd256", Ripemd256);
impl_hasher!("ripemd320", Ripemd320);
