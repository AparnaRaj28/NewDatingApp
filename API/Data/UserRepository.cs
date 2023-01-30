using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Interface;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public UserRepository(DataContext context,IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
            
        }

        public async Task<IEnumerable<MemberDto>> GetAllMembersAsync()
        {
            return await _context.Users
                          .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                          .ToListAsync();
        }
        public async Task<MemberDto> GetMemberByUsername(string username)
        {
            return await _context.Users
                         .Where(x=>x.UserName == username)
                         .ProjectTo<MemberDto>(_mapper.ConfigurationProvider) //automapper mapping
                         .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<AppUser>> GetAllUsersAsync()
        {
            var users = await _context.Users
                       .Include(p=>p.Photos)
                       .ToListAsync();
            return users;
        }



        public async Task<AppUser> GetUserByIdAsync(int id)
        {
           var user = await _context.Users.Include(p=>p.Photos)
                                        .FirstOrDefaultAsync(x=>x.Id == id);
                                    
           return user;
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            var user = await _context.Users.Include(p=>p.Photos)
                                     .FirstOrDefaultAsync(x=>x.UserName == username);
            return user;;
        }

        public async Task<bool> SaveAllAsync()
        {
            //returns 0 if nothing is saved into the database
            return await _context.SaveChangesAsync() >0;
        }

        public void Update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
            //this tells the EF tracker that something has changed   
            //with the entity(user)
            //It is not getting saved..just inorming the EF tracker that changes have been applied
        }
    }
}