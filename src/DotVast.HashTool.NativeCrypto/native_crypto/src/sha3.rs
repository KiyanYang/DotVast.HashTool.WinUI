use crate::hasher::impl_hasher;
use sha3::{Keccak224, Keccak256, Keccak384, Keccak512, Sha3_224, Sha3_256, Sha3_384, Sha3_512};

impl_hasher!("sha3_224", Sha3_224);
impl_hasher!("sha3_256", Sha3_256);
impl_hasher!("sha3_384", Sha3_384);
impl_hasher!("sha3_512", Sha3_512);

impl_hasher!("keccak224", Keccak224);
impl_hasher!("keccak256", Keccak256);
impl_hasher!("keccak384", Keccak384);
impl_hasher!("keccak512", Keccak512);
