// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Rewards.NLinkedRewardSet
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Screens;
using MegaCrit.Sts2.Core.Rewards;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Rewards;

[ScriptPath("res://src/Core/Nodes/Rewards/NLinkedRewardSet.cs")]
public class NLinkedRewardSet : Control
{
  private NRewardsScreen _rewardsScreen;
  private Control _rewardContainer;
  private Control _chainsContainer;
  private 
  #nullable disable
  NLinkedRewardSet.RewardClaimedEventHandler backing_RewardClaimed;

  public 
  #nullable enable
  LinkedRewardSet LinkedRewardSet { get; private set; }

  private static string ScenePath => SceneHelper.GetScenePath("/rewards/linked_reward_set");

  private static string ChainImagePath
  {
    get => ImageHelper.GetImagePath("/ui/reward_screen/reward_chain.png");
  }

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new string[2]
      {
        NLinkedRewardSet.ScenePath,
        NLinkedRewardSet.ChainImagePath
      };
    }
  }

  public override void _Ready()
  {
    this._rewardContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%RewardContainer"));
    this._chainsContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%ChainContainer"));
    this.Reload();
  }

  public static NLinkedRewardSet Create(LinkedRewardSet linkedReward, NRewardsScreen screen)
  {
    NLinkedRewardSet nlinkedRewardSet = PreloadManager.Cache.GetScene(NLinkedRewardSet.ScenePath).Instantiate<NLinkedRewardSet>((PackedScene.GenEditState) 0L);
    nlinkedRewardSet._rewardsScreen = screen;
    nlinkedRewardSet.SetReward(linkedReward);
    return nlinkedRewardSet;
  }

  private void SetReward(LinkedRewardSet linkedReward)
  {
    this.LinkedRewardSet = linkedReward;
    if (!((Node) this).IsNodeReady())
      return;
    this.Reload();
  }

  private void Reload()
  {
    if (!((Node) this).IsNodeReady())
      return;
    for (int index = 0; index < this.LinkedRewardSet.Rewards.Count; ++index)
    {
      NRewardButton child1 = NRewardButton.Create(this.LinkedRewardSet.Rewards[index], this._rewardsScreen);
      NRewardButton nrewardButton = child1;
      nrewardButton.CustomMinimumSize = Vector2.op_Subtraction(nrewardButton.CustomMinimumSize, Vector2.op_Multiply(Vector2.Right, 20f));
      ((Node) this._rewardContainer).AddChildSafely((Node) child1);
      ((GodotObject) child1).Connect(NRewardButton.SignalName.RewardClaimed, Callable.From(new Action(this.GetReward)), 0U);
      if (index < this.LinkedRewardSet.Rewards.Count - 1)
      {
        TextureRect child2 = new TextureRect();
        ((Control) child2).MouseFilter = (Control.MouseFilterEnum) 2L;
        child2.Texture = (Texture2D) PreloadManager.Cache.GetCompressedTexture2D(NLinkedRewardSet.ChainImagePath);
        ((Control) child2).Size = Vector2.op_Multiply(Vector2.One, 50f);
        ((Node) this._chainsContainer).AddChildSafely((Node) child2);
        ((Control) child2).GlobalPosition = Vector2.op_Addition(this._chainsContainer.GlobalPosition, Vector2.op_Multiply(Vector2.op_Multiply(Vector2.Down, (float) index), 3f + child1.CustomMinimumSize.Y));
      }
    }
  }

  private void GetReward()
  {
    this._rewardsScreen.RewardCollectedFrom((Control) this);
    this.LinkedRewardSet.OnSkipped();
    ((GodotObject) this).EmitSignal(NLinkedRewardSet.SignalName.RewardClaimed, Array.Empty<Variant>());
    ((Node) this).QueueFreeSafely();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(3)
    {
      new MethodInfo(NLinkedRewardSet.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NLinkedRewardSet.MethodName.Reload, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NLinkedRewardSet.MethodName.GetReward, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NLinkedRewardSet.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NLinkedRewardSet.MethodName.Reload) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Reload();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NLinkedRewardSet.MethodName.GetReward) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.GetReward();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NLinkedRewardSet.MethodName._Ready) || StringName.op_Equality(ref method, NLinkedRewardSet.MethodName.Reload) || StringName.op_Equality(ref method, NLinkedRewardSet.MethodName.GetReward) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NLinkedRewardSet.PropertyName._rewardsScreen))
    {
      this._rewardsScreen = VariantUtils.ConvertTo<NRewardsScreen>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NLinkedRewardSet.PropertyName._rewardContainer))
    {
      this._rewardContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NLinkedRewardSet.PropertyName._chainsContainer))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._chainsContainer = VariantUtils.ConvertTo<Control>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NLinkedRewardSet.PropertyName._rewardsScreen))
    {
      value = VariantUtils.CreateFrom<NRewardsScreen>(ref this._rewardsScreen);
      return true;
    }
    if (StringName.op_Equality(ref name, NLinkedRewardSet.PropertyName._rewardContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._rewardContainer);
      return true;
    }
    if (!StringName.op_Equality(ref name, NLinkedRewardSet.PropertyName._chainsContainer))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Control>(ref this._chainsContainer);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NLinkedRewardSet.PropertyName._rewardsScreen, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NLinkedRewardSet.PropertyName._rewardContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NLinkedRewardSet.PropertyName._chainsContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NLinkedRewardSet.PropertyName._rewardsScreen, Variant.From<NRewardsScreen>(ref this._rewardsScreen));
    info.AddProperty(NLinkedRewardSet.PropertyName._rewardContainer, Variant.From<Control>(ref this._rewardContainer));
    info.AddProperty(NLinkedRewardSet.PropertyName._chainsContainer, Variant.From<Control>(ref this._chainsContainer));
    info.AddSignalEventDelegate(NLinkedRewardSet.SignalName.RewardClaimed, (Delegate) this.backing_RewardClaimed);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NLinkedRewardSet.PropertyName._rewardsScreen, ref variant1))
      this._rewardsScreen = ((Variant) ref variant1).As<NRewardsScreen>();
    Variant variant2;
    if (info.TryGetProperty(NLinkedRewardSet.PropertyName._rewardContainer, ref variant2))
      this._rewardContainer = ((Variant) ref variant2).As<Control>();
    Variant variant3;
    if (info.TryGetProperty(NLinkedRewardSet.PropertyName._chainsContainer, ref variant3))
      this._chainsContainer = ((Variant) ref variant3).As<Control>();
    NLinkedRewardSet.RewardClaimedEventHandler claimedEventHandler;
    if (!info.TryGetSignalEventDelegate<NLinkedRewardSet.RewardClaimedEventHandler>(NLinkedRewardSet.SignalName.RewardClaimed, ref claimedEventHandler))
      return;
    this.backing_RewardClaimed = claimedEventHandler;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<MethodInfo> GetGodotSignalList()
  {
    return new List<MethodInfo>(1)
    {
      new MethodInfo(NLinkedRewardSet.SignalName.RewardClaimed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("linkedRewardSet"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null)
    };
  }

  public event NLinkedRewardSet.RewardClaimedEventHandler RewardClaimed
  {
    add => this.backing_RewardClaimed += value;
    remove => this.backing_RewardClaimed -= value;
  }

  protected void EmitSignalRewardClaimed(NLinkedRewardSet linkedRewardSet)
  {
    ((GodotObject) this).EmitSignal(NLinkedRewardSet.SignalName.RewardClaimed, new Variant[1]
    {
      Variant.op_Implicit((GodotObject) linkedRewardSet)
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RaiseGodotClassSignalCallbacks(
    in godot_string_name signal,
    NativeVariantPtrArgs args)
  {
    if (StringName.op_Equality(ref signal, NLinkedRewardSet.SignalName.RewardClaimed) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NLinkedRewardSet.RewardClaimedEventHandler backingRewardClaimed = this.backing_RewardClaimed;
      if (backingRewardClaimed == null)
        return;
      backingRewardClaimed(VariantUtils.ConvertTo<NLinkedRewardSet>(ref ((NativeVariantPtrArgs) ref args)[0]));
    }
    else
      ((GodotObject) this).RaiseGodotClassSignalCallbacks(ref signal, args);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassSignal(in godot_string_name signal)
  {
    return StringName.op_Equality(ref signal, NLinkedRewardSet.SignalName.RewardClaimed) || base.HasGodotClassSignal(ref signal);
  }

  [Signal]
  public delegate void RewardClaimedEventHandler(
  #nullable enable
  NLinkedRewardSet linkedRewardSet);

  public class MethodName : Control.MethodName
  {
    public static readonly 
    #nullable disable
    StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName Reload = StringName.op_Implicit(nameof (Reload));
    public static readonly StringName GetReward = StringName.op_Implicit(nameof (GetReward));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName _rewardsScreen = StringName.op_Implicit(nameof (_rewardsScreen));
    public static readonly StringName _rewardContainer = StringName.op_Implicit(nameof (_rewardContainer));
    public static readonly StringName _chainsContainer = StringName.op_Implicit(nameof (_chainsContainer));
  }

  public class SignalName : Control.SignalName
  {
    public static readonly StringName RewardClaimed = StringName.op_Implicit(nameof (RewardClaimed));
  }
}
