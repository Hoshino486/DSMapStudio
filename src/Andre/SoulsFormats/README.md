
# SoulsFormats

> 本文为简体中文版，英文原文见 [README.en.md](README.en.md)。

一个用于读写各种 FromSoftware 文件格式的 .NET 库，目标框架为 .NET Framework 4.6.2。
重点覆盖《黑暗之魂》《恶魔之魂》与《血源诅咒》，较早的 From 作品可能获得不同程度支持。各格式的支持情况见下文。

## 用法

每种格式都可以通过静态方法 Read 创建对象，参数可以是字节数组或文件路径。推荐传路径，因为它会以流式读取，内存占用更低。
```cs
BND3 bnd = BND3.Read(@"C:\your\path\here.chrbnd");

// 或者

byte[] bytes = File.ReadAllBytes(@"C:\your\path\here.chrbnd");
BND3 bnd = BND3.Read(bytes);
```

Write 方法用于把对象写成新文件。传路径则以流式写入该位置，否则返回字节数组。
```cs
bnd.Write(@"C:\your\path\here.chrbnd");

// 或者

byte[] bytes = bnd.Write();
File.WriteAllBytes(@"C:\your\path\here.chrbnd", bytes);
```

DCX（压缩文件）会被自动解压，压缩类型会被记录，并在写回时原样重新施加。
```cs
BND3 bnd = BND3.Read(@"C:\your\path\here.chrbnd.dcx");
bnd.Write(@"C:\your\path\here.chrbnd.dcx");
```

压缩类型可以通过设置对象的 Compression 字段更改，也可以在调用 Write 时指定。
```cs
BND3 bnd = BND3.Read(@"C:\your\path\here.chrbnd.dcx");
bnd.Write(@"C:\your\path\here.chrbnd", DCX.Type.None);

// 或者

BND3 bnd = BND3.Read(@"C:\your\path\here.chrbnd.dcx");
bnd.Compression = DCX.Type.None;
bnd.Write(@"C:\your\path\here.chrbnd");
```

最后，如有需要，DCX 文件也可以用静态方法直接读写。DCX 不携带重要元数据，因此直接以字节数组进出，而不创建对象。
```cs
byte[] bndBytes = DCX.Decompress(@"C:\your\path\here.chrbnd.dcx");
BND3 bnd = BND3.Read(bndBytes);

// 或者

byte[] dcxBytes = File.ReadAllBytes(@"C:\your\path\here.chrbnd.dcx");
byte[] bndBytes = DCX.Decompress(dcxBytes);
BND3 bnd = BND3.Read(bndBytes);
```

写入新的 DCX 需要传入 DCX.Type 参数以指明所属游戏。DCX.Decompress 有一个可选的 out 参数会回传识别出的类型，通常应当使用它，而不是自行指定。
```cs
byte[] bndBytes = DCX.Decompress(@"C:\your\path\here.chrbnd.dcx", out DCX.Type type);
DCX.Compress(bndBytes, type, @"C:\your\path\here.chrbnd.dcx");

// 或者

byte[] bndBytes = DCX.Decompress(@"C:\your\path\here.chrbnd.dcx", out DCX.Type type);
byte[] dcxBytes = DCX.Compress(bndBytes, type);
File.WriteAllBytes(@"C:\your\path\here.chrbnd.dcx", dcxBytes);
```

## 格式支持

除非某格式在该游戏中根本未被使用，否则兼容性总会列出 DS 1/R/2/3、DeS 与 BB。其他 From 作品的支持情况不定。

### BND3
DS2 之前使用的通用文件容器。
扩展名：`.*bnd`
* DS1：完整读写
* DSR：完整读写
* DeS：部分读写
* NB：完整读写

### BND4
自 DS2 起使用的通用文件容器。
扩展名：`.*bnd`
* DS2：完整读写
* DS3：完整读写
* BB：完整读写

### BTAB
与地图部件的照明/阴影相关。
扩展名：`*.btab`
* DS3：完整读写
* BB：完整读写

### BTL
地图中的点光源。
扩展名：`*.btl`
* DS3：完整读写
* BB：部分读写

### BXF3
本质上是把一个 BND3 拆分为独立的头部与数据文件。
扩展名：`.*bhd`（头部）与 `.*bdt`（数据）
* DS1：完整读写
* DSR：完整读写

