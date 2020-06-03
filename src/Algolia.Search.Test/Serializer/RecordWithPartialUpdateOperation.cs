using Algolia.Search.Models.Search;

class RecordWithPartialUpdateOperation<T>
{
    public string ObjectID { get; set; }

    public PartialUpdateOperation<T> Update { get; set; }

}
