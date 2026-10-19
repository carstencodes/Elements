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
    ptr_user_id: *mut u32,
    ptr_group_id: *mut u32,
    ptr_elevated_user_id: *mut u32,
    ptr_elevated_group_id: *mut u32,
) -> u16 {
    if ptr_user_id.is_null() {
        return CallResult::NullParameter.into();
    }
    if ptr_group_id.is_null() {
        return CallResult::NullParameter.into();
    }
    if ptr_elevated_user_id.is_null() {
        return CallResult::NullParameter.into();
    }
    if ptr_elevated_group_id.is_null() {
        return CallResult::NullParameter.into();
    }

    let (current_user_id, current_group_id, elevated_user_id, elevated_group_id) = proc::get_current_user_ids();

    let tmp_current_user_id = current_user_id;
    let tmp_current_group_id = current_group_id;
    let tmp_elev_user_id = elevated_user_id;
    let tmp_elev_group_id = elevated_group_id;

    unsafe {
        *ptr_user_id = tmp_current_user_id;
        *ptr_group_id = tmp_current_group_id;
        *ptr_elevated_user_id = tmp_elev_user_id;
        *ptr_elevated_group_id = tmp_elev_group_id;
    }

    CallResult::Ok.into()
}
