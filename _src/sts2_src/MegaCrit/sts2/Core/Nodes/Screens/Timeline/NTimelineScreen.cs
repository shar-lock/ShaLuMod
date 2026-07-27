// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Timeline.NTimelineScreen
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Achievements;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Nodes.Screens.Timeline.UnlockScreens;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.TestSupport;
using MegaCrit.Sts2.Core.Timeline;
using MegaCrit.Sts2.Core.Timeline.Epochs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Timeline;

[ScriptPath("res://src/Core/Nodes/Screens/Timeline/NTimelineScreen.cs")]
public class NTimelineScreen : NSubmenu
{
  private static readonly string _scenePath = SceneHelper.GetScenePath("timeline_screen/timeline_screen");
  private const string _placeEpochSparksPath = "res://scenes/timeline_screen/place_epoch_sparks.tscn";
  private NEpochInspectScreen _inspectScreen;
  private NEpochReminderText _reminderText;
  private Control _reminderVfxHolder;
  private ColorRect _backstop;
  private Control _inputBlocker;
  private Control _lineContainer;
  private Control _line;
  private HBoxContainer _epochSlotContainer;
  private NSlotsContainer _slotsContainer;
  private NBackButton _backButton;
  private Control _unlockScreenHolder;
  private NTimelineTutorial? _tutorial;
  private ProgressState _save;
  private bool _isUiVisible;
  private Dictionary<EpochEra, NEraColumn> _uniqueEpochEras = new Dictionary<EpochEra, NEraColumn>();
  private NEpochSlot? _queuedInspectScreen;
  private Queue<NUnlockScreen> _unlockScreens = new Queue<NUnlockScreen>();
  private Tween? _lineGrowTween;
  private Tween? _backstopTween;

  public static string[] AssetPaths
  {
    get
    {
      List<string> stringList = new List<string>();
      stringList.Add(NTimelineScreen._scenePath);
      stringList.Add("res://scenes/timeline_screen/place_epoch_sparks.tscn");
      stringList.Add(NEpochHighlightVfx.scenePath);
      stringList.Add(NEpochOffscreenVfx.scenePath);
      stringList.Add(NEpochInspectScreen.lockedImagePath);
      stringList.AddRange(NUnlockTimelineScreen.AssetPaths);
      stringList.AddRange(NUnlockPotionsScreen.AssetPaths);
      stringList.AddRange(NUnlockRelicsScreen.AssetPaths);
      stringList.AddRange(NUnlockCardsScreen.AssetPaths);
      stringList.AddRange(NUnlockMiscScreen.AssetPaths);
      stringList.AddRange(NUnlockCharacterScreen.AssetPaths);
      stringList.AddRange(NEraColumn.assetPaths);
      stringList.AddRange(NTimelineScreen.GetAllEraTexturePaths());
      return stringList.ToArray();
    }
  }

  public static NTimelineScreen Instance
  {
    get => NGame.Instance.MainMenu.SubmenuStack.GetSubmenuType<NTimelineScreen>();
  }

  public NUnlockScreen? CurrentUnlockScreen { get; set; }

  public static NTimelineScreen? Create()
  {
    return TestMode.IsOn ? (NTimelineScreen) null : PreloadManager.Cache.GetScene(NTimelineScreen._scenePath).Instantiate<NTimelineScreen>((PackedScene.GenEditState) 0L);
  }

  public override void OnSubmenuOpened()
  {
    this.ResetScreen();
    this.DisableInput();
    SerializableEpoch serializableEpoch = SaveManager.Instance.Progress.Epochs.FirstOrDefault<SerializableEpoch>((Func<SerializableEpoch, bool>) (e => e.Id == EpochModel.GetId<NeowEpoch>()));
    bool flag1 = serializableEpoch == null;
    if (!flag1)
    {
      bool flag2;
      switch (serializableEpoch.State)
      {
        case EpochState.NoSlot:
        case EpochState.NotObtained:
          flag2 = true;
          break;
        default:
          flag2 = false;
          break;
      }
      flag1 = flag2;
    }
    if (flag1)
      SaveManager.Instance.Progress.ObtainEpoch(EpochModel.GetId<NeowEpoch>());
    if (SaveManager.Instance.IsNeowDiscovered())
    {
      TaskHelper.RunSafely(this.FirstTimeLogic());
    }
    else
    {
      SfxCmd.Play("event:/sfx/ui/timeline/ui_timeline_open");
      TaskHelper.RunSafely(this.InitScreen());
    }
    this.SetScreenDraggability();
    AchievementsHelper.CheckTimelineComplete();
  }

  public override void OnSubmenuClosed()
  {
    base.OnSubmenuClosed();
    this.ResetScreen();
  }

  protected override void OnSubmenuShown()
  {
    ((Node) this).ProcessMode = (Node.ProcessModeEnum) 0L;
    this.RefreshBackButton();
  }

  protected override void OnSubmenuHidden()
  {
    ((Node) this).ProcessMode = (Node.ProcessModeEnum) 4L;
    NGame.Instance.MainMenu?.RefreshButtons();
  }

  public override void _Ready()
  {
    this.ConnectSignals();
    this._epochSlotContainer = ((Node) this).GetNode<HBoxContainer>(NodePath.op_Implicit("%EpochSlots"));
    this._reminderText = ((Node) this).GetNode<NEpochReminderText>(NodePath.op_Implicit("%EpochReminderText"));
    this._reminderVfxHolder = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%ReminderVfxHolder"));
    this._inputBlocker = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%InputBlocker"));
    this._backstop = ((Node) this).GetNode<ColorRect>(NodePath.op_Implicit("%SharedBackstop"));
    this._inspectScreen = ((Node) this).GetNode<NEpochInspectScreen>(NodePath.op_Implicit("%EpochInspectScreen"));
    this._line = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%Line"));
    this._lineContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%LineContainer"));
    this._slotsContainer = ((Node) this).GetNode<NSlotsContainer>(NodePath.op_Implicit("%SlotsContainer"));
    this._backButton = ((Node) this).GetNode<NBackButton>(NodePath.op_Implicit("BackButton"));
    this._unlockScreenHolder = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%UnlockScreenHolder"));
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    ((GodotObject) this._backButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(NTimelineScreen.\u003C\u003EO.\u003C0\u003E__OnBackButtonPressed ?? (NTimelineScreen.\u003C\u003EO.\u003C0\u003E__OnBackButtonPressed = new Action<NButton>(NTimelineScreen.OnBackButtonPressed))), 0U);
    this._save = SaveManager.Instance.Progress;
    ((Node) this).CreateTween().TweenProperty((GodotObject) this._slotsContainer, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 1.0);
  }

