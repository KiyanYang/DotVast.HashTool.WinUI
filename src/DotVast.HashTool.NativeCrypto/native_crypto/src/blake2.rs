use crate::hasher::impl_hasher;
use blake2::{Blake2b, Blake2s};
use digest::consts::{U16, U20, U28, U32, U48, U64};

type Blake2b160 = Blake2b<U20>;
type Blake2b256 = Blake2b<U32>;
type Blake2b384 = Blake2b<U48>;
type Blake2b512 = Blake2b<U64>;

type Blake2s128 = Blake2s<U16>;
type Blake2s160 = Blake2s<U20>;
type Blake2s225 = Blake2s<U28>;
type Blake2s256 = Blake2s<U32>;

impl_hasher!("blake2b160", Blake2b160);
impl_hasher!("blake2b256", Blake2b256);
impl_hasher!("blake2b384", Blake2b384);
impl_hasher!("blake2b512", Blake2b512);

impl_hasher!("blake2s128", Blake2s128);
impl_hasher!("blake2s160", Blake2s160);
impl_hasher!("blake2s225", Blake2s225);
impl_hasher!("blake2s256", Blake2s256);
