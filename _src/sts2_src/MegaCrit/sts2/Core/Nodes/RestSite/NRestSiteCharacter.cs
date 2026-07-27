// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.RestSite.NRestSiteCharacter
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.RestSite;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.Random;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.RestSite;

[ScriptPath("res://src/Core/Nodes/RestSite/NRestSiteCharacter.cs")]
public class NRestSiteCharacter : Node2D
{
  private static readonly StringName _v = new StringName("v");
  private static readonly StringName _s = new StringName("s");
  private static readonly StringName _noise2Panning = new StringName("Noise2Panning");
  private static readonly StringName _noise1Panning = new StringName("Noise1Panning");
  private static readonly StringName _globalOffset = new StringName("GlobalOffset");
  private static readonly Vector2 _multiplayerConfirmationOffset = new Vector2(-25f, -123f);
  private static readonly Vector2 _multiplayerConfirmationFlipOffset = new Vector2(-155f, 0.0f);
  private static readonly string _multiplayerConfirmationScenePath = SceneHelper.GetScenePath("rest_site/rest_site_multiplayer_confirmation");
  private Control _controlRoot;
  private NSelectionReticle _selectionReticle;
  private Control _leftThoughtAnchor;
  private Control _rightThoughtAnchor;
  private int _characterIndex;
  private NThoughtBubbleVfx? _thoughtBubbleVfx;
  private CancellationTokenSource? _thoughtBubbleGoAwayCancellation;
  private CancellationTokenSource _cts = new CancellationTokenSource();
  private Control? _selectedOptionConfirmation;
  private RestSiteOption? _hoveredRestSiteOption;
  private RestSiteOption? _selectingRestSiteOption;
  private RestSiteOption? _restSiteOptionInThoughtBubble;

  public Control Hitbox { get; private set; }

  public Player Player { get; private set; }

  public static NRestSiteCharacter Create(Player player, int characterIndex)
  {
    NRestSiteCharacter nrestSiteCharacter = PreloadManager.Cache.GetScene(player.Character.RestSiteAnimPath).Instantiate<NRestSiteCharacter>((PackedScene.GenEditState) 0L);
    nrestSiteCharacter.Player = player;
    nrestSiteCharacter._characterIndex = characterIndex;
    return nrestSiteCharacter;
  }

  public override void _EnterTree() => this._cts = new CancellationTokenSource();

  public override void _ExitTree()
  {
    this._thoughtBubbleGoAwayCancellation?.Cancel();
    this._cts.Cancel();
  }

  public override void _Ready()
  {
    this._controlRoot = ((Node) this).GetNode<Control>(NodePath.op_Implicit("ControlRoot"));
    this.Hitbox = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%Hitbox"));
    this._selectionReticle = ((Node) this).GetNode<NSelectionReticle>(NodePath.op_Implicit("%SelectionReticle"));
    this._leftThoughtAnchor = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%ThoughtBubbleLeft"));
    this._rightThoughtAnchor = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%ThoughtBubbleRight"));
    string str;
    switch (this.Player.RunState.CurrentActIndex)
    {
      case 0:
        str = "overgrowth_loop";
        break;
      case 1:
        str = "hive_loop";
        break;
      case 2:
        str = "glory_loop";
        break;
      default:
        throw new InvalidOperationException("Unexpected act");
    }
    string animName = str;
    foreach (GodotObject childSpineNode in this.GetChildSpineNodes())
      ((Node) this).RunWhenSpineReady(new MegaSprite(Variant.op_Implicit(childSpineNode)), (Action<MegaAnimationState>) (animState =>
      {
        animState.SetAnimation(animName);
        using (MegaTrackEntry current = animState.GetCurrent(0))
          current?.SetTrackTime(current.GetAnimationEnd() * Rng.Chaotic.NextFloat());
      }));
    if (this.Player.Character is Necrobinder)
    {
      Sprite2D node1 = ((Node) this).GetNode<Sprite2D>(NodePath.op_Implicit("%NecroFire"));
      Sprite2D node2 = ((Node) this).GetNode<Sprite2D>(NodePath.op_Implicit("%OstyFire"));
      this.RandomizeFire((ShaderMaterial) ((CanvasItem) node1).Material);
      this.RandomizeFire((ShaderMaterial) ((CanvasItem) node2).Material);
      if (this._characterIndex >= 2)
      {
        Node2D node3 = ((Node) this).GetNode<Node2D>(NodePath.op_Implicit("Osty"));
        Node2D node4 = ((Node) this).GetNode<Node2D>(NodePath.op_Implicit("OstyRightAnchor"));
        node3.Position = node4.Position;
        ((Node) this).MoveChildSafely((Node) node3, 0);
      }
    }
    ((GodotObject) this.Hitbox).Connect(Control.SignalName.FocusEntered, Callable.From(new Action(this.OnFocus)), 0U);
    ((GodotObject) this.Hitbox).Connect(Control.SignalName.FocusExited, Callable.From(new Action(this.OnUnfocus)), 0U);
    ((GodotObject) this.Hitbox).Connect(Control.SignalName.MouseEntered, Callable.From(new Action(this.OnFocus)), 0U);
    ((GodotObject) this.Hitbox).Connect(Control.SignalName.MouseExited, Callable.From(new Action(this.OnUnfocus)), 0U);
  }

