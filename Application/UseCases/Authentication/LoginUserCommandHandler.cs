﻿using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.Authentication
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, string>
    {
        private readonly IUserRepository _userRepository;

        public LoginUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<string> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = new User
            {
                Email = request.Email,
                Password = request.Password
            };
            var token = await _userRepository.Login(user);
            return token;
        }
    }
}