using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Repositories;

public class UserRepository : IUserRepository
{
    OurStoreContext _OurStoreContext;
    public UserRepository(OurStoreContext ourStoreContext)
    {
        _OurStoreContext = ourStoreContext;
    }
    public async Task<User> getById(int id)
    {
        return await _OurStoreContext.Users.Include(user=>user.Orders).FirstOrDefaultAsync(user => user.Id == id);
    }
    public async Task<User> addUser(User user)
    {
        await _OurStoreContext.Users.AddAsync(user);
        await _OurStoreContext.SaveChangesAsync();
        return user;
    }
    public async Task<User> login(string email, string password)
    {
       return await _OurStoreContext.Users.FirstOrDefaultAsync(user=>user.Email==email&& user.Password == password);
    }
    public async Task<User> updateUser(int id, User userToUpdate)
    {
        userToUpdate.Id = id;
        _OurStoreContext.Users.Update(userToUpdate);
        await _OurStoreContext.SaveChangesAsync();
        return userToUpdate;
    }
   
}