  private static void OnBackButtonPressed(NButton obj)
  {
    SfxCmd.Play("event:/sfx/ui/map/map_close");
  }

  private async Task FirstTimeLogic()
  {
    double num = (double) await ((Node) this).AwaitProcessFrame();
    this._tutorial = SceneHelper.Instantiate<NTimelineTutorial>("timeline_screen/timeline_tutorial");
    ((Node) this).AddChildSafely((Node) this._tutorial);
    this._tutorial.Init(this);
  }

  public async Task SpawnFirstTimeTimeline()
  {
    SfxCmd.Play("event:/sfx/ui/timeline/ui_timeline_open");
    Log.Info("Running first time logic");
    await this.AddEpochSlots(new List<EpochSlotData>(1)
    {
      new EpochSlotData(EpochModel.GetId<NeowEpoch>(), EpochSlotState.Obtained)
    }, true);
    SaveManager.Instance.UnlockSlot(EpochModel.GetId<NeowEpoch>());
    this.EnableInput();
  }

  private async Task InitScreen()
  {
    Log.Info("Initializing Timeline:");
    List<EpochSlotData> source = new List<EpochSlotData>();
    this._lineGrowTween?.Kill();
    this._lineGrowTween = ((Node) this).CreateTween();
    this._lineGrowTween.TweenProperty((GodotObject) this._lineContainer, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.5);
    foreach (SerializableEpoch epoch in (IEnumerable<SerializableEpoch>) this._save.Epochs)
    {
      if (epoch.State != EpochState.ObtainedNoSlot)
        source.Add(new EpochSlotData(epoch.Id, EpochSlotState.NotObtained));
    }
    await this.AddEpochSlots(source.OrderBy<EpochSlotData, int>((Func<EpochSlotData, int>) (a => a.EraPosition)).ToList<EpochSlotData>(), false);
    int num = 0;
    foreach (SerializableEpoch epoch in (IEnumerable<SerializableEpoch>) this._save.Epochs)
    {
      EpochModel epochModel = EpochModel.Get(epoch.Id);
      if (epoch.State > EpochState.ObtainedNoSlot)
      {
        foreach (Node child in ((Node) this._uniqueEpochEras[epochModel.Era]).GetChildren(false))
        {
          if (child is NEpochSlot nepochSlot && nepochSlot.eraPosition == epochModel.EraPosition)
          {
            ++num;
            nepochSlot.SetState(epoch.State >= EpochState.Revealed ? EpochSlotState.Complete : EpochSlotState.Obtained);
            TaskHelper.RunSafely(child.GetParent<NEraColumn>().SpawnNameAndYear());
          }
        }
      }
    }
    Log.Info($"{num} Epochs are complete");
    TaskHelper.RunSafely(this.NavigateToRevealableSlot());
  }

  private async Task NavigateToRevealableSlot()
  {
    if (SaveManager.Instance.GetDiscoveredEpochCount() == 0)
    {
      this.EnableInput();
    }
    else
    {
      double num1 = (double) await ((Node) this).AwaitProcessFrame();
      float getInitX = this._slotsContainer.GetInitX;
      float slotPositionX = 0.0f;
      float num2 = float.MaxValue;
      foreach (Node node in this._uniqueEpochEras.Values)
      {
        foreach (NEpochSlot nepochSlot in node.GetChildrenRecursive<NEpochSlot>())
        {
          if (nepochSlot.State == EpochSlotState.Obtained)
          {
            float num3 = Math.Abs(getInitX - nepochSlot.GlobalPosition.X);
            if ((double) num3 < (double) num2)
            {
              slotPositionX = nepochSlot.GlobalPosition.X;
              num2 = num3;
            }
          }
        }
      }
      await TaskHelper.RunSafely(this._slotsContainer.LerpToSlot(slotPositionX));
      this.EnableInput();
    }
  }

  public async Task AddEpochSlots(List<EpochSlotData> slotsToAdd, bool isAnimated)
  {
    List<NEraColumn> newlyCreatedColumns = new List<NEraColumn>();
    if (isAnimated)
    {
      foreach (NEraColumn neraColumn in this._uniqueEpochEras.Values)
        TaskHelper.RunSafely(neraColumn.SaveBeforeAnimationPosition());
    }
    foreach (EpochSlotData epochSlotData in slotsToAdd)
    {
      NEraColumn neraColumn1;
      if (!this._uniqueEpochEras.TryGetValue(epochSlotData.Era, out neraColumn1))
      {
        NEraColumn child1 = NEraColumn.Create(epochSlotData);
        ((Node) this._epochSlotContainer).AddChildSafely((Node) child1);
        int index = 0;
        foreach (Node child2 in ((Node) this._epochSlotContainer).GetChildren(false))
        {
          if (child2 is NEraColumn neraColumn2 && child1.era > neraColumn2.era)
            index = ((Node) neraColumn2).GetIndex(false) + 1;
        }
        ((Node) this._epochSlotContainer).MoveChildSafely((Node) child1, index);
        newlyCreatedColumns.Add(child1);
        this._uniqueEpochEras.Add(epochSlotData.Era, child1);
      }
      else
        neraColumn1.AddSlot(epochSlotData);
    }
    Log.Info($" Created {slotsToAdd.Count} Epoch slots");
    Log.Info($" Created {newlyCreatedColumns.Count} Era columns");
    if (isAnimated)
    {
      List<Vector2> vector2List = this.PredictHBoxLayout(this._epochSlotContainer);
      foreach (NEraColumn neraColumn in this._uniqueEpochEras.Values)
        neraColumn.SetPredictedPosition(vector2List[((Node) neraColumn).GetIndex(false)]);
      await this.GrowTimelineAndAddEraIcons(newlyCreatedColumns);
    }
    else
      this.InitLineAndIcons(newlyCreatedColumns);
  }

