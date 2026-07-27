// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Extensions.SignalAwaiterExtensions
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Extensions;

public static class SignalAwaiterExtensions
{
  public static async Task ToTask(this SignalAwaiter awaiter)
  {
    Variant[] variantArray = await awaiter;
  }
}
