// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.Mocks.MockTemporaryStrengthLossPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Models.Cards.Mocks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers.Mocks;

public class MockTemporaryStrengthLossPower : TemporaryStrengthPower
{
  public override bool IsMock => true;

  public override AbstractModel OriginModel => (AbstractModel) ModelDb.Card<MockSkillCard>();

  protected override bool IsPositive => false;
}
