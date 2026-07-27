// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Rewards.NRewardButton
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Screens;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Rewards;

[ScriptPath("res://src/Core/Nodes/Rewards/NRewardButton.cs")]
public class NRewardButton : NButton
{
  private static readonly StringName _v = new StringName("v");
  private static readonly StringName _fontOutlineColor = new StringName("theme_override_colors/font_outline_color");
  private static readonly StringName _defaultColor = new StringName("theme_override_colors/default_color");
  private TextureRect _background;
  private Control _iconContainer;
  private MegaRichTextLabel _label;
  private NSelectionReticle _reticle;
  private ShaderMaterial _hsv;
  private Tween? _currentTween;
  private Variant _hsvDefault = Variant.op_Implicit(0.9);
  private Variant _hsvHover = Variant.op_Implicit(1.1);
  private Variant _hsvDown = Variant.op_Implicit(0.7);
  private 
  #nullable disable
  NRewardButton.RewardClaimedEventHandler backing_RewardClaimed;
  private NRewardButton.RewardSkippedEventHandler backing_RewardSkipped;

  public 
  #nullable enable
  Reward? Reward { get; private set; }

  private static string ScenePath => "res://scenes/rewards/reward_button.tscn";

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NRewardButton.ScenePath);
    }
  }

  public static NRewardButton Create(Reward reward, NRewardsScreen screen)
  {
    NRewardButton nrewardButton = PreloadManager.Cache.GetScene(NRewardButton.ScenePath).Instantiate<NRewardButton>((PackedScene.GenEditState) 0L);
    nrewardButton.SetReward(reward);
    return nrewardButton;
  }

  public override void _Ready()
  {
    this.ConnectSignals();
    this._background = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%Background"));
    this._iconContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%Icon"));
    this._label = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%Label"));
    this._reticle = ((Node) this).GetNode<NSelectionReticle>(NodePath.op_Implicit("%SelectionReticle"));
    this._hsv = (ShaderMaterial) ((CanvasItem) this._background).Material;
    this.Reload();
  }

  private void SetReward(Reward reward)
  {
    this.Reward = !(reward is LinkedRewardSet) ? reward : throw new ArgumentException("You aren't allowed to apply a RewardChainSet to a NRewardButton");
    if (!((Node) this).IsNodeReady())
      return;
    this.Reload();
  }

  private void Reload()
  {
    if (!((Node) this).IsNodeReady() || this.Reward == null)
      return;
    Control icon = this.Reward.CreateIcon();
    ((Node) this._iconContainer).AddChildSafely((Node) icon);
    icon.Position = this.Reward.IconPosition;
    if (this.Reward is PotionReward)
      icon.Scale = Vector2.op_Multiply(0.8f, Vector2.One);
    this._label.Text = this.Reward.Description.GetFormattedText();
  }

  private async Task GetReward()
  {
    this.Disable();
    if (await RunManager.Instance.RewardsSetSynchronizer.SelectLocalReward(this.Reward))
    {
      if (TestMode.IsOff)
      {
        NGlobalUi globalUi = NRun.Instance.GlobalUi;
        switch (this.Reward)
        {
          case RelicReward relicReward:
            if (relicReward.ClaimedRelic != null)
            {
              globalUi.RelicInventory.AnimateRelic(relicReward.ClaimedRelic, new Vector2?(this._iconContainer.GlobalPosition));
              break;
            }
            break;
          case PotionReward potionReward:
            globalUi.TopBar.PotionContainer.AnimatePotion(potionReward.ClaimedPotion, new Vector2?(this._iconContainer.GlobalPosition));
            break;
        }
      }
      this._isEnabled = false;
      ((GodotObject) this).EmitSignal(NRewardButton.SignalName.RewardClaimed, new Variant[1]
      {
        Variant.op_Implicit((GodotObject) this)
      });
    }
    else
    {
      this.Enable();
      this.TryGrabFocus();
      ((GodotObject) this).EmitSignal(NRewardButton.SignalName.RewardSkipped, new Variant[1]
      {
        Variant.op_Implicit((GodotObject) this)
      });
    }
  }

  protected override void OnRelease()
  {
    base.OnRelease();
    this.OnUnfocus();
    TaskHelper.RunSafely(this.GetReward());
  }

  protected override void OnPress()
  {
    base.OnPress();
    this._currentTween?.Kill();
    this._currentTween = ((Node) this).CreateTween().SetParallel(true);
    this._currentTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderParam)), this._hsvHover, this._hsvDown, 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._currentTween.TweenProperty((GodotObject) this._label, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    NHoverTipSet.Remove((Control) this);
  }

  protected override void OnFocus()
  {
    base.OnFocus();
    ((GodotObject) this._label).Set(NRewardButton._defaultColor, Variant.op_Implicit(StsColors.gold));
    ((GodotObject) this._label).Set(NRewardButton._fontOutlineColor, Variant.op_Implicit(StsColors.rewardLabelGoldOutline));
    this._currentTween?.Kill();
    this._hsv.SetShaderParameter(NRewardButton._v, this._hsvHover);
    NHoverTipSet andShow = NHoverTipSet.CreateAndShow((Control) this, this.Reward.HoverTips);
    andShow?.SetGlobalPosition(Vector2.op_Addition(this.GlobalPosition, Vector2.op_Multiply(Vector2.Left, 45f)), false);
    andShow?.SetAlignment((Control) this, HoverTipAlignment.Left);
    this._reticle.OnSelect();
  }

  protected override void OnUnfocus()
  {
    base.OnUnfocus();
    ((GodotObject) this._label).Set(NRewardButton._defaultColor, Variant.op_Implicit(StsColors.cream));
    ((GodotObject) this._label).Set(NRewardButton._fontOutlineColor, Variant.op_Implicit(StsColors.rewardLabelOutline));
    this._currentTween?.Kill();
    this._currentTween = ((Node) this).CreateTween();
    this._currentTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderParam)), this._hsv.GetShaderParameter(NRewardButton._v), this._hsvDefault, 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    NHoverTipSet.Remove((Control) this);
    this._reticle.OnDeselect();
  }

  private void UpdateShaderParam(float value)
  {
    this._hsv.SetShaderParameter(NRewardButton._v, Variant.op_Implicit(value));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(7)
    {
      new MethodInfo(NRewardButton.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRewardButton.MethodName.Reload, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRewardButton.MethodName.OnRelease, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRewardButton.MethodName.OnPress, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRewardButton.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRewardButton.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRewardButton.MethodName.UpdateShaderParam, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("value"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NRewardButton.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRewardButton.MethodName.Reload) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Reload();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRewardButton.MethodName.OnRelease) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnRelease();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRewardButton.MethodName.OnPress) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnPress();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRewardButton.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRewardButton.MethodName.OnUnfocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnfocus();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NRewardButton.MethodName.UpdateShaderParam) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.UpdateShaderParam(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NRewardButton.MethodName._Ready) || StringName.op_Equality(ref method, NRewardButton.MethodName.Reload) || StringName.op_Equality(ref method, NRewardButton.MethodName.OnRelease) || StringName.op_Equality(ref method, NRewardButton.MethodName.OnPress) || StringName.op_Equality(ref method, NRewardButton.MethodName.OnFocus) || StringName.op_Equality(ref method, NRewardButton.MethodName.OnUnfocus) || StringName.op_Equality(ref method, NRewardButton.MethodName.UpdateShaderParam) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NRewardButton.PropertyName._background))
    {
      this._background = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRewardButton.PropertyName._iconContainer))
    {
      this._iconContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRewardButton.PropertyName._label))
    {
      this._label = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRewardButton.PropertyName._reticle))
    {
      this._reticle = VariantUtils.ConvertTo<NSelectionReticle>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRewardButton.PropertyName._hsv))
    {
      this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRewardButton.PropertyName._currentTween))
    {
      this._currentTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRewardButton.PropertyName._hsvDefault))
    {
      this._hsvDefault = VariantUtils.ConvertTo<Variant>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRewardButton.PropertyName._hsvHover))
    {
      this._hsvHover = VariantUtils.ConvertTo<Variant>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NRewardButton.PropertyName._hsvDown))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._hsvDown = VariantUtils.ConvertTo<Variant>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NRewardButton.PropertyName._background))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._background);
      return true;
    }
    if (StringName.op_Equality(ref name, NRewardButton.PropertyName._iconContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._iconContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NRewardButton.PropertyName._label))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._label);
      return true;
    }
    if (StringName.op_Equality(ref name, NRewardButton.PropertyName._reticle))
    {
      value = VariantUtils.CreateFrom<NSelectionReticle>(ref this._reticle);
      return true;
    }
    if (StringName.op_Equality(ref name, NRewardButton.PropertyName._hsv))
    {
      value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._hsv);
      return true;
    }
    if (StringName.op_Equality(ref name, NRewardButton.PropertyName._currentTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._currentTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NRewardButton.PropertyName._hsvDefault))
    {
      value = VariantUtils.CreateFrom<Variant>(ref this._hsvDefault);
      return true;
    }
    if (StringName.op_Equality(ref name, NRewardButton.PropertyName._hsvHover))
    {
      value = VariantUtils.CreateFrom<Variant>(ref this._hsvHover);
      return true;
    }
    if (!StringName.op_Equality(ref name, NRewardButton.PropertyName._hsvDown))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<Variant>(ref this._hsvDown);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NRewardButton.PropertyName._background, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRewardButton.PropertyName._iconContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRewardButton.PropertyName._label, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRewardButton.PropertyName._reticle, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRewardButton.PropertyName._hsv, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRewardButton.PropertyName._currentTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 0L, NRewardButton.PropertyName._hsvDefault, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 0L, NRewardButton.PropertyName._hsvHover, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 0L, NRewardButton.PropertyName._hsvDown, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NRewardButton.PropertyName._background, Variant.From<TextureRect>(ref this._background));
    info.AddProperty(NRewardButton.PropertyName._iconContainer, Variant.From<Control>(ref this._iconContainer));
    info.AddProperty(NRewardButton.PropertyName._label, Variant.From<MegaRichTextLabel>(ref this._label));
    info.AddProperty(NRewardButton.PropertyName._reticle, Variant.From<NSelectionReticle>(ref this._reticle));
    info.AddProperty(NRewardButton.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
    info.AddProperty(NRewardButton.PropertyName._currentTween, Variant.From<Tween>(ref this._currentTween));
    info.AddProperty(NRewardButton.PropertyName._hsvDefault, Variant.From<Variant>(ref this._hsvDefault));
    info.AddProperty(NRewardButton.PropertyName._hsvHover, Variant.From<Variant>(ref this._hsvHover));
    info.AddProperty(NRewardButton.PropertyName._hsvDown, Variant.From<Variant>(ref this._hsvDown));
    info.AddSignalEventDelegate(NRewardButton.SignalName.RewardClaimed, (Delegate) this.backing_RewardClaimed);
    info.AddSignalEventDelegate(NRewardButton.SignalName.RewardSkipped, (Delegate) this.backing_RewardSkipped);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NRewardButton.PropertyName._background, ref variant1))
      this._background = ((Variant) ref variant1).As<TextureRect>();
    Variant variant2;
    if (info.TryGetProperty(NRewardButton.PropertyName._iconContainer, ref variant2))
      this._iconContainer = ((Variant) ref variant2).As<Control>();
    Variant variant3;
    if (info.TryGetProperty(NRewardButton.PropertyName._label, ref variant3))
      this._label = ((Variant) ref variant3).As<MegaRichTextLabel>();
    Variant variant4;
    if (info.TryGetProperty(NRewardButton.PropertyName._reticle, ref variant4))
      this._reticle = ((Variant) ref variant4).As<NSelectionReticle>();
    Variant variant5;
    if (info.TryGetProperty(NRewardButton.PropertyName._hsv, ref variant5))
      this._hsv = ((Variant) ref variant5).As<ShaderMaterial>();
    Variant variant6;
    if (info.TryGetProperty(NRewardButton.PropertyName._currentTween, ref variant6))
      this._currentTween = ((Variant) ref variant6).As<Tween>();
    Variant variant7;
    if (info.TryGetProperty(NRewardButton.PropertyName._hsvDefault, ref variant7))
      this._hsvDefault = ((Variant) ref variant7).As<Variant>();
    Variant variant8;
    if (info.TryGetProperty(NRewardButton.PropertyName._hsvHover, ref variant8))
      this._hsvHover = ((Variant) ref variant8).As<Variant>();
    Variant variant9;
    if (info.TryGetProperty(NRewardButton.PropertyName._hsvDown, ref variant9))
      this._hsvDown = ((Variant) ref variant9).As<Variant>();
    NRewardButton.RewardClaimedEventHandler claimedEventHandler;
    if (info.TryGetSignalEventDelegate<NRewardButton.RewardClaimedEventHandler>(NRewardButton.SignalName.RewardClaimed, ref claimedEventHandler))
      this.backing_RewardClaimed = claimedEventHandler;
    NRewardButton.RewardSkippedEventHandler skippedEventHandler;
    if (!info.TryGetSignalEventDelegate<NRewardButton.RewardSkippedEventHandler>(NRewardButton.SignalName.RewardSkipped, ref skippedEventHandler))
      return;
    this.backing_RewardSkipped = skippedEventHandler;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<MethodInfo> GetGodotSignalList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NRewardButton.SignalName.RewardClaimed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("button"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NRewardButton.SignalName.RewardSkipped, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("button"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null)
    };
  }

  public event NRewardButton.RewardClaimedEventHandler RewardClaimed
  {
    add => this.backing_RewardClaimed += value;
    remove => this.backing_RewardClaimed -= value;
  }

  protected void EmitSignalRewardClaimed(NRewardButton button)
  {
    ((GodotObject) this).EmitSignal(NRewardButton.SignalName.RewardClaimed, new Variant[1]
    {
      Variant.op_Implicit((GodotObject) button)
    });
  }

  public event NRewardButton.RewardSkippedEventHandler RewardSkipped
  {
    add => this.backing_RewardSkipped += value;
    remove => this.backing_RewardSkipped -= value;
  }

  protected void EmitSignalRewardSkipped(NRewardButton button)
  {
    ((GodotObject) this).EmitSignal(NRewardButton.SignalName.RewardSkipped, new Variant[1]
    {
      Variant.op_Implicit((GodotObject) button)
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RaiseGodotClassSignalCallbacks(
    in godot_string_name signal,
    NativeVariantPtrArgs args)
  {
    if (StringName.op_Equality(ref signal, NRewardButton.SignalName.RewardClaimed) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NRewardButton.RewardClaimedEventHandler backingRewardClaimed = this.backing_RewardClaimed;
      if (backingRewardClaimed == null)
        return;
      backingRewardClaimed(VariantUtils.ConvertTo<NRewardButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
    }
    else if (StringName.op_Equality(ref signal, NRewardButton.SignalName.RewardSkipped) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NRewardButton.RewardSkippedEventHandler backingRewardSkipped = this.backing_RewardSkipped;
      if (backingRewardSkipped == null)
        return;
      backingRewardSkipped(VariantUtils.ConvertTo<NRewardButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
    }
    else
      base.RaiseGodotClassSignalCallbacks(in signal, args);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassSignal(in godot_string_name signal)
  {
    return StringName.op_Equality(ref signal, NRewardButton.SignalName.RewardClaimed) || StringName.op_Equality(ref signal, NRewardButton.SignalName.RewardSkipped) || base.HasGodotClassSignal(in signal);
  }

  [Signal]
  public delegate void RewardClaimedEventHandler(
  #nullable enable
  NRewardButton button);

  [Signal]
  public delegate void RewardSkippedEventHandler(NRewardButton button);

  public new class MethodName : NButton.MethodName
  {
    public new static readonly 
    #nullable disable
    StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName Reload = StringName.op_Implicit(nameof (Reload));
    public new static readonly StringName OnRelease = StringName.op_Implicit(nameof (OnRelease));
    public new static readonly StringName OnPress = StringName.op_Implicit(nameof (OnPress));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
    public static readonly StringName UpdateShaderParam = StringName.op_Implicit(nameof (UpdateShaderParam));
  }

  public new class PropertyName : NButton.PropertyName
  {
    public static readonly StringName _background = StringName.op_Implicit(nameof (_background));
    public static readonly StringName _iconContainer = StringName.op_Implicit(nameof (_iconContainer));
    public static readonly StringName _label = StringName.op_Implicit(nameof (_label));
    public static readonly StringName _reticle = StringName.op_Implicit(nameof (_reticle));
    public static readonly StringName _hsv = StringName.op_Implicit(nameof (_hsv));
    public static readonly StringName _currentTween = StringName.op_Implicit(nameof (_currentTween));
    public static readonly StringName _hsvDefault = StringName.op_Implicit(nameof (_hsvDefault));
    public static readonly StringName _hsvHover = StringName.op_Implicit(nameof (_hsvHover));
    public static readonly StringName _hsvDown = StringName.op_Implicit(nameof (_hsvDown));
  }

  public new class SignalName : NButton.SignalName
  {
    public static readonly StringName RewardClaimed = StringName.op_Implicit(nameof (RewardClaimed));
    public static readonly StringName RewardSkipped = StringName.op_Implicit(nameof (RewardSkipped));
  }
}