  private List<Vector2> PredictHBoxLayout(HBoxContainer hbox)
  {
    float num1 = 0.0f;
    float themeConstant = (float) ((Control) hbox).GetThemeConstant(ThemeConstants.BoxContainer.Separation, StringName.op_Implicit("HBoxContainer"));
    List<Control> list = ((IEnumerable) ((Node) hbox).GetChildren(false)).OfType<Control>().Where<Control>((Func<Control, bool>) (c => ((CanvasItem) c).Visible)).ToList<Control>();
    int num2 = 0;
    foreach (Control control in list)
    {
      num1 += control.CustomMinimumSize.X;
      if ((control.SizeFlagsHorizontal & 2L) != null)
        ++num2;
    }
    float num3 = num1 + themeConstant * (float) Math.Max(list.Count - 1, 0);
    float num4 = ((Control) hbox).Size.X - num3;
    float num5 = num2 > 0 ? num4 / (float) num2 : 0.0f;
    float num6 = 0.0f;
    List<Vector2> vector2List = new List<Vector2>();
    foreach (Control control in list)
    {
      float x = control.CustomMinimumSize.X;
      if ((control.SizeFlagsHorizontal & 2L) != null)
        x += num5;
      vector2List.Add(new Vector2(num6, 0.0f));
      num6 += x + themeConstant;
    }
    return vector2List;
  }

  private async Task GrowTimelineAndAddEraIcons(List<NEraColumn> newlyCreatedColumns)
  {
    if (newlyCreatedColumns.Count > 0)
    {
      this._lineGrowTween?.Kill();
      this._lineGrowTween = ((Node) this).CreateTween().SetParallel(true);
      this._lineGrowTween.TweenProperty((GodotObject) this._lineContainer, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.5);
      this._lineGrowTween.TweenProperty((GodotObject) this._line, NodePath.op_Implicit("custom_minimum_size:x"), Variant.op_Implicit((float) this._uniqueEpochEras.Count * 226f), 2.0).SetEase((Tween.EaseType) 2L).SetTrans((Tween.TransitionType) 7L);
      if (!await this._lineGrowTween.AwaitFinished((Node) this))
        return;
      Log.Info("Spawning slots...");
      foreach (NEraColumn newlyCreatedColumn in newlyCreatedColumns)
        newlyCreatedColumn.SpawnIcon();
      newlyCreatedColumns.Clear();
    }
    foreach (NEraColumn neraColumn in this._uniqueEpochEras.Values)
      TaskHelper.RunSafely(neraColumn.SpawnSlots(true));
  }

  private void InitLineAndIcons(List<NEraColumn> newlyCreatedColumns)
  {
    if (newlyCreatedColumns.Count == 0)
      return;
    this._line.CustomMinimumSize = new Vector2((float) this._uniqueEpochEras.Count * 226f, this._line.CustomMinimumSize.Y);
    foreach (NEraColumn newlyCreatedColumn in newlyCreatedColumns)
      newlyCreatedColumn.SpawnIcon();
    foreach (NEraColumn neraColumn in this._uniqueEpochEras.Values)
      TaskHelper.RunSafely(neraColumn.SpawnSlots(false));
    newlyCreatedColumns.Clear();
  }

  public static (Texture2D Texture, string Name) GetEraIcon(EpochEra era)
  {
    return (PreloadManager.Cache.GetTexture2D(NTimelineScreen.GetEraTexturePath(era)), StringHelper.Slugify(era.ToString()));
  }

  private static string GetEraTexturePath(EpochEra era)
  {
    if (era < EpochEra.Seeds0)
      return $"res://images/atlases/era_atlas.sprites/era_minus_{Math.Abs((int) era)}.tres";
    return $"res://images/atlases/era_atlas.sprites/era_{(int) era}.tres";
  }

  private static IEnumerable<string> GetAllEraTexturePaths()
  {
    EpochEra[] epochEraArray = Enum.GetValues<EpochEra>();
    for (int index = 0; index < epochEraArray.Length; ++index)
      yield return NTimelineScreen.GetEraTexturePath(epochEraArray[index]);
    epochEraArray = (EpochEra[]) null;
  }

  private NEpochSlot? GetSlot(EpochEra era, int position)
  {
    foreach (KeyValuePair<EpochEra, NEraColumn> uniqueEpochEra in this._uniqueEpochEras)
    {
      if (uniqueEpochEra.Key == era)
        return (NEpochSlot) ((Node) uniqueEpochEra.Value).GetChild(((Node) uniqueEpochEra.Value).GetChildCount(false) - position - 2, false);
    }
    Log.Error($"Could not find Epoch slot: {era}, {position}");
    return (NEpochSlot) null;
  }

  public void OpenInspectScreen(NEpochSlot slot, bool playAnimation)
  {
    if (playAnimation)
      TaskHelper.RunSafely(((Node) slot).GetParent<NEraColumn>().SpawnNameAndYear());
    this._lastFocusedControl = (Control) slot;
    TaskHelper.RunSafely(this._inspectScreen.Open(slot, slot.model, playAnimation));
  }

  public void QueueMiscUnlock(string text)
  {
    NUnlockMiscScreen nunlockMiscScreen = NUnlockMiscScreen.Create();
    nunlockMiscScreen.SetUnlocks(text);
    this._unlockScreens.Enqueue((NUnlockScreen) nunlockMiscScreen);
  }

