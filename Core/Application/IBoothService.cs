using System;
using Core.Entity;

namespace Core.Application
{
    public interface IBoothService
    {
        //CRUD
        Booth GetById(int id);
        Booth GetAll();
        Booth Delete(int id);
        Booth Add(Booth newBooth);
        Booth Update(Booth updatedBooth);

        //Extra
        int CountAvalibleBooths();
        Booth Book(int id);
        Booth GetUsersBooking(int userId);
        int WaitingListPosition(int userId);


    }
}
