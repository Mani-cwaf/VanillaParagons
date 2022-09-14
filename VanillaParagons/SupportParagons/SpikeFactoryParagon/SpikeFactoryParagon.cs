using Assets.Scripts.Models.Towers;
using Assets.Scripts.Models.Towers.Behaviors;
using Assets.Scripts.Models.Towers.Behaviors.Abilities;
using Assets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Assets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;
using Assets.Scripts.Models.Towers.Projectiles;
using Assets.Scripts.Models.Towers.Projectiles.Behaviors;
using Assets.Scripts.Models.Towers.Weapons.Behaviors;
using Assets.Scripts.Simulation.Towers.Behaviors;
using Assets.Scripts.Unity;
using Assets.Scripts.Unity.Display;
using Assets.Scripts.Utils;
using BTD_Mod_Helper.Api.Display;
using BTD_Mod_Helper.Api.Towers;
using BTD_Mod_Helper.Extensions;
using System.Runtime.InteropServices;
using System.Linq;
using static VanillaParagons.Main;
using Assets.Scripts.Models.Towers.Behaviors.Emissions;

namespace VanillaParagons.SupportParagons.SpikeFactoryParagon
{
    public class SpikeFactoryParagonBase : ModVanillaParagon
    {
        public override string BaseTower => "SpikeFactory-002";
        public override string Name => "SpikeFactory";
    }
    public class SpikeFactoryParagon : ModParagonUpgrade<SpikeFactoryParagonBase>
    {
        public override string DisplayName => "Lego Factory";
        public override int Cost => 2000000;
        public override string Description => "nothing's worse than stepping on a lego.";
        //public override string Icon => "";
        //public override string Portrait => "";
        public override void ApplyUpgrade(TowerModel tower)
        {
            var weapon = tower.GetWeapon();
            var projectile = weapon.projectile = Game.instance.model.GetTowerFromId("SpikeFactory-005").GetWeapon().projectile.Duplicate();
            var abilty = Game.instance.model.GetTowerFromId("SpikeFactory-050").GetAbility().Duplicate();
            var explosionBehavior = Game.instance.model.GetTowerFromId("SpikeFactory-500").GetDescendants<ProjectileModel>().ToIl2CppList().First(a => a.name.Contains("ProjectileModel_Projectile")).GetBehavior<CreateProjectileOnExhaustPierceModel>().Duplicate();
            var explosion = Game.instance.model.GetTowerFromId("SpikeFactory-500").GetDescendants<ProjectileModel>().ToIl2CppList().First(a => a.name.Contains("ProjectileModel_Explosion")).Duplicate();
            var aat1 = abilty.GetBehavior<ActivateAttackModel>().attacks[0].weapons[0];
            var aat2 = abilty.GetBehavior<ActivateAttackModel>().attacks[1].weapons[0];
            var aat3 = abilty.GetBehavior<ActivateAttackModel>().attacks[2].weapons[0];
            var aat4 = abilty.GetBehavior<ActivateAttackModel>().attacks[3].weapons[0];
            if (OPParagons)
            {
                weapon.Rate *= 0.1f;
                projectile.GetDamageModel().damage = 125;
                projectile.pierce = 3000;
                projectile.GetBehavior<AgeModel>().Lifespan = 9999999;
                projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                projectile.RemoveBehavior<FadeProjectileModel>();
                projectile.RemoveBehavior<EndOfRoundClearBypassModel>();
                explosion.GetDamageModel().damage = 100;
                tower.GetAttackModel().RemoveBehavior<FarTargetTrackModel>();
                tower.GetAttackModel().RemoveBehavior<TargetTrackModel>();
                tower.GetAttackModel().RemoveBehavior<SmartTargetTrackModel>();
                aat1.projectile = projectile;
                aat2.projectile = projectile;
                aat3.projectile = projectile;
                aat4.projectile = projectile;
                aat1.GetBehavior<WeaponRateMinModel>().min *= 0.25f;
                aat2.GetBehavior<WeaponRateMinModel>().min *= 0.25f;
                aat3.GetBehavior<WeaponRateMinModel>().min *= 0.25f;
                aat4.GetBehavior<WeaponRateMinModel>().min *= 0.25f;
                aat1.Rate *= 0.25f;
                aat2.Rate *= 0.25f;
                aat3.Rate *= 0.25f;
                aat4.Rate *= 0.25f;
            }
            else
            {
                weapon.Rate *= 0.1f;
                projectile.GetDamageModel().damage = 3f;
                projectile.pierce = 30;
                explosion.GetDamageModel().damage = 0.5f;
                //explosion.pierce = 5;
                projectile.GetBehavior<AgeModel>().Lifespan = 9999999;
                projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                projectile.RemoveBehavior<FadeProjectileModel>();
                projectile.RemoveBehavior<EndOfRoundClearBypassModel>();
                tower.GetAttackModel().RemoveBehavior<FarTargetTrackModel>();
                tower.GetAttackModel().RemoveBehavior<TargetTrackModel>();
                tower.GetAttackModel().RemoveBehavior<SmartTargetTrackModel>();
                aat1.projectile = projectile;
                aat2.projectile = projectile;
                aat3.projectile = projectile;
                aat4.projectile = projectile;
                aat1.GetBehavior<WeaponRateMinModel>().min *= 0.25f;
                aat2.GetBehavior<WeaponRateMinModel>().min *= 0.25f;
                aat3.GetBehavior<WeaponRateMinModel>().min *= 0.25f;
                aat4.GetBehavior<WeaponRateMinModel>().min *= 0.25f;
                aat1.Rate *= 0.25f;
                aat2.Rate *= 0.25f;
                aat3.Rate *= 0.25f;
                aat4.Rate *= 0.25f;
            }
            explosionBehavior.projectile = explosion;
            projectile.AddBehavior(explosionBehavior);
            tower.AddBehavior(abilty);

        }
    }
    public class SpikeFactoryParagonBaseDisplay : ModTowerDisplay<SpikeFactoryParagonBase>
    {
        public override string BaseDisplay => GetDisplay(TowerType.SpikeFactory, 0, 0, 5);
        public override bool UseForTower(int[] tiers) => IsParagon(tiers);
        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
        }
    }
}