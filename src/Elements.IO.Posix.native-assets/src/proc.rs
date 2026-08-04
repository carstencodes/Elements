use rustix::process;
use std::ffi::c_uint;

pub fn get_current_user_ids() -> (c_uint, c_uint, c_uint, c_uint) {
    let uid: process::Uid = process::getuid();
    let gid: process::Gid = process::getgid();
    let euid: process::Uid = process::geteuid();
    let egid: process::Gid = process::getegid();

    (uid.as_raw(), gid.as_raw(), euid.as_raw(), egid.as_raw())
}
