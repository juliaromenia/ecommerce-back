using ECommerce.Application.DTOs.Users;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user is null) return null;

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Phone = user.Phone,
                CreatedAt = user.CreatedAt
            };
        }

        //TODO: FAZER HASH DA SENHA ANTES DE SALVAR NO BANCO
        public async Task<UserDto> CreateUserAsync(CreateUserDto request)
        {
            if (
                string.IsNullOrWhiteSpace(request.Name) ||
                string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Phone) ||
                string.IsNullOrWhiteSpace(request.PasswordHash)
                )
            {
                throw new ArgumentException("Name, Email, Telefone e Senha são obrigatórios.");
            }

            if (await _userRepository.GetByEmailAsync(request.Email) is not null)
            {
                throw new ArgumentException("Email já cadastrado.");
            }

            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                Phone = request.Phone,
                PasswordHash = request.PasswordHash,
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.AddAsync(user);

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Phone = user.Phone,
                CreatedAt = user.CreatedAt
            };
        }

        public async Task<string> UpdateUserAsync(int id, UpdateUserDto request)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user is null) throw new ArgumentException("Usuário não encontrado.");

            if (request.Email is not null && request.Email != user.Email)
            {
                if (await _userRepository.GetByEmailAsync(request.Email) is not null)
                {
                    throw new ArgumentException("Email já cadastrado.");
                }
            }

            user.Name = request.Name ?? user.Name;
            user.Email = request.Email ?? user.Email;
            user.Phone = request.Phone ?? user.Phone;

            await _userRepository.UpdateAsync(user);

            return "Usuário atualizado com sucesso.";
        }
    }
}
