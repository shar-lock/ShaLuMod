// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Debug.Multiplayer.NMultiplayerTestCharacterPaginator
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Nodes.Screens.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Debug.Multiplayer;

[ScriptPath("res://src/Core/Nodes/Debug/Multiplayer/NMultiplayerTestCharacterPaginator.cs")]
public class NMultiplayerTestCharacterPaginator : NPaginator
{
  private readonly CharacterModel[] _characters = new CharacterModel[5]
  {
    (CharacterModel) ModelDb.Character<Ironclad>(),
    (CharacterModel) ModelDb.Character<Silent>(),
    (CharacterModel) ModelDb.Character<Regent>(),
    (CharacterModel) ModelDb.Character<Necrobinder>(),
    (CharacterModel) ModelDb.Character<Defect>()
  };

  public event Action<CharacterModel>? CharacterChanged;

  public CharacterModel Character => this._characters[this._currentIndex];

  public override void _Ready()
  {
    this.ConnectSignals();
    foreach (AbstractModel character in this._characters)
      this._options.Add(new LocString("characters", character.Id.Entry + ".title").GetFormattedText());
    this._label.Text = this._options[this._currentIndex];
  }

  protected override void OnIndexChanged(int index)
  {
    this._currentIndex = index;
    this._label.Text = this._characters[index].Title.GetFormattedText();
    Action<CharacterModel> characterChanged = this.CharacterChanged;
    if (characterChanged == null)
      return;
    characterChanged(this._characters[index]);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NMultiplayerTestCharacterPaginator.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMultiplayerTestCharacterPaginator.MethodName.OnIndexChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("index"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NMultiplayerTestCharacterPaginator.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NMultiplayerTestCharacterPaginator.MethodName.OnIndexChanged) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.OnIndexChanged(VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NMultiplayerTestCharacterPaginator.MethodName._Ready) || StringName.op_Equality(ref method, NMultiplayerTestCharacterPaginator.MethodName.OnIndexChanged) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
  }

  public new class MethodName : NPaginator.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName OnIndexChanged = StringName.op_Implicit(nameof (OnIndexChanged));
  }

  public new class PropertyName : NPaginator.PropertyName
  {
  }

  public new class SignalName : NPaginator.SignalName
  {
  }
}
