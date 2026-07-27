// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.ReptileTrinketPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Models.Relics;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public class ReptileTrinketPower : TemporaryStrengthPower
{
  public override AbstractModel OriginModel => (AbstractModel) ModelDb.Relic<ReptileTrinket>();
}
