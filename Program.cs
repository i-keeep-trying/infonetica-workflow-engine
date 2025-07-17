using WorkflowEngine.Models; // Import your models
using System.Collections.Concurrent; // For thread-safe in-memory storage

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();


// ✅ In-memory storage for workflow definitions
var workflowDefinitions = new ConcurrentDictionary<string, WorkflowDefinition>();

// ✅ POST endpoint: Create a new workflow definition
app.MapPost("/workflow-definitions", (WorkflowDefinition definition) =>
{
    // Validation: must have exactly one initial state
    if (!definition.HasSingleInitialState())
    {
        return Results.BadRequest("Workflow must have exactly one initial state.");
    }

    // Validation: unique workflow ID (optional check)
    if (workflowDefinitions.ContainsKey(definition.Id))
    {
        return Results.Conflict($"Workflow with ID '{definition.Id}' already exists.");
    }

    // Save to in-memory store
    workflowDefinitions[definition.Id] = definition;

    return Results.Created($"/workflow-definitions/{definition.Id}", definition);
});


// ✅ GET endpoint: Retrieve a workflow definition by ID
app.MapGet("/workflow-definitions/{id}", (string id) =>
{
    if (workflowDefinitions.TryGetValue(id, out var def))
    {
        return Results.Ok(def);
    }

    return Results.NotFound($"Workflow with ID '{id}' not found.");
});

var workflowInstances = new ConcurrentDictionary<string, WorkflowInstance>();

// ✅ POST /workflow-instances: Start a new instance of a workflow
app.MapPost("/workflow-instances", (StartInstanceRequest req) =>
{
    string definitionId = req.DefinitionId;

    if (!workflowDefinitions.TryGetValue(definitionId, out var definition))
    {
        return Results.NotFound($"Workflow definition '{definitionId}' not found.");
    }

    var initialState = definition.States.FirstOrDefault(s => s.IsInitial);
    if (initialState == null)
    {
        return Results.BadRequest("Workflow definition has no initial state.");
    }

    var instance = new WorkflowInstance
    {
        DefinitionId = definitionId,
        CurrentStateId = initialState.Id
    };

    workflowInstances[instance.Id] = instance;

    return Results.Created($"/workflow-instances/{instance.Id}", instance);
});

app.MapPost("/workflow-instances/{id}/actions", (string id, ExecuteActionRequest req) =>
{
    // Get instance
    if (!workflowInstances.TryGetValue(id, out var instance))
    {
        return Results.NotFound($"Workflow instance '{id}' not found.");
    }

    // Get definition
    if (!workflowDefinitions.TryGetValue(instance.DefinitionId, out var definition))
    {
        return Results.NotFound($"Workflow definition '{instance.DefinitionId}' not found.");
    }

    var currentState = definition.States.FirstOrDefault(s => s.Id == instance.CurrentStateId);
    if (currentState == null || currentState.IsFinal)
    {
        return Results.BadRequest("Current state is final or invalid. No further transitions allowed.");
    }

    // Find action
    var action = definition.Actions.FirstOrDefault(a => a.Id == req.ActionId);
    if (action == null)
    {
        return Results.NotFound($"Action '{req.ActionId}' not found in definition.");
    }

    if (!action.Enabled)
    {
        return Results.BadRequest($"Action '{req.ActionId}' is disabled.");
    }

    if (!action.FromStates.Contains(currentState.Id))
    {
        return Results.BadRequest($"Action '{req.ActionId}' is not valid from current state '{currentState.Id}'.");
    }

    // Check destination state exists
    if (!definition.States.Any(s => s.Id == action.ToState))
    {
        return Results.BadRequest($"Target state '{action.ToState}' does not exist.");
    }

    // All good — transition to new state
    instance.CurrentStateId = action.ToState;
    instance.History.Add(new ActionHistory
    {
        ActionId = action.Id,
        Timestamp = DateTime.UtcNow
    });

    return Results.Ok(instance);
});

// ✅ GET /workflow-instances/{id}
app.MapGet("/workflow-instances/{id}", (string id) =>
{
    if (!workflowInstances.TryGetValue(id, out var instance))
    {
        return Results.NotFound($"Workflow instance '{id}' not found.");
    }

    return Results.Ok(instance);
});

app.Run();
