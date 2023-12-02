var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointDefinitions(typeof(Program));

Swagger.DefineServices(builder.Services);

var app = builder.Build();
app.UseEndpointDefinitions();

Swagger.DefineEndpoints(app);
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API Name");
});
// Configure the HTTP request pipeline.



app.MapGet("/HelloWorld", () =>
{
    return $"ms3dprintMachine : 0.0.0 : {DateTime.Now}";
}).AllowAnonymous();



app.Run();
