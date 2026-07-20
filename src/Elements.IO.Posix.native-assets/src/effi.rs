pub enum CallResult {
    Ok = 0,
    NullParameter = 1
}

impl Into<u16> for CallResult {
    fn into(self) -> u16 {
        match self {
            CallResult::Ok => 0,
            CallResult::NullParameter => 1
        }
    }
}
