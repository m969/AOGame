public class AOTGenericReferences : UnityEngine.MonoBehaviour
{

	// {{ constraint implement type
	// }} 

	// {{ AOT generic type
	//ET.AddComponentSystem`1<System.Object>
	//ET.AEvent`1<ET.EventType.EntryEvent1>
	//ET.AEvent`1<AO.EventType.CreateMyUnit>
	//ET.AEvent`1<ET.EventType.EntryEvent2>
	//ET.AEvent`1<ET.Client.NetClientComponentOnRead>
	//ET.ATimer`1<System.Object>
	//ET.AwakeSystem`1<System.Object>
	//ET.AwakeSystem`2<System.Object,System.Int32>
	//ET.AwakeSystem`2<System.Object,System.Net.Sockets.AddressFamily>
	//ET.DestroySystem`1<System.Object>
	//ET.ETAsyncTaskMethodBuilder`1<System.Object>
	//ET.ETTask`1<System.Object>
	//ET.IAwake`1<System.Int32>
	//ET.IAwake`1<System.Net.Sockets.AddressFamily>
	//ET.IAwake`1<System.Object>
	//ET.LateUpdateSystem`1<System.Object>
	//ET.LoadSystem`1<System.Object>
	//ET.Singleton`1<System.Object>
	//ET.UnOrderMultiMap`2<System.Object,System.Object>
	//ET.UpdateSystem`1<System.Object>
	//System.Action`1<System.Object>
	//System.Action`2<System.Int64,System.Int32>
	//System.Action`3<System.Int64,System.Int64,System.Object>
	//System.Collections.Generic.Dictionary`2<System.UInt16,System.Object>
	//System.Collections.Generic.Dictionary`2<System.Int32,ET.RpcInfo>
	//System.Collections.Generic.Dictionary`2<System.Object,System.Object>
	//System.Collections.Generic.Dictionary`2<System.Int64,System.Object>
	//System.Collections.Generic.HashSet`1<System.Object>
	//System.Collections.Generic.HashSet`1<System.UInt16>
	//System.Collections.Generic.HashSet`1/Enumerator<System.Object>
	//System.Collections.Generic.List`1<System.Object>
	//System.Collections.Generic.List`1/Enumerator<System.Object>
	//System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<System.Object>
	//System.Runtime.CompilerServices.TaskAwaiter`1<System.Object>
	//System.Threading.Tasks.Task`1<System.Object>
	// }}

	public void RefMethods()
	{
		// System.Object ET.Entity::AddChildWithId<System.Object>(System.Int64,System.Boolean)
		// System.Object ET.Entity::AddChildWithId<System.Object,System.Int32>(System.Int64,System.Int32,System.Boolean)
		// System.Object ET.Entity::AddComponent<System.Object>(System.Boolean)
		// System.Object ET.Entity::AddComponent<System.Object,System.Net.Sockets.AddressFamily>(System.Net.Sockets.AddressFamily,System.Boolean)
		// System.Object ET.Entity::GetChild<System.Object>(System.Int64)
		// System.Object ET.Entity::GetComponent<System.Object>()
		// System.Object ET.Entity::GetOrAdd<System.Object>()
		// System.Object ET.Entity::GetParent<System.Object>()
		// System.Void ET.Entity::RemoveComponent<System.Object>()
		// System.Void ET.ETAsyncTaskMethodBuilder::AwaitUnsafeOnCompleted<System.Object,System.Object>(System.Object&,System.Object&)
		// System.Void ET.ETAsyncTaskMethodBuilder::AwaitUnsafeOnCompleted<ET.ETTaskCompleted,System.Object>(ET.ETTaskCompleted&,System.Object&)
		// System.Void ET.ETAsyncTaskMethodBuilder::Start<System.Object>(System.Object&)
		// System.Void ET.ETAsyncTaskMethodBuilder`1<System.Object>::AwaitUnsafeOnCompleted<System.Object,System.Object>(System.Object&,System.Object&)
		// System.Void ET.ETAsyncTaskMethodBuilder`1<System.Object>::Start<System.Object>(System.Object&)
		// System.Void ET.EventSystem::Publish<AO.EventType.CreateMyUnit>(ET.Scene,AO.EventType.CreateMyUnit)
		// System.Void ET.EventSystem::Publish<ET.Client.NetClientComponentOnRead>(ET.Scene,ET.Client.NetClientComponentOnRead)
		// System.Void ET.EventSystem::Publish<System.Int32>(ET.Scene,System.Int32)
		// ET.ETTask ET.EventSystem::PublishAsync<ET.EventType.EntryEvent1>(ET.Scene,ET.EventType.EntryEvent1)
		// ET.ETTask ET.EventSystem::PublishAsync<ET.EventType.EntryEvent2>(ET.Scene,ET.EventType.EntryEvent2)
		// ET.ETTask ET.EventSystem::PublishAsync<ET.EventType.EntryEvent3>(ET.Scene,ET.EventType.EntryEvent3)
		// System.Object ET.Game::AddSingleton<System.Object>()
		// System.Object Puerts.JsEnv::ExecuteModule<System.Object>(System.String,System.String)
		// ET.RpcInfo[] System.Linq.Enumerable::ToArray<ET.RpcInfo>(System.Collections.Generic.IEnumerable`1<ET.RpcInfo>)
		// System.Object[] System.Linq.Enumerable::ToArray<System.Object>(System.Collections.Generic.IEnumerable`1<System.Object>)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder::AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter`1<System.Object>,System.Object>(System.Runtime.CompilerServices.TaskAwaiter`1<System.Object>&,System.Object&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder::AwaitUnsafeOnCompleted<System.Object,System.Object>(System.Object&,System.Object&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder::Start<System.Object>(System.Object&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<System.Object>::AwaitUnsafeOnCompleted<System.Object,System.Object>(System.Object&,System.Object&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<System.Object>::Start<System.Object>(System.Object&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder::AwaitUnsafeOnCompleted<System.Object,System.Object>(System.Object&,System.Object&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder::Start<System.Object>(System.Object&)
		// System.Object UnityEngine.Object::Instantiate<System.Object>(System.Object)
	}
}