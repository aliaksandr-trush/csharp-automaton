namespace RegOnline.RegressionTest.Keyword
{
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.PageObject;

    public class AddHotel
    {
        public void Add_Hotel(Hotel hotel)
        {
            PageObjectProvider.Builder.EventDetails.FormPages.LodgingTravelPage.AddNewHotel_Click();
            PageObjectProvider.Builder.EventDetails.FormPages.LodgingTravelPage.AddHotelFrame.SelectByName();

            if (PageObjectProvider.Builder.EventDetails.FormPages.LodgingTravelPage.AddHotelFrame.ChooseHotel.Options.Find(
                h => h.Text == hotel.HotelName) != null)
            {
                PageObjectProvider.Builder.EventDetails.FormPages.LodgingTravelPage.AddHotelFrame.ChooseHotel.SelectWithText(hotel.HotelName);
            }
            else
            {
                PageObjectProvider.Builder.EventDetails.FormPages.LodgingTravelPage.AddHotelFrame.AddHotelTemplate_Click();
                PageObjectProvider.Builder.EventDetails.FormPages.LodgingTravelPage.AddHotelFrame.HotelDefine.SelectByName();
                PageObjectProvider.Builder.EventDetails.FormPages.LodgingTravelPage.AddHotelFrame.HotelDefine.HotelName.Type(hotel.HotelName);

                if (hotel.RoomTypes.Count != 0)
                {
                    for (int i = 0; i < hotel.RoomTypes.Count; i++)
                    {
                        PageObjectProvider.Builder.EventDetails.FormPages.LodgingTravelPage.AddHotelFrame.HotelDefine.AddRoomType_Click();
                        PageObjectProvider.Builder.EventDetails.FormPages.LodgingTravelPage.AddHotelFrame.HotelDefine.RoomType(i).Type(
                            hotel.RoomTypes[i].RoomTypeName);
                        PageObjectProvider.Builder.EventDetails.FormPages.LodgingTravelPage.AddHotelFrame.HotelDefine.RoomRate(i).Type(
                            hotel.RoomTypes[i].RoomRate);
                    }
                }

                PageObjectProvider.Builder.EventDetails.FormPages.LodgingTravelPage.AddHotelFrame.HotelDefine.SaveAndClose_Click();
                PageObjectProvider.Builder.EventDetails.FormPages.LodgingTravelPage.AddHotelFrame.SelectByName();
            }

            if (hotel.RoomBlocks.Count != 0)
            {
                for (int i = 0; i < hotel.RoomBlocks.Count; i++)
                {
                    PageObjectProvider.Builder.EventDetails.FormPages.LodgingTravelPage.AddHotelFrame.AddRoomBlock_Click();
                    PageObjectProvider.Builder.EventDetails.FormPages.LodgingTravelPage.AddHotelFrame.RoomBlockDate(
                        hotel.RoomTypes[0], i).Type(hotel.RoomBlocks[i].Date);

                    if (hotel.RoomBlocks[i].RoomBlockRoomTypes.Count != 0)
                    {
                        foreach (RoomBlockRoomType type in hotel.RoomBlocks[i].RoomBlockRoomTypes)
                        {
                            if (type.Capacity.HasValue)
                            {
                                PageObjectProvider.Builder.EventDetails.FormPages.LodgingTravelPage.AddHotelFrame.RoomBlockCapacity(
                                    type.RoomType, i).Type(type.Capacity.Value);
                            }

                            if (type.RoomRate.HasValue)
                            {
                                PageObjectProvider.Builder.EventDetails.FormPages.LodgingTravelPage.AddHotelFrame.RoomBlockRate(
                                    type.RoomType, i).Type(type.RoomRate.Value);
                            }
                        }
                    }
                }
            }

            PageObjectProvider.Builder.EventDetails.FormPages.LodgingTravelPage.AddHotelFrame.SaveAndClose();
        }
    }
}
