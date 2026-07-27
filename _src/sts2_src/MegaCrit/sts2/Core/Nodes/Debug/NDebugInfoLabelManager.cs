// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Debug.NDebugInfoLabelManager
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Debug;

[ScriptPath("res://src/Core/Nodes/Debug/NDebugInfoLabelManager.cs")]
public class NDebugInfoLabelManager : Node
{
  [Export]
  public bool isMainMenu;
  private MegaLabel _releaseInfo;
  private MegaLabel _moddedWarning;
  private MegaLabel? _seed;
  private Control? _modWarningContainer;
  private MegaRichTextLabel? _modWarningLabel;

  public override void _Ready()
  {
    this._releaseInfo = this.GetNode<MegaLabel>(NodePath.op_Implicit("%ReleaseInfo"));
    this._moddedWarning = this.GetNode<MegaLabel>(NodePath.op_Implicit("%ModdedWarning"));
    this._seed = this.GetNodeOrNull<MegaLabel>(NodePath.op_Implicit("%DebugSeed"));
    this._modWarningContainer = this.GetNodeOrNull<Control>(NodePath.op_Implicit("%ModWarningContainer"));
    this._modWarningLabel = this.GetNodeOrNull<MegaRichTextLabel>(NodePath.op_Implicit("%ModWarningLabel"));
    ((GodotObject) this._moddedWarning).Connect(Control.SignalName.MouseEntered, Callable.From(new Action(this.OnModdedWarningHovered)), 0U);
    ((GodotObject) this._moddedWarning).Connect(Control.SignalName.MouseExited, Callable.From(new Action(this.OnModdedWarningUnhovered)), 0U);
    ((CanvasItem) this._modWarningContainer)?.SetVisible(false);
    this.UpdateText((string) null);
    if (ReleaseInfoManager.Instance.ReleaseInfo != null)
      return;
    TaskHelper.RunSafely(this.SetCommitIdInEditor());
  }

  private async Task SetCommitIdInEditor()
  {
    if (GitHelper.ShortCommitIdTask == null)
      return;
    this.UpdateText(await GitHelper.ShortCommitIdTask);
  }

  private void UpdateText(string? commitId)
  {
    ReleaseInfo releaseInfo = ReleaseInfoManager.Instance.ReleaseInfo;
    string str1 = releaseInfo?.Date.ToString("yyyy.MM.dd") ?? "???";
    string str2 = releaseInfo?.Version ?? commitId ?? "NONE";
    if (this.isMainMenu)
      this._releaseInfo.Text = $"{str2}\n{str1}";
    else
      this._releaseInfo.Text = $"[{str2}] ({str1})";
    ((CanvasItem) this._moddedWarning).Visible = ModManager.IsRunningModded();
    if (!((CanvasItem) this._moddedWarning).Visible)
      return;
    bool variable = ModManager.Mods.Any<Mod>((Func<Mod, bool>) (m =>
    {
      if (m.state == ModLoadState.Failed)
        return true;
      List<LocString> errors = m.errors;
      // ISSUE: explicit non-virtual call
      return errors != null && __nonvirtual (errors.Count) > 0;
    }));
    if (this.isMainMenu)
    {
      LocString locString1 = new LocString("main_menu_ui", "MODDED_WARNING");
      locString1.Add("count", (Decimal) ModManager.GetLoadedMods().Count<Mod>());
      locString1.Add("hasError", variable);
      this._moddedWarning.SetTextAutoSize(locString1.GetFormattedText());
      LocString[] array = ModManager.Mods.SelectMany<Mod, LocString>((Func<Mod, IEnumerable<LocString>>) (m => (IEnumerable<LocString>) m.errors ?? (IEnumerable<LocString>) new List<LocString>())).ToArray<LocString>();
      if (array.Length != 0)
      {
        this._modWarningLabel.Text = string.Join("\n", ((IEnumerable<LocString>) array).Select<LocString, string>((Func<LocString, string>) (s => s.GetFormattedText())));
      }
      else
      {
        LocString locString2 = new LocString("main_menu_ui", "MOD_ERROR.NONE");
        locString2.Add("mods", string.Join(", ", ModManager.GetLoadedMods().Select<Mod, string>((Func<Mod, string>) (m =>
        {
          string name = m.manifest?.name;
          if (name != null)
            return name;
          return m.manifest?.id;
        }))));
        this._modWarningLabel.Text = locString2.GetFormattedText();
      }
    }
    else
      this._moddedWarning.SetTextAutoSize($"MODDED ({ModManager.GetLoadedMods().Count<Mod>()})");
    if (!variable)
      return;
    ((CanvasItem) this._moddedWarning).Modulate = StsColors.redGlow;
  }

