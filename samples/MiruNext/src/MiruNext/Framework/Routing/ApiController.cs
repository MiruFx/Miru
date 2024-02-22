﻿using System.ComponentModel.DataAnnotations;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using MiruNext.Framework.Validation;

namespace MiruNext.Framework.Routing;

public abstract class ApiController
{
    public ValidationResponse Validation = new();

    public ValidationResponse Validate<TData>(TData data)
    {
        // var ctx = new ValidationContext(data);
        // var results = new List<ValidationResult>();
        ValidationResponse validationResponse = new();
        // if (!Validator.TryValidateObject(data, ctx, results, true))
        // {
        //     validationResponse.HasErrors = true;
        //     foreach (var error in results)
        //     {
        //         var ve = new ValidationError()
        //         {
        //             Message = error.ErrorMessage,
        //             MemberName = error.MemberNames.First(),
        //         };
        //         validationResponse.Errors.Add(ve);
        //     }
        // }
        // else
        // {
        //     validationResponse.HasErrors = false;
        // }
        Validation = validationResponse;
        return validationResponse;
    }

    public ValidationResponse Validate<TData>(TData data, AbstractValidator<TData> validator)
    {
        ValidationResponse validationResponse = new();
        FluentValidation.Results.ValidationResult results = validator.Validate(data);

        if (!results.IsValid)
        {
            validationResponse.HasErrors = true;
            foreach (var error in results.Errors)
            {
                var ve = new ValidationError()
                {
                    Message = error.ErrorMessage,
                    MemberName = error.PropertyName
                };
                validationResponse.Errors.Add(ve);
            }
        }
        else
        {
            validationResponse.HasErrors = false;
        }
        Validation = validationResponse;
        return validationResponse;
    }

    public ValidationResponse GetErrors()
    {
        return Validation;
    }

    public void AddError(string key, string message)
    {
        Validation.HasErrors = true;
        Validation.Errors.Add(new ValidationError() { MemberName = key, Message = message });
    }

    public IResult Redirect(string url)
    {
        return Results.Redirect(url);
    }
}
