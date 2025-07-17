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

---

## 🧪 API Endpoints

### 1. POST /workflow-definitions

Create a new workflow. For example:
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

### 2. GET /workflow-definitions/{id}

Retrieve a workflow definition by ID.

### 3. POST /workflow-instances

Start a new instance:
```bash
{
  "definitionId": "your-workflow-definition-id"
}
```

### 4.POST /workflow-instances/{instanceId}/actions

```bash
{
  "actionId": "submit"
}
```

### 5. GET /workflow-instances/{id}

Get current state and action history.

---

## 🛠 Assumptions

- Data is stored in-memory and **resets on restart**
- Each workflow must have **exactly one initial state**
- Final states cannot be **transitioned** from
- Transition validation is strictly **enforced**

---

## 🧠 Design Notes

- Minimal API style with clear separation of models and logic
- Uses ConcurrentDictionary for thread-safe in-memory storage
- Input validation returns meaningful error responses
- Well-structured and extensible without over-engineering

---

## 📂 Project Structure

```bash
/Models
  - State.cs
  - ActionDefinition.cs
  - WorkflowDefinition.cs
  - WorkflowInstance.cs
  - StartInstanceRequest.cs
  - ExecuteActionRequest.cs

Program.cs
README.md
```