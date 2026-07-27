// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.PlayerHurtVignetteHelper
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Vfx.Ui;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx;

public static class PlayerHurtVignetteHelper
{
  private static NLowHpBorderVfx? _currentVfx;

  public static void Play()
  {
    if (PlayerHurtVignetteHelper._currentVfx != null && GodotObject.IsInstanceValid((GodotObject) PlayerHurtVignetteHelper._currentVfx))
    {
      PlayerHurtVignetteHelper._currentVfx.Play();
    }
    else
    {
      PlayerHurtVignetteHelper._currentVfx = NLowHpBorderVfx.Create();
      NRun instance = NRun.Instance;
      if (instance != null)
        ((Node) instance.GlobalUi).AddChildSafely((Node) PlayerHurtVignetteHelper._currentVfx);
      PlayerHurtVignetteHelper._currentVfx?.Play();
    }
  }
}
