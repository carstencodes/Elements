use rustix::process;
use std::ffi::c_uint;

pub fn get_current_user_ids() -> (c_uint, c_uint, c_uint, c_uint) {
    let current_user_id: process::Uid = process::getuid();
    let current_group_id: process::Gid = process::getgid();
    let elevated_user_id: process::Uid = process::geteuid();
    let elevated_group_id: process::Gid = process::getegid();

    (
        current_user_id.as_raw(),
        current_group_id.as_raw(),
        elevated_user_id.as_raw(),
        elevated_group_id.as_raw(),
    )
}
