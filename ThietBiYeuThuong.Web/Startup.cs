using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ThietBiYeuThuong.Data.Models;
using ThietBiYeuThuong.Data.Repositories;
using ThietBiYeuThuong.Web.Services;

namespace ThietBiYeuThuong.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ThietBiYeuThuongDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))/*.EnableSensitiveDataLogging()*/);

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IRoleRepository, RoleRepository>();
            services.AddTransient<IHoSoBNRepository, HoSoBNRepository>();
            services.AddTransient<ICTHoSoBNRepository, CTHoSoBNRepository>();
            services.AddTransient<ITinhTonRepository, TinhTonRepository>();
            services.AddTransient<ILoaiThietBiRepository, LoaiThietBiRepository>();
            services.AddTransient<IBenhNhanRepository, BenhNhanRepository>();
            services.AddTransient<IBenhNhanThietBiRepository, BenhNhanThietBiRepository>();
            services.AddTransient<IThietBiRepository, ThietBiRepository>();
            services.AddTransient<ITinhTrangBNRepository, TinhTrangBNRepository>();
            services.AddTransient<IPhieuNhapRepository, PhieuNhapRepository>();
            services.AddTransient<IPhieuXuatRepository, PhieuXuatRepository>();
            services.AddTransient<ICTPhieuRepository, CTPhieuRepository>();
            services.AddTransient<INVYTRepository, NVYTRepository>();

            services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddTransient<IHoSoBNService, HoSoBNService>();
            services.AddTransient<ICTHoSoBNService, CTHoSoBNService>();
            services.AddTransient<ITinhTonService, TinhTonService>();
            services.AddTransient<ILoaiThietBiService, LoaiThietBiService>();
            services.AddTransient<IBenhNhanService, BenhNhanService>();
            services.AddTransient<ITinhTrangBNService, TinhTrangBNService>();
            services.AddTransient<IPhieuNhapService, PhieuNhapService>();
            services.AddTransient<IPhieuXuatService, PhieuXuatService>();
            services.AddTransient<ICTPhieuService, CTPhieuService>();
            services.AddTransient<ITrangThaiService, TrangThaiService>();
            services.AddTransient<IThietBiService, ThietBiService>();
            services.AddTransient<IBenhNhanThietBiService, BenhNhanThietBiService>();
            services.AddTransient<INVYTService, NVYTService>();

            // FOR session
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
            });

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            // for session
            app.UseSession();

            // culture format
            var supportedCultures = new[] { new CultureInfo("en-AU") };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-AU"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}