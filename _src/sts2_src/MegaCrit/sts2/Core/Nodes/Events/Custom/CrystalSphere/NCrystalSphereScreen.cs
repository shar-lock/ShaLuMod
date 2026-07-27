// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Events.Custom.CrystalSphere.NCrystalSphereScreen
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Events.Custom.CrystalSphereEvent;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Events.Custom.CrystalSphere;

[ScriptPath("res://src/Core/Nodes/Events/Custom/CrystalSphere/NCrystalSphereScreen.cs")]
public class NCrystalSphereScreen : Control, IOverlayScreen, IScreenContext
{
  private readonly LocString _instructionsTitleLoc = new LocString("events", "CRYSTAL_SPHERE.minigame.instructions.title");
  private readonly LocString _instructionsDescriptionLoc = new LocString("events", "CRYSTAL_SPHERE.minigame.instructions.description");
  private readonly LocString _divinationsRemainLoc = new LocString("events", "CRYSTAL_SPHERE.minigame.divinationsRemain");
  private const string _scenePath = "res://scenes/events/custom/crystal_sphere/crystal_sphere_screen.tscn";
  private CrystalSphereMinigame _entity;
  private Control _itemsContainer;
  private Control _cellContainer;
  private NDivinationButton _bigDivinationButton;
  private NDivinationButton _smallDivinationButton;
  private MegaRichTextLabel _divinationsLeftLabel;
  private NCrystalSphereMask _mask;
  private NProceedButton _proceedButton;
  private MegaRichTextLabel _instructionsTitleLabel;
  private MegaRichTextLabel _instructionsDescriptionLabel;
  private Control _instructionsContainer;
  private NCrystalSphereDialogue _dialogue;
  private Tween? _fadeTween;

  public static NCrystalSphereScreen ShowScreen(CrystalSphereMinigame grid)
  {
    NCrystalSphereScreen screen = PreloadManager.Cache.GetScene("res://scenes/events/custom/crystal_sphere/crystal_sphere_screen.tscn").Instantiate<NCrystalSphereScreen>((PackedScene.GenEditState) 0L);
    screen._entity = grid;
    NOverlayStack.Instance.Push((IOverlayScreen) screen);
    return screen;
  }