  private void OnModdedWarningHovered()
  {
    if (this._modWarningContainer == null)
      return;
    ((CanvasItem) this._modWarningContainer).Visible = true;
  }

  private void OnModdedWarningUnhovered()
  {
    if (this._modWarningContainer == null)
      return;
    ((CanvasItem) this._modWarningContainer).Visible = false;
  }

  public override void _Input(InputEvent inputEvent)
  {
    if (!inputEvent.IsActionReleased(DebugHotkey.hideVersionInfo, false))
      return;
    ((CanvasItem) this._releaseInfo).Visible = !((CanvasItem) this._releaseInfo).Visible;
    ((CanvasItem) this._moddedWarning).Visible = ModManager.IsRunningModded() && !((CanvasItem) this._moddedWarning).Visible;
    ((CanvasItem) this._seed)?.SetVisible(!((CanvasItem) this._seed).Visible);
    ((Node) NGame.Instance).AddChildSafely((Node) NFullscreenTextVfx.Create(((CanvasItem) this._releaseInfo).Visible ? "Show Version Info" : "Hide Version Info"));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(5)
    {
      new MethodInfo(NDebugInfoLabelManager.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDebugInfoLabelManager.MethodName.UpdateText, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("commitId"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NDebugInfoLabelManager.MethodName.OnModdedWarningHovered, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDebugInfoLabelManager.MethodName.OnModdedWarningUnhovered, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDebugInfoLabelManager.MethodName._Input, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NDebugInfoLabelManager.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      base._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDebugInfoLabelManager.MethodName.UpdateText) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.UpdateText(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDebugInfoLabelManager.MethodName.OnModdedWarningHovered) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnModdedWarningHovered();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDebugInfoLabelManager.MethodName.OnModdedWarningUnhovered) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnModdedWarningUnhovered();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NDebugInfoLabelManager.MethodName._Input) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    base._Input(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NDebugInfoLabelManager.MethodName._Ready) || StringName.op_Equality(ref method, NDebugInfoLabelManager.MethodName.UpdateText) || StringName.op_Equality(ref method, NDebugInfoLabelManager.MethodName.OnModdedWarningHovered) || StringName.op_Equality(ref method, NDebugInfoLabelManager.MethodName.OnModdedWarningUnhovered) || StringName.op_Equality(ref method, NDebugInfoLabelManager.MethodName._Input) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NDebugInfoLabelManager.PropertyName.isMainMenu))
    {
      this.isMainMenu = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDebugInfoLabelManager.PropertyName._releaseInfo))
    {
      this._releaseInfo = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDebugInfoLabelManager.PropertyName._moddedWarning))
    {
      this._moddedWarning = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDebugInfoLabelManager.PropertyName._seed))
    {
      this._seed = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDebugInfoLabelManager.PropertyName._modWarningContainer))
    {
      this._modWarningContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NDebugInfoLabelManager.PropertyName._modWarningLabel))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._modWarningLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NDebugInfoLabelManager.PropertyName.isMainMenu))
    {
      value = VariantUtils.CreateFrom<bool>(ref this.isMainMenu);
      return true;
    }
    if (StringName.op_Equality(ref name, NDebugInfoLabelManager.PropertyName._releaseInfo))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._releaseInfo);
      return true;
    }
    if (StringName.op_Equality(ref name, NDebugInfoLabelManager.PropertyName._moddedWarning))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._moddedWarning);
      return true;
    }
    if (StringName.op_Equality(ref name, NDebugInfoLabelManager.PropertyName._seed))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._seed);
      return true;
    }
    if (StringName.op_Equality(ref name, NDebugInfoLabelManager.PropertyName._modWarningContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._modWarningContainer);
      return true;
    }
    if (!StringName.op_Equality(ref name, NDebugInfoLabelManager.PropertyName._modWarningLabel))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._modWarningLabel);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 1L, NDebugInfoLabelManager.PropertyName.isMainMenu, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NDebugInfoLabelManager.PropertyName._releaseInfo, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDebugInfoLabelManager.PropertyName._moddedWarning, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDebugInfoLabelManager.PropertyName._seed, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDebugInfoLabelManager.PropertyName._modWarningContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDebugInfoLabelManager.PropertyName._modWarningLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NDebugInfoLabelManager.PropertyName.isMainMenu, Variant.From<bool>(ref this.isMainMenu));
    info.AddProperty(NDebugInfoLabelManager.PropertyName._releaseInfo, Variant.From<MegaLabel>(ref this._releaseInfo));
    info.AddProperty(NDebugInfoLabelManager.PropertyName._moddedWarning, Variant.From<MegaLabel>(ref this._moddedWarning));
    info.AddProperty(NDebugInfoLabelManager.PropertyName._seed, Variant.From<MegaLabel>(ref this._seed));
    info.AddProperty(NDebugInfoLabelManager.PropertyName._modWarningContainer, Variant.From<Control>(ref this._modWarningContainer));
    info.AddProperty(NDebugInfoLabelManager.PropertyName._modWarningLabel, Variant.From<MegaRichTextLabel>(ref this._modWarningLabel));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NDebugInfoLabelManager.PropertyName.isMainMenu, ref variant1))
      this.isMainMenu = ((Variant) ref variant1).As<bool>();
    Variant variant2;
    if (info.TryGetProperty(NDebugInfoLabelManager.PropertyName._releaseInfo, ref variant2))
      this._releaseInfo = ((Variant) ref variant2).As<MegaLabel>();
    Variant variant3;
    if (info.TryGetProperty(NDebugInfoLabelManager.PropertyName._moddedWarning, ref variant3))
      this._moddedWarning = ((Variant) ref variant3).As<MegaLabel>();
    Variant variant4;
    if (info.TryGetProperty(NDebugInfoLabelManager.PropertyName._seed, ref variant4))
      this._seed = ((Variant) ref variant4).As<MegaLabel>();
    Variant variant5;
    if (info.TryGetProperty(NDebugInfoLabelManager.PropertyName._modWarningContainer, ref variant5))
      this._modWarningContainer = ((Variant) ref variant5).As<Control>();
    Variant variant6;
    if (!info.TryGetProperty(NDebugInfoLabelManager.PropertyName._modWarningLabel, ref variant6))
      return;
    this._modWarningLabel = ((Variant) ref variant6).As<MegaRichTextLabel>();
  }

  public class MethodName : Node.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName UpdateText = StringName.op_Implicit(nameof (UpdateText));
    public static readonly StringName OnModdedWarningHovered = StringName.op_Implicit(nameof (OnModdedWarningHovered));
    public static readonly StringName OnModdedWarningUnhovered = StringName.op_Implicit(nameof (OnModdedWarningUnhovered));
    public static readonly StringName _Input = StringName.op_Implicit(nameof (_Input));
  }

  public class PropertyName : Node.PropertyName
  {
    public static readonly StringName isMainMenu = StringName.op_Implicit(nameof (isMainMenu));
    public static readonly StringName _releaseInfo = StringName.op_Implicit(nameof (_releaseInfo));
    public static readonly StringName _moddedWarning = StringName.op_Implicit(nameof (_moddedWarning));
    public static readonly StringName _seed = StringName.op_Implicit(nameof (_seed));
    public static readonly StringName _modWarningContainer = StringName.op_Implicit(nameof (_modWarningContainer));
    public static readonly StringName _modWarningLabel = StringName.op_Implicit(nameof (_modWarningLabel));
  }

  public class SignalName : Node.SignalName
  {
  }
}