  private void RandomizeFire(ShaderMaterial mat)
  {
    mat.SetShaderParameter(NRestSiteCharacter._globalOffset, Variant.op_Implicit(new Vector2(Rng.Chaotic.NextFloat(-50f, 50f), Rng.Chaotic.NextFloat(-50f, 50f))));
    ShaderMaterial shaderMaterial1 = mat;
    StringName noise1Panning = NRestSiteCharacter._noise1Panning;
    Variant shaderParameter1 = mat.GetShaderParameter(NRestSiteCharacter._noise1Panning);
    Variant variant1 = Variant.op_Implicit(Vector2.op_Addition(((Variant) ref shaderParameter1).AsVector2(), new Vector2(Rng.Chaotic.NextFloat(-0.1f, 0.1f), Rng.Chaotic.NextFloat(-0.1f, 0.1f))));
    shaderMaterial1.SetShaderParameter(noise1Panning, variant1);
    ShaderMaterial shaderMaterial2 = mat;
    StringName noise2Panning = NRestSiteCharacter._noise2Panning;
    Variant shaderParameter2 = mat.GetShaderParameter(NRestSiteCharacter._noise2Panning);
    Variant variant2 = Variant.op_Implicit(Vector2.op_Addition(((Variant) ref shaderParameter2).AsVector2(), new Vector2(Rng.Chaotic.NextFloat(-0.1f, 0.1f), Rng.Chaotic.NextFloat(-0.1f, 0.1f))));
    shaderMaterial2.SetShaderParameter(noise2Panning, variant2);
  }

  private void OnFocus()
  {
    if (!NTargetManager.Instance.IsInSelection || !NTargetManager.Instance.AllowedToTargetNode((Node) this))
      return;
    NTargetManager.Instance.OnNodeHovered((Node) this);
    this._selectionReticle.OnSelect();
    NRun.Instance.GlobalUi.MultiplayerPlayerContainer.HighlightPlayer(this.Player);
  }

  private void OnUnfocus()
  {
    if (NTargetManager.Instance.IsInSelection && NTargetManager.Instance.AllowedToTargetNode((Node) this))
      NTargetManager.Instance.OnNodeUnhovered((Node) this);
    this.Deselect();
  }

  public void Deselect()
  {
    if (this._selectionReticle.IsSelected)
      this._selectionReticle.OnDeselect();
    NRun.Instance.GlobalUi.MultiplayerPlayerContainer.UnhighlightPlayer(this.Player);
  }

  public void FlipX()
  {
    foreach (Node2D childSpineNode in this.GetChildSpineNodes())
    {
      Node2D node2D1 = childSpineNode;
      Vector2 vector2_1 = childSpineNode.Scale;
      vector2_1.X = -childSpineNode.Scale.X;
      Vector2 vector2_2 = vector2_1;
      node2D1.Scale = vector2_2;
      Node2D node2D2 = childSpineNode;
      vector2_1 = childSpineNode.Position;
      vector2_1.X = -childSpineNode.Position.X;
      Vector2 vector2_3 = vector2_1;
      node2D2.Position = vector2_3;
    }
    Control controlRoot = this._controlRoot;
    Vector2 scale = this._controlRoot.Scale;
    scale.X = -this._controlRoot.Scale.X;
    Vector2 vector2 = scale;
    controlRoot.Scale = vector2;
  }

