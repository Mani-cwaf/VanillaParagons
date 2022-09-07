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
using static VanillaParagons.Main;

namespace VanillaParagons.MilitaryParagons.MonkeyAceParagon
{
    public class MonkeyAceParagonBase : ModVanillaParagon
    {
        public override string BaseTower => "MonkeyAce-502";
    }
    public class MonkeyAceParagon : ModParagonUpgrade<MonkeyAceParagonBase>
    {
        public override string DisplayName => "Rain of fire";
        public override int Cost => 960000;
        public override string Description => "why does this exist";
        public override void ApplyUpgrade(TowerModel tower)
        {
            var weapon = tower.GetWeapon();
            var projectile = weapon.projectile;
            var groundzeroAbility = Game.instance.model.GetTower(TowerType.MonkeyAce, 0, 5).GetAbility().Duplicate();
            var goundzeroAbilityAttackModel = groundzeroAbility.GetBehavior<ActivateAttackModel>();
            var missile = tower.GetBehaviors<AttackAirUnitModel>().First(a => a.name == "AttackAirUnitModel_Anti-MoabMissile_");
            var pineapples = Game.instance.model.GetTowerFromId("MonkeyAce-220").GetBehaviors<AttackAirUnitModel>()[1].Duplicate();
            var flyingFortress1 = Game.instance.model.GetTowerFromId("MonkeyAce-005").GetBehaviors<AttackAirUnitModel>()[0].Duplicate();
            var flyingFortress2 = Game.instance.model.GetTowerFromId("MonkeyAce-005").GetBehaviors<AttackAirUnitModel>()[1].Duplicate();
            var flyingFortress3 = Game.instance.model.GetTowerFromId("MonkeyAce-005").GetBehaviors<AttackAirUnitModel>()[2].Duplicate();
            var flyingFortress4 = Game.instance.model.GetTowerFromId("MonkeyAce-005").GetBehaviors<AttackAirUnitModel>()[3].Duplicate();
            var seekingBehavior = new TrackTargetModel("MonkeyAceParagonTrackTargetModel", 9999999, true, false, 360, true, 700, false, false);
            AttackAirUnitModel[] fortressAttacks = new AttackAirUnitModel[] { flyingFortress1, flyingFortress2, flyingFortress3, flyingFortress4 };

            if (OPParagons)
            {
                weapon.emission = new ArcEmissionModel("MonkeyAceParagonArcEmissionModel", 46, 180, 360, null, false);
                weapon.Rate *= 0.5f;

                projectile.AddBehavior(new DamageModifierForTagModel("DamageModifierForTagModel_Moabs", "Moabs", 1, 220, false, false));
                projectile.GetDamageModel().damage += 360;
                projectile.pierce = 50;

                missile.weapons[0].Rate *= 0.1f;
                missile.weapons[0].projectile.AddBehavior(new DamageModifierForTagModel("DamageModifierForTagModel_Moabs", "Moabs", 1, 22500f, false, false));
                missile.GetDescendants<DamageModel>().ForEach(damage => damage.damage = 4500);
                missile.GetDescendants<DamageModel>().ForEach(damage => damage.immuneBloonProperties = BloonProperties.None);

                pineapples.weapons[0].Rate *= 0.05f;
                pineapples.GetDescendants<DamageModel>().ForEach(damage => damage.damage = 4500);
                pineapples.GetDescendants<DamageModel>().ForEach(damage => damage.immuneBloonProperties = BloonProperties.None);

                goundzeroAbilityAttackModel.GetDescendant<AttackAirUnitModel>().weapons[0].projectile.GetDamageModel().maxDamage = 999999;
                goundzeroAbilityAttackModel.GetDescendant<AttackAirUnitModel>().weapons[0].projectile.GetDamageModel().CapDamage(999999);
                goundzeroAbilityAttackModel.GetDescendant<AttackAirUnitModel>().weapons[0].projectile.GetDamageModel().damage = 450000;
                goundzeroAbilityAttackModel.GetDescendant<AttackAirUnitModel>().weapons[0].projectile.maxPierce = 999999;
                goundzeroAbilityAttackModel.GetDescendant<AttackAirUnitModel>().weapons[0].projectile.CapPierce(999999);
                goundzeroAbilityAttackModel.GetDescendant<AttackAirUnitModel>().weapons[0].projectile.pierce = 250000;

                foreach (var fortressAttack in fortressAttacks)
                {
                    if (fortressAttack.weapons.Length > 0)
                    {
                        fortressAttack.weapons[0].projectile.AddBehavior(new TrackTargetModel("MonkeyAceParagonTrackTargetModel", 9999999, true, false, 360, true, 10000, false, false));
                        fortressAttack.weapons[0].projectile.AddBehavior(new DamageModifierForTagModel("DamageModifierForTagModel_Moabs", "Moabs", 1, 1100f, false, false));
                        fortressAttack.weapons[0].projectile.GetDamageModel().damage = 750;
                        fortressAttack.weapons[0].Rate *= 0.5f;
                    }
                }
            }
            else
            {
                weapon.emission = new ArcEmissionModel("MonkeyAceParagonArcEmissionModel", 46, 180, 360, null, false);
                weapon.Rate *= 0.5f;

                projectile.AddBehavior(new DamageModifierForTagModel("DamageModifierForTagModel_Moabs", "Moabs", 1, 2, false, false));
                projectile.GetDamageModel().damage += 4;
                projectile.pierce = 50;

                missile.weapons[0].Rate *= 0.1f;
                missile.weapons[0].projectile.AddBehavior(new DamageModifierForTagModel("DamageModifierForTagModel_Moabs", "Moabs", 1, 56, false, false));
                missile.GetDescendants<DamageModel>().ForEach(damage => damage.damage = 56);
                missile.GetDescendants<DamageModel>().ForEach(damage => damage.immuneBloonProperties = BloonProperties.None);

                pineapples.weapons[0].Rate *= 0.05f;
                pineapples.GetDescendants<DamageModel>().ForEach(damage => damage.damage = 56);
                pineapples.GetDescendants<DamageModel>().ForEach(damage => damage.immuneBloonProperties = BloonProperties.None);

                goundzeroAbilityAttackModel.GetDescendant<AttackAirUnitModel>().weapons[0].projectile.GetDamageModel().maxDamage = 999999;
                goundzeroAbilityAttackModel.GetDescendant<AttackAirUnitModel>().weapons[0].projectile.GetDamageModel().CapDamage(999999);
                goundzeroAbilityAttackModel.GetDescendant<AttackAirUnitModel>().weapons[0].projectile.GetDamageModel().damage = 2000;
                goundzeroAbilityAttackModel.GetDescendant<AttackAirUnitModel>().weapons[0].projectile.maxPierce = 999999;
                goundzeroAbilityAttackModel.GetDescendant<AttackAirUnitModel>().weapons[0].projectile.CapPierce(999999);
                goundzeroAbilityAttackModel.GetDescendant<AttackAirUnitModel>().weapons[0].projectile.pierce = 250000;

                foreach (var fortressAttack in fortressAttacks)
                {
                    if (fortressAttack.weapons.Length > 0)
                    {
                        fortressAttack.weapons[0].projectile.AddBehavior(new TrackTargetModel("MonkeyAceParagonTrackTargetModel", 9999999, true, false, 360, true, 10000, false, false));
                        fortressAttack.weapons[0].projectile.AddBehavior(new DamageModifierForTagModel("DamageModifierForTagModel_Moabs", "Moabs", 1, 13, false, false));
                        fortressAttack.weapons[0].projectile.GetDamageModel().damage = 9;
                        fortressAttack.weapons[0].Rate *= 0.5f;
                    }
                }
            }
            foreach (var fortressAttack in fortressAttacks)
            {
                tower.AddBehavior(fortressAttack);
            }
            projectile.AddBehavior(seekingBehavior);
            tower.AddBehavior(pineapples);
            tower.AddBehavior(groundzeroAbility);
            tower.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
            tower.AddBehavior(new OverrideCamoDetectionModel("OverrideCamoDetectionModel_", true));
        }
    }
}