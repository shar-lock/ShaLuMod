// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Encounters.SlitheringStranglerNormal
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Encounters;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Encounters;

public sealed class SlitheringStranglerNormal : EncounterModel
{
  private static readonly MonsterModel[] _smallSlimes = new MonsterModel[2]
  {
    (MonsterModel) ModelDb.Monster<LeafSlimeS>(),
    (MonsterModel) ModelDb.Monster<TwigSlimeS>()
  };
  private static readonly MonsterModel[] _mediumSlimes = new MonsterModel[2]
  {
    (MonsterModel) ModelDb.Monster<LeafSlimeM>(),
    (MonsterModel) ModelDb.Monster<TwigSlimeM>()
  };

  public override RoomType RoomType => RoomType.Monster;

  public override IEnumerable<EncounterTag> Tags
  {
    get
    {
      return (IEnumerable<EncounterTag>) new \u003C\u003Ez__ReadOnlyArray<EncounterTag>(new EncounterTag[2]
      {
        EncounterTag.Jaxfruit,
        EncounterTag.Slimes
      });
    }
  }

  public override IEnumerable<MonsterModel> AllPossibleMonsters
  {
    get
    {
      MonsterModel[] smallSlimes = SlitheringStranglerNormal._smallSlimes;
      MonsterModel[] mediumSlimes = SlitheringStranglerNormal._mediumSlimes;
      int num1 = 0;
      MonsterModel[] items = new MonsterModel[2 + (smallSlimes.Length + mediumSlimes.Length)];
      ReadOnlySpan<MonsterModel> readOnlySpan1 = new ReadOnlySpan<MonsterModel>(smallSlimes);
      readOnlySpan1.CopyTo(new Span<MonsterModel>(items).Slice(num1, readOnlySpan1.Length));
      int num2 = num1 + readOnlySpan1.Length;
      ReadOnlySpan<MonsterModel> readOnlySpan2 = new ReadOnlySpan<MonsterModel>(mediumSlimes);
      readOnlySpan2.CopyTo(new Span<MonsterModel>(items).Slice(num2, readOnlySpan2.Length));
      int index1 = num2 + readOnlySpan2.Length;
      items[index1] = (MonsterModel) ModelDb.Monster<SnappingJaxfruit>();
      int index2 = index1 + 1;
      items[index2] = (MonsterModel) ModelDb.Monster<SlitheringStrangler>();
      return (IEnumerable<MonsterModel>) new \u003C\u003Ez__ReadOnlyArray<MonsterModel>(items);
    }
  }

  protected override IReadOnlyList<(MonsterModel, string?)> GenerateMonsters()
  {
    List<MonsterModel> monsterModelList1;
    switch (this.Rng.NextItem<SlitheringStranglerNormal.SecondaryEnemyType>((IEnumerable<SlitheringStranglerNormal.SecondaryEnemyType>) Enum.GetValues<SlitheringStranglerNormal.SecondaryEnemyType>()))
    {
      case SlitheringStranglerNormal.SecondaryEnemyType.SnappingJaxfruit:
        int capacity1 = 1;
        List<MonsterModel> monsterModelList2 = new List<MonsterModel>(capacity1);
        CollectionsMarshal.SetCount<MonsterModel>(monsterModelList2, capacity1);
        CollectionsMarshal.AsSpan<MonsterModel>(monsterModelList2)[0] = (MonsterModel) ModelDb.Monster<SnappingJaxfruit>();
        monsterModelList1 = monsterModelList2;
        break;
      case SlitheringStranglerNormal.SecondaryEnemyType.MediumSlime:
        int capacity2 = 1;
        List<MonsterModel> monsterModelList3 = new List<MonsterModel>(capacity2);
        CollectionsMarshal.SetCount<MonsterModel>(monsterModelList3, capacity2);
        CollectionsMarshal.AsSpan<MonsterModel>(monsterModelList3)[0] = this.Rng.NextItem<MonsterModel>((IEnumerable<MonsterModel>) SlitheringStranglerNormal._mediumSlimes);
        monsterModelList1 = monsterModelList3;
        break;
      case SlitheringStranglerNormal.SecondaryEnemyType.SmallSlimes:
        int capacity3 = 2;
        List<MonsterModel> monsterModelList4 = new List<MonsterModel>(capacity3);
        CollectionsMarshal.SetCount<MonsterModel>(monsterModelList4, capacity3);
        Span<MonsterModel> span = CollectionsMarshal.AsSpan<MonsterModel>(monsterModelList4);
        int num1 = 0;
        span[num1] = this.Rng.NextItem<MonsterModel>((IEnumerable<MonsterModel>) SlitheringStranglerNormal._smallSlimes);
        int num2 = num1 + 1;
        span[num2] = this.Rng.NextItem<MonsterModel>((IEnumerable<MonsterModel>) SlitheringStranglerNormal._smallSlimes);
        monsterModelList1 = monsterModelList4;
        break;
      default:
        throw new ArgumentOutOfRangeException();
    }
    List<MonsterModel> source = monsterModelList1;
    source.Add((MonsterModel) ModelDb.Monster<SlitheringStrangler>());
    return (IReadOnlyList<(MonsterModel, string)>) source.Select<MonsterModel, (MonsterModel, string)>((Func<MonsterModel, (MonsterModel, string)>) (m => (m.ToMutable(), (string) null))).ToList<(MonsterModel, string)>();
  }

  private enum SecondaryEnemyType
  {
    SnappingJaxfruit,
    MediumSlime,
    SmallSlimes,
  }
}
