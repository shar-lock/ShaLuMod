// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Timeline.UnlockScreens.NUnlockEpochScreen
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Timeline;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Timeline.UnlockScreens;

[ScriptPath("res://src/Core/Nodes/Screens/Timeline/UnlockScreens/NUnlockEpochScreen.cs")]
public class NUnlockEpochScreen : NUnlockScreen
{
  private IReadOnlyList<EpochModel> _unlockedEpochs;
  private Tween? _cardFlyTween;
  private const double _initDelay = 0.3;
  private RichTextLabel _infoLabel;

  public override void _Ready()
  {
    this.ConnectSignals();
    this._infoLabel = ((Node) this).GetNode<RichTextLabel>(NodePath.op_Implicit("%InfoLabel"));
    this._infoLabel.Text = $"[center]{new LocString("timeline", "UNLOCK_EPOCHS").GetFormattedText()}[/center]";
    ((CanvasItem) this._infoLabel).Modulate = StsColors.transparentWhite;
  }

  public override void Open()
  {
    base.Open();
    this._cardFlyTween = ((Node) this).CreateTween().SetParallel(true);
    double num = 0.3;
    Vector2 position = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%Center")).Position;
    PackedScene scene = PreloadManager.Cache.GetScene("res://scenes/timeline_screen/epoch.tscn");
    if (this._unlockedEpochs.Count == 3)
    {
      for (int index = 0; index < this._unlockedEpochs.Count; ++index)
      {
        NEpochCard child = scene.Instantiate<NEpochCard>((PackedScene.GenEditState) 0L);
        child.Init(this._unlockedEpochs[index]);
        Control node = ((Node) this).GetNode<Control>(NodePath.op_Implicit($"Slot{index}"));
        ((Node) node).AddChildSafely((Node) child);
        child.SetToWigglyUnlockPreviewMode();
        this._cardFlyTween.TweenProperty((GodotObject) node, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.White), 1.0).SetDelay(num - 0.3);
        this._cardFlyTween.TweenProperty((GodotObject) node, NodePath.op_Implicit("position"), Variant.op_Implicit(node.Position), 1.0).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 10L).SetDelay(num);
        ((CanvasItem) node).Modulate = StsColors.transparentBlack;
        node.Position = position;
        num += 0.25;
      }
    }
    else if (this._unlockedEpochs.Count == 2)
    {
      for (int index = 0; index < this._unlockedEpochs.Count; ++index)
      {
        NEpochCard child = scene.Instantiate<NEpochCard>((PackedScene.GenEditState) 0L);
        child.Init(this._unlockedEpochs[index]);
        Control node = ((Node) this).GetNode<Control>(NodePath.op_Implicit($"Slot{3 + index}"));
        ((Node) node).AddChildSafely((Node) child);
        child.SetToWigglyUnlockPreviewMode();
        this._cardFlyTween.TweenProperty((GodotObject) node, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.White), 1.0).SetDelay(num - 0.3);
        this._cardFlyTween.TweenProperty((GodotObject) node, NodePath.op_Implicit("position"), Variant.op_Implicit(node.Position), 1.0).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 10L).SetDelay(num);
        ((CanvasItem) node).Modulate = StsColors.transparentBlack;
        node.Position = position;
        num += 0.33;
      }
    }
    else
      Log.Error("Unlocking exactly 1 OR more than 3 Epochs are not supported.");
    this._cardFlyTween.TweenProperty((GodotObject) this._infoLabel, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.White), 1.0).SetDelay(0.25);
  }

  public void SetUnlocks(IReadOnlyList<EpochModel> epochs) => this._unlockedEpochs = epochs;

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NUnlockEpochScreen.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NUnlockEpochScreen.MethodName.Open, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NUnlockEpochScreen.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NUnlockEpochScreen.MethodName.Open) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.Open();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NUnlockEpochScreen.MethodName._Ready) || StringName.op_Equality(ref method, NUnlockEpochScreen.MethodName.Open) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NUnlockEpochScreen.PropertyName._cardFlyTween))
    {
      this._cardFlyTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NUnlockEpochScreen.PropertyName._infoLabel))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._infoLabel = VariantUtils.ConvertTo<RichTextLabel>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NUnlockEpochScreen.PropertyName._cardFlyTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._cardFlyTween);
      return true;
    }
    if (!StringName.op_Equality(ref name, NUnlockEpochScreen.PropertyName._infoLabel))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<RichTextLabel>(ref this._infoLabel);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NUnlockEpochScreen.PropertyName._cardFlyTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NUnlockEpochScreen.PropertyName._infoLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NUnlockEpochScreen.PropertyName._cardFlyTween, Variant.From<Tween>(ref this._cardFlyTween));
    info.AddProperty(NUnlockEpochScreen.PropertyName._infoLabel, Variant.From<RichTextLabel>(ref this._infoLabel));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NUnlockEpochScreen.PropertyName._cardFlyTween, ref variant1))
      this._cardFlyTween = ((Variant) ref variant1).As<Tween>();
    Variant variant2;
    if (!info.TryGetProperty(NUnlockEpochScreen.PropertyName._infoLabel, ref variant2))
      return;
    this._infoLabel = ((Variant) ref variant2).As<RichTextLabel>();
  }

  public new class MethodName : NUnlockScreen.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName Open = StringName.op_Implicit(nameof (Open));
  }

  public new class PropertyName : NUnlockScreen.PropertyName
  {
    public static readonly StringName _cardFlyTween = StringName.op_Implicit(nameof (_cardFlyTween));
    public static readonly StringName _infoLabel = StringName.op_Implicit(nameof (_infoLabel));
  }

  public new class SignalName : NUnlockScreen.SignalName
  {
  }
}
