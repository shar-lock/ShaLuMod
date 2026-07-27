// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.CommonUi.NMultiplayerVoteContainer
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.CommonUi;

[ScriptPath("res://src/Core/Nodes/CommonUi/NMultiplayerVoteContainer.cs")]
public class NMultiplayerVoteContainer : Control
{
  private const string _voteIconPath = "ui/multiplayer_vote_icon";
  private readonly List<NMultiplayerVoteContainer.VoteIcon> _votes = new List<NMultiplayerVoteContainer.VoteIcon>();
  private readonly List<NMultiplayerVoteContainer.VoteIcon> _iconsAnimatingOut = new List<NMultiplayerVoteContainer.VoteIcon>();
  private NMultiplayerVoteContainer.PlayerVotedDelegate _playerVotedDelegate;
  private readonly List<Player> _allPlayers = new List<Player>();

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(SceneHelper.GetScenePath("ui/multiplayer_vote_icon"));
    }
  }

  public IEnumerable<Player> Players
  {
    get
    {
      return this._votes.Select<NMultiplayerVoteContainer.VoteIcon, Player>((Func<NMultiplayerVoteContainer.VoteIcon, Player>) (v => v.player));
    }
  }

  public void Initialize(
    NMultiplayerVoteContainer.PlayerVotedDelegate del,
    IReadOnlyList<Player> players)
  {
    this._playerVotedDelegate = del;
    this._allPlayers.AddRange((IEnumerable<Player>) players);
  }

  public void RefreshPlayerVotes(bool animate = true)
  {
    if (this._allPlayers.Count == 1)
      return;
    for (int index = 0; index < this._votes.Count; ++index)
    {
      NMultiplayerVoteContainer.VoteIcon vote = this._votes[index];
      if (!this._playerVotedDelegate(vote.player))
      {
        this.AnimVoteOut(vote, animate);
        this._votes.RemoveAt(index);
        --index;
      }
    }
    foreach (Player allPlayer in this._allPlayers)
    {
      Player player = allPlayer;
      if (this._playerVotedDelegate(player) && this._votes.FindIndex((Predicate<NMultiplayerVoteContainer.VoteIcon>) (p => p.player == player)) < 0)
      {
        NMultiplayerVoteContainer.VoteIcon vote = new NMultiplayerVoteContainer.VoteIcon()
        {
          player = player,
          node = SceneHelper.Instantiate<TextureRect>("ui/multiplayer_vote_icon")
        };
        vote.node.Texture = player.Character.IconTexture;
        ((Node) vote.node).GetNode<TextureRect>(NodePath.op_Implicit("Outline")).Texture = player.Character.IconOutlineTexture;
        this._votes.Add(vote);
        ((Node) this).AddChildSafely((Node) vote.node);
        ((Control) vote.node).PivotOffset = Vector2.op_Multiply(((Control) vote.node).Size, 0.5f);
        this.AnimVoteIn(vote, animate);
      }
    }
  }

  public int GetVoteIndex(Player player)
  {
    return this._votes.FindIndex((Predicate<NMultiplayerVoteContainer.VoteIcon>) (v => v.player == player));
  }

  public void SetPlayerHighlighted(Player player, bool isHighlighted)
  {
    NMultiplayerVoteContainer.VoteIcon voteIcon = this._votes.FirstOrDefault<NMultiplayerVoteContainer.VoteIcon>((Func<NMultiplayerVoteContainer.VoteIcon, bool>) (v => v.player == player));
    Tween t = !(voteIcon == (NMultiplayerVoteContainer.VoteIcon) null) ? voteIcon.tween : throw new InvalidOperationException();
    if (t != null)
      t.FastForwardToCompletion();
    if (isHighlighted)
      ((Control) voteIcon.node).Scale = Vector2.op_Multiply(Vector2.One, 1.25f);
    else
      ((Control) voteIcon.node).Scale = Vector2.One;
  }

  public void BouncePlayers()
  {
    foreach (NMultiplayerVoteContainer.VoteIcon vote in this._votes)
    {
      Tween tween = vote.tween;
      if (tween != null)
        tween.FastForwardToCompletion();
      vote.tween = ((Node) this).CreateTween().SetParallel(true);
      vote.tween.TweenProperty((GodotObject) vote.node, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 1.3f)), 0.15).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 4L);
      vote.tween.TweenProperty((GodotObject) vote.node, NodePath.op_Implicit("position:y"), Variant.op_Implicit(-10f), 0.15).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 4L);
      vote.tween.Chain().TweenProperty((GodotObject) vote.node, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), 0.2).SetEase((Tween.EaseType) 0L).SetTrans((Tween.TransitionType) 4L);
      vote.tween.TweenProperty((GodotObject) vote.node, NodePath.op_Implicit("position:y"), Variant.op_Implicit(0.0f), 0.3).SetEase((Tween.EaseType) 0L).SetTrans((Tween.TransitionType) 4L);
    }
  }

  private void AnimVoteIn(NMultiplayerVoteContainer.VoteIcon vote, bool animate)
  {
    int index = this._iconsAnimatingOut.FindIndex((Predicate<NMultiplayerVoteContainer.VoteIcon>) (i => i.player == vote.player));
    if (index > 0)
    {
      this._iconsAnimatingOut[index].tween?.Kill();
      ((Node) this._iconsAnimatingOut[index].node).QueueFreeSafely();
      this._iconsAnimatingOut.RemoveAt(index);
    }
    if (!animate)
      return;
    vote.tween?.Kill();
    vote.tween = ((Node) this).CreateTween().SetParallel(true);
    vote.tween.TweenProperty((GodotObject) vote.node, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.2).From(Variant.op_Implicit(0.0f));
    vote.tween.TweenProperty((GodotObject) vote.node, NodePath.op_Implicit("position:y"), Variant.op_Implicit(0.0f), 0.3).From(Variant.op_Implicit(20f)).SetTrans((Tween.TransitionType) 10L).SetEase((Tween.EaseType) 1L);
  }

  private void AnimVoteOut(NMultiplayerVoteContainer.VoteIcon vote, bool animate)
  {
    this._iconsAnimatingOut.Add(vote);
    if (animate)
    {
      vote.tween?.Kill();
      vote.tween = ((Node) this).CreateTween().SetParallel(true);
      vote.tween.TweenProperty((GodotObject) vote.node, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.1).SetDelay(0.15000000596046448);
      vote.tween.TweenProperty((GodotObject) vote.node, NodePath.op_Implicit("position:y"), Variant.op_Implicit(20f), 0.25).SetTrans((Tween.TransitionType) 5L).SetEase((Tween.EaseType) 0L);
      vote.tween.Chain().TweenCallback(Callable.From((Action) (() => this.RemoveVoteAfterAnimation(vote))));
    }
    else
      this.RemoveVoteAfterAnimation(vote);
  }

  private void RemoveVoteAfterAnimation(NMultiplayerVoteContainer.VoteIcon vote)
  {
    this._iconsAnimatingOut.Remove(vote);
    ((Node) vote.node).QueueFreeSafely();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NMultiplayerVoteContainer.MethodName.RefreshPlayerVotes, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("animate"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NMultiplayerVoteContainer.MethodName.BouncePlayers, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NMultiplayerVoteContainer.MethodName.RefreshPlayerVotes) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.RefreshPlayerVotes(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NMultiplayerVoteContainer.MethodName.BouncePlayers) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.BouncePlayers();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NMultiplayerVoteContainer.MethodName.RefreshPlayerVotes) || StringName.op_Equality(ref method, NMultiplayerVoteContainer.MethodName.BouncePlayers) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
  }

  public delegate bool PlayerVotedDelegate(
  #nullable enable
  Player player);

  private record VoteIcon()
  {
    public required Player player;
    public required TextureRect node;
    public Tween? tween;

    [CompilerGenerated]
    protected virtual bool PrintMembers(StringBuilder builder)
    {
      RuntimeHelpers.EnsureSufficientExecutionStack();
      builder.Append("player = ");
      builder.Append((object) this.player);
      builder.Append(", node = ");
      builder.Append((object) this.node);
      builder.Append(", tween = ");
      builder.Append((object) this.tween);
      return true;
    }

    [CompilerGenerated]
    public override int GetHashCode()
    {
      return ((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<Player>.Default.GetHashCode(this.player)) * -1521134295 + EqualityComparer<TextureRect>.Default.GetHashCode(this.node)) * -1521134295 + EqualityComparer<Tween>.Default.GetHashCode(this.tween);
    }

    [CompilerGenerated]
    public virtual bool Equals(NMultiplayerVoteContainer.VoteIcon? other)
    {
      if ((object) this == (object) other)
        return true;
      return (object) other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<Player>.Default.Equals(this.player, other.player) && EqualityComparer<TextureRect>.Default.Equals(this.node, other.node) && EqualityComparer<Tween>.Default.Equals(this.tween, other.tween);
    }

    [CompilerGenerated]
    [SetsRequiredMembers]
    protected VoteIcon(NMultiplayerVoteContainer.VoteIcon original)
    {
      this.player = original.player;
      this.node = original.node;
      this.tween = original.tween;
    }
  }

  public class MethodName : Control.MethodName
  {
    public static readonly 
    #nullable disable
    StringName RefreshPlayerVotes = StringName.op_Implicit(nameof (RefreshPlayerVotes));
    public static readonly StringName BouncePlayers = StringName.op_Implicit(nameof (BouncePlayers));
  }

  public class PropertyName : Control.PropertyName
  {
  }

  public class SignalName : Control.SignalName
  {
  }
}
