using System;

namespace ET
{
	public interface IEvent
	{
		Type Type { get; }
	}
	
	public abstract class AEvent<A>: IEvent where A: struct
	{
		public Type Type
		{
			get
			{
				return typeof (A);
			}
		}

		protected abstract ETTask Run(Entity source, A a);

		public async ETTask Handle(Entity source, A a)
		{
			try
			{
				await Run(source, a);
			}
			catch (Exception e)
			{
				Log.Error(e);
			}
		}
	}
}