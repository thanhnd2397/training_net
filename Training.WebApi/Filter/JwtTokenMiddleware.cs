namespace Training.WebApi.Filter;

public class JwtTokenMiddleware
{
    public static IApplicationBuilder UseJwtTokenMiddleware(IApplicationBuilder app)
    {
        return app.UseMiddleware<JwtTokenFilter>();
    }

}