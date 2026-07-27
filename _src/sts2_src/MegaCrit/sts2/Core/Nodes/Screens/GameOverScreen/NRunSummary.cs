// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.GameOverScreen.NRunSummary
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.GameOverScreen;

[ScriptPath("res://src/Core/Nodes/Screens/GameOverScreen/NRunSummary.cs")]
public class NRunSummary : Control
{
  private Control _discoveryContainer;
  private Control _discoveryHeader;
  private Control _discoveredContents;
  private NDiscoveredItem _discoveredCards;
  private NDiscoveredItem _discoveredRelics;
  private NDiscoveredItem _discoveredPotions;
  private NDiscoveredItem _discoveredEnemies;
  private NDiscoveredItem _discoveredEpochs;
  private Tween? _tween;
  private Tween? _waitTween;
  private const int _maxItemsToList = 10;

  public override void _Ready()
  {
    this._discoveryContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%DiscoveryContainer"));
    this._discoveryHeader = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%DiscoveryHeader"));
    this._discoveredContents = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%DiscoveredContents"));
    this._discoveredCards = ((Node) this).GetNode<NDiscoveredItem>(NodePath.op_Implicit("%DiscoveredCards"));
    this._discoveredRelics = ((Node) this).GetNode<NDiscoveredItem>(NodePath.op_Implicit("%DiscoveredRelics"));
    this._discoveredPotions = ((Node) this).GetNode<NDiscoveredItem>(NodePath.op_Implicit("%DiscoveredPotions"));
    this._discoveredEnemies = ((Node) this).GetNode<NDiscoveredItem>(NodePath.op_Implicit("%DiscoveredEnemies"));
    this._discoveredEpochs = ((Node) this).GetNode<NDiscoveredItem>(NodePath.op_Implicit("%DiscoveredEpochs"));
    ((CanvasItem) this._discoveredCards).Visible = false;
    ((CanvasItem) this._discoveredRelics).Visible = false;
    ((CanvasItem) this._discoveredPotions).Visible = false;
    ((CanvasItem) this._discoveredEnemies).Visible = false;
    ((CanvasItem) this._discoveredEpochs).Visible = false;
  }

