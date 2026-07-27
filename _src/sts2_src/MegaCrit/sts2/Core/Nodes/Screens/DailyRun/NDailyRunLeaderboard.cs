// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.DailyRun.NDailyRunLeaderboard
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Daily;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Leaderboard;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Platform;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.DailyRun;

[ScriptPath("res://src/Core/Nodes/Screens/DailyRun/NDailyRunLeaderboard.cs")]
public class NDailyRunLeaderboard : Control
{
  private static readonly string _scenePath = SceneHelper.GetScenePath("screens/daily_run/daily_run_leaderboard");
  private const int _maxEntries = 10;
  private NLeaderboardDayPaginator _paginator;
  private VBoxContainer _scoreContainer;
  private NLeaderboardPageArrow _leftArrow;
  private NLeaderboardPageArrow _rightArrow;
  private MegaRichTextLabel _loadingIndicator;
  private MegaLabel _noScoresIndicator;
  private MegaLabel _noFriendsIndicator;
  private Control? _noScoreUploadIndicator;
  private Control _separators;
  private NGlobalRankTickbox _globalTickbox;
  private const int _defaultGlobalIndex = -4;
  private int _currentPage;
  private int _currentIndex = -4;
  private DateTimeOffset _todaysDailyTime;
  private DateTimeOffset? _leaderboardTime;
  private readonly List<ulong> _playersInRun = new List<ulong>();
  private bool _hasNegativeScore;
  private CancellationTokenSource? _loadCts;
  private static readonly LocString _titleLoc = new LocString("main_menu_ui", "DAILY_RUN_MENU.LEADERBOARDS.title");
  private static readonly LocString _scoreLoc = new LocString("main_menu_ui", "DAILY_RUN_MENU.LEADERBOARDS.noScore");
  private static readonly LocString _fetchingScoreLoc = new LocString("main_menu_ui", "DAILY_RUN_MENU.LEADERBOARDS.fetchingScores");
  private static readonly LocString _friendsLoc = new LocString("main_menu_ui", "DAILY_RUN_MENU.LEADERBOARDS.noFriends");

  public static string[] AssetPaths
  {
    get => new string[1]{ NDailyRunLeaderboard._scenePath };
  }

  public override void _Ready()
  {
    this._paginator = ((Node) this).GetNode<NLeaderboardDayPaginator>(NodePath.op_Implicit("Paginator"));
    VBoxContainer vboxContainer = ((Node) this).GetNodeOrNull<VBoxContainer>(NodePath.op_Implicit("%ScoreContainer"));
    if (vboxContainer == null)
      vboxContainer = ((Node) this).GetNodeOrNull<VBoxContainer>(NodePath.op_Implicit("%LeaderboardScoreContainer")) ?? throw new InvalidOperationException("Couldn't find score container");
    this._scoreContainer = vboxContainer;
    this._leftArrow = ((Node) this).GetNode<NLeaderboardPageArrow>(NodePath.op_Implicit("%LeftArrow"));
    this._rightArrow = ((Node) this).GetNode<NLeaderboardPageArrow>(NodePath.op_Implicit("%RightArrow"));
    this._loadingIndicator = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%LoadingText"));
    this._noScoresIndicator = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%NoScoresIndicator"));
    this._noFriendsIndicator = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%NoFriendsIndicator"));
    this._noScoreUploadIndicator = ((Node) this).GetNodeOrNull<Control>(NodePath.op_Implicit("%ScoreWarning"));
    this._separators = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%Separators"));
    this._globalTickbox = ((Node) this).GetNode<NGlobalRankTickbox>(NodePath.op_Implicit("%GlobalTickbox"));
    ((GodotObject) this._globalTickbox).Connect(NTickbox.SignalName.Toggled, Callable.From<NTickbox>(new Action<NTickbox>(this.ToggleGlobalRank)), 0U);
    this._globalTickbox.SetLabel(new LocString("main_menu_ui", "DAILY_RUN_MENU.LEADERBOARDS.globalTickbox").GetRawText());
    ((GodotObject) this).CallDeferred(NDailyRunLeaderboard.MethodName.SetLocalizedText, Array.Empty<Variant>());
    this._loadingIndicator.SetTextAutoSize(NDailyRunLeaderboard._fetchingScoreLoc.GetFormattedText());
    this._rightArrow.Connect((Action) (() => this.ChangePage(this._currentIndex += 10)));
    this._leftArrow.Connect((Action) (() => this.ChangePage(this._currentIndex -= 10)));
  }

