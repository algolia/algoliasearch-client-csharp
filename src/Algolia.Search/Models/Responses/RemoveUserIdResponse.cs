using System;
using System.Threading;
using Algolia.Search.Utils;

namespace Algolia.Search.Models.Responses
{
    public class RemoveUserIdResponse : IAlgoliaWaitableResponse
    {
        public Func<string, RemoveUserIdResponse> RemoveDelegate { get; set; }

        public string UserId { get; set; }

        public DateTime DeletedAt { get; set; }

        public void Wait()
        {
            RemoveUserIdResponse deleteResponse;

            while (true)
            {
                try
                {
                    deleteResponse = RemoveDelegate(UserId);
                }
                catch (AlgoliaApiException ex)
                {
                    // Loop until we don't have Error 400: "Another mapping operation is already running for this userID"
                    if (ex.Message.Contains("Another mapping operation is already running for this userID"))
                    {
                        Thread.Sleep(1000);
                        continue;
                    }

                    throw;
                }

                DeletedAt = deleteResponse.DeletedAt;
                break;
            }
        }
    }
}