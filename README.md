# idx2dec

Command line tool to extract the IDOLM@STER Dearly Stars BIN file.

---

## Download

<s>[Release](https://github.com/XyLe-GBP/idx2dec/Release)</s>

Currently, there is no release build.

You will need to build your application from the c-sharp source files.

---

## Description

Supports extract of IDOLM@STER Dearly Stars archive files.

This tool uses this arguments:

  `idx2dec <BIN-PATH> <IDX-PATH>`
  
The `<BIN-PATH>` argument is the full path of the BIN archive file.
  
The `<IDX-PATH>` argument is the full path of the index file (*.IDX) containing the file's packed information.

Example usage:

`D:\DebugDirectory\idx2dec\F_BGM.BIN D:\DebugDirectory\idx2dec\F_BGM.IDX` (Extract F_BGM.BIN)
  
```diff
- ※INPORTANT※

- BIN and IDX files should be placed in the same directory as the application.

- Otherwise, the files may fail to extract.
```

---

### About Licensing

<p>This tool is released under the MIT license.</p>
