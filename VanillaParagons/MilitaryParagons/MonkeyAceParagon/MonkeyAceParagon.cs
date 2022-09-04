using Assets.Scripts.Models.Towers;
using Assets.Scripts.Unity;
using BTD_Mod_Helper.Api.Towers;
using Assets.Scripts.Simulation.Towers.Behaviors;
using Assets.Scripts.Models.Towers.Behaviors.Attack;
using Assets.Scripts.Models.Towers.Behaviors.Emissions;
using Assets.Scripts.Models.Towers.Filters;
using Assets.Scripts.Models.Towers.Projectiles;
using Assets.Scripts.Models.Towers.Projectiles.Behaviors;
using BTD_Mod_Helper.Extensions;
using System.Linq;
using UnhollowerBaseLib;
using Assets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Assets.Scripts.Models.Towers.Weapons;
using Assets.Scripts.Models.Towers.Behaviors;
using Assets.Scripts.Models.Towers.Weapons.Behaviors;
using Il2CppSystem.Collections.Generic;

namespace VanillaParagons.MilitaryParagons.MonkeyAceParagon
{
    public class MonkeyAceParagonBase : ModVanillaParagon
    {
        public override string BaseTower => "MonkeyAce-502"; // the base bahaviors I'll build off of, basically needed cuz I'm not coding in all that monkey ace flying stuff.
    }
    public class MonkeyAceParagon : ModParagonUpgrade<MonkeyAceParagonBase>
    {
        public override string DisplayName => "Rain of fire";
        public override int Cost => 1367000;
        public override string Description => "why does this exist";
        public override void ApplyUpgrade(TowerModel tower) //modifying the behaviors, and btd mod helper supplies the parameter of TowerModel (makes the tower and adds a variable so I can edit the tower).
        {
            // created variables to use later, using the tower inserted into the tower parimeter.
            var weapon = tower.GetWeapon();
            var projectile = weapon.projectile;

            //projectile count.
            weapon.emission = new ArcEmissionModel("MonkeyAceParagonArcEmissionModel", 46, 180, 360, null, false);
            weapon.rate *= 0.5f;
            //damage.
            projectile.AddBehavior(new DamageModifierForTagModel("DamageModifierForTagModel_Moabs", "Moabs", 1, 220, false, false));
            projectile.GetDamageModel().damage += 360;
            projectile.pierce = 50;
            //seeking.
            var seekingBehavior = new TrackTargetModel("MonkeyAceParagonTrackTargetModel", 9999999, true, false, 360, true, 700, false, false);
            projectile.AddBehavior(seekingBehavior);

            //using the tower perimeter to get the missles, since the 5-0-2 already has them.
            var missile = tower.GetBehaviors<AttackAirUnitModel>().First(a => a.name == "AttackAirUnitModel_Anti-MoabMissile_");

            //missle attack speed
            missile.weapons[0].Rate *= 0.1f;
            //missle damage
            missile.weapons[0].projectile.AddBehavior(new DamageModifierForTagModel("DamageModifierForTagModel_Moabs", "Moabs", 1, 22500f, false, false));
            missile.GetDescendants<DamageModel>().ForEach(damage => damage.damage = 3000.0f);
            missile.GetDescendants<DamageModel>().ForEach(damage => damage.immuneBloonProperties = BloonProperties.None);

            //found pineapples from an existing tower and duplicating them into my tower.
            tower.AddBehavior(Game.instance.model.GetTowerFromId("MonkeyAce-220").GetBehaviors<AttackAirUnitModel>()[1].Duplicate());
            var pineapples = tower.GetBehaviors<AttackAirUnitModel>()[2];
            //pineapple attack speed
            pineapples.weapons[0].Rate *= 0.05f;
            //pineapple damage
            pineapples.GetDescendants<DamageModel>().ForEach(damage => damage.damage = 4500.0f);
            pineapples.GetDescendants<DamageModel>().ForEach(damage => damage.immuneBloonProperties = BloonProperties.None);

            //found ability model from an existing tower and duplicating it into my tower, and also getting the activate attack model (a model containing attack models exclusive to when the ability is active).
            var groundzeroAbility = Game.instance.model.GetTower(TowerType.MonkeyAce, 0, 5).GetAbility().Duplicate();
            var goundzeroAbilityAttackModel = groundzeroAbility.GetBehavior<ActivateAttackModel>();

            //getting rid of the caps and changing the damage.
            goundzeroAbilityAttackModel.GetDescendant<AttackAirUnitModel>().weapons[0].projectile.GetDamageModel().maxDamage = 999999;
            goundzeroAbilityAttackModel.GetDescendant<AttackAirUnitModel>().weapons[0].projectile.GetDamageModel().CapDamage(999999);
            goundzeroAbilityAttackModel.GetDescendant<AttackAirUnitModel>().weapons[0].projectile.GetDamageModel().damage = 450000;
            //getting rid of the caps and changing the pierce.
            goundzeroAbilityAttackModel.GetDescendant<AttackAirUnitModel>().weapons[0].projectile.maxPierce = 999999;
            goundzeroAbilityAttackModel.GetDescendant<AttackAirUnitModel>().weapons[0].projectile.CapPierce(999999);
            goundzeroAbilityAttackModel.GetDescendant<AttackAirUnitModel>().weapons[0].projectile.pierce = 250000;

            tower.AddBehavior(groundzeroAbility);

            var flyingFortress1 = Game.instance.model.GetTowerFromId("MonkeyAce-005").GetBehaviors<AttackAirUnitModel>()[0].Duplicate();
            var flyingFortress2 = Game.instance.model.GetTowerFromId("MonkeyAce-005").GetBehaviors<AttackAirUnitModel>()[1].Duplicate();
            var flyingFortress3 = Game.instance.model.GetTowerFromId("MonkeyAce-005").GetBehaviors<AttackAirUnitModel>()[2].Duplicate();
            var flyingFortress4 = Game.instance.model.GetTowerFromId("MonkeyAce-005").GetBehaviors<AttackAirUnitModel>()[3].Duplicate();
            List<AttackAirUnitModel> fortressAttacks = new List<AttackAirUnitModel>() { };
            fortressAttacks.Add(flyingFortress1);
            fortressAttacks.Add(flyingFortress2);
            fortressAttacks.Add(flyingFortress3);
            fortressAttacks.Add(flyingFortress4);
            foreach (var fortressAttack in fortressAttacks)
            {
                if (fortressAttack.weapons.Length > 0)
                {
                    fortressAttack.weapons[0].projectile.AddBehavior(new TrackTargetModel("MonkeyAceParagonTrackTargetModel", 9999999, true, false, 360, true, 10000, false, false));
                    fortressAttack.weapons[0].projectile.AddBehavior(new DamageModifierForTagModel("DamageModifierForTagModel_Moabs", "Moabs", 1, 1100f, false, false));
                    fortressAttack.weapons[0].projectile.GetDamageModel().damage = 750;
                    fortressAttack.weapons[0].rate *= 0.5f;
                }
                tower.AddBehavior(fortressAttack);
            }

            //making the tower hit camo by removing the invisible filters in some of my projectiles, and making the tower see camo like the ninja.
            tower.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
            tower.AddBehavior(new OverrideCamoDetectionModel("OverrideCamoDetectionModel_", true));
        }
    }
}