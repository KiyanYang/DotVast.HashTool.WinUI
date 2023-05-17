use digest::{Digest, FixedOutputReset, Reset};

pub unsafe fn new<T: Digest>() -> *mut T {
    Box::into_raw(Box::new(T::new()))
}

pub unsafe fn reset<T: Reset>(hasher: *mut T) {
    let hasher = &mut *hasher;
    hasher.reset();
}

pub unsafe fn update<T: Digest>(hasher: *mut T, ptr: *const u8, size: i32) {
    let hasher = &mut *hasher;
    let slice = std::slice::from_raw_parts(ptr, size.try_into().unwrap());
    hasher.update(slice);
}

pub unsafe fn finalize<T: FixedOutputReset>(hasher: *mut T, ptr: *mut u8, size: i32) {
    let hasher = &mut *hasher;
    let slice = std::slice::from_raw_parts_mut(ptr, size.try_into().unwrap());
    hasher.finalize_into_reset(slice.into());
}

pub unsafe fn free<T>(raw: *mut T) {
    drop(Box::from_raw(raw));
}

macro_rules! impl_hasher {
    (
        $fn_prefix:literal, $hasher:ident
    ) => {
        paste::paste! {
            #[no_mangle]
            pub unsafe fn [<$fn_prefix _new>]() -> *mut $hasher {
                crate::hasher::new::<$hasher>()
            }

            #[no_mangle]
            pub unsafe fn [<$fn_prefix _reset>](hasher: *mut $hasher) {
                crate::hasher::reset(hasher);
            }

            #[no_mangle]
            pub unsafe fn [<$fn_prefix _update>](hasher: *mut $hasher, ptr: *const u8, size: i32) {
                crate::hasher::update(hasher, ptr, size);
            }

            #[no_mangle]
            pub unsafe fn [<$fn_prefix _finalize>](hasher: *mut $hasher, ptr: *mut u8, size: i32) {
                crate::hasher::finalize(hasher, ptr, size);
            }

            #[no_mangle]
            pub unsafe fn [<$fn_prefix _free>](hasher: *mut $hasher) {
                crate::hasher::free(hasher);
            }
        }
    };
}

pub(crate) use impl_hasher;
