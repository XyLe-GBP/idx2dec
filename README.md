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

## ScreenShots

![image](https://user-images.githubusercontent.com/59692068/119649540-b43c0380-be5d-11eb-839a-882bd197fc11.png)

![image](https://user-images.githubusercontent.com/59692068/119649796-054bf780-be5e-11eb-8d79-20e840f194bb.png)

![image](https://user-images.githubusercontent.com/59692068/119649833-139a1380-be5e-11eb-8b48-2f8b88d42efe.png)

---

### About Licensing

<p>This tool is released under the MIT license.</p>

Perl Script by: mirai-iro.