  private void ToggleGlobalRank(NTickbox tickbox)
  {
    if (tickbox.IsTicked)
    {
      DateTimeOffset? leaderboardTime = this._leaderboardTime;
      if (!leaderboardTime.HasValue)
        return;
      DateTimeOffset valueOrDefault = leaderboardTime.GetValueOrDefault();
      this._currentIndex = -4;
      TaskHelper.RunSafely(this.LoadLeaderboard(valueOrDefault, this._currentPage, LeaderboardQueryType.AroundUser));
    }
    else
      this.SetPage(0);
  }

  private void SetLocalizedText()
  {
    ((Node) this).GetNodeOrNull<MegaLabel>(NodePath.op_Implicit("%Title"))?.SetTextAutoSize(NDailyRunLeaderboard._titleLoc.GetRawText());
    this._noScoresIndicator.SetTextAutoSize(NDailyRunLeaderboard._scoreLoc.GetRawText());
    this._noFriendsIndicator.SetTextAutoSize(NDailyRunLeaderboard._friendsLoc.GetRawText());
  }

  public void Cleanup()
  {
    this._loadCts?.Cancel();
    this._loadCts = (CancellationTokenSource) null;
    ((CanvasItem) this._leftArrow).Visible = false;
    ((CanvasItem) this._rightArrow).Visible = false;
    ((CanvasItem) this._loadingIndicator).Visible = false;
    ((CanvasItem) this._noScoresIndicator).Visible = false;
    ((CanvasItem) this._noFriendsIndicator).Visible = false;
    ((CanvasItem) this._paginator).Visible = false;
    ((CanvasItem) this._globalTickbox).Visible = false;
    this._currentIndex = -4;
    this._leaderboardTime = new DateTimeOffset?();
    if (this._noScoreUploadIndicator != null)
      ((CanvasItem) this._noScoreUploadIndicator).Visible = false;
    this.ClearEntries();
  }

  public void Initialize(
    DateTimeOffset dateTime,
    IEnumerable<ulong> playersInRun,
    bool allowPagination)
  {
    this._playersInRun.Clear();
    this._playersInRun.AddRange(playersInRun);
    this._paginator.Initialize(this, dateTime, allowPagination);
    ((CanvasItem) this._paginator).Visible = true;
    this._todaysDailyTime = dateTime;
    this.SetDay(dateTime);
  }

  public void SetDay(DateTimeOffset dateTime)
  {
    this._leaderboardTime = new DateTimeOffset?(dateTime);
    if (!this._globalTickbox.IsTicked)
      this.SetPage(0);
    else
      TaskHelper.RunSafely(this.LoadLeaderboard(dateTime, this._currentPage, LeaderboardQueryType.AroundUser));
  }

  private void ChangePage(int _ = 0) => this.NavigateGlobalRank();

  private void SetPage(int page)
  {
    DateTimeOffset? leaderboardTime = this._leaderboardTime;
    if (!leaderboardTime.HasValue)
      return;
    DateTimeOffset valueOrDefault = leaderboardTime.GetValueOrDefault();
    this._currentPage = page;
    TaskHelper.RunSafely(this.LoadLeaderboard(valueOrDefault, this._currentPage, LeaderboardQueryType.FriendsOnly));
  }

