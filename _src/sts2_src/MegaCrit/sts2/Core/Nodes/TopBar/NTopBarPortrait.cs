// Decompiled with JetBrains decompiler
// Type: MegaCrit.sts2.Core.Nodes.TopBar.NTopBarPortrait
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.sts2.Core.Nodes.TopBar;

[ScriptPath("res://src/Core/Nodes/TopBar/NTopBarPortrait.cs")]
public class NTopBarPortrait : Control
{
  public void Initialize(Player player)
  {
    ((Node) this).AddChildSafely((Node) player.Character.Icon);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(
  #nullable disable
  GodotSerializationInfo info) => ((GodotObject) this).SaveGodotObjectData(info);

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
  }

  public class MethodName : Control.MethodName
  {
  }

  public class PropertyName : Control.PropertyName
  {
  }

  public class SignalName : Control.SignalName
  {
  }
}
