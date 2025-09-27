using ProyectoDistribuidora;

var builder = WebApplication.CreateBuilder(args);

// Carga la clase Startup
var startup = new Startup(builder.Configuration);

// Llama a ConfigureServices en Startup
startup.ConfigureServices(builder.Services);

var app = builder.Build();

// Configura el pipeline de solicitudes HTTP usando el método Configure de Startup
startup.Configure(app, app.Environment);

app.Run();