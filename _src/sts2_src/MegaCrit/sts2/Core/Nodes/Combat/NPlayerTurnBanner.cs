// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Combat.NPlayerTurnBanner
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Combat;

[ScriptPath("res://src/Core/Nodes/Combat/NPlayerTurnBanner.cs")]
public class NPlayerTurnBanner : Control
{
  private MegaLabel _label;
  private MegaLabel _turnLabel;
  private int _roundNumber;
  private static readonly string _scenePath = SceneHelper.GetScenePath("combat/player_turn_banner");

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NPlayerTurnBanner._scenePath);
    }
  }

  public static NPlayerTurnBanner? Create(int roundNumber)
  {
    if (TestMode.IsOn)
      return (NPlayerTurnBanner) null;
    if (NCombatUi.IsDebugHideTextVfx)
      return (NPlayerTurnBanner) null;
    NPlayerTurnBanner nplayerTurnBanner = PreloadManager.Cache.GetScene(NPlayerTurnBanner._scenePath).Instantiate<NPlayerTurnBanner>((PackedScene.GenEditState) 0L);
    nplayerTurnBanner._roundNumber = roundNumber;
    return nplayerTurnBanner;
  }

  public override void _Ready()
  {
    this._label = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("Label"));
    if (CombatManager.Instance.PlayersTakingExtraTurn.Count > 0)
      this._label.SetTextAutoSize(new LocString("gameplay_ui", "PLAYER_TURN_EXTRA").GetFormattedText());
    else
      this._label.SetTextAutoSize(new LocString("gameplay_ui", "PLAYER_TURN").GetFormattedText());
    this._turnLabel = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("TurnNumber"));
    LocString locString = new LocString("gameplay_ui", "TURN_COUNT");
    locString.Add("turnNumber", (Decimal) this._roundNumber);
    this._turnLabel.SetTextAutoSize(locString.GetFormattedText());
    ((CanvasItem) this).Modulate = Colors.Transparent;
    TaskHelper.RunSafely(this.Display());
  }

  private async Task Display()
  {
    NDebugAudioManager.Instance?.Play("player_turn.mp3");
    Tween tween1 = ((Node) this).CreateTween();
    tween1.SetParallel(true);
    tween1.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 1.0).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    tween1.TweenProperty((GodotObject) this._label, NodePath.op_Implicit("position"), Variant.op_Implicit(Vector2.op_Addition(((Control) this._label).Position, new Vector2(0.0f, -50f))), 1.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    tween1.TweenProperty((GodotObject) this._turnLabel, NodePath.op_Implicit("position"), Variant.op_Implicit(Vector2.op_Addition(((Control) this._turnLabel).Position, new Vector2(0.0f, 50f))), 1.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    if (!await tween1.AwaitFinished((Node) this))
      return;
    Tween tween2 = ((Node) this).CreateTween();
    tween2.TweenInterval(0.4);
    tween2.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.30000001192092896).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 1L);
    bool flag = await tween2.AwaitFinished((Node) this);
    ((Node) this).QueueFreeSafely();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NPlayerTurnBanner.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("roundNumber"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NPlayerTurnBanner.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NPlayerTurnBanner.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NPlayerTurnBanner nplayerTurnBanner = NPlayerTurnBanner.Create(VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NPlayerTurnBanner>(ref nplayerTurnBanner);
      return true;
    }
    if (!StringName.op_Equality(ref method, NPlayerTurnBanner.MethodName._Ready) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ((Node) this)._Ready();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NPlayerTurnBanner.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NPlayerTurnBanner nplayerTurnBanner = NPlayerTurnBanner.Create(VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NPlayerTurnBanner>(ref nplayerTurnBanner);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NPlayerTurnBanner.MethodName.Create) || StringName.op_Equality(ref method, NPlayerTurnBanner.MethodName._Ready) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NPlayerTurnBanner.PropertyName._label))
    {
      this._label = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPlayerTurnBanner.PropertyName._turnLabel))
    {
      this._turnLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NPlayerTurnBanner.PropertyName._roundNumber))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._roundNumber = VariantUtils.ConvertTo<int>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NPlayerTurnBanner.PropertyName._label))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._label);
      return true;
    }
    if (StringName.op_Equality(ref name, NPlayerTurnBanner.PropertyName._turnLabel))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._turnLabel);
      return true;
    }
    if (!StringName.op_Equality(ref name, NPlayerTurnBanner.PropertyName._roundNumber))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<int>(ref this._roundNumber);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NPlayerTurnBanner.PropertyName._label, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPlayerTurnBanner.PropertyName._turnLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NPlayerTurnBanner.PropertyName._roundNumber, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NPlayerTurnBanner.PropertyName._label, Variant.From<MegaLabel>(ref this._label));
    info.AddProperty(NPlayerTurnBanner.PropertyName._turnLabel, Variant.From<MegaLabel>(ref this._turnLabel));
    info.AddProperty(NPlayerTurnBanner.PropertyName._roundNumber, Variant.From<int>(ref this._roundNumber));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NPlayerTurnBanner.PropertyName._label, ref variant1))
      this._label = ((Variant) ref variant1).As<MegaLabel>();
    Variant variant2;
    if (info.TryGetProperty(NPlayerTurnBanner.PropertyName._turnLabel, ref variant2))
      this._turnLabel = ((Variant) ref variant2).As<MegaLabel>();
    Variant variant3;
    if (!info.TryGetProperty(NPlayerTurnBanner.PropertyName._roundNumber, ref variant3))
      return;
    this._roundNumber = ((Variant) ref variant3).As<int>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName _label = StringName.op_Implicit(nameof (_label));
    public static readonly StringName _turnLabel = StringName.op_Implicit(nameof (_turnLabel));
    public static readonly StringName _roundNumber = StringName.op_Implicit(nameof (_roundNumber));
  }

  public class SignalName : Control.SignalName
  {
  }
}