  public async Task AnimateInDiscoveries(RunState runState, CancellationToken ct)
  {
    Player player = LocalContext.GetMe((IPlayerCollection) runState);
    if (player.DiscoveredCards.Count + player.DiscoveredRelics.Count + player.DiscoveredPotions.Count + player.DiscoveredEnemies.Count + player.DiscoveredEpochs.Count == 0)
    {
      Log.Info("No discoveries this time. Very sad");
      player = (Player) null;
    }
    else
    {
      ((Node) this).CreateTween().TweenProperty((GodotObject) this._discoveryHeader, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.25);
      await Task.Delay(100, ct);
      if (!((Node) this).IsValid())
      {
        player = (Player) null;
      }
      else
      {
        if (player.DiscoveredCards.Count > 0)
        {
          this._discoveredCards.SetHoverTip(new HoverTip(new LocString("game_over_screen", "DISCOVERY_HEADER_CARD"), NRunSummary.GetDiscoveryBodyText<ModelId>(player.DiscoveredCards, (Func<ModelId, string>) (id => SaveUtil.CardOrDeprecated(id).Title), "game_over_screen", "DISCOVERY_BODY_CARD", "CardCount")));
          ((CanvasItem) this._discoveredCards).Visible = true;
          ((CanvasItem) this._discoveredCards).Modulate = StsColors.transparentBlack;
        }
        if (player.DiscoveredRelics.Count > 0)
        {
          this._discoveredRelics.SetHoverTip(new HoverTip(new LocString("game_over_screen", "DISCOVERY_HEADER_RELIC"), NRunSummary.GetDiscoveryBodyText<ModelId>(player.DiscoveredRelics, (Func<ModelId, string>) (id => SaveUtil.RelicOrDeprecated(id).Title.GetFormattedText()), "game_over_screen", "DISCOVERY_BODY_RELIC", "RelicCount")));
          ((CanvasItem) this._discoveredRelics).Visible = true;
          ((CanvasItem) this._discoveredRelics).Modulate = StsColors.transparentBlack;
        }
        if (player.DiscoveredPotions.Count > 0)
        {
          this._discoveredPotions.SetHoverTip(new HoverTip(new LocString("game_over_screen", "DISCOVERY_HEADER_POTION"), NRunSummary.GetDiscoveryBodyText<ModelId>(player.DiscoveredPotions, (Func<ModelId, string>) (id => SaveUtil.PotionOrDeprecated(id).Title.GetFormattedText()), "game_over_screen", "DISCOVERY_BODY_POTION", "PotionCount")));
          ((CanvasItem) this._discoveredPotions).Visible = true;
          ((CanvasItem) this._discoveredPotions).Modulate = StsColors.transparentBlack;
        }
        if (player.DiscoveredEnemies.Count > 0)
        {
          this._discoveredEnemies.SetHoverTip(new HoverTip(new LocString("game_over_screen", "DISCOVERY_HEADER_ENEMY"), NRunSummary.GetDiscoveryBodyText<ModelId>(player.DiscoveredEnemies, (Func<ModelId, string>) (id => SaveUtil.MonsterOrDeprecated(id).Title.GetFormattedText()), "game_over_screen", "DISCOVERY_BODY_ENEMY", "EnemyCount")));
          ((CanvasItem) this._discoveredEnemies).Visible = true;
          ((CanvasItem) this._discoveredEnemies).Modulate = StsColors.transparentBlack;
        }
        if (player.DiscoveredEpochs.Count > 0)
        {
          LocString title = new LocString("game_over_screen", "DISCOVERY_HEADER_EPOCH");
          LocString description = new LocString("game_over_screen", "DISCOVERY_BODY_EPOCH");
          description.Add("EpochCount", (Decimal) player.DiscoveredEpochs.Count);
          this._discoveredEpochs.SetHoverTip(new HoverTip(title, description));
          ((CanvasItem) this._discoveredEpochs).Visible = true;
          ((CanvasItem) this._discoveredEpochs).Modulate = StsColors.transparentBlack;
        }
        if (((CanvasItem) this._discoveredCards).Visible)
        {
          this._discoveredCards.SetText($"{player.DiscoveredCards.Count}");
          await TaskHelper.RunSafely(this.DiscoveryAnimHelper((Control) this._discoveredCards));
        }
        if (((CanvasItem) this._discoveredRelics).Visible)
        {
          this._discoveredRelics.SetText($"{player.DiscoveredRelics.Count}");
          await TaskHelper.RunSafely(this.DiscoveryAnimHelper((Control) this._discoveredRelics));
        }
        if (((CanvasItem) this._discoveredPotions).Visible)
        {
          this._discoveredPotions.SetText($"{player.DiscoveredPotions.Count}");
          await TaskHelper.RunSafely(this.DiscoveryAnimHelper((Control) this._discoveredPotions));
        }
        if (((CanvasItem) this._discoveredEnemies).Visible)
        {
          this._discoveredEnemies.SetText($"{player.DiscoveredEnemies.Count}");
          await TaskHelper.RunSafely(this.DiscoveryAnimHelper((Control) this._discoveredEnemies));
        }
        if (!((CanvasItem) this._discoveredEpochs).Visible)
        {
          player = (Player) null;
        }
        else
        {
          this._discoveredEpochs.SetText($"{player.DiscoveredEpochs.Count}");
          await TaskHelper.RunSafely(this.DiscoveryAnimHelper((Control) this._discoveredEpochs));
          player = (Player) null;
        }
      }
    }
  }

