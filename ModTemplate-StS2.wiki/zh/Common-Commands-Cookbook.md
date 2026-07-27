本页收录了一系列代码片段，用于实现游戏中不同动作常用模式。它们参考了基础游戏的代码示例，并尽量遵循其编码约定。你可以将这些片段组合起来，为自己的内容实现想要的效果。

# Actions
以下是一些 Model 中常用方法及其一般用途
| Action | Notes |
| ------------- | ------------- |
|OnPlay | |
|OnUpgrade| |
|BeforeCardPlayed| |
|AfterCardPlayed| |
|AfterSideTurnStart| |
|AfterTurnEnd| |
|AfterCardDrawn| |
|AfterPowerAmountChanged| |


# Variables

## Canonical Vars
StS2 中的 Model 通常会在 `CanonicalVars` 中定义基础数值。配置正确后，这些变量就作为 Model 的基础 _数据真相源_（source of truth）。你可以为 Model 定义任意数量的变量。

下面各节会给出对应变量的代码片段，例如：
```c#
protected override IEnumerable<DynamicVar> CanonicalVars => [ new DamageVar(6, ValueProp.Move)), new PowerVar<StrengthPower>(1) ];
```

### Generic Vars
如果你有一个不属于任何现有类别的变量，可以使用泛型变量类型来自定义属于你自己的变量。
```c#
protected override IEnumerable<DynamicVar> CanonicalVars =>
[
    //通过 nameof
    new IntVar(nameof(LostFingers), 2),

    //通过硬编码字符串
    new IntVar("NumberOfGremlins", 5),
    new StringVar("TamerName", "Franklin"),
    new BoolVar("IsStinky", true)
];
```

_注意：所有变量底层都遵循同样的机制，但通过其 Dynamic 变量来调用会更方便。_

### Calculated Vars
TODO

## Dynamic Vars

游戏会通过 Model 的 `DynamicVars` 计算被修改后的数值。这些数值才是你执行命令时真正要用到的。

大多数泛型 dynamic 变量都可以通过其内置 getter 直接调用，例如 `DynamicVars.Damage.BaseValue`。如果某个变量没有 getter，则必须通过名称来获取，例如 `DynamicVars[nameof(StrengthLossPower)].BaseValue`。

# Commands
命令是构建 Model 动作的基础积木。

## Attack Commands
根据你卡牌的命令，你通常还需要相应地修改卡牌的 `TargetType`。

**Variable**:
```c# 
new DamageVar(6, ValueProp.Move))
```

### Single Target (TargetType.AnyEnemy)
**Commmand**:
```c#
await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
    .FromCard(this)
    .Targeting(play.Target)
    .WithHitFx("vfx/vfx_attack_slash")
    .Execute(choiceContext);
```

### All enemies (TargetType.AllEnemies)
**Commmand**:
```c#
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .TargetingAllOpponents(CombatState)
            .WithHitFx("vfx/vfx_attack_blunt", null, "heavy_attack.mp3")
            .Execute(choiceContext);
```

### Random (TargetType.RandomEnemy)
**Commmand**:
```c#
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .TargetingRandomOpponents(CombatState)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
```

### Multi-hit
**Variables**:
```c# 
new DamageVar(6, ValueProp.Move)),
new RepeatVar(3),
```

**Command**:
```c#
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .WithHitCount(DynamicVars.Repeat.IntValue)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
```

## Block Commands
**Variable**:
```c# 
new BlockVar(6, ValueProp.Move))
```

**Command**:
```c#
await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, play);
```

## Power Commands

Power（即 `PowerModel`）是可以施加到生物或玩家身上的任何增益或减益。它们既可以表示从 Power 类卡牌获得的增益，也可以表示由 Skill 类卡牌或其他效果赋予的效果。

它们都需要一个 `PowerType` 和一个 `PowerStackType`。

```c#
public class ExplosivesPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;                           //增益或减益
    public override PowerStackType StackType => PowerStackType.Counter;         //None、Counter、Single
}
```

**Variable**:

Power 的 canonical 变量独特之处在于它们需要一个 `type` 参数。
```c#
new PowerVar<ExplosivesPower>(1),
new PowerVar<ExplosivesPower>("FollowupExplosivePower", 1),
```

### Applying Powers
**Command**:

与其对应的变量类似，施加 power 也需要一个 `type` 参数。
```c#
await PowerCmd.Apply<ExplosivesPower>(
    new ThrowingPlayerChoiceContext(),
    Owner.Creature,
    DynamicVars[nameof(ExplosivesPower)].BaseValue,
    Owner.Creature,
    this);
```

### Modifying Powers
**Commands**:
```c#
await PowerCmd.Decrement(this);
await PowerCmd.Remove(this);
await PowerCmd.ModifyAmount(ctx, this, -1, null, null);
SetAmount(Amount - 6);
```

## Card Commands

### Upgrade
**Command**:
```c#
CardCmd.Upgrade(card);
```

### Transform
**Command**:
TODO

### AutoPlay
**Command**:
```c#
await CardCmd.AutoPlay(choiceContext, card, target);
```

### Exhaust
**Command**:
```c#
await CardCmd.Exhaust(choiceContext, card);
```

### Discard
**Command**:
```c#
await CardCmd.Discard(choiceContext, card);
```

### Apply Keyword
**Command**:
```c#
CardCmd.ApplyKeyword(card, CardKeyword.Ethereal);
```

### Enchant
**Command**:
```c#
CardCmd.Enchant<Sharp>(card, amount);
```

## Card Select Commands

### From Grid
**Command**:
```c#
var prefs = new CardSelectorPrefs(SelectionScreenPrompt, 1);
var card = (await CardSelectCmd.FromSimpleGrid(
    choiceContext,
    PileType.Discard.GetPile(Owner).Cards,
    Owner,
    prefs)).FirstOrDefault();
```

### From Hand
**Command**:
```c#
var prefs = new CardSelectorPrefs(SelectionScreenPrompt, 1);
var selected = (await CardSelectCmd.FromHand(choiceContext, Owner, prefs, null, this))
    .FirstOrDefault();
```

### From Hand (range, with filter)
**Command**:
```c#
var prefs = new CardSelectorPrefs(SelectionScreenPrompt, 0, DynamicVars.Cards.IntValue);
var selected = await CardSelectCmd.FromHand(choiceContext, Owner, prefs,
    c => c.IsTransformable, this);
```

## CardPile Commands

### Draw
**Variable**:
```c#
new CardsVar(1)
```

**Command**:
```c#
await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
```

### Add (existing card)
**Command**:
```c#
await CardPileCmd.Add(card, PileType.Hand);
await CardPileCmd.Add(card, PileType.Draw, CardPilePosition.Top);
await CardPileCmd.Add(card, PileType.Draw, CardPilePosition.Random, this);
```

### Add Generated Card to Combat
**Command**:
```c#
CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardToCombat(
    card, PileType.Draw, Owner, CardPilePosition.Random));
```

### Add Generated Cards (plural) to Combat
**Command**:
```c#
CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardsToCombat(
    cards, PileType.Hand, Owner.Player));
```

### Add to Combat and Preview
**Command**:
```c#
await CardPileCmd.AddToCombatAndPreview<Debris>(Owner.Creature, PileType.Hand, 4, Owner);
```

### Add Curses to Deck
**Command**:
```c#
await CardPileCmd.AddCursesToDeck(Enumerable.Repeat(ModelDb.Card<Guilty>(), 1), Owner);
```
