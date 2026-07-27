// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.RunHistoryScreen.NMapPointHistoryEntry
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
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.RunHistoryScreen;

[ScriptPath("res://src/Core/Nodes/Screens/RunHistoryScreen/NMapPointHistoryEntry.cs")]
public class NMapPointHistoryEntry : NClickableControl
{
  private Vector2 _baseScale = Vector2.op_Multiply(Vector2.One, 0.7f);
  private RunHistory _runHistory;
  private MapPointHistoryEntry _entry;
  private RunHistoryPlayer? _player;
  private TextureRect _texture;
  private TextureRect _outline;
  private TextureRect _questIcon;
  private Tween? _animateInTween;
  private Tween? _hoverTween;
  private float _baseAngle;
  private bool _hurryUp;

  private static string ScenePath
  {
    get => SceneHelper.GetScenePath("screens/run_history_screen/map_point_history_entry");
  }

  public static IEnumerable<string> AssetPaths => NMapPointHistoryEntry.GetAssetPaths();

  private static IEnumerable<string> GetAssetPaths()
  {
    yield return NMapPointHistoryEntry.ScenePath;
    RoomType[] roomTypeArray = new RoomType[6]
    {
      RoomType.Monster,
      RoomType.Elite,
      RoomType.Event,
      RoomType.Shop,
      RoomType.Treasure,
      RoomType.RestSite
    };
    int index;
    RoomType roomType;
    for (index = 0; index < roomTypeArray.Length; ++index)
    {
      roomType = roomTypeArray[index];
      string roomIconPath = ImageHelper.GetRoomIconPath(MapPointType.Monster, roomType, (ModelId) null);
      if (roomIconPath != null)
        yield return roomIconPath;
      string roomIconOutlinePath = ImageHelper.GetRoomIconOutlinePath(MapPointType.Monster, roomType, (ModelId) null);
      if (roomIconOutlinePath != null)
        yield return roomIconOutlinePath;
    }
    roomTypeArray = (RoomType[]) null;
    roomTypeArray = new RoomType[4]
    {
      RoomType.Monster,
      RoomType.Elite,
      RoomType.Shop,
      RoomType.Treasure
    };
    for (index = 0; index < roomTypeArray.Length; ++index)
    {
      roomType = roomTypeArray[index];
      string roomIconPath = ImageHelper.GetRoomIconPath(MapPointType.Unknown, roomType, (ModelId) null);
      if (roomIconPath != null)
        yield return roomIconPath;
      string roomIconOutlinePath = ImageHelper.GetRoomIconOutlinePath(MapPointType.Unknown, roomType, (ModelId) null);
      if (roomIconOutlinePath != null)
        yield return roomIconOutlinePath;
    }
    roomTypeArray = (RoomType[]) null;
    foreach (EncounterModel encounter in ModelDb.AllEncounters.Where<EncounterModel>((Func<EncounterModel, bool>) (e => e.RoomType == RoomType.Boss)))
    {
      string roomIconPath = ImageHelper.GetRoomIconPath(MapPointType.Boss, RoomType.Boss, encounter.Id);
      if (roomIconPath != null)
        yield return roomIconPath;
      string roomIconOutlinePath = ImageHelper.GetRoomIconOutlinePath(MapPointType.Boss, RoomType.Boss, encounter.Id);
      if (roomIconOutlinePath != null)
        yield return roomIconOutlinePath;
    }
    foreach (AncientEventModel ancient in ModelDb.AllAncients)
    {
      string roomIconPath = ImageHelper.GetRoomIconPath(MapPointType.Ancient, RoomType.Event, ancient.Id);
      if (roomIconPath != null)
        yield return roomIconPath;
      string roomIconOutlinePath = ImageHelper.GetRoomIconOutlinePath(MapPointType.Ancient, RoomType.Event, ancient.Id);
      if (roomIconOutlinePath != null)
        yield return roomIconOutlinePath;
    }
  }

  public int FloorNum { get; private set; }

