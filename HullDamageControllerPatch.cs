using CG.Game;
using CG.Ship.Repair;
using CG.Space;
using HarmonyLib;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;
using VoidManager.Utilities;
using static TimeMachineBehaviour;

namespace NoUnrepairableDamage
{
    [HarmonyPatch(typeof(HullDamageController), "OnRepairedBreach")]
    internal class HullDamageControllerPatch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            //Find "this.playerShip.HitPoints += num;"
            List<CodeInstruction> targetSequence = new()
            {
                new CodeInstruction(OpCodes.Ldarg_0),   //this
                new CodeInstruction(OpCodes.Ldfld),     //playership
                new CodeInstruction(OpCodes.Dup),       //playership
                new CodeInstruction(OpCodes.Callvirt),  //Hitpoints (getter)
                new CodeInstruction(OpCodes.Ldloc_0),   //num
                new CodeInstruction(OpCodes.Add)        //+
                //new CodeInstruction(OpCodes.Callvirt) //Hitpoints (setter)
            };

            //Insert "ClampHitpoints" after "add" and before "setter"
            List<CodeInstruction> patchSequence = new()
            {
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(HullDamageControllerPatch), nameof(ClampHitpoints)))
            };

            return HarmonyHelpers.PatchBySequence(instructions, targetSequence, patchSequence, HarmonyHelpers.PatchMode.AFTER, HarmonyHelpers.CheckMode.NEVER);
        }

        [HarmonyPostfix]
        static void PostfixOnRepairedBreach(ref HullBreach breach, HullDamageController __instance, PlayerControlledShip ___playerShip)
        {
            List<HullBreach> list = new List<HullBreach>();
            foreach (HullBreach hullBreach in __instance.Breaches)
            {
                if (hullBreach.State.condition == BreachCondition.Major || hullBreach.State.condition == BreachCondition.Minor)
                {
                    list.Add(hullBreach);
                }
            }

            if (list.Count == 0)
            {
                ___playerShip.HitPoints = ___playerShip.MaxHitPointsValue;
            }
        }

        //Prevent ship hitpoints being higher than max hitpoints
        private static float ClampHitpoints(float hitpoints)
        {
            return Mathf.Min(ClientGame.Current.PlayerShip.MaxHitPointsValue, hitpoints); //Changed to use MaxHitPointsValue as that seems to be working now
        }
    }
}
