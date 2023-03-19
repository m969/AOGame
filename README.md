# AOGame
base on ET framework(基于ET框架魔改的一个框架)

---框架尚在开发当中---

---
## [Accelerate Auto Obvious]
## 【加速 自动 清晰】

---

 # AOGame保留了ET框架的核心ECS模块、事件模块、网络模块以及消息模块等，也就是说AOGame与ET框架的底层是差不多的，AOGame仅在业务层及业务结构方面做了调整。

---
# 以下是一些主要的改动和调整：
- 缩减程序集
- Scene流程拆分调整
- Unit拆分调整
- 消息调用接口生成
- 实体组件、属性自动同步
- 分离网络Session相关为单独程序集
- 接入PuerTs脚本方案（仅用于ui逻辑编写，C#侧仍基于HybridCLR热更方案）
- 接入Luban表格配置方案
- 接入EGamePlay技能系统
- 引入AOP面向切面编程流程
- 以及其他

<img src="Readme/AOGameDemo.gif" width="100%">


---
# Scene和Unit拆分调整
AOGame将ET的Scene拆分成App和Scene，App表示业务应用，App又进一步拆分成各个类型App，例如RealmApp、GateApp、MapApp等，Scene则单独表示场景Scene。

AOGame将ET的Unit拆分成Avatar、Npc和ItemUnit，Avatar表示玩家地图单位，Npc表示非玩家地图单位（怪物也属于Npc单位），ItemUnit表示其他非生命地图单位。

为什么要拆分Scene和Unit？实体的类型和类型实体的不同。

- 实体的类型即功能一样的用同一实体辅以字段区分不同类型，比如场景Scene或物品Item，包子和馒头就是功能一样的Item，都有着相同的字段，只是字段的值不一样，id和name的值不一样，物品的功能都一样所以用同一实体统一管理。

- 类型实体即功能有所差别的用不同类型实体辅以接口判断归类，比如App，MapApp和GateApp虽然都是IApp，但是他们的功能有非常大的差别，所以用类型实体流程分而治之。

# App进程
从下图我们可以非常清晰明了的看到组成整个游戏服的所有类型App
- b 表示base，基础服。
- c 表示center，中心服，一个区只有一个。
- g 表示global，全局服，整个分布式服务机制里只有一个。
- s 表示serve，游戏的主要业务服。

<img src="Readme/Apps.png" width="40%">

# Client模式
走向不固定，可叠加的状态，可使用模式机制，比如ClientMode客户端模式

# 流程 WorkFlow
走向固定，互斥且不可叠加的状态，可使用流程WorkFlow，比如回合制的战斗流程，开始-进行-结束

CombatFlowLauncher

ToBranch