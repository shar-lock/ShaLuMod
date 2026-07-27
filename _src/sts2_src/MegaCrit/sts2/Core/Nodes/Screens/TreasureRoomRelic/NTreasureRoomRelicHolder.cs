// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.TreasureRoomRelic.NTreasureRoomRelicHolder
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Relics;
using MegaCrit.Sts2.Core.Runs;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.TreasureRoomRelic;

[ScriptPath("res://src/Core/Nodes/Screens/TreasureRoomRelic/NTreasureRoomRelicHolder.cs")]
public class NTreasureRoomRelicHolder : NButton
{
  private GpuParticles2D _uncommonGlow;
  private GpuParticles2D _rareGlow;
  private bool _animatedIn;
  private Tween? _tween;
  private Tween? _initTween;

  public int Index { get; set; }

  public NMultiplayerVoteContainer VoteContainer { get; private set; }

  public NRelic Relic { get; private set; }

  public override void _Ready()
  {
    this.ConnectSignals();
    this.VoteContainer = ((Node) this).GetNode<NMultiplayerVoteContainer>(NodePath.op_Implicit("%MultiplayerVoteContainer"));
    this.Relic = ((Node) this).GetNode<NRelic>(NodePath.op_Implicit("%Relic"));
    this._uncommonGlow = ((Node) this).GetNode<GpuParticles2D>(NodePath.op_Implicit("%UncommonGlow"));
    this._rareGlow = ((Node) this).GetNode<GpuParticles2D>(NodePath.op_Implicit("%RareGlow"));
  }

