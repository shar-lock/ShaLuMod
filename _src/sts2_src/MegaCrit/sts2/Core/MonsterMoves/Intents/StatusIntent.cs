// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.MonsterMoves.Intents.StatusIntent
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Localization;
using System;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.MonsterMoves.Intents;

public class StatusIntent : AbstractIntent
{
  public override IntentType IntentType => IntentType.StatusCard;

  protected override LocString IntentLabelFormat
  {
    get => new LocString("intents", "FORMAT_STATUS_CARD_COUNT");
  }

  protected override string SpritePath => "atlases/intent_atlas.sprites/intent_status_card.tres";

  protected override string IntentPrefix => "STATUS";

  public int CardCount { get; }

  public StatusIntent(int count) => this.CardCount = count;

  public override LocString GetIntentLabel(IEnumerable<Creature> _, Creature __)
  {
    LocString intentLabelFormat = this.IntentLabelFormat;
    intentLabelFormat.Add("CardCount", (Decimal) this.CardCount);
    return intentLabelFormat;
  }

  protected override LocString GetIntentDescription(IEnumerable<Creature> targets, Creature owner)
  {
    LocString intentDescription = base.GetIntentDescription(targets, owner);
    intentDescription.Add("CardCount", (Decimal) this.CardCount);
    return intentDescription;
  }
}
