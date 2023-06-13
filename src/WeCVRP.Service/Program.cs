using WeCVRP.Algorithms;
using WeCVRP.Core.Format;
using WeCVRP.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<IRemoteSolverService, RemoteSolverService>();
builder.Services.AddSingleton<ICVRPCalculatorProvider, CVRPCalculatorProvider>();
builder.Services.AddSingleton<ITspLib95Deserializer, TspLib95Deserializer>();

var app = builder.Build();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
