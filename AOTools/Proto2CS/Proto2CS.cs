using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ET
{
    internal class OpcodeInfo
    {
        public string Name;
        public int Opcode;
    }

    class Log 
    {
        public static void Console(string log) 
        { 
            System.Console.WriteLine(log);
        }
    }

    public static class Proto2CS
    {
        public static void Export()
        {
            // InnerMessage.proto生成cs代码
            InnerProto2CS.Proto2CS();
            Log.Console("proto2cs succeed!");
        }
    }

    public static class InnerProto2CS
    {
        private const string protoDir = "../../../Proto";// Unity/Assets/Config/
        private const string clientMessagePath = "../../../AOClient/Unity/Assets/Game.Model/_AutoGenerates/ProtoMessages/";
        private const string clientEntityCallPath = "../../../AOClient/Unity/Assets/Game.Model/_AutoGenerates/EntityCallBase/";
        private const string clientServerCallPath = "../../../AOClient/Unity/Assets/Game.Model/_AutoGenerates/EntityCallBase/";
        private const string clientReceiveMessagesPath = "../../../AOClient/Unity/Assets/Game.Run/_AutoGenerates/";

        private const string serverMessagePath = "../../../AOServer/Game.Model/_AutoGenerates/ProtoMessages/";
        private const string serverEntityCallPath = "../../../AOServer/Game.Model/_AutoGenerates/EntityCallBase/";
        private const string serverEntityRequestsPath = "../../../AOServer/Game.Run/_AutoGenerates/EntityRequestsBase/";
        //private const string clientServerMessagePath = "../Unity/Assets/Scripts/Codes/Model/Generate/ClientServer/Message/";
        private static readonly char[] splitChars = { ' ', '\t' };
        private static readonly List<OpcodeInfo> msgOpcode = new List<OpcodeInfo>();

        public static void Proto2CS()
        {
            msgOpcode.Clear();

            if (Directory.Exists(clientMessagePath))
            {
                Directory.Delete(clientMessagePath, true);
            }

            if (Directory.Exists(serverMessagePath))
            {
                Directory.Delete(serverMessagePath, true);
            }
            
            //if (Directory.Exists(clientServerMessagePath))
            //{
            //    Directory.Delete(clientServerMessagePath, true);
            //}

            List<string> list = FileHelper.GetAllFiles(protoDir, "*proto");
            foreach (string s in list)
            {
                if (!s.EndsWith(".proto"))
                {
                    continue;
                }
                string fileName = Path.GetFileNameWithoutExtension(s);
                if (fileName.Contains("."))
                {
                    continue;
                }
                string[] ss2 = fileName.Split('_');
                string protoName = ss2[0];
                string cs = fileName.Contains("OuterMessage") ? "C" : "S";
                int startOpcode = int.Parse(ss2[1]);
                ProtoFile2CS(fileName, protoName, cs, startOpcode);
            }
        }

        public static void ProtoFile2EntityCallCS(string fileName, string fileText)
        {
            string s = fileText;

            StringBuilder sb = new StringBuilder();

            string template = @"namespace AO
{
    using ET;
    using ET.Server;
    using ActorSendEvent = AO.EventType.ActorSendEvent;

    public class {EntityCall}AwakeSystem: AwakeSystem<{EntityCall}, long>
    {
        protected override void Awake({EntityCall} self, long sessionId)
        {
            self.Parent.AddComponent<GateSessionIdComponent, long>(sessionId);
            self.Parent.AddComponent<MailBoxComponent>();
            self.Client = new {EntityCall}.ClientCall();
            self.Client.SessionId = sessionId;
        }
    }

    public class {EntityCall} : Entity, IAwake<long>
    {
        public ClientCall Client { get; set; }

        public class ClientCall
        {
            public long SessionId { get; set; }
{ClientCalls}
        }
    }
}";
            var lastComment = string.Empty;
            foreach (string line in s.Split('\n'))
            {
                string newline = line.Trim();

                if (newline.StartsWith("//"))
                {
                    //sb.Append($"{newline}\n");
                    lastComment += $"{newline}\n";
                    continue;
                }

                if (newline.StartsWith("message"))
                {
                    string parentClass = "";
                    string msgName = newline.Split(splitChars, StringSplitOptions.RemoveEmptyEntries)[1];
                    string[] ss = newline.Split(new[] { "//" }, StringSplitOptions.RemoveEmptyEntries);

                    if (ss.Length == 2)
                    {
                        parentClass = ss[1].Trim();
                    }

                    if (parentClass == "IActorMessage")
                    {
                        if (!string.IsNullOrEmpty(lastComment))
                        {
                            sb.Append($"{lastComment}");
                            lastComment = string.Empty;
                        }
                        sb.AppendLine($"\t\t\tpublic void {msgName}({msgName} msg) => AOGame.Publish(new ActorSendEvent() {{ ActorId = SessionId, Message = msg }});");
                    }

                    continue;
                }
            }

            var className = $"{Path.GetFileNameWithoutExtension(fileName)}Call";
            template = template.Replace("{EntityCall}", className);
            template = template.Replace("{ClientCalls}", sb.ToString());

            GenerateCS(template, serverEntityCallPath, className + ".cs");
        }

        public static void ProtoFile2ClientReceiveMessages(string fileName, string fileText)
        {
            string s = fileText;

            StringBuilder sb = new StringBuilder();

            string template = @"namespace AO
{
    using ET;

    public static partial class ClientReceiveMessages
    {
{MessageHandler}
    }
}";
            string handlerTemplate = @"        [MessageHandler(SceneType.Client)]
        public class {Message}Handler : AMHandler<{Message}>
        {
            protected override async ETTask Run({Message} message)
            {
                {Message}(message).Coroutine();
                await ETTask.CompletedTask;
            }
        }
        /// <summary>
        /// {Comment}
        /// </summary>
        public static partial ETTask {Message}({Message} message);";
            var lastComment = string.Empty;
            foreach (string line in s.Split('\n'))
            {
                string newline = line.Trim();

                if (newline.StartsWith("//ResponseType"))
                {
                    continue;
                }

                if (newline.StartsWith("//"))
                {
                    lastComment += $"{newline}\n";
                    continue;
                }

                if (newline.StartsWith("message"))
                {
                    string parentClass = "";
                    string msgName = newline.Split(splitChars, StringSplitOptions.RemoveEmptyEntries)[1];
                    string[] ss = newline.Split(new[] { "//" }, StringSplitOptions.RemoveEmptyEntries);

                    if (ss.Length == 2)
                    {
                        parentClass = ss[1].Trim();
                    }

                    if (parentClass.Contains(','))
                    {
                        parentClass = parentClass.Split(',')[0];
                    }

                    if (parentClass == "IActorMessage")
                    {
                        var handlerStr = handlerTemplate.Replace("{Message}", msgName);
                        handlerStr = handlerStr.Replace("{Comment}", lastComment);
                        lastComment = string.Empty;
                        sb.Append(handlerStr);
                        sb.Append("\n");
                        sb.Append("\n");
                    }

                    continue;
                }
            }

            template = template.Replace("{MessageHandler}", sb.ToString());

            GenerateCS(template, clientReceiveMessagesPath, "ClientReceiveMessages.cs");
        }

        public static void ProtoFile2EntityCallCS_Client(string fileName, string fileText)
        {
            string s = fileText;

            StringBuilder sb = new StringBuilder();
            StringBuilder sbTs = new StringBuilder();

            string template = @"namespace AO
{
    using ET;
    using System.Threading.Tasks;

    public static class {ClassName}
    {
{Requests}
    }

    public static class {ClassName}Ts
    {
{RequestsTs}
    }
}";
            string requestTemplate = @"        public static async ETTask<{Response}> {Request}({Request} request)
        {
            return await new EventType.RequestCall().CallAsync(request) as {Response};
        }";
            string locationTemplate = @"        public static void {Request}({Request} request) => EventType.RequestCall.SendAction(request);";
            string requestTemplateTs = @"        public static async Task<{Response}> {Request}({Request} request)
        {
            return await new EventType.RequestCall().CallAsync(request) as {Response};
        }";

            var lastComment = string.Empty;
            var lastResponse = string.Empty;
            var entityName = $"{Path.GetFileNameWithoutExtension(fileName)}";
            var className = $"{entityName}Call";
            foreach (string line in s.Split('\n'))
            {
                string newline = line.Trim();

                if (newline.StartsWith("//ResponseType"))
                {
                    string responseType = line.Split(" ")[1].TrimEnd('\r', '\n');
                    lastResponse = responseType;
                    continue;
                }

                if (newline.StartsWith("//"))
                {
                    lastComment += $"{newline}\n";
                    continue;
                }

                if (newline.StartsWith("message"))
                {
                    string parentClass = "";
                    string msgName = newline.Split(splitChars, StringSplitOptions.RemoveEmptyEntries)[1];
                    string[] ss = newline.Split(new[] { "//" }, StringSplitOptions.RemoveEmptyEntries);

                    if (ss.Length == 2)
                    {
                        parentClass = ss[1].Trim();
                    }

                    if (parentClass.Contains(','))
                    {
                        parentClass = parentClass.Split(',')[0];
                    }

                    if (parentClass == "IActorLocationRequest")
                    {
                        if (!string.IsNullOrEmpty(lastComment))
                        {
                            sb.Append($"{lastComment}");
                            sbTs.Append($"{lastComment}");
                            lastComment = string.Empty;
                        }
                        var handlerStr = requestTemplate.Replace("{Request}", msgName);
                        handlerStr = handlerStr.Replace("{Response}", lastResponse);
                        sb.Append(handlerStr);
                        sb.Append("\n");
                        sb.Append("\n");
                        var handlerStrTs = requestTemplateTs.Replace("{Request}", msgName);
                        handlerStrTs = handlerStrTs.Replace("{Response}", lastResponse);
                        sbTs.Append(handlerStrTs);
                        sbTs.Append("\n");
                        sbTs.Append("\n");
                    }
                    if (parentClass == "IActorLocationMessage")
                    {
                        if (!string.IsNullOrEmpty(lastComment))
                        {
                            sb.Append($"{lastComment}");
                            lastComment = string.Empty;
                        }
                        var handlerStr = locationTemplate.Replace("{Request}", msgName);
                        sb.Append(handlerStr);
                        sb.Append("\n");
                        sb.Append("\n");
                    }
                    continue;
                }
            }

            template = template.Replace("{ClassName}", className);
            template = template.Replace("{Requests}", sb.ToString());
            template = template.Replace("{RequestsTs}", sbTs.ToString());

            GenerateCS(template, clientEntityCallPath, $"{className}.cs");
        }

        public static void ProtoFile2ServerCallCS_Client(string fileName, string fileText)
        {
            string s = fileText;

            StringBuilder sb = new StringBuilder();
            StringBuilder sbTs = new StringBuilder();

            string template = @"namespace AO
{
    using ET;
    using System.Threading.Tasks;

    public static class {ClassName}
    {
{Requests}
    }

    public static class {ClassName}Ts
    {
{RequestsTs}
    }
}";
            string requestTemplate = @"        public static async ETTask<{Response}> {Request}({Request} request)
        {
            var msg = new EventType.RequestCall();
            await msg.CallAsync(request);
            return msg.Response as {Response};
        }";
            string requestTemplateTs = @"        public static async Task<{Response}> {Request}({Request} request)
        {
            var msg = new EventType.RequestCall();
            await msg.CallAsync(request);
            return msg.Response as {Response};
        }";

            var lastComment = string.Empty;
            var lastResponse = string.Empty;
            var entityName = $"{Path.GetFileNameWithoutExtension(fileName)}";
            var className = $"ServerCall";
            foreach (string line in s.Split('\n'))
            {
                string newline = line.Trim();

                if (newline.StartsWith("//ResponseType"))
                {
                    string responseType = line.Split(" ")[1].TrimEnd('\r', '\n');
                    lastResponse = responseType;
                    continue;
                }

                if (newline.StartsWith("//"))
                {
                    lastComment += $"{newline}\n";
                    continue;
                }

                if (newline.StartsWith("message"))
                {
                    string parentClass = "";
                    string msgName = newline.Split(splitChars, StringSplitOptions.RemoveEmptyEntries)[1];
                    string[] ss = newline.Split(new[] { "//" }, StringSplitOptions.RemoveEmptyEntries);

                    if (ss.Length == 2)
                    {
                        parentClass = ss[1].Trim();
                    }

                    if (parentClass.Contains(','))
                    {
                        parentClass = parentClass.Split(',')[0];
                    }

                    if (parentClass == "IRequest")
                    {
                        if (!string.IsNullOrEmpty(lastComment))
                        {
                            sb.Append($"{lastComment}");
                            sbTs.Append($"{lastComment}");
                            lastComment = string.Empty;
                        }
                        var handlerStr = requestTemplate.Replace("{Request}", msgName);
                        handlerStr = handlerStr.Replace("{Response}", lastResponse);
                        sb.Append(handlerStr);
                        sb.Append("\n");
                        sb.Append("\n");
                        var handlerStrTs = requestTemplateTs.Replace("{Request}", msgName);
                        handlerStrTs = handlerStrTs.Replace("{Response}", lastResponse);
                        sbTs.Append(handlerStrTs);
                        sbTs.Append("\n");
                        sbTs.Append("\n");
                    }

                    continue;
                }
            }

            template = template.Replace("{ClassName}", "ServerCall");
            template = template.Replace("{Requests}", sb.ToString());
            template = template.Replace("{RequestsTs}", sbTs.ToString());

            GenerateCS(template, clientServerCallPath, $"ServerCall.cs");
        }

        public static void ProtoFile2EntityOuterRequests(string fileName, string fileText)
        {
            string s = fileText;

            StringBuilder sb = new StringBuilder();

            string template = @"namespace AO
{
    using AO;
    using ET;
    using ET.Server;

    public static partial class {ClassName}
    {
{EntiyOuterRequests}
    }
}";
            string handlerTemplate = @"        [ActorMessageHandler(SceneType.Gate)]
        public class {Request}Handler : AMActorLocationRpcHandler<{EntityName}, {Request}, {Response}>
        {
            protected override async ETTask Run({EntityName} entity, {Request} request, {Response} response)
            {
                await {Request}(entity, request, response);
            }
        }

        public static partial ETTask {Request}({EntityName} {EntityNameLower}, {Request} request, {Response} response);";

            string locationTemplate = @"        [ActorMessageHandler(SceneType.Gate)]
        public class {Request}Handler : AMActorLocationHandler<{EntityName}, {Request}>
        {
            protected override async ETTask Run({EntityName} entity, {Request} request)
            {
                await {Request}(entity, request);
            }
        }

        public static partial ETTask {Request}({EntityName} {EntityNameLower}, {Request} request);";
            var lastComment = string.Empty;
            var lastResponse = string.Empty;
            var entityName = $"{Path.GetFileNameWithoutExtension(fileName)}";
            var className = $"{entityName}OuterRequests";
            foreach (string line in s.Split('\n'))
            {
                string newline = line.Trim();

                if (newline.StartsWith("//ResponseType"))
                {
                    string responseType = line.Split(" ")[1].TrimEnd('\r', '\n');
                    lastResponse = responseType;
                    continue;
                }

                if (newline.StartsWith("//"))
                {
                    lastComment += $"{newline}\n";
                    continue;
                }

                if (newline.StartsWith("message"))
                {
                    string parentClass = "";
                    string msgName = newline.Split(splitChars, StringSplitOptions.RemoveEmptyEntries)[1];
                    string[] ss = newline.Split(new[] { "//" }, StringSplitOptions.RemoveEmptyEntries);

                    if (ss.Length == 2)
                    {
                        parentClass = ss[1].Trim();
                    }

                    if (parentClass.Contains(','))
                    {
                        parentClass = parentClass.Split(',')[0];
                    }

                    if (parentClass == "IActorLocationRequest")
                    {
                        var handlerStr = handlerTemplate.Replace("{Request}", msgName);
                        handlerStr = handlerStr.Replace("{Response}", lastResponse);
                        handlerStr = handlerStr.Replace("{EntityName}", entityName);
                        handlerStr = handlerStr.Replace("{EntityNameLower}", entityName.ToLower());
                        sb.Append(handlerStr);
                        sb.Append("\n");
                        if (!string.IsNullOrEmpty(lastComment))
                        {
                            sb.Append($"{lastComment}");
                            lastComment = string.Empty;
                        }
                        sb.Append("\n");
                        sb.Append("\n");
                    }
                    if (parentClass == "IActorLocationMessage")
                    {
                        var handlerStr = locationTemplate.Replace("{Request}", msgName);
                        handlerStr = handlerStr.Replace("{EntityName}", entityName);
                        handlerStr = handlerStr.Replace("{EntityNameLower}", entityName.ToLower());
                        sb.Append(handlerStr);
                        sb.Append("\n");
                        if (!string.IsNullOrEmpty(lastComment))
                        {
                            sb.Append($"{lastComment}");
                            lastComment = string.Empty;
                        }
                        sb.Append("\n");
                        sb.Append("\n");
                    }
                    continue;
                }
            }

            template = template.Replace("{ClassName}", className);
            template = template.Replace("{EntiyOuterRequests}", sb.ToString());

            GenerateCS(template, serverEntityRequestsPath, Path.GetFileNameWithoutExtension(fileName) + ".OuterRequestsBase.cs");
        }

        public static void ProtoFile2CS(string fileName, string protoName, string cs, int startOpcode)
        {
            string ns = "ET";
            msgOpcode.Clear();
            string proto = Path.Combine(protoDir, $"{fileName}.proto");
            
            string s = File.ReadAllText(proto);

            var ext = "OuterMessage";
            if (cs == "S") ext = "InnerMessage";
            var list = FileHelper.GetAllFiles(protoDir, "*proto");
            foreach (var s2 in list)
            {
                if (!s2.EndsWith(".proto"))
                {
                    continue;
                }
                string fileName2 = Path.GetFileNameWithoutExtension(s2);
                if (fileName2.Contains(".") && fileName2.EndsWith(ext))
                {
                    //string[] ss2 = fileName2.Split('.');
                    var s3 = File.ReadAllText(Path.Combine(protoDir, $"{fileName2}.proto"));
                    ProtoFile2EntityCallCS(fileName2, s3);
                    ProtoFile2EntityCallCS_Client(fileName2, s3);
                    ProtoFile2EntityOuterRequests(fileName2, s3);
                    s += "\n";
                    s += s3;
                }
            }
            ProtoFile2ClientReceiveMessages(fileName, s);
            ProtoFile2ServerCallCS_Client(fileName, s);

            StringBuilder sb = new StringBuilder();
            sb.Append("using ET;\n");
            sb.Append("using ProtoBuf;\n");
            sb.Append("using System.Collections.Generic;\n");
            sb.Append($"namespace {ns}\n");
            sb.Append("{\n");
            
            bool isMsgStart = false;
            foreach (string line in s.Split('\n'))
            {
                string newline = line.Trim();

                if (newline == "")
                {
                    continue;
                }

                if (newline.StartsWith("//ResponseType"))
                {
                    string responseType = line.Split(" ")[1].TrimEnd('\r', '\n');
                    sb.Append($"\t[ResponseType(nameof({responseType}))]\n");
                    continue;
                }

                if (newline.StartsWith("//"))
                {
                    sb.Append($"{newline}\n");
                    continue;
                }

                if (newline.StartsWith("message"))
                {
                    string parentClass = "";
                    isMsgStart = true;
                    string msgName = newline.Split(splitChars, StringSplitOptions.RemoveEmptyEntries)[1];
                    string[] ss = newline.Split(new[] { "//" }, StringSplitOptions.RemoveEmptyEntries);

                    if (ss.Length == 2)
                    {
                        parentClass = ss[1].Trim();
                    }

                    msgOpcode.Add(new OpcodeInfo() { Name = msgName, Opcode = ++startOpcode });

                    sb.Append($"\t[Message({protoName}.{msgName})]\n");
                    sb.Append($"\t[ProtoContract]\n");
                    sb.Append($"\tpublic partial class {msgName}: ProtoObject");
                    if (parentClass == "IActorMessage" || parentClass == "IActorRequest" || parentClass == "IActorResponse")
                    {
                        sb.Append($", {parentClass}\n");
                    }
                    else if (parentClass != "")
                    {
                        sb.Append($", {parentClass}\n");
                    }
                    else
                    {
                        sb.Append("\n");
                    }

                    continue;
                }

                if (isMsgStart)
                {
                    if (newline == "{")
                    {
                        sb.Append("\t{\n");
                        continue;
                    }

                    if (newline == "}")
                    {
                        isMsgStart = false;
                        sb.Append("\t}\n\n");
                        continue;
                    }

                    if (newline.Trim().StartsWith("//"))
                    {
                        sb.Append($"{newline}\n");
                        continue;
                    }

                    if (newline.Trim() != "" && newline != "}")
                    {
                        if (newline.StartsWith("map<"))
                        {
                            Map(sb, ns, newline);
                        }
                        else if (newline.StartsWith("repeated"))
                        {
                            Repeated(sb, ns, newline);
                        }
                        else
                        {
                            Members(sb, newline, true);
                        }
                    }
                }
            }


            sb.Append("\tpublic static class " + protoName + "\n\t{\n");
            foreach (OpcodeInfo info in msgOpcode)
            {
                sb.Append($"\t\t public const ushort {info.Name} = {info.Opcode};\n");
            }

            sb.Append("\t}\n");
            

            sb.Append("}\n");

            if (cs.Contains("C"))
            {
                GenerateCS(sb, clientMessagePath, proto);
                GenerateCS(sb, serverMessagePath, proto);
                //GenerateCS(sb, clientServerMessagePath, proto);
            }
            
            if (cs.Contains("S"))
            {
                GenerateCS(sb, serverMessagePath, proto);
                //GenerateCS(sb, clientServerMessagePath, proto);
            }
        }

        private static void GenerateCS(StringBuilder sb, string path, string proto)
        {
            GenerateCS(sb.ToString(), path, Path.GetFileNameWithoutExtension(proto) + ".cs");
        }

        private static void GenerateCS(string sb, string path, string fileName)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string csPath = Path.Combine(path, fileName);
            using FileStream txt = new FileStream(csPath, FileMode.Create, FileAccess.ReadWrite);
            using StreamWriter sw = new StreamWriter(txt);
            sw.Write(sb);
        }

        private static void Map(StringBuilder sb, string ns, string newline)
        {
            int start = newline.IndexOf("<") + 1;
            int end = newline.IndexOf(">");
            string types = newline.Substring(start, end - start);
            string[] ss = types.Split(",");
            string keyType = ConvertType(ss[0].Trim());
            string valueType = ConvertType(ss[1].Trim());
            string tail = newline.Substring(end + 1);
            ss = tail.Trim().Replace(";", "").Split(" ");
            string v = ss[0];
            string n = ss[2];
            
            sb.Append("\t\t[MongoDB.Bson.Serialization.Attributes.BsonDictionaryOptions(MongoDB.Bson.Serialization.Options.DictionaryRepresentation.ArrayOfArrays)]\n");
            sb.Append($"\t\t[ProtoMember({n})]\n");
            sb.Append($"\t\tpublic Dictionary<{keyType}, {valueType}> {v} {{ get; set; }}\n");
        }
        
        private static void Repeated(StringBuilder sb, string ns, string newline)
        {
            try
            {
                int index = newline.IndexOf(";");
                newline = newline.Remove(index);
                string[] ss = newline.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);
                string type = ss[1];
                type = ConvertType(type);
                string name = ss[2];
                int n = int.Parse(ss[4]);

                sb.Append($"\t\t[ProtoMember({n})]\n");
                sb.Append($"\t\tpublic List<{type}> {name} {{ get; set; }}\n\n");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{newline}\n {e}");
            }
        }

        private static string ConvertType(string type)
        {
            string typeCs = "";
            switch (type)
            {
                case "int16":
                    typeCs = "short";
                    break;
                case "int32":
                    typeCs = "int";
                    break;
                case "bytes":
                    typeCs = "byte[]";
                    break;
                case "uint32":
                    typeCs = "uint";
                    break;
                case "long":
                    typeCs = "long";
                    break;
                case "int64":
                    typeCs = "long";
                    break;
                case "uint64":
                    typeCs = "ulong";
                    break;
                case "uint16":
                    typeCs = "ushort";
                    break;
                default:
                    typeCs = type;
                    break;
            }

            return typeCs;
        }

        private static void Members(StringBuilder sb, string newline, bool isRequired)
        {
            try
            {
                int index = newline.IndexOf(";");
                newline = newline.Remove(index);
                string[] ss = newline.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);
                string type = ss[0];
                string name = ss[1];
                int n = int.Parse(ss[3]);
                string typeCs = ConvertType(type);

                sb.Append($"\t\t[ProtoMember({n})]\n");
                sb.Append($"\t\tpublic {typeCs} {name} {{ get; set; }}\n\n");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{newline}\n {e}");
            }
        }
    }
}