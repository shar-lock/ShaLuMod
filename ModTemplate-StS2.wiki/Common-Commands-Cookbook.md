This page contains a series of code snippets for common patterns that are used within the game's code for different actions. They are inspired by base game code examples and attempt to follow their coding conventions. Compose these together to create the actions you want for your content.

# Actions
Here are a Model methods you will commonly use and their general use case
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
Models in STS2 will often have base values defined within `CanonicalVars`. When set up properly, these act as the base _source of truth_ for your Models. You can define as many variables for a Model as you like.

In the sections below, there will be a code snippet for their associated variable Eg.
```c#
protected override IEnumerable<DynamicVar> CanonicalVars => [ new DamageVar(6, ValueProp.Move)), new PowerVar<StrengthPower>(1) ];
```

### Generic Vars
If you have a variable that does not fall into any existing category, you can use the generic var types to make custom your own custom variable. 
```c#
protected override IEnumerable<DynamicVar> CanonicalVars =>
[
    //via nameof
    new IntVar(nameof(LostFingers), 2),

    //via hardcoded strings
    new IntVar("NumberOfGremlins", 5),
    new StringVar("TamerName", "Franklin"),
    new BoolVar("IsStinky", true)
];
```

_Note: All variables follow this same mechanic underlyingly, but are easier to call their Dynamic Variable._

### Calculated Vars
TODO

## Dynamic Vars

The game calculates modified values through a Model's `DynamicVars`. These are the values you want to use when performing commands.

You can invoke most generic dynamic vars via their built in getter Eg. `DynamicVars.Damage.BaseValue`. If they do not have a getter you have to fetch them by name Eg. `DynamicVars[nameof(StrengthLossPower)].BaseValue`.

# Commands
Commands are the building blocks of your Model's actions.

## Attack Commands
Depending on your card's command, you will also have to change your card's `TargetType`.

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

Powers, as in `PowerModel`s are any buff or debuff that can be applied to a creature or player. They can represent buffs gained from Power cards, but also effects bestowed by Skill cards or by other other effects.

They require both a `PowerType` and a `PowerStackType`.

```c#
public class ExplosivesPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;                           //Buff or Debuff
    public override PowerStackType StackType => PowerStackType.Counter;         //None, Counter, Single
}
```

**Variable**:

Power canonical variables are unique in that they require a `type` parameter.
```c#
new PowerVar<ExplosivesPower>(1),
new PowerVar<ExplosivesPower>("FollowupExplosivePower", 1),
```

### Applying Powers
**Command**:

Similarly to their Variables, applying powers also needs a `type` parameter.
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