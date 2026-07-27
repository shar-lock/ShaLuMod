// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Events.NEventOptionButton
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Events;

[ScriptPath("res://src/Core/Nodes/Events/NEventOptionButton.cs")]
public class NEventOptionButton : NButton
{
  private static readonly StringName _v = new StringName("v");
  private static readonly StringName _s = new StringName("s");
  private static readonly StringName _h = new StringName("h");
  private static readonly string _voteIconPath = SceneHelper.GetScenePath("ui/multiplayer_vote_icon");
  private MegaRichTextLabel _label;
  private NinePatchRect _image;
  private NinePatchRect _outline;
  private NinePatchRect _killGlow;
  private NinePatchRect _confirmFlash;
  private ShaderMaterial? _hsv;
  private NMultiplayerVoteContainer _playerVoteContainer;
  private Tween? _animInTween;
  private Tween? _flashTween;
  private Tween? _killGlowTween;
  private Tween? _tween;
  private static readonly Vector2 _hoverScale = Vector2.op_Multiply(Vector2.One, 1.01f);
  private static readonly Vector2 _pressScale = Vector2.op_Multiply(Vector2.One, 0.99f);
  private const float _defaultV = 0.9f;
  private const float _hoverV = 1.2f;
  private readonly CancellationTokenSource _cancelToken = new CancellationTokenSource();
  private Color _buttonColor;
  private NThoughtBubbleVfx? _deathPreventionVfx;
  private CancellationTokenSource? _deathPreventionCancellation;
  private Vector2 _deathPreventionVfxPosition;

  public EventModel Event { get; private set; }

  public EventOption Option { get; private set; }

  private int Index { get; set; }

  public NMultiplayerVoteContainer VoteContainer => this._playerVoteContainer;

  private static string ScenePath => SceneHelper.GetScenePath("events/event_option_button");

