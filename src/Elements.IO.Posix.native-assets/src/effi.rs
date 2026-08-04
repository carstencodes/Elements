pub enum CallResult {
    Ok = 0,
    NullParameter = 1,
}

impl From<CallResult> for u16 {
    fn from(value: CallResult) -> Self {
        match value {
            CallResult::Ok => 0,
            CallResult::NullParameter => 1,
        }
    }
}
