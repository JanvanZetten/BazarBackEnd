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
        Booth Delete(int id);
        Booth Create(Booth newBooth);
        Booth Update(Booth updatedBooth);

        //Extra
        int CountAvailableBooths();
        Booth Book(string token);
        Booth GetUsersBooking(int userId);
        int WaitingListPosition(int userId);
        Booth CancelReservation(int boothId, string token);
        WaitingListItem CancelWaitingPosition(int waitingId, string token);


    }
}