  private void NavigateGlobalRank()
  {
    DateTimeOffset? leaderboardTime = this._leaderboardTime;
    if (!leaderboardTime.HasValue)
      return;
    TaskHelper.RunSafely(this.LoadLeaderboard(leaderboardTime.GetValueOrDefault(), this._currentPage, LeaderboardQueryType.AroundUser));
  }

  private async Task LoadLeaderboard(
    DateTimeOffset dateTime,
    int page,
    LeaderboardQueryType queryType)
  {
    if (this._loadCts != null)
      await this._loadCts.CancelAsync();
    this._loadCts = new CancellationTokenSource();
    CancellationToken ct = this._loadCts.Token;
    this.ClearEntries();
    this._rightArrow.Disable();
    this._leftArrow.Disable();
    this._paginator.Disable();
    ((CanvasItem) this._noFriendsIndicator).Visible = false;
    ((CanvasItem) this._noScoresIndicator).Visible = false;
    ((CanvasItem) this._loadingIndicator).Visible = true;
    ((CanvasItem) this._separators).Visible = false;
    ((CanvasItem) this._globalTickbox).Visible = false;
    try
    {
      string leaderboardName = DailyRunUtility.GetLeaderboardName(dateTime, this._playersInRun.Count);
      DateTimeOffset? nullable1 = DailyRunUtility.AddLeaderboardDays(dateTime, -1);
      DateTimeOffset? rightLeaderboardTime = DailyRunUtility.AddLeaderboardDays(dateTime, 1);
      Task<ILeaderboardHandle> mainTask = LeaderboardManager.GetLeaderboard(leaderboardName, ct);
      Task<ILeaderboardHandle> leftTask = !nullable1.HasValue ? Task.FromResult<ILeaderboardHandle>((ILeaderboardHandle) null) : LeaderboardManager.GetLeaderboard(DailyRunUtility.GetLeaderboardName(nullable1.GetValueOrDefault(), this._playersInRun.Count), ct);
      Task<ILeaderboardHandle> rightTask = !rightLeaderboardTime.HasValue ? Task.FromResult<ILeaderboardHandle>((ILeaderboardHandle) null) : LeaderboardManager.GetLeaderboard(DailyRunUtility.GetLeaderboardName(rightLeaderboardTime.GetValueOrDefault(), this._playersInRun.Count), ct);
      \u003C\u003Ey__InlineArray3<Task<ILeaderboardHandle>> buffer = new \u003C\u003Ey__InlineArray3<Task<ILeaderboardHandle>>();
      // ISSUE: reference to a compiler-generated method
      \u003CPrivateImplementationDetails\u003E.InlineArrayElementRef<\u003C\u003Ey__InlineArray3<Task<ILeaderboardHandle>>, Task<ILeaderboardHandle>>(ref buffer, 0) = mainTask;
      // ISSUE: reference to a compiler-generated method
      \u003CPrivateImplementationDetails\u003E.InlineArrayElementRef<\u003C\u003Ey__InlineArray3<Task<ILeaderboardHandle>>, Task<ILeaderboardHandle>>(ref buffer, 1) = leftTask;
      // ISSUE: reference to a compiler-generated method
      \u003CPrivateImplementationDetails\u003E.InlineArrayElementRef<\u003C\u003Ey__InlineArray3<Task<ILeaderboardHandle>>, Task<ILeaderboardHandle>>(ref buffer, 2) = rightTask;
      // ISSUE: reference to a compiler-generated method
      ILeaderboardHandle[] leaderboardHandleArray = await Task.WhenAll<ILeaderboardHandle>(\u003CPrivateImplementationDetails\u003E.InlineArrayAsReadOnlySpan<\u003C\u003Ey__InlineArray3<Task<ILeaderboardHandle>>, Task<ILeaderboardHandle>>(in buffer, 3));
      ILeaderboardHandle handle = await mainTask;
      if (ct.IsCancellationRequested)
      {
        ct = new CancellationToken();
      }
      else
      {
        if (handle != null)
        {
          if (queryType == LeaderboardQueryType.FriendsOnly)
            await this.QueryFriendScores(handle, page, ct);
          else if (queryType == LeaderboardQueryType.AroundUser)
            await this.QueryGlobalScores(handle, ct);
        }
        else
        {
          ((CanvasItem) this._noScoresIndicator).Visible = true;
          ((CanvasItem) this._leftArrow).Visible = false;
          ((CanvasItem) this._rightArrow).Visible = false;
          ((CanvasItem) this._separators).Visible = false;
          ((CanvasItem) this._globalTickbox).Visible = false;
        }
        bool hasLeftLeaderboard = await leftTask != null;
        int num;
        if (await rightTask == null)
        {
          DateTimeOffset? nullable2 = rightLeaderboardTime;
          DateTimeOffset todaysDailyTime = this._todaysDailyTime;
          num = nullable2.HasValue ? (nullable2.GetValueOrDefault() == todaysDailyTime ? 1 : 0) : 0;
        }
        else
          num = 1;
        bool rightArrowEnabled = num != 0;
        this._currentPage = page;
        this._paginator.Enable(hasLeftLeaderboard, rightArrowEnabled);
        ((CanvasItem) this._loadingIndicator).Visible = false;
        if (this._noScoreUploadIndicator != null && this._todaysDailyTime == dateTime)
        {
          bool flag = await DailyRunUtility.ShouldUploadScore(handle, (IReadOnlyList<ulong>) this._playersInRun, ct);
          if (ct.IsCancellationRequested)
          {
            ct = new CancellationToken();
            return;
          }
          ((CanvasItem) this._noScoreUploadIndicator).Visible = !flag;
        }
        mainTask = (Task<ILeaderboardHandle>) null;
        leftTask = (Task<ILeaderboardHandle>) null;
        rightTask = (Task<ILeaderboardHandle>) null;
        handle = (ILeaderboardHandle) null;
        ct = new CancellationToken();
      }
    }
    catch (OperationCanceledException ex)
    {
      ct = new CancellationToken();
    }
  }

