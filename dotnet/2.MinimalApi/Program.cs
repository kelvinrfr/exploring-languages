using System.Net;
using FluentValidation;

/*
curl -X POST "http://localhost:5155/person" \
  -H "Content-Type: application/json" \
  -d '{"name": "asfasdfasdfasdfasdfasdfafsdf"}
*/

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<IValidator<Person>, PersonValidator>();

var app = builder.Build();

app.MapPost("/person", (Person request) => 
{
    var validator = app.Services.GetRequiredService<IValidator<Person>>(); 
    var validationResult = validator.Validate(request);
    if(!validationResult.IsValid)
    {
        var error = validationResult.Errors.First(); // getting first for example purposes

        return Results.Problem(
            type: error.ErrorCode,
            title: "Error validating person request",
            instance: "/check-name",
            detail: error.ErrorMessage,
            extensions: (Dictionary<string, object?>)error.CustomState,
            statusCode: (int)HttpStatusCode.BadRequest
        );
    }
    return Results.Ok($"Hello {request.Name}");
});

app.Run();

record Person(string Name);

class PersonValidator : AbstractValidator<Person>
{
    public PersonValidator()
    {
        RuleFor(person => person.Name)
            .NotEmpty() // triggers when null, empty or whitespace
            .WithErrorCode("invalid-name-state")
            .WithMessage(p => $"Invalid name provided: `{p.Name}`")
            .WithState(p => new Dictionary<string, object?> 
            {
                { "name", p.Name }
            });

        RuleFor(person => person.Name)
            .Length(10)
            .WithErrorCode("invalid-name-length")
            .WithMessage(p => $"Name with invalid length: `{p.Name}`")
            .WithState(p => new Dictionary<string, object?> 
            {
                { "name", p.Name },
                { "length", p.Name.Length },
            });
    }
}
