using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Shouldly;

namespace Stravaig.RulesEngine.Tests.Framework
{
    [TestFixture]
    public class DependencyInjectionResolutionTests
    {
        public class MyTestClass<T>
        {
            public T Value { get; set; }
        }
        
        [Test]
        public void ResolveGenericClassTest()
        {
            var services = new ServiceCollection();
            services.AddTransient(typeof(MyTestClass<>));
            var provider = services.BuildServiceProvider();
            var testClassInstance = provider.GetRequiredService<MyTestClass<string>>();
            testClassInstance.ShouldBeOfType<MyTestClass<string>>();
        }
    }
}