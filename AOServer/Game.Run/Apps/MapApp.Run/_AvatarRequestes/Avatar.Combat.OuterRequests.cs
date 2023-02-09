namespace AO
{
    using AO;
    using ET;
    using EGamePlay;
    using EGamePlay.Combat;

    public static partial class AvatarOuterRequests
    {
        public static async partial ETTask C2M_SpellRequest(Avatar avatar, C2M_SpellRequest request, M2C_SpellResponse response)
        {
            var combatEntity = avatar.GetComponent<UnitCombatComponent>().CombatEntity;
            var skill = combatEntity.IdSkills[1001];
            combatEntity.GetComponent<SpellComponent>().SpellWithPoint(skill, request.CastPoint);
            await ETTask.CompletedTask;
        }
    }
}