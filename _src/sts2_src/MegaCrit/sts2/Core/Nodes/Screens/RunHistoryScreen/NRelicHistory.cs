// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.RunHistoryScreen.NRelicHistory
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Exceptions;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Relics;
using MegaCrit.Sts2.Core.Saves.Runs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.RunHistoryScreen;

[ScriptPath("res://src/Core/Nodes/Screens/RunHistoryScreen/NRelicHistory.cs")]
public class NRelicHistory : VBoxContainer
{
  private readonly LocString _relicHeader = new LocString("run_history", "RELIC_HISTORY.header");
  private readonly LocString _relicCategories = new LocString("run_history", "RELIC_HISTORY.categories");
  private MegaRichTextLabel _headerLabel;
  private Control _relicsContainer;
  private 
  #nullable disable
  NRelicHistory.HoveredEventHandler backing_Hovered;
  private NRelicHistory.UnhoveredEventHandler backing_Unhovered;

  public override void _Ready()
  {
    this._headerLabel = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("Header"));
    this._relicsContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%RelicsContainer"));
  }

  public void LoadRelics(
  #nullable enable
  Player player, IEnumerable<SerializableRelic> relics)
  {
    StringBuilder stringBuilder1 = new StringBuilder();
    foreach (Node child in ((Node) this._relicsContainer).GetChildren(false))
      child.QueueFreeSafely();
    Dictionary<RelicRarity, int> dictionary = new Dictionary<RelicRarity, int>();
    foreach (RelicRarity key in Enum.GetValues<RelicRarity>())
      dictionary.Add(key, 0);
    List<SerializableRelic> list = relics.ToList<SerializableRelic>();
    foreach (SerializableRelic save in list)
    {
      RelicModel relic;
      try
      {
        relic = RelicModel.FromSerializable(save);
      }
      catch (ModelNotFoundException ex)
      {
        relic = ModelDb.Relic<DeprecatedRelic>().ToMutable();
      }
      relic.Owner = player;
      NRelicBasicHolder holder = NRelicBasicHolder.Create(relic);
      holder.MouseDefaultCursorShape = (Control.CursorShape) 16L /*0x10*/;
      ((Node) this._relicsContainer).AddChildSafely((Node) holder);
      ((GodotObject) holder).Connect(Control.SignalName.FocusEntered, Callable.From((Action) (() => ((GodotObject) this).EmitSignal(NRelicHistory.SignalName.Hovered, new Variant[1]
      {
        Variant.op_Implicit((GodotObject) holder)
      }))), 0U);
      ((GodotObject) holder).Connect(Control.SignalName.FocusExited, Callable.From((Action) (() => ((GodotObject) this).EmitSignal(NRelicHistory.SignalName.Unhovered, new Variant[1]
      {
        Variant.op_Implicit((GodotObject) holder)
      }))), 0U);
      ((GodotObject) holder).Connect(Control.SignalName.MouseEntered, Callable.From((Action) (() => ((GodotObject) this).EmitSignal(NRelicHistory.SignalName.Hovered, new Variant[1]
      {
        Variant.op_Implicit((GodotObject) holder)
      }))), 0U);
      ((GodotObject) holder).Connect(Control.SignalName.MouseExited, Callable.From((Action) (() => ((GodotObject) this).EmitSignal(NRelicHistory.SignalName.Unhovered, new Variant[1]
      {
        Variant.op_Implicit((GodotObject) holder)
      }))), 0U);
      ((GodotObject) holder).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>((Action<NButton>) (_ => this.OnRelicClicked(holder.Relic))), 0U);
      dictionary[relic.Rarity]++;
    }
    this._relicHeader.Add("totalRelics", (Decimal) list.Count);
    foreach (KeyValuePair<RelicRarity, int> keyValuePair in dictionary)
      this._relicCategories.Add(keyValuePair.Key.ToString() + "Relics", (Decimal) keyValuePair.Value);
    StringBuilder stringBuilder2 = stringBuilder1;
    StringBuilder stringBuilder3 = stringBuilder2;
    StringBuilder.AppendInterpolatedStringHandler interpolatedStringHandler;
    // ISSUE: explicit constructor call
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(20, 1, stringBuilder2);
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("[gold][b]");
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(this._relicHeader.GetFormattedText());
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("[/b][/gold]");
    ref StringBuilder.AppendInterpolatedStringHandler local = ref interpolatedStringHandler;
    stringBuilder3.Append(ref local);
    stringBuilder1.Append(this._relicCategories.GetFormattedText().Trim(','));
    this._headerLabel.Text = stringBuilder1.ToString();
  }

  private void OnRelicClicked(NRelic node)
  {
    List<RelicModel> relics = new List<RelicModel>();
    foreach (NRelicBasicHolder nrelicBasicHolder in ((IEnumerable) ((Node) this._relicsContainer).GetChildren(false)).OfType<NRelicBasicHolder>())
      relics.Add(nrelicBasicHolder.Relic.Model);
    NGame.Instance.GetInspectRelicScreen().Open((IReadOnlyList<RelicModel>) relics, node.Model);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NRelicHistory.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRelicHistory.MethodName.OnRelicClicked, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("node"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NRelicHistory.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NRelicHistory.MethodName.OnRelicClicked) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.OnRelicClicked(VariantUtils.ConvertTo<NRelic>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NRelicHistory.MethodName._Ready) || StringName.op_Equality(ref method, NRelicHistory.MethodName.OnRelicClicked) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NRelicHistory.PropertyName._headerLabel))
    {
      this._headerLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NRelicHistory.PropertyName._relicsContainer))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._relicsContainer = VariantUtils.ConvertTo<Control>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NRelicHistory.PropertyName._headerLabel))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._headerLabel);
      return true;
    }
    if (!StringName.op_Equality(ref name, NRelicHistory.PropertyName._relicsContainer))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Control>(ref this._relicsContainer);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NRelicHistory.PropertyName._headerLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRelicHistory.PropertyName._relicsContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NRelicHistory.PropertyName._headerLabel, Variant.From<MegaRichTextLabel>(ref this._headerLabel));
    info.AddProperty(NRelicHistory.PropertyName._relicsContainer, Variant.From<Control>(ref this._relicsContainer));
    info.AddSignalEventDelegate(NRelicHistory.SignalName.Hovered, (Delegate) this.backing_Hovered);
    info.AddSignalEventDelegate(NRelicHistory.SignalName.Unhovered, (Delegate) this.backing_Unhovered);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NRelicHistory.PropertyName._headerLabel, ref variant1))
      this._headerLabel = ((Variant) ref variant1).As<MegaRichTextLabel>();
    Variant variant2;
    if (info.TryGetProperty(NRelicHistory.PropertyName._relicsContainer, ref variant2))
      this._relicsContainer = ((Variant) ref variant2).As<Control>();
    NRelicHistory.HoveredEventHandler hoveredEventHandler;
    if (info.TryGetSignalEventDelegate<NRelicHistory.HoveredEventHandler>(NRelicHistory.SignalName.Hovered, ref hoveredEventHandler))
      this.backing_Hovered = hoveredEventHandler;
    NRelicHistory.UnhoveredEventHandler unhoveredEventHandler;
    if (!info.TryGetSignalEventDelegate<NRelicHistory.UnhoveredEventHandler>(NRelicHistory.SignalName.Unhovered, ref unhoveredEventHandler))
      return;
    this.backing_Unhovered = unhoveredEventHandler;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<MethodInfo> GetGodotSignalList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NRelicHistory.SignalName.Hovered, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("relic"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NRelicHistory.SignalName.Unhovered, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("relic"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null)
    };
  }

  public event NRelicHistory.HoveredEventHandler Hovered
  {
    add => this.backing_Hovered += value;
    remove => this.backing_Hovered -= value;
  }

  protected void EmitSignalHovered(NRelicBasicHolder relic)
  {
    ((GodotObject) this).EmitSignal(NRelicHistory.SignalName.Hovered, new Variant[1]
    {
      Variant.op_Implicit((GodotObject) relic)
    });
  }

  public event NRelicHistory.UnhoveredEventHandler Unhovered
  {
    add => this.backing_Unhovered += value;
    remove => this.backing_Unhovered -= value;
  }

  protected void EmitSignalUnhovered(NRelicBasicHolder relic)
  {
    ((GodotObject) this).EmitSignal(NRelicHistory.SignalName.Unhovered, new Variant[1]
    {
      Variant.op_Implicit((GodotObject) relic)
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RaiseGodotClassSignalCallbacks(
    in godot_string_name signal,
    NativeVariantPtrArgs args)
  {
    if (StringName.op_Equality(ref signal, NRelicHistory.SignalName.Hovered) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NRelicHistory.HoveredEventHandler backingHovered = this.backing_Hovered;
      if (backingHovered == null)
        return;
      backingHovered(VariantUtils.ConvertTo<NRelicBasicHolder>(ref ((NativeVariantPtrArgs) ref args)[0]));
    }
    else if (StringName.op_Equality(ref signal, NRelicHistory.SignalName.Unhovered) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NRelicHistory.UnhoveredEventHandler backingUnhovered = this.backing_Unhovered;
      if (backingUnhovered == null)
        return;
      backingUnhovered(VariantUtils.ConvertTo<NRelicBasicHolder>(ref ((NativeVariantPtrArgs) ref args)[0]));
    }
    else
      ((GodotObject) this).RaiseGodotClassSignalCallbacks(ref signal, args);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassSignal(in godot_string_name signal)
  {
    return StringName.op_Equality(ref signal, NRelicHistory.SignalName.Hovered) || StringName.op_Equality(ref signal, NRelicHistory.SignalName.Unhovered) || base.HasGodotClassSignal(ref signal);
  }

  [Signal]
  public delegate void HoveredEventHandler(
  #nullable enable
  NRelicBasicHolder relic);

  [Signal]
  public delegate void UnhoveredEventHandler(NRelicBasicHolder relic);

  public class MethodName : VBoxContainer.MethodName
  {
    public static readonly 
    #nullable disable
    StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName OnRelicClicked = StringName.op_Implicit(nameof (OnRelicClicked));
  }

  public class PropertyName : VBoxContainer.PropertyName
  {
    public static readonly StringName _headerLabel = StringName.op_Implicit(nameof (_headerLabel));
    public static readonly StringName _relicsContainer = StringName.op_Implicit(nameof (_relicsContainer));
  }

  public class SignalName : VBoxContainer.SignalName
  {
    public static readonly StringName Hovered = StringName.op_Implicit(nameof (Hovered));
    public static readonly StringName Unhovered = StringName.op_Implicit(nameof (Unhovered));
  }
}
