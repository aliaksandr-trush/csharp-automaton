namespace RegOnline.RegressionTest.DataCollection
{
    using System;
    using System.Collections.Generic;

    public class Lodging
    {
        public List<Hotel> Hotels = new List<Hotel>();
        public List<LodgingStandardFields> StandardFields = new List<LodgingStandardFields>();
    }

    public class Hotel
    {
        public string HotelName;
        public List<RoomType> RoomTypes = new List<RoomType>();
        public List<RoomBlock> RoomBlocks = new List<RoomBlock>();

        public Hotel(string hotelName)
        {
            this.HotelName = hotelName;
        }
    }

    public class LodgingStandardFields
    {
        public FormData.LodgingStandardFields Field;
        public bool? Visible;
        public bool? Required;
    }

    public class RoomType
    {
        public string RoomTypeName;
        public double RoomRate;

        public RoomType(string roomTypeName)
        {
            this.RoomTypeName = roomTypeName;
        }
    }

    public class RoomBlock
    {
        public DateTime Date;
        public List<RoomBlockRoomType> RoomBlockRoomTypes = new List<RoomBlockRoomType>();
    }

    public class RoomBlockRoomType
    {
        public RoomType RoomType;
        public int? Capacity;
        public double? RoomRate;
    }
}
