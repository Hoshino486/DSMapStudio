# DSMapStudio

适用于《艾尔登法环》《只狼》《黑暗之魂》系列、《血源诅咒》与《恶魔之魂》（PS3）的模组工具套件

> 本文为简体中文版，英文原文见 [README.en.md](README.en.md)。

[![GitHub release](https://img.shields.io/github/release/soulsmods/DSMapStudio.svg)](https://github.com/soulsmods/DSMapStudio/releases/latest)
[![Github All Releases](https://img.shields.io/github/downloads/soulsmods/DSMapStudio/total.svg)](https://github.com/soulsmods/DSMapStudio/releases/latest)
[![Discord](https://img.shields.io/badge/Discord%20-%237289DA.svg?&logo=discord&logoColor=white)](https://discord.com/invite/CKDBCUFhB3)

## 关于 DS Map Studio
DS Map Studio 是一款面向近代 FromSoft 游戏的独立一体化模组工具，覆盖《恶魔之魂》（PS3）、《黑暗之魂》系列、《血源诅咒》《只狼》和《艾尔登法环》。目前包含地图编辑器、游戏 Param 编辑器，以及用于编辑游戏内文本的文本编辑器。

![msb-editor-screenshot](https://user-images.githubusercontent.com/44953920/209740902-ab75c7fb-e281-4833-aeab-4c2ea41da815.png)

## 系统需求
* Windows 7/8/8.1/10/11（仅 64 位）。Linux 环境在 wine 9+ 下或许可用，可能需要自行配置以访问 vulkan 1.3 并正确识别显卡。
* Visual C++ 运行库 x64 —— **程序启动即崩溃时请安装此项**（https://aka.ms/vs/16/release/vc_redist.x64.exe）
* DSMapStudio 已不再需要单独安装 .NET。
* 地图编辑器需要支持 Vulkan 1.3 的显卡与最新驱动：NVIDIA Maxwell（900 系列）及更新型号，或 AMD Polaris（Radeon 400 系列）及更新型号。Windows 7 的驱动不支持这些特性，此时只能改用低配置版 exe。
* 地图编辑器目前在 Intel 核显上无法正常工作，需要独立 NVIDIA 或 AMD 显卡。若只用 Param 编辑器与文本编辑器，可以使用低配置版 exe。
* 编辑 DS3/BB/只狼/ER/AC6 地图时建议 4GB 显存（推荐 8GB），这些地图体量极大。

## 基本使用说明
### 各游戏准备步骤
* **Dark Souls Prepare to die Edition**：使用 Map Studio 前必须先用 UDSFM 解包游戏（https://www.nexusmods.com/darksouls/mods/1304）。
* **Dark Souls Remastered**：游戏本身即为解包状态，无需其他工具。
* **Dark Souls 2 SOTFS**：使用 UXM（https://www.nexusmods.com/sekiro/mods/26）解包。原版 Dark Souls 2 不受支持。
* **Dark Souls 3 与只狼**：使用 UXM 提取游戏文件。
* **恶魔之魂**：若通过模拟器使用，务必关闭 RPCS3 的文件缓存，否则改动无法验证。
* **血源诅咒**：任何完整的游戏转储理论上开箱即用。注意部分转储将本体（1.0）与补丁分开存放，需先把补丁合并到本体之上再用于 Map Studio。向主机安装模组目前请自行处理。
* **只狼**：使用 UXM 提取游戏文件。
* **艾尔登法环**：使用 UXM 选择性解包（https://github.com/Nordgaren/UXM-Selective-Unpack）提取游戏文件。建议全量解包，但编辑器基本功能至少需要 `map`、`asset`、`chr`、`msg` 四个目录。
* **装甲核心 6**：使用 UXM 选择性解包（https://github.com/Nordgaren/UXM-Selective-Unpack）提取游戏文件。建议全量解包；在地图支持完成前，编辑器基本功能至少需要 `msg` 目录。

### 模组项目
Map Studio 以所谓「模组项目」为工作单位。项目通常存放在与本体分离的目录中，所有修改后的文件都写入该目录，不会覆盖本体文件。推荐流程是：为对应游戏安装 Mod Engine，并把 modengine.ini 里的 `modoverridedirectory` 指向你的模组项目目录。这样既不必改动本体文件，又能同时维护多个模组项目，分发时只需打包项目目录上传即可。

## 游戏支持限制
* **Dark Souls Remastered**：当前无法在视口中渲染地图碰撞。
* **只狼**：当前无法在视口中渲染地图碰撞与导航网格。
* **艾尔登法环**：当前无法在视口中渲染地图碰撞与导航网格。
* **装甲核心 6**：当前无法打开地图。

## 常见问题

### 问：DSMapStudio 能像 DSTools 那样把模型/地图导出为 FBX 吗？
答：目前不行。DSMapStudio 尚未实现任何资产的导入导出，资产管线计划在后续版本中补上。

### 问：为什么显卡要求这么高？以后还会支持 DX11 吗？
答：DS3、只狼、艾尔登法环的地图在保证帧率的前提下渲染开销极大，因此 DSMapStudio 围绕现代 Vulkan 与 GPU 驱动渲染等较新的 GPU 特性设计。这确实意味着需要一块支持较新 Vulkan 特性的现代显卡（NVIDIA Maxwell、AMD Polaris 或更新）。我们也清楚这对只想用 Param 编辑器的人并不友好，若需求足够，会考虑增加一个仅支持 Param 编辑器的降级渲染器。

### 问：会支持真正的自定义地图吗？
答：真正的自定义地图是长期目标，但达成之前还有不少障碍。DSMapStudio 目前没有导入自定义资产的资产管线，不过社区其他工具或许能产出可在 DSMapStudio 与游戏中使用的资产。

### 问：为什么放弃 DSTools？
答：DSTools 曾用于制作大量模组，至今仍有人在用。但把 Unity 的数据结构绑定到魂系结构上的做法越滚越乱、bug 不断，用户体验相当反直觉（例如多数用户无法直观判断 DSTools 究竟支持哪些 Unity 操作可以导出）。此外 Unity 并未对其光照烘焙、导航网格生成等子系统提供足够底层的 API，要让这些子系统服务于黑暗之魂，难度从「非常痛苦」到「根本做不到」不等。

不过最大的问题还是 Unity 过于笨重，导入资产时性能极差。所有黑暗之魂资产都必须先导入 Unity，既占空间，单张地图的导入又可能耗费数十分钟。这些因素促使我从零重写一个编辑器，目标是：A）高度专注魂系游戏，界面即为编辑这些游戏而设计；B）直接加载游戏资产、无中间转换产物，从而实现极快的加载速度。Map Studio 目前仍缺少 DSTools + Unity 支持的部分高级功能，但核心体验已明显更好——地图加载以秒计，而不是以分钟计。

## 致谢
* Katalash —— 项目负责人与原作者
* philiquaz —— 内置 Param 编辑器的主要维护者
* george —— 核心维护者与贡献者
* thefifthmatt —— SoapstoneLib 作者，实现跨工具协作功能

## 特别感谢
* TKGP —— 制作了 Soulsformats
* [Pav](https://github.com/JohrnaJohrna)
* [Meowmaritus](https://github.com/meowmaritus) —— 制作了 DSAnimStudio，DSMapStudio 在其基础上发展而来
* [PredatorCZ](https://github.com/PredatorCZ) —— 完整逆向出样条压缩动画
* [Horkrux](https://github.com/horkrux) —— 逆向出非 PC 平台贴图所用的文件头与 swizzling
* [Vawser](https://github.com/vawser) —— DS2/3 文档

## 使用的库
* Soulsformats
* [Newtonsoft Json.NET](https://www.newtonsoft.com/json)
* 基于 Veldrid 大幅改造的渲染后端
* Vortice.Vulkan —— Vulkan 绑定
* ImGui.NET —— 界面
* [HavokLib](https://github.com/PredatorCZ/HavokLib) 的一小部分，具体为样条压缩动画解压器，已移植到 C#
* Recast —— 导航网格生成
* Fork Awesome 字体 —— 图标
