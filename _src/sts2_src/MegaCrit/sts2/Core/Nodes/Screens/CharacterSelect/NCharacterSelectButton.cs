// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect.NCharacterSelectButton
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Unlocks;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect;

[ScriptPath("res://src/Core/Nodes/Screens/CharacterSelect/NCharacterSelectButton.cs")]
public class NCharacterSelectButton : NButton
{
  private static readonly StringName _v = new StringName("v");
  private static readonly StringName _s = new StringName("s");
  private static readonly StringName _h = new StringName("h");
  private static readonly string _playerIconScenePath = SceneHelper.GetScenePath("screens/char_select/char_select_player_icon");
  private static readonly string _unlockedIconPath = ImageHelper.GetImagePath("packed/character_select/char_select_lock3_unlocked.png");
  private TextureRect _icon;
  private TextureRect _iconAdd;
  private TextureRect _lock;
  private Control _outlineLocal;
  private Control _outlineRemote;
  private Control _outlineMixed;
  private Control _shadow;
  private Control _playerIconContainer;
  private CharacterModel _character;
  private ShaderMaterial _hsv;
  private bool _isLocked;
  private static readonly Vector2 _hoverTipOffset = new Vector2(-90f, -180f);
  private ICharacterSelectButtonDelegate? _delegate;
  private Control? _currentOutline;
  private bool _isSelected;
  private readonly HashSet<ulong> _remoteSelectedPlayers = new HashSet<ulong>();
  private NCharacterSelectButton.State _state;
  private static readonly Vector2 _hoverScale = Vector2.op_Multiply(Vector2.One, 1.1f);
  private Tween? _hoverTween;
  private Tween? _hsvTween;
  private const float _unhoverDuration = 0.5f;
  private const float _glowSpeed = 1.6f;
  private const float _selectedSaturation = 1f;
  private const float _selectedValue = 1.1f;
  private const float _remotelySelectedSaturation = 0.8f;
  private const float _remotelySelectedValue = 0.4f;
  private const float _notSelectedSaturation = 0.2f;
  private const float _notSelectedValue = 0.4f;

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlyArray<string>(new string[2]
      {
        NCharacterSelectButton._playerIconScenePath,
        NCharacterSelectButton._unlockedIconPath
      });
    }
  }

  public bool IsRandom { get; private set; }

  public IReadOnlyCollection<ulong> RemoteSelectedPlayers
  {
    get => (IReadOnlyCollection<ulong>) this._remoteSelectedPlayers;
  }

  public CharacterModel Character => this._character;

  public bool IsLocked => this._isLocked;

  public bool IsSelected => this._isSelected;

  public override void _Ready()
  {
    this.ConnectSignals();
    this._icon = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%Icon"));
    this._iconAdd = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%IconAdd"));
    this._lock = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%Lock"));
    this._outlineLocal = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%OutlineLocal"));
    this._outlineRemote = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%OutlineRemote"));
    this._outlineMixed = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%OutlineMixed"));
    this._shadow = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%Shadow"));
    this._playerIconContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%PlayerIconContainer"));
    this._hsv = (ShaderMaterial) ((CanvasItem) this._icon).Material;
    this._hsv.SetShaderParameter(NCharacterSelectButton._s, Variant.op_Implicit(0.2f));
    this._hsv.SetShaderParameter(NCharacterSelectButton._v, Variant.op_Implicit(0.4f));
    ((GodotObject) this).Connect(Control.SignalName.FocusEntered, Callable.From(new Action(this.Select)), 0U);
  }

  public void Init(CharacterModel character, ICharacterSelectButtonDelegate del)
  {
    this._delegate = del;
    this._character = character;
    UnlockState unlockState = SaveManager.Instance.GenerateUnlockStateFromProgress();
    if (character is RandomCharacter)
    {
      this.IsRandom = true;
      this._isLocked = ModelDb.AllCharacters.Any<CharacterModel>((Func<CharacterModel, bool>) (c => !unlockState.Characters.Contains<CharacterModel>(c)));
    }
    else
      this._isLocked = !unlockState.Characters.Contains<CharacterModel>(this._character);
    if (this._isLocked)
    {
      this._icon.Texture = (Texture2D) character.CharacterSelectLockedIcon;
      ((CanvasItem) this._lock).Visible = true;
    }
    else
      this._icon.Texture = (Texture2D) character.CharacterSelectIcon;
  }

  protected override void OnFocus()
  {
    if (this._isSelected)
      return;
    this._hoverTween?.Kill();
    this.Scale = NCharacterSelectButton._hoverScale;
    this._hsv.SetShaderParameter(NCharacterSelectButton._s, Variant.op_Implicit(1f));
    this._hsv.SetShaderParameter(NCharacterSelectButton._v, Variant.op_Implicit(1.1f));
    if (this._isLocked)
      NHoverTipSet.CreateAndShow((Control) this, (IHoverTip) new HoverTip(new LocString("main_menu_ui", "CHARACTER_SELECT.locked.title"), this._character.GetUnlockText()))?.SetGlobalPosition(Vector2.op_Addition(this.GlobalPosition, NCharacterSelectButton._hoverTipOffset), false);
    SfxCmd.Play("event:/sfx/ui/clicks/ui_hover");
  }

  protected override void OnPress()
  {
  }

  protected override void OnUnfocus()
  {
    this._hoverTween?.Kill();
    this._hoverTween = ((Node) this).CreateTween().SetParallel(true);
    this._hoverTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    NHoverTipSet.Remove((Control) this);
    this.AnimateSaturationToCurrentState(this._hoverTween);
  }

  public override void _Process(double delta)
  {
    if (this._currentOutline == null)
      return;
    if (this._isSelected)
    {
      float num = Mathf.Lerp(0.35f, 1f, (float) (((double) Mathf.Cos((float) ((double) Time.GetTicksMsec() * (1.0 / 1000.0) * 1.6000000238418579 * 3.1415927410125732)) + 1.0) * 0.5));
      Control currentOutline = this._currentOutline;
      Color modulate = ((CanvasItem) this._currentOutline).Modulate;
      modulate.A = num;
      Color color = modulate;
      ((CanvasItem) currentOutline).Modulate = color;
    }
    else
    {
      Control currentOutline = this._currentOutline;
      Color modulate = ((CanvasItem) this._currentOutline).Modulate;
      modulate.A = 0.5f;
      Color color = modulate;
      ((CanvasItem) currentOutline).Modulate = color;
    }
  }

  public void LockForAnimation()
  {
    this._icon.Texture = (Texture2D) this._character.CharacterSelectLockedIcon;
    ((CanvasItem) this._lock).Visible = true;
    ((CanvasItem) this).ZIndex = 1;
    ((CanvasItem) this._lock).Modulate = Colors.White;
    this.Disable();
  }

  public async Task AnimateUnlock()
  {
    GpuParticles2D chargeParticles = ((Node) this).GetNode<GpuParticles2D>(NodePath.op_Implicit("%UnlockChargeParticles"));
    chargeParticles.Emitting = true;
    float num1 = 1f;
    Vector2 originalLockPosition = ((Control) this._lock).Position;
    float p = 0.0f;
    NDebugAudioManager.Instance.Play("character_unlock_charge.mp3");
    while ((double) p < 1.0)
    {
      Vector2 right = Vector2.Right;
      Vector2 vector2 = Vector2.op_Multiply(((Vector2) ref right).Rotated(Rng.Chaotic.NextFloat(6.28318548f)), num1);
      ((Control) this._lock).Position = Vector2.op_Addition(originalLockPosition, vector2);
      float num = p;
      p = num + await ((Node) this).AwaitProcessFrame();
      num1 = Mathf.Lerp(1f, 5f, Ease.QuadOut(p));
    }
    NDebugAudioManager.Instance.Play("character_unlock.mp3");
    ((Control) this._lock).Position = originalLockPosition;
    this._lock.Texture = PreloadManager.Cache.GetTexture2D(NCharacterSelectButton._unlockedIconPath);
    this._icon.Texture = (Texture2D) this._character.CharacterSelectIcon;
    this._iconAdd.Texture = this._icon.Texture;
    ((CanvasItem) this._iconAdd).Visible = true;
    ((Node) this).GetNode<GpuParticles2D>(NodePath.op_Implicit("%UnlockParticles")).Emitting = true;
    chargeParticles.Emitting = false;
    Tween tween = ((Node) this).CreateTween();
    tween.SetParallel(true);
    tween.TweenProperty((GodotObject) this._iconAdd, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 1.5f)), 1.0);
    tween.TweenProperty((GodotObject) this._iconAdd, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 1.0).SetEase((Tween.EaseType) 0L).SetTrans((Tween.TransitionType) 5L);
    tween.TweenProperty((GodotObject) this._lock, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.5).SetDelay(0.5);
    ((CanvasItem) this).ZIndex = 0;
    this.Enable();
    chargeParticles = (GpuParticles2D) null;
  }

  public void Reset()
  {
    foreach (Node child in ((Node) this._playerIconContainer).GetChildren(false))
      child.QueueFreeSafely();
    this._remoteSelectedPlayers.Clear();
    this.Deselect();
  }

  public void OnRemotePlayerSelected(ulong playerId)
  {
    this._remoteSelectedPlayers.Add(playerId);
    this.RefreshState();
  }

  public void OnRemotePlayerDeselected(ulong playerId)
  {
    this._remoteSelectedPlayers.Remove(playerId);
    this.RefreshState();
  }

  public void Select()
  {
    if (this._isSelected)
      return;
    this._hoverTween?.Kill();
    this._isSelected = true;
    this._delegate.SelectCharacter(this, this._character);
    this.RefreshState();
  }

  public void Deselect()
  {
    this._isSelected = false;
    this.RefreshState();
  }

  private void RefreshState()
  {
    NCharacterSelectButton.State state1 = !this._isSelected ? (this._remoteSelectedPlayers.Count <= 0 ? NCharacterSelectButton.State.NotSelected : NCharacterSelectButton.State.SelectedRemotely) : NCharacterSelectButton.State.SelectedLocally;
    NCharacterSelectButton.State state2 = this._state;
    if (state2 != state1)
    {
      this._state = state1;
      if (state2 == NCharacterSelectButton.State.NotSelected)
      {
        this._hsv.SetShaderParameter(NCharacterSelectButton._s, Variant.op_Implicit(this.GetSaturationForCurrentState()));
        this._hsv.SetShaderParameter(NCharacterSelectButton._v, Variant.op_Implicit(this.GetValueForCurrentState()));
      }
      else
      {
        this._hoverTween?.Kill();
        this._hoverTween = ((Node) this).CreateTween().SetParallel(true);
        this.AnimateSaturationToCurrentState(this._hoverTween);
      }
    }
    this.RefreshOutline();
    this.RefreshPlayerIcons();
  }

  private float GetSaturationForCurrentState()
  {
    switch (this._state)
    {
      case NCharacterSelectButton.State.NotSelected:
        return 0.2f;
      case NCharacterSelectButton.State.SelectedLocally:
        return 1f;
      case NCharacterSelectButton.State.SelectedRemotely:
        return 0.8f;
      default:
        throw new ArgumentOutOfRangeException();
    }
  }

  private float GetValueForCurrentState()
  {
    switch (this._state)
    {
      case NCharacterSelectButton.State.NotSelected:
        return 0.8f;
      case NCharacterSelectButton.State.SelectedLocally:
        return 1.1f;
      case NCharacterSelectButton.State.SelectedRemotely:
        return 0.4f;
      default:
        throw new ArgumentOutOfRangeException();
    }
  }

  private void AnimateSaturationToCurrentState(Tween tween)
  {
    tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderS)), this._hsv.GetShaderParameter(NCharacterSelectButton._s), Variant.op_Implicit(this.GetSaturationForCurrentState()), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NCharacterSelectButton._v), Variant.op_Implicit(this.GetValueForCurrentState()), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
  }

  private void RefreshOutline()
  {
    if (this._currentOutline != null)
      ((CanvasItem) this._currentOutline).Visible = false;
    this._currentOutline = !this._isSelected || this._remoteSelectedPlayers.Count <= 0 ? (!this._isSelected ? (this._remoteSelectedPlayers.Count <= 0 ? (Control) null : this._outlineRemote) : this._outlineLocal) : this._outlineMixed;
    if (this._currentOutline == null)
      return;
    ((CanvasItem) this._currentOutline).Visible = true;
  }

  private void RefreshPlayerIcons()
  {
    if (this._delegate == null || this._delegate.Lobby.NetService.Type == NetGameType.Singleplayer)
      return;
    int num = this._remoteSelectedPlayers.Count + (this._isSelected ? 1 : 0);
    for (int childCount = ((Node) this._playerIconContainer).GetChildCount(false); childCount < num; ++childCount)
      ((Node) this._playerIconContainer).AddChildSafely((Node) PreloadManager.Cache.GetScene(NCharacterSelectButton._playerIconScenePath).Instantiate<TextureRect>((PackedScene.GenEditState) 0L));
    while (((Node) this._playerIconContainer).GetChildCount(false) > num)
    {
      Control child = ((Node) this._playerIconContainer).GetChild<Control>(0, false);
      ((Node) this._playerIconContainer).RemoveChildSafely((Node) child);
      ((Node) child).QueueFreeSafely();
    }
    for (int index = 0; index < ((Node) this._playerIconContainer).GetChildCount(false); ++index)
      ((CanvasItem) ((Node) this._playerIconContainer).GetChild<Control>(index, false)).Modulate = !this._isSelected || index != 0 ? StsColors.blue : StsColors.gold;
  }

  public void DebugUnlock()
  {
    this._icon.Texture = (Texture2D) this._character.CharacterSelectIcon;
    this._isLocked = false;
    ((CanvasItem) this._lock).Visible = false;
    this.Enable();
  }

  public void UnlockIfPossible()
  {
    if (!SaveManager.Instance.GenerateUnlockStateFromProgress().Characters.Contains<CharacterModel>(this._character))
      return;
    this._icon.Texture = (Texture2D) this._character.CharacterSelectIcon;
    this._isLocked = false;
    ((CanvasItem) this._lock).Visible = false;
    this.Enable();
  }

  private void UpdateShaderH(float value)
  {
    this._hsv.SetShaderParameter(NCharacterSelectButton._h, Variant.op_Implicit(value));
  }

  private void UpdateShaderS(float value)
  {
    this._hsv.SetShaderParameter(NCharacterSelectButton._s, Variant.op_Implicit(value));
  }

  private void UpdateShaderV(float value)
  {
    this._hsv.SetShaderParameter(NCharacterSelectButton._v, Variant.op_Implicit(value));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(22)
    {
      new MethodInfo(NCharacterSelectButton.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCharacterSelectButton.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCharacterSelectButton.MethodName.OnPress, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCharacterSelectButton.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCharacterSelectButton.MethodName._Process, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("delta"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCharacterSelectButton.MethodName.LockForAnimation, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCharacterSelectButton.MethodName.Reset, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCharacterSelectButton.MethodName.OnRemotePlayerSelected, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("playerId"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCharacterSelectButton.MethodName.OnRemotePlayerDeselected, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("playerId"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCharacterSelectButton.MethodName.Select, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCharacterSelectButton.MethodName.Deselect, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCharacterSelectButton.MethodName.RefreshState, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCharacterSelectButton.MethodName.GetSaturationForCurrentState, new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCharacterSelectButton.MethodName.GetValueForCurrentState, new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCharacterSelectButton.MethodName.AnimateSaturationToCurrentState, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("tween"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Tween"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCharacterSelectButton.MethodName.RefreshOutline, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCharacterSelectButton.MethodName.RefreshPlayerIcons, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCharacterSelectButton.MethodName.DebugUnlock, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCharacterSelectButton.MethodName.UnlockIfPossible, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCharacterSelectButton.MethodName.UpdateShaderH, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("value"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCharacterSelectButton.MethodName.UpdateShaderS, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("value"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCharacterSelectButton.MethodName.UpdateShaderV, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
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
    if (StringName.op_Equality(ref method, NCharacterSelectButton.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCharacterSelectButton.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCharacterSelectButton.MethodName.OnPress) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnPress();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCharacterSelectButton.MethodName.OnUnfocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnfocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCharacterSelectButton.MethodName._Process) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Node) this)._Process(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCharacterSelectButton.MethodName.LockForAnimation) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.LockForAnimation();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCharacterSelectButton.MethodName.Reset) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Reset();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCharacterSelectButton.MethodName.OnRemotePlayerSelected) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnRemotePlayerSelected(VariantUtils.ConvertTo<ulong>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCharacterSelectButton.MethodName.OnRemotePlayerDeselected) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnRemotePlayerDeselected(VariantUtils.ConvertTo<ulong>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCharacterSelectButton.MethodName.Select) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Select();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCharacterSelectButton.MethodName.Deselect) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Deselect();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCharacterSelectButton.MethodName.RefreshState) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RefreshState();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCharacterSelectButton.MethodName.GetSaturationForCurrentState) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      float saturationForCurrentState = this.GetSaturationForCurrentState();
      ret = VariantUtils.CreateFrom<float>(ref saturationForCurrentState);
      return true;
    }
    if (StringName.op_Equality(ref method, NCharacterSelectButton.MethodName.GetValueForCurrentState) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      float valueForCurrentState = this.GetValueForCurrentState();
      ret = VariantUtils.CreateFrom<float>(ref valueForCurrentState);
      return true;
    }
    if (StringName.op_Equality(ref method, NCharacterSelectButton.MethodName.AnimateSaturationToCurrentState) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.AnimateSaturationToCurrentState(VariantUtils.ConvertTo<Tween>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCharacterSelectButton.MethodName.RefreshOutline) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RefreshOutline();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCharacterSelectButton.MethodName.RefreshPlayerIcons) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RefreshPlayerIcons();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCharacterSelectButton.MethodName.DebugUnlock) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.DebugUnlock();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCharacterSelectButton.MethodName.UnlockIfPossible) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UnlockIfPossible();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCharacterSelectButton.MethodName.UpdateShaderH) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.UpdateShaderH(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCharacterSelectButton.MethodName.UpdateShaderS) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.UpdateShaderS(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NCharacterSelectButton.MethodName.UpdateShaderV) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.UpdateShaderV(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NCharacterSelectButton.MethodName._Ready) || StringName.op_Equality(ref method, NCharacterSelectButton.MethodName.OnFocus) || StringName.op_Equality(ref method, NCharacterSelectButton.MethodName.OnPress) || StringName.op_Equality(ref method, NCharacterSelectButton.MethodName.OnUnfocus) || StringName.op_Equality(ref method, NCharacterSelectButton.MethodName._Process) || StringName.op_Equality(ref method, NCharacterSelectButton.MethodName.LockForAnimation) || StringName.op_Equality(ref method, NCharacterSelectButton.MethodName.Reset) || StringName.op_Equality(ref method, NCharacterSelectButton.MethodName.OnRemotePlayerSelected) || StringName.op_Equality(ref method, NCharacterSelectButton.MethodName.OnRemotePlayerDeselected) || StringName.op_Equality(ref method, NCharacterSelectButton.MethodName.Select) || StringName.op_Equality(ref method, NCharacterSelectButton.MethodName.Deselect) || StringName.op_Equality(ref method, NCharacterSelectButton.MethodName.RefreshState) || StringName.op_Equality(ref method, NCharacterSelectButton.MethodName.GetSaturationForCurrentState) || StringName.op_Equality(ref method, NCharacterSelectButton.MethodName.GetValueForCurrentState) || StringName.op_Equality(ref method, NCharacterSelectButton.MethodName.AnimateSaturationToCurrentState) || StringName.op_Equality(ref method, NCharacterSelectButton.MethodName.RefreshOutline) || StringName.op_Equality(ref method, NCharacterSelectButton.MethodName.RefreshPlayerIcons) || StringName.op_Equality(ref method, NCharacterSelectButton.MethodName.DebugUnlock) || StringName.op_Equality(ref method, NCharacterSelectButton.MethodName.UnlockIfPossible) || StringName.op_Equality(ref method, NCharacterSelectButton.MethodName.UpdateShaderH) || StringName.op_Equality(ref method, NCharacterSelectButton.MethodName.UpdateShaderS) || StringName.op_Equality(ref method, NCharacterSelectButton.MethodName.UpdateShaderV) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCharacterSelectButton.PropertyName.IsRandom))
    {
      this.IsRandom = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectButton.PropertyName._icon))
    {
      this._icon = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectButton.PropertyName._iconAdd))
    {
      this._iconAdd = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectButton.PropertyName._lock))
    {
      this._lock = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectButton.PropertyName._outlineLocal))
    {
      this._outlineLocal = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectButton.PropertyName._outlineRemote))
    {
      this._outlineRemote = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectButton.PropertyName._outlineMixed))
    {
      this._outlineMixed = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectButton.PropertyName._shadow))
    {
      this._shadow = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectButton.PropertyName._playerIconContainer))
    {
      this._playerIconContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectButton.PropertyName._hsv))
    {
      this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectButton.PropertyName._isLocked))
    {
      this._isLocked = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectButton.PropertyName._currentOutline))
    {
      this._currentOutline = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectButton.PropertyName._isSelected))
    {
      this._isSelected = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectButton.PropertyName._state))
    {
      this._state = VariantUtils.ConvertTo<NCharacterSelectButton.State>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectButton.PropertyName._hoverTween))
    {
      this._hoverTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCharacterSelectButton.PropertyName._hsvTween))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._hsvTween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCharacterSelectButton.PropertyName.IsRandom))
    {
      ref godot_variant local = ref value;
      bool isRandom = this.IsRandom;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref isRandom);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectButton.PropertyName.IsLocked))
    {
      ref godot_variant local = ref value;
      bool isLocked = this.IsLocked;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref isLocked);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectButton.PropertyName.IsSelected))
    {
      ref godot_variant local = ref value;
      bool isSelected = this.IsSelected;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref isSelected);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectButton.PropertyName._icon))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._icon);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectButton.PropertyName._iconAdd))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._iconAdd);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectButton.PropertyName._lock))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._lock);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectButton.PropertyName._outlineLocal))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._outlineLocal);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectButton.PropertyName._outlineRemote))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._outlineRemote);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectButton.PropertyName._outlineMixed))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._outlineMixed);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectButton.PropertyName._shadow))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._shadow);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectButton.PropertyName._playerIconContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._playerIconContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectButton.PropertyName._hsv))
    {
      value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._hsv);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectButton.PropertyName._isLocked))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isLocked);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectButton.PropertyName._currentOutline))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._currentOutline);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectButton.PropertyName._isSelected))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isSelected);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectButton.PropertyName._state))
    {
      value = VariantUtils.CreateFrom<NCharacterSelectButton.State>(ref this._state);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectButton.PropertyName._hoverTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._hoverTween);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCharacterSelectButton.PropertyName._hsvTween))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<Tween>(ref this._hsvTween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NCharacterSelectButton.PropertyName._icon, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCharacterSelectButton.PropertyName._iconAdd, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCharacterSelectButton.PropertyName._lock, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCharacterSelectButton.PropertyName._outlineLocal, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCharacterSelectButton.PropertyName._outlineRemote, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCharacterSelectButton.PropertyName._outlineMixed, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCharacterSelectButton.PropertyName._shadow, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCharacterSelectButton.PropertyName._playerIconContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCharacterSelectButton.PropertyName._hsv, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NCharacterSelectButton.PropertyName._isLocked, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NCharacterSelectButton.PropertyName.IsRandom, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCharacterSelectButton.PropertyName._currentOutline, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NCharacterSelectButton.PropertyName._isSelected, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NCharacterSelectButton.PropertyName._state, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCharacterSelectButton.PropertyName._hoverTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCharacterSelectButton.PropertyName._hsvTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NCharacterSelectButton.PropertyName.IsLocked, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NCharacterSelectButton.PropertyName.IsSelected, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo = info;
    StringName isRandom1 = NCharacterSelectButton.PropertyName.IsRandom;
    bool isRandom2 = this.IsRandom;
    Variant variant = Variant.From<bool>(ref isRandom2);
    serializationInfo.AddProperty(isRandom1, variant);
    info.AddProperty(NCharacterSelectButton.PropertyName._icon, Variant.From<TextureRect>(ref this._icon));
    info.AddProperty(NCharacterSelectButton.PropertyName._iconAdd, Variant.From<TextureRect>(ref this._iconAdd));
    info.AddProperty(NCharacterSelectButton.PropertyName._lock, Variant.From<TextureRect>(ref this._lock));
    info.AddProperty(NCharacterSelectButton.PropertyName._outlineLocal, Variant.From<Control>(ref this._outlineLocal));
    info.AddProperty(NCharacterSelectButton.PropertyName._outlineRemote, Variant.From<Control>(ref this._outlineRemote));
    info.AddProperty(NCharacterSelectButton.PropertyName._outlineMixed, Variant.From<Control>(ref this._outlineMixed));
    info.AddProperty(NCharacterSelectButton.PropertyName._shadow, Variant.From<Control>(ref this._shadow));
    info.AddProperty(NCharacterSelectButton.PropertyName._playerIconContainer, Variant.From<Control>(ref this._playerIconContainer));
    info.AddProperty(NCharacterSelectButton.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
    info.AddProperty(NCharacterSelectButton.PropertyName._isLocked, Variant.From<bool>(ref this._isLocked));
    info.AddProperty(NCharacterSelectButton.PropertyName._currentOutline, Variant.From<Control>(ref this._currentOutline));
    info.AddProperty(NCharacterSelectButton.PropertyName._isSelected, Variant.From<bool>(ref this._isSelected));
    info.AddProperty(NCharacterSelectButton.PropertyName._state, Variant.From<NCharacterSelectButton.State>(ref this._state));
    info.AddProperty(NCharacterSelectButton.PropertyName._hoverTween, Variant.From<Tween>(ref this._hoverTween));
    info.AddProperty(NCharacterSelectButton.PropertyName._hsvTween, Variant.From<Tween>(ref this._hsvTween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NCharacterSelectButton.PropertyName.IsRandom, ref variant1))
      this.IsRandom = ((Variant) ref variant1).As<bool>();
    Variant variant2;
    if (info.TryGetProperty(NCharacterSelectButton.PropertyName._icon, ref variant2))
      this._icon = ((Variant) ref variant2).As<TextureRect>();
    Variant variant3;
    if (info.TryGetProperty(NCharacterSelectButton.PropertyName._iconAdd, ref variant3))
      this._iconAdd = ((Variant) ref variant3).As<TextureRect>();
    Variant variant4;
    if (info.TryGetProperty(NCharacterSelectButton.PropertyName._lock, ref variant4))
      this._lock = ((Variant) ref variant4).As<TextureRect>();
    Variant variant5;
    if (info.TryGetProperty(NCharacterSelectButton.PropertyName._outlineLocal, ref variant5))
      this._outlineLocal = ((Variant) ref variant5).As<Control>();
    Variant variant6;
    if (info.TryGetProperty(NCharacterSelectButton.PropertyName._outlineRemote, ref variant6))
      this._outlineRemote = ((Variant) ref variant6).As<Control>();
    Variant variant7;
    if (info.TryGetProperty(NCharacterSelectButton.PropertyName._outlineMixed, ref variant7))
      this._outlineMixed = ((Variant) ref variant7).As<Control>();
    Variant variant8;
    if (info.TryGetProperty(NCharacterSelectButton.PropertyName._shadow, ref variant8))
      this._shadow = ((Variant) ref variant8).As<Control>();
    Variant variant9;
    if (info.TryGetProperty(NCharacterSelectButton.PropertyName._playerIconContainer, ref variant9))
      this._playerIconContainer = ((Variant) ref variant9).As<Control>();
    Variant variant10;
    if (info.TryGetProperty(NCharacterSelectButton.PropertyName._hsv, ref variant10))
      this._hsv = ((Variant) ref variant10).As<ShaderMaterial>();
    Variant variant11;
    if (info.TryGetProperty(NCharacterSelectButton.PropertyName._isLocked, ref variant11))
      this._isLocked = ((Variant) ref variant11).As<bool>();
    Variant variant12;
    if (info.TryGetProperty(NCharacterSelectButton.PropertyName._currentOutline, ref variant12))
      this._currentOutline = ((Variant) ref variant12).As<Control>();
    Variant variant13;
    if (info.TryGetProperty(NCharacterSelectButton.PropertyName._isSelected, ref variant13))
      this._isSelected = ((Variant) ref variant13).As<bool>();
    Variant variant14;
    if (info.TryGetProperty(NCharacterSelectButton.PropertyName._state, ref variant14))
      this._state = ((Variant) ref variant14).As<NCharacterSelectButton.State>();
    Variant variant15;
    if (info.TryGetProperty(NCharacterSelectButton.PropertyName._hoverTween, ref variant15))
      this._hoverTween = ((Variant) ref variant15).As<Tween>();
    Variant variant16;
    if (!info.TryGetProperty(NCharacterSelectButton.PropertyName._hsvTween, ref variant16))
      return;
    this._hsvTween = ((Variant) ref variant16).As<Tween>();
  }

  private enum State
  {
    NotSelected,
    SelectedLocally,
    SelectedRemotely,
  }

  public new class MethodName : NButton.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnPress = StringName.op_Implicit(nameof (OnPress));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
    public static readonly StringName _Process = StringName.op_Implicit(nameof (_Process));
    public static readonly StringName LockForAnimation = StringName.op_Implicit(nameof (LockForAnimation));
    public static readonly StringName Reset = StringName.op_Implicit(nameof (Reset));
    public static readonly StringName OnRemotePlayerSelected = StringName.op_Implicit(nameof (OnRemotePlayerSelected));
    public static readonly StringName OnRemotePlayerDeselected = StringName.op_Implicit(nameof (OnRemotePlayerDeselected));
    public static readonly StringName Select = StringName.op_Implicit(nameof (Select));
    public static readonly StringName Deselect = StringName.op_Implicit(nameof (Deselect));
    public static readonly StringName RefreshState = StringName.op_Implicit(nameof (RefreshState));
    public static readonly StringName GetSaturationForCurrentState = StringName.op_Implicit(nameof (GetSaturationForCurrentState));
    public static readonly StringName GetValueForCurrentState = StringName.op_Implicit(nameof (GetValueForCurrentState));
    public static readonly StringName AnimateSaturationToCurrentState = StringName.op_Implicit(nameof (AnimateSaturationToCurrentState));
    public static readonly StringName RefreshOutline = StringName.op_Implicit(nameof (RefreshOutline));
    public static readonly StringName RefreshPlayerIcons = StringName.op_Implicit(nameof (RefreshPlayerIcons));
    public static readonly StringName DebugUnlock = StringName.op_Implicit(nameof (DebugUnlock));
    public static readonly StringName UnlockIfPossible = StringName.op_Implicit(nameof (UnlockIfPossible));
    public static readonly StringName UpdateShaderH = StringName.op_Implicit(nameof (UpdateShaderH));
    public static readonly StringName UpdateShaderS = StringName.op_Implicit(nameof (UpdateShaderS));
    public static readonly StringName UpdateShaderV = StringName.op_Implicit(nameof (UpdateShaderV));
  }

  public new class PropertyName : NButton.PropertyName
  {
    public static readonly StringName IsRandom = StringName.op_Implicit(nameof (IsRandom));
    public static readonly StringName IsLocked = StringName.op_Implicit(nameof (IsLocked));
    public static readonly StringName IsSelected = StringName.op_Implicit(nameof (IsSelected));
    public static readonly StringName _icon = StringName.op_Implicit(nameof (_icon));
    public static readonly StringName _iconAdd = StringName.op_Implicit(nameof (_iconAdd));
    public static readonly StringName _lock = StringName.op_Implicit(nameof (_lock));
    public static readonly StringName _outlineLocal = StringName.op_Implicit(nameof (_outlineLocal));
    public static readonly StringName _outlineRemote = StringName.op_Implicit(nameof (_outlineRemote));
    public static readonly StringName _outlineMixed = StringName.op_Implicit(nameof (_outlineMixed));
    public static readonly StringName _shadow = StringName.op_Implicit(nameof (_shadow));
    public static readonly StringName _playerIconContainer = StringName.op_Implicit(nameof (_playerIconContainer));
    public static readonly StringName _hsv = StringName.op_Implicit(nameof (_hsv));
    public static readonly StringName _isLocked = StringName.op_Implicit(nameof (_isLocked));
    public static readonly StringName _currentOutline = StringName.op_Implicit(nameof (_currentOutline));
    public static readonly StringName _isSelected = StringName.op_Implicit(nameof (_isSelected));
    public static readonly StringName _state = StringName.op_Implicit(nameof (_state));
    public static readonly StringName _hoverTween = StringName.op_Implicit(nameof (_hoverTween));
    public static readonly StringName _hsvTween = StringName.op_Implicit(nameof (_hsvTween));
  }

  public new class SignalName : NButton.SignalName
  {
  }
}
