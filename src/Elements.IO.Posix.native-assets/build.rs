fn main() {
    println!("cargo:rerun-if-changed=src/lib.rs");
    csbindgen::Builder::default()
        .input_extern_file("src/lib.rs")
        .csharp_dll_name("posixioelements")
        .csharp_namespace("Elements.IO.Posix.Internal")
        .csharp_class_accessibility("internal")
        .generate_csharp_file("bindings/NativeMethods.g.cs")
        .unwrap();
}