### BXF4
本质上是把一个 BND4 拆分为独立的头部与数据文件。
扩展名：`.*bhd`（头部）与 `.*bdt`（数据）
* DS2：完整读写
* DS3：完整读写
* BB：完整读写

### CCM
DeS、DS1、DS2 与 DS3 使用的字库布局文件。
扩展名：`.ccm`
* DeS：完整读写
* DS1：完整读写
* DS2：只读
* DS3：只读

### DCX
NB 之后所有游戏中用于包裹单个压缩文件的封装。
扩展名：`.dcx`
* DS1：完整读取、写入与创建
* DSR：完整读取、写入与创建
* DS2：完整读取、写入与创建
* DS3：完整读取、写入与创建
* DeS：完整读取、写入与创建
* BB：完整读取、写入与创建

### DRB
DS2 之前、在引入 Scaleform 时使用的界面元素配置文件。
扩展名：`.drb`
* DS1：完整读写
* DSR：完整读写
* DeS：不支持
* NB：未测试

### ENFL
未知。据信用于决定随玩家所处位置加载哪些资产。
扩展名：`.entryfilelist`
* DS3：完整读写
* BB：未测试

### ESD
定义一组状态机，用于控制角色、菜单、对话与地图事件。
扩展名：`.esd`
* DS1：完整读取、写入与创建
* DS2：完整读取、写入与创建
* DS3：完整读取、写入与创建
* DeS：不支持
* BB：完整读取、写入与创建

### FLTPARAM
DS2 使用的图形配置格式。
扩展名：`.fltparam`
* DS2：部分读写

### FLVER
自 DS1 起使用的 3D 模型文件。
扩展名：`.flv` 或 `.flver`
* DS1：完整读写
* DSR：完整读写
* DS2：完整读写
* DS3：完整读写
* BB：完整读写

### FLVERD
DS1 之前使用的 3D 模型文件。
扩展名：`.flver`
* DeS：可完整读取，不支持写入
* NB：部分读取，不支持写入

### FMG
系列通用的文本包格式。
扩展名：`.fmg`
* DS1：完整读取、写入与创建
* DS2：完整读取、写入与创建
* DS3：完整读取、写入与创建
* DeS：完整读取、写入与创建
* BB：完整读取、写入与创建

### FXR
DS3 使用的特效格式。
扩展名：`.fxr`
* DS3：只读

### GPARAM
DS3 与 BB 使用的图形配置格式。
扩展名：`.gparam`
* DS3：完整读写
* BB：未测试

### LUAGNL
Lua 脚本的全局变量名列表。
扩展名：`.luagnl`
* DS1：完整读取、写入与创建
* BB：完整读取、写入与创建
* DS3：完整读取、写入与创建

### LUAINFO
Lua 脚本中 AI 目标的相关信息。
扩展名：`.luainfo`
* DS1：完整读取、写入与创建
* BB：完整读取、写入与创建
* DS3：完整读取、写入与创建

### MSB3
DS3 使用的地图定义格式。
扩展名：`.msb`
* DS3：完整读写

### MTD
系列通用的材质定义文件。
扩展名：`.mtd`
* DS1：完整读写
* DSR：完整读写
* DS2：完整读写
* DS3：完整读写
* DeS：完整读写
* BB：完整读写
* NB：完整读写

### NVM
DeS 与 DS1 使用的导航网格格式。
扩展名：`.nvm`
* DS1：完整读写
* DeS：完整读写

### PARAM64
DS3 使用的通用配置文件。
扩展名：`.param`
* DS3：完整读写

### PMDCL
定义 DS3 地图中的静态贴花。
扩展名：`.pmdcl`
* DS3：完整读写

### TAE3
控制动画播放过程中各类事件的发生时机；此版本专用于 DS3。
扩展名：`.tae`
* DS3：完整读写

### TPF
系列通用的多 DDS 贴图容器。
扩展名：`.tpf`
* DS1：完整读写
* DSR：完整读写
* DS2：完整读写
* DS3：完整读写
* DeS：完整读写
* BB：完整读写
* NB：完整读写

## 特别感谢

感谢下列各位：或是我从你们制作的工具中学到了东西，或是你们在破解这些格式时帮过忙。若遗漏了你，欢迎在 Discord 上找我。
* albeartron
* Atvaark
* B3LYP
* HotPocketRemix
* katalash
* Lance
* Meowmaritus
* Nyxojaele
* Pav
* SeanP
* Wulf2k
