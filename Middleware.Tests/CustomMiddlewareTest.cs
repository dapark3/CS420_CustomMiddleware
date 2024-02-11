using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;

using WebRazor.CustomMiddleware;

namespace Middleware.Tests; 

    public class CustomMiddlewareTest : IAsyncLifetime {

        IHost? host;
        public Task DisposeAsync() {
            return Task.CompletedTask;
        }

        public async Task InitializeAsync() {
            host = await new HostBuilder()
                .ConfigureWebHost(webBuilder => {
                    webBuilder
                        .UseTestServer()
                        .ConfigureServices(services => { })
                        .Configure(app => {
                            app.UseMiddleware<CustomMiddleware>();
                            app.Run(async context =>
                            {
                            await context.Response.WriteAsync("Authorized.");
                            });
                        });
                })
                .StartAsync();
        }

        [Fact]
        public async Task TestNoCredentials(){
#pragma warning disable CS8604 // Possible null reference argument.
            var response = await host.GetTestClient().GetAsync("/");
#pragma warning restore CS8604 // Possible null reference argument.
            var result = await response.Content.ReadAsStringAsync();
            Assert.Equal("Not authorized.", result);
        }

        [Fact]
        public async Task TestCorrectCredentials() {
#pragma warning disable CS8604 // Possible null reference argument.
            var response = await host.GetTestClient().GetAsync("/?username=user1&password=password1");
#pragma warning restore CS8604 // Possible null reference argument.
            var result = await response.Content.ReadAsStringAsync();
            Assert.Equal("Authorized.", result);
        }

        [Fact]
        public async Task TestOnlyUsername() {
#pragma warning disable CS8604 // Possible null reference argument.
            var response = await host.GetTestClient().GetAsync("/?username=user1");
#pragma warning restore CS8604 // Possible null reference argument.
            var result = await response.Content.ReadAsStringAsync();
            Assert.Equal("Not authorized.", result);
        }

        [Fact]
        public async Task TestOnlyPassword() {
#pragma warning disable CS8604 // Possible null reference argument.
            var response = await host.GetTestClient().GetAsync("/?username=password1");
#pragma warning restore CS8604 // Possible null reference argument.
            var result = await response.Content.ReadAsStringAsync();
            Assert.Equal("Not authorized.", result);
        }

        [Fact]
        public async Task TestIncorrectUsername() {
#pragma warning disable CS8604 // Possible null reference argument.
            var response = await host.GetTestClient().GetAsync("/?username=user5&password=password1");
#pragma warning restore CS8604 // Possible null reference argument.
            var result = await response.Content.ReadAsStringAsync();
            Assert.Equal("Not authorized.", result);
        }

        [Fact]
        public async Task TestIncorrectPassworrd() {
#pragma warning disable CS8604 // Possible null reference argument.
            var response = await host.GetTestClient().GetAsync("/?username=user1&password=password2");
#pragma warning restore CS8604 // Possible null reference argument.
            var result = await response.Content.ReadAsStringAsync();
            Assert.Equal("Not authorized.", result);
        }

        [Fact]
        public async Task TestIncorrectCreditials() {
#pragma warning disable CS8604 // Possible null reference argument.
            var response = await host.GetTestClient().GetAsync("/?username=user5&password=password2");
#pragma warning restore CS8604 // Possible null reference argument.
            var result = await response.Content.ReadAsStringAsync();
            Assert.Equal("Not authorized.", result);
        }

}