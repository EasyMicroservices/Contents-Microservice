using CodeReviewer.Engine;
using EasyMicroservices.ContentsMicroservice.Contracts.Common;
using EasyMicroservices.ContentsMicroservice.Database.Entities;
using EasyMicroservices.ContentsMicroservice.WebApi.Controllers;
using EasyMicroservices.Tests;

namespace EasyMicroservices.ContentsMicroservice.Tests
{
    public class CodeReviewerCheckTests : CodeReviewerTests
    {
        static CodeReviewerCheckTests()
        {
            //types to check (this will check all of types in assembly so no need to add all of types of assembly)
            AssemblyManager.AddAssemblyToReview(
                typeof(DatabaseBuilder),
                //typeof(CompileTimeClassesMappers),
                typeof(CategoryEntity),
                typeof(CategoryContract),
                typeof(CategoryController));
        }
    }
}