  private async Task QueryFriendScores(ILeaderboardHandle handle, int page, CancellationToken ct)
  {
    List<LeaderboardEntry> entries = await LeaderboardManager.QueryLeaderboard(handle, LeaderboardQueryType.FriendsOnly, page * 10, 10, ct);
    if (ct.IsCancellationRequested)
      return;
    if (entries.Count > 0)
    {
      ((CanvasItem) this._noScoresIndicator).Visible = false;
      ((CanvasItem) this._globalTickbox).Visible = true;
      ((CanvasItem) this._separators).Visible = true;
    }
    else
    {
      ((CanvasItem) this._noScoresIndicator).Visible = true;
      ((CanvasItem) this._globalTickbox).Visible = false;
      ((CanvasItem) this._separators).Visible = false;
    }
    this.FillEntries(entries);
    ((CanvasItem) this._leftArrow).Visible = false;
    ((CanvasItem) this._rightArrow).Visible = false;
  }

  private async Task QueryGlobalScores(ILeaderboardHandle handle, CancellationToken ct)
  {
    List<LeaderboardEntry> entries = await LeaderboardManager.QueryLeaderboard(handle, LeaderboardQueryType.AroundUser, this._currentIndex, 10, ct);
    if (ct.IsCancellationRequested)
      return;
    if (entries.Count > 0)
    {
      ((CanvasItem) this._noScoresIndicator).Visible = false;
      ((CanvasItem) this._globalTickbox).Visible = true;
      ((CanvasItem) this._separators).Visible = true;
    }
    else
    {
      ((CanvasItem) this._noScoresIndicator).Visible = true;
      ((CanvasItem) this._globalTickbox).Visible = false;
      ((CanvasItem) this._separators).Visible = false;
    }
    this.FillEntries(entries);
    if (entries.Count <= 0)
      return;
    int leaderboardEntryCount = LeaderboardManager.GetLeaderboardEntryCount(handle);
    ((CanvasItem) this._leftArrow).Visible = true;
    ((CanvasItem) this._rightArrow).Visible = true;
    if (entries[0].rank > 0)
      this._leftArrow.Enable();
    else
      this._leftArrow.Disable();
    List<LeaderboardEntry> leaderboardEntryList = entries;
    if (leaderboardEntryList[leaderboardEntryList.Count - 1].rank < leaderboardEntryCount - 1 && !this._hasNegativeScore)
      this._rightArrow.Enable();
    else
      this._rightArrow.Disable();
  }

