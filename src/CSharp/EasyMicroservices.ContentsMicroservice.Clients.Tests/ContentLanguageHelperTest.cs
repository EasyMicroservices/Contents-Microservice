﻿using EasyMicroservices.ContentsMicroservice.Clients.Helpers;
using EasyMicroservices.ContentsMicroservice.Clients.Tests.Contracts.Common;
using EasyMicroservices.Laboratory.Engine;
using EasyMicroservices.Laboratory.Engine.Net.Http;

namespace EasyMicroservices.ContentsMicroservice.Clients.Tests
{
    public class ContentLanguageHelperTest
    {
        const int Port = 1202;
        string _routeAddress = "";
        public static HttpClient HttpClient { get; set; } = new HttpClient();
        public ContentLanguageHelperTest()
        {
            _routeAddress = $"http://localhost:{Port}";
        }

        static bool _isInitialized = false;
        static SemaphoreSlim Semaphore = new SemaphoreSlim(1);
        async Task OnInitialize()
        {
            if (_isInitialized)
                return;
            try
            {
                await Semaphore.WaitAsync();
                _isInitialized = true;

                ResourceManager resourceManager = new ResourceManager();
                HttpHandler httpHandler = new HttpHandler(resourceManager);
                await httpHandler.Start(Port);
                resourceManager.Append(@$"POST *RequestSkipBody* HTTP/1.1
Host: localhost:{Port}
Accept: text/plain*RequestSkipBody*

{{""language"":""fa-IR"",""key"":""1-1-Title""}}"
    ,
    @"HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8
Content-Length: 0

{""result"":{""Data"": ""Hello My Title Language""},""isSuccess"":true,""error"":null}");

                resourceManager.Append(@$"POST *RequestSkipBody* HTTP/1.1
Host: localhost:{Port}
Accept: text/plain*RequestSkipBody*

{{""language"":""fa-IR"",""key"":""1-1-Content""}}"
   ,
   @"HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8
Content-Length: 0

{""result"":{""Data"": ""Hello My Content Language""},""isSuccess"":true,""error"":null}");
            }
            finally
            {
                Semaphore.Release();
            }
        }

        [Fact]
        public async Task ContentLanguageTest()
        {
            await OnInitialize();
            var microserviceClient = new Contents.GeneratedServices.ContentClient(_routeAddress, HttpClient);
            var microservices = await microserviceClient.GetByLanguageAsync(new Contents.GeneratedServices.GetByLanguageRequestContract()
            {
                Key = "1-1-Title",
                Language = "fa-IR"
            });
            Assert.True(microservices.IsSuccess);
            Assert.NotEmpty(microservices.Result.Data);
        }

        [Fact]
        public async Task ContentLanguagePersonTest()
        {
            await OnInitialize();

            PersonContract personContract = new PersonContract();
            personContract.UniqueIdentity = "1-1";
            personContract.Post = new PostContract()
            {
                Person = personContract,
                UniqueIdentity = "1-1"
            };
            personContract.Posts = new List<PostContract>()
            {
                 new PostContract()
                 {
                      Person = personContract,
                      UniqueIdentity = "1-1"
                 }
            };

            ContentLanguageHelper contentLanguageHelper = new ContentLanguageHelper(new Contents.GeneratedServices.ContentClient(_routeAddress, HttpClient));
            await contentLanguageHelper.ResolveContentLanguage(personContract, "fa-IR");

            Assert.NotEmpty(personContract.Title);
            Assert.NotEmpty(personContract.Post.Title);
            Assert.NotEmpty(personContract.Post.Content);
            Assert.NotEmpty(personContract.Posts[0].Title);
            Assert.NotEmpty(personContract.Posts[0].Content);
        }
    }
}