  public void HideFlameGlow()
  {
    foreach (GodotObject childSpineNode in this.GetChildSpineNodes())
    {
      MegaSprite megaSprite = new MegaSprite(Variant.op_Implicit(childSpineNode));
      if (megaSprite.HasAnimation("_tracks/light_off"))
        megaSprite.GetAnimationState().SetAnimation("_tracks/light_off", trackId: 1);
    }
  }

  public void ShowHoveredRestSiteOption(RestSiteOption? option)
  {
    this._hoveredRestSiteOption = option;
    this.RefreshThoughtBubbleVfx();
  }

  public void SetSelectingRestSiteOption(RestSiteOption? option)
  {
    this._selectingRestSiteOption = option;
    this.RefreshThoughtBubbleVfx();
  }

  private void RefreshThoughtBubbleVfx()
  {
    if (this._selectedOptionConfirmation != null)
      return;
    RestSiteOption restSiteOption1 = this._selectingRestSiteOption;
    if ((object) restSiteOption1 == null)
      restSiteOption1 = this._hoveredRestSiteOption;
    RestSiteOption restSiteOption2 = restSiteOption1;
    if (this._restSiteOptionInThoughtBubble == restSiteOption2)
      return;
    this._restSiteOptionInThoughtBubble = restSiteOption2;
    if (restSiteOption2 == (RestSiteOption) null)
    {
      TaskHelper.RunSafely(this.RemoveThoughtBubbleAfterDelay());
    }
    else
    {
      this._thoughtBubbleGoAwayCancellation?.Cancel();
      if (this._thoughtBubbleVfx == null)
      {
        bool flag1;
        switch (this._characterIndex)
        {
          case 0:
          case 3:
            flag1 = true;
            break;
          default:
            flag1 = false;
            break;
        }
        bool flag2 = flag1;
        this._thoughtBubbleVfx = NThoughtBubbleVfx.Create(restSiteOption2.Icon, flag2 ? DialogueSide.Right : DialogueSide.Left, new double?());
        ShaderMaterial material = (ShaderMaterial) ((CanvasItem) ((Node) this._thoughtBubbleVfx).GetNode<TextureRect>(NodePath.op_Implicit("%Image"))).Material;
        material.SetShaderParameter(NRestSiteCharacter._s, Variant.op_Implicit(0.145f));
        material.SetShaderParameter(NRestSiteCharacter._v, Variant.op_Implicit(0.85f));
        ((Node) this).AddChildSafely((Node) this._thoughtBubbleVfx);
        this._thoughtBubbleVfx.GlobalPosition = this.GetRestSiteOptionAnchor().GlobalPosition;
      }
      else
        this._thoughtBubbleVfx.SetTexture(restSiteOption2.Icon);
    }
  }

  public void ShowSelectedRestSiteOption(RestSiteOption option)
  {
    if (this._thoughtBubbleVfx != null)
      TaskHelper.RunSafely(this._thoughtBubbleVfx.GoAway());
    this._thoughtBubbleVfx = (NThoughtBubbleVfx) null;
    this._selectedOptionConfirmation = PreloadManager.Cache.GetScene(NRestSiteCharacter._multiplayerConfirmationScenePath).Instantiate<Control>((PackedScene.GenEditState) 0L);
    ((Node) this._selectedOptionConfirmation).GetNode<TextureRect>(NodePath.op_Implicit("%Icon")).Texture = option.Icon;
    ((Node) this).AddChildSafely((Node) this._selectedOptionConfirmation);
    bool flag1;
    switch (this._characterIndex)
    {
      case 0:
      case 3:
        flag1 = true;
        break;
      default:
        flag1 = false;
        break;
    }
    bool flag2 = flag1;
    this._selectedOptionConfirmation.GlobalPosition = this.GetRestSiteOptionAnchor().GlobalPosition;
    Control optionConfirmation = this._selectedOptionConfirmation;
    optionConfirmation.Position = Vector2.op_Addition(optionConfirmation.Position, Vector2.op_Addition(NRestSiteCharacter._multiplayerConfirmationOffset, flag2 ? NRestSiteCharacter._multiplayerConfirmationFlipOffset : Vector2.Zero));
  }

