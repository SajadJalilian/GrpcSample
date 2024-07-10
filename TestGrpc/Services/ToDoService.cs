using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using TestGrpc.Data;
using TestGrpc.Models;

namespace TestGrpc.Services;

public class ToDoService(AppDbContext dbContext) : ToDoIt.ToDoItBase
{
    public override async Task<CreateToToResponse> CreateDoTo(CreateToDoRequest request, ServerCallContext context)
    {
        if (request.Description == string.Empty || request.Title == string.Empty)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "You must supply a valid object"));
        }

        var toDoItem = new TodoItem()
        {
            Title = request.Title,
            Descreption = request.Description
        };
        await dbContext.AddAsync(toDoItem);
        await dbContext.SaveChangesAsync();

        return await Task.FromResult(new CreateToToResponse
        {
            Id = toDoItem.Id
        });
    }

    public override async Task<ReadToDoresponse> ReadTodo(ReadToDoRequest request, ServerCallContext context)
    {
        var toDoItem = await dbContext.TodoItems.FirstOrDefaultAsync(x => x.Id == request.Id);

        if (toDoItem is null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Item with provided Id not found"));
        }

        return await Task.FromResult(new ReadToDoresponse()
        {
            Id = request.Id,
            Title = toDoItem.Title,
            Description = toDoItem.Descreption,
            ToDoStatus = toDoItem.TodoStatus
        });
    }

    public override Task<GetAllResponse> ListTodo(GetAllRequest request, ServerCallContext context)
    {
        return base.ListTodo(request, context);
    }

    public override Task<UpdateToDoResponse> UpdateToDo(UpdateToDoRequest request, ServerCallContext context)
    {
        return base.UpdateToDo(request, context);
    }

    public override Task<DeleteToDoResponse> DeleteToDo(DeleteToDoRequest request, ServerCallContext context)
    {
        return base.DeleteToDo(request, context);
    }
}