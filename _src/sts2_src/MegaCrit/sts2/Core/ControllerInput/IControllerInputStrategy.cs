// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.ControllerInput.IControllerInputStrategy
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.ControllerInput;

public interface IControllerInputStrategy
{
  Task Init();

  void ProcessInput();

  Texture2D? GetHotkeyIcon(string hotkey);

  ControllerConfig? ControllerConfig { get; }

  Dictionary<StringName, StringName> GetDefaultControllerInputMap { get; }

  string GetControllerName();

  bool ShouldAllowControllerRebinding { get; }

  Vector2 GetLeftAnalogStickDirection();
}
