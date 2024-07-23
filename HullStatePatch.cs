using HarmonyLib;
using CG.Ship.Repair;

namespace NoUnrepairableDamage
{
    [HarmonyPatch(typeof(HullState), "SetRepairableHp")]
    public class HullStatePatch
    {
        [HarmonyPrefix]
        static bool PrefixSetRepairableHp(ref float newRepairableHp, ref float newUnrepairableHp)
        {
            // Convert all unrepairable damage to repairable damage
            newRepairableHp += newUnrepairableHp;
            newUnrepairableHp = 0;

            return true;
        }
    }
}