  public override void _Ready()
  {
    this._baseAngle = Rng.Chaotic.NextGaussianFloat(max: 5f);
    this._texture = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%Icon"));
    this._outline = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%Outline"));
    this._questIcon = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%QuestIcon"));
    ((Control) this._texture).RotationDegrees = Rng.Chaotic.NextBool() ? this._baseAngle : -this._baseAngle;
    MapPointType mapPointType1 = this._entry.MapPointType;
    RoomType roomType1 = this._entry.Rooms.First<MapPointRoomHistoryEntry>().RoomType;
    bool flag1;
    switch (this._entry.MapPointType)
    {
      case MapPointType.Boss:
      case MapPointType.Ancient:
        flag1 = true;
        break;
      default:
        flag1 = false;
        break;
    }
    string roomIconPath = ImageHelper.GetRoomIconPath(mapPointType1, roomType1, flag1 ? this._entry.Rooms.First<MapPointRoomHistoryEntry>().ModelId : (ModelId) null);
    MapPointType mapPointType2 = this._entry.MapPointType;
    RoomType roomType2 = this._entry.Rooms.First<MapPointRoomHistoryEntry>().RoomType;
    bool flag2;
    switch (this._entry.MapPointType)
    {
      case MapPointType.Boss:
      case MapPointType.Ancient:
        flag2 = true;
        break;
      default:
        flag2 = false;
        break;
    }
    string roomIconOutlinePath = ImageHelper.GetRoomIconOutlinePath(mapPointType2, roomType2, flag2 ? this._entry.Rooms.First<MapPointRoomHistoryEntry>().ModelId : (ModelId) null);
    if (roomIconPath != null)
    {
      ((CanvasItem) this._texture).Visible = true;
      this._texture.Texture = (Texture2D) PreloadManager.Cache.GetCompressedTexture2D(roomIconPath);
    }
    else
      ((CanvasItem) this._texture).Visible = false;
    if (roomIconOutlinePath != null)
    {
      ((CanvasItem) this._outline).Visible = true;
      this._outline.Texture = (Texture2D) PreloadManager.Cache.GetCompressedTexture2D(roomIconOutlinePath);
    }
    else
      ((CanvasItem) this._outline).Visible = false;
    ((CanvasItem) this._questIcon).Visible = false;
    Color modulate = ((CanvasItem) this).Modulate;
    modulate.A = 1f;
    ((CanvasItem) this).Modulate = modulate;
    this.ConnectSignals();
  }

  public static NMapPointHistoryEntry Create(
    RunHistory history,
    MapPointHistoryEntry entry,
    int floorNum)
  {
    NMapPointHistoryEntry pointHistoryEntry = PreloadManager.Cache.GetScene(NMapPointHistoryEntry.ScenePath).Instantiate<NMapPointHistoryEntry>((PackedScene.GenEditState) 0L);
    pointHistoryEntry._runHistory = history;
    pointHistoryEntry._entry = entry;
    pointHistoryEntry.FloorNum = floorNum;
    return pointHistoryEntry;
  }

  public void SetPlayer(RunHistoryPlayer player)
  {
    this._player = player;
    PlayerMapPointHistoryEntry entry = this._entry.GetEntry(this._player.Id);
    ((CanvasItem) this._questIcon).Visible = entry.CompletedQuests.Count > 0 || entry.IsAffectedByFurCoat;
  }

  protected override void OnFocus()
  {
    if (this._player == null)
      throw new InvalidOperationException("Player has not been set!");
    this.Highlight();
    HoverTipAlignment alignment = HoverTip.GetHoverTipAlignment((Control) this);
    NHoverTipSet tip = NHoverTipSet.CreateAndShowMapPointHistory((Control) this, NMapPointHistoryHoverTip.Create(this.FloorNum, this._player.Id, this._entry));
    Callable callable = Callable.From((Action) (() => tip.SetAlignment((Control) this, alignment)));
    ((Callable) ref callable).CallDeferred(Array.Empty<Variant>());
    NHoverTipSet nhoverTipSet = tip;
    nhoverTipSet.GlobalPosition = Vector2.op_Addition(nhoverTipSet.GlobalPosition, Vector2.op_Multiply(Vector2.Down, 96f));
  }

