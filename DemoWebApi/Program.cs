﻿using DemoWebApi.EFCore;
using Microsoft.EntityFrameworkCore;

namespace DemoWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // thiết lập kết nối đến CSDL

            // lấy ra chuỗi kết nối từ config appsetting.json
            string connection = builder.Configuration.GetConnectionString("Default");

            // bát đầu kết nối
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connection));

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseAuthorization();

            app.Run();
        }
    }
}