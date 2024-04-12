// using FastEndpoints;
// using infrastructure;
// using infrastructure.DomainModels;
// using Microsoft.AspNetCore.Mvc;
//
// namespace api.Endpoints;
//
// // public class GetTodoRequestDto
// // {
// //     [BindFrom(nameof(Id))]
// //     public int Id { get; set; }
// // }
//
// public class GetTodo(Db db) : EndpointWithoutRequest<Todo>
// {
//     public override void Configure()
//     {
//         Get("/api/todo/{id}");
//         AllowAnonymous();
//     }
//     
//     public override async Task HandleAsync( CancellationToken ct)
//     {
//         int id = Route<int>("id");
//      //   await SendAsync(db.GetTodoWithTags(id), cancellation: ct);
//     }
// }

