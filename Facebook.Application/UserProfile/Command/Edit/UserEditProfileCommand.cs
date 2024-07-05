﻿using ErrorOr;
using Facebook.Domain.User;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.UserProfile.Command.Edit;
public record UserEditProfileCommand(
    string UserId,
    string? FirstName,
    string? LastName,
    byte[]? CoverPhoto,
    byte[]? Avatar,
    string? Pronouns,
    string? Biography,
    bool? IsProfilePublic,
    bool? isBlocked,
    string? Country,
    string? Region
) : IRequest<ErrorOr<UserProfileEntity>>;
