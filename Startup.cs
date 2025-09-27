using Microsoft.EntityFrameworkCore;
using ProyectoDistribuidora.CasoDeUso.Login;
using ProyectoDistribuidora.Compartida.Interfaces;
using ProyectoDistribuidora.Compartida.Interfaces.IRepositorio;
using ProyectoDistribuidora.Compartida.Maper;
using ProyectoDistribuidora.Data;
using ProyectoDistribuidora.Repositorios.GestionPedidos;
using ProyectoDistribuidora.Repositorios.Legacy;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ProyectoDistribuidora.ServicioMigracion;
using ProyectoDistribuidora.CasoDeUso.Cliente;
using ProyectoDistribuidora.CasoDeUso.Pedido;
using ProyectoDistribuidora.CasoDeUso.Stock;
using ProyectoDistribuidora.Compartida.Repositorios;
using ProyectoDistribuidora.CasosDeUso.Pedido;
using Microsoft.OpenApi.Models;

namespace ProyectoDistribuidora
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // Configura los servicios que la aplicación utilizará
        public void ConfigureServices(IServiceCollection services)
        {
            // Configuración de Entity Framework Core con SQL Server
            services.AddDbContext<LegacyContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("ApiConnection")));
            services.AddDbContext<GestionPedidosContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("ApiConnectionGP")));

            // Configuración JWT
            var key = Encoding.ASCII.GetBytes("ClaveSecretaMuySegura123456789101112!"); // Cambia por tu clave secreta
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Bearer";
                options.DefaultChallengeScheme = "Bearer";
            })
            .AddJwtBearer("Bearer", options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "TuApiIssuer", // Cambia este valor
                    ValidAudience = "TuApiAudience", // Cambia este valor
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("ClaveSecretaMuySegura123456789101112!")) // Mismo valor que se usa para firmar el token
                };
            });

            // Startup.cs - Configuración de servicios

            // Casos de uso relacionados con Usuarios
            services.AddScoped<CUAltaUsuario>();       // Alta de usuarios en Gestión de Pedidos
            services.AddScoped<CUBajaUsuario>();       // Baja de usuarios en Gestión de Pedidos
            services.AddScoped<CUModificarUsuario>();  // Modificación de usuarios en Gestión de Pedidos
            services.AddScoped<CUObtenerUsuarios>();      // Obtención de usuarios desde Gestión de Pedidos
            services.AddScoped<LCUObtenerUsuarioById>(); // Obtención de un usuario por ID desde el sistema Legacy
            services.AddScoped<LCUObtenerUsuarios>();    // Obtención de usuarios desde el sistema Legacy
            services.AddScoped<CULogin>();
            services.AddScoped<CUObtenerUsuarioById>();

            // Casos de uso relacionados con Clientes
            services.AddScoped<CUAltaCliente>();       // Alta de clientes en Gestión de Pedidos
            services.AddScoped<CUBajaCliente>();       // Baja de clientes en Gestión de Pedidos
            services.AddScoped<CUModificarCliente>();  // Modificación de clientes en Gestión de Pedidos
            services.AddScoped<LCUObtenerClienteById>(); // Obtención de un cliente por ID desde el sistema Legacy
            services.AddScoped<LCUObtenerClientes>();    // Obtención de clientes desde el sistema Legacy
            services.AddScoped<CUObtenerClienteById>();
            services.AddScoped<CUObtenerClientes>();
            services.AddScoped<LCUAltaCliente>();
            services.AddScoped<LCUModificarCliente>();
            services.AddScoped<LCUBajaCliente>();

            // Casos de uso relacionados con Pedidos (Gestión de Pedidos)
            services.AddScoped<CUAltaPedido>(); // Alta de pedidos en Gestión de Pedidos
            services.AddScoped<CUBajaPedido>(); // Baja de pedidos en Gestión de Pedidos
            services.AddScoped<CUModificarPedido>(); // Modificación de pedidos en Gestión de Pedidos
            services.AddScoped<CUObtenerPedidoById>(); // Obtención de un pedido por ID en Gestión de Pedidos
            services.AddScoped<CUObtenerPedidos>(); // Obtención de todos los pedidos en Gestión de Pedidos
            services.AddScoped<CUObtenerPedidosPorCliente>(); // Obtención de pedidos por cliente en Gestión de Pedidos
            services.AddScoped<CUObtenerPedidosPorEstado>(); // Obtención de pedidos por estado en Gestión de Pedidos
            services.AddScoped<CUObtenerPedidosPorFechaEntrega>(); // Obtención de pedidos por fecha de entrega en Gestión de Pedidos
            services.AddScoped<CUObtenerPedidosPorVendedor>(); // Obtención de pedidos por vendedor en Gestión de Pedidos
            services.AddScoped<CUObtenerLineasDePedido>(); // Obtención de líneas de un pedido en Gestión de Pedidos
            services.AddScoped<CUObtenerLineasConProducto>();
            services.AddScoped<LCUCambiarEstadoPedido>(); // Actualización del estado de pedidos
            services.AddScoped<CUAltaLineaPedido>();
            services.AddScoped<CUModificarLineaPedido>();
            services.AddScoped<CUBajaLineaPedido>();
            services.AddScoped<CUObtenerLineaDePedidoById>();
            services.AddScoped<LCUObtenerLineaDePedidoById>();
            // Casos de uso relacionados con Pedidos (Legacy)
            services.AddScoped<LCUAltaPedido>(); // Alta de pedidos en la base Legacy
            services.AddScoped<LCUInsertarLineaPedido>(); // Inserción de líneas de pedido en la base Legacy
            services.AddScoped<LCUObtenerPedidoById>(); // Obtención de un pedido por ID desde la base Legacy
            services.AddScoped<LCUActualizarPedido>(); // Caso de uso para actualizar pedido en Legacy
            services.AddScoped<LCUActualizarPedidoSinLineas>();
            services.AddScoped<LCUObtenerVendedores>();

            // Casos de uso relacionados con Stock
            services.AddScoped<CUAltaStock>();         // Alta de stock en Gestión de Pedidos
            services.AddScoped<CUBajaStock>();         // Baja de stock en Gestión de Pedidos
            services.AddScoped<CUModificarStock>();    // Modificación de stock en Gestión de Pedidos
            services.AddScoped<CUObtenerStocks>(); // Obtención de un stock por ID desde el sistema Legacy
            services.AddScoped<CUObtenerStockById>();
            services.AddScoped<CUObtenerStocksPorDescripcion>();
            services.AddScoped<LCUAltaStock>();
            services.AddScoped<LCUBajaStock>();
            services.AddScoped<LCUModificarStock>();
            services.AddScoped<LCUObtenerStockById>(); 

            // Repositorios relacionados con Usuarios
            services.AddScoped<IUsuarioRepositorio, LoginRepositorio>(); // Repositorio de usuarios en Gestión de Pedidos
            services.AddScoped<LIUsuarioRepositorio, LLoginRepositorio>();        // Repositorio de usuarios en el sistema Legacy

            // Repositorios relacionados con Clientes
            services.AddScoped<IClienteRepositorio, ClienteRepositorio>(); // Repositorio de clientes en Gestión de Pedidos
            services.AddScoped<LIClienteRepositorio, LClienteRepositorio>();        // Repositorio de clientes en el sistema Legacy

            // Repositorios relacionados con Pedidos
            services.AddScoped<IPedidoRepositorio, PedidoRepositorio>();   // Repositorio de pedidos en Gestión de Pedidos
            services.AddScoped<LIPedidoRepositorio, LPedidoRepositorio>();          // Repositorio de pedidos en el sistema Legacy

            // Repositorios relacionados con Stock
            services.AddScoped<IStockRepositorio, StockRepositorio>();     // Repositorio de stock en Gestión de Pedidos
            services.AddScoped<LIStockRepositorio, LStockRepositorio>();            // Repositorio de stock en el sistema Legacy

            // Registro de ServiceMapping:
            services.AddScoped<ServiceMapping>();

            // Servicios relacionados con Usuarios
            services.AddScoped<LoginServicio>();     // Servicio para lógica de negocio de usuarios
            services.AddScoped<UsuarioMapeo>();        // Mapeo de datos para usuarios

            // Servicios relacionados con Clientes
            services.AddScoped<ClienteServicio>();     // Servicio para lógica de negocio de clientes
            services.AddScoped<ClienteMapeo>();         // Mapeo de datos para clientes

            // Servicios relacionados con Pedidos
            services.AddScoped<PedidoServicio>();      // Servicio para lógica de negocio de pedidos
            services.AddScoped<PedidoMapeo>();         // Mapeo de datos para pedidos

            // Servicios relacionados con Stock
            services.AddScoped<StockServicio>();       // Servicio para lógica de negocio de stock
            services.AddScoped<StockMapeo>();          // Mapeo de datos para stock

            services.AddHostedService<Observer>();//Se inicia el observer

            // Configuración de CORS para uso LOCAL
           /* services.AddCors(options =>
            {
                options.AddPolicy("PermitirFrontend", builder =>
                {
                    builder.WithOrigins("https://localhost:7159") // Cambia por la URL de tu frontend
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
            });*/

            services.AddCors(options =>
            {/*
                options.AddPolicy("PermitirFrontend", builder => // Configuración de CORS para uso Azure
                {
                    builder.WithOrigins(
                        "https://webgestionpedidos-apeefdbxbcafb5e3.canadacentral-01.azurewebsites.net"  // Frontend en Azure
                    )
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });*/

                options.AddPolicy("PermitirFrontend", builder =>
                {
                    builder.AllowAnyOrigin()  // Permite cualquier origen
                           .AllowAnyHeader()   // Permite cualquier encabezado HTTP
                           .AllowAnyMethod();  // Permite cualquier método HTTP (GET, POST, PUT, DELETE, etc.)
                });

            });


            // Configura los controladores y Swagger para la documentación de la API
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Distribuidora API",
                    Version = "v1",
                    Description = "API para la gestión de pedidos en empresas distribuidoras."
                });

                // 🔹 Agregar autenticación en Swagger
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Ingrese el token JWT en el formato 'Bearer {token}'",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });

                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });
        }

        // Configura el pipeline de solicitudes HTTP
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(); // Solo se usa en desarrollo para depuración.
            }

            if (env.IsDevelopment() || env.IsProduction())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Distribuidora API v1");

                    // Esto asegura que Swagger UI esté en la raíz
                    // Si se quiere mostrar swagger en local hay que comentar esta linea de RoutePrefix
                    //c.RoutePrefix = string.Empty; 
                });
            }

            // Usar CORS antes de autorización
            app.UseCors("PermitirFrontend");


            app.UseHttpsRedirection();

            app.UseRouting();

            // Usar CORS antes de autorización
            app.UseCors("PermitirFrontend");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            
        }
    }
}