  public void Initialize(RelicModel relic, IRunState runState)
  {
    this.Relic.Model = relic;
    this.VoteContainer.Initialize(new NMultiplayerVoteContainer.PlayerVotedDelegate(this.PlayerVotedForRelic), runState.Players);
    ((CanvasItem) this.Relic).Modulate = StsColors.transparentBlack;
    this._initTween?.Kill();
    this._initTween = ((Node) this).CreateTween().SetParallel(true);
    this._initTween.TweenProperty((GodotObject) this.Relic, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.White), 0.25);
    if (this.Relic.Model.Rarity == RelicRarity.Uncommon)
    {
      Tween tween = ((Node) this).CreateTween().SetParallel(true);
      ((CanvasItem) this._uncommonGlow).Visible = true;
      ((CanvasItem) this._uncommonGlow).Modulate = StsColors.transparentWhite;
      ((Node2D) this._uncommonGlow).GlobalPosition = Vector2.op_Addition(this.Relic.GlobalPosition, Vector2.op_Multiply(Vector2.One, 68f));
      tween.TweenProperty((GodotObject) this._uncommonGlow, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.White), 0.25);
    }
    else
    {
      if (this.Relic.Model.Rarity != RelicRarity.Rare)
        return;
      Tween tween = ((Node) this).CreateTween().SetParallel(true);
      ((CanvasItem) this._rareGlow).Visible = true;
      ((CanvasItem) this._rareGlow).Modulate = StsColors.transparentWhite;
      ((Node2D) this._rareGlow).GlobalPosition = Vector2.op_Addition(this.Relic.GlobalPosition, Vector2.op_Multiply(Vector2.One, 68f));
      tween.TweenProperty((GodotObject) this._rareGlow, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.White), 0.25);
    }
  }

  private bool PlayerVotedForRelic(Player player)
  {
    TreasureRoomRelicSynchronizer.PlayerVote playerVote = RunManager.Instance.TreasureRoomRelicSynchronizer.GetPlayerVote(player);
    if (!playerVote.voteReceived)
      return false;
    int? index1 = playerVote.index;
    int index2 = this.Index;
    return index1.GetValueOrDefault() == index2 & index1.HasValue;
  }

  public void AnimateAwayVotes()
  {
    ((Node) this).CreateTween().TweenProperty((GodotObject) this.VoteContainer, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.25);
  }

  protected override void OnFocus()
  {
    base.OnFocus();
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this.Relic, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 2.1f)), 0.05);
    NHoverTipSet.CreateAndShow((Control) this, this.Relic.Model.HoverTips)?.SetAlignmentForRelic(this.Relic);
  }

  protected override void OnUnfocus()
  {
    base.OnUnfocus();
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this.Relic, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 2f)), 0.4).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    NHoverTipSet.Remove((Control) this);
  }

  protected override void OnPress()
  {
    base.OnPress();
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this.Relic, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 1.9f)), 0.4).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
  }

  protected override void OnRelease()
  {
    base.OnRelease();
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this.Relic, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 2f)), 0.05).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(6)
    {
      new MethodInfo(NTreasureRoomRelicHolder.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTreasureRoomRelicHolder.MethodName.AnimateAwayVotes, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTreasureRoomRelicHolder.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTreasureRoomRelicHolder.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTreasureRoomRelicHolder.MethodName.OnPress, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTreasureRoomRelicHolder.MethodName.OnRelease, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NTreasureRoomRelicHolder.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTreasureRoomRelicHolder.MethodName.AnimateAwayVotes) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AnimateAwayVotes();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTreasureRoomRelicHolder.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTreasureRoomRelicHolder.MethodName.OnUnfocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnfocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTreasureRoomRelicHolder.MethodName.OnPress) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnPress();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NTreasureRoomRelicHolder.MethodName.OnRelease) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.OnRelease();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NTreasureRoomRelicHolder.MethodName._Ready) || StringName.op_Equality(ref method, NTreasureRoomRelicHolder.MethodName.AnimateAwayVotes) || StringName.op_Equality(ref method, NTreasureRoomRelicHolder.MethodName.OnFocus) || StringName.op_Equality(ref method, NTreasureRoomRelicHolder.MethodName.OnUnfocus) || StringName.op_Equality(ref method, NTreasureRoomRelicHolder.MethodName.OnPress) || StringName.op_Equality(ref method, NTreasureRoomRelicHolder.MethodName.OnRelease) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NTreasureRoomRelicHolder.PropertyName.Index))
    {
      this.Index = VariantUtils.ConvertTo<int>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTreasureRoomRelicHolder.PropertyName.VoteContainer))
    {
      this.VoteContainer = VariantUtils.ConvertTo<NMultiplayerVoteContainer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTreasureRoomRelicHolder.PropertyName.Relic))
    {
      this.Relic = VariantUtils.ConvertTo<NRelic>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTreasureRoomRelicHolder.PropertyName._uncommonGlow))
    {
      this._uncommonGlow = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTreasureRoomRelicHolder.PropertyName._rareGlow))
    {
      this._rareGlow = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTreasureRoomRelicHolder.PropertyName._animatedIn))
    {
      this._animatedIn = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTreasureRoomRelicHolder.PropertyName._tween))
    {
      this._tween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NTreasureRoomRelicHolder.PropertyName._initTween))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._initTween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NTreasureRoomRelicHolder.PropertyName.Index))
    {
      ref godot_variant local = ref value;
      int index = this.Index;
      godot_variant from = VariantUtils.CreateFrom<int>(ref index);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NTreasureRoomRelicHolder.PropertyName.VoteContainer))
    {
      ref godot_variant local = ref value;
      NMultiplayerVoteContainer voteContainer = this.VoteContainer;
      godot_variant from = VariantUtils.CreateFrom<NMultiplayerVoteContainer>(ref voteContainer);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NTreasureRoomRelicHolder.PropertyName.Relic))
    {
      ref godot_variant local = ref value;
      NRelic relic = this.Relic;
      godot_variant from = VariantUtils.CreateFrom<NRelic>(ref relic);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NTreasureRoomRelicHolder.PropertyName._uncommonGlow))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._uncommonGlow);
      return true;
    }
    if (StringName.op_Equality(ref name, NTreasureRoomRelicHolder.PropertyName._rareGlow))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._rareGlow);
      return true;
    }
    if (StringName.op_Equality(ref name, NTreasureRoomRelicHolder.PropertyName._animatedIn))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._animatedIn);
      return true;
    }
    if (StringName.op_Equality(ref name, NTreasureRoomRelicHolder.PropertyName._tween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._tween);
      return true;
    }
    if (!StringName.op_Equality(ref name, NTreasureRoomRelicHolder.PropertyName._initTween))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<Tween>(ref this._initTween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 2L, NTreasureRoomRelicHolder.PropertyName.Index, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTreasureRoomRelicHolder.PropertyName.VoteContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTreasureRoomRelicHolder.PropertyName.Relic, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTreasureRoomRelicHolder.PropertyName._uncommonGlow, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTreasureRoomRelicHolder.PropertyName._rareGlow, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NTreasureRoomRelicHolder.PropertyName._animatedIn, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTreasureRoomRelicHolder.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTreasureRoomRelicHolder.PropertyName._initTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo1 = info;
    StringName index1 = NTreasureRoomRelicHolder.PropertyName.Index;
    int index2 = this.Index;
    Variant variant1 = Variant.From<int>(ref index2);
    serializationInfo1.AddProperty(index1, variant1);
    GodotSerializationInfo serializationInfo2 = info;
    StringName voteContainer1 = NTreasureRoomRelicHolder.PropertyName.VoteContainer;
    NMultiplayerVoteContainer voteContainer2 = this.VoteContainer;
    Variant variant2 = Variant.From<NMultiplayerVoteContainer>(ref voteContainer2);
    serializationInfo2.AddProperty(voteContainer1, variant2);
    GodotSerializationInfo serializationInfo3 = info;
    StringName relic1 = NTreasureRoomRelicHolder.PropertyName.Relic;
    NRelic relic2 = this.Relic;
    Variant variant3 = Variant.From<NRelic>(ref relic2);
    serializationInfo3.AddProperty(relic1, variant3);
    info.AddProperty(NTreasureRoomRelicHolder.PropertyName._uncommonGlow, Variant.From<GpuParticles2D>(ref this._uncommonGlow));
    info.AddProperty(NTreasureRoomRelicHolder.PropertyName._rareGlow, Variant.From<GpuParticles2D>(ref this._rareGlow));
    info.AddProperty(NTreasureRoomRelicHolder.PropertyName._animatedIn, Variant.From<bool>(ref this._animatedIn));
    info.AddProperty(NTreasureRoomRelicHolder.PropertyName._tween, Variant.From<Tween>(ref this._tween));
    info.AddProperty(NTreasureRoomRelicHolder.PropertyName._initTween, Variant.From<Tween>(ref this._initTween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NTreasureRoomRelicHolder.PropertyName.Index, ref variant1))
      this.Index = ((Variant) ref variant1).As<int>();
    Variant variant2;
    if (info.TryGetProperty(NTreasureRoomRelicHolder.PropertyName.VoteContainer, ref variant2))
      this.VoteContainer = ((Variant) ref variant2).As<NMultiplayerVoteContainer>();
    Variant variant3;
    if (info.TryGetProperty(NTreasureRoomRelicHolder.PropertyName.Relic, ref variant3))
      this.Relic = ((Variant) ref variant3).As<NRelic>();
    Variant variant4;
    if (info.TryGetProperty(NTreasureRoomRelicHolder.PropertyName._uncommonGlow, ref variant4))
      this._uncommonGlow = ((Variant) ref variant4).As<GpuParticles2D>();
    Variant variant5;
    if (info.TryGetProperty(NTreasureRoomRelicHolder.PropertyName._rareGlow, ref variant5))
      this._rareGlow = ((Variant) ref variant5).As<GpuParticles2D>();
    Variant variant6;
    if (info.TryGetProperty(NTreasureRoomRelicHolder.PropertyName._animatedIn, ref variant6))
      this._animatedIn = ((Variant) ref variant6).As<bool>();
    Variant variant7;
    if (info.TryGetProperty(NTreasureRoomRelicHolder.PropertyName._tween, ref variant7))
      this._tween = ((Variant) ref variant7).As<Tween>();
    Variant variant8;
    if (!info.TryGetProperty(NTreasureRoomRelicHolder.PropertyName._initTween, ref variant8))
      return;
    this._initTween = ((Variant) ref variant8).As<Tween>();
  }

  public new class MethodName : NButton.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName AnimateAwayVotes = StringName.op_Implicit(nameof (AnimateAwayVotes));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
    public new static readonly StringName OnPress = StringName.op_Implicit(nameof (OnPress));
    public new static readonly StringName OnRelease = StringName.op_Implicit(nameof (OnRelease));
  }

  public new class PropertyName : NButton.PropertyName
  {
    public static readonly StringName Index = StringName.op_Implicit(nameof (Index));
    public static readonly StringName VoteContainer = StringName.op_Implicit(nameof (VoteContainer));
    public static readonly StringName Relic = StringName.op_Implicit(nameof (Relic));
    public static readonly StringName _uncommonGlow = StringName.op_Implicit(nameof (_uncommonGlow));
    public static readonly StringName _rareGlow = StringName.op_Implicit(nameof (_rareGlow));
    public static readonly StringName _animatedIn = StringName.op_Implicit(nameof (_animatedIn));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
    public static readonly StringName _initTween = StringName.op_Implicit(nameof (_initTween));
  }

  public new class SignalName : NButton.SignalName
  {
  }
}
