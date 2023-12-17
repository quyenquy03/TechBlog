﻿using AspNetCoreHero.ToastNotification;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using TechBlog.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<TechBlogContext>(options => options.UseSqlServer(connection));

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddSassCompiler();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(cfg => {                    // Đăng ký dịch vụ Session
	cfg.Cookie.Name = "BonsaiShop";             // Đặt tên Session - tên này sử dụng ở Browser (Cookie)
	cfg.IdleTimeout = new TimeSpan(0, 30, 0);    // Thời gian tồn tại của Session
	cfg.Cookie.HttpOnly = true;
	cfg.Cookie.IsEssential = true;
});
builder.Services.AddControllers()
		.AddJsonOptions(options =>
		{
			options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
		});
builder.Services.AddNotyf(config => { config.DurationInSeconds = 10; config.IsDismissable = true; config.Position = NotyfPosition.TopRight; });
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
	endpoints.MapControllerRoute(
	  name: "areas",
	  pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
	);
});
app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
//Scaffold-DbContext "Server=.\SQLExpress;Database=TechBlog;Trusted_Connection=True;TrustServerCertificate=True; Connection Timeout=3600" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Force