  private async Task DiscoveryAnimHelper(Control node)
  {
    ((CanvasItem) node).Modulate = StsColors.transparentBlack;
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) node, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.White), 0.3);
    this._tween.TweenProperty((GodotObject) node, NodePath.op_Implicit("position:y"), Variant.op_Implicit(0.0f), 0.3).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 10L).From(Variant.op_Implicit(100f));
    bool flag = await this._tween.AwaitFinished((Node) this);
  }

  private static string GetDiscoveryBodyText<T>(
    List<T> discoveredIds,
    Func<T, string> getTitle,
    string locTable,
    string locKey,
    string countParam)
  {
    LocString locString = new LocString(locTable, locKey);
    locString.Add(countParam, (Decimal) discoveredIds.Count);
    string str = string.Join("\n", discoveredIds.Take<T>(10).Select<T, string>(getTitle));
    if (discoveredIds.Count > 10)
      str += "\n....";
    return $"{locString.GetFormattedText()}\n\n{str}";
  }

  public Control? DefaultFocusedControl
  {
    get
    {
      return ((IEnumerable) ((Node) this._discoveredContents).GetChildren(false)).OfType<Control>().FirstOrDefault<Control>((Func<Control, bool>) (c => ((CanvasItem) c).IsVisible()));
    }
  }

  public void SetControllerNav(Control? focusNeighborTop)
  {
    Control[] array = ((IEnumerable) ((Node) this._discoveredContents).GetChildren(false)).OfType<Control>().Where<Control>((Func<Control, bool>) (c => ((CanvasItem) c).IsVisible())).ToArray<Control>();
    for (int index = 0; index < array.Length; ++index)
    {
      Control control1 = array[index];
      control1.FocusNeighborTop = focusNeighborTop != null ? ((Node) focusNeighborTop).GetPath() : ((Node) control1).GetPath();
      control1.FocusNeighborBottom = ((Node) control1).GetPath();
      Control control2 = control1;
      NodePath path;
      if (index <= 0)
      {
        Control[] controlArray = array;
        path = ((Node) controlArray[controlArray.Length - 1]).GetPath();
      }
      else
        path = ((Node) array[index - 1]).GetPath();
      control2.FocusNeighborLeft = path;
      control1.FocusNeighborRight = index < array.Length - 1 ? ((Node) array[index + 1]).GetPath() : ((Node) array[0]).GetPath();
    }
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NRunSummary.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRunSummary.MethodName.SetControllerNav, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("focusNeighborTop"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NRunSummary.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NRunSummary.MethodName.SetControllerNav) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.SetControllerNav(VariantUtils.ConvertTo<Control>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NRunSummary.MethodName._Ready) || StringName.op_Equality(ref method, NRunSummary.MethodName.SetControllerNav) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NRunSummary.PropertyName._discoveryContainer))
    {
      this._discoveryContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunSummary.PropertyName._discoveryHeader))
    {
      this._discoveryHeader = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunSummary.PropertyName._discoveredContents))
    {
      this._discoveredContents = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunSummary.PropertyName._discoveredCards))
    {
      this._discoveredCards = VariantUtils.ConvertTo<NDiscoveredItem>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunSummary.PropertyName._discoveredRelics))
    {
      this._discoveredRelics = VariantUtils.ConvertTo<NDiscoveredItem>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunSummary.PropertyName._discoveredPotions))
    {
      this._discoveredPotions = VariantUtils.ConvertTo<NDiscoveredItem>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunSummary.PropertyName._discoveredEnemies))
    {
      this._discoveredEnemies = VariantUtils.ConvertTo<NDiscoveredItem>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunSummary.PropertyName._discoveredEpochs))
    {
      this._discoveredEpochs = VariantUtils.ConvertTo<NDiscoveredItem>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunSummary.PropertyName._tween))
    {
      this._tween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NRunSummary.PropertyName._waitTween))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._waitTween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NRunSummary.PropertyName.DefaultFocusedControl))
    {
      ref godot_variant local = ref value;
      Control defaultFocusedControl = this.DefaultFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NRunSummary.PropertyName._discoveryContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._discoveryContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunSummary.PropertyName._discoveryHeader))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._discoveryHeader);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunSummary.PropertyName._discoveredContents))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._discoveredContents);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunSummary.PropertyName._discoveredCards))
    {
      value = VariantUtils.CreateFrom<NDiscoveredItem>(ref this._discoveredCards);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunSummary.PropertyName._discoveredRelics))
    {
      value = VariantUtils.CreateFrom<NDiscoveredItem>(ref this._discoveredRelics);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunSummary.PropertyName._discoveredPotions))
    {
      value = VariantUtils.CreateFrom<NDiscoveredItem>(ref this._discoveredPotions);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunSummary.PropertyName._discoveredEnemies))
    {
      value = VariantUtils.CreateFrom<NDiscoveredItem>(ref this._discoveredEnemies);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunSummary.PropertyName._discoveredEpochs))
    {
      value = VariantUtils.CreateFrom<NDiscoveredItem>(ref this._discoveredEpochs);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunSummary.PropertyName._tween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._tween);
      return true;
    }
    if (!StringName.op_Equality(ref name, NRunSummary.PropertyName._waitTween))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Tween>(ref this._waitTween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NRunSummary.PropertyName._discoveryContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRunSummary.PropertyName._discoveryHeader, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRunSummary.PropertyName._discoveredContents, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRunSummary.PropertyName._discoveredCards, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRunSummary.PropertyName._discoveredRelics, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRunSummary.PropertyName._discoveredPotions, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRunSummary.PropertyName._discoveredEnemies, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRunSummary.PropertyName._discoveredEpochs, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRunSummary.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRunSummary.PropertyName._waitTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRunSummary.PropertyName.DefaultFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NRunSummary.PropertyName._discoveryContainer, Variant.From<Control>(ref this._discoveryContainer));
    info.AddProperty(NRunSummary.PropertyName._discoveryHeader, Variant.From<Control>(ref this._discoveryHeader));
    info.AddProperty(NRunSummary.PropertyName._discoveredContents, Variant.From<Control>(ref this._discoveredContents));
    info.AddProperty(NRunSummary.PropertyName._discoveredCards, Variant.From<NDiscoveredItem>(ref this._discoveredCards));
    info.AddProperty(NRunSummary.PropertyName._discoveredRelics, Variant.From<NDiscoveredItem>(ref this._discoveredRelics));
    info.AddProperty(NRunSummary.PropertyName._discoveredPotions, Variant.From<NDiscoveredItem>(ref this._discoveredPotions));
    info.AddProperty(NRunSummary.PropertyName._discoveredEnemies, Variant.From<NDiscoveredItem>(ref this._discoveredEnemies));
    info.AddProperty(NRunSummary.PropertyName._discoveredEpochs, Variant.From<NDiscoveredItem>(ref this._discoveredEpochs));
    info.AddProperty(NRunSummary.PropertyName._tween, Variant.From<Tween>(ref this._tween));
    info.AddProperty(NRunSummary.PropertyName._waitTween, Variant.From<Tween>(ref this._waitTween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NRunSummary.PropertyName._discoveryContainer, ref variant1))
      this._discoveryContainer = ((Variant) ref variant1).As<Control>();
    Variant variant2;
    if (info.TryGetProperty(NRunSummary.PropertyName._discoveryHeader, ref variant2))
      this._discoveryHeader = ((Variant) ref variant2).As<Control>();
    Variant variant3;
    if (info.TryGetProperty(NRunSummary.PropertyName._discoveredContents, ref variant3))
      this._discoveredContents = ((Variant) ref variant3).As<Control>();
    Variant variant4;
    if (info.TryGetProperty(NRunSummary.PropertyName._discoveredCards, ref variant4))
      this._discoveredCards = ((Variant) ref variant4).As<NDiscoveredItem>();
    Variant variant5;
    if (info.TryGetProperty(NRunSummary.PropertyName._discoveredRelics, ref variant5))
      this._discoveredRelics = ((Variant) ref variant5).As<NDiscoveredItem>();
    Variant variant6;
    if (info.TryGetProperty(NRunSummary.PropertyName._discoveredPotions, ref variant6))
      this._discoveredPotions = ((Variant) ref variant6).As<NDiscoveredItem>();
    Variant variant7;
    if (info.TryGetProperty(NRunSummary.PropertyName._discoveredEnemies, ref variant7))
      this._discoveredEnemies = ((Variant) ref variant7).As<NDiscoveredItem>();
    Variant variant8;
    if (info.TryGetProperty(NRunSummary.PropertyName._discoveredEpochs, ref variant8))
      this._discoveredEpochs = ((Variant) ref variant8).As<NDiscoveredItem>();
    Variant variant9;
    if (info.TryGetProperty(NRunSummary.PropertyName._tween, ref variant9))
      this._tween = ((Variant) ref variant9).As<Tween>();
    Variant variant10;
    if (!info.TryGetProperty(NRunSummary.PropertyName._waitTween, ref variant10))
      return;
    this._waitTween = ((Variant) ref variant10).As<Tween>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName SetControllerNav = StringName.op_Implicit(nameof (SetControllerNav));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName DefaultFocusedControl = StringName.op_Implicit(nameof (DefaultFocusedControl));
    public static readonly StringName _discoveryContainer = StringName.op_Implicit(nameof (_discoveryContainer));
    public static readonly StringName _discoveryHeader = StringName.op_Implicit(nameof (_discoveryHeader));
    public static readonly StringName _discoveredContents = StringName.op_Implicit(nameof (_discoveredContents));
    public static readonly StringName _discoveredCards = StringName.op_Implicit(nameof (_discoveredCards));
    public static readonly StringName _discoveredRelics = StringName.op_Implicit(nameof (_discoveredRelics));
    public static readonly StringName _discoveredPotions = StringName.op_Implicit(nameof (_discoveredPotions));
    public static readonly StringName _discoveredEnemies = StringName.op_Implicit(nameof (_discoveredEnemies));
    public static readonly StringName _discoveredEpochs = StringName.op_Implicit(nameof (_discoveredEpochs));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
    public static readonly StringName _waitTween = StringName.op_Implicit(nameof (_waitTween));
  }

  public class SignalName : Control.SignalName
  {
  }
}
