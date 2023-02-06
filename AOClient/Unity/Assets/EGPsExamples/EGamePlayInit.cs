using EGamePlay;
using EGamePlay.Combat;
using ET;
using System.Threading;
using Sirenix.Serialization;
using Sirenix.OdinInspector;
using Entity = EGamePlay.Entity;

#if UNITY
public class EGamePlayInit : SerializedMonoBehaviour
{
    public static EGamePlayInit Instance { get; private set; }
    //public ReferenceCollector ConfigsCollector;
    public bool EntityLog;


    private void Awake()
    {
        Instance = this;
        //SynchronizationContext.SetSynchronizationContext(ThreadSynchronizationContext.Instance);
        Entity.EnableLog = EntityLog;
        MasterEntity.Create();
        Entity.Create<CombatContext>();
        //Entity.Create<TimerManager>();
        //MasterEntity.Instance.AddComponent<ConfigManageComponent>(ConfigsCollector);
    }

    private void Update()
    {
        MasterEntity.Instance.Update();
        //ThreadSynchronizationContext.Instance.Update();
        //TimerManager.Instance.Update();
    }

    private void OnApplicationQuit()
    {
        MasterEntity.Destroy();
    }
}
#endif