// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Ftue.NCombatRewardFtue
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Ftue;

[ScriptPath("res://src/Core/Nodes/Ftue/NCombatRewardFtue.cs")]
public class NCombatRewardFtue : NFtue
{
  public const string id = "combat_reward_ftue";
  private static readonly string _scenePath = SceneHelper.GetScenePath("ftue/combat_reward_ftue");
  private NButton _confirmButton;
  private MegaLabel _header;
  private MegaRichTextLabel _description;
  private Control _rewardsContainer;
  private int _defaultZIndex;

  public override void _Ready()
  {
    this._header = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("FtuePopup/Header"));
    this._header.SetTextAutoSize(new LocString("ftues", "REWARDS_FTUE_TITLE").GetFormattedText());
    this._description = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("FtuePopup/DescriptionContainer/Description"));
    this._description.Text = new LocString("ftues", "REWARDS_FTUE_DESCRIPTION").GetFormattedText();
    this._confirmButton = ((Node) this).GetNode<NButton>(NodePath.op_Implicit("FtuePopup/FtueConfirmButton"));
    ((GodotObject) this._confirmButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.CloseFtue)), 0U);
    this._defaultZIndex = ((CanvasItem) this._rewardsContainer).ZIndex;
    Control rewardsContainer = this._rewardsContainer;
    ((CanvasItem) rewardsContainer).ZIndex = ((CanvasItem) rewardsContainer).ZIndex + 1;
  }

  public static NCombatRewardFtue? Create(Control rewardsContainer)
  {
    if (TestMode.IsOn)
      return (NCombatRewardFtue) null;
    NCombatRewardFtue ncombatRewardFtue = PreloadManager.Cache.GetScene(NCombatRewardFtue._scenePath).Instantiate<NCombatRewardFtue>((PackedScene.GenEditState) 0L);
    ncombatRewardFtue._rewardsContainer = rewardsContainer;
    return ncombatRewardFtue;
  }

  private void CloseFtue(NButton _)
  {
    ((CanvasItem) this._rewardsContainer).ZIndex = this._defaultZIndex;
    this.CloseFtue();
  }

  public async Task WaitForPlayerToConfirm()
  {
    NClickableControl nclickableControl = await ((GodotObject) this._confirmButton).AwaitSignal<NClickableControl>(NClickableControl.SignalName.Released, (Node) this);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(3)
    {
      new MethodInfo(NCombatRewardFtue.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCombatRewardFtue.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("rewardsContainer"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCombatRewardFtue.MethodName.CloseFtue, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NCombatRewardFtue.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCombatRewardFtue.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NCombatRewardFtue ncombatRewardFtue = NCombatRewardFtue.Create(VariantUtils.ConvertTo<Control>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NCombatRewardFtue>(ref ncombatRewardFtue);
      return true;
    }
    if (!StringName.op_Equality(ref method, NCombatRewardFtue.MethodName.CloseFtue) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.CloseFtue(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NCombatRewardFtue.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NCombatRewardFtue ncombatRewardFtue = NCombatRewardFtue.Create(VariantUtils.ConvertTo<Control>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NCombatRewardFtue>(ref ncombatRewardFtue);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NCombatRewardFtue.MethodName._Ready) || StringName.op_Equality(ref method, NCombatRewardFtue.MethodName.Create) || StringName.op_Equality(ref method, NCombatRewardFtue.MethodName.CloseFtue) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCombatRewardFtue.PropertyName._confirmButton))
    {
      this._confirmButton = VariantUtils.ConvertTo<NButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatRewardFtue.PropertyName._header))
    {
      this._header = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatRewardFtue.PropertyName._description))
    {
      this._description = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatRewardFtue.PropertyName._rewardsContainer))
    {
      this._rewardsContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCombatRewardFtue.PropertyName._defaultZIndex))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._defaultZIndex = VariantUtils.ConvertTo<int>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCombatRewardFtue.PropertyName._confirmButton))
    {
      value = VariantUtils.CreateFrom<NButton>(ref this._confirmButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatRewardFtue.PropertyName._header))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._header);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatRewardFtue.PropertyName._description))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._description);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatRewardFtue.PropertyName._rewardsContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._rewardsContainer);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCombatRewardFtue.PropertyName._defaultZIndex))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<int>(ref this._defaultZIndex);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NCombatRewardFtue.PropertyName._confirmButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCombatRewardFtue.PropertyName._header, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCombatRewardFtue.PropertyName._description, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCombatRewardFtue.PropertyName._rewardsContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NCombatRewardFtue.PropertyName._defaultZIndex, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NCombatRewardFtue.PropertyName._confirmButton, Variant.From<NButton>(ref this._confirmButton));
    info.AddProperty(NCombatRewardFtue.PropertyName._header, Variant.From<MegaLabel>(ref this._header));
    info.AddProperty(NCombatRewardFtue.PropertyName._description, Variant.From<MegaRichTextLabel>(ref this._description));
    info.AddProperty(NCombatRewardFtue.PropertyName._rewardsContainer, Variant.From<Control>(ref this._rewardsContainer));
    info.AddProperty(NCombatRewardFtue.PropertyName._defaultZIndex, Variant.From<int>(ref this._defaultZIndex));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NCombatRewardFtue.PropertyName._confirmButton, ref variant1))
      this._confirmButton = ((Variant) ref variant1).As<NButton>();
    Variant variant2;
    if (info.TryGetProperty(NCombatRewardFtue.PropertyName._header, ref variant2))
      this._header = ((Variant) ref variant2).As<MegaLabel>();
    Variant variant3;
    if (info.TryGetProperty(NCombatRewardFtue.PropertyName._description, ref variant3))
      this._description = ((Variant) ref variant3).As<MegaRichTextLabel>();
    Variant variant4;
    if (info.TryGetProperty(NCombatRewardFtue.PropertyName._rewardsContainer, ref variant4))
      this._rewardsContainer = ((Variant) ref variant4).As<Control>();
    Variant variant5;
    if (!info.TryGetProperty(NCombatRewardFtue.PropertyName._defaultZIndex, ref variant5))
      return;
    this._defaultZIndex = ((Variant) ref variant5).As<int>();
  }

  public new class MethodName : NFtue.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public new static readonly StringName CloseFtue = StringName.op_Implicit(nameof (CloseFtue));
  }

  public new class PropertyName : NFtue.PropertyName
  {
    public static readonly StringName _confirmButton = StringName.op_Implicit(nameof (_confirmButton));
    public static readonly StringName _header = StringName.op_Implicit(nameof (_header));
    public static readonly StringName _description = StringName.op_Implicit(nameof (_description));
    public static readonly StringName _rewardsContainer = StringName.op_Implicit(nameof (_rewardsContainer));
    public static readonly StringName _defaultZIndex = StringName.op_Implicit(nameof (_defaultZIndex));
  }

  public new class SignalName : NFtue.SignalName
  {
  }
}
