namespace AO
{
    using AO;
    using ET;
    using EGamePlay;
    using EGamePlay.Combat;

    public static partial class AvatarOuterRequests
    {
        public static async partial ETTask C2M_SpellCastRequest(Avatar avatar, C2M_SpellCastRequest request, M2C_SpellCastResponse response)
        {
            var combatEntity = avatar.GetComponent<UnitCombatComponent>().CombatEntity;
            var skill = combatEntity.IdSkills[1001];
            combatEntity.GetComponent<SpellComponent>().SpellWithPoint(skill, request.CastPoint);
            await ETTask.CompletedTask;
        }
    }
}