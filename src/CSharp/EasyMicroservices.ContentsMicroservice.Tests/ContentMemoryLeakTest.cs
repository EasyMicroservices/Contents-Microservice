using EasyMicroservices.ContentsMicroservice.Contracts.Common;
using EasyMicroservices.ContentsMicroservice.Contracts.Requests;
using EasyMicroservices.ContentsMicroservice.Database.Contexts;
using EasyMicroservices.ContentsMicroservice.Database.Entities;
using EasyMicroservices.ContentsMicroservice.WebApi.Controllers;
using EasyMicroservices.Cores.AspEntityFrameworkCoreApi;
using EasyMicroservices.Cores.AspEntityFrameworkCoreApi.Interfaces;
using EasyMicroservices.Cores.Relational.EntityFrameworkCore;
using EasyMicroservices.Cores.Relational.EntityFrameworkCore.Intrerfaces;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace EasyMicroservices.ContentsMicroservice.Tests
{
    public class ContentMemoryLeakTest
    {
        static bool IsStarted = false;

        static async Task StartServer()
        {
            if (IsStarted)
                return;
            IsStarted = true;
            _ = Task.Run(async () =>
            {
                await EasyMicroservices.ContentsMicroservice.WebApi.Program.Run(null, (s) =>
                {
                    s.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(CategoryController).Assembly));
                });
            });
            await Task.Delay(2000);
        }

        //[Fact]
        //public async Task CheckMemoryLeakAddContentTest()
        //{
        //    await StartServer();
        //    Contents.GeneratedServices.CategoryClient client = new Contents.GeneratedServices.CategoryClient("http://localhost:2003", new System.Net.Http.HttpClient());
        //    var categoryResult = await client.AddAsync(new Contents.GeneratedServices.CreateCategoryRequestContract()
        //    {
        //        Key = $"1-1-Title{Guid.NewGuid()}",
        //        UniqueIdentity = "1-2"
        //    });
        //    Assert.True(categoryResult.IsSuccess);
        //    Contents.GeneratedServices.ContentClient contentClient = new Contents.GeneratedServices.ContentClient("http://localhost:2003", new System.Net.Http.HttpClient());
        //    for (int i = 0; i < 5000; i++)
        //    {
        //        var contentResult = await contentClient.AddAsync(new Contents.GeneratedServices.CreateContentRequestContract()
        //        {
        //            CategoryId = categoryResult.Result,
        //            UniqueIdentity = "1-2",
        //            Data = i.ToString(),
        //            LanguageId = 1
        //        });
        //        Assert.True(contentResult.IsSuccess);
        //    }

        //    while (true)
        //    {
        //        GC.Collect();
        //        await Task.Delay(1000);
        //    }
        //}

        //[Fact]
        //public async Task MemoryLeackTest()
        //{
        //    HostApplicationBuilder builder = Host.CreateApplicationBuilder(null);
        //    UnitOfWork.DefaultUniqueIdentity = "1-1";
        //    UnitOfWork.MicroserviceId = 10;
        //    builder.Services.AddTransient<IEntityFrameworkCoreDatabaseBuilder, DatabaseBuilder>();
        //    builder.Services.AddTransient<RelationalCoreContext, ContentContext>();
        //    builder.Services.AddTransient<ContentContext>();
        //    //builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

        //    using IHost host = builder.Build();

        //    for (int i = 0; i < 5000; i++)
        //    {
        //        using var scope = host.Services.CreateScope();
        //        using var uow = scope.ServiceProvider.GetService<RelationalCoreContext>();
        //        //using var logic = uow.GetLongContractLogic<ContentEntity, CreateContentRequestContract, UpdateContentRequestContract, ContentContract>();
        //    }
        //}
    }
}