  private void FillEntries(List<LeaderboardEntry> entries)
  {
    if (entries.Count == 0)
      return;
    this._hasNegativeScore = false;
    ((Node) this._scoreContainer).AddChildSafely((Node) NDailyRunLeaderboardHeader.Create());
    ((Node) this._scoreContainer).AddChildSafely((Node) NDailyRunLeaderboardSeparator.Create());
    ulong localPlayerId = PlatformUtil.GetLocalPlayerId(LeaderboardManager.CurrentPlatform);
    foreach (LeaderboardEntry entry in entries)
    {
      if (entry.score < 0)
      {
        this._hasNegativeScore = true;
      }
      else
      {
        ((Node) this._scoreContainer).AddChildSafely((Node) NDailyRunLeaderboardRow.Create(entry, entry.userIds.Contains(localPlayerId)));
        ((Node) this._scoreContainer).AddChildSafely((Node) NDailyRunLeaderboardSeparator.Create());
      }
    }
  }

  private void ClearEntries()
  {
    ((CanvasItem) this._noScoresIndicator).Visible = false;
    ((CanvasItem) this._noFriendsIndicator).Visible = false;
    foreach (Node child in ((Node) this._scoreContainer).GetChildren(false))
      child.QueueFreeSafely();
  }

  public override void _ExitTree()
  {
    this._loadCts?.Cancel();
    this._loadCts = (CancellationTokenSource) null;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(9)
    {
      new MethodInfo(NDailyRunLeaderboard.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDailyRunLeaderboard.MethodName.ToggleGlobalRank, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("tickbox"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NDailyRunLeaderboard.MethodName.SetLocalizedText, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDailyRunLeaderboard.MethodName.Cleanup, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDailyRunLeaderboard.MethodName.ChangePage, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NDailyRunLeaderboard.MethodName.SetPage, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("page"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NDailyRunLeaderboard.MethodName.NavigateGlobalRank, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDailyRunLeaderboard.MethodName.ClearEntries, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDailyRunLeaderboard.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NDailyRunLeaderboard.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDailyRunLeaderboard.MethodName.ToggleGlobalRank) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.ToggleGlobalRank(VariantUtils.ConvertTo<NTickbox>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDailyRunLeaderboard.MethodName.SetLocalizedText) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.SetLocalizedText();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDailyRunLeaderboard.MethodName.Cleanup) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Cleanup();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDailyRunLeaderboard.MethodName.ChangePage) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.ChangePage(VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDailyRunLeaderboard.MethodName.SetPage) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetPage(VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDailyRunLeaderboard.MethodName.NavigateGlobalRank) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.NavigateGlobalRank();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDailyRunLeaderboard.MethodName.ClearEntries) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ClearEntries();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NDailyRunLeaderboard.MethodName._ExitTree) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ((Node) this)._ExitTree();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NDailyRunLeaderboard.MethodName._Ready) || StringName.op_Equality(ref method, NDailyRunLeaderboard.MethodName.ToggleGlobalRank) || StringName.op_Equality(ref method, NDailyRunLeaderboard.MethodName.SetLocalizedText) || StringName.op_Equality(ref method, NDailyRunLeaderboard.MethodName.Cleanup) || StringName.op_Equality(ref method, NDailyRunLeaderboard.MethodName.ChangePage) || StringName.op_Equality(ref method, NDailyRunLeaderboard.MethodName.SetPage) || StringName.op_Equality(ref method, NDailyRunLeaderboard.MethodName.NavigateGlobalRank) || StringName.op_Equality(ref method, NDailyRunLeaderboard.MethodName.ClearEntries) || StringName.op_Equality(ref method, NDailyRunLeaderboard.MethodName._ExitTree) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NDailyRunLeaderboard.PropertyName._paginator))
    {
      this._paginator = VariantUtils.ConvertTo<NLeaderboardDayPaginator>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunLeaderboard.PropertyName._scoreContainer))
    {
      this._scoreContainer = VariantUtils.ConvertTo<VBoxContainer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunLeaderboard.PropertyName._leftArrow))
    {
      this._leftArrow = VariantUtils.ConvertTo<NLeaderboardPageArrow>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunLeaderboard.PropertyName._rightArrow))
    {
      this._rightArrow = VariantUtils.ConvertTo<NLeaderboardPageArrow>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunLeaderboard.PropertyName._loadingIndicator))
    {
      this._loadingIndicator = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunLeaderboard.PropertyName._noScoresIndicator))
    {
      this._noScoresIndicator = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunLeaderboard.PropertyName._noFriendsIndicator))
    {
      this._noFriendsIndicator = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunLeaderboard.PropertyName._noScoreUploadIndicator))
    {
      this._noScoreUploadIndicator = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunLeaderboard.PropertyName._separators))
    {
      this._separators = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunLeaderboard.PropertyName._globalTickbox))
    {
      this._globalTickbox = VariantUtils.ConvertTo<NGlobalRankTickbox>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunLeaderboard.PropertyName._currentPage))
    {
      this._currentPage = VariantUtils.ConvertTo<int>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunLeaderboard.PropertyName._currentIndex))
    {
      this._currentIndex = VariantUtils.ConvertTo<int>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NDailyRunLeaderboard.PropertyName._hasNegativeScore))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._hasNegativeScore = VariantUtils.ConvertTo<bool>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NDailyRunLeaderboard.PropertyName._paginator))
    {
      value = VariantUtils.CreateFrom<NLeaderboardDayPaginator>(ref this._paginator);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunLeaderboard.PropertyName._scoreContainer))
    {
      value = VariantUtils.CreateFrom<VBoxContainer>(ref this._scoreContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunLeaderboard.PropertyName._leftArrow))
    {
      value = VariantUtils.CreateFrom<NLeaderboardPageArrow>(ref this._leftArrow);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunLeaderboard.PropertyName._rightArrow))
    {
      value = VariantUtils.CreateFrom<NLeaderboardPageArrow>(ref this._rightArrow);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunLeaderboard.PropertyName._loadingIndicator))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._loadingIndicator);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunLeaderboard.PropertyName._noScoresIndicator))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._noScoresIndicator);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunLeaderboard.PropertyName._noFriendsIndicator))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._noFriendsIndicator);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunLeaderboard.PropertyName._noScoreUploadIndicator))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._noScoreUploadIndicator);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunLeaderboard.PropertyName._separators))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._separators);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunLeaderboard.PropertyName._globalTickbox))
    {
      value = VariantUtils.CreateFrom<NGlobalRankTickbox>(ref this._globalTickbox);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunLeaderboard.PropertyName._currentPage))
    {
      value = VariantUtils.CreateFrom<int>(ref this._currentPage);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunLeaderboard.PropertyName._currentIndex))
    {
      value = VariantUtils.CreateFrom<int>(ref this._currentIndex);
      return true;
    }
    if (!StringName.op_Equality(ref name, NDailyRunLeaderboard.PropertyName._hasNegativeScore))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<bool>(ref this._hasNegativeScore);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NDailyRunLeaderboard.PropertyName._paginator, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDailyRunLeaderboard.PropertyName._scoreContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDailyRunLeaderboard.PropertyName._leftArrow, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDailyRunLeaderboard.PropertyName._rightArrow, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDailyRunLeaderboard.PropertyName._loadingIndicator, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDailyRunLeaderboard.PropertyName._noScoresIndicator, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDailyRunLeaderboard.PropertyName._noFriendsIndicator, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDailyRunLeaderboard.PropertyName._noScoreUploadIndicator, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDailyRunLeaderboard.PropertyName._separators, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDailyRunLeaderboard.PropertyName._globalTickbox, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NDailyRunLeaderboard.PropertyName._currentPage, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NDailyRunLeaderboard.PropertyName._currentIndex, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NDailyRunLeaderboard.PropertyName._hasNegativeScore, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NDailyRunLeaderboard.PropertyName._paginator, Variant.From<NLeaderboardDayPaginator>(ref this._paginator));
    info.AddProperty(NDailyRunLeaderboard.PropertyName._scoreContainer, Variant.From<VBoxContainer>(ref this._scoreContainer));
    info.AddProperty(NDailyRunLeaderboard.PropertyName._leftArrow, Variant.From<NLeaderboardPageArrow>(ref this._leftArrow));
    info.AddProperty(NDailyRunLeaderboard.PropertyName._rightArrow, Variant.From<NLeaderboardPageArrow>(ref this._rightArrow));
    info.AddProperty(NDailyRunLeaderboard.PropertyName._loadingIndicator, Variant.From<MegaRichTextLabel>(ref this._loadingIndicator));
    info.AddProperty(NDailyRunLeaderboard.PropertyName._noScoresIndicator, Variant.From<MegaLabel>(ref this._noScoresIndicator));
    info.AddProperty(NDailyRunLeaderboard.PropertyName._noFriendsIndicator, Variant.From<MegaLabel>(ref this._noFriendsIndicator));
    info.AddProperty(NDailyRunLeaderboard.PropertyName._noScoreUploadIndicator, Variant.From<Control>(ref this._noScoreUploadIndicator));
    info.AddProperty(NDailyRunLeaderboard.PropertyName._separators, Variant.From<Control>(ref this._separators));
    info.AddProperty(NDailyRunLeaderboard.PropertyName._globalTickbox, Variant.From<NGlobalRankTickbox>(ref this._globalTickbox));
    info.AddProperty(NDailyRunLeaderboard.PropertyName._currentPage, Variant.From<int>(ref this._currentPage));
    info.AddProperty(NDailyRunLeaderboard.PropertyName._currentIndex, Variant.From<int>(ref this._currentIndex));
    info.AddProperty(NDailyRunLeaderboard.PropertyName._hasNegativeScore, Variant.From<bool>(ref this._hasNegativeScore));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NDailyRunLeaderboard.PropertyName._paginator, ref variant1))
      this._paginator = ((Variant) ref variant1).As<NLeaderboardDayPaginator>();
    Variant variant2;
    if (info.TryGetProperty(NDailyRunLeaderboard.PropertyName._scoreContainer, ref variant2))
      this._scoreContainer = ((Variant) ref variant2).As<VBoxContainer>();
    Variant variant3;
    if (info.TryGetProperty(NDailyRunLeaderboard.PropertyName._leftArrow, ref variant3))
      this._leftArrow = ((Variant) ref variant3).As<NLeaderboardPageArrow>();
    Variant variant4;
    if (info.TryGetProperty(NDailyRunLeaderboard.PropertyName._rightArrow, ref variant4))
      this._rightArrow = ((Variant) ref variant4).As<NLeaderboardPageArrow>();
    Variant variant5;
    if (info.TryGetProperty(NDailyRunLeaderboard.PropertyName._loadingIndicator, ref variant5))
      this._loadingIndicator = ((Variant) ref variant5).As<MegaRichTextLabel>();
    Variant variant6;
    if (info.TryGetProperty(NDailyRunLeaderboard.PropertyName._noScoresIndicator, ref variant6))
      this._noScoresIndicator = ((Variant) ref variant6).As<MegaLabel>();
    Variant variant7;
    if (info.TryGetProperty(NDailyRunLeaderboard.PropertyName._noFriendsIndicator, ref variant7))
      this._noFriendsIndicator = ((Variant) ref variant7).As<MegaLabel>();
    Variant variant8;
    if (info.TryGetProperty(NDailyRunLeaderboard.PropertyName._noScoreUploadIndicator, ref variant8))
      this._noScoreUploadIndicator = ((Variant) ref variant8).As<Control>();
    Variant variant9;
    if (info.TryGetProperty(NDailyRunLeaderboard.PropertyName._separators, ref variant9))
      this._separators = ((Variant) ref variant9).As<Control>();
    Variant variant10;
    if (info.TryGetProperty(NDailyRunLeaderboard.PropertyName._globalTickbox, ref variant10))
      this._globalTickbox = ((Variant) ref variant10).As<NGlobalRankTickbox>();
    Variant variant11;
    if (info.TryGetProperty(NDailyRunLeaderboard.PropertyName._currentPage, ref variant11))
      this._currentPage = ((Variant) ref variant11).As<int>();
    Variant variant12;
    if (info.TryGetProperty(NDailyRunLeaderboard.PropertyName._currentIndex, ref variant12))
      this._currentIndex = ((Variant) ref variant12).As<int>();
    Variant variant13;
    if (!info.TryGetProperty(NDailyRunLeaderboard.PropertyName._hasNegativeScore, ref variant13))
      return;
    this._hasNegativeScore = ((Variant) ref variant13).As<bool>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName ToggleGlobalRank = StringName.op_Implicit(nameof (ToggleGlobalRank));
    public static readonly StringName SetLocalizedText = StringName.op_Implicit(nameof (SetLocalizedText));
    public static readonly StringName Cleanup = StringName.op_Implicit(nameof (Cleanup));
    public static readonly StringName ChangePage = StringName.op_Implicit(nameof (ChangePage));
    public static readonly StringName SetPage = StringName.op_Implicit(nameof (SetPage));
    public static readonly StringName NavigateGlobalRank = StringName.op_Implicit(nameof (NavigateGlobalRank));
    public static readonly StringName ClearEntries = StringName.op_Implicit(nameof (ClearEntries));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName _paginator = StringName.op_Implicit(nameof (_paginator));
    public static readonly StringName _scoreContainer = StringName.op_Implicit(nameof (_scoreContainer));
    public static readonly StringName _leftArrow = StringName.op_Implicit(nameof (_leftArrow));
    public static readonly StringName _rightArrow = StringName.op_Implicit(nameof (_rightArrow));
    public static readonly StringName _loadingIndicator = StringName.op_Implicit(nameof (_loadingIndicator));
    public static readonly StringName _noScoresIndicator = StringName.op_Implicit(nameof (_noScoresIndicator));
    public static readonly StringName _noFriendsIndicator = StringName.op_Implicit(nameof (_noFriendsIndicator));
    public static readonly StringName _noScoreUploadIndicator = StringName.op_Implicit(nameof (_noScoreUploadIndicator));
    public static readonly StringName _separators = StringName.op_Implicit(nameof (_separators));
    public static readonly StringName _globalTickbox = StringName.op_Implicit(nameof (_globalTickbox));
    public static readonly StringName _currentPage = StringName.op_Implicit(nameof (_currentPage));
    public static readonly StringName _currentIndex = StringName.op_Implicit(nameof (_currentIndex));
    public static readonly StringName _hasNegativeScore = StringName.op_Implicit(nameof (_hasNegativeScore));
  }

  public class SignalName : Control.SignalName
  {
  }
}
