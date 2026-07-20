mod effi;
mod proc;

use effi::CallResult;

#[unsafe(no_mangle)]
/// External
pub extern "C" fn fs_owning_user_name(
) -> u16 {
    0
}

#[unsafe(no_mangle)]
/// External
pub extern "C" fn proc_get_process_ids(
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

    let uid: u32 = c_uid.into();
    let gid: u32 = c_gid.into();
    let euid: u32 = c_euid.into();
    let egid: u32 = c_egid.into();

    unsafe {
        *p_uid = uid;
        *p_gid = gid;
        *p_euid = euid;
        *p_egid = egid;
    }

    return CallResult::Ok.into();
}