  public override void _Ready()
  {
    this._itemsContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%Items"));
    this._cellContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%Cells"));
    this._bigDivinationButton = ((Node) this).GetNode<NDivinationButton>(NodePath.op_Implicit("%BigDivinationButton"));
    this._smallDivinationButton = ((Node) this).GetNode<NDivinationButton>(NodePath.op_Implicit("%SmallDivinationButton"));
    this._bigDivinationButton.SetLabel(new LocString("events", "CRYSTAL_SPHERE.button.DIVINATION_LABEL_BIG"));
    this._smallDivinationButton.SetLabel(new LocString("events", "CRYSTAL_SPHERE.button.DIVINATION_LABEL_SMALL"));
    this._divinationsLeftLabel = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%DivinationsLeft"));
    this._mask = ((Node) this).GetNode<NCrystalSphereMask>(NodePath.op_Implicit("%ScryMask"));
    this._proceedButton = ((Node) this).GetNode<NProceedButton>(NodePath.op_Implicit("%ProceedButton"));
    this._instructionsTitleLabel = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%InstructionsTitle"));
    this._instructionsDescriptionLabel = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%InstructionsDescription"));
    this._instructionsContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%Instructions"));
    this._instructionsTitleLabel.SetTextAutoSize(this._instructionsTitleLoc.GetFormattedText());
    this._instructionsDescriptionLabel.SetTextAutoSize(this._instructionsDescriptionLoc.GetFormattedText());
    this._dialogue = ((Node) this).GetNode<NCrystalSphereDialogue>(NodePath.op_Implicit("%Dialogue"));
    Vector2 vector2 = Vector2.op_Multiply(Vector2.op_Multiply(Vector2.One, (float) -(57 * this._entity.GridSize.X)), 0.5f);
    NCrystalSphereCell[,] ncrystalSphereCellArray = new NCrystalSphereCell[this._entity.GridSize.X, this._entity.GridSize.Y];
    for (int index1 = 0; index1 < this._entity.GridSize.X; ++index1)
    {
      for (int index2 = 0; index2 < this._entity.GridSize.Y; ++index2)
      {
        NCrystalSphereCell cell = NCrystalSphereCell.Create(this._entity.cells[index1, index2], this._mask);
        ((Node) this._cellContainer).AddChildSafely((Node) cell);
        ncrystalSphereCellArray[index1, index2] = cell;
        cell.Position = Vector2.op_Addition(vector2, Vector2.op_Multiply(57f, new Vector2((float) index1, (float) index2)));
        ((GodotObject) cell).Connect(Control.SignalName.MouseEntered, Callable.From((Action) (() => this.OnHoverCell(cell))), 0U);
        ((GodotObject) cell).Connect(Control.SignalName.MouseExited, Callable.From((Action) (() => this.OnUnhoverCell(cell))), 0U);
        ((GodotObject) cell).Connect(Control.SignalName.FocusEntered, Callable.From((Action) (() => this.OnHoverCell(cell))), 0U);
        ((GodotObject) cell).Connect(Control.SignalName.FocusExited, Callable.From((Action) (() => this.OnUnhoverCell(cell))), 0U);
        ((GodotObject) cell).Connect(NClickableControl.SignalName.MouseReleased, Callable.From<InputEvent>((Action<InputEvent>) (_ => TaskHelper.RunSafely(this.OnCellClicked(cell)))), 0U);
        ((GodotObject) cell).Connect(NClickableControl.SignalName.Released, Callable.From<NClickableControl>((Action<NClickableControl>) (_ => TaskHelper.RunSafely(this.OnCellClicked(cell)))), 0U);
      }
    }
    foreach (CrystalSphereItem crystalSphereItem in (IEnumerable<CrystalSphereItem>) this._entity.Items)
    {
      NCrystalSphereItem child = NCrystalSphereItem.Create(crystalSphereItem);
      child.Size = Vector2I.op_Implicit(Vector2I.op_Multiply(crystalSphereItem.Size, 57));
      ((Node) this._itemsContainer).AddChildSafely((Node) child);
      child.Position = Vector2.op_Addition(vector2, Vector2.op_Multiply(57f, new Vector2((float) crystalSphereItem.Position.X, (float) crystalSphereItem.Position.Y)));
      crystalSphereItem.Revealed += new Action<CrystalSphereItem>(this.OnItemRevealed);
    }
    ((GodotObject) this._bigDivinationButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.SetBigDivination)), 0U);
    ((GodotObject) this._smallDivinationButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.SetSmallDivination)), 0U);
    ((GodotObject) this._proceedButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnProceedButtonPressed)), 0U);
    this._smallDivinationButton.SetHotkeys(new string[1]
    {
      StringName.op_Implicit(MegaInput.viewDrawPile)
    });
    this._bigDivinationButton.SetHotkeys(new string[1]
    {
      StringName.op_Implicit(MegaInput.viewDiscardPile)
    });
    this.UpdateDivinationsLeft();
    this._proceedButton.Disable();
    this._proceedButton.UpdateText(NProceedButton.ProceedLoc);
    for (int index3 = 0; index3 < ncrystalSphereCellArray.GetLength(0); ++index3)
    {
      for (int index4 = 0; index4 < ncrystalSphereCellArray.GetLength(1); ++index4)
      {
        Control control = (Control) ncrystalSphereCellArray[index3, index4];
        control.FocusNeighborTop = index4 > 0 ? ((Node) ncrystalSphereCellArray[index3, index4 - 1]).GetPath() : ((Node) ncrystalSphereCellArray[index3, index4]).GetPath();
        control.FocusNeighborBottom = index4 < ncrystalSphereCellArray.GetLength(1) - 1 ? ((Node) ncrystalSphereCellArray[index3, index4 + 1]).GetPath() : ((Node) ncrystalSphereCellArray[index3, index4]).GetPath();
        control.FocusNeighborLeft = index3 > 0 ? ((Node) ncrystalSphereCellArray[index3 - 1, index4]).GetPath() : ((Node) ncrystalSphereCellArray[index3, index4]).GetPath();
        control.FocusNeighborRight = index3 < ncrystalSphereCellArray.GetLength(0) - 1 ? ((Node) ncrystalSphereCellArray[index3 + 1, index4]).GetPath() : ((Node) ncrystalSphereCellArray[index3, index4]).GetPath();
      }
    }
  }

  private void SetBigDivination(NButton obj)
  {
    this._bigDivinationButton.SetActive(true);
    this._smallDivinationButton.SetActive(false);
    this._entity.SetTool(CrystalSphereMinigame.CrystalSphereToolType.Big);
  }

  private void SetSmallDivination(NButton obj)
  {
    this._smallDivinationButton.SetActive(true);
    this._bigDivinationButton.SetActive(false);
    this._entity.SetTool(CrystalSphereMinigame.CrystalSphereToolType.Small);
  }

  public override void _EnterTree()
  {
    this._entity.DivinationCountChanged += new Action(this.UpdateDivinationsLeft);
    this._entity.Finished += new Action(this.OnMinigameFinished);
  }

  public override void _ExitTree()
  {
    this._entity.DivinationCountChanged -= new Action(this.UpdateDivinationsLeft);
    this._entity.Finished -= new Action(this.OnMinigameFinished);
    this._fadeTween?.Kill();
    foreach (CrystalSphereItem crystalSphereItem in (IEnumerable<CrystalSphereItem>) this._entity.Items)
      crystalSphereItem.Revealed -= new Action<CrystalSphereItem>(this.OnItemRevealed);
    this._entity.ForceMinigameEnd();
  }

  private void OnItemRevealed(CrystalSphereItem item)
  {
    if (item.IsGood)
      this._dialogue.PlayGood();
    else
      this._dialogue.PlayBad();
  }

  private async Task OnCellClicked(NCrystalSphereCell cell)
  {
    if (this._entity.DivinationCount <= 0)
      ;
    else
    {
      this.UpdateDivinationsLeft();
      await this._entity.CellClicked(cell.Entity);
      ((IEnumerable) ((Node) this._cellContainer).GetChildren(false)).OfType<NCrystalSphereCell>().Where<NCrystalSphereCell>((Func<NCrystalSphereCell, bool>) (c => c.Entity.IsHidden)).ToList<NCrystalSphereCell>().OrderBy<NCrystalSphereCell, float>((Func<NCrystalSphereCell, float>) (c1 =>
      {
        Vector2I vector2I = new Vector2I(cell.Entity.X, cell.Entity.Y);
        return ((Vector2I) ref vector2I).DistanceTo(new Vector2I(c1.Entity.X, c1.Entity.Y));
      })).First<NCrystalSphereCell>().TryGrabFocus();
    }
  }

  private void OnHoverCell(NCrystalSphereCell cell)
  {
    if (this._entity.IsFinished)
      return;
    this._entity.SetHoveredCell(cell.Entity);
  }

  private void OnUnhoverCell(NCrystalSphereCell cell) => this._entity.UnsetHoveredCell();

  private void UpdateDivinationsLeft()
  {
    this._divinationsRemainLoc.Add("Count", (Decimal) this._entity.DivinationCount);
    this._divinationsLeftLabel.Text = this._divinationsRemainLoc.GetFormattedText() ?? "";
  }

  private void OnMinigameFinished()
  {
    ((CanvasItem) this._bigDivinationButton).Visible = false;
    ((CanvasItem) this._smallDivinationButton).Visible = false;
    ((CanvasItem) this._divinationsLeftLabel).Visible = false;
    ((CanvasItem) this._instructionsContainer).Visible = false;
    ((CanvasItem) this._proceedButton).Visible = true;
    this._dialogue.PlayEnd();
    this._proceedButton.Enable();
    NMapScreen.Instance.SetTravelEnabled(true);
  }

  private void OnProceedButtonPressed(NButton _)
  {
    TaskHelper.RunSafely(RunManager.Instance.ProceedFromTerminalRewardsScreen());
  }

  public NetScreenType ScreenType => NetScreenType.None;

  public bool UseSharedBackstop => false;

  public void AfterOverlayOpened()
  {
    ((CanvasItem) this._itemsContainer).Visible = false;
    Tween fadeTween = this._fadeTween;
    if (fadeTween != null)
      fadeTween.FastForwardToCompletion();
    this._fadeTween = ((Node) this).CreateTween();
    this._fadeTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1.0), 0.5).From(Variant.op_Implicit(0.0f));
    this._fadeTween.Chain().TweenCallback(Callable.From((Action) (() =>
    {
      this._dialogue.PlayStart();
      ((CanvasItem) this._itemsContainer).Visible = true;
    })));
  }

  public void AfterOverlayClosed()
  {
    Tween fadeTween = this._fadeTween;
    if (fadeTween != null)
      fadeTween.FastForwardToCompletion();
    ((Node) this).QueueFreeSafely();
  }

  public void AfterOverlayShown()
  {
    if (!this._entity.IsFinished)
      return;
    this._proceedButton.Enable();
  }

  public void AfterOverlayHidden() => this._proceedButton.Disable();

  public Control DefaultFocusedControl
  {
    get
    {
      List<NCrystalSphereCell> list = ((IEnumerable) ((Node) this._cellContainer).GetChildren(false)).OfType<NCrystalSphereCell>().Where<NCrystalSphereCell>((Func<NCrystalSphereCell, bool>) (c => c.Entity.IsHidden)).ToList<NCrystalSphereCell>();
      return (Control) list[list.Count / 2];
    }
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(14)
    {
      new MethodInfo(NCrystalSphereScreen.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCrystalSphereScreen.MethodName.SetBigDivination, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("obj"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCrystalSphereScreen.MethodName.SetSmallDivination, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("obj"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCrystalSphereScreen.MethodName._EnterTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCrystalSphereScreen.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCrystalSphereScreen.MethodName.OnHoverCell, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("cell"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCrystalSphereScreen.MethodName.OnUnhoverCell, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("cell"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCrystalSphereScreen.MethodName.UpdateDivinationsLeft, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCrystalSphereScreen.MethodName.OnMinigameFinished, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCrystalSphereScreen.MethodName.OnProceedButtonPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCrystalSphereScreen.MethodName.AfterOverlayOpened, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCrystalSphereScreen.MethodName.AfterOverlayClosed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCrystalSphereScreen.MethodName.AfterOverlayShown, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCrystalSphereScreen.MethodName.AfterOverlayHidden, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NCrystalSphereScreen.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCrystalSphereScreen.MethodName.SetBigDivination) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetBigDivination(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCrystalSphereScreen.MethodName.SetSmallDivination) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetSmallDivination(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCrystalSphereScreen.MethodName._EnterTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._EnterTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCrystalSphereScreen.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCrystalSphereScreen.MethodName.OnHoverCell) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnHoverCell(VariantUtils.ConvertTo<NCrystalSphereCell>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCrystalSphereScreen.MethodName.OnUnhoverCell) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnUnhoverCell(VariantUtils.ConvertTo<NCrystalSphereCell>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCrystalSphereScreen.MethodName.UpdateDivinationsLeft) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdateDivinationsLeft();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCrystalSphereScreen.MethodName.OnMinigameFinished) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnMinigameFinished();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCrystalSphereScreen.MethodName.OnProceedButtonPressed) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnProceedButtonPressed(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCrystalSphereScreen.MethodName.AfterOverlayOpened) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AfterOverlayOpened();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCrystalSphereScreen.MethodName.AfterOverlayClosed) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AfterOverlayClosed();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCrystalSphereScreen.MethodName.AfterOverlayShown) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AfterOverlayShown();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NCrystalSphereScreen.MethodName.AfterOverlayHidden) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.AfterOverlayHidden();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NCrystalSphereScreen.MethodName._Ready) || StringName.op_Equality(ref method, NCrystalSphereScreen.MethodName.SetBigDivination) || StringName.op_Equality(ref method, NCrystalSphereScreen.MethodName.SetSmallDivination) || StringName.op_Equality(ref method, NCrystalSphereScreen.MethodName._EnterTree) || StringName.op_Equality(ref method, NCrystalSphereScreen.MethodName._ExitTree) || StringName.op_Equality(ref method, NCrystalSphereScreen.MethodName.OnHoverCell) || StringName.op_Equality(ref method, NCrystalSphereScreen.MethodName.OnUnhoverCell) || StringName.op_Equality(ref method, NCrystalSphereScreen.MethodName.UpdateDivinationsLeft) || StringName.op_Equality(ref method, NCrystalSphereScreen.MethodName.OnMinigameFinished) || StringName.op_Equality(ref method, NCrystalSphereScreen.MethodName.OnProceedButtonPressed) || StringName.op_Equality(ref method, NCrystalSphereScreen.MethodName.AfterOverlayOpened) || StringName.op_Equality(ref method, NCrystalSphereScreen.MethodName.AfterOverlayClosed) || StringName.op_Equality(ref method, NCrystalSphereScreen.MethodName.AfterOverlayShown) || StringName.op_Equality(ref method, NCrystalSphereScreen.MethodName.AfterOverlayHidden) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCrystalSphereScreen.PropertyName._itemsContainer))
    {
      this._itemsContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCrystalSphereScreen.PropertyName._cellContainer))
    {
      this._cellContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCrystalSphereScreen.PropertyName._bigDivinationButton))
    {
      this._bigDivinationButton = VariantUtils.ConvertTo<NDivinationButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCrystalSphereScreen.PropertyName._smallDivinationButton))
    {
      this._smallDivinationButton = VariantUtils.ConvertTo<NDivinationButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCrystalSphereScreen.PropertyName._divinationsLeftLabel))
    {
      this._divinationsLeftLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCrystalSphereScreen.PropertyName._mask))
    {
      this._mask = VariantUtils.ConvertTo<NCrystalSphereMask>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCrystalSphereScreen.PropertyName._proceedButton))
    {
      this._proceedButton = VariantUtils.ConvertTo<NProceedButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCrystalSphereScreen.PropertyName._instructionsTitleLabel))
    {
      this._instructionsTitleLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCrystalSphereScreen.PropertyName._instructionsDescriptionLabel))
    {
      this._instructionsDescriptionLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCrystalSphereScreen.PropertyName._instructionsContainer))
    {
      this._instructionsContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCrystalSphereScreen.PropertyName._dialogue))
    {
      this._dialogue = VariantUtils.ConvertTo<NCrystalSphereDialogue>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCrystalSphereScreen.PropertyName._fadeTween))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._fadeTween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCrystalSphereScreen.PropertyName.ScreenType))
    {
      ref godot_variant local = ref value;
      NetScreenType screenType = this.ScreenType;
      godot_variant from = VariantUtils.CreateFrom<NetScreenType>(ref screenType);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCrystalSphereScreen.PropertyName.UseSharedBackstop))
    {
      ref godot_variant local = ref value;
      bool useSharedBackstop = this.UseSharedBackstop;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref useSharedBackstop);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCrystalSphereScreen.PropertyName.DefaultFocusedControl))
    {
      ref godot_variant local = ref value;
      Control defaultFocusedControl = this.DefaultFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCrystalSphereScreen.PropertyName._itemsContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._itemsContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NCrystalSphereScreen.PropertyName._cellContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._cellContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NCrystalSphereScreen.PropertyName._bigDivinationButton))
    {
      value = VariantUtils.CreateFrom<NDivinationButton>(ref this._bigDivinationButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NCrystalSphereScreen.PropertyName._smallDivinationButton))
    {
      value = VariantUtils.CreateFrom<NDivinationButton>(ref this._smallDivinationButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NCrystalSphereScreen.PropertyName._divinationsLeftLabel))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._divinationsLeftLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NCrystalSphereScreen.PropertyName._mask))
    {
      value = VariantUtils.CreateFrom<NCrystalSphereMask>(ref this._mask);
      return true;
    }
    if (StringName.op_Equality(ref name, NCrystalSphereScreen.PropertyName._proceedButton))
    {
      value = VariantUtils.CreateFrom<NProceedButton>(ref this._proceedButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NCrystalSphereScreen.PropertyName._instructionsTitleLabel))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._instructionsTitleLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NCrystalSphereScreen.PropertyName._instructionsDescriptionLabel))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._instructionsDescriptionLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NCrystalSphereScreen.PropertyName._instructionsContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._instructionsContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NCrystalSphereScreen.PropertyName._dialogue))
    {
      value = VariantUtils.CreateFrom<NCrystalSphereDialogue>(ref this._dialogue);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCrystalSphereScreen.PropertyName._fadeTween))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Tween>(ref this._fadeTween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NCrystalSphereScreen.PropertyName._itemsContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCrystalSphereScreen.PropertyName._cellContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCrystalSphereScreen.PropertyName._bigDivinationButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCrystalSphereScreen.PropertyName._smallDivinationButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCrystalSphereScreen.PropertyName._divinationsLeftLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCrystalSphereScreen.PropertyName._mask, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCrystalSphereScreen.PropertyName._proceedButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCrystalSphereScreen.PropertyName._instructionsTitleLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCrystalSphereScreen.PropertyName._instructionsDescriptionLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCrystalSphereScreen.PropertyName._instructionsContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCrystalSphereScreen.PropertyName._dialogue, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCrystalSphereScreen.PropertyName._fadeTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NCrystalSphereScreen.PropertyName.ScreenType, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NCrystalSphereScreen.PropertyName.UseSharedBackstop, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCrystalSphereScreen.PropertyName.DefaultFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NCrystalSphereScreen.PropertyName._itemsContainer, Variant.From<Control>(ref this._itemsContainer));
    info.AddProperty(NCrystalSphereScreen.PropertyName._cellContainer, Variant.From<Control>(ref this._cellContainer));
    info.AddProperty(NCrystalSphereScreen.PropertyName._bigDivinationButton, Variant.From<NDivinationButton>(ref this._bigDivinationButton));
    info.AddProperty(NCrystalSphereScreen.PropertyName._smallDivinationButton, Variant.From<NDivinationButton>(ref this._smallDivinationButton));
    info.AddProperty(NCrystalSphereScreen.PropertyName._divinationsLeftLabel, Variant.From<MegaRichTextLabel>(ref this._divinationsLeftLabel));
    info.AddProperty(NCrystalSphereScreen.PropertyName._mask, Variant.From<NCrystalSphereMask>(ref this._mask));
    info.AddProperty(NCrystalSphereScreen.PropertyName._proceedButton, Variant.From<NProceedButton>(ref this._proceedButton));
    info.AddProperty(NCrystalSphereScreen.PropertyName._instructionsTitleLabel, Variant.From<MegaRichTextLabel>(ref this._instructionsTitleLabel));
    info.AddProperty(NCrystalSphereScreen.PropertyName._instructionsDescriptionLabel, Variant.From<MegaRichTextLabel>(ref this._instructionsDescriptionLabel));
    info.AddProperty(NCrystalSphereScreen.PropertyName._instructionsContainer, Variant.From<Control>(ref this._instructionsContainer));
    info.AddProperty(NCrystalSphereScreen.PropertyName._dialogue, Variant.From<NCrystalSphereDialogue>(ref this._dialogue));
    info.AddProperty(NCrystalSphereScreen.PropertyName._fadeTween, Variant.From<Tween>(ref this._fadeTween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NCrystalSphereScreen.PropertyName._itemsContainer, ref variant1))
      this._itemsContainer = ((Variant) ref variant1).As<Control>();
    Variant variant2;
    if (info.TryGetProperty(NCrystalSphereScreen.PropertyName._cellContainer, ref variant2))
      this._cellContainer = ((Variant) ref variant2).As<Control>();
    Variant variant3;
    if (info.TryGetProperty(NCrystalSphereScreen.PropertyName._bigDivinationButton, ref variant3))
      this._bigDivinationButton = ((Variant) ref variant3).As<NDivinationButton>();
    Variant variant4;
    if (info.TryGetProperty(NCrystalSphereScreen.PropertyName._smallDivinationButton, ref variant4))
      this._smallDivinationButton = ((Variant) ref variant4).As<NDivinationButton>();
    Variant variant5;
    if (info.TryGetProperty(NCrystalSphereScreen.PropertyName._divinationsLeftLabel, ref variant5))
      this._divinationsLeftLabel = ((Variant) ref variant5).As<MegaRichTextLabel>();
    Variant variant6;
    if (info.TryGetProperty(NCrystalSphereScreen.PropertyName._mask, ref variant6))
      this._mask = ((Variant) ref variant6).As<NCrystalSphereMask>();
    Variant variant7;
    if (info.TryGetProperty(NCrystalSphereScreen.PropertyName._proceedButton, ref variant7))
      this._proceedButton = ((Variant) ref variant7).As<NProceedButton>();
    Variant variant8;
    if (info.TryGetProperty(NCrystalSphereScreen.PropertyName._instructionsTitleLabel, ref variant8))
      this._instructionsTitleLabel = ((Variant) ref variant8).As<MegaRichTextLabel>();
    Variant variant9;
    if (info.TryGetProperty(NCrystalSphereScreen.PropertyName._instructionsDescriptionLabel, ref variant9))
      this._instructionsDescriptionLabel = ((Variant) ref variant9).As<MegaRichTextLabel>();
    Variant variant10;
    if (info.TryGetProperty(NCrystalSphereScreen.PropertyName._instructionsContainer, ref variant10))
      this._instructionsContainer = ((Variant) ref variant10).As<Control>();
    Variant variant11;
    if (info.TryGetProperty(NCrystalSphereScreen.PropertyName._dialogue, ref variant11))
      this._dialogue = ((Variant) ref variant11).As<NCrystalSphereDialogue>();
    Variant variant12;
    if (!info.TryGetProperty(NCrystalSphereScreen.PropertyName._fadeTween, ref variant12))
      return;
    this._fadeTween = ((Variant) ref variant12).As<Tween>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName SetBigDivination = StringName.op_Implicit(nameof (SetBigDivination));
    public static readonly StringName SetSmallDivination = StringName.op_Implicit(nameof (SetSmallDivination));
    public static readonly StringName _EnterTree = StringName.op_Implicit(nameof (_EnterTree));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName OnHoverCell = StringName.op_Implicit(nameof (OnHoverCell));
    public static readonly StringName OnUnhoverCell = StringName.op_Implicit(nameof (OnUnhoverCell));
    public static readonly StringName UpdateDivinationsLeft = StringName.op_Implicit(nameof (UpdateDivinationsLeft));
    public static readonly StringName OnMinigameFinished = StringName.op_Implicit(nameof (OnMinigameFinished));
    public static readonly StringName OnProceedButtonPressed = StringName.op_Implicit(nameof (OnProceedButtonPressed));
    public static readonly StringName AfterOverlayOpened = StringName.op_Implicit(nameof (AfterOverlayOpened));
    public static readonly StringName AfterOverlayClosed = StringName.op_Implicit(nameof (AfterOverlayClosed));
    public static readonly StringName AfterOverlayShown = StringName.op_Implicit(nameof (AfterOverlayShown));
    public static readonly StringName AfterOverlayHidden = StringName.op_Implicit(nameof (AfterOverlayHidden));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName ScreenType = StringName.op_Implicit(nameof (ScreenType));
    public static readonly StringName UseSharedBackstop = StringName.op_Implicit(nameof (UseSharedBackstop));
    public static readonly StringName DefaultFocusedControl = StringName.op_Implicit(nameof (DefaultFocusedControl));
    public static readonly StringName _itemsContainer = StringName.op_Implicit(nameof (_itemsContainer));
    public static readonly StringName _cellContainer = StringName.op_Implicit(nameof (_cellContainer));
    public static readonly StringName _bigDivinationButton = StringName.op_Implicit(nameof (_bigDivinationButton));
    public static readonly StringName _smallDivinationButton = StringName.op_Implicit(nameof (_smallDivinationButton));
    public static readonly StringName _divinationsLeftLabel = StringName.op_Implicit(nameof (_divinationsLeftLabel));
    public static readonly StringName _mask = StringName.op_Implicit(nameof (_mask));
    public static readonly StringName _proceedButton = StringName.op_Implicit(nameof (_proceedButton));
    public static readonly StringName _instructionsTitleLabel = StringName.op_Implicit(nameof (_instructionsTitleLabel));
    public static readonly StringName _instructionsDescriptionLabel = StringName.op_Implicit(nameof (_instructionsDescriptionLabel));
    public static readonly StringName _instructionsContainer = StringName.op_Implicit(nameof (_instructionsContainer));
    public static readonly StringName _dialogue = StringName.op_Implicit(nameof (_dialogue));
    public static readonly StringName _fadeTween = StringName.op_Implicit(nameof (_fadeTween));
  }

  public class SignalName : Control.SignalName
  {
  }
}
