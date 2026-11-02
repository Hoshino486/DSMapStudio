using System.Collections.Generic;

namespace StudioCore.Localization;

/// <summary>
///     Supplementary entries that never appear as a literal inside an ImGui call in the source
///     tree, so the extractor in <c>gen_dict.py</c> cannot see them: window titles that are
///     assembled at run time, the four editor tab names, and popup captions.
///     Kept in a separate hand-maintained table so regenerating <see cref="LocalizationDict" />
///     never drops them.
/// </summary>
public static class LocalizationExtra
{
    /// <summary>Window titles, popup captions and other run-time assembled labels.</summary>
    public static readonly Dictionary<string, string> Strings = new()
    {
        // ---------------------------------------------------------- editor tabs
        ["Map Editor"] = "地图编辑器",
        ["Model Editor"] = "模型编辑器",
        ["Param Editor"] = "参数编辑器",
        ["Text Editor"] = "文本编辑器",

        // ------------------------------------------------ floating / docked panes
        ["Viewport"] = "视口",
        ["Profiling"] = "性能分析",
        ["Properties"] = "属性",
        ["Map Object List"] = "地图对象列表",
        ["Model Hierarchy"] = "模型层级",
        ["Render Groups"] = "渲染组",
        ["Navmesh Build"] = "导航网格构建",
        ["Search Properties"] = "搜索属性",
        ["Resource List"] = "资源列表",
        ["Resource Loading Tasks"] = "资源加载任务",
        ["Text Categories"] = "文本分类",
        ["Text Entries"] = "文本条目",
        ["Text"] = "文本",
        ["Search Text"] = "搜索文本",
        ["Text Entries Home"] = "文本条目主页",
        ["Key Bind Settings"] = "按键绑定设置",
        ["Fanfic Writer"] = "同人写作器",
        ["GizmosDebug"] = "辅助线调试",

        // ------------------------------------------------------- asset browser
        ["Asset Browser"] = "资源浏览器",
        ["Asset Browser: Category"] = "资源浏览器：分类",
        ["Asset Browser: Contents"] = "资源浏览器：内容",
        ["Asset Browser: Actions"] = "资源浏览器：操作",

        // --------------------------------------------------------------- popups
        ["Settings Menu"] = "设置菜单",
        ["Logger"] = "日志",
        ["Help"] = "帮助",
        ["New Project"] = "新建项目",
        ["Viewport Controls"] = "视口控制",

        // ------------------------------------------------- miscellaneous windows
        ["Mass Edit"] = "批量编辑",
        ["Mass Edit Regex"] = "批量编辑正则",
        ["Map List"] = "地图列表",
        ["Program Settings"] = "程序设置",
        ["Project Settings"] = "项目设置",
        ["About"] = "关于",
        ["Confirm"] = "确认",
        ["Unsaved Changes"] = "未保存的更改",

        // ----------------------------------------- program title and dialog buttons
        ["Dark Souls Map Studio version"] = "Dark Souls Map Studio 版本",
        ["Ok"] = "确定",
        ["Cancel"] = "取消",
        ["Yes"] = "是",
        ["No"] = "否",

        // ------------------------------------------ extractor blind spots / retranslations
        ["Gizmos"] = "辅助线",
        ["Tool"] = "工具",
        ["Asset Prefab"] = "资产预制体",
        ["Asset Prefabs"] = "资产预制体",
        ["Type View"] = "类型视图",
        ["Hierarchy View"] = "层级视图",
        ["Flat View"] = "平铺视图",
        ["Check Program Updates"] = "检查程序更新",

        // The visible half of an id-suffixed label ("Controls##ViewportControls"). Resolvable
        // through the "##" fallback too, but the direct entry keeps the common case cheap.
        ["Controls"] = "控制",

        // ------------------------------------------------------- task log messages
        ["Running passive task: {TaskId}"] = "正在运行后台任务：{TaskId}",
        ["Task Completed: {TaskId}"] = "任务完成：{TaskId}",
        ["Task Failed: {TaskId}"] = "任务失败：{TaskId}",
    };
}
