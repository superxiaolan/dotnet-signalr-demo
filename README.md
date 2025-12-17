# dotnet-signalr-demo

> 一个用于**学习和实践 ASP.NET Core SignalR** 的后端示例项目

---

## 📌 项目简介

本仓库用于系统性学习 **ASP.NET Core SignalR**，通过可运行的 Demo，逐步掌握：

* SignalR 的基础使用方式
* Hub 的生命周期与调用模型
* 服务端向客户端的实时推送
* SignalR 在真实后端项目中的常见用法

该项目以 **学习记录 + 可运行代码** 为目标，适合作为个人技术沉淀。

---

## 🎯 学习目标

* [x] 创建标准 ASP.NET Core Web API 项目
* [x] 集成 SignalR
* [x] 实现基础 ChatHub（消息广播）
* [ ] 使用 HTML / JavaScript 连接 SignalR Hub
* [ ] 理解 Clients / Groups / ConnectionId
* [ ] 模拟真实业务场景（通知 / 在线用户 / 实时推送）

---

## 🧰 技术栈

* **.NET**：.NET 8 (LTS)
* **Web 框架**：ASP.NET Core Web API
* **实时通信**：SignalR
* **API 文档**：Swagger / OpenAPI
* **版本控制**：Git + GitHub

---

## 📂 项目结构说明

```
dotnet-signalr-demo
 ├─ src
 │   └─ SignalRDemo.Api        # Web API + SignalR 服务端
 │       ├─ Controllers        # Web API 控制器
 │       ├─ Hubs               # SignalR Hub
 │       │   └─ ChatHub.cs
 │       ├─ Program.cs         # 程序入口
 │       └─ appsettings.json
 ├─ docs                        # 学习笔记 / 文档（后续补充）
 └─ README.md
```

---

## 🚀 快速开始

### 1️⃣ 克隆仓库

```bash
git clone https://github.com/superxiaolan/dotnet-signalr-demo.git
cd dotnet-signalr-demo
```

---

### 2️⃣ 启动项目

使用 Visual Studio / Rider 打开解决方案：

```
src/SignalRDemo/SignalRDemo.sln
```

直接运行（F5）：

* 启动成功后会自动打开 Swagger 页面
* 表示 Web API 服务正常运行

---

### 3️⃣ SignalR Hub 说明

当前已实现基础 Hub：

* 路径：`/hub/chat`
* 功能：向所有已连接客户端广播消息

```csharp
public class ChatHub : Hub
{
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }
}
```

---

## 📝 学习记录（持续更新）

* **2025-xx-xx**：

  * 初始化 ASP.NET Core Web API 项目
  * 集成 SignalR
  * 实现基础 ChatHub

> 每一个功能点都会对应一次 commit，便于回溯学习过程。

---

## 📌 约定与说明

* 本项目以 **学习和实践** 为目的
* 不追求完整业务系统
* 代码力求：

  * 可运行
  * 易理解
  * 便于扩展

---

## 📜 License

MIT License

---

## 🙋 说明

这是一个个人学习项目，用于记录 SignalR 的学习过程。
如果对你有帮助，欢迎 Star ⭐️
