using EcommerceDev.Application.Common;
using EcommerceDev.Core.Entities;
using EcommerceDev.Core.Repositories;
using EcommerceDev.Core.Services;

namespace EcommerceDev.Application.Commands.Auth.Register;

public class RegisterCommandHandler : IHandler<RegisterCommand, ResultViewModel<Guid>>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterCommandHandler(
        ICustomerRepository customerRepository,
        IPasswordHasher passwordHasher)
    {
        _customerRepository = customerRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<ResultViewModel<Guid>> HandleAsync(RegisterCommand request)
    {
        if (request.Password != request.ConfirmPassword)
        {
            return ResultViewModel<Guid>.Error("Passwords do not match");
        }

        if (request.Password.Length < 6)
        {
            return ResultViewModel<Guid>.Error("Password must be at least 6 characters long");
        }

        if (await _customerRepository.EmailExists(request.Email))
        {
            return ResultViewModel<Guid>.Error("Email already registered");
        }

        request.BirthDate = DateTime.SpecifyKind(request.BirthDate, DateTimeKind.Utc);

        var customer = new Customer(
            request.FullName,
            request.Email,
            request.PhoneNumber,
            request.BirthDate,
            request.Document
        );

        customer.PasswordHash = _passwordHasher.HashPassword(request.Password);
        customer.IsActive = true;
        customer.IsEmailVerified = false;

        var customerId = await _customerRepository.Create(customer);

        return ResultViewModel<Guid>.Success(customerId);
    }
}
