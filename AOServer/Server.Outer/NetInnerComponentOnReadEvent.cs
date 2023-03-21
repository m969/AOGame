using System;

namespace ET.Server
{
    [Event(SceneType.Process)]
    public class NetInnerComponentOnReadEvent : AEvent<NetInnerComponentOnRead>
    {
        [EnableAccessEntiyChild]
        protected override async ETTask Run(Entity source, NetInnerComponentOnRead args)
        {
            try
            {
                long actorId = args.ActorId;
                object message = args.Message;
                ushort opcode = NetServices.Instance.GetOpcode(message.GetType());
                if (!(message is ObjectAddRequest || message is ObjectRemoveRequest || message is ObjectGetRequest) && OpcodeHelper.IsNeedLogMessage(opcode))
                {
                    Log.Console($"{args.Message.GetType().Name} {args.Message}");
                }

                // 收到actor消息,放入actor队列
                switch (message)
                {
                    case IActorResponse iActorResponse:
                        {
                            ActorHandleHelper.HandleIActorResponse(iActorResponse);
                            break;
                        }
                    case IActorRequest iActorRequest:
                        {
                            await ActorHandleHelper.HandleIActorRequest(actorId, iActorRequest);
                            break;
                        }
                    case IActorMessage iActorMessage:
                        {
                            await ActorHandleHelper.HandleIActorMessage(actorId, iActorMessage);
                            break;
                        }
                }
            }
            catch (Exception e)
            {
                Log.Error($"InnerMessageDispatcher error: {args.Message.GetType().Name}\n{e}");
            }
        }
    }
}