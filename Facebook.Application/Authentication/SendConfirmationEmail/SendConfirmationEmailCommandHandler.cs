﻿using ErrorOr;
using Facebook.Application.Common.Interfaces.Authentication;
using Facebook.Application.Common.Interfaces.User;
using Facebook.Application.Common.Interfaces.User.IRepository;
using Facebook.Application.Services;
using MediatR;

namespace Facebook.Application.Authentication.SendConfirmationEmail;

public class SendConfirmationEmailCommandHandler : IRequestHandler<SendConfirmationEmailCommand, ErrorOr<Success>>
{
	private readonly IUserRepository _userRepository;
	private readonly IUserAuthenticationService _userAuthenticationService;
	private readonly EmailService _emailService;

	public SendConfirmationEmailCommandHandler(IUserRepository userRepository, 
		IUserAuthenticationService userAuthenticationService, 
		EmailService emailService) 
	{
		_userRepository = userRepository;
		_userAuthenticationService = userAuthenticationService;
		_emailService = emailService;
	}

	public async Task<ErrorOr<Success>> Handle(SendConfirmationEmailCommand request, CancellationToken cancellationToken)
	{
		var errorOrUser = await _userRepository.GetByEmailAsync(request.Email);

		if (errorOrUser.IsError)
			return Error.Validation("User with such email doesn't exist");

		var user = errorOrUser.Value;

		var token = await _userAuthenticationService.GenerateEmailConfirmationTokenAsync(user);

		string? userName;

		if (string.IsNullOrEmpty(user.FirstName) || string.IsNullOrEmpty(user.LastName))
		{
			if (string.IsNullOrEmpty(user.LastName) && string.IsNullOrEmpty(user.FirstName))
			{
				userName = user.Email;
			}
			else if (string.IsNullOrEmpty(user.LastName))
			{
				userName = user.FirstName;
			}
			else
			{
				userName = user.LastName;
			}
		}
		else
		{
			userName = user.FirstName + " " + user.LastName;
		}

		var sendEmailResult = await _emailService.SendEmailConfirmationEmailAsync(
			user.Id, user.Email!, token, request.BaseUrl, userName!);

		return sendEmailResult;
	}
}
