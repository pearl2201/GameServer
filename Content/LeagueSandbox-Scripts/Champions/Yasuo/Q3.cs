using System.Numerics;
using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;

namespace Spells
{
    public class YasuoQ3W : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };

        private Vector2 trueCoords;
        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            //empty
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
            //empty
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
        }

        public void OnSpellCast(ISpell spell)
        {
            var current = new Vector2(spell.CastInfo.Owner.Position.X, spell.CastInfo.Owner.Position.Y);
            var spellPos = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            var to = Vector2.Normalize(spellPos - current);
            var range = to * spell.SpellData.CastRangeDisplayOverride;
            trueCoords = current + range;

            FaceDirection(trueCoords, spell.CastInfo.Owner, true);
        }

        public void OnSpellPostCast(ISpell spell)
        {
            if (HasBuff(spell.CastInfo.Owner, "YasuoE"))
            {
                //spell.CastInfo.Owner.SpellAnimation("SPELL3b");
                AddParticleTarget(spell.CastInfo.Owner, "Yasuo_Base_EQ3_cas.troy", spell.CastInfo.Owner);
                AddParticleTarget(spell.CastInfo.Owner, "Yasuo_Base_EQ_SwordGlow.troy", spell.CastInfo.Owner, bone: "C_BUFFBONE_GLB_Weapon_1");
                foreach (var affectEnemys in GetUnitsInRange(spell.CastInfo.Owner.Position, 270f, true))
                {
                    if (affectEnemys is IAttackableUnit && affectEnemys.Team != spell.CastInfo.Owner.Team)
                    {
                        affectEnemys.TakeDamage(spell.CastInfo.Owner, spell.CastInfo.SpellLevel * 20f + spell.CastInfo.Owner.Stats.AttackDamage.Total, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                        AddParticleTarget(spell.CastInfo.Owner, "Yasuo_Base_Q_WindStrike.troy", affectEnemys);
                        AddParticleTarget(spell.CastInfo.Owner, "Yasuo_Base_Q_windstrike_02.troy", affectEnemys);
                        AddParticleTarget(spell.CastInfo.Owner, "Yasuo_Base_Q_hit_tar.troy", affectEnemys);
                        ForceMovement(affectEnemys, "RUN", new Vector2(affectEnemys.Position.X + 10f, affectEnemys.Position.Y + 10f), 13f, 0, 16.5f, 0, movementOrdersFacing: ForceMovementOrdersFacing.KEEP_CURRENT_FACING);
                    }
                }
            }
            else
            {
                //spell.AddProjectile("YasuoQ3Mis", spell.CastInfo.Owner.Position, spell.CastInfo.Owner.Position, trueCoords);
                //spell.CastInfo.Owner.SpellAnimation("SPELL1C");
                (spell.CastInfo.Owner as IChampion).SetSpell("YasuoQW", 0, true);
                AddParticleTarget(spell.CastInfo.Owner, "Yasuo_Base_Q3_Hand.troy", spell.CastInfo.Owner);
                AddParticleTarget(spell.CastInfo.Owner, "Yasuo_Base_Q3_cast_sound.troy", spell.CastInfo.Owner);
            }
            if (HasBuff(spell.CastInfo.Owner, "YasuoQ02"))
            {
                RemoveBuff(spell.CastInfo.Owner, "YasuoQ02");
            }
        }

        public void ApplyEffects(IObjAiBase owner, IAttackableUnit target, ISpell spell, ISpellMissile missile)
        {
            AddParticleTarget(owner, "Yasuo_Base_Q_WindStrike.troy", target);
            AddParticleTarget(owner, "Yasuo_Base_Q_windstrike_02.troy", target);
            AddParticleTarget(owner, "Yasuo_Base_Q_hit_tar.troy", target);
            target.TakeDamage(owner, spell.CastInfo.SpellLevel * 20f + owner.Stats.AttackDamage.Total,DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
            ForceMovement(target, "RUN", new Vector2(target.Position.X + 10f, target.Position.Y + 10f), 13f, 0, 16.5f, 0);
        }

        public void OnSpellChannel(ISpell spell)
        {
        }

        public void OnSpellChannelCancel(ISpell spell)
        {
        }

        public void OnSpellPostChannel(ISpell spell)
        {
        }

        public void OnUpdate(float diff)
        {
            //empty
        }
    }
}
