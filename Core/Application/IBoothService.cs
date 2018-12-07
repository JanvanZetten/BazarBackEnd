using System;
using System.Collections.Generic;
using Core.Entity;

namespace Core.Application
{
    public interface IBoothService
    {
        //CRUD
        Booth GetById(int id);
        List<Booth> GetAll();
        List<Booth> GetAllIncludeAll();
        Booth Delete(int id);
        Booth Create(Booth newBooth);
        Booth Update(Booth updatedBooth);

        //Extra
        int CountAvailableBooths();
        Booth Book(string token);
        List<Booth> GetUsersBooking(string token);
        Booth CancelReservation(int boothId, string token);
        WaitingListItem CancelWaitingPosition(string token);
        int GetWaitingListItemPosition(string token);


    }
}
