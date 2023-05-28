using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.WindowsServices;
using Microsoft.OpenApi.Models;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.PortableExecutable;
using DTL23_API.Middleware;
using DTL23_API.Attributes;
using DTL23_API.Utilities;

namespace DTL23_API
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class Program {
		internal static AdoClickHouseClient ClickHouseClient { get; private set; } = new();

		public static void Main(string[] args) {
			Logging.ToLog("---Запуск");

			try {
				WebApplicationOptions options = new WebApplicationOptions() {
					Args = args,
					ContentRootPath = WindowsServiceHelpers.IsWindowsService() ?
							AppContext.BaseDirectory : default
				};

				WebApplicationBuilder builder = WebApplication.CreateBuilder(options);

				builder.Services.AddRazorPages(options => {
					options.Conventions.AddPageApplicationModelConvention(
						"/StreamedSingleFileUploadPhysical", model => {
							model.Filters.Add(
								new GenerateAntiforgeryTokenCookieAttribute());
							model.Filters.Add(
								new DisableFormValueModelBindingAttribute());
						});
				});

				builder.Services.AddEndpointsApiExplorer();
				builder.Services.AddSwaggerGen();
				builder.Host.UseWindowsService();

				builder.Services.AddSwaggerGen(c => {
					c.SwaggerDoc("v1", new OpenApiInfo {
						Contact = new OpenApiContact() {
							Email = "nibble@yandex.ru",
							Name = "Грашкин Павел"
						},
						Description = "API для DTL23 по задаче 18",
						Title = "DTL23_API",
						Version = "1.0"
					});

					string xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
					string xmlFilePath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
					Console.WriteLine(xmlFilePath);
					c.IncludeXmlComments(xmlFilePath);

					c.AddSecurityDefinition(
						"ApiKey",
						new OpenApiSecurityScheme {
							Description = "XApiKey must appear in header",
							In = ParameterLocation.Header,
							Name = "XApiKey",
							Type = SecuritySchemeType.ApiKey
						}
					);

					var key = new OpenApiSecurityScheme() {
						Reference = new OpenApiReference {
							Type = ReferenceType.SecurityScheme,
							Id = "ApiKey"
						},
						In = ParameterLocation.Header
					};

					var requirement = new OpenApiSecurityRequirement {
						{ key, new List<string>() }
					};

					c.AddSecurityRequirement(requirement);
				});

				builder.Services.AddControllers();
				WebApplication app = builder.Build();

				app.UseSwagger();
				app.UseSwaggerUI();

				string host = "http://10.101.80.60:" + (Debugger.IsAttached ? "671" : "670");
				app.Urls.Add(host);

				app.MapControllers();
				app.UseMiddleware<ApiKeyMiddleware>();

				Configure(app);
				app.Run();
			} catch (Exception e) {
				Logging.ToLog(e.Message + Environment.NewLine + e.StackTrace);
			}
		}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

		private static void Configure(IApplicationBuilder app) {
			app.Use(async (context, next) => {
				string headers = string.Empty;
				foreach (string headerKey in context.Request.Headers.Keys)
					headers += headerKey + ":" + context.Request.Headers[headerKey].ToString() + "; ";
				headers = headers.TrimEnd(' ').TrimEnd(';');

				Logging.ToLog(
					context.Connection.Id + " from " +
					context.Connection.RemoteIpAddress + ":" + context.Connection.RemotePort + " -> " +
					context.Connection.LocalIpAddress + ":" + context.Connection.LocalPort + " -> " +
					context.Request.Method + " -> " +
					context.Request.Path + " (" + context.Request.QueryString + ") -> headers: " + headers);

				await next.Invoke();
			});
		}
	}
}