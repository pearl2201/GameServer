using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using LeagueSandbox.GameServer.GameObjects.Stats;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Scripting.CSharp;

namespace Overdrive
{
    internal class Overdrive : IBuffGameScript
    {
        public BuffType BuffType => BuffType.HASTE;
        public BuffAddType BuffAddType => BuffAddType.REPLACE_EXISTING;
        public int MaxStacks => 5;
        public bool IsHidden => false;

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        //IParticle p;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            //p = AddParticleTarget(unit, "Overdrive_buf.troy", unit, 1);
            StatsModifier.MoveSpeed.PercentBonus = StatsModifier.MoveSpeed.PercentBonus + (12f + ownerSpell.CastInfo.SpellLevel * 4) / 100f;
            StatsModifier.AttackSpeed.PercentBonus = StatsModifier.AttackSpeed.PercentBonus + (22f + 8f * ownerSpell.CastInfo.SpellLevel) / 100f;
            unit.AddStatModifier(StatsModifier);

        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            //RemoveParticle(p);
        }

        public void OnUpdate(float diff)
        {

        }
    }
}

