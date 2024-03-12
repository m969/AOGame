namespace AO
{
    using ET;
    using System;
    using System.Net;

    public interface IAppCall<T> where T : Entity
    {
        public Type AppType();
    }
}