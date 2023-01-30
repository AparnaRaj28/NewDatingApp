using API.DTOs;
using API.Interface;
using AutoMapper;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
       
        public UsersController(IUserRepository userRepository,IMapper mapper)
        {
            _mapper = mapper;
            _userRepository = userRepository;
                      
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> getAllUsers()
        {

          var users= await _userRepository.GetAllUsersAsync();
          var usersToReturn = _mapper.Map<IEnumerable<MemberDto>>(users);
          return Ok(usersToReturn);
           
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<AppUser>> getUserById(int id){
            // var user = _context.Users.ToList().FirstOrDefault(u=>u.Id == id);
            // var user = await _userRepository.GetUserByIdAsync(id);
            // var userToReturn = _mapper.Map<MemberDto>(user);
            // return Ok(userToReturn);

            var users = await _userRepository.GetAllMembersAsync();
            return Ok(users);
        }

        
        [HttpGet("{username}")]
        public async Task<ActionResult<AppUser>> getUserByUsername(string username)
        {
            // var user = _context.Users.ToList().FirstOrDefault(u=>u.Id == id);
        //     var user = await _userRepository.GetUserByUsernameAsync(username);
        // //    var userToReturn = _mapper.Map<MemberDto>(user);
        //     return Ok(userToReturn);
         
            var user = await _userRepository.GetMemberByUsername(username);
            return Ok(user);
        }
    }
}