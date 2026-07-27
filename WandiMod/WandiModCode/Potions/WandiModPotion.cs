using BaseLib.Abstracts;
using BaseLib.Utils;
using WandiMod.WandiModCode.Character;

namespace WandiMod.WandiModCode.Potions;

[Pool(typeof(WandiModPotionPool))]
public abstract class WandiModPotion : CustomPotionModel;