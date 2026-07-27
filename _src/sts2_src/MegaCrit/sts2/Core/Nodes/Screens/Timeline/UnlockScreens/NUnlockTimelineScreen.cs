// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Timeline.UnlockScreens.NUnlockTimelineScreen
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Timeline;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Timeline.UnlockScreens;

[ScriptPath("res://src/Core/Nodes/Screens/Timeline/UnlockScreens/NUnlockTimelineScreen.cs")]
public class NUnlockTimelineScreen : NUnlockScreen
{
  private static readonly string _scenePath = SceneHelper.GetScenePath("timeline_screen/unlock_timeline_screen");
  private List<EpochSlotData> _erasToUnlock;

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NUnlockTimelineScreen._scenePath);
    }
  }

  public static NUnlockTimelineScreen Create()
  {
    return PreloadManager.Cache.GetScene(NUnlockTimelineScreen._scenePath).Instantiate<NUnlockTimelineScreen>((PackedScene.GenEditState) 0L);
  }

  public override void _Ready()
  {
  }

  public void SetUnlocks(List<EpochSlotData> eras)
  {
    this._erasToUnlock = eras.OrderBy<EpochSlotData, int>((Func<EpochSlotData, int>) (a => a.EraPosition)).ToList<EpochSlotData>();
  }

  public override void Open()
  {
    base.Open();
    TaskHelper.RunSafely(this.AnimateExpansion());
  }

  private async Task AnimateExpansion()
  {
    await NTimelineScreen.Instance.HideBackstopAndShowUi(false);
    await NTimelineScreen.Instance.AddEpochSlots(this._erasToUnlock, true);
    NTimelineScreen.Instance.ShowHeaderAndActionsUi();
    NTimelineScreen.Instance.SetScreenDraggability();
    await this.Close();
    NTimelineScreen.Instance.EnableInput();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(3)
    {
      new MethodInfo(NUnlockTimelineScreen.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NUnlockTimelineScreen.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NUnlockTimelineScreen.MethodName.Open, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NUnlockTimelineScreen.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NUnlockTimelineScreen nunlockTimelineScreen = NUnlockTimelineScreen.Create();
      ret = VariantUtils.CreateFrom<NUnlockTimelineScreen>(ref nunlockTimelineScreen);
      return true;
    }
    if (StringName.op_Equality(ref method, NUnlockTimelineScreen.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NUnlockTimelineScreen.MethodName.Open) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.Open();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NUnlockTimelineScreen.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NUnlockTimelineScreen nunlockTimelineScreen = NUnlockTimelineScreen.Create();
      ret = VariantUtils.CreateFrom<NUnlockTimelineScreen>(ref nunlockTimelineScreen);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NUnlockTimelineScreen.MethodName.Create) || StringName.op_Equality(ref method, NUnlockTimelineScreen.MethodName._Ready) || StringName.op_Equality(ref method, NUnlockTimelineScreen.MethodName.Open) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
  }

  public new class MethodName : NUnlockScreen.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName Open = StringName.op_Implicit(nameof (Open));
  }

  public new class PropertyName : NUnlockScreen.PropertyName
  {
  }

  public new class SignalName : NUnlockScreen.SignalName
  {
  }
}