  private static string AncientScenePath
  {
    get => SceneHelper.GetScenePath("events/ancient_event_option_button");
  }

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlyArray<string>(new string[3]
      {
        NEventOptionButton.ScenePath,
        NEventOptionButton.AncientScenePath,
        NEventOptionButton._voteIconPath
      });
    }
  }

  public static NEventOptionButton Create(EventModel eventModel, EventOption option, int index)
  {
    NEventOptionButton neventOptionButton = eventModel is AncientEventModel ? PreloadManager.Cache.GetScene(NEventOptionButton.AncientScenePath).Instantiate<NEventOptionButton>((PackedScene.GenEditState) 0L) : PreloadManager.Cache.GetScene(NEventOptionButton.ScenePath).Instantiate<NEventOptionButton>((PackedScene.GenEditState) 0L);
    neventOptionButton.Event = eventModel;
    neventOptionButton.Option = option;
    neventOptionButton.Index = index;
    neventOptionButton._buttonColor = eventModel.ButtonColor;
    return neventOptionButton;
  }

  public override void _Ready()
  {
    this.ConnectSignals();
    this.Event.DynamicVars.AddTo(this.Option.Description);
    this.Event.DynamicVars.AddTo(this.Option.Title);
    string formattedText1 = this.Option.Title.GetFormattedText();
    string formattedText2 = this.Option.Description.GetFormattedText();
    this._label = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%Text"));
    if (string.IsNullOrEmpty(formattedText1))
    {
      this._label.Text = formattedText2;
    }
    else
    {
      string str = this.Option.IsLocked ? "red" : "gold";
      this._label.Text = $"[{str}][b]{formattedText1}[/b][/{str}]\n{formattedText2}";
    }
    this._image = ((Node) this).GetNode<NinePatchRect>(NodePath.op_Implicit("Image"));
    ((CanvasItem) this._image).Modulate = this._buttonColor;
    this._hsv = (ShaderMaterial) ((CanvasItem) this._image).Material;
    this._outline = ((Node) this).GetNode<NinePatchRect>(NodePath.op_Implicit("Outline"));
    this._killGlow = ((Node) this).GetNode<NinePatchRect>(NodePath.op_Implicit("RedFlash"));
    this._confirmFlash = ((Node) this).GetNode<NinePatchRect>(NodePath.op_Implicit("BlueFlash"));
    this._playerVoteContainer = ((Node) this).GetNode<NMultiplayerVoteContainer>(NodePath.op_Implicit("PlayerVoteContainer"));
    this._playerVoteContainer.Initialize(new NMultiplayerVoteContainer.PlayerVotedDelegate(this.ShouldDisplayPlayerVote), this.Event.Owner.RunState.Players);
    if (this.Event is AncientEventModel && this.Option.Relic != null)
    {
      TextureRect node = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%RelicIcon"));
      node.SetTexture(this.Option.Relic.Icon);
      ((Node) node).GetNode<TextureRect>(NodePath.op_Implicit("%Outline")).SetTexture(this.Option.Relic.IconOutline);
      ((CanvasItem) node).Visible = true;
    }
    if (this.Option.IsLocked)
    {
      this.SetVisuallyLocked();
    }
    else
    {
      if (!this.WillKillPlayer())
        return;
      this.ShowPersistentKillGlow();
    }
  }

  private void SetVisuallyLocked()
  {
    ((CanvasItem) this._label).Modulate = new Color(((CanvasItem) this._label).Modulate.R, ((CanvasItem) this._label).Modulate.G, ((CanvasItem) this._label).Modulate.B, 0.7f);
    this._hsv?.SetShaderParameter(NEventOptionButton._h, Variant.op_Implicit(0.48f));
    this._hsv?.SetShaderParameter(NEventOptionButton._s, Variant.op_Implicit(0.2f));
    this._hsv?.SetShaderParameter(NEventOptionButton._v, Variant.op_Implicit(0.65f));
  }

  public void AnimateIn()
  {
    if (!GodotObject.IsInstanceValid((GodotObject) this))
      return;
    if (SaveManager.Instance.PrefsSave.FastMode == FastModeType.Instant)
    {
      this.EnableButton();
    }
    else
    {
      ((CanvasItem) this).Modulate = StsColors.transparentWhite;
      bool flag = SaveManager.Instance.PrefsSave.FastMode == FastModeType.Fast;
      this._animInTween = ((Node) this).CreateTween().SetParallel(true);
      this._animInTween.TweenInterval(flag ? 0.25 : 0.5);
      this._animInTween.Chain();
      this._animInTween.TweenInterval((flag ? 0.1 : 0.2) * (double) this.Index);
      this._animInTween.Chain();
      this._animInTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("position"), Variant.op_Implicit(this.Position), flag ? 0.25 : 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 10L).From(Variant.op_Implicit(Vector2.op_Addition(this.Position, new Vector2(-60f, 0.0f))));
      this._animInTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.White), flag ? 0.25 : 0.5);
      this._animInTween.Finished += new Action(this.EnableButton);
    }
  }

  public void EnableButton() => this.MouseFilter = (Control.MouseFilterEnum) 0L;

  protected override void OnRelease()
  {
    if (this.Option.IsLocked)
      return;
    if (this.WillKillPlayer())
    {
      if (this.Event.Owner.RunState.Players.Count > 1)
      {
        if (this._deathPreventionVfx == null)
        {
          this._deathPreventionVfx = NThoughtBubbleVfx.Create(this.Event.Owner.Character.EventDeathPreventionLine.GetFormattedText(), DialogueSide.Right, new double?());
          NEventRoom instance = NEventRoom.Instance;
          if (instance != null)
          {
            Control vfxContainer = instance.VfxContainer;
            if (vfxContainer != null)
              ((Node) vfxContainer).AddChildSafely((Node) this._deathPreventionVfx);
          }
          this._deathPreventionVfxPosition = Vector2.op_Addition(this.GlobalPosition, new Vector2(-50f, -15f));
          this._deathPreventionVfx.GlobalPosition = this._deathPreventionVfxPosition;
        }
        else
          this._deathPreventionCancellation?.Cancel();
        TaskHelper.RunSafely(this.RumbleDeathVfx());
        TaskHelper.RunSafely(this.ExpireDeathPreventionVfx());
        return;
      }
      this.ShowPersistentKillGlow();
    }
    NEventRoom.Instance.OptionButtonClicked(this.Option, this.Index);
  }

  private async Task RumbleDeathVfx()
  {
    ScreenRumbleInstance rumble = new ScreenRumbleInstance(20f, 0.30000001192092896, 10f, RumbleStyle.Rumble);
    while (!rumble.IsDone)
      this._deathPreventionVfx.GlobalPosition = Vector2.op_Addition(this._deathPreventionVfxPosition, rumble.Update((double) await ((Node) this).AwaitProcessFrame(this._cancelToken.Token)));
    rumble = (ScreenRumbleInstance) null;
  }

  private async Task ExpireDeathPreventionVfx()
  {
    this._deathPreventionCancellation = new CancellationTokenSource();
    await Cmd.Wait(2.5f, this._deathPreventionCancellation.Token, true);
    if (this._deathPreventionCancellation.IsCancellationRequested)
      return;
    if (this._deathPreventionVfx != null)
      TaskHelper.RunSafely(this._deathPreventionVfx.GoAway());
    this._deathPreventionVfx = (NThoughtBubbleVfx) null;
  }

  protected override void OnPress()
  {
    if (this.Option.IsLocked)
      return;
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("scale"), Variant.op_Implicit(NEventOptionButton._pressScale), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderParam)), Variant.op_Implicit(1.2f), Variant.op_Implicit(0.9f), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    base.OnPress();
  }

  protected override void OnFocus()
  {
    if (this.Option.IsLocked)
      return;
    base.OnFocus();
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("scale"), Variant.op_Implicit(NEventOptionButton._hoverScale), 0.05);
    this._tween.TweenProperty((GodotObject) this._outline, NodePath.op_Implicit("modulate"), Variant.op_Implicit(StsColors.blueGlow), 0.05);
    this._hsv?.SetShaderParameter(NEventOptionButton._v, Variant.op_Implicit(1.2f));
    NHoverTipSet.CreateAndShow((Control) this, this.Option.HoverTips, this.Event.LayoutType == EventLayoutType.Combat ? HoverTipAlignment.Right : HoverTipAlignment.Left);
    if (!this.WillKillPlayer())
      return;
    this.PulseKillGlow();
  }

  protected override void OnUnfocus()
  {
    if (this.Option.IsLocked)
      return;
    base.OnUnfocus();
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderParam)), Variant.op_Implicit(1.2f), Variant.op_Implicit(0.9f), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._tween.TweenProperty((GodotObject) this._outline, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.3);
    NHoverTipSet.Remove((Control) this);
    if (!this.WillKillPlayer())
      return;
    this.ShowPersistentKillGlow();
  }

  public async Task FlashConfirmation()
  {
    this._flashTween?.Kill();
    NinePatchRect confirmFlash = this._confirmFlash;
    Color modulate = ((CanvasItem) this._confirmFlash).Modulate;
    modulate.A = 0.0f;
    Color color = modulate;
    ((CanvasItem) confirmFlash).Modulate = color;
    this._flashTween = ((Node) this).CreateTween();
    this._flashTween.TweenProperty((GodotObject) this._confirmFlash, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.8f), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._flashTween.Parallel().TweenProperty((GodotObject) this._confirmFlash, NodePath.op_Implicit("scale"), Variant.op_Implicit(new Vector2(1.03f, 1.1f)), 0.25).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._flashTween.TweenProperty((GodotObject) this._confirmFlash, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), 0.25).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._flashTween.TweenProperty((GodotObject) this._confirmFlash, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.3);
    if (!await this._flashTween.AwaitFinished((Node) this))
      return;
    await Cmd.Wait(0.5f, this._cancelToken.Token);
  }

  public void GrayOut()
  {
    this._flashTween?.Kill();
    this._flashTween = ((Node) this).CreateTween();
    Tween flashTween = this._flashTween;
    NodePath nodePath = NodePath.op_Implicit("modulate");
    Color lightGray = StsColors.lightGray;
    lightGray.A = 0.5f;
    Variant variant = Variant.op_Implicit(lightGray);
    flashTween.TweenProperty((GodotObject) this, nodePath, variant, 0.3).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
  }

  private void UpdateShaderParam(float newV)
  {
    this._hsv?.SetShaderParameter(NEventOptionButton._v, Variant.op_Implicit(newV));
  }

  private bool ShouldDisplayPlayerVote(Player player)
  {
    uint? playerVote = RunManager.Instance.EventSynchronizer.GetPlayerVote(player);
    long? nullable = playerVote.HasValue ? new long?((long) playerVote.GetValueOrDefault()) : new long?();
    long index = (long) this.Index;
    return nullable.GetValueOrDefault() == index & nullable.HasValue;
  }

  public void RefreshVotes() => this._playerVoteContainer.RefreshPlayerVotes();

  public override void _ExitTree()
  {
    base._ExitTree();
    this._cancelToken.Cancel();
    if (this._animInTween == null)
      return;
    this._animInTween.Finished -= new Action(this.EnableButton);
  }

  private bool WillKillPlayer()
  {
    if (this.Event.Owner == null)
      return false;
    Func<Player, bool> willKillPlayer = this.Option.WillKillPlayer;
    return willKillPlayer != null && willKillPlayer(this.Event.Owner);
  }

  private void ShowPersistentKillGlow()
  {
    this._killGlowTween?.Kill();
    NinePatchRect killGlow = this._killGlow;
    Color modulate = ((CanvasItem) this._killGlow).Modulate;
    modulate.A = 0.5f;
    Color color = modulate;
    ((CanvasItem) killGlow).Modulate = color;
  }

  private void PulseKillGlow()
  {
    this._killGlowTween?.Kill();
    NinePatchRect killGlow = this._killGlow;
    Color modulate = ((CanvasItem) this._killGlow).Modulate;
    modulate.A = 0.25f;
    Color color = modulate;
    ((CanvasItem) killGlow).Modulate = color;
    this._killGlowTween = ((Node) this).CreateTween();
    this._killGlowTween.TweenProperty((GodotObject) this._killGlow, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.2).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._killGlowTween.TweenProperty((GodotObject) this._killGlow, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.25f), 0.8);
    this._killGlowTween.SetLoops(0);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(15)
    {
      new MethodInfo(NEventOptionButton.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEventOptionButton.MethodName.SetVisuallyLocked, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEventOptionButton.MethodName.AnimateIn, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEventOptionButton.MethodName.EnableButton, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEventOptionButton.MethodName.OnRelease, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEventOptionButton.MethodName.OnPress, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEventOptionButton.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEventOptionButton.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEventOptionButton.MethodName.GrayOut, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEventOptionButton.MethodName.UpdateShaderParam, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("newV"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NEventOptionButton.MethodName.RefreshVotes, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEventOptionButton.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEventOptionButton.MethodName.WillKillPlayer, new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEventOptionButton.MethodName.ShowPersistentKillGlow, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEventOptionButton.MethodName.PulseKillGlow, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NEventOptionButton.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEventOptionButton.MethodName.SetVisuallyLocked) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.SetVisuallyLocked();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEventOptionButton.MethodName.AnimateIn) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AnimateIn();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEventOptionButton.MethodName.EnableButton) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.EnableButton();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEventOptionButton.MethodName.OnRelease) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnRelease();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEventOptionButton.MethodName.OnPress) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnPress();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEventOptionButton.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEventOptionButton.MethodName.OnUnfocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnfocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEventOptionButton.MethodName.GrayOut) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.GrayOut();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEventOptionButton.MethodName.UpdateShaderParam) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.UpdateShaderParam(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEventOptionButton.MethodName.RefreshVotes) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RefreshVotes();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEventOptionButton.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEventOptionButton.MethodName.WillKillPlayer) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      bool flag = this.WillKillPlayer();
      ret = VariantUtils.CreateFrom<bool>(ref flag);
      return true;
    }
    if (StringName.op_Equality(ref method, NEventOptionButton.MethodName.ShowPersistentKillGlow) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ShowPersistentKillGlow();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NEventOptionButton.MethodName.PulseKillGlow) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.PulseKillGlow();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NEventOptionButton.MethodName._Ready) || StringName.op_Equality(ref method, NEventOptionButton.MethodName.SetVisuallyLocked) || StringName.op_Equality(ref method, NEventOptionButton.MethodName.AnimateIn) || StringName.op_Equality(ref method, NEventOptionButton.MethodName.EnableButton) || StringName.op_Equality(ref method, NEventOptionButton.MethodName.OnRelease) || StringName.op_Equality(ref method, NEventOptionButton.MethodName.OnPress) || StringName.op_Equality(ref method, NEventOptionButton.MethodName.OnFocus) || StringName.op_Equality(ref method, NEventOptionButton.MethodName.OnUnfocus) || StringName.op_Equality(ref method, NEventOptionButton.MethodName.GrayOut) || StringName.op_Equality(ref method, NEventOptionButton.MethodName.UpdateShaderParam) || StringName.op_Equality(ref method, NEventOptionButton.MethodName.RefreshVotes) || StringName.op_Equality(ref method, NEventOptionButton.MethodName._ExitTree) || StringName.op_Equality(ref method, NEventOptionButton.MethodName.WillKillPlayer) || StringName.op_Equality(ref method, NEventOptionButton.MethodName.ShowPersistentKillGlow) || StringName.op_Equality(ref method, NEventOptionButton.MethodName.PulseKillGlow) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NEventOptionButton.PropertyName.Index))
    {
      this.Index = VariantUtils.ConvertTo<int>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEventOptionButton.PropertyName._label))
    {
      this._label = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEventOptionButton.PropertyName._image))
    {
      this._image = VariantUtils.ConvertTo<NinePatchRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEventOptionButton.PropertyName._outline))
    {
      this._outline = VariantUtils.ConvertTo<NinePatchRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEventOptionButton.PropertyName._killGlow))
    {
      this._killGlow = VariantUtils.ConvertTo<NinePatchRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEventOptionButton.PropertyName._confirmFlash))
    {
      this._confirmFlash = VariantUtils.ConvertTo<NinePatchRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEventOptionButton.PropertyName._hsv))
    {
      this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEventOptionButton.PropertyName._playerVoteContainer))
    {
      this._playerVoteContainer = VariantUtils.ConvertTo<NMultiplayerVoteContainer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEventOptionButton.PropertyName._animInTween))
    {
      this._animInTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEventOptionButton.PropertyName._flashTween))
    {
      this._flashTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEventOptionButton.PropertyName._killGlowTween))
    {
      this._killGlowTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEventOptionButton.PropertyName._tween))
    {
      this._tween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEventOptionButton.PropertyName._buttonColor))
    {
      this._buttonColor = VariantUtils.ConvertTo<Color>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEventOptionButton.PropertyName._deathPreventionVfx))
    {
      this._deathPreventionVfx = VariantUtils.ConvertTo<NThoughtBubbleVfx>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NEventOptionButton.PropertyName._deathPreventionVfxPosition))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._deathPreventionVfxPosition = VariantUtils.ConvertTo<Vector2>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NEventOptionButton.PropertyName.Index))
    {
      ref godot_variant local = ref value;
      int index = this.Index;
      godot_variant from = VariantUtils.CreateFrom<int>(ref index);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NEventOptionButton.PropertyName.VoteContainer))
    {
      ref godot_variant local = ref value;
      NMultiplayerVoteContainer voteContainer = this.VoteContainer;
      godot_variant from = VariantUtils.CreateFrom<NMultiplayerVoteContainer>(ref voteContainer);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NEventOptionButton.PropertyName._label))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._label);
      return true;
    }
    if (StringName.op_Equality(ref name, NEventOptionButton.PropertyName._image))
    {
      value = VariantUtils.CreateFrom<NinePatchRect>(ref this._image);
      return true;
    }
    if (StringName.op_Equality(ref name, NEventOptionButton.PropertyName._outline))
    {
      value = VariantUtils.CreateFrom<NinePatchRect>(ref this._outline);
      return true;
    }
    if (StringName.op_Equality(ref name, NEventOptionButton.PropertyName._killGlow))
    {
      value = VariantUtils.CreateFrom<NinePatchRect>(ref this._killGlow);
      return true;
    }
    if (StringName.op_Equality(ref name, NEventOptionButton.PropertyName._confirmFlash))
    {
      value = VariantUtils.CreateFrom<NinePatchRect>(ref this._confirmFlash);
      return true;
    }
    if (StringName.op_Equality(ref name, NEventOptionButton.PropertyName._hsv))
    {
      value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._hsv);
      return true;
    }
    if (StringName.op_Equality(ref name, NEventOptionButton.PropertyName._playerVoteContainer))
    {
      value = VariantUtils.CreateFrom<NMultiplayerVoteContainer>(ref this._playerVoteContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NEventOptionButton.PropertyName._animInTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._animInTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NEventOptionButton.PropertyName._flashTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._flashTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NEventOptionButton.PropertyName._killGlowTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._killGlowTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NEventOptionButton.PropertyName._tween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._tween);
      return true;
    }
    if (StringName.op_Equality(ref name, NEventOptionButton.PropertyName._buttonColor))
    {
      value = VariantUtils.CreateFrom<Color>(ref this._buttonColor);
      return true;
    }
    if (StringName.op_Equality(ref name, NEventOptionButton.PropertyName._deathPreventionVfx))
    {
      value = VariantUtils.CreateFrom<NThoughtBubbleVfx>(ref this._deathPreventionVfx);
      return true;
    }
    if (!StringName.op_Equality(ref name, NEventOptionButton.PropertyName._deathPreventionVfxPosition))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<Vector2>(ref this._deathPreventionVfxPosition);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NEventOptionButton.PropertyName._label, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEventOptionButton.PropertyName._image, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEventOptionButton.PropertyName._outline, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEventOptionButton.PropertyName._killGlow, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEventOptionButton.PropertyName._confirmFlash, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEventOptionButton.PropertyName._hsv, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEventOptionButton.PropertyName._playerVoteContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEventOptionButton.PropertyName._animInTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEventOptionButton.PropertyName._flashTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEventOptionButton.PropertyName._killGlowTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEventOptionButton.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NEventOptionButton.PropertyName.Index, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEventOptionButton.PropertyName.VoteContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 20L, NEventOptionButton.PropertyName._buttonColor, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEventOptionButton.PropertyName._deathPreventionVfx, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NEventOptionButton.PropertyName._deathPreventionVfxPosition, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo = info;
    StringName index1 = NEventOptionButton.PropertyName.Index;
    int index2 = this.Index;
    Variant variant = Variant.From<int>(ref index2);
    serializationInfo.AddProperty(index1, variant);
    info.AddProperty(NEventOptionButton.PropertyName._label, Variant.From<MegaRichTextLabel>(ref this._label));
    info.AddProperty(NEventOptionButton.PropertyName._image, Variant.From<NinePatchRect>(ref this._image));
    info.AddProperty(NEventOptionButton.PropertyName._outline, Variant.From<NinePatchRect>(ref this._outline));
    info.AddProperty(NEventOptionButton.PropertyName._killGlow, Variant.From<NinePatchRect>(ref this._killGlow));
    info.AddProperty(NEventOptionButton.PropertyName._confirmFlash, Variant.From<NinePatchRect>(ref this._confirmFlash));
    info.AddProperty(NEventOptionButton.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
    info.AddProperty(NEventOptionButton.PropertyName._playerVoteContainer, Variant.From<NMultiplayerVoteContainer>(ref this._playerVoteContainer));
    info.AddProperty(NEventOptionButton.PropertyName._animInTween, Variant.From<Tween>(ref this._animInTween));
    info.AddProperty(NEventOptionButton.PropertyName._flashTween, Variant.From<Tween>(ref this._flashTween));
    info.AddProperty(NEventOptionButton.PropertyName._killGlowTween, Variant.From<Tween>(ref this._killGlowTween));
    info.AddProperty(NEventOptionButton.PropertyName._tween, Variant.From<Tween>(ref this._tween));
    info.AddProperty(NEventOptionButton.PropertyName._buttonColor, Variant.From<Color>(ref this._buttonColor));
    info.AddProperty(NEventOptionButton.PropertyName._deathPreventionVfx, Variant.From<NThoughtBubbleVfx>(ref this._deathPreventionVfx));
    info.AddProperty(NEventOptionButton.PropertyName._deathPreventionVfxPosition, Variant.From<Vector2>(ref this._deathPreventionVfxPosition));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NEventOptionButton.PropertyName.Index, ref variant1))
      this.Index = ((Variant) ref variant1).As<int>();
    Variant variant2;
    if (info.TryGetProperty(NEventOptionButton.PropertyName._label, ref variant2))
      this._label = ((Variant) ref variant2).As<MegaRichTextLabel>();
    Variant variant3;
    if (info.TryGetProperty(NEventOptionButton.PropertyName._image, ref variant3))
      this._image = ((Variant) ref variant3).As<NinePatchRect>();
    Variant variant4;
    if (info.TryGetProperty(NEventOptionButton.PropertyName._outline, ref variant4))
      this._outline = ((Variant) ref variant4).As<NinePatchRect>();
    Variant variant5;
    if (info.TryGetProperty(NEventOptionButton.PropertyName._killGlow, ref variant5))
      this._killGlow = ((Variant) ref variant5).As<NinePatchRect>();
    Variant variant6;
    if (info.TryGetProperty(NEventOptionButton.PropertyName._confirmFlash, ref variant6))
      this._confirmFlash = ((Variant) ref variant6).As<NinePatchRect>();
    Variant variant7;
    if (info.TryGetProperty(NEventOptionButton.PropertyName._hsv, ref variant7))
      this._hsv = ((Variant) ref variant7).As<ShaderMaterial>();
    Variant variant8;
    if (info.TryGetProperty(NEventOptionButton.PropertyName._playerVoteContainer, ref variant8))
      this._playerVoteContainer = ((Variant) ref variant8).As<NMultiplayerVoteContainer>();
    Variant variant9;
    if (info.TryGetProperty(NEventOptionButton.PropertyName._animInTween, ref variant9))
      this._animInTween = ((Variant) ref variant9).As<Tween>();
    Variant variant10;
    if (info.TryGetProperty(NEventOptionButton.PropertyName._flashTween, ref variant10))
      this._flashTween = ((Variant) ref variant10).As<Tween>();
    Variant variant11;
    if (info.TryGetProperty(NEventOptionButton.PropertyName._killGlowTween, ref variant11))
      this._killGlowTween = ((Variant) ref variant11).As<Tween>();
    Variant variant12;
    if (info.TryGetProperty(NEventOptionButton.PropertyName._tween, ref variant12))
      this._tween = ((Variant) ref variant12).As<Tween>();
    Variant variant13;
    if (info.TryGetProperty(NEventOptionButton.PropertyName._buttonColor, ref variant13))
      this._buttonColor = ((Variant) ref variant13).As<Color>();
    Variant variant14;
    if (info.TryGetProperty(NEventOptionButton.PropertyName._deathPreventionVfx, ref variant14))
      this._deathPreventionVfx = ((Variant) ref variant14).As<NThoughtBubbleVfx>();
    Variant variant15;
    if (!info.TryGetProperty(NEventOptionButton.PropertyName._deathPreventionVfxPosition, ref variant15))
      return;
    this._deathPreventionVfxPosition = ((Variant) ref variant15).As<Vector2>();
  }

  public new class MethodName : NButton.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName SetVisuallyLocked = StringName.op_Implicit(nameof (SetVisuallyLocked));
    public static readonly StringName AnimateIn = StringName.op_Implicit(nameof (AnimateIn));
    public static readonly StringName EnableButton = StringName.op_Implicit(nameof (EnableButton));
    public new static readonly StringName OnRelease = StringName.op_Implicit(nameof (OnRelease));
    public new static readonly StringName OnPress = StringName.op_Implicit(nameof (OnPress));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
    public static readonly StringName GrayOut = StringName.op_Implicit(nameof (GrayOut));
    public static readonly StringName UpdateShaderParam = StringName.op_Implicit(nameof (UpdateShaderParam));
    public static readonly StringName RefreshVotes = StringName.op_Implicit(nameof (RefreshVotes));
    public new static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName WillKillPlayer = StringName.op_Implicit(nameof (WillKillPlayer));
    public static readonly StringName ShowPersistentKillGlow = StringName.op_Implicit(nameof (ShowPersistentKillGlow));
    public static readonly StringName PulseKillGlow = StringName.op_Implicit(nameof (PulseKillGlow));
  }

  public new class PropertyName : NButton.PropertyName
  {
    public static readonly StringName Index = StringName.op_Implicit(nameof (Index));
    public static readonly StringName VoteContainer = StringName.op_Implicit(nameof (VoteContainer));
    public static readonly StringName _label = StringName.op_Implicit(nameof (_label));
    public static readonly StringName _image = StringName.op_Implicit(nameof (_image));
    public static readonly StringName _outline = StringName.op_Implicit(nameof (_outline));
    public static readonly StringName _killGlow = StringName.op_Implicit(nameof (_killGlow));
    public static readonly StringName _confirmFlash = StringName.op_Implicit(nameof (_confirmFlash));
    public static readonly StringName _hsv = StringName.op_Implicit(nameof (_hsv));
    public static readonly StringName _playerVoteContainer = StringName.op_Implicit(nameof (_playerVoteContainer));
    public static readonly StringName _animInTween = StringName.op_Implicit(nameof (_animInTween));
    public static readonly StringName _flashTween = StringName.op_Implicit(nameof (_flashTween));
    public static readonly StringName _killGlowTween = StringName.op_Implicit(nameof (_killGlowTween));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
    public static readonly StringName _buttonColor = StringName.op_Implicit(nameof (_buttonColor));
    public static readonly StringName _deathPreventionVfx = StringName.op_Implicit(nameof (_deathPreventionVfx));
    public static readonly StringName _deathPreventionVfxPosition = StringName.op_Implicit(nameof (_deathPreventionVfxPosition));
  }

  public new class SignalName : NButton.SignalName
  {
  }
}
