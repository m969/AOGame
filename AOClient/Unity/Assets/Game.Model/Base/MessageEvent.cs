namespace AO
{
    using ET;
    using System;

    namespace EventType
    {
        public class RequestCall
        {
            public static Action<RequestCall> CallAction;

            public IRequest Request;
            public ETTask<IResponse> Task;
            public IResponse Response;

            public async ETTask<IResponse> CallAsync(IRequest request)
            {
                Request = request;
                Task = ETTask<IResponse>.Create();
                CallAction.Invoke(this);
                await Task;
                return Response;
            }
        }
    }
}