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



namespace cfg.Unit
{

public sealed partial class UnitCfg :  Bright.Config.BeanBase 
{
    public UnitCfg(JsonElement _json) 
    {
        Id = _json.GetProperty("Id").GetInt32();
        Type = _json.GetProperty("Type").GetInt32();
        Name = _json.GetProperty("Name").GetString();
        Desc = _json.GetProperty("Desc").GetString();
        Position = _json.GetProperty("Position").GetInt32();
        Height = _json.GetProperty("Height").GetInt32();
        Weight = _json.GetProperty("Weight").GetInt32();
        PostInit();
    }

    public UnitCfg(int Id, int Type, string Name, string Desc, int Position, int Height, int Weight ) 
    {
        this.Id = Id;
        this.Type = Type;
        this.Name = Name;
        this.Desc = Desc;
        this.Position = Position;
        this.Height = Height;
        this.Weight = Weight;
        PostInit();
    }

    public static UnitCfg DeserializeUnitCfg(JsonElement _json)
    {
        return new Unit.UnitCfg(_json);
    }

    /// <summary>
    /// Id
    /// </summary>
    public int Id { get; private set; }
    /// <summary>
    /// Type
    /// </summary>
    public int Type { get; private set; }
    /// <summary>
    /// 名字
    /// </summary>
    public string Name { get; private set; }
    /// <summary>
    /// 描述
    /// </summary>
    public string Desc { get; private set; }
    /// <summary>
    /// 位置
    /// </summary>
    public int Position { get; private set; }
    /// <summary>
    /// 身高
    /// </summary>
    public int Height { get; private set; }
    /// <summary>
    /// 体重
    /// </summary>
    public int Weight { get; private set; }

    public const int __ID__ = -436428330;
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
        + "Type:" + Type + ","
        + "Name:" + Name + ","
        + "Desc:" + Desc + ","
        + "Position:" + Position + ","
        + "Height:" + Height + ","
        + "Weight:" + Weight + ","
        + "}";
    }

    partial void PostInit();
    partial void PostResolve();
}
}
