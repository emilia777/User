using System.Linq;
using System;
using System.Text;
using AutoMapper;
using Library.Dtos;
using Library.Models;
using Library.Repositories;
using User.Dtos;

namespace Library.Servises
{
    public interface IAuthService
    {
        GetUserDto Register(RegisterRequestDto registerDto);
    }
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IMapper _mapper;

        public AuthService(IAuthRepository authRepository, IMapper mapper)
        {
            _authRepository = authRepository;
            _mapper = mapper;
        }

        public GetUserDto Register(RegisterRequestDto registerDto)
        {
            var userInDb = _authRepository.GetUserByEmail(registerDto.Email);
            if (userInDb != null)
            {
                throw new Exception($"User already exist by email : {registerDto.Email} !");
            }

            CreatePasswordHash(registerDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var userToAdd = _mapper.Map<User>(registerDto);
            userToAdd.PasswordHash = passwordHash;
            userToAdd.PasswordSalt = passwordSalt;

            _authRepository.AddUser(userToAdd);
            var user = _authRepository.GetUserByEmail(userToAdd.Email);

            return _mapper.Map<GetUserDto>(user);
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

                return computeHash.SequenceEqual(passwordHash);
            }
        }
    }
}