fn main() {
    let libname = "posixioelements";
    let bindings_file = &(format!("bindings/NativeMethods.{}.g.cs", libname));
    println!("cargo:rerun-if-changed=src/lib.rs");
    let build = csbindgen::Builder::default()
        .input_extern_file("src/lib.rs")
        .csharp_dll_name(libname)
        .csharp_namespace("HedgeCraft.Elements.IO.Posix.Internal")
        .csharp_class_accessibility("internal")
        .generate_csharp_file(bindings_file);

    match build {
        Ok(_) => {
            0
        },
        Err(ref e) => {
            eprintln!("Error during build: {:?}", e);
            1
        },
    };

}