  public void QueueCharacterUnlock<T>(EpochModel epoch) where T : CharacterModel
  {
    this._unlockScreens.Enqueue((NUnlockScreen) NUnlockCharacterScreen.Create(epoch, (CharacterModel) ModelDb.Character<T>()));
  }

  public void QueueCardUnlock(IReadOnlyList<CardModel> cards)
  {
    NUnlockCardsScreen nunlockCardsScreen = NUnlockCardsScreen.Create();
    nunlockCardsScreen.SetCards(cards);
    this._unlockScreens.Enqueue((NUnlockScreen) nunlockCardsScreen);
  }

  public void QueueRelicUnlock(List<RelicModel> relics)
  {
    NUnlockRelicsScreen nunlockRelicsScreen = NUnlockRelicsScreen.Create();
    nunlockRelicsScreen.SetRelics((IReadOnlyList<RelicModel>) relics);
    this._unlockScreens.Enqueue((NUnlockScreen) nunlockRelicsScreen);
  }

  public void QueuePotionUnlock(List<PotionModel> potions)
  {
    NUnlockPotionsScreen nunlockPotionsScreen = NUnlockPotionsScreen.Create();
    nunlockPotionsScreen.SetPotions((IReadOnlyList<PotionModel>) potions);
    this._unlockScreens.Enqueue((NUnlockScreen) nunlockPotionsScreen);
  }

  public void QueueTimelineExpansion(List<EpochSlotData> eraData)
  {
    NUnlockTimelineScreen nunlockTimelineScreen = NUnlockTimelineScreen.Create();
    nunlockTimelineScreen.SetUnlocks(eraData);
    this._unlockScreens.Enqueue((NUnlockScreen) nunlockTimelineScreen);
  }

  public void SetScreenDraggability()
  {
    this._slotsContainer.MouseFilter = this._save.Epochs.Count <= 4 ? (Control.MouseFilterEnum) 2L : (Control.MouseFilterEnum) 0L;
  }

