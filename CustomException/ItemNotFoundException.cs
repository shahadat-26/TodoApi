namespace TodoApi.CustomException
{
    public class ItemNotFoundException:Exception
    {
        public ItemNotFoundException()
            :base(String.Format("Item with Given ID NOT FOUND"))
        {

        }
    }
}
