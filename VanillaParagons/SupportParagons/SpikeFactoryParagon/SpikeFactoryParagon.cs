using Assets.Scripts.Models.Towers;
using Assets.Scripts.Models.Towers.Behaviors.Abilities;
using Assets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Assets.Scripts.Models.Towers.Projectiles.Behaviors;
using BTD_Mod_Helper.Api.Towers;
using BTD_Mod_Helper.Extensions;

namespace VanillaParagons.SupportParagons.SpikeFactoryParagon
{
    public class SpikeFactoryParagonBase : ModVanillaParagon
    {
        public override string BaseTower => "SpikeFactory-250";
    }
    public class SpikeFactoryParagon : ModParagonUpgrade<SpikeFactoryParagonBase>
    {
        public override string DisplayName => "Perfect Spike";
        public override int Cost => 2000000;
        public override string Description => "Worse than stepping on a lego.";
        //public override string Icon => "";
        //public override string Portrait => "";
        public override void ApplyUpgrade(TowerModel tower)
        {
        }
    }
}