  protected override void OnUnfocus()
  {
    this.Unhighlight();
    NHoverTipSet.Remove((Control) this);
  }

  public void Highlight()
  {
    this._hoverTween?.Kill();
    this._hoverTween = ((Node) this).CreateTween().SetParallel(true);
    this._hoverTween.TweenProperty((GodotObject) this._texture, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(this._baseScale, 1.5f)), 0.05).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._hoverTween.TweenProperty((GodotObject) this._texture, NodePath.op_Implicit("rotation_degrees"), Variant.op_Implicit(0.0f), 0.05).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._hoverTween.TweenProperty((GodotObject) this._outline, NodePath.op_Implicit("modulate"), Variant.op_Implicit(StsColors.halfTransparentWhite), 0.05).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
  }

  public void Unhighlight()
  {
    this._hoverTween?.Kill();
    this._hoverTween = ((Node) this).CreateTween().SetParallel(true);
    this._hoverTween.TweenProperty((GodotObject) this._texture, NodePath.op_Implicit("scale"), Variant.op_Implicit(this._baseScale), 1.0).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._hoverTween.TweenProperty((GodotObject) this._texture, NodePath.op_Implicit("rotation_degrees"), Variant.op_Implicit(this._baseAngle), 1.0).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._hoverTween.TweenProperty((GodotObject) this._outline, NodePath.op_Implicit("modulate"), Variant.op_Implicit(StsColors.quarterTransparentBlack), 1.0).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
  }

  public async Task AnimateIn(int index)
  {
    ((CanvasItem) this).Visible = true;
    this.Scale = Vector2.op_Multiply(Vector2.One, 0.01f);
    this._animateInTween?.Kill();
    this._animateInTween = ((Node) this).CreateTween().SetParallel(true);
    this._animateInTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), this._hurryUp ? 0.05 : 0.4).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 11L);
    this._animateInTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.White), this._hurryUp ? 0.05 : 0.1);
    TaskHelper.RunSafely(this.DoAnimateInEffects());
    if (this._hurryUp)
      await Cmd.Wait(0.05f);
    else
      await Cmd.Wait(Mathf.Lerp(SaveManager.Instance.PrefsSave.FastMode == FastModeType.Fast ? 0.4f : 0.5f, 0.2f, (float) index / (float) (index + (SaveManager.Instance.PrefsSave.FastMode == FastModeType.Fast ? 5 : 9))));
  }

  private async Task DoAnimateInEffects()
  {
    PlayerMapPointHistoryEntry entry = this._entry.GetEntry(this._player.Id);
    RoomType roomType = this._entry.Rooms.Last<MapPointRoomHistoryEntry>().RoomType;
    if (this._runHistory.MapPointHistory.Last<List<MapPointHistoryEntry>>().Last<MapPointHistoryEntry>() == this._entry && !this._runHistory.Win)
    {
      SfxCmd.Play("event:/sfx/block_break");
      NDebugAudioManager.Instance?.Play("STS_DeathStinger_v4_Short_SFX.mp3", 0.75f * this.GetSfxVolume());
    }
    else
    {
      bool flag;
      switch (roomType)
      {
        case RoomType.Monster:
        case RoomType.Elite:
        case RoomType.Boss:
          flag = true;
          break;
        default:
          flag = false;
          break;
      }
      if (flag)
      {
        await this.DoCombatAnimateInEffects(roomType);
      }
      else
      {
        switch (roomType)
        {
          case RoomType.Treasure:
            SfxCmd.Play("event:/sfx/ui/gold/gold_2", this.GetSfxVolume());
            break;
          case RoomType.Shop:
            SfxCmd.Play("event:/sfx/npcs/merchant/merchant_welcome");
            break;
          case RoomType.Event:
            SfxCmd.Play("event:/sfx/ui/clicks/ui_hover");
            break;
          case RoomType.RestSite:
            if (entry.RestSiteChoices.Contains("SMITH"))
            {
              NGame.Instance.ScreenRumble(ShakeStrength.Medium, ShakeDuration.Short, RumbleStyle.Rumble);
              NDebugAudioManager instance = NDebugAudioManager.Instance;
              if (instance == null)
                break;
              instance.Play("card_smith.mp3", this.GetSfxVolume(), PitchVariance.Small);
              break;
            }
            if (entry.RestSiteChoices.Contains("HEAL"))
            {
              NDebugAudioManager instance = NDebugAudioManager.Instance;
              if (instance == null)
                break;
              instance.Play("SOTE_SFX_SleepBlanket_v1.mp3", this.GetSfxVolume(), PitchVariance.Medium);
              break;
            }
            if (entry.RestSiteChoices.Contains("DIG"))
            {
              NDebugAudioManager.Instance.Play("sts_sfx_shovel_v1.mp3", this.GetSfxVolume(), PitchVariance.Small);
              break;
            }
            if (entry.RestSiteChoices.Contains("HATCH"))
            {
              SfxCmd.Play("event:/sfx/byrdpip/byrdpip_attack");
              break;
            }
            if (entry.RestSiteChoices.Contains("LIFT"))
            {
              NGame.Instance?.ScreenShake(ShakeStrength.Strong, ShakeDuration.Short);
              break;
            }
            if (!entry.RestSiteChoices.Contains("MEND"))
              break;
            break;
        }
      }
    }
  }

  private async Task DoCombatAnimateInEffects(RoomType roomType)
  {
    CharacterModel character = SaveUtil.CharacterOrDeprecated(this._player.Character);
    ShakeStrength? shakeStrength = new ShakeStrength?();
    switch (roomType)
    {
      case RoomType.Monster:
        shakeStrength = new ShakeStrength?(ShakeStrength.Weak);
        await this.PlaySfx(this.GetSmallHitSfx(character));
        break;
      case RoomType.Elite:
        shakeStrength = new ShakeStrength?(ShakeStrength.Medium);
        await this.PlaySfx(this.GetBigHitSfx(character));
        break;
      case RoomType.Boss:
        shakeStrength = new ShakeStrength?(ShakeStrength.Strong);
        await this.PlaySfx(this.GetBigHitSfx(character));
        break;
    }
    if (shakeStrength.HasValue)
      NGame.Instance.ScreenRumble(shakeStrength.Value, ShakeDuration.Normal, RumbleStyle.Rumble);
    await Cmd.Wait(0.25f);
    foreach (ModelId monsterId in this._entry.Rooms.Last<MapPointRoomHistoryEntry>().MonsterIds)
    {
      MonsterModel monsterModel = SaveUtil.MonsterOrDeprecated(monsterId);
      if (monsterModel.HasDeathSfx)
      {
        SfxCmd.Play(monsterModel.DeathSfx);
        await Cmd.Wait(0.25f);
      }
    }
  }

  private List<string> GetSmallHitSfx(CharacterModel character)
  {
    List<string> smallHitSfx;
    switch (character)
    {
      case Defect _:
        int capacity1 = 1;
        List<string> stringList1 = new List<string>(capacity1);
        CollectionsMarshal.SetCount<string>(stringList1, capacity1);
        CollectionsMarshal.AsSpan<string>(stringList1)[0] = "slash_attack.mp3";
        smallHitSfx = stringList1;
        break;
      case Ironclad _:
        int capacity2 = 1;
        List<string> stringList2 = new List<string>(capacity2);
        CollectionsMarshal.SetCount<string>(stringList2, capacity2);
        CollectionsMarshal.AsSpan<string>(stringList2)[0] = "blunt_attack.mp3";
        smallHitSfx = stringList2;
        break;
      case Necrobinder _:
        int capacity3 = 1;
        List<string> stringList3 = new List<string>(capacity3);
        CollectionsMarshal.SetCount<string>(stringList3, capacity3);
        CollectionsMarshal.AsSpan<string>(stringList3)[0] = "slash_attack.mp3";
        smallHitSfx = stringList3;
        break;
      case Regent _:
        int capacity4 = 1;
        List<string> stringList4 = new List<string>(capacity4);
        CollectionsMarshal.SetCount<string>(stringList4, capacity4);
        CollectionsMarshal.AsSpan<string>(stringList4)[0] = "slash_attack.mp3";
        smallHitSfx = stringList4;
        break;
      case Silent _:
        int capacity5 = 1;
        List<string> stringList5 = new List<string>(capacity5);
        CollectionsMarshal.SetCount<string>(stringList5, capacity5);
        CollectionsMarshal.AsSpan<string>(stringList5)[0] = "slash_attack.mp3";
        smallHitSfx = stringList5;
        break;
      default:
        smallHitSfx = new List<string>();
        break;
    }
    return smallHitSfx;
  }

  private List<string> GetBigHitSfx(CharacterModel character)
  {
    List<string> bigHitSfx;
    switch (character)
    {
      case Defect _:
        int capacity1 = 1;
        List<string> stringList1 = new List<string>(capacity1);
        CollectionsMarshal.SetCount<string>(stringList1, capacity1);
        CollectionsMarshal.AsSpan<string>(stringList1)[0] = "lightning_orb_evoke.mp3";
        bigHitSfx = stringList1;
        break;
      case Ironclad _:
        int capacity2 = 1;
        List<string> stringList2 = new List<string>(capacity2);
        CollectionsMarshal.SetCount<string>(stringList2, capacity2);
        CollectionsMarshal.AsSpan<string>(stringList2)[0] = "heavy_attack.mp3";
        bigHitSfx = stringList2;
        break;
      case Necrobinder _:
        int capacity3 = 1;
        List<string> stringList3 = new List<string>(capacity3);
        CollectionsMarshal.SetCount<string>(stringList3, capacity3);
        CollectionsMarshal.AsSpan<string>(stringList3)[0] = "heavy_attack.mp3";
        bigHitSfx = stringList3;
        break;
      case Regent _:
        int capacity4 = 1;
        List<string> stringList4 = new List<string>(capacity4);
        CollectionsMarshal.SetCount<string>(stringList4, capacity4);
        CollectionsMarshal.AsSpan<string>(stringList4)[0] = "heavy_attack.mp3";
        bigHitSfx = stringList4;
        break;
      case Silent _:
        int capacity5 = 2;
        List<string> stringList5 = new List<string>(capacity5);
        CollectionsMarshal.SetCount<string>(stringList5, capacity5);
        Span<string> span = CollectionsMarshal.AsSpan<string>(stringList5);
        int num1 = 0;
        span[num1] = "dagger_throw.mp3";
        int num2 = num1 + 1;
        span[num2] = "dagger_throw.mp3";
        bigHitSfx = stringList5;
        break;
      default:
        bigHitSfx = new List<string>();
        break;
    }
    return bigHitSfx;
  }

  private async Task PlaySfx(List<string> sfxPaths)
  {
    for (int i = 0; i < sfxPaths.Count; ++i)
    {
      string sfxPath = sfxPaths[i];
      if (sfxPath.StartsWith("event:"))
        SfxCmd.Play(sfxPath, this.GetSfxVolume());
      else
        NDebugAudioManager.Instance?.Play(sfxPath, this.GetSfxVolume(), PitchVariance.Medium);
      if (i < sfxPaths.Count - 1)
        await Cmd.Wait(0.1f);
    }
  }

  private float GetSfxVolume() => !this._hurryUp ? 1f : 0.5f;

  public void HurryUp() => this._hurryUp = true;

  public void SetupForAnimation()
  {
    ((CanvasItem) this).Visible = false;
    ((CanvasItem) this).Modulate = StsColors.transparentBlack;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(8)
    {
      new MethodInfo(NMapPointHistoryEntry.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMapPointHistoryEntry.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMapPointHistoryEntry.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMapPointHistoryEntry.MethodName.Highlight, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMapPointHistoryEntry.MethodName.Unhighlight, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMapPointHistoryEntry.MethodName.GetSfxVolume, new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMapPointHistoryEntry.MethodName.HurryUp, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMapPointHistoryEntry.MethodName.SetupForAnimation, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NMapPointHistoryEntry.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMapPointHistoryEntry.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMapPointHistoryEntry.MethodName.OnUnfocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnfocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMapPointHistoryEntry.MethodName.Highlight) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Highlight();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMapPointHistoryEntry.MethodName.Unhighlight) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Unhighlight();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMapPointHistoryEntry.MethodName.GetSfxVolume) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      float sfxVolume = this.GetSfxVolume();
      ret = VariantUtils.CreateFrom<float>(ref sfxVolume);
      return true;
    }
    if (StringName.op_Equality(ref method, NMapPointHistoryEntry.MethodName.HurryUp) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.HurryUp();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NMapPointHistoryEntry.MethodName.SetupForAnimation) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.SetupForAnimation();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NMapPointHistoryEntry.MethodName._Ready) || StringName.op_Equality(ref method, NMapPointHistoryEntry.MethodName.OnFocus) || StringName.op_Equality(ref method, NMapPointHistoryEntry.MethodName.OnUnfocus) || StringName.op_Equality(ref method, NMapPointHistoryEntry.MethodName.Highlight) || StringName.op_Equality(ref method, NMapPointHistoryEntry.MethodName.Unhighlight) || StringName.op_Equality(ref method, NMapPointHistoryEntry.MethodName.GetSfxVolume) || StringName.op_Equality(ref method, NMapPointHistoryEntry.MethodName.HurryUp) || StringName.op_Equality(ref method, NMapPointHistoryEntry.MethodName.SetupForAnimation) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMapPointHistoryEntry.PropertyName.FloorNum))
    {
      this.FloorNum = VariantUtils.ConvertTo<int>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapPointHistoryEntry.PropertyName._baseScale))
    {
      this._baseScale = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapPointHistoryEntry.PropertyName._texture))
    {
      this._texture = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapPointHistoryEntry.PropertyName._outline))
    {
      this._outline = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapPointHistoryEntry.PropertyName._questIcon))
    {
      this._questIcon = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapPointHistoryEntry.PropertyName._animateInTween))
    {
      this._animateInTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapPointHistoryEntry.PropertyName._hoverTween))
    {
      this._hoverTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapPointHistoryEntry.PropertyName._baseAngle))
    {
      this._baseAngle = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMapPointHistoryEntry.PropertyName._hurryUp))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._hurryUp = VariantUtils.ConvertTo<bool>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMapPointHistoryEntry.PropertyName.FloorNum))
    {
      ref godot_variant local = ref value;
      int floorNum = this.FloorNum;
      godot_variant from = VariantUtils.CreateFrom<int>(ref floorNum);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NMapPointHistoryEntry.PropertyName._baseScale))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._baseScale);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapPointHistoryEntry.PropertyName._texture))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._texture);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapPointHistoryEntry.PropertyName._outline))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._outline);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapPointHistoryEntry.PropertyName._questIcon))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._questIcon);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapPointHistoryEntry.PropertyName._animateInTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._animateInTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapPointHistoryEntry.PropertyName._hoverTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._hoverTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapPointHistoryEntry.PropertyName._baseAngle))
    {
      value = VariantUtils.CreateFrom<float>(ref this._baseAngle);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMapPointHistoryEntry.PropertyName._hurryUp))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<bool>(ref this._hurryUp);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 5L, NMapPointHistoryEntry.PropertyName._baseScale, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NMapPointHistoryEntry.PropertyName.FloorNum, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMapPointHistoryEntry.PropertyName._texture, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMapPointHistoryEntry.PropertyName._outline, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMapPointHistoryEntry.PropertyName._questIcon, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMapPointHistoryEntry.PropertyName._animateInTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMapPointHistoryEntry.PropertyName._hoverTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NMapPointHistoryEntry.PropertyName._baseAngle, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NMapPointHistoryEntry.PropertyName._hurryUp, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo = info;
    StringName floorNum1 = NMapPointHistoryEntry.PropertyName.FloorNum;
    int floorNum2 = this.FloorNum;
    Variant variant = Variant.From<int>(ref floorNum2);
    serializationInfo.AddProperty(floorNum1, variant);
    info.AddProperty(NMapPointHistoryEntry.PropertyName._baseScale, Variant.From<Vector2>(ref this._baseScale));
    info.AddProperty(NMapPointHistoryEntry.PropertyName._texture, Variant.From<TextureRect>(ref this._texture));
    info.AddProperty(NMapPointHistoryEntry.PropertyName._outline, Variant.From<TextureRect>(ref this._outline));
    info.AddProperty(NMapPointHistoryEntry.PropertyName._questIcon, Variant.From<TextureRect>(ref this._questIcon));
    info.AddProperty(NMapPointHistoryEntry.PropertyName._animateInTween, Variant.From<Tween>(ref this._animateInTween));
    info.AddProperty(NMapPointHistoryEntry.PropertyName._hoverTween, Variant.From<Tween>(ref this._hoverTween));
    info.AddProperty(NMapPointHistoryEntry.PropertyName._baseAngle, Variant.From<float>(ref this._baseAngle));
    info.AddProperty(NMapPointHistoryEntry.PropertyName._hurryUp, Variant.From<bool>(ref this._hurryUp));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NMapPointHistoryEntry.PropertyName.FloorNum, ref variant1))
      this.FloorNum = ((Variant) ref variant1).As<int>();
    Variant variant2;
    if (info.TryGetProperty(NMapPointHistoryEntry.PropertyName._baseScale, ref variant2))
      this._baseScale = ((Variant) ref variant2).As<Vector2>();
    Variant variant3;
    if (info.TryGetProperty(NMapPointHistoryEntry.PropertyName._texture, ref variant3))
      this._texture = ((Variant) ref variant3).As<TextureRect>();
    Variant variant4;
    if (info.TryGetProperty(NMapPointHistoryEntry.PropertyName._outline, ref variant4))
      this._outline = ((Variant) ref variant4).As<TextureRect>();
    Variant variant5;
    if (info.TryGetProperty(NMapPointHistoryEntry.PropertyName._questIcon, ref variant5))
      this._questIcon = ((Variant) ref variant5).As<TextureRect>();
    Variant variant6;
    if (info.TryGetProperty(NMapPointHistoryEntry.PropertyName._animateInTween, ref variant6))
      this._animateInTween = ((Variant) ref variant6).As<Tween>();
    Variant variant7;
    if (info.TryGetProperty(NMapPointHistoryEntry.PropertyName._hoverTween, ref variant7))
      this._hoverTween = ((Variant) ref variant7).As<Tween>();
    Variant variant8;
    if (info.TryGetProperty(NMapPointHistoryEntry.PropertyName._baseAngle, ref variant8))
      this._baseAngle = ((Variant) ref variant8).As<float>();
    Variant variant9;
    if (!info.TryGetProperty(NMapPointHistoryEntry.PropertyName._hurryUp, ref variant9))
      return;
    this._hurryUp = ((Variant) ref variant9).As<bool>();
  }

  public new class MethodName : NClickableControl.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
    public static readonly StringName Highlight = StringName.op_Implicit(nameof (Highlight));
    public static readonly StringName Unhighlight = StringName.op_Implicit(nameof (Unhighlight));
    public static readonly StringName GetSfxVolume = StringName.op_Implicit(nameof (GetSfxVolume));
    public static readonly StringName HurryUp = StringName.op_Implicit(nameof (HurryUp));
    public static readonly StringName SetupForAnimation = StringName.op_Implicit(nameof (SetupForAnimation));
  }

  public new class PropertyName : NClickableControl.PropertyName
  {
    public static readonly StringName FloorNum = StringName.op_Implicit(nameof (FloorNum));
    public static readonly StringName _baseScale = StringName.op_Implicit(nameof (_baseScale));
    public static readonly StringName _texture = StringName.op_Implicit(nameof (_texture));
    public static readonly StringName _outline = StringName.op_Implicit(nameof (_outline));
    public static readonly StringName _questIcon = StringName.op_Implicit(nameof (_questIcon));
    public static readonly StringName _animateInTween = StringName.op_Implicit(nameof (_animateInTween));
    public static readonly StringName _hoverTween = StringName.op_Implicit(nameof (_hoverTween));
    public static readonly StringName _baseAngle = StringName.op_Implicit(nameof (_baseAngle));
    public static readonly StringName _hurryUp = StringName.op_Implicit(nameof (_hurryUp));
  }

  public new class SignalName : NClickableControl.SignalName
  {
  }
}
