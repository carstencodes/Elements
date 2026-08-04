mod effi;
mod proc;

use effi::CallResult;

#[unsafe(no_mangle)]
/// External
pub extern "C" fn fs_owning_user_name() -> u16 {
    0
}

#[unsafe(no_mangle)]
/// External
///
/// # Safety
/// The caller must ensure that all pointer arguments are valid and non-null.
pub unsafe extern "C" fn proc_get_process_ids(
    p_uid: *mut u32,
    p_gid: *mut u32,
    p_euid: *mut u32,
    p_egid: *mut u32,
) -> u16 {
    if p_uid.is_null() {
        return CallResult::NullParameter.into();
    }
    if p_gid.is_null() {
        return CallResult::NullParameter.into();
    }
    if p_euid.is_null() {
        return CallResult::NullParameter.into();
    }
    if p_egid.is_null() {
        return CallResult::NullParameter.into();
    }

    let (c_uid, c_gid, c_euid, c_egid) = proc::get_current_user_ids();

    let uid = c_uid;
    let gid = c_gid;
    let euid = c_euid;
    let egid = c_egid;

    unsafe {
        *p_uid = uid;
        *p_gid = gid;
        *p_euid = euid;
        *p_egid = egid;
    }

    CallResult::Ok.into()
}
