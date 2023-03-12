# AOGame
base on ET framework(基于ET框架魔改的一个框架)

---框架尚在开发当中---

---
## [Accelerate Auto Obvious]
## 【加速 自动 清晰】

---

 ## AOGame保留了ET框架的核心ECS模块、事件模块、网络模块以及消息模块等，也就是说AOGame与ET框架的底层是差不多的，AOGame仅在业务层及业务结构方面做了调整。

---
## 以下是一些主要的改动和调整：
- 缩减程序集
- Scene流程拆分调整
- Unit拆分调整
- 消息调用接口生成
- 实体组件、属性自动同步
- 分离网络Session相关为单独程序集
- 接入PuerTs脚本方案（仅用于ui逻辑编写，C#侧仍基于HybridClr热更方案）
- 接入Luban表格配置方案
- 接入EGamePlay技能系统
- 引入AOP面向切面编程流程
- 以及其他

<img src="Readme/AOGameDemo.gif" width="100%">