  public void ShowBackstopAndHideUi()
  {
    ((CanvasItem) this._backstop).Visible = true;
    this._backstopTween?.Kill();
    this._backstopTween = ((Node) this).CreateTween().SetParallel(true);
    this._backstopTween.TweenProperty((GodotObject) this._slotsContainer, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.1f), 0.4);
    this._backstopTween.TweenProperty((GodotObject) this._backstop, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.5f), 0.4);
    this._backButton.Disable();
    this._reminderText.AnimateOut();
  }

  public async Task HideBackstopAndShowUi(bool showBackButton)
  {
    Tween backstopTween = this._backstopTween;
    if (backstopTween != null)
      backstopTween.FastForwardToCompletion();
    this._backstopTween = ((Node) this).CreateTween().SetParallel(true);
    this._backstopTween.TweenProperty((GodotObject) this._slotsContainer, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.4).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
    this._backstopTween.TweenProperty((GodotObject) this._backstop, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.4).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
    if (showBackButton)
      this.RefreshBackButton();
    if (!await this._backstopTween.AwaitFinished((Node) this))
      return;
    ((CanvasItem) this._backstop).Visible = false;
  }

  public void OpenQueuedScreen()
  {
    NUnlockScreen child = this._unlockScreens.Dequeue();
    ((Node) this._unlockScreenHolder).AddChildSafely((Node) child);
    child.Open();
  }

  public bool IsScreenQueued() => this._unlockScreens.Count > 0;

  private bool IsInspectScreenQueued() => this._queuedInspectScreen != null;

  private async Task SpawnEraLabel(EpochEra era)
  {
    await this._uniqueEpochEras[era].SpawnNameAndYear();
  }

  public void ShowHeaderAndActionsUi()
  {
    if (this._isUiVisible)
      return;
    this._isUiVisible = true;
  }

  public void DisableInput()
  {
    ((CanvasItem) this._inputBlocker).Visible = true;
    this._inputBlocker.MouseFilter = (Control.MouseFilterEnum) 0L;
    this._slotsContainer.SetEnabled(false);
    ((Node) this).GetViewport().GuiReleaseFocus();
  }

  public void EnableInput()
  {
    if (this._queuedInspectScreen != null || this._unlockScreens.Count != 0)
      return;
    this.RefreshBackButton();
    ((CanvasItem) this._inputBlocker).Visible = false;
    this._inputBlocker.MouseFilter = (Control.MouseFilterEnum) 2L;
    this._slotsContainer.SetEnabled(true);
    ActiveScreenContext.Instance.Update();
  }

  private void RefreshBackButton()
  {
    if (SaveManager.Instance.GetDiscoveredEpochCount() > 0)
    {
      if (((Node) this._epochSlotContainer).GetChildCount(false) > 1)
        this._reminderText.AnimateIn();
      this._backButton.Disable();
    }
    else
      this._backButton.Enable();
  }

  private void ResetScreen()
  {
    Control lineContainer = this._lineContainer;
    Color modulate = ((CanvasItem) this._lineContainer).Modulate;
    modulate.A = 0.0f;
    Color color = modulate;
    ((CanvasItem) lineContainer).Modulate = color;
    Log.Info("Cleaning up Timeline screen...");
    this._uniqueEpochEras = new Dictionary<EpochEra, NEraColumn>();
    this._queuedInspectScreen = (NEpochSlot) null;
    this._unlockScreens = new Queue<NUnlockScreen>();
    ((Node) this._epochSlotContainer).FreeChildren();
    ((Node) this._reminderVfxHolder).FreeChildren();
    this._slotsContainer.Reset();
    if (this._tutorial == null)
      return;
    ((Node) this._tutorial).QueueFreeSafely();
    this._tutorial = (NTimelineTutorial) null;
  }

  protected override Control? InitialFocusedControl
  {
    get
    {
      return (Control) ((IEnumerable<Node>) ((Node) this._epochSlotContainer).GetChildren(false)).SelectMany<Node, NEpochSlot>((Func<Node, IEnumerable<NEpochSlot>>) (c => ((IEnumerable) c.GetChildren(false)).OfType<NEpochSlot>())).FirstOrDefault<NEpochSlot>((Func<NEpochSlot, bool>) (s => s.model is NeowEpoch));
    }
  }

  public Control GetReminderVfxHolder() => this._reminderVfxHolder;

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(22)
    {
      new MethodInfo(NTimelineScreen.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTimelineScreen.MethodName.OnSubmenuOpened, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTimelineScreen.MethodName.OnSubmenuClosed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTimelineScreen.MethodName.OnSubmenuShown, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTimelineScreen.MethodName.OnSubmenuHidden, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTimelineScreen.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTimelineScreen.MethodName.OnBackButtonPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("obj"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NTimelineScreen.MethodName.GetEraTexturePath, new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("era"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NTimelineScreen.MethodName.GetSlot, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("era"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("position"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NTimelineScreen.MethodName.OpenInspectScreen, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("slot"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false),
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("playAnimation"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NTimelineScreen.MethodName.QueueMiscUnlock, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("text"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NTimelineScreen.MethodName.SetScreenDraggability, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTimelineScreen.MethodName.ShowBackstopAndHideUi, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTimelineScreen.MethodName.OpenQueuedScreen, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTimelineScreen.MethodName.IsScreenQueued, new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTimelineScreen.MethodName.IsInspectScreenQueued, new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTimelineScreen.MethodName.ShowHeaderAndActionsUi, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTimelineScreen.MethodName.DisableInput, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTimelineScreen.MethodName.EnableInput, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTimelineScreen.MethodName.RefreshBackButton, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTimelineScreen.MethodName.ResetScreen, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTimelineScreen.MethodName.GetReminderVfxHolder, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NTimelineScreen.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NTimelineScreen ntimelineScreen = NTimelineScreen.Create();
      ret = VariantUtils.CreateFrom<NTimelineScreen>(ref ntimelineScreen);
      return true;
    }
    if (StringName.op_Equality(ref method, NTimelineScreen.MethodName.OnSubmenuOpened) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnSubmenuOpened();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTimelineScreen.MethodName.OnSubmenuClosed) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnSubmenuClosed();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTimelineScreen.MethodName.OnSubmenuShown) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnSubmenuShown();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTimelineScreen.MethodName.OnSubmenuHidden) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnSubmenuHidden();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTimelineScreen.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTimelineScreen.MethodName.OnBackButtonPressed) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NTimelineScreen.OnBackButtonPressed(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTimelineScreen.MethodName.GetEraTexturePath) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      string eraTexturePath = NTimelineScreen.GetEraTexturePath(VariantUtils.ConvertTo<EpochEra>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<string>(ref eraTexturePath);
      return true;
    }
    if (StringName.op_Equality(ref method, NTimelineScreen.MethodName.GetSlot) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      NEpochSlot slot = this.GetSlot(VariantUtils.ConvertTo<EpochEra>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = VariantUtils.CreateFrom<NEpochSlot>(ref slot);
      return true;
    }
    if (StringName.op_Equality(ref method, NTimelineScreen.MethodName.OpenInspectScreen) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      this.OpenInspectScreen(VariantUtils.ConvertTo<NEpochSlot>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTimelineScreen.MethodName.QueueMiscUnlock) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.QueueMiscUnlock(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTimelineScreen.MethodName.SetScreenDraggability) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.SetScreenDraggability();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTimelineScreen.MethodName.ShowBackstopAndHideUi) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ShowBackstopAndHideUi();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTimelineScreen.MethodName.OpenQueuedScreen) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OpenQueuedScreen();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTimelineScreen.MethodName.IsScreenQueued) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      bool flag = this.IsScreenQueued();
      ret = VariantUtils.CreateFrom<bool>(ref flag);
      return true;
    }
    if (StringName.op_Equality(ref method, NTimelineScreen.MethodName.IsInspectScreenQueued) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      bool flag = this.IsInspectScreenQueued();
      ret = VariantUtils.CreateFrom<bool>(ref flag);
      return true;
    }
    if (StringName.op_Equality(ref method, NTimelineScreen.MethodName.ShowHeaderAndActionsUi) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ShowHeaderAndActionsUi();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTimelineScreen.MethodName.DisableInput) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.DisableInput();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTimelineScreen.MethodName.EnableInput) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.EnableInput();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTimelineScreen.MethodName.RefreshBackButton) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RefreshBackButton();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTimelineScreen.MethodName.ResetScreen) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ResetScreen();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NTimelineScreen.MethodName.GetReminderVfxHolder) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    Control reminderVfxHolder = this.GetReminderVfxHolder();
    ret = VariantUtils.CreateFrom<Control>(ref reminderVfxHolder);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NTimelineScreen.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NTimelineScreen ntimelineScreen = NTimelineScreen.Create();
      ret = VariantUtils.CreateFrom<NTimelineScreen>(ref ntimelineScreen);
      return true;
    }
    if (StringName.op_Equality(ref method, NTimelineScreen.MethodName.OnBackButtonPressed) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NTimelineScreen.OnBackButtonPressed(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTimelineScreen.MethodName.GetEraTexturePath) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      string eraTexturePath = NTimelineScreen.GetEraTexturePath(VariantUtils.ConvertTo<EpochEra>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<string>(ref eraTexturePath);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NTimelineScreen.MethodName.Create) || StringName.op_Equality(ref method, NTimelineScreen.MethodName.OnSubmenuOpened) || StringName.op_Equality(ref method, NTimelineScreen.MethodName.OnSubmenuClosed) || StringName.op_Equality(ref method, NTimelineScreen.MethodName.OnSubmenuShown) || StringName.op_Equality(ref method, NTimelineScreen.MethodName.OnSubmenuHidden) || StringName.op_Equality(ref method, NTimelineScreen.MethodName._Ready) || StringName.op_Equality(ref method, NTimelineScreen.MethodName.OnBackButtonPressed) || StringName.op_Equality(ref method, NTimelineScreen.MethodName.GetEraTexturePath) || StringName.op_Equality(ref method, NTimelineScreen.MethodName.GetSlot) || StringName.op_Equality(ref method, NTimelineScreen.MethodName.OpenInspectScreen) || StringName.op_Equality(ref method, NTimelineScreen.MethodName.QueueMiscUnlock) || StringName.op_Equality(ref method, NTimelineScreen.MethodName.SetScreenDraggability) || StringName.op_Equality(ref method, NTimelineScreen.MethodName.ShowBackstopAndHideUi) || StringName.op_Equality(ref method, NTimelineScreen.MethodName.OpenQueuedScreen) || StringName.op_Equality(ref method, NTimelineScreen.MethodName.IsScreenQueued) || StringName.op_Equality(ref method, NTimelineScreen.MethodName.IsInspectScreenQueued) || StringName.op_Equality(ref method, NTimelineScreen.MethodName.ShowHeaderAndActionsUi) || StringName.op_Equality(ref method, NTimelineScreen.MethodName.DisableInput) || StringName.op_Equality(ref method, NTimelineScreen.MethodName.EnableInput) || StringName.op_Equality(ref method, NTimelineScreen.MethodName.RefreshBackButton) || StringName.op_Equality(ref method, NTimelineScreen.MethodName.ResetScreen) || StringName.op_Equality(ref method, NTimelineScreen.MethodName.GetReminderVfxHolder) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NTimelineScreen.PropertyName.CurrentUnlockScreen))
    {
      this.CurrentUnlockScreen = VariantUtils.ConvertTo<NUnlockScreen>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTimelineScreen.PropertyName._inspectScreen))
    {
      this._inspectScreen = VariantUtils.ConvertTo<NEpochInspectScreen>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTimelineScreen.PropertyName._reminderText))
    {
      this._reminderText = VariantUtils.ConvertTo<NEpochReminderText>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTimelineScreen.PropertyName._reminderVfxHolder))
    {
      this._reminderVfxHolder = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTimelineScreen.PropertyName._backstop))
    {
      this._backstop = VariantUtils.ConvertTo<ColorRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTimelineScreen.PropertyName._inputBlocker))
    {
      this._inputBlocker = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTimelineScreen.PropertyName._lineContainer))
    {
      this._lineContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTimelineScreen.PropertyName._line))
    {
      this._line = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTimelineScreen.PropertyName._epochSlotContainer))
    {
      this._epochSlotContainer = VariantUtils.ConvertTo<HBoxContainer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTimelineScreen.PropertyName._slotsContainer))
    {
      this._slotsContainer = VariantUtils.ConvertTo<NSlotsContainer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTimelineScreen.PropertyName._backButton))
    {
      this._backButton = VariantUtils.ConvertTo<NBackButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTimelineScreen.PropertyName._unlockScreenHolder))
    {
      this._unlockScreenHolder = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTimelineScreen.PropertyName._tutorial))
    {
      this._tutorial = VariantUtils.ConvertTo<NTimelineTutorial>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTimelineScreen.PropertyName._isUiVisible))
    {
      this._isUiVisible = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTimelineScreen.PropertyName._queuedInspectScreen))
    {
      this._queuedInspectScreen = VariantUtils.ConvertTo<NEpochSlot>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTimelineScreen.PropertyName._lineGrowTween))
    {
      this._lineGrowTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NTimelineScreen.PropertyName._backstopTween))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._backstopTween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NTimelineScreen.PropertyName.CurrentUnlockScreen))
    {
      ref godot_variant local = ref value;
      NUnlockScreen currentUnlockScreen = this.CurrentUnlockScreen;
      godot_variant from = VariantUtils.CreateFrom<NUnlockScreen>(ref currentUnlockScreen);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NTimelineScreen.PropertyName.InitialFocusedControl))
    {
      ref godot_variant local = ref value;
      Control initialFocusedControl = this.InitialFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref initialFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NTimelineScreen.PropertyName._inspectScreen))
    {
      value = VariantUtils.CreateFrom<NEpochInspectScreen>(ref this._inspectScreen);
      return true;
    }
    if (StringName.op_Equality(ref name, NTimelineScreen.PropertyName._reminderText))
    {
      value = VariantUtils.CreateFrom<NEpochReminderText>(ref this._reminderText);
      return true;
    }
    if (StringName.op_Equality(ref name, NTimelineScreen.PropertyName._reminderVfxHolder))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._reminderVfxHolder);
      return true;
    }
    if (StringName.op_Equality(ref name, NTimelineScreen.PropertyName._backstop))
    {
      value = VariantUtils.CreateFrom<ColorRect>(ref this._backstop);
      return true;
    }
    if (StringName.op_Equality(ref name, NTimelineScreen.PropertyName._inputBlocker))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._inputBlocker);
      return true;
    }
    if (StringName.op_Equality(ref name, NTimelineScreen.PropertyName._lineContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._lineContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NTimelineScreen.PropertyName._line))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._line);
      return true;
    }
    if (StringName.op_Equality(ref name, NTimelineScreen.PropertyName._epochSlotContainer))
    {
      value = VariantUtils.CreateFrom<HBoxContainer>(ref this._epochSlotContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NTimelineScreen.PropertyName._slotsContainer))
    {
      value = VariantUtils.CreateFrom<NSlotsContainer>(ref this._slotsContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NTimelineScreen.PropertyName._backButton))
    {
      value = VariantUtils.CreateFrom<NBackButton>(ref this._backButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NTimelineScreen.PropertyName._unlockScreenHolder))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._unlockScreenHolder);
      return true;
    }
    if (StringName.op_Equality(ref name, NTimelineScreen.PropertyName._tutorial))
    {
      value = VariantUtils.CreateFrom<NTimelineTutorial>(ref this._tutorial);
      return true;
    }
    if (StringName.op_Equality(ref name, NTimelineScreen.PropertyName._isUiVisible))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isUiVisible);
      return true;
    }
    if (StringName.op_Equality(ref name, NTimelineScreen.PropertyName._queuedInspectScreen))
    {
      value = VariantUtils.CreateFrom<NEpochSlot>(ref this._queuedInspectScreen);
      return true;
    }
    if (StringName.op_Equality(ref name, NTimelineScreen.PropertyName._lineGrowTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._lineGrowTween);
      return true;
    }
    if (!StringName.op_Equality(ref name, NTimelineScreen.PropertyName._backstopTween))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<Tween>(ref this._backstopTween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NTimelineScreen.PropertyName._inspectScreen, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTimelineScreen.PropertyName._reminderText, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTimelineScreen.PropertyName._reminderVfxHolder, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTimelineScreen.PropertyName._backstop, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTimelineScreen.PropertyName._inputBlocker, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTimelineScreen.PropertyName._lineContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTimelineScreen.PropertyName._line, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTimelineScreen.PropertyName._epochSlotContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTimelineScreen.PropertyName._slotsContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTimelineScreen.PropertyName._backButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTimelineScreen.PropertyName._unlockScreenHolder, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTimelineScreen.PropertyName._tutorial, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NTimelineScreen.PropertyName._isUiVisible, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTimelineScreen.PropertyName._queuedInspectScreen, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTimelineScreen.PropertyName.CurrentUnlockScreen, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTimelineScreen.PropertyName._lineGrowTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTimelineScreen.PropertyName._backstopTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTimelineScreen.PropertyName.InitialFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo = info;
    StringName currentUnlockScreen1 = NTimelineScreen.PropertyName.CurrentUnlockScreen;
    NUnlockScreen currentUnlockScreen2 = this.CurrentUnlockScreen;
    Variant variant = Variant.From<NUnlockScreen>(ref currentUnlockScreen2);
    serializationInfo.AddProperty(currentUnlockScreen1, variant);
    info.AddProperty(NTimelineScreen.PropertyName._inspectScreen, Variant.From<NEpochInspectScreen>(ref this._inspectScreen));
    info.AddProperty(NTimelineScreen.PropertyName._reminderText, Variant.From<NEpochReminderText>(ref this._reminderText));
    info.AddProperty(NTimelineScreen.PropertyName._reminderVfxHolder, Variant.From<Control>(ref this._reminderVfxHolder));
    info.AddProperty(NTimelineScreen.PropertyName._backstop, Variant.From<ColorRect>(ref this._backstop));
    info.AddProperty(NTimelineScreen.PropertyName._inputBlocker, Variant.From<Control>(ref this._inputBlocker));
    info.AddProperty(NTimelineScreen.PropertyName._lineContainer, Variant.From<Control>(ref this._lineContainer));
    info.AddProperty(NTimelineScreen.PropertyName._line, Variant.From<Control>(ref this._line));
    info.AddProperty(NTimelineScreen.PropertyName._epochSlotContainer, Variant.From<HBoxContainer>(ref this._epochSlotContainer));
    info.AddProperty(NTimelineScreen.PropertyName._slotsContainer, Variant.From<NSlotsContainer>(ref this._slotsContainer));
    info.AddProperty(NTimelineScreen.PropertyName._backButton, Variant.From<NBackButton>(ref this._backButton));
    info.AddProperty(NTimelineScreen.PropertyName._unlockScreenHolder, Variant.From<Control>(ref this._unlockScreenHolder));
    info.AddProperty(NTimelineScreen.PropertyName._tutorial, Variant.From<NTimelineTutorial>(ref this._tutorial));
    info.AddProperty(NTimelineScreen.PropertyName._isUiVisible, Variant.From<bool>(ref this._isUiVisible));
    info.AddProperty(NTimelineScreen.PropertyName._queuedInspectScreen, Variant.From<NEpochSlot>(ref this._queuedInspectScreen));
    info.AddProperty(NTimelineScreen.PropertyName._lineGrowTween, Variant.From<Tween>(ref this._lineGrowTween));
    info.AddProperty(NTimelineScreen.PropertyName._backstopTween, Variant.From<Tween>(ref this._backstopTween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NTimelineScreen.PropertyName.CurrentUnlockScreen, ref variant1))
      this.CurrentUnlockScreen = ((Variant) ref variant1).As<NUnlockScreen>();
    Variant variant2;
    if (info.TryGetProperty(NTimelineScreen.PropertyName._inspectScreen, ref variant2))
      this._inspectScreen = ((Variant) ref variant2).As<NEpochInspectScreen>();
    Variant variant3;
    if (info.TryGetProperty(NTimelineScreen.PropertyName._reminderText, ref variant3))
      this._reminderText = ((Variant) ref variant3).As<NEpochReminderText>();
    Variant variant4;
    if (info.TryGetProperty(NTimelineScreen.PropertyName._reminderVfxHolder, ref variant4))
      this._reminderVfxHolder = ((Variant) ref variant4).As<Control>();
    Variant variant5;
    if (info.TryGetProperty(NTimelineScreen.PropertyName._backstop, ref variant5))
      this._backstop = ((Variant) ref variant5).As<ColorRect>();
    Variant variant6;
    if (info.TryGetProperty(NTimelineScreen.PropertyName._inputBlocker, ref variant6))
      this._inputBlocker = ((Variant) ref variant6).As<Control>();
    Variant variant7;
    if (info.TryGetProperty(NTimelineScreen.PropertyName._lineContainer, ref variant7))
      this._lineContainer = ((Variant) ref variant7).As<Control>();
    Variant variant8;
    if (info.TryGetProperty(NTimelineScreen.PropertyName._line, ref variant8))
      this._line = ((Variant) ref variant8).As<Control>();
    Variant variant9;
    if (info.TryGetProperty(NTimelineScreen.PropertyName._epochSlotContainer, ref variant9))
      this._epochSlotContainer = ((Variant) ref variant9).As<HBoxContainer>();
    Variant variant10;
    if (info.TryGetProperty(NTimelineScreen.PropertyName._slotsContainer, ref variant10))
      this._slotsContainer = ((Variant) ref variant10).As<NSlotsContainer>();
    Variant variant11;
    if (info.TryGetProperty(NTimelineScreen.PropertyName._backButton, ref variant11))
      this._backButton = ((Variant) ref variant11).As<NBackButton>();
    Variant variant12;
    if (info.TryGetProperty(NTimelineScreen.PropertyName._unlockScreenHolder, ref variant12))
      this._unlockScreenHolder = ((Variant) ref variant12).As<Control>();
    Variant variant13;
    if (info.TryGetProperty(NTimelineScreen.PropertyName._tutorial, ref variant13))
      this._tutorial = ((Variant) ref variant13).As<NTimelineTutorial>();
    Variant variant14;
    if (info.TryGetProperty(NTimelineScreen.PropertyName._isUiVisible, ref variant14))
      this._isUiVisible = ((Variant) ref variant14).As<bool>();
    Variant variant15;
    if (info.TryGetProperty(NTimelineScreen.PropertyName._queuedInspectScreen, ref variant15))
      this._queuedInspectScreen = ((Variant) ref variant15).As<NEpochSlot>();
    Variant variant16;
    if (info.TryGetProperty(NTimelineScreen.PropertyName._lineGrowTween, ref variant16))
      this._lineGrowTween = ((Variant) ref variant16).As<Tween>();
    Variant variant17;
    if (!info.TryGetProperty(NTimelineScreen.PropertyName._backstopTween, ref variant17))
      return;
    this._backstopTween = ((Variant) ref variant17).As<Tween>();
  }

  public new class MethodName : NSubmenu.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public new static readonly StringName OnSubmenuOpened = StringName.op_Implicit(nameof (OnSubmenuOpened));
    public new static readonly StringName OnSubmenuClosed = StringName.op_Implicit(nameof (OnSubmenuClosed));
    public new static readonly StringName OnSubmenuShown = StringName.op_Implicit(nameof (OnSubmenuShown));
    public new static readonly StringName OnSubmenuHidden = StringName.op_Implicit(nameof (OnSubmenuHidden));
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName OnBackButtonPressed = StringName.op_Implicit(nameof (OnBackButtonPressed));
    public static readonly StringName GetEraTexturePath = StringName.op_Implicit(nameof (GetEraTexturePath));
    public static readonly StringName GetSlot = StringName.op_Implicit(nameof (GetSlot));
    public static readonly StringName OpenInspectScreen = StringName.op_Implicit(nameof (OpenInspectScreen));
    public static readonly StringName QueueMiscUnlock = StringName.op_Implicit(nameof (QueueMiscUnlock));
    public static readonly StringName SetScreenDraggability = StringName.op_Implicit(nameof (SetScreenDraggability));
    public static readonly StringName ShowBackstopAndHideUi = StringName.op_Implicit(nameof (ShowBackstopAndHideUi));
    public static readonly StringName OpenQueuedScreen = StringName.op_Implicit(nameof (OpenQueuedScreen));
    public static readonly StringName IsScreenQueued = StringName.op_Implicit(nameof (IsScreenQueued));
    public static readonly StringName IsInspectScreenQueued = StringName.op_Implicit(nameof (IsInspectScreenQueued));
    public static readonly StringName ShowHeaderAndActionsUi = StringName.op_Implicit(nameof (ShowHeaderAndActionsUi));
    public static readonly StringName DisableInput = StringName.op_Implicit(nameof (DisableInput));
    public static readonly StringName EnableInput = StringName.op_Implicit(nameof (EnableInput));
    public static readonly StringName RefreshBackButton = StringName.op_Implicit(nameof (RefreshBackButton));
    public static readonly StringName ResetScreen = StringName.op_Implicit(nameof (ResetScreen));
    public static readonly StringName GetReminderVfxHolder = StringName.op_Implicit(nameof (GetReminderVfxHolder));
  }

  public new class PropertyName : NSubmenu.PropertyName
  {
    public static readonly StringName CurrentUnlockScreen = StringName.op_Implicit(nameof (CurrentUnlockScreen));
    public new static readonly StringName InitialFocusedControl = StringName.op_Implicit(nameof (InitialFocusedControl));
    public static readonly StringName _inspectScreen = StringName.op_Implicit(nameof (_inspectScreen));
    public static readonly StringName _reminderText = StringName.op_Implicit(nameof (_reminderText));
    public static readonly StringName _reminderVfxHolder = StringName.op_Implicit(nameof (_reminderVfxHolder));
    public static readonly StringName _backstop = StringName.op_Implicit(nameof (_backstop));
    public static readonly StringName _inputBlocker = StringName.op_Implicit(nameof (_inputBlocker));
    public static readonly StringName _lineContainer = StringName.op_Implicit(nameof (_lineContainer));
    public static readonly StringName _line = StringName.op_Implicit(nameof (_line));
    public static readonly StringName _epochSlotContainer = StringName.op_Implicit(nameof (_epochSlotContainer));
    public static readonly StringName _slotsContainer = StringName.op_Implicit(nameof (_slotsContainer));
    public new static readonly StringName _backButton = StringName.op_Implicit(nameof (_backButton));
    public static readonly StringName _unlockScreenHolder = StringName.op_Implicit(nameof (_unlockScreenHolder));
    public static readonly StringName _tutorial = StringName.op_Implicit(nameof (_tutorial));
    public static readonly StringName _isUiVisible = StringName.op_Implicit(nameof (_isUiVisible));
    public static readonly StringName _queuedInspectScreen = StringName.op_Implicit(nameof (_queuedInspectScreen));
    public static readonly StringName _lineGrowTween = StringName.op_Implicit(nameof (_lineGrowTween));
    public static readonly StringName _backstopTween = StringName.op_Implicit(nameof (_backstopTween));
  }

  public new class SignalName : NSubmenu.SignalName
  {
  }
}