  public void Shake() => TaskHelper.RunSafely(this.DoShake());

  private async Task DoShake()
  {
    ScreenPunchInstance shake = new ScreenPunchInstance(15f, 0.4, 0.0f);
    Vector2 originalPosition = this.Position;
    while (!shake.IsDone)
    {
      Vector2 vector2 = shake.Update((double) await ((Node) this).AwaitProcessFrame(this._cts.Token));
      this.Position = Vector2.op_Addition(originalPosition, vector2);
    }
    this.Position = originalPosition;
    shake = (ScreenPunchInstance) null;
  }

  private Control GetRestSiteOptionAnchor()
  {
    return this._characterIndex < 2 ? this._leftThoughtAnchor : this._rightThoughtAnchor;
  }

  private async Task RemoveThoughtBubbleAfterDelay()
  {
    this._thoughtBubbleGoAwayCancellation = new CancellationTokenSource();
    await Cmd.Wait(0.5f, this._thoughtBubbleGoAwayCancellation.Token);
    if (this._thoughtBubbleGoAwayCancellation.IsCancellationRequested)
      return;
    if (this._thoughtBubbleVfx != null)
      TaskHelper.RunSafely(this._thoughtBubbleVfx.GoAway());
    this._thoughtBubbleVfx = (NThoughtBubbleVfx) null;
  }

