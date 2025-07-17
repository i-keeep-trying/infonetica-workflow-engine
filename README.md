# Infonetica Workflow Engine (Take-Home Assignment)

## 📌 Objective

This is a minimal backend service written in **.NET 8 / C#** that allows clients to define and run configurable **workflow state machines**.

---

## ✅ Features

- Define workflows with **states** and **actions**
- Start workflow **instances** from definitions
- Execute **transitions** between states
- Validate all actions and transitions
- Retrieve the **current state** and **history** of instances

---

## 🚀 Getting Started

### 🔧 Requirements

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Postman or any API testing tool (optional)

### ▶️ Run the API

```bash
dotnet run
```
Then access it at http://localhost:{port}
Example: http://localhost:5077

## 🧪 API Endpoints

### 1. POST /workflow-definitions
Create a new workflow:
```bash
{
  "name": "Document Approval",
  "states": [
    { "id": "draft", "name": "Draft", "isInitial": true, "isFinal": false, "enabled": true },
    { "id": "review", "name": "Review", "isInitial": false, "isFinal": false, "enabled": true },
    { "id": "approved", "name": "Approved", "isInitial": false, "isFinal": true, "enabled": true }
  ],
  "actions": [
    { "id": "submit", "name": "Submit", "enabled": true, "fromStates": ["draft"], "toState": "review" },
    { "id": "approve", "name": "Approve", "enabled": true, "fromStates": ["review"], "toState": "approved" }
  ],
  "description": "A simple approval process"
}
```