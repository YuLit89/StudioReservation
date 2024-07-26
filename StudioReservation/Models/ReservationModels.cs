using Microsoft.Ajax.Utilities;
using StudioRoomType.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudioReservation.Models
{
   
    public class RoomViewModels
    {
        public List<Room> Rooms { get; set; }
    }

    public class RoomStatus
    {
        public string timeslot { get; set; }
        public string status { get; set; }
    }

    public class ScheduleDate
    {
        public string Date { get; set; }
        public bool Enable { get; set; }
        public List<RoomStatus> roomStatuses {get;set;}
    }

    public class ScheduleViewModel
    {
        public string DisplayDate { get; set; }
        public List<ScheduleDate> scheduleDates { get; set; }
    }

    public class Order
    {
        [Display(Name = "Date:")]
        public string date { get; set; }
        [Display(Name = "Booked slot:")]
        public List<string> slots { get; set; }
        public decimal fee { get; set; }
    }

    public class ReservationConfirmViewModel
    {
        [Display(Name = "Room:")]
        public string roomName { get; set; }
        [Display(Name = "Terms and Conditions:")]
        public List<string> termCondition { get; set; }
        public List<Order>  orders { get; set; }
        [Display(Name = "Basic Rental Cost:")]
        public decimal rentalCost { get; set; }
        [Display(Name = "Platform fee:")]
        public decimal platformCharge { get; set; }
        [Display(Name = "TOTAL AMOUNT:")]
        public decimal totalAmount { get; set; }

    }
}