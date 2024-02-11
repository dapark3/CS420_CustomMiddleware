namespace WebRazor.CustomMiddleware {

    public static class CustomMiddlewareExtensions {
        public static IApplicationBuilder UseMiddleware(this IApplicationBuilder builder) {
            return builder.UseMiddleware<CustomMiddleware>();
        }
    }

}