namespace Backend.Features;

using Microsoft.EntityFrameworkCore;
using Carter;
using static EndpointConstants;

public class TodoModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(BASE_URL, (TodoContext db, LinkGenerator linkGenerator, HttpContext context) =>
        {
            var url = linkGenerator.GetUriByName(context, "GetTodos", default);
            return db.Todos
                .Select(i => TodoViewModel.MapFrom(i, $"{url}/{i.Id}"))
                .ToList();
        }).WithName("GetTodos");

        app.MapGet($"{BASE_URL}/{{id}}", (int id, TodoContext db) => db.Todos.Find(id))
            .WithName("GetTodoById");

        app.MapPost(BASE_URL,
            async (Todo todo, TodoContext db, LinkGenerator linkGenerator, HttpContext context) =>
        {
            db.Todos.Add(todo);
            await db.SaveChangesAsync();
            var url = linkGenerator.GetUriByName(context, "GetTodoById", new { id = todo.Id });
            return TodoViewModel.MapFrom(todo, url);
        });

        app.MapMethods($"{BASE_URL}/{{id}}", new string[] { "PATCH" },
        async (int id, Todo item, TodoContext db, LinkGenerator linkGenerator, HttpContext context) =>
        {
            var entity = await db.Todos.FindAsync(id);

            if (entity is null)
            {
                return Results.NotFound();
            }

            entity.Title = item.Title;
            entity.Completed = item.Completed;
            entity.Order = item.Order;

            await db.SaveChangesAsync();
            var url = linkGenerator.GetUriByName(context, "GetTodoById", new { id = entity.Id });
            return Results.Ok(TodoViewModel.MapFrom(entity, url));
        });

        app.MapDelete(BASE_URL, async (TodoContext db) =>
        {
            db.Todos.RemoveRange(db.Todos.ToList());
            await db.SaveChangesAsync();
            return Enumerable.Empty<Todo>();
        });

        app.MapDelete($"{BASE_URL}/{{id}}", async (int id, TodoContext db) =>
        {
            var entity = await db.Todos.FindAsync(id);

            if (entity is null)
            {
                return Results.NotFound();
            }
            db.Remove(entity);
            await db.SaveChangesAsync();

            return Results.NoContent();
        });
    }
}

public record class Todo
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public bool Completed { get; set; }

    public int Order { get; set; }
}

public class TodoViewModel
{
    public int Id { get; init; }

    public string? Title { get; init; }

    public bool Completed { get; init; }

    public string? Url { get; init; }

    public int Order { get; set; }

    public static TodoViewModel MapFrom(Todo todo, string? url) => new()
    {
        Id = todo.Id,
        Title = todo.Title,
        Order = todo.Order,
        Completed = todo.Completed,
        Url = url ?? string.Empty,
    };
}

public class TodoContext : DbContext
{
    public TodoContext(DbContextOptions<TodoContext> options)
        : base(options)
    {
    }

    public DbSet<Todo> Todos { get; set; } = default!;
}

public static class EndpointConstants
{
    public const string BASE_URL = "/api/todos";
}
