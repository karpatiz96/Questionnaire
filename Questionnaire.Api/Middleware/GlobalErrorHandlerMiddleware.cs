using Microsoft.AspNetCore.Http;
using Questionnaire.Bll.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Questionnaire.Api.Middleware
{
    public class GlobalErrorHandlerMiddleware
    {
        private readonly RequestDelegate next;

        public GlobalErrorHandlerMiddleware(RequestDelegate _next)
        {
            next = _next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            } catch(Exception error) {
                var response = context.Response;
                response.ContentType = "application/json";

                switch (error)
                {
                    case UserGroupNotFoundExcetpion:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    case InvitationNotFoundException:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    case QuestionnaireNotFoundException:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    case GroupNotFoundException:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    case QuestionNotFoundException:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    case UserNotAdminException:
                        response.StatusCode = (int)HttpStatusCode.Forbidden;
                        break;
                    case UserNotMemberException:
                        response.StatusCode = (int)HttpStatusCode.Forbidden;
                        break;
                    case QuestionnaireNotEditableException:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case QuestionnaireStartValidationException:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case InvitationValidationException:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case QuestionnaireResultValidationException:
                        response.StatusCode = (int)HttpStatusCode.Forbidden;
                        break;
                    case QuestionAnswerValidationException:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                var result = JsonSerializer.Serialize(new { message = error?.Message });
                await response.WriteAsync(result);
            }
        }
    }
}
