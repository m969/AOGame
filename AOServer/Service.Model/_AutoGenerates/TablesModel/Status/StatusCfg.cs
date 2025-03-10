//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Bright.Serialization;
using System.Collections.Generic;
using System.Text.Json;



namespace cfg.Status
{

public sealed partial class StatusCfg :  Bright.Config.BeanBase 
{
    public StatusCfg(JsonElement _json) 
    {
        Id = _json.GetProperty("Id").GetInt32();
        ID = _json.GetProperty("ID").GetString();
        Name = _json.GetProperty("Name").GetString();
        Type = _json.GetProperty("Type").GetString();
        StatusSlot = _json.GetProperty("StatusSlot").GetString();
        CanStack = _json.GetProperty("CanStack").GetString();
        Description = _json.GetProperty("Description").GetString();
        PostInit();
    }

    public StatusCfg(int Id, string ID, string Name, string Type, string StatusSlot, string CanStack, string Description ) 
    {
        this.Id = Id;
        this.ID = ID;
        this.Name = Name;
        this.Type = Type;
        this.StatusSlot = StatusSlot;
        this.CanStack = CanStack;
        this.Description = Description;
        PostInit();
    }

    public static StatusCfg DeserializeStatusCfg(JsonElement _json)
    {
        return new Status.StatusCfg(_json);
    }

    /// <summary>
    /// Id
    /// </summary>
    public int Id { get; private set; }
    /// <summary>
    /// 状态ID
    /// </summary>
    public string ID { get; private set; }
    /// <summary>
    /// 状态名称
    /// </summary>
    public string Name { get; private set; }
    /// <summary>
    /// 状态类型
    /// </summary>
    public string Type { get; private set; }
    /// <summary>
    /// 显示在状态栏
    /// </summary>
    public string StatusSlot { get; private set; }
    /// <summary>
    /// 能否叠加
    /// </summary>
    public string CanStack { get; private set; }
    /// <summary>
    /// 状态描述
    /// </summary>
    public string Description { get; private set; }

    public const int __ID__ = -901014506;
    public override int GetTypeId() => __ID__;

    public  void Resolve(Dictionary<string, object> _tables)
    {
        PostResolve();
    }

    public  void TranslateText(System.Func<string, string, string> translator)
    {
    }

    public override string ToString()
    {
        return "{ "
        + "Id:" + Id + ","
        + "ID:" + ID + ","
        + "Name:" + Name + ","
        + "Type:" + Type + ","
        + "StatusSlot:" + StatusSlot + ","
        + "CanStack:" + CanStack + ","
        + "Description:" + Description + ","
        + "}";
    }

    partial void PostInit();
    partial void PostResolve();
}
}
