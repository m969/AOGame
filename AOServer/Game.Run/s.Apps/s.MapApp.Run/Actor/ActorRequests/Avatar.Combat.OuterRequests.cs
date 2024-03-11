namespace AO
{
    using AO;
    using ET;
    using EGamePlay;
    using EGamePlay.Combat;

    public static partial class ActorOuterRequests
    {
        public static async partial ETTask C2M_SpellRequest(Actor avatar, C2M_SpellRequest request, M2C_SpellResponse response)
        {
            var combatEntity = avatar.GetComponent<UnitCombatComponent>().CombatEntity;
            if (combatEntity.IdSkills.TryGetValue(request.SkillId, out var skillAbility))
            {
                if (skillAbility.SkillConfig.Id == 1002)
                {
                    combatEntity.GetComponent<SpellComponent>().SpellWithPoint(skillAbility, request.CastPoint);
                }
                else
                {
                    combatEntity.GetComponent<SpellComponent>().SpellWithTarget(skillAbility, skillAbility.OwnerEntity);
                }
            }
            await ETTask.CompletedTask;
        }
    }
}