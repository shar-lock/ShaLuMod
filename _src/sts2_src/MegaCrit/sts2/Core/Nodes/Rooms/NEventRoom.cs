// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Rooms.NEventRoom
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Ancients;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Events;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Rooms;

[ScriptPath("res://src/Core/Nodes/Rooms/NEventRoom.cs")]
public class NEventRoom : Control, IScreenContext
{
  private readonly CancellationTokenSource _cts = new CancellationTokenSource();
  private EventModel _event;
  private IRunState _runState = (IRunState) NullRunState.Instance;
  private bool _isPreFinished;
  private NSceneContainer _eventContainer;
  private const string _scenePath = "res://scenes/rooms/event_room.tscn";
  private readonly List<EventOption> _connectedOptions = new List<EventOption>();

  public static NEventRoom? Instance => NRun.Instance?.EventRoom;

  public NEventLayout? Layout => this._eventContainer.CurrentScene as NEventLayout;

  public ICustomEventNode? CustomEventNode => this._eventContainer.CurrentScene as ICustomEventNode;

  public NCombatRoom? EmbeddedCombatRoom
  {
    get
    {
      return !(this.Layout is NCombatEventLayout layout) ? (NCombatRoom) null : layout.EmbeddedCombatRoom;
    }
  }

  public Control? VfxContainer { get; private set; }

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>("res://scenes/rooms/event_room.tscn");
    }
  }

  public static NEventRoom? Create(EventModel eventModel, IRunState? runState, bool isPreFinished)
  {
    if (TestMode.IsOn)
      return (NEventRoom) null;
    NEventRoom neventRoom = PreloadManager.Cache.GetScene("res://scenes/rooms/event_room.tscn").Instantiate<NEventRoom>((PackedScene.GenEditState) 0L);
    neventRoom._event = eventModel;
    neventRoom._isPreFinished = isPreFinished;
    if (runState != null)
      neventRoom._runState = runState;
    return neventRoom;
  }

  public override void _Ready()
  {
    if (this._event.Node != null)
      throw new InvalidOperationException("Tried to create event room, but event already has a node!");
    this._eventContainer = ((Node) this).GetNode<NSceneContainer>(NodePath.op_Implicit("%EventContainer"));
    NGame.Instance.SetScreenShakeTarget((Control) this._eventContainer);
    Control node = this._event.CreateScene().Instantiate<Control>((PackedScene.GenEditState) 0L);
    this._event.SetNode(node);
    this._eventContainer.SetCurrentScene(node);
    this.VfxContainer = this.Layout?.VfxContainer;
    TaskHelper.RunSafely(this.SetupLayout());
  }

  public override void _EnterTree()
  {
    ActiveScreenContext.Instance.Updated += new Action(this.OnActiveScreenUpdated);
  }

  public override void _ExitTree()
  {
    this._cts.Cancel();
    NGame.Instance.ClearScreenShakeTarget();
    this._event.StateChanged -= new Action<EventModel>(this.RefreshEventState);
    this._event.EnteringEventCombat -= new Action(this.OnEnteringEventCombat);
    foreach (EventOption connectedOption in this._connectedOptions)
      connectedOption.BeforeChosen -= new Func<EventOption, Task>(this.BeforeOptionChosen);
    this._connectedOptions.Clear();
    ActiveScreenContext.Instance.Updated -= new Action(this.OnActiveScreenUpdated);
  }

  private async Task SetupLayout()
  {
    if (this._event.Owner == null)
      throw new InvalidOperationException("Event must be started before passed to NEventRoom!");
    if (this.Layout == null)
      return;
    this.Layout.SetEvent(this._event);
    this.SetTitle(this._event.Title);
    this._event.StateChanged += new Action<EventModel>(this.RefreshEventState);
    this._event.EnteringEventCombat += new Action(this.OnEnteringEventCombat);
    await Cmd.Wait(0.2f, this._cts.Token);
    this.SetDescription(this.GetDescriptionOrFallback());
    if (this._event is AncientEventModel ancientEventModel && !this._isPreFinished)
    {
      ModelId id = this._event.Owner.Character.Id;
      AncientStats statsForAncient = SaveManager.Instance.Progress.GetStatsForAncient(ancientEventModel.Id);
      int visitsAs = statsForAncient != null ? statsForAncient.GetVisitsAs(id) : 0;
      int totalVisits = statsForAncient != null ? statsForAncient.TotalVisits : 0;
      AncientDialogue ancientDialogue = Rng.Chaotic.NextItem<AncientDialogue>(ancientEventModel.DialogueSet.GetValidDialogues(id, visitsAs, totalVisits, !ancientEventModel.AnyCharacterDialogueBlacklist.Contains<CharacterModel>(this._event.Owner.Character)));
      foreach (AncientDialogueLine line in (IEnumerable<AncientDialogueLine>) ancientDialogue.Lines)
        line.LineText?.Add("Act1Name", this._runState.Acts[0].Title);
      ((NAncientEventLayout) this.Layout).SetDialogue(ancientDialogue.Lines);
    }
    this.SetOptions(this._event);
    this.Layout.OnSetupComplete();
  }

  public void SetPortrait(Texture2D portrait) => this.Layout.SetPortrait(portrait);

  private void SetTitle(LocString title) => this.Layout.SetTitle(title.GetFormattedText());

  private void SetDescription(LocString description)
  {
    if (!description.Exists())
      return;
    this._event.Owner.Character.AddDetailsTo(description);
    description.Add("IsMultiplayer", this._event.Owner.RunState.Players.Count > 1);
    this._event.DynamicVars.AddTo(description);
    this.Layout.SetDescription(description.GetFormattedText());
  }

  private void SetOptions(EventModel eventModel)
  {
    this.Layout.ClearOptions();
    IReadOnlyList<EventOption> options = eventModel.CurrentOptions;
    if (eventModel.IsFinished)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      options = (IReadOnlyList<EventOption>) new \u003C\u003Ez__ReadOnlySingleElementList<EventOption>(new EventOption(eventModel, NEventRoom.\u003C\u003EO.\u003C0\u003E__Proceed ?? (NEventRoom.\u003C\u003EO.\u003C0\u003E__Proceed = new Func<Task>(NEventRoom.Proceed)), "PROCEED", false, true, Array.Empty<IHoverTip>()));
    }
    foreach (EventOption eventOption in (IEnumerable<EventOption>) options)
    {
      eventOption.BeforeChosen += new Func<EventOption, Task>(this.BeforeOptionChosen);
      this._connectedOptions.Add(eventOption);
    }
    this.Layout.AddOptions((IEnumerable<EventOption>) options);
    Control defaultFocusedControl = this.DefaultFocusedControl;
    if (defaultFocusedControl == null)
      return;
    defaultFocusedControl.TryGrabFocus();
  }

  public void OptionButtonClicked(EventOption option, int index)
  {
    if (option.IsLocked)
      return;
    if (option.IsProceed)
    {
      TaskHelper.RunSafely(option.Chosen());
    }
    else
    {
      if (!this._event.IsShared)
        this.Layout.ClearOptions();
      RunManager.Instance.EventSynchronizer.ChooseLocalOption(index);
    }
  }

  private async Task BeforeOptionChosen(EventOption option)
  {
    if (this._event.Owner.RunState.Players.Count > 1 && RunManager.Instance.EventSynchronizer.IsShared && !option.IsProceed)
    {
      await this.Layout.BeforeSharedOptionChosen(option);
    }
    else
    {
      if (option.IsProceed)
        return;
      this.DisableOptionButtons();
    }
  }

  private void RefreshEventState(EventModel eventModel)
  {
    this.SetDescription(this.GetDescriptionOrFallback());
    if (eventModel is AncientEventModel)
      ((NAncientEventLayout) this.Layout).ClearDialogue();
    this.SetOptions(this._event);
  }

  private void DisableOptionButtons() => this.Layout?.DisableEventOptions();

  private void OnEnteringEventCombat()
  {
    this.DisableOptionButtons();
    if (!(this.Layout is NCombatEventLayout layout))
      return;
    layout.HideEventVisuals();
  }

  public static Task Proceed()
  {
    NMapScreen.Instance.SetTravelEnabled(true);
    NMapScreen.Instance.Open();
    return Task.CompletedTask;
  }

  public Control? DefaultFocusedControl
  {
    get
    {
      IScreenContext customEventNode = (IScreenContext) this.CustomEventNode;
      if (customEventNode != null)
        return customEventNode.DefaultFocusedControl;
      return this.Layout?.DefaultFocusedControl;
    }
  }

  private LocString GetDescriptionOrFallback()
  {
    return this._event.Description ?? new LocString("events", "ERROR.description");
  }

  private void OnActiveScreenUpdated() => this.UpdateControllerNavEnabled<NEventRoom>();

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(7)
    {
      new MethodInfo(NEventRoom.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEventRoom.MethodName._EnterTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEventRoom.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEventRoom.MethodName.SetPortrait, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("portrait"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Texture2D"), false)
      }, (List<Variant>) null),
      new MethodInfo(NEventRoom.MethodName.DisableOptionButtons, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEventRoom.MethodName.OnEnteringEventCombat, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEventRoom.MethodName.OnActiveScreenUpdated, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NEventRoom.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEventRoom.MethodName._EnterTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._EnterTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEventRoom.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEventRoom.MethodName.SetPortrait) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetPortrait(VariantUtils.ConvertTo<Texture2D>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEventRoom.MethodName.DisableOptionButtons) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.DisableOptionButtons();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEventRoom.MethodName.OnEnteringEventCombat) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnEnteringEventCombat();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NEventRoom.MethodName.OnActiveScreenUpdated) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.OnActiveScreenUpdated();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NEventRoom.MethodName._Ready) || StringName.op_Equality(ref method, NEventRoom.MethodName._EnterTree) || StringName.op_Equality(ref method, NEventRoom.MethodName._ExitTree) || StringName.op_Equality(ref method, NEventRoom.MethodName.SetPortrait) || StringName.op_Equality(ref method, NEventRoom.MethodName.DisableOptionButtons) || StringName.op_Equality(ref method, NEventRoom.MethodName.OnEnteringEventCombat) || StringName.op_Equality(ref method, NEventRoom.MethodName.OnActiveScreenUpdated) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NEventRoom.PropertyName.VfxContainer))
    {
      this.VfxContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEventRoom.PropertyName._isPreFinished))
    {
      this._isPreFinished = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NEventRoom.PropertyName._eventContainer))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._eventContainer = VariantUtils.ConvertTo<NSceneContainer>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NEventRoom.PropertyName.Layout))
    {
      ref godot_variant local = ref value;
      NEventLayout layout = this.Layout;
      godot_variant from = VariantUtils.CreateFrom<NEventLayout>(ref layout);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NEventRoom.PropertyName.EmbeddedCombatRoom))
    {
      ref godot_variant local = ref value;
      NCombatRoom embeddedCombatRoom = this.EmbeddedCombatRoom;
      godot_variant from = VariantUtils.CreateFrom<NCombatRoom>(ref embeddedCombatRoom);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NEventRoom.PropertyName.VfxContainer))
    {
      ref godot_variant local = ref value;
      Control vfxContainer = this.VfxContainer;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref vfxContainer);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NEventRoom.PropertyName.DefaultFocusedControl))
    {
      ref godot_variant local = ref value;
      Control defaultFocusedControl = this.DefaultFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NEventRoom.PropertyName._isPreFinished))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isPreFinished);
      return true;
    }
    if (!StringName.op_Equality(ref name, NEventRoom.PropertyName._eventContainer))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<NSceneContainer>(ref this._eventContainer);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 1L, NEventRoom.PropertyName._isPreFinished, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEventRoom.PropertyName._eventContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEventRoom.PropertyName.Layout, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEventRoom.PropertyName.EmbeddedCombatRoom, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEventRoom.PropertyName.VfxContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEventRoom.PropertyName.DefaultFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo = info;
    StringName vfxContainer1 = NEventRoom.PropertyName.VfxContainer;
    Control vfxContainer2 = this.VfxContainer;
    Variant variant = Variant.From<Control>(ref vfxContainer2);
    serializationInfo.AddProperty(vfxContainer1, variant);
    info.AddProperty(NEventRoom.PropertyName._isPreFinished, Variant.From<bool>(ref this._isPreFinished));
    info.AddProperty(NEventRoom.PropertyName._eventContainer, Variant.From<NSceneContainer>(ref this._eventContainer));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NEventRoom.PropertyName.VfxContainer, ref variant1))
      this.VfxContainer = ((Variant) ref variant1).As<Control>();
    Variant variant2;
    if (info.TryGetProperty(NEventRoom.PropertyName._isPreFinished, ref variant2))
      this._isPreFinished = ((Variant) ref variant2).As<bool>();
    Variant variant3;
    if (!info.TryGetProperty(NEventRoom.PropertyName._eventContainer, ref variant3))
      return;
    this._eventContainer = ((Variant) ref variant3).As<NSceneContainer>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _EnterTree = StringName.op_Implicit(nameof (_EnterTree));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName SetPortrait = StringName.op_Implicit(nameof (SetPortrait));
    public static readonly StringName DisableOptionButtons = StringName.op_Implicit(nameof (DisableOptionButtons));
    public static readonly StringName OnEnteringEventCombat = StringName.op_Implicit(nameof (OnEnteringEventCombat));
    public static readonly StringName OnActiveScreenUpdated = StringName.op_Implicit(nameof (OnActiveScreenUpdated));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName Layout = StringName.op_Implicit(nameof (Layout));
    public static readonly StringName EmbeddedCombatRoom = StringName.op_Implicit(nameof (EmbeddedCombatRoom));
    public static readonly StringName VfxContainer = StringName.op_Implicit(nameof (VfxContainer));
    public static readonly StringName DefaultFocusedControl = StringName.op_Implicit(nameof (DefaultFocusedControl));
    public static readonly StringName _isPreFinished = StringName.op_Implicit(nameof (_isPreFinished));
    public static readonly StringName _eventContainer = StringName.op_Implicit(nameof (_eventContainer));
  }

  public class SignalName : Control.SignalName
  {
  }
}
