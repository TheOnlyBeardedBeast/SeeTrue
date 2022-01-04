﻿using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SeeTrue.Infrastructure.Services;
using SeeTrue.Infrastructure.Types;
using SeeTrue.Models;

namespace SeeTrue.Infrastructure.Commands
{
    public static class AdminUpdateUser
    {
        public record Command(Guid UserId, AdminUpdateUserRequest data) : IRequest<User>;

        public class Handler : IRequestHandler<Command, User>
        {
            private readonly IQueryService queries;
            private readonly ICommandService commands;

            public Handler(IQueryService queries, ICommandService commands)
            {
                this.queries = queries;
                this.commands = commands;
            }

            public async Task<User> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await queries.FindUserById(request.UserId);

                if (user is null)
                {
                    throw new SeeTrueException(HttpStatusCode.BadRequest, "User not found");
                }

                if (!string.IsNullOrWhiteSpace(request.data.Password))
                {
                    await commands.UpdateUserPassword(user, request.data.Password);
                }

                if (request.data.Confirm.HasValue && request.data.Confirm.Value)
                {
                    await commands.ConfirmUser(user);
                }

                if (!string.IsNullOrWhiteSpace(request.data.Email))
                {
                    await commands.UpdateUserEmail(user, request.data.Email);
                }

                if (user.AppMetaData is not null)
                {
                    await commands.UpdateAppMetaData(user, request.data.AppMetaData);
                }

                if (user.UserMetaData is not null)
                {
                    await commands.UpdateUserMetaData(user, request.data.UserMetaData);
                }

                await commands.NewAuditLogEntry(null, AuditAction.UserModifiedAction, null);

                return user;
            }
        }
    }
}