  private IEnumerable<Node2D> GetChildSpineNodes()
  {
    foreach (Node2D childSpineNode in ((IEnumerable) ((Node) this).GetChildren(false)).OfType<Node2D>())
    {
      if (!(((GodotObject) childSpineNode).GetClass() != "SpineSprite"))
        yield return childSpineNode;
    }
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(12)
    {
      new MethodInfo(NRestSiteCharacter.MethodName._EnterTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRestSiteCharacter.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRestSiteCharacter.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRestSiteCharacter.MethodName.RandomizeFire, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("mat"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("ShaderMaterial"), false)
      }, (List<Variant>) null),
      new MethodInfo(NRestSiteCharacter.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRestSiteCharacter.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRestSiteCharacter.MethodName.Deselect, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRestSiteCharacter.MethodName.FlipX, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRestSiteCharacter.MethodName.HideFlameGlow, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRestSiteCharacter.MethodName.RefreshThoughtBubbleVfx, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRestSiteCharacter.MethodName.Shake, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRestSiteCharacter.MethodName.GetRestSiteOptionAnchor, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NRestSiteCharacter.MethodName._EnterTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._EnterTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRestSiteCharacter.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRestSiteCharacter.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRestSiteCharacter.MethodName.RandomizeFire) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.RandomizeFire(VariantUtils.ConvertTo<ShaderMaterial>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRestSiteCharacter.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRestSiteCharacter.MethodName.OnUnfocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnfocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRestSiteCharacter.MethodName.Deselect) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Deselect();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRestSiteCharacter.MethodName.FlipX) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.FlipX();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRestSiteCharacter.MethodName.HideFlameGlow) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.HideFlameGlow();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRestSiteCharacter.MethodName.RefreshThoughtBubbleVfx) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RefreshThoughtBubbleVfx();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRestSiteCharacter.MethodName.Shake) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Shake();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NRestSiteCharacter.MethodName.GetRestSiteOptionAnchor) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    Control siteOptionAnchor = this.GetRestSiteOptionAnchor();
    ret = VariantUtils.CreateFrom<Control>(ref siteOptionAnchor);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NRestSiteCharacter.MethodName._EnterTree) || StringName.op_Equality(ref method, NRestSiteCharacter.MethodName._ExitTree) || StringName.op_Equality(ref method, NRestSiteCharacter.MethodName._Ready) || StringName.op_Equality(ref method, NRestSiteCharacter.MethodName.RandomizeFire) || StringName.op_Equality(ref method, NRestSiteCharacter.MethodName.OnFocus) || StringName.op_Equality(ref method, NRestSiteCharacter.MethodName.OnUnfocus) || StringName.op_Equality(ref method, NRestSiteCharacter.MethodName.Deselect) || StringName.op_Equality(ref method, NRestSiteCharacter.MethodName.FlipX) || StringName.op_Equality(ref method, NRestSiteCharacter.MethodName.HideFlameGlow) || StringName.op_Equality(ref method, NRestSiteCharacter.MethodName.RefreshThoughtBubbleVfx) || StringName.op_Equality(ref method, NRestSiteCharacter.MethodName.Shake) || StringName.op_Equality(ref method, NRestSiteCharacter.MethodName.GetRestSiteOptionAnchor) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NRestSiteCharacter.PropertyName.Hitbox))
    {
      this.Hitbox = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteCharacter.PropertyName._controlRoot))
    {
      this._controlRoot = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteCharacter.PropertyName._selectionReticle))
    {
      this._selectionReticle = VariantUtils.ConvertTo<NSelectionReticle>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteCharacter.PropertyName._leftThoughtAnchor))
    {
      this._leftThoughtAnchor = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteCharacter.PropertyName._rightThoughtAnchor))
    {
      this._rightThoughtAnchor = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteCharacter.PropertyName._characterIndex))
    {
      this._characterIndex = VariantUtils.ConvertTo<int>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteCharacter.PropertyName._thoughtBubbleVfx))
    {
      this._thoughtBubbleVfx = VariantUtils.ConvertTo<NThoughtBubbleVfx>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NRestSiteCharacter.PropertyName._selectedOptionConfirmation))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._selectedOptionConfirmation = VariantUtils.ConvertTo<Control>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NRestSiteCharacter.PropertyName.Hitbox))
    {
      ref godot_variant local = ref value;
      Control hitbox = this.Hitbox;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref hitbox);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteCharacter.PropertyName._controlRoot))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._controlRoot);
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteCharacter.PropertyName._selectionReticle))
    {
      value = VariantUtils.CreateFrom<NSelectionReticle>(ref this._selectionReticle);
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteCharacter.PropertyName._leftThoughtAnchor))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._leftThoughtAnchor);
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteCharacter.PropertyName._rightThoughtAnchor))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._rightThoughtAnchor);
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteCharacter.PropertyName._characterIndex))
    {
      value = VariantUtils.CreateFrom<int>(ref this._characterIndex);
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteCharacter.PropertyName._thoughtBubbleVfx))
    {
      value = VariantUtils.CreateFrom<NThoughtBubbleVfx>(ref this._thoughtBubbleVfx);
      return true;
    }
    if (!StringName.op_Equality(ref name, NRestSiteCharacter.PropertyName._selectedOptionConfirmation))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Control>(ref this._selectedOptionConfirmation);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NRestSiteCharacter.PropertyName._controlRoot, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRestSiteCharacter.PropertyName.Hitbox, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRestSiteCharacter.PropertyName._selectionReticle, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRestSiteCharacter.PropertyName._leftThoughtAnchor, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRestSiteCharacter.PropertyName._rightThoughtAnchor, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NRestSiteCharacter.PropertyName._characterIndex, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRestSiteCharacter.PropertyName._thoughtBubbleVfx, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRestSiteCharacter.PropertyName._selectedOptionConfirmation, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo = info;
    StringName hitbox1 = NRestSiteCharacter.PropertyName.Hitbox;
    Control hitbox2 = this.Hitbox;
    Variant variant = Variant.From<Control>(ref hitbox2);
    serializationInfo.AddProperty(hitbox1, variant);
    info.AddProperty(NRestSiteCharacter.PropertyName._controlRoot, Variant.From<Control>(ref this._controlRoot));
    info.AddProperty(NRestSiteCharacter.PropertyName._selectionReticle, Variant.From<NSelectionReticle>(ref this._selectionReticle));
    info.AddProperty(NRestSiteCharacter.PropertyName._leftThoughtAnchor, Variant.From<Control>(ref this._leftThoughtAnchor));
    info.AddProperty(NRestSiteCharacter.PropertyName._rightThoughtAnchor, Variant.From<Control>(ref this._rightThoughtAnchor));
    info.AddProperty(NRestSiteCharacter.PropertyName._characterIndex, Variant.From<int>(ref this._characterIndex));
    info.AddProperty(NRestSiteCharacter.PropertyName._thoughtBubbleVfx, Variant.From<NThoughtBubbleVfx>(ref this._thoughtBubbleVfx));
    info.AddProperty(NRestSiteCharacter.PropertyName._selectedOptionConfirmation, Variant.From<Control>(ref this._selectedOptionConfirmation));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NRestSiteCharacter.PropertyName.Hitbox, ref variant1))
      this.Hitbox = ((Variant) ref variant1).As<Control>();
    Variant variant2;
    if (info.TryGetProperty(NRestSiteCharacter.PropertyName._controlRoot, ref variant2))
      this._controlRoot = ((Variant) ref variant2).As<Control>();
    Variant variant3;
    if (info.TryGetProperty(NRestSiteCharacter.PropertyName._selectionReticle, ref variant3))
      this._selectionReticle = ((Variant) ref variant3).As<NSelectionReticle>();
    Variant variant4;
    if (info.TryGetProperty(NRestSiteCharacter.PropertyName._leftThoughtAnchor, ref variant4))
      this._leftThoughtAnchor = ((Variant) ref variant4).As<Control>();
    Variant variant5;
    if (info.TryGetProperty(NRestSiteCharacter.PropertyName._rightThoughtAnchor, ref variant5))
      this._rightThoughtAnchor = ((Variant) ref variant5).As<Control>();
    Variant variant6;
    if (info.TryGetProperty(NRestSiteCharacter.PropertyName._characterIndex, ref variant6))
      this._characterIndex = ((Variant) ref variant6).As<int>();
    Variant variant7;
    if (info.TryGetProperty(NRestSiteCharacter.PropertyName._thoughtBubbleVfx, ref variant7))
      this._thoughtBubbleVfx = ((Variant) ref variant7).As<NThoughtBubbleVfx>();
    Variant variant8;
    if (!info.TryGetProperty(NRestSiteCharacter.PropertyName._selectedOptionConfirmation, ref variant8))
      return;
    this._selectedOptionConfirmation = ((Variant) ref variant8).As<Control>();
  }

  public class MethodName : Node2D.MethodName
  {
    public static readonly StringName _EnterTree = StringName.op_Implicit(nameof (_EnterTree));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName RandomizeFire = StringName.op_Implicit(nameof (RandomizeFire));
    public static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
    public static readonly StringName Deselect = StringName.op_Implicit(nameof (Deselect));
    public static readonly StringName FlipX = StringName.op_Implicit(nameof (FlipX));
    public static readonly StringName HideFlameGlow = StringName.op_Implicit(nameof (HideFlameGlow));
    public static readonly StringName RefreshThoughtBubbleVfx = StringName.op_Implicit(nameof (RefreshThoughtBubbleVfx));
    public static readonly StringName Shake = StringName.op_Implicit(nameof (Shake));
    public static readonly StringName GetRestSiteOptionAnchor = StringName.op_Implicit(nameof (GetRestSiteOptionAnchor));
  }

  public class PropertyName : Node2D.PropertyName
  {
    public static readonly StringName Hitbox = StringName.op_Implicit(nameof (Hitbox));
    public static readonly StringName _controlRoot = StringName.op_Implicit(nameof (_controlRoot));
    public static readonly StringName _selectionReticle = StringName.op_Implicit(nameof (_selectionReticle));
    public static readonly StringName _leftThoughtAnchor = StringName.op_Implicit(nameof (_leftThoughtAnchor));
    public static readonly StringName _rightThoughtAnchor = StringName.op_Implicit(nameof (_rightThoughtAnchor));
    public static readonly StringName _characterIndex = StringName.op_Implicit(nameof (_characterIndex));
    public static readonly StringName _thoughtBubbleVfx = StringName.op_Implicit(nameof (_thoughtBubbleVfx));
    public static readonly StringName _selectedOptionConfirmation = StringName.op_Implicit(nameof (_selectedOptionConfirmation));
  }

  public class SignalName : Node2D.SignalName
  {
  }
}
