namespace AO
{
    using ET;

    /// <summary>
    /// 技能编辑器模式
    /// </summary>
    public class ExecutionEditorModeComponent : Entity, IAwake, IClientMode
    {
        public Actor BossUnit { get; set; }